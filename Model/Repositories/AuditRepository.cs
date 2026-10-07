using Core.Config;
using Core.Interfaces;
using Model.DataAccess;
using Model.Entities;
using MySqlConnector;

namespace Model.Repositories;

public class AuditRepository : IAuditRepository<AuditLogModel>
{
    private readonly Func<ServerModel> _serverProvider;

    public AuditRepository(Func<ServerModel> serverProvider)
    {
        _serverProvider = serverProvider;
    }

    private static CancellationTokenSource Timeout()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(AppConstants.ConnectionTimeoutSeconds));
        return cts;
    }

    public async Task<AuditLogModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, username, action, details, timestamp FROM audit_logs WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<IReadOnlyList<AuditLogModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<AuditLogModel>();
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, username, action, details, timestamp FROM audit_logs ORDER BY timestamp DESC", conn);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        while (reader.Read()) list.Add(Map(reader));
        return list;
    }

    public async Task<IReadOnlyList<AuditLogModel>> GetByUserAsync(string username, CancellationToken cancellationToken = default)
    {
        var list = new List<AuditLogModel>();
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, username, action, details, timestamp FROM audit_logs WHERE username=@u ORDER BY timestamp DESC", conn);
        cmd.Parameters.AddWithValue("@u", username);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        while (reader.Read()) list.Add(Map(reader));
        return list;
    }

    public async Task<int> InsertAsync(AuditLogModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "INSERT INTO audit_logs (username, action, details, timestamp) VALUES (@u,@a,@d,@t); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@u", entity.Username);
        cmd.Parameters.AddWithValue("@a", entity.Action.ToString());
        cmd.Parameters.AddWithValue("@d", entity.Details);
        cmd.Parameters.AddWithValue("@t", entity.Timestamp);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(AuditLogModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "UPDATE audit_logs SET username=@u, action=@a, details=@d WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@u", entity.Username);
        cmd.Parameters.AddWithValue("@a", entity.Action.ToString());
        cmd.Parameters.AddWithValue("@d", entity.Details);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("DELETE FROM audit_logs WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    private static AuditLogModel Map(MySqlDataReader reader)
    {
        return new AuditLogModel
        {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            Action = Enum.Parse<Core.Enums.LogAction>(reader.GetString(2)),
            Details = reader.GetString(3),
            Timestamp = reader.GetDateTime(4)
        };
    }
}
