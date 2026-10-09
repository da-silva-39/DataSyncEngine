using Core.Config;
using Core.Interfaces;
using Model.DataAccess;
using Model.Entities;
using MySqlConnector;

namespace Model.Repositories;

public class UserRepository : IUserRepository<UserModel>
{
    private readonly Func<ServerModel> _serverProvider;

    public UserRepository(Func<ServerModel> serverProvider)
    {
        _serverProvider = serverProvider;
    }

    private static CancellationTokenSource Timeout()
    {
        var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(AppConstants.ConnectionTimeoutSeconds));
        return cts;
    }

    public async Task<UserModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, username, password_hash, salt, role, is_active, avatar_blob FROM users WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<IReadOnlyList<UserModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var list = new List<UserModel>();
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, username, password_hash, salt, role, is_active, avatar_blob FROM users", conn);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        while (reader.Read()) list.Add(Map(reader));
        return list;
    }

    public async Task<int> InsertAsync(UserModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "INSERT INTO users (username, password_hash, salt, role, is_active, avatar_blob) VALUES (@u,@h,@s,@r,@a,@av); SELECT LAST_INSERT_ID();", conn);
        cmd.Parameters.AddWithValue("@u", entity.Username);
        cmd.Parameters.AddWithValue("@h", entity.PasswordHash);
        cmd.Parameters.AddWithValue("@s", entity.Salt);
        cmd.Parameters.AddWithValue("@r", entity.Role.ToString());
        cmd.Parameters.AddWithValue("@a", entity.IsActive);
        cmd.Parameters.AddWithValue("@av", (object?)entity.Avatar ?? DBNull.Value);
        object? result = await cmd.ExecuteScalarAsync(linked.Token);
        return Convert.ToInt32(result);
    }

    public async Task<bool> UpdateAsync(UserModel entity, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand(
            "UPDATE users SET username=@u, password_hash=@h, salt=@s, role=@r, is_active=@a, avatar_blob=@av WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@u", entity.Username);
        cmd.Parameters.AddWithValue("@h", entity.PasswordHash);
        cmd.Parameters.AddWithValue("@s", entity.Salt);
        cmd.Parameters.AddWithValue("@r", entity.Role.ToString());
        cmd.Parameters.AddWithValue("@a", entity.IsActive);
        cmd.Parameters.AddWithValue("@av", (object?)entity.Avatar ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", entity.Id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("DELETE FROM users WHERE id=@id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync(linked.Token) > 0;
    }

    public async Task<UserModel?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        using var timeout = Timeout();
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);
        await using MySqlConnection conn = await DbConnectionFactory.OpenConnectionAsync(_serverProvider(), linked.Token);
        await using var cmd = new MySqlCommand("SELECT id, username, password_hash, salt, role, is_active, avatar_blob FROM users WHERE username=@u", conn);
        cmd.Parameters.AddWithValue("@u", username);
        await using MySqlDataReader reader = await cmd.ExecuteReaderAsync(linked.Token);
        return reader.Read() ? Map(reader) : null;
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string passwordHash, string salt, CancellationToken cancellationToken = default)
    {
        UserModel? user = await GetByUsernameAsync(username, cancellationToken);
        return user != null && user.PasswordHash == passwordHash && user.Salt == salt && user.IsActive;
    }

    private static UserModel Map(MySqlDataReader reader)
    {
        return new UserModel
        {
            Id = reader.GetInt32(0),
            Username = reader.GetString(1),
            PasswordHash = reader.GetString(2),
            Salt = reader.GetString(3),
            Role = Enum.Parse<Core.Enums.UserRole>(reader.GetString(4)),
            Avatar = reader.IsDBNull(6) ? null : (byte[])reader.GetValue(6),
            IsActive = reader.GetBoolean(5)
        };
    }
}
