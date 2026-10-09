using System.Text.Json;

namespace Views;

public class AppSettings
{
    public string ServerName { get; set; } = "local";
    public string ServerHost { get; set; } = "localhost";
    public int ServerPort { get; set; } = 3306;
    public string Database { get; set; } = "datasync_db";
    public string User { get; set; } = "root";
    public string Password { get; set; } = "jose200739";
    public string MasterKey { get; set; } = "DataSyncEngine-MasterKey-2026";
    public bool ThemeDark { get; set; } = true;
    public string Accent { get; set; } = "Blue";
    public float FontSize { get; set; } = 9.5f;
    public bool FontBold { get; set; } = false;
    public bool Animations { get; set; } = true;
    public bool ConfirmDelete { get; set; } = true;
    public bool Notifications { get; set; } = true;
    public bool KeepColdStorage { get; set; } = true;
    public bool MinimizeToTray { get; set; } = true;
    public string LastBackupAt { get; set; } = string.Empty;
    public bool AutoSync { get; set; } = false;
    public string ExcludePatterns { get; set; } = "*.tmp;*.log;~$*;*.bak;*.swp";
    public int IdleLogoutMinutes { get; set; } = 15;
    public int AutoBackupHours { get; set; } = 0;
    public string AutoBackupFolder { get; set; } = string.Empty;

    private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

    public static AppSettings Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath)) ?? new AppSettings();
            }
        }
        catch
        {
        }
        var settings = new AppSettings();
        Save(settings);
        return settings;
    }

    public static void Save(AppSettings settings)
    {
        try
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch
        {
        }
    }
}
