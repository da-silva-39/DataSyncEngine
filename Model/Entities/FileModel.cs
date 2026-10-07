using Core.Enums;

namespace Model.Entities;

public class FileModel
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public string Sha256Hash { get; set; } = string.Empty;
    public SyncStatus Status { get; set; } = SyncStatus.Pending;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
