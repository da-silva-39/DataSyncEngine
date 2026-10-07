using Core.Enums;

namespace Model.Entities;

public class AuditLogModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public LogAction Action { get; set; }
    public string Details { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
