using Views.Components;

namespace Views.Forms.SubViews;

public partial class UserConfigView : Krypton.Toolkit.KryptonForm
{
    private byte[]? _avatarBytes;

    public UserConfigView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        cmbRole.Items.AddRange(new object[] { Core.Enums.UserRole.Admin, Core.Enums.UserRole.Operator });
        cmbRole.SelectedIndex = 1;
    }

    private void btnPhoto_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Choose profile photo",
            Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            using var src = Image.FromFile(dialog.FileName);
            int max = 256;
            int w = src.Width, h = src.Height;
            if (w > max || h > max)
            {
                float scale = Math.Min((float)max / w, (float)max / h);
                w = (int)(w * scale);
                h = (int)(h * scale);
            }
            using var bmp = new Bitmap(src, new Size(w, h));
            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            _avatarBytes = ms.ToArray();
            picAvatar.Image?.Dispose();
            picAvatar.Image = new Bitmap(src, new Size(170, 170));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not load photo: {ex.Message}", "UserConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
                IsActive = chkActive.Checked,
                Avatar = _avatarBytes
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
