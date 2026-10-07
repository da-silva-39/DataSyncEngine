using Views.Components;
using Views.ViewModels;

namespace Views.Forms;

public partial class LoginView : Form
{
    private readonly LoginViewModel _viewModel = new();

    public LoginView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtUsername.Focus();
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
