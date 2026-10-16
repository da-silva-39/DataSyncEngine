using Microsoft.Win32;

namespace Controller.System;

/// <summary>
/// Manages the per-user Windows "Run" registry entry so the application can
/// start automatically with the session. Lives in the controller layer so the
/// views only toggle a setting and never touch the registry directly.
/// </summary>
public static class AutoStartController
{
    private const string RunKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    private const string ValueName = "DataSyncEngine";

    public static bool IsEnabled()
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, false);
            return key?.GetValue(ValueName) != null;
        }
        catch
        {
            return false;
        }
    }

    public static bool SetEnabled(bool enabled)
    {
        try
        {
            using RegistryKey? key = Registry.CurrentUser.OpenSubKey(RunKeyPath, true);
            if (key == null) return false;
            if (enabled)
            {
                string exe = Environment.ProcessPath ?? Path.Combine(AppContext.BaseDirectory, "DataSyncEngine.exe");
                key.SetValue(ValueName, $"\"{exe}\"");
            }
            else
            {
                key.DeleteValue(ValueName, false);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }
}
