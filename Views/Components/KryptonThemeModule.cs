using Krypton.Toolkit;

namespace Views.Components;

public static class KryptonThemeModule
{
    private static readonly KryptonManager _manager = new();

    public static readonly string[] AccentNames = { "Blue", "Green", "Red", "Purple" };

    /// <summary>Raised after the global palette or accent changed so custom
    /// (non-Krypton) surfaces such as the GitHub-style shell can re-colour.</summary>
    public static event Action? ThemeChanged;

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
        LocalizationModule.Apply(form);
        DarkThemeModule.Apply(form);
    }

    public static void SetTheme(bool dark)
    {
        SetTheme(dark, true);
    }

    private static void SetTheme(bool dark, bool reapply)
    {
        var settings = AppSettings.Load();
        if (dark)
        {
            // Use the custom GitHub-dark palette so every Krypton surface shares
            // the same #0D1117 family as the splash / processing screen.
            _manager.GlobalCustomPalette = new GitHubDarkPalette(ResolveAccent(settings.Accent, true));
        }
        else
        {
            _manager.GlobalCustomPalette = null;
            _manager.GlobalPaletteMode = PaletteMode.Microsoft365White;
        }
        DarkThemeModule.SetTheme(dark);
        ApplyAccentColor(settings.Accent, dark);
        if (!reapply) return;
        foreach (Form form in Application.OpenForms)
            DarkThemeModule.Apply(form);
        ThemeChanged?.Invoke();
    }

    public static void SetAccent(string name)
    {
        SetAccent(name, true);
    }

    private static void SetAccent(string name, bool reapply)
    {
        ApplyAccentColor(name, DarkThemeModule.IsDark);
        if (DarkThemeModule.IsDark)
            _manager.GlobalCustomPalette = new GitHubDarkPalette(ResolveAccent(name, true));
        if (!reapply) return;
        foreach (Form form in Application.OpenForms)
            DarkThemeModule.Apply(form);
        ThemeChanged?.Invoke();
    }

    private static Color ResolveAccent(string name, bool dark)
    {
        return (name, dark) switch
        {
            ("Green", true) => ColorTranslator.FromHtml("#238636"),
            ("Green", false) => ColorTranslator.FromHtml("#1F883D"),
            ("Red", true) => ColorTranslator.FromHtml("#DA3633"),
            ("Red", false) => ColorTranslator.FromHtml("#CF222E"),
            ("Purple", true) => ColorTranslator.FromHtml("#8957E5"),
            ("Purple", false) => ColorTranslator.FromHtml("#8250DF"),
            (_, true) => ColorTranslator.FromHtml("#1F6FEB"),
            _ => ColorTranslator.FromHtml("#0969DA"),
        };
    }

    private static void ApplyAccentColor(string name, bool dark)
    {
        DarkThemeModule.SetAccent(ResolveAccent(name, dark));
    }

    public static void ApplyFontToOpenForms(float size, bool bold)
    {
        var font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
        foreach (Form form in Application.OpenForms)
        {
            form.Font = font;
            DarkThemeModule.Apply(form);
        }
        ThemeChanged?.Invoke();
    }
}
