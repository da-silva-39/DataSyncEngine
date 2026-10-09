using MaterialSkin;
using MaterialSkin.Controls;

namespace Views.Components;

public static class MaterialThemeModule
{
    public static void Initialize()
    {
        var skin = MaterialSkinManager.Instance;
        skin.Theme = MaterialSkinManager.Themes.DARK;
        skin.ColorScheme = new ColorScheme(
            ColorTranslator.FromHtml("#007ACC"),
            ColorTranslator.FromHtml("#005A9E"),
            ColorTranslator.FromHtml("#339DDD"),
            ColorTranslator.FromHtml("#40C4FF"),
            TextShade.WHITE);
        DarkThemeModule.SetTheme(true);
    }

    public static void Apply(Form form)
    {
        if (form is MaterialForm materialForm)
            MaterialSkinManager.Instance.AddFormToManage(materialForm);
        DarkThemeModule.Apply(form);
    }

    public static void SetTheme(bool dark)
    {
        MaterialSkinManager.Instance.Theme = dark
            ? MaterialSkinManager.Themes.DARK
            : MaterialSkinManager.Themes.LIGHT;
        DarkThemeModule.SetTheme(dark);
        foreach (Form form in Application.OpenForms)
            DarkThemeModule.Apply(form);
    }
}
