namespace Core.Interfaces;

public interface IFileRepository<T> : IRepository<T> where T : class
{
    Task<byte[]?> GetFileBlobAsync(int fileId, CancellationToken cancellationToken = default);
    Task<int> InsertWithBlobAsync(T entity, byte[] blob, CancellationToken cancellationToken = default);
}
