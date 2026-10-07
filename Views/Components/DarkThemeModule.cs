namespace Views.Components;

public static class DarkThemeModule
{
    public static readonly Color Background = ColorTranslator.FromHtml("#1E1E1E");
    public static readonly Color Panel = ColorTranslator.FromHtml("#2D2D30");
    public static readonly Color Text = Color.White;
    public static readonly Color Accent = ColorTranslator.FromHtml("#007ACC");

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
                form.BackColor = Background;
                form.ForeColor = Text;
                break;
            case Panel or GroupBox or TabControl or TabPage:
                control.BackColor = Panel;
                control.ForeColor = Text;
                break;
            case Button button:
                button.BackColor = Accent;
                button.ForeColor = Text;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = Accent;
                break;
            case TextBox textBox:
                textBox.BackColor = Panel;
                textBox.ForeColor = Text;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                break;
            case Label label:
                label.ForeColor = Text;
                label.BackColor = Color.Transparent;
                break;
            case DataGridView grid:
                grid.BackgroundColor = Background;
                grid.ForeColor = Text;
                grid.GridColor = Panel;
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Panel;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Text;
                grid.DefaultCellStyle.BackColor = Background;
                grid.DefaultCellStyle.ForeColor = Text;
                grid.DefaultCellStyle.SelectionBackColor = Accent;
                break;
            case ComboBox combo:
                combo.BackColor = Panel;
                combo.ForeColor = Text;
                break;
            default:
                if (control.BackColor == SystemColors.Control)
                    control.BackColor = Background;
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
