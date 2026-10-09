namespace Model.Entities;

public class TrashEntry
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public string Sha256Hash { get; set; } = string.Empty;
    public Core.Enums.SyncStatus Status { get; set; }
    public DateTime UploadedAt { get; set; }
    public string DeletedBy { get; set; } = string.Empty;
    public DateTime DeletedAt { get; set; }

    public FileModel ToFileModel()
    {
        return new FileModel
        {
            FileName = FileName,
            FilePath = FilePath,
            SizeBytes = SizeBytes,
            Sha256Hash = Sha256Hash,
            Status = Core.Enums.SyncStatus.Pending,
            UploadedAt = DateTime.UtcNow
        };
    }
}
