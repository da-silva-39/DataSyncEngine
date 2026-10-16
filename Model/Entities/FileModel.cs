using System.ComponentModel;
using Core.Catalog;
using Core.Enums;

namespace Model.Entities;

public class FileModel
{
    [Browsable(false)]
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    [Browsable(false)]
    public long SizeBytes { get; set; }
    public string Sha256Hash { get; set; } = string.Empty;
    public SyncStatus Status { get; set; } = SyncStatus.Pending;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Type")]
    public string Type => FileTypeCatalog.TypeName(FileName);

    [DisplayName("Size")]
    public string SizeText => FileTypeCatalog.HumanSize(SizeBytes);

    [DisplayName("Modified")]
    public DateTime Modified => UploadedAt.ToLocalTime();

    [DisplayName("Vault")]
    public string EncryptedText => IsEncrypted ? "AES-256-GCM" : "Pending";

    [Browsable(false)]
    public bool IsEncrypted => Status == SyncStatus.Synced;
}
