using Controller.Security;
using Model.Entities;
using Model.Repositories;

namespace Controller.Sync;

public class BackupController
{
    private readonly FileRepository _fileRepository;
    private readonly SecureVaultController _vault;
    private readonly Func<string> _passphraseProvider;

    public event Action<string>? FileExported;

    public BackupController(FileRepository fileRepository, SecureVaultController vault,
        Func<string> passphraseProvider)
    {
        _fileRepository = fileRepository;
        _vault = vault;
        _passphraseProvider = passphraseProvider;
    }

    public async Task<(int exported, int failed)> ExportAsync(string destFolder, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(destFolder);
        int ok = 0, fail = 0;
        IReadOnlyList<FileModel> files = await _fileRepository.GetAllAsync(cancellationToken);
        foreach (FileModel file in files)
        {
            try
            {
                byte[]? blob = await _fileRepository.GetFileBlobAsync(file.Id, cancellationToken);
                if (blob == null || blob.Length == 0) { fail++; continue; }
                byte[] plain = _vault.Open(blob, _passphraseProvider());
                string dest = GetUniquePath(destFolder, file.FileName);
                await File.WriteAllBytesAsync(dest, plain, cancellationToken);
                ok++;
                FileExported?.Invoke(file.FileName);
            }
            catch
            {
                fail++;
            }
        }
        return (ok, fail);
    }

    public async Task ExportSingleAsync(FileModel file, string destPath, CancellationToken cancellationToken = default)
    {
        byte[]? blob = await _fileRepository.GetFileBlobAsync(file.Id, cancellationToken);
        if (blob == null || blob.Length == 0) throw new InvalidOperationException("No content stored for this file.");
        byte[] plain = _vault.Open(blob, _passphraseProvider());
        string? dir = Path.GetDirectoryName(destPath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
        await File.WriteAllBytesAsync(destPath, plain, cancellationToken);
        FileExported?.Invoke(file.FileName);
    }

    public async Task<(int exported, int failed, int alreadyCurrent)> ExportUpdatedAsync(string destFolder, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(destFolder);
        int ok = 0, fail = 0, skipped = 0;
        IReadOnlyList<FileModel> files = await _fileRepository.GetAllAsync(cancellationToken);
        foreach (FileModel file in files)
        {
            try
            {
                byte[]? blob = await _fileRepository.GetFileBlobAsync(file.Id, cancellationToken);
                if (blob == null || blob.Length == 0) { fail++; continue; }

                string dest = Path.Combine(destFolder, file.FileName);
                if (File.Exists(dest))
                {
                    string existingHash = await HashFileAsync(dest, cancellationToken);
                    if (string.Equals(existingHash, file.Sha256Hash, StringComparison.OrdinalIgnoreCase))
                    {
                        skipped++;
                        continue;
                    }
                }

                byte[] plain = _vault.Open(blob, _passphraseProvider());
                if (!File.Exists(dest)) dest = GetUniquePath(destFolder, file.FileName);
                await File.WriteAllBytesAsync(dest, plain, cancellationToken);
                ok++;
                FileExported?.Invoke(file.FileName);
            }
            catch
            {
                fail++;
            }
        }
        return (ok, fail, skipped);
    }

    private static async Task<string> HashFileAsync(string path, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(path);
        byte[] hash = await global::System.Security.Cryptography.SHA256.HashDataAsync(stream, cancellationToken);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string GetUniquePath(string folder, string fileName)
    {
        string dest = Path.Combine(folder, fileName);
        if (!File.Exists(dest)) return dest;
        string stem = Path.GetFileNameWithoutExtension(fileName);
        string ext = Path.GetExtension(fileName);
        int i = 1;
        while (File.Exists(dest))
        {
            dest = Path.Combine(folder, $"{stem} ({i++}){ext}");
        }
        return dest;
    }
}
