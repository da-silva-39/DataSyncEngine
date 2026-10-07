namespace Core.Interfaces;

public interface IServerRepository<T> : IRepository<T> where T : class
{
    Task<T?> GetActiveServerAsync(CancellationToken cancellationToken = default);
}
