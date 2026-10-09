using System.Runtime.InteropServices;

namespace Views.Components;

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


    public static Color BackgroundColor { get; private set; } = ColorTranslator.FromHtml("#000000");
    public static Color PanelColor { get; private set; } = ColorTranslator.FromHtml("#161616");
    public static Color TextColor { get; private set; } = Color.White;
    public static Color AccentColor { get; private set; } = ColorTranslator.FromHtml("#005A9E");

    public static readonly Color SuccessColor = ColorTranslator.FromHtml("#4CAF50");
    public static readonly Color WarningColor = ColorTranslator.FromHtml("#FFC107");
    public static readonly Color ModifiedColor = ColorTranslator.FromHtml("#FF9800");
    public static readonly Color DangerColor = ColorTranslator.FromHtml("#F44336");

    public static bool IsDark { get; private set; } = true;

    public static bool IsDesignTime => System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime;

    public static void SetTheme(bool dark)
    {
        IsDark = dark;
        BackgroundColor = dark ? ColorTranslator.FromHtml("#000000") : ColorTranslator.FromHtml("#F3F3F3");
        PanelColor = dark ? ColorTranslator.FromHtml("#161616") : Color.White;
        TextColor = dark ? Color.White : ColorTranslator.FromHtml("#1E1E1E");
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

    private static bool IsMaterialControl(Control control)
    {
        return control.GetType().Namespace == "MaterialSkin.Controls";
    }

    private static void ApplyControl(Control control)
    {
        if (!IsMaterialControl(control))
        {
            switch (control)
            {
                case Form form when form.GetType().Namespace != "MaterialSkin.Controls":
                    form.BackColor = BackgroundColor;
                    form.ForeColor = TextColor;
                    form.Font = new Font("Segoe UI", 9.5F);
                    break;
                case Panel or GroupBox or TabControl or TabPage:
                    control.BackColor = PanelColor;
                    control.ForeColor = TextColor;
                    break;
                case Button button:
                    button.BackColor = AccentColor;
                    button.ForeColor = TextColor;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.Cursor = Cursors.Hand;
                    button.Padding = new Padding(4);
                    break;
                case TextBox textBox:
                    textBox.BackColor = PanelColor;
                    textBox.ForeColor = TextColor;
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case Label label:
                    if (label.ForeColor == SystemColors.ControlText)
                        label.ForeColor = TextColor;
                    label.BackColor = Color.Transparent;
                    break;
                case DataGridView grid:
                    grid.BackgroundColor = BackgroundColor;
                    grid.ForeColor = TextColor;
                    grid.GridColor = PanelColor;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = PanelColor;
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = TextColor;
                    grid.DefaultCellStyle.BackColor = BackgroundColor;
                    grid.DefaultCellStyle.ForeColor = TextColor;
                    grid.DefaultCellStyle.SelectionBackColor = AccentColor;
                    grid.AlternatingRowsDefaultCellStyle.BackColor = PanelColor;
                    grid.AlternatingRowsDefaultCellStyle.ForeColor = TextColor;
                    grid.RowTemplate.Height = 28;
                    grid.BorderStyle = BorderStyle.None;
                    break;
                case ComboBox combo:
                    combo.BackColor = PanelColor;
                    combo.ForeColor = TextColor;
                    break;
                case StatusStrip strip:
                    strip.BackColor = PanelColor;
                    strip.ForeColor = TextColor;
                    break;
                case TreeView tree:
                    tree.BackColor = PanelColor;
                    tree.ForeColor = TextColor;
                    tree.BorderStyle = BorderStyle.None;
                    break;
                case ToolStrip ts:
                    ts.BackColor = PanelColor;
                    ts.ForeColor = TextColor;
                    break;
                default:
                    control.BackColor = BackgroundColor;
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
