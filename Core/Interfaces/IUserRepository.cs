namespace Core.Interfaces;

public interface IUserRepository<T> : IRepository<T> where T : class
{
    Task<T?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> ValidateCredentialsAsync(string username, string passwordHash, string salt, CancellationToken cancellationToken = default);
}
