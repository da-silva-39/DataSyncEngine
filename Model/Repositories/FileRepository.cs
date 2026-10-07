using Core.Config;
using Core.Interfaces;
using Model.DataAccess;
using Model.Entities;
using MySqlConnector;

namespace Model.Repositories;

public class FileRepository : IFileRepository<FileModel>
{
    private readonly Func<ServerModel> _serverProvider;

    public FileRepository(Func<ServerModel> serverProvider)
    {
        _serverProvider = serverProvider;
    }

    private static CancellationTokenSource Timeout()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(AppConstants.ConnectionTimeoutSeconds));
        return cts;
    }

    public async Task<FileModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, file_name, file_path, size_bytes, sha256_hash, status, uploaded_at FROM files WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<IReadOnlyList<FileModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<FileModel>();
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, file_name, file_path, size_bytes, sha256_hash, status, uploaded_at FROM files", conn);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        while (reader.Read()) list.Add(Map(reader));
        return list;
    }

    public async Task<int> InsertAsync(FileModel entity, CancellationToken cancellationToken = default)
    {
        return await InsertWithBlobAsync(entity, Array.Empty<byte>(), cancellationToken);
    }

    public async Task<int> InsertWithBlobAsync(FileModel entity, byte[] blob, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "INSERT INTO files (file_name, file_path, size_bytes, sha256_hash, status, uploaded_at, content_blob) VALUES (@n,@p,@s,@h,@st,@u,@b); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@n", entity.FileName);
        cmd.Parameters.AddWithValue("@p", entity.FilePath);
        cmd.Parameters.AddWithValue("@s", entity.SizeBytes);
        cmd.Parameters.AddWithValue("@h", entity.Sha256Hash);
        cmd.Parameters.AddWithValue("@st", entity.Status.ToString());
        cmd.Parameters.AddWithValue("@u", entity.UploadedAt);
        cmd.Parameters.AddWithValue("@b", blob);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(FileModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "UPDATE files SET file_name=@n, file_path=@p, size_bytes=@s, sha256_hash=@h, status=@st WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@n", entity.FileName);
        cmd.Parameters.AddWithValue("@p", entity.FilePath);
        cmd.Parameters.AddWithValue("@s", entity.SizeBytes);
        cmd.Parameters.AddWithValue("@h", entity.Sha256Hash);
        cmd.Parameters.AddWithValue("@st", entity.Status.ToString());
        cmd.Parameters.AddWithValue("@id", entity.Id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("DELETE FROM files WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<byte[]?> GetFileBlobAsync(int fileId, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT content_blob FROM files WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", fileId);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return result is byte[] blob ? blob : null;
    }

    private static FileModel Map(MySqlDataReader reader)
    {
        return new FileModel
        {
            Id = reader.GetInt32(0),
            FileName = reader.GetString(1),
            FilePath = reader.GetString(2),
            SizeBytes = reader.GetInt64(3),
            Sha256Hash = reader.GetString(4),
            Status = Enum.Parse<Core.Enums.SyncStatus>(reader.GetString(5)),
            UploadedAt = reader.GetDateTime(6)
        };
    }
}
