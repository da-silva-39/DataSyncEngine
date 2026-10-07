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
