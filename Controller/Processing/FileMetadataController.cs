using System.ComponentModel;
using Controller.Security;
using Core.Catalog;
using Model.Entities;
using Model.Repositories;

namespace Controller.Processing;

public sealed record MetadataItem(string Group, string Label, string Value);

public class FileMetadataController
{
    public IReadOnlyList<MetadataItem> Inspect(FileModel file)
    {
        var list = new List<MetadataItem>
        {
            new("Summary", "Name", file.FileName),
            new("Summary", "Status", file.Status.ToString()),
            new("Summary", "Vault", file.EncryptedText)
        };

        if (string.IsNullOrEmpty(file.FilePath)) return list;

        try
        {
            var fi = new FileInfo(file.FilePath);
            list.Add(new("Filesystem", "Full path", fi.FullName));
            list.Add(new("Filesystem", "Extension", FileTypeCatalog.Extension(file.FileName)));
            list.Add(new("Filesystem", "Type", FileTypeCatalog.TypeName(file.FileName, fi.Attributes.HasFlag(FileAttributes.Directory))));
            list.Add(new("Filesystem", "Category", FileTypeCatalog.Category(file.FileName)));
            list.Add(new("Filesystem", "Size on disk", FileTypeCatalog.HumanSize(fi.Length)));
            list.Add(new("Filesystem", "Size recorded", FileTypeCatalog.HumanSize(file.SizeBytes)));
            list.Add(new("Filesystem", "Created (local)", fi.CreationTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
            list.Add(new("Filesystem", "Modified (local)", fi.LastWriteTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
            list.Add(new("Filesystem", "Accessed (local)", fi.LastAccessTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
            list.Add(new("Filesystem", "Attributes", fi.Attributes.ToString()));
            list.Add(new("Filesystem", "Read-only", fi.IsReadOnly ? "Yes" : "No"));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            list.Add(new("Filesystem", "Info", "Cannot access: " + ex.Message));
        }

        list.Add(new("Sync", "SHA-256", file.Sha256Hash));
        list.Add(new("Sync", "Uploaded at", file.UploadedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")));
        list.Add(new("Sync", "Server ID", file.Id > 0 ? file.Id.ToString() : "(not synced)"));

        return list;
    }

    public async Task<IReadOnlyList<MetadataItem>> InspectVaultAsync(FileModel file, FileRepository repo, SecureVaultController vault)
    {
        var items = new List<MetadataItem>(Inspect(file));
        if (file.Id <= 0) return items;

        try
        {
            byte[]? blob = await repo.GetFileBlobAsync(file.Id, CancellationToken.None);
            if (blob == null || blob.Length == 0) return items;

            var desc = vault.Describe(blob);
            items.Add(new("Vault", "Format", desc.Format));
            items.Add(new("Vault", "Algorithm", desc.Algorithm));
            items.Add(new("Vault", "Key derivation", desc.KeyDerivation));
            items.Add(new("Vault", "Authenticated (GCM)", desc.IsAuthenticated ? "Yes" : "No"));
            if (!string.IsNullOrEmpty(desc.SaltHex))
                items.Add(new("Vault", "Salt (hex)", desc.SaltHex));
            if (!string.IsNullOrEmpty(desc.NonceHex))
                items.Add(new("Vault", "Nonce (hex)", desc.NonceHex));
            items.Add(new("Vault", "Container bytes", desc.PayloadBytes.ToString()));
        }
        catch
        {
        }
        return items;
    }
}
