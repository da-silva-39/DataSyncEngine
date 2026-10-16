using Controller.Security;
using Core.Enums;
using Model.Entities;
using Model.Repositories;
using System.Text.RegularExpressions;

namespace Controller.Sync;

public class DirectoryScannerController
{
    private readonly Sha256Controller _sha256;
    private readonly FileRepository _fileRepository;

    public Func<string>? ExcludePatternsProvider { get; set; }

    /// <summary>Maximum file size in MB accepted by the scanner (0 = unlimited).
    /// Oversized files are skipped instead of entering the sync list.</summary>
    public Func<int>? MaxFileSizeMbProvider { get; set; }

    public DirectoryScannerController(Sha256Controller sha256, FileRepository fileRepository)
    {
        _sha256 = sha256;
        _fileRepository = fileRepository;
    }

    public static bool IsColdStoragePath(string path)
    {
        string cold = Core.Config.AppConstants.ColdStorageFolderName;
        foreach (string segment in path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))
        {
            if (segment.Equals(cold, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    public static bool IsExcluded(string fileName, string? patterns)
    {
        if (string.IsNullOrWhiteSpace(patterns)) return false;
        string name = Path.GetFileName(fileName);
        foreach (string raw in patterns.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            string regex = "^" + Regex.Escape(raw).Replace(@"\*", ".*").Replace(@"\?", ".") + "$";
            if (Regex.IsMatch(name, regex, RegexOptions.IgnoreCase)) return true;
        }
        return false;
    }

    public async Task<IReadOnlyList<FileModel>> ScanAsync(string directory, CancellationToken cancellationToken = default)
    {
        var results = new List<FileModel>();
        if (!Directory.Exists(directory)) return results;

        IReadOnlyList<FileModel> stored;
        try
        {
            stored = await _fileRepository.GetAllAsync(cancellationToken);
        }
        catch
        {
            stored = Array.Empty<FileModel>();
        }

        var byPath = new Dictionary<string, FileModel>(StringComparer.OrdinalIgnoreCase);
        foreach (FileModel f in stored)
        {
            byPath.TryAdd(f.FilePath, f);
        }

        var pending = new Stack<string>();
        pending.Push(directory);
        while (pending.Count > 0)
        {
            string current = pending.Pop();
            string[] subdirs = Array.Empty<string>();
            string[] files = Array.Empty<string>();
            try
            {
                subdirs = Directory.GetDirectories(current);
                files = Directory.GetFiles(current);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                continue;
            }
            foreach (string sub in subdirs)
            {
                if (!IsColdStoragePath(sub)) pending.Push(sub);
            }
            foreach (string path in files)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (IsColdStoragePath(path)) continue;
                if (IsExcluded(path, ExcludePatternsProvider?.Invoke())) continue;
                int maxSizeMb = MaxFileSizeMbProvider?.Invoke() ?? 0;
                string hash;
                long size;
                DateTime modified;
                try
                {
                    await using FileStream stream = File.OpenRead(path);
                    size = stream.Length;
                    if (maxSizeMb > 0 && size > maxSizeMb * 1024L * 1024L) continue;
                    hash = await _sha256.ComputeHashAsync(stream, cancellationToken);
                    modified = File.GetLastWriteTimeUtc(path);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    continue;
                }

            SyncStatus status;
            if (byPath.TryGetValue(path, out FileModel? existing))
                status = existing.Sha256Hash == hash ? SyncStatus.Synced : SyncStatus.Modified;
            else
                status = SyncStatus.Pending;

            results.Add(new FileModel
            {
                Id = existing?.Id ?? 0,
                FileName = Path.GetFileName(path),
                FilePath = path,
                SizeBytes = size,
                Sha256Hash = hash,
                Status = status,
                UploadedAt = modified
            });
            }
        }
        return results;
    }
}
