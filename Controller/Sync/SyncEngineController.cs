using Controller.Security;
using Core.Enums;
using Model.Entities;
using Model.Repositories;

namespace Controller.Sync;

public class SyncEngineController
{
    private readonly SecureVaultController _vault;
    private readonly FileRepository _fileRepository;
    private readonly Func<string> _passphraseProvider;
    private readonly List<string> _pendingResume = new();
    private readonly object _resumeLock = new();

    private static readonly string ResumeQueuePath = Path.Combine(AppContext.BaseDirectory, "resume_queue.txt");

    public IReadOnlyList<string> PendingResume
    {
        get { lock (_resumeLock) return _pendingResume.ToArray(); }
    }

    public bool KeepColdStorage { get; set; } = true;

    /// <summary>How many files are sealed and stored concurrently (1 = sequential).</summary>
    public int MaxParallelism { get; set; } = 1;

    /// <summary>When enabled, every stored blob is re-opened right after upload to
    /// prove the vault payload is readable and identical to the source bytes.</summary>
    public bool VerifyAfterSync { get; set; } = true;

    public void MarkForResume(string path)
    {
        lock (_resumeLock)
        {
            if (!_pendingResume.Contains(path)) _pendingResume.Add(path);
            SaveQueue();
        }
    }

    public event Action<string>? FileProcessed;
    public event Action<string>? FileFailed;
    public event Action<string>? FileVerified;

    public SyncEngineController(SecureVaultController vault, FileRepository fileRepository,
        Func<string> passphraseProvider)
    {
        _vault = vault;
        _fileRepository = fileRepository;
        _passphraseProvider = passphraseProvider;
        try
        {
            if (File.Exists(ResumeQueuePath))
            {
                _pendingResume.AddRange(File.ReadAllLines(ResumeQueuePath).Where(l => !string.IsNullOrWhiteSpace(l)));
            }
        }
        catch
        {
        }
    }

    private void SaveQueue()
    {
        try
        {
            File.WriteAllLines(ResumeQueuePath, _pendingResume);
        }
        catch
        {
        }
    }

    public async Task<bool> ProcessFileAsync(FileModel file, byte[] content, CancellationToken cancellationToken = default)
    {
        try
        {
            string passphrase = _passphraseProvider();
            byte[] sealedPayload = _vault.Seal(content, passphrase);
            int id = await _fileRepository.InsertWithBlobAsync(file, sealedPayload, cancellationToken);
            file.Id = id;

            if (VerifyAfterSync)
            {
                byte[]? stored = await _fileRepository.GetFileBlobAsync(id, cancellationToken);
                if (stored == null || !_vault.Open(stored, passphrase).AsSpan().SequenceEqual(content))
                {
                    file.Status = SyncStatus.Pending;
                    lock (_resumeLock)
                    {
                        if (!_pendingResume.Contains(file.FilePath)) _pendingResume.Add(file.FilePath);
                        SaveQueue();
                    }
                    FileFailed?.Invoke(file.FilePath);
                    return false;
                }
                FileVerified?.Invoke(file.FilePath);
            }

            if (KeepColdStorage)
            {
                try
                {
                    await SaveColdStorageAsync(file, sealedPayload, cancellationToken);
                }
                catch
                {
                }
            }
            file.Status = SyncStatus.Synced;
            lock (_resumeLock)
            {
                if (_pendingResume.Remove(file.FilePath)) SaveQueue();
            }
            FileProcessed?.Invoke(file.FilePath);
            return true;
        }
        catch (Exception ex) when (ex is OperationCanceledException or TimeoutException or InvalidOperationException or MySqlConnector.MySqlException)
        {
            file.Status = SyncStatus.Pending;
            lock (_resumeLock)
            {
                if (!_pendingResume.Contains(file.FilePath)) _pendingResume.Add(file.FilePath);
                SaveQueue();
            }
            FileFailed?.Invoke(file.FilePath);
            return false;
        }
    }

    public async Task<int> ProcessBatchAsync(IEnumerable<FileModel> files, Func<FileModel, byte[]> contentProvider, CancellationToken cancellationToken = default)
    {
        int max = Math.Clamp(MaxParallelism, 1, 8);
        if (max <= 1)
        {
            int sequential = 0;
            foreach (FileModel file in files)
            {
                if (await ProcessFileAsync(file, contentProvider(file), cancellationToken)) sequential++;
            }
            return sequential;
        }

        using var gate = new SemaphoreSlim(max, max);
        int ok = 0;
        IEnumerable<Task> tasks = files.Select(async file =>
        {
            await gate.WaitAsync(cancellationToken);
            try
            {
                if (await ProcessFileAsync(file, contentProvider(file), cancellationToken))
                    Interlocked.Increment(ref ok);
            }
            finally
            {
                gate.Release();
            }
        });
        await Task.WhenAll(tasks);
        return ok;
    }

    public async Task<int> ResumePendingAsync(Func<FileModel, byte[]> contentProvider, Func<string, FileModel> modelResolver, CancellationToken cancellationToken = default)
    {
        int ok = 0;
        foreach (string path in PendingResume)
        {
            FileModel model = modelResolver(path);
            if (await ProcessFileAsync(model, contentProvider(model), cancellationToken))
            {
                lock (_resumeLock)
                {
                    _pendingResume.Remove(path);
                    SaveQueue();
                }
                ok++;
            }
        }
        return ok;
    }

    private static async Task SaveColdStorageAsync(FileModel file, byte[] payload, CancellationToken cancellationToken)
    {
        if (DirectoryScannerController.IsColdStoragePath(file.FilePath)) return;
        string folder = Path.Combine(Path.GetDirectoryName(file.FilePath) ?? Path.GetTempPath(), Core.Config.AppConstants.ColdStorageFolderName);
        Directory.CreateDirectory(folder);
        string dest = Path.Combine(folder, Path.GetFileName(file.FilePath) + ".dsc");
        await File.WriteAllBytesAsync(dest, payload, cancellationToken);
        try
        {
            File.SetAttributes(folder, FileAttributes.Hidden);
        }
        catch (IOException)
        {
        }
    }
}
