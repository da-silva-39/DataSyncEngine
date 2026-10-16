using Krypton.Toolkit;

namespace Views.Components;

/// <summary>
/// Renders the application shell with a GitHub-web inspired look: a dark
/// left sidebar, a flat content header, bordered cards and list-style
/// navigation items with an accent marker for the active section.
/// </summary>
public static class GitHubThemeModule
{
    private sealed record Surface(Color Pen, bool Top, bool Bottom, bool Left, bool Right);

    public static void StyleSurface(Control surface, Color background, bool top = false, bool bottom = false, bool left = false, bool right = false)
    {
        surface.BackColor = background;
        surface.Tag = new Surface(DarkThemeModule.BorderMutedColor, top, bottom, left, right);
        surface.Paint -= SurfacePaint;
        surface.Paint += SurfacePaint;
    }

    public static void StyleCard(Control card)
    {
        card.BackColor = DarkThemeModule.SubtleColor;
        card.Tag = new Surface(DarkThemeModule.BorderColor, true, true, true, true);
        card.Paint -= SurfacePaint;
        card.Paint += SurfacePaint;
    }

    private static void SurfacePaint(object? sender, PaintEventArgs e)
    {
        if (sender is not Control control || control.Tag is not Surface surface) return;
        Rectangle r = control.ClientRectangle;
        using var pen = new Pen(surface.Pen);
        if (surface.Top) e.Graphics.DrawLine(pen, 0, 0, r.Width, 0);
        if (surface.Bottom) e.Graphics.DrawLine(pen, 0, r.Height - 1, r.Width, r.Height - 1);
        if (surface.Left) e.Graphics.DrawLine(pen, 0, 0, 0, r.Height);
        if (surface.Right) e.Graphics.DrawLine(pen, r.Width - 1, 0, r.Width - 1, r.Height);
    }

    public static void StyleNav(KryptonButton button, bool selected)
    {
        Color sidebar = DarkThemeModule.SidebarColor;
        Color hover = DarkThemeModule.SubtleColor;
        Color text = DarkThemeModule.TextColor;
        Color muted = DarkThemeModule.MutedTextColor;
        Color background = selected ? hover : sidebar;
        Color foreground = selected ? text : muted;

        button.ButtonStyle = ButtonStyle.Standalone;
        button.StateCommon.Border.DrawBorders = PaletteDrawBorders.None;
        button.StateCommon.Border.Width = 0;

        ConfigureState(button.StateCommon, background, foreground, selected ? DarkThemeModule.AccentColor : Color.Empty, selected);
        ConfigureState(button.StateTracking, hover, text, Color.Empty, false);
        ConfigureState(button.StatePressed, hover, text, Color.Empty, false);
    }

    public static void StyleAction(KryptonButton button, Color background, Color foreground, Color hover)
    {
        button.ButtonStyle = ButtonStyle.Standalone;
        button.StateCommon.Border.Rounding = 6;
        button.StateCommon.Border.Width = 1;
        button.StateCommon.Border.DrawBorders = PaletteDrawBorders.All;
        button.StateCommon.Border.Color1 = background;
        button.StateCommon.Border.Color2 = background;
        button.StateCommon.Back.Color1 = background;
        button.StateCommon.Back.Color2 = background;
        button.StateCommon.Back.ColorStyle = PaletteColorStyle.Solid;
        button.StateCommon.Content.ShortText.Color1 = foreground;
        button.StateCommon.Content.ShortText.Color2 = foreground;
        button.StateCommon.Content.ShortText.TextH = PaletteRelativeAlign.Center;
        button.StateTracking.Back.Color1 = hover;
        button.StateTracking.Back.Color2 = hover;
        button.StateTracking.Back.ColorStyle = PaletteColorStyle.Solid;
        button.StateTracking.Border.Color1 = hover;
        button.StateTracking.Border.Color2 = hover;
        button.StateTracking.Content.ShortText.Color1 = foreground;
        button.StateTracking.Content.ShortText.Color2 = foreground;
        button.StatePressed.Back.Color1 = hover;
        button.StatePressed.Back.Color2 = hover;
        button.StatePressed.Back.ColorStyle = PaletteColorStyle.Solid;
        button.StatePressed.Content.ShortText.Color1 = foreground;
        button.StatePressed.Content.ShortText.Color2 = foreground;
    }

