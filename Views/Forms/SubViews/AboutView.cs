using Views.Components;

namespace Views.Forms.SubViews;

public partial class AboutView : MaterialSkin.Controls.MaterialForm
{
    public AboutView()
    {
        InitializeComponent();
        MaterialThemeModule.Apply(this);
        lblVersion.Text = $"Version {Application.ProductVersion}";
    }

    private void btnOk_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
