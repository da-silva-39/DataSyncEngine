using Controller.Processing;
using Controller.Security;
using Core.Enums;
using Model.Entities;
using Model.Repositories;

namespace Controller.Sync;

public class SyncEngineController
{
    private readonly Aes256Controller _aes;
    private readonly StreamCompressorController _compressor;
    private readonly FileRepository _fileRepository;
    private readonly Func<byte[]> _masterKeyProvider;
    private readonly List<string> _pendingResume = new();

    public IReadOnlyList<string> PendingResume => _pendingResume;

    public void MarkForResume(string path)
    {
        if (!_pendingResume.Contains(path)) _pendingResume.Add(path);
    }

    public event Action<string>? FileProcessed;
    public event Action<string>? FileFailed;

    public SyncEngineController(Aes256Controller aes, StreamCompressorController compressor,
        FileRepository fileRepository, Func<byte[]> masterKeyProvider)
    {
        _aes = aes;
        _compressor = compressor;
        _fileRepository = fileRepository;
        _masterKeyProvider = masterKeyProvider;
    }

    public async Task<bool> ProcessFileAsync(FileModel file, byte[] content, CancellationToken cancellationToken = default)
    {
        try
        {
            byte[] encrypted = _aes.Encrypt(content, _masterKeyProvider());
            byte[] compressed = _compressor.Compress(encrypted);
            await _fileRepository.InsertWithBlobAsync(file, compressed, cancellationToken);
            await SaveColdStorageAsync(file, compressed, cancellationToken);
            file.Status = SyncStatus.Synced;
            FileProcessed?.Invoke(file.FilePath);
            return true;
        }
        catch (Exception ex) when (ex is OperationCanceledException or TimeoutException or InvalidOperationException or MySqlConnector.MySqlException)
        {
            file.Status = SyncStatus.Pending;
            if (!_pendingResume.Contains(file.FilePath)) _pendingResume.Add(file.FilePath);
            FileFailed?.Invoke(file.FilePath);
            return false;
        }
    }

    public async Task<int> ProcessBatchAsync(IEnumerable<FileModel> files, Func<FileModel, byte[]> contentProvider, CancellationToken cancellationToken = default)
    {
        int ok = 0;
        foreach (FileModel file in files)
        {
            if (await ProcessFileAsync(file, contentProvider(file), cancellationToken)) ok++;
        }
        return ok;
    }

    public async Task<int> ResumePendingAsync(Func<FileModel, byte[]> contentProvider, Func<string, FileModel> modelResolver, CancellationToken cancellationToken = default)
    {
        int ok = 0;
        foreach (string path in _pendingResume.ToArray())
        {
            FileModel model = modelResolver(path);
            if (await ProcessFileAsync(model, contentProvider(model), cancellationToken))
            {
                _pendingResume.Remove(path);
                ok++;
            }
        }
        return ok;
    }

    private static async Task SaveColdStorageAsync(FileModel file, byte[] payload, CancellationToken cancellationToken)
    {
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
