using Core.Config;
using Model.DataAccess;
using Model.Entities;
using MySqlConnector;

namespace Model.Repositories;

public class TrashRepository
{
    private readonly Func<ServerModel> _serverProvider;

    public TrashRepository(Func<ServerModel> serverProvider)
    {
        _serverProvider = serverProvider;
    }

    private static CancellationTokenSource Timeout()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(AppConstants.ConnectionTimeoutSeconds));
        return cts;
    }

    public async Task<int> MoveToTrashAsync(FileModel file, byte[] blob, string deletedBy, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "INSERT INTO trash_files (file_name, file_path, size_bytes, sha256_hash, status, uploaded_at, content_blob, deleted_by, deleted_at) VALUES (@n,@p,@s,@h,@st,@u,@b,@d,@t); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@n", file.FileName);
        cmd.Parameters.AddWithValue("@p", file.FilePath);
        cmd.Parameters.AddWithValue("@s", file.SizeBytes);
        cmd.Parameters.AddWithValue("@h", file.Sha256Hash);
        cmd.Parameters.AddWithValue("@st", file.Status.ToString());
        cmd.Parameters.AddWithValue("@u", file.UploadedAt);
        cmd.Parameters.AddWithValue("@b", blob);
        cmd.Parameters.AddWithValue("@d", deletedBy);
        cmd.Parameters.AddWithValue("@t", DateTime.UtcNow);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return Convert.ToInt32(result);
    }

    public async Task<IReadOnlyList<TrashEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<TrashEntry>();
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, file_name, file_path, size_bytes, sha256_hash, status, uploaded_at, deleted_by, deleted_at FROM trash_files ORDER BY deleted_at DESC", conn);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        while (reader.Read())
        {
            list.Add(new TrashEntry
            {
                Id = reader.GetInt32(0),
                FileName = reader.GetString(1),
                FilePath = reader.GetString(2),
                SizeBytes = reader.GetInt64(3),
                Sha256Hash = reader.GetString(4),
                Status = Enum.Parse<Core.Enums.SyncStatus>(reader.GetString(5)),
                UploadedAt = reader.GetDateTime(6),
                DeletedBy = reader.GetString(7),
                DeletedAt = reader.GetDateTime(8)
            });
        }
        return list;
    }

    public async Task<byte[]?> GetTrashBlobAsync(int trashId, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT content_blob FROM trash_files WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", trashId);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return result is byte[] blob ? blob : null;
    }

    public async Task<bool> DeleteAsync(int trashId, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("DELETE FROM trash_files WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", trashId);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }
}
