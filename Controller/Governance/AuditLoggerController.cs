using Core.Enums;
using Model.Entities;
using Model.Repositories;

namespace Controller.Governance;

public class AuditLoggerController
{
    private readonly Func<AuditRepository> _auditRepositoryFactory;
    private readonly Func<string?> _usernameProvider;

    public AuditLoggerController(Func<AuditRepository> auditRepositoryFactory, Func<string?> usernameProvider)
    {
        _auditRepositoryFactory = auditRepositoryFactory;
        _usernameProvider = usernameProvider;
    }

    public async Task LogAsync(LogAction action, string details, CancellationToken cancellationToken = default)
    {
        try
        {
            var entry = new AuditLogModel
            {
                Username = _usernameProvider() ?? "anonymous",
                Action = action,
                Details = details,
                Timestamp = DateTime.UtcNow
            };
            await _auditRepositoryFactory().InsertAsync(entry, cancellationToken);
        }
        catch
        {
        }
    }

    public Task LogLoginAsync(string username) => LogAsync(LogAction.Login, $"User {username} logged in.");
    public Task LogLogoutAsync(string username) => LogAsync(LogAction.Logout, $"User {username} logged out.");
    public Task LogErrorAsync(string details) => LogAsync(LogAction.Error, details);
}
