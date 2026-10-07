namespace Core.Interfaces;

public interface IAuditRepository<T> : IRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetByUserAsync(string username, CancellationToken cancellationToken = default);
}