    private static void ConfigureState(PaletteTriple state, Color background, Color text, Color accent, bool accentBar)
        => ApplyTriple(state.Back, state.Border, state.Content, background, text, accent, accentBar);

    private static void ConfigureState(PaletteTripleRedirect state, Color background, Color text, Color accent, bool accentBar)
        => ApplyTriple(state.Back, state.Border, state.Content, background, text, accent, accentBar);

    private static void ApplyTriple(PaletteBack back, PaletteBorder border, PaletteContent content, Color background, Color text, Color accent, bool accentBar)
    {
        back.Color1 = background;
        back.Color2 = background;
        back.ColorStyle = PaletteColorStyle.Solid;
        border.DrawBorders = accentBar ? PaletteDrawBorders.Left : PaletteDrawBorders.None;
        border.Color1 = accentBar ? accent : background;
        border.Width = accentBar ? 3 : 0;
        content.ShortText.Color1 = text;
        content.ShortText.TextH = PaletteRelativeAlign.Near;
    }

    public static void StyleHeading(KryptonLabel label, Color color, float size, FontStyle style = FontStyle.Regular, string family = "Segoe UI")
    {
        label.StateCommon.ShortText.Color1 = color;
        label.StateCommon.ShortText.Font = new Font(family, size, style);
    }

    public static void StyleTree(KryptonTreeView tree)
    {
        tree.StateCommon.Back.Color1 = DarkThemeModule.SidebarColor;
        tree.StateCommon.Back.Color2 = DarkThemeModule.SidebarColor;
        tree.StateCommon.Node.Content.ShortText.Color1 = DarkThemeModule.MutedTextColor;
        tree.StateCommon.Node.Content.ShortText.Font = new Font("Segoe UI", 9F);
    }

    public static void StyleGrid(KryptonDataGridView grid)
    {
        grid.StateCommon.Background.Color1 = DarkThemeModule.CanvasColor;
        grid.StateCommon.Background.Color2 = DarkThemeModule.CanvasColor;
        grid.StateCommon.Background.ColorStyle = PaletteColorStyle.Solid;
        grid.StateCommon.DataCell.Back.Color1 = DarkThemeModule.CanvasColor;
        grid.StateCommon.DataCell.Back.Color2 = DarkThemeModule.CanvasColor;
        grid.StateCommon.DataCell.Border.Color1 = DarkThemeModule.BorderMutedColor;
        grid.StateCommon.DataCell.Border.Color2 = DarkThemeModule.BorderMutedColor;
        grid.StateCommon.DataCell.Content.Color1 = DarkThemeModule.TextColor;
        grid.StateCommon.DataCell.Content.Font = new Font("Segoe UI", 9F);
        grid.StateCommon.HeaderColumn.Back.Color1 = DarkThemeModule.SubtleColor;
        grid.StateCommon.HeaderColumn.Back.Color2 = DarkThemeModule.SubtleColor;
        grid.StateCommon.HeaderColumn.Content.Color1 = DarkThemeModule.MutedTextColor;
        grid.StateCommon.HeaderColumn.Content.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        grid.StateCommon.HeaderColumn.Border.Color1 = DarkThemeModule.BorderMutedColor;
        grid.StateCommon.HeaderRow.Back.Color1 = DarkThemeModule.SubtleColor;
        grid.StateCommon.HeaderRow.Back.Color2 = DarkThemeModule.SubtleColor;
        grid.GridColor = DarkThemeModule.BorderMutedColor;
        grid.BackgroundColor = DarkThemeModule.CanvasColor;
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.RowTemplate.Height = 30;
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
    }
}
