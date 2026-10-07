using System.Runtime.InteropServices;

namespace Views.Components;

public static class DarkThemeModule
{
    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

    public static void SetDarkTitleBar(Form form)
    {
        try
        {
            int useDark = 1;
            DwmSetWindowAttribute(form.Handle, 20, ref useDark, sizeof(int));
        }
        catch
        {
        }
    }


    public static readonly Color BackgroundColor = ColorTranslator.FromHtml("#1E1E1E");
    public static readonly Color PanelColor = ColorTranslator.FromHtml("#2D2D30");
    public static readonly Color TextColor = Color.White;
    public static readonly Color AccentColor = ColorTranslator.FromHtml("#007ACC");

    public static void Apply(Control root)
    {
        ApplyControl(root);
        int tabIndex = 0;
        AssignTabIndexes(root, ref tabIndex);
    }

    private static void ApplyControl(Control control)
    {
        switch (control)
        {
            case Form form:
                form.BackColor = BackgroundColor;
                form.ForeColor = TextColor;
                SetDarkTitleBar(form);
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
            default:
                if (control.BackColor == SystemColors.Control)
                    control.BackColor = BackgroundColor;
                break;
        }

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
