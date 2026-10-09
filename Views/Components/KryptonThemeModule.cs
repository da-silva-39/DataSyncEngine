using Krypton.Toolkit;

namespace Views.Components;

public static class KryptonThemeModule
{
    private static readonly KryptonManager _manager = new();    public static readonly string[] AccentNames = { "Blue", "Green", "Red", "Purple" };

    public static void Initialize()
    {
        var settings = AppSettings.Load();
        SetTheme(settings.ThemeDark, false);
        DarkThemeModule.SetTheme(settings.ThemeDark);
    }

    public static void Apply(Form form)
    {
        if (!DarkThemeModule.IsDesignTime)
        {
            try
            {
                var s = AppSettings.Load();
                form.Font = new Font("Segoe UI", s.FontSize, s.FontBold ? FontStyle.Bold : FontStyle.Regular);
            }
            catch
            {
            }
        }
        DarkThemeModule.Apply(form);
    }

    public static void SetTheme(bool dark)
    {
        SetTheme(dark, true);
    }

    private static void SetTheme(bool dark, bool reapply)
    {
        _manager.GlobalPaletteMode = dark
            ? PaletteMode.Microsoft365BlackDarkMode
            : PaletteMode.Microsoft365White;
        DarkThemeModule.SetTheme(dark);
        if (!reapply) return;
        foreach (Form form in Application.OpenForms)
            DarkThemeModule.Apply(form);
    }

    public static void SetAccent(string name)
    {
        SetAccent(name, true);
    }

    private static void SetAccent(string name, bool reapply)
    {
        Color accent = name switch
        {
            "Green" => ColorTranslator.FromHtml("#2E7D32"),
            "Red" => ColorTranslator.FromHtml("#C62828"),
            "Purple" => ColorTranslator.FromHtml("#6A1B9A"),
            _ => ColorTranslator.FromHtml("#005A9E"),
        };
        DarkThemeModule.SetAccent(accent);
        if (!reapply) return;
        foreach (Form form in Application.OpenForms)
            DarkThemeModule.Apply(form);
    }

    public static void ApplyFontToOpenForms(float size, bool bold)
    {
        var font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
        foreach (Form form in Application.OpenForms)
        {
            form.Font = font;
            DarkThemeModule.Apply(form);
        }
    }
}
