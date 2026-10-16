using Views.Components;

namespace Views.Forms.SubViews;

public partial class ShortcutsView : Krypton.Toolkit.KryptonForm
{
    public ShortcutsView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
