using Views.Components;

namespace Views.Forms.SubViews;

public partial class UserConfigView : Form
{
    public UserConfigView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
        cmbRole.Items.AddRange(new object[] { Core.Enums.UserRole.Admin, Core.Enums.UserRole.Operator });
        cmbRole.SelectedIndex = 1;
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        string username = txtUsername.Text.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            MessageBox.Show("Username and password are required.", "UserConfig", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        try
        {
            string salt = AppServices.Hasher.GenerateSalt();
            var user = new Model.Entities.UserModel
            {
                Username = username,
                Salt = salt,
                PasswordHash = AppServices.Hasher.Hash(txtPassword.Text, salt),
                Role = (Core.Enums.UserRole)cmbRole.SelectedItem!,
                IsActive = chkActive.Checked
            };
            int id = await AppServices.Users.InsertAsync(user);
            MessageBox.Show($"User saved with id {id}.", "UserConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save user: {ex.Message}", "UserConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
