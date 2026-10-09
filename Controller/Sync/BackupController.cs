using Controller.Processing;
using Controller.Security;
using Model.Entities;
using Model.Repositories;

namespace Controller.Sync;

public class BackupController
{
    private readonly FileRepository _fileRepository;
    private readonly Aes256Controller _aes;
    private readonly StreamCompressorController _compressor;
    private readonly Func<byte[]> _masterKeyProvider;

    public event Action<string>? FileExported;

    public BackupController(FileRepository fileRepository, Aes256Controller aes,
        StreamCompressorController compressor, Func<byte[]> masterKeyProvider)
    {
        _fileRepository = fileRepository;
        _aes = aes;
        _compressor = compressor;
        _masterKeyProvider = masterKeyProvider;
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
                byte[] encrypted = _compressor.Decompress(blob);
                byte[] plain = _aes.Decrypt(encrypted, _masterKeyProvider());
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
