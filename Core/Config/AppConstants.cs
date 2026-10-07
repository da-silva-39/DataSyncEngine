namespace Core.Config;

public static class AppConstants
{
    public const int ConnectionTimeoutSeconds = 5;
    public const int CommandTimeoutSeconds = 5;
    public const int SaltSizeBytes = 32;
    public const int MasterKeyLength = 32;
    public const string DatabaseName = "datasync_db";
    public const string ColdStorageFolderName = ".datasync_cold";
}
