using Krypton.Toolkit;

namespace Views.Components;

/// <summary>
/// Custom Krypton palette that renders every Krypton control with the same
/// GitHub Primer dark tokens used by the application shell (#0D1117 canvas,
/// #161B22 subtle, #010409 sidebar). This keeps the whole system — forms,
/// buttons, inputs, toolbars and menus — visually identical to the splash /
/// processing screen instead of the neutral near-black Microsoft 365 palette.
/// </summary>
public sealed class GitHubDarkPalette : KryptonCustomPaletteBase
{
    private static Color H(string hex) => ColorTranslator.FromHtml(hex);

    public GitHubDarkPalette(Color accent)
    {
        BasePalette = KryptonManager.PaletteMicrosoft365BlackDarkMode;
        Define(accent);
    }

    private void Define(Color accent)
    {
        Color canvas = H("#0D1117");
        Color subtle = H("#161B22");
        Color sidebar = H("#010409");
        Color hover = H("#21262D");
        Color border = H("#30363D");
        Color borderMuted = H("#21262D");
        Color text = H("#E6EDF3");
        Color muted = H("#8B949E");

        Common.StateCommon.Back.Color1 = canvas;
        Common.StateCommon.Back.Color2 = canvas;
        Common.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;
        Common.StateCommon.Border.Color1 = border;
        Common.StateCommon.Border.Color2 = border;
        Common.StateCommon.Content.ShortText.Color1 = text;
        Common.StateCommon.Content.LongText.Color1 = text;

        FormStyles.FormCommon.StateCommon.Back.Color1 = canvas;
        FormStyles.FormCommon.StateCommon.Back.Color2 = canvas;
        FormStyles.FormCommon.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;

        HeaderStyles.HeaderCommon.StateCommon.Back.Color1 = canvas;
        HeaderStyles.HeaderCommon.StateCommon.Back.Color2 = canvas;
        HeaderStyles.HeaderCommon.StateCommon.Content.ShortText.Color1 = text;

        KryptonPaletteCheckButton[] buttons =
        {
            ButtonStyles.ButtonCommon, ButtonStyles.ButtonStandalone, ButtonStyles.ButtonAlternate,
            ButtonStyles.ButtonLowProfile, ButtonStyles.ButtonCommand, ButtonStyles.ButtonInputControl, ButtonStyles.ButtonForm
        };
        foreach (KryptonPaletteCheckButton b in buttons)
        {
            b.StateCommon.Back.Color1 = subtle;
            b.StateCommon.Back.Color2 = subtle;
            b.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;
            b.StateCommon.Border.Color1 = borderMuted;
            b.StateCommon.Border.Color2 = borderMuted;
            b.StateCommon.Content.ShortText.Color1 = text;
            b.StateCommon.Content.LongText.Color1 = text;
            b.StateNormal.Back.Color1 = subtle;
            b.StateNormal.Back.Color2 = subtle;
            b.StateNormal.Content.ShortText.Color1 = text;
            b.StateTracking.Back.Color1 = hover;
            b.StateTracking.Back.Color2 = hover;
            b.StateTracking.Content.ShortText.Color1 = text;
            b.StatePressed.Back.Color1 = border;
            b.StatePressed.Back.Color2 = border;
            b.StatePressed.Content.ShortText.Color1 = text;
            b.StateCheckedNormal.Back.Color1 = accent;
            b.StateCheckedNormal.Back.Color2 = accent;
            b.StateCheckedNormal.Content.ShortText.Color1 = Color.White;
        }

        ButtonStyles.ButtonListItem.StateCommon.Back.Color1 = sidebar;
        ButtonStyles.ButtonListItem.StateCommon.Back.Color2 = sidebar;
        ButtonStyles.ButtonListItem.StateCommon.Content.ShortText.Color1 = muted;
        ButtonStyles.ButtonListItem.StateTracking.Back.Color1 = subtle;
        ButtonStyles.ButtonListItem.StateTracking.Back.Color2 = subtle;
        ButtonStyles.ButtonListItem.StateTracking.Content.ShortText.Color1 = text;

        InputControlStyles.InputControlCommon.StateCommon.Back.Color1 = canvas;
        InputControlStyles.InputControlCommon.StateCommon.Back.Color2 = canvas;
        InputControlStyles.InputControlCommon.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;
        InputControlStyles.InputControlCommon.StateCommon.Border.Color1 = border;
        InputControlStyles.InputControlCommon.StateCommon.Border.Color2 = border;
        InputControlStyles.InputControlCommon.StateCommon.Border.Rounding = 4;
        InputControlStyles.InputControlCommon.StateCommon.Content.ShortText.Color1 = text;
        InputControlStyles.InputControlCommon.StateNormal.Back.Color1 = canvas;
        InputControlStyles.InputControlCommon.StateNormal.Content.ShortText.Color1 = text;
        InputControlStyles.InputControlCommon.StateActive.Back.Color1 = canvas;
        InputControlStyles.InputControlCommon.StateActive.Border.Color1 = accent;
        InputControlStyles.InputControlCommon.StateActive.Content.ShortText.Color1 = text;

        LabelStyles.LabelCommon.StateCommon.ShortText.Color1 = text;
        LabelStyles.LabelNormalControl.StateCommon.ShortText.Color1 = text;
        LabelStyles.LabelBoldControl.StateCommon.ShortText.Color1 = text;
        LabelStyles.LabelNormalPanel.StateCommon.ShortText.Color1 = text;
        LabelStyles.LabelBoldPanel.StateCommon.ShortText.Color1 = text;
        LabelStyles.LabelTitleControl.StateCommon.ShortText.Color1 = text;

        PanelStyles.PanelCommon.StateCommon.Color1 = canvas;
        PanelStyles.PanelCommon.StateCommon.Color2 = canvas;
        PanelStyles.PanelCommon.StateCommon.ColorStyle = PaletteColorStyle.Solid;
        PanelStyles.PanelClient.StateCommon.Color1 = canvas;
        PanelStyles.PanelClient.StateCommon.Color2 = canvas;
        PanelStyles.PanelClient.StateCommon.ColorStyle = PaletteColorStyle.Solid;

        ControlStyles.ControlClient.StateCommon.Back.Color1 = canvas;
        ControlStyles.ControlClient.StateCommon.Back.Color2 = canvas;
        ControlStyles.ControlClient.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;
        ControlStyles.ControlCommon.StateCommon.Back.Color1 = canvas;
        ControlStyles.ControlCommon.StateCommon.Back.Color2 = canvas;
        ControlStyles.ControlCommon.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;

        var separatorState = SeparatorStyles.SeparatorCommon?.StateCommon;
        if (separatorState != null)
        {
            separatorState.Back.Color1 = borderMuted;
        }

        ToolMenuStatus.ToolStrip.ToolStripGradientBegin = canvas;
        ToolMenuStatus.ToolStrip.ToolStripGradientMiddle = canvas;
        ToolMenuStatus.ToolStrip.ToolStripGradientEnd = canvas;
        ToolMenuStatus.ToolStrip.ToolStripText = text;
        ToolMenuStatus.ToolStrip.ToolStripBorder = borderMuted;
        ToolMenuStatus.ToolStrip.ToolStripDropDownBackground = subtle;

        ToolMenuStatus.StatusStrip.StatusStripGradientBegin = canvas;
        ToolMenuStatus.StatusStrip.StatusStripGradientEnd = canvas;
        ToolMenuStatus.StatusStrip.StatusStripText = muted;

        ToolMenuStatus.Menu.MenuItemText = text;
        ToolMenuStatus.Menu.MenuItemBorder = border;
        ToolMenuStatus.Menu.MenuItemSelected = subtle;
        ToolMenuStatus.Menu.MenuItemSelectedGradientBegin = subtle;
        ToolMenuStatus.Menu.MenuItemSelectedGradientEnd = subtle;
        ToolMenuStatus.Menu.MenuItemPressedGradientBegin = subtle;
        ToolMenuStatus.Menu.MenuItemPressedGradientMiddle = subtle;
        ToolMenuStatus.Menu.MenuItemPressedGradientEnd = subtle;
        ToolMenuStatus.Menu.MenuBorder = border;
        ToolMenuStatus.Menu.ImageMarginGradientBegin = subtle;
        ToolMenuStatus.Menu.ImageMarginGradientMiddle = subtle;
        ToolMenuStatus.Menu.ImageMarginGradientEnd = subtle;
    }
}
