using Core.Config;
using Core.Interfaces;
using Model.DataAccess;
using Model.Entities;
using MySqlConnector;

namespace Model.Repositories;

public class ServerRepository : IServerRepository<ServerModel>
{
    private readonly Func<ServerModel> _serverProvider;

    public ServerRepository(Func<ServerModel> serverProvider)
    {
        _serverProvider = serverProvider;
    }

    private static CancellationTokenSource Timeout()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(AppConstants.ConnectionTimeoutSeconds));
        return cts;
    }

    public async Task<ServerModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, name, host, port, database_name, user_name, password, is_active FROM servers WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<IReadOnlyList<ServerModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<ServerModel>();
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, name, host, port, database_name, user_name, password, is_active FROM servers", conn);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        while (reader.Read()) list.Add(Map(reader));
        return list;
    }

    public async Task<int> InsertAsync(ServerModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "INSERT INTO servers (name, host, port, database_name, user_name, password, is_active) VALUES (@n,@h,@p,@d,@u,@pw,@a); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@n", entity.Name);
        cmd.Parameters.AddWithValue("@h", entity.Host);
        cmd.Parameters.AddWithValue("@p", entity.Port);
        cmd.Parameters.AddWithValue("@d", entity.Database);
        cmd.Parameters.AddWithValue("@u", entity.User);
        cmd.Parameters.AddWithValue("@pw", entity.Password);
        cmd.Parameters.AddWithValue("@a", entity.IsActive);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(ServerModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "UPDATE servers SET name=@n, host=@h, port=@p, database_name=@d, user_name=@u, password=@pw, is_active=@a WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@n", entity.Name);
        cmd.Parameters.AddWithValue("@h", entity.Host);
        cmd.Parameters.AddWithValue("@p", entity.Port);
        cmd.Parameters.AddWithValue("@d", entity.Database);
        cmd.Parameters.AddWithValue("@u", entity.User);
        cmd.Parameters.AddWithValue("@pw", entity.Password);
        cmd.Parameters.AddWithValue("@a", entity.IsActive);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("DELETE FROM servers WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<ServerModel?> GetActiveServerAsync(CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, name, host, port, database_name, user_name, password, is_active FROM servers WHERE is_active=1 LIMIT 1", conn);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        return reader.Read() ? Map(reader) : null;
    }

    private static ServerModel Map(MySqlDataReader reader)
    {
        return new ServerModel
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Host = reader.GetString(2),
            Port = reader.GetInt32(3),
            Database = reader.GetString(4),
            User = reader.GetString(5),
            Password = reader.GetString(6),
            IsActive = reader.GetBoolean(7)
        };
    }
}
