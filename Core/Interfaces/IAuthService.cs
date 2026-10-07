namespace Core.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
    void Logout();
    bool IsAuthenticated { get; }
    string? CurrentUsername { get; }
    Core.Enums.UserRole? CurrentRole { get; }
}
