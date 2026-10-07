namespace Controller.System;

public class SessionController
{
    public string? Username { get; set; }
    public Core.Enums.UserRole? Role { get; set; }
    public DateTime? StartedAt { get; set; }
    public bool IsActive => Username != null;

    public void Start(string username, Core.Enums.UserRole role)
    {
        Username = username;
        Role = role;
        StartedAt = DateTime.UtcNow;
    }

    public void End()
    {
        Username = null;
        Role = null;
        StartedAt = null;
    }
}
