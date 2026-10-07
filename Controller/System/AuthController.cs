using Controller.Security;
using Core.Enums;
using Core.Interfaces;
using Model.Entities;
using Model.Repositories;

namespace Controller.System;

public class AuthController : IAuthService
{
    private static AuthController? _instance;
    private static readonly object _lock = new();

    private readonly SessionController _session;
    private readonly PasswordHasher _hasher;
    private readonly Func<UserRepository> _userRepositoryFactory;

    public bool IsAuthenticated => _session.IsActive;
    public string? CurrentUsername => _session.Username;
    public UserRole? CurrentRole => _session.Role;

    private AuthController(SessionController session, PasswordHasher hasher, Func<UserRepository> userRepositoryFactory)
    {
        _session = session;
        _hasher = hasher;
        _userRepositoryFactory = userRepositoryFactory;
    }

    public static AuthController Initialize(SessionController session, PasswordHasher hasher, Func<UserRepository> userRepositoryFactory)
    {
        lock (_lock)
        {
            _instance ??= new AuthController(session, hasher, userRepositoryFactory);
            return _instance;
        }
    }

    public static AuthController Instance => _instance ?? throw new InvalidOperationException("AuthController not initialized.");

    public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            UserRepository repo = _userRepositoryFactory();
            UserModel? user = await repo.GetByUsernameAsync(username, cancellationToken);
            if (user == null || !user.IsActive) return false;
            if (!_hasher.Verify(password, user.Salt, user.PasswordHash)) return false;
            _session.Start(user.Username, user.Role);
            return true;
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException("Connection to the database timed out.");
        }
    }

    public void Logout() => _session.End();
}
