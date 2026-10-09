using Views.Components;

namespace Views.Forms.SubViews;

public partial class ChangePasswordView : Krypton.Toolkit.KryptonForm
{
    private readonly string? _targetUsername;

    public ChangePasswordView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
    }

    public ChangePasswordView(string targetUsername) : this()
    {
        _targetUsername = targetUsername;
        Text = $"Reset Password - {targetUsername}";
        txtCurrent.Visible = false;
        lblError.Location = new System.Drawing.Point(20, 175);
        txtNew.Location = new System.Drawing.Point(20, 80);
        txtConfirm.Location = new System.Drawing.Point(20, 130);
        btnSave.Location = new System.Drawing.Point(20, 205);
        ClientSize = new System.Drawing.Size(380, 265);
        btnSave.Text = "Reset Password";
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        lblError.Text = string.Empty;
        string? username = _targetUsername ?? AppServices.Session.Username;
        if (username == null) return;
        if (txtNew.Text.Length < 6)
        {
            lblError.Text = "New password must have at least 6 characters.";
            return;
        }
        if (txtNew.Text != txtConfirm.Text)
        {
            lblError.Text = "New passwords do not match.";
            return;
        }
        try
        {
            var user = await AppServices.Users.GetByUsernameAsync(username);
            if (user == null)
            {
                lblError.Text = "User not found.";
                return;
            }
            if (_targetUsername == null && !AppServices.Hasher.Verify(txtCurrent.Text, user.Salt, user.PasswordHash))
            {
                lblError.Text = "Current password is incorrect.";
                return;
            }
            string salt = AppServices.Hasher.GenerateSalt();
            user.Salt = salt;
            user.PasswordHash = AppServices.Hasher.Hash(txtNew.Text, salt);
            await AppServices.Users.UpdateAsync(user);
            string details = _targetUsername != null
                ? $"{AppServices.Session.Username} reset password of {username}."
                : $"User {username} changed password.";
            await AppServices.AuditLogger.LogAsync(Core.Enums.LogAction.UserUpdated, details);
            MessageBox.Show("Password changed.", "Change Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            lblError.Text = $"Failed: {ex.Message}";
        }
    }
}
