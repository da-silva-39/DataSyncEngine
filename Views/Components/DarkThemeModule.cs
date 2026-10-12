using System.Runtime.InteropServices;

namespace Views.Components;

/// <summary>
/// Central colour registry for the application. The palette mirrors GitHub's
/// Primer design tokens (canvas, subtle, border, accent, success, danger) so the
/// desktop client shares the same visual language as GitHub web.
/// </summary>
public static class DarkThemeModule
{
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

    public static void SetDarkTitleBar(Form form)
    {
        if (IsDesignTime) return;
        try
        {
            int useDark = IsDark ? 1 : 0;
            DwmSetWindowAttribute(form.Handle, 20, ref useDark, sizeof(int));
        }
        catch
        {
        }
    }

    // ---- GitHub Primer palette (dark / light) ---------------------------------
    public static Color CanvasColor { get; private set; } = ColorTranslator.FromHtml("#0D1117");
    public static Color SubtleColor { get; private set; } = ColorTranslator.FromHtml("#161B22");
    public static Color SidebarColor { get; private set; } = ColorTranslator.FromHtml("#010409");
    public static Color HeaderColor { get; private set; } = ColorTranslator.FromHtml("#0D1117");
    public static Color BorderColor { get; private set; } = ColorTranslator.FromHtml("#30363D");
    public static Color BorderMutedColor { get; private set; } = ColorTranslator.FromHtml("#21262D");
    public static Color TextColor { get; private set; } = ColorTranslator.FromHtml("#E6EDF3");
    public static Color MutedTextColor { get; private set; } = ColorTranslator.FromHtml("#8B949E");
    public static Color AccentColor { get; private set; } = ColorTranslator.FromHtml("#58A6FF");
    public static Color PrimaryColor { get; private set; } = ColorTranslator.FromHtml("#238636");
    public static Color SuccessColor { get; private set; } = ColorTranslator.FromHtml("#3FB950");
    public static Color WarningColor { get; private set; } = ColorTranslator.FromHtml("#D29922");
    public static Color ModifiedColor { get; private set; } = ColorTranslator.FromHtml("#DB6D28");
    public static Color DangerColor { get; private set; } = ColorTranslator.FromHtml("#F85149");

    // Backwards compatible aliases
    public static Color BackgroundColor => CanvasColor;
    public static Color PanelColor => SubtleColor;

    public static bool IsDark { get; private set; } = true;

    public static bool IsDesignTime => System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;

    public static void SetTheme(bool dark)
    {
        IsDark = dark;
        if (dark)
        {
            CanvasColor = ColorTranslator.FromHtml("#0D1117");
            SubtleColor = ColorTranslator.FromHtml("#161B22");
            SidebarColor = ColorTranslator.FromHtml("#010409");
            HeaderColor = ColorTranslator.FromHtml("#0D1117");
            BorderColor = ColorTranslator.FromHtml("#30363D");
            BorderMutedColor = ColorTranslator.FromHtml("#21262D");
            TextColor = ColorTranslator.FromHtml("#E6EDF3");
            MutedTextColor = ColorTranslator.FromHtml("#8B949E");
            AccentColor = ColorTranslator.FromHtml("#58A6FF");
            PrimaryColor = ColorTranslator.FromHtml("#238636");
            SuccessColor = ColorTranslator.FromHtml("#3FB950");
            WarningColor = ColorTranslator.FromHtml("#D29922");
            ModifiedColor = ColorTranslator.FromHtml("#DB6D28");
            DangerColor = ColorTranslator.FromHtml("#F85149");
        }
        else
        {
            CanvasColor = ColorTranslator.FromHtml("#FFFFFF");
            SubtleColor = ColorTranslator.FromHtml("#F6F8FA");
            SidebarColor = ColorTranslator.FromHtml("#F6F8FA");
            HeaderColor = ColorTranslator.FromHtml("#FFFFFF");
            BorderColor = ColorTranslator.FromHtml("#D0D7DE");
            BorderMutedColor = ColorTranslator.FromHtml("#D8DEE4");
            TextColor = ColorTranslator.FromHtml("#1F2328");
            MutedTextColor = ColorTranslator.FromHtml("#656D76");
            AccentColor = ColorTranslator.FromHtml("#0969DA");
            PrimaryColor = ColorTranslator.FromHtml("#1F883D");
            SuccessColor = ColorTranslator.FromHtml("#1A7F37");
            WarningColor = ColorTranslator.FromHtml("#9A6700");
            ModifiedColor = ColorTranslator.FromHtml("#BC4C00");
            DangerColor = ColorTranslator.FromHtml("#CF222E");
        }
    }

