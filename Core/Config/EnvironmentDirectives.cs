namespace Core.Config;

public static class EnvironmentDirectives
{
    public const bool RequireSslTls = false;
    public const string MySqlDriver = "MySqlConnector";
    public static string BuildConnectionString(string server, int port, string database, string user, string password)
    {
        return $"Server={server};Port={port};Database={database};User ID={user};Password={password};SslMode=Preferred;Connection Timeout={AppConstants.ConnectionTimeoutSeconds};Default Command Timeout={AppConstants.CommandTimeoutSeconds}";
    }
}
