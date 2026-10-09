using Views.Components;
using Views.ViewModels;

namespace Views.Forms;

public partial class LoginView : Krypton.Toolkit.KryptonForm
{
    private readonly LoginViewModel _viewModel = new();

    public LoginView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        AcceptButton = btnLogin;
        if (!DarkThemeModule.IsDesignTime)
        {
            Icon = LogoModule.GetIcon() ?? AppIcon.Create();
            Image? logo = LogoModule.GetImage(96);
            picLogo.Image = logo ?? AppIcon.Create().ToBitmap();
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtUsername.Focus();
    }

    private void chkShowPassword_CheckedChanged(object? sender, EventArgs e)
    {
        txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
    }

    private async void btnLogin_Click(object? sender, EventArgs e)
    {
        _viewModel.Username = txtUsername.Text.Trim();
        _viewModel.Password = txtPassword.Text;
        lblError.Text = string.Empty;
        btnLogin.Enabled = false;
        Cursor = Cursors.WaitCursor;
        try
        {
            bool ok = await _viewModel.LoginAsync();
            if (ok)
            {
                await AppServices.AuditLogger.LogLoginAsync(_viewModel.Username);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblError.Text = _viewModel.ErrorMessage;
            }
        }
        finally
        {
            btnLogin.Enabled = true;
            Cursor = Cursors.Default;
        }
    }
}
