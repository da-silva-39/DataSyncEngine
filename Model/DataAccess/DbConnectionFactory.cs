using Core.Config;
using MySqlConnector;

namespace Model.DataAccess;

public static class DbConnectionFactory
{
    public static MySqlConnection CreateConnection(Model.Entities.ServerModel server)
    {
        string connectionString = EnvironmentDirectives.BuildConnectionString(
            server.Host, server.Port, server.Database, server.User, server.Password);
        return new MySqlConnection(connectionString);
    }

    public static async Task<MySqlConnection> OpenConnectionAsync(
        Model.Entities.ServerModel server,
        CancellationToken cancellationToken = default)
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(AppConstants.ConnectionTimeoutSeconds));
        MySqlConnection connection = CreateConnection(server);
        try
        {
            await connection.OpenAsync(timeoutCts.Token);
            return connection;
        }
        catch
        {
            connection.Dispose();
            throw;
        }
    }
}