    public static void SetAccent(Color accent)
    {
        AccentColor = accent;
    }

    public static void Apply(Control root)
    {
        ApplyControl(root);
        int tabIndex = 0;
        AssignTabIndexes(root, ref tabIndex);
    }

    private static bool IsSkinnedControl(Control control)
    {
        return control.GetType().Namespace == "Krypton.Toolkit";
    }

    private static void ApplyControl(Control control)
    {
        if (!IsSkinnedControl(control))
        {
            switch (control)
            {
                case Form form when form.GetType().Namespace != "Krypton.Toolkit":
                    form.BackColor = CanvasColor;
                    form.ForeColor = TextColor;
                    form.Font = new Font("Segoe UI", 9.5F);
                    break;
                case TabControl or TabPage:
                    control.BackColor = CanvasColor;
                    control.ForeColor = TextColor;
                    break;
                case Panel or GroupBox:
                    control.BackColor = CanvasColor;
                    control.ForeColor = TextColor;
                    break;
                case Button button:
                    button.BackColor = PrimaryColor;
                    button.ForeColor = Color.White;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Cursor = Cursors.Hand;
                    button.Padding = new Padding(4);
                    break;
                case TextBox textBox:
                    textBox.BackColor = IsDark ? CanvasColor : SubtleColor;
                    textBox.ForeColor = TextColor;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case Label label:
                    if (label.ForeColor == SystemColors.ControlText)
                        label.ForeColor = TextColor;
                    label.BackColor = Color.Transparent;
                    break;
                case DataGridView grid:
                    grid.BackgroundColor = CanvasColor;
                    grid.ForeColor = TextColor;
                    grid.GridColor = BorderMutedColor;
                    grid.EnableHeadersVisualStyles = false;
                    grid.BorderStyle = BorderStyle.None;
                    grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = SubtleColor;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = MutedTextColor;
                    grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = SubtleColor;
                    grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = MutedTextColor;
                    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
                    grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                    grid.ColumnHeadersHeight = 38;
                    grid.DefaultCellStyle.BackColor = CanvasColor;
                    grid.DefaultCellStyle.ForeColor = TextColor;
                    grid.DefaultCellStyle.SelectionBackColor = IsDark ? ColorTranslator.FromHtml("#1F2A3A") : ColorTranslator.FromHtml("#DDF4FF");
                    grid.DefaultCellStyle.SelectionForeColor = TextColor;
                    grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
                    grid.AlternatingRowsDefaultCellStyle.BackColor = CanvasColor;
                    grid.AlternatingRowsDefaultCellStyle.ForeColor = TextColor;
                    grid.RowTemplate.Height = 30;
                    break;
                case ComboBox combo:
                    combo.BackColor = SubtleColor;
                    combo.ForeColor = TextColor;
                    break;
                case ListBox list:
                    list.BackColor = CanvasColor;
                    list.ForeColor = TextColor;
                    break;
                case StatusStrip strip:
                    strip.BackColor = CanvasColor;
                    strip.ForeColor = MutedTextColor;
                    strip.SizingGrip = false;
                    break;
                case TreeView tree:
                    tree.BackColor = CanvasColor;
                    tree.ForeColor = TextColor;
                    tree.BorderStyle = BorderStyle.None;
                    tree.ShowLines = false;
                    tree.FullRowSelect = true;
                    break;
                case ToolStrip ts:
                    ts.BackColor = SubtleColor;
                    ts.ForeColor = TextColor;
                    break;
                default:
                    control.BackColor = CanvasColor;
                    break;
            }
        }

        if (control is Form anyForm)
            SetDarkTitleBar(anyForm);

        foreach (Control child in control.Controls)
        {
            ApplyControl(child);
        }
    }

    private static void AssignTabIndexes(Control root, ref int index)
    {
        foreach (Control child in root.Controls)
        {
            if (child.CanFocus && child is not Label)
            {
                child.TabIndex = index++;
            }
            AssignTabIndexes(child, ref index);
        }
    }
}
