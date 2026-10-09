using Views.Components;

namespace Views.Forms.SubViews;

public partial class UserProfileView : Krypton.Toolkit.KryptonForm
{
    public UserProfileView(Model.Entities.UserModel user, IReadOnlyList<Model.Entities.AuditLogModel> history)
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        Text = $"Profile - {user.Username}";
        lblName.Text = user.Username;
        lblRole.Text = $"Role: {user.Role}";
        lblActive.Text = user.IsActive ? "Status: Active" : "Status: Disabled";
        if (user.Avatar != null && user.Avatar.Length > 0)
        {
            try
            {
                using var ms = new MemoryStream(user.Avatar);
                picPhoto.Image = new Bitmap(ms);
            }
            catch
            {
            }
        }
        DateTime? lastSync = null;
        foreach (var entry in history)
        {
            if (entry.Action == Core.Enums.LogAction.SyncCompleted || entry.Action == Core.Enums.LogAction.FileUpload)
            {
                if (lastSync == null || entry.Timestamp > lastSync) lastSync = entry.Timestamp;
            }
            lstHistory.Items.Add($"{entry.Timestamp:yyyy-MM-dd HH:mm} - {entry.Action}: {entry.Details}");
        }
        lblLastSync.Text = lastSync == null ? "Last sync: never" : $"Last sync: {lastSync:yyyy-MM-dd HH:mm}";
    }

    private void btnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Close();
    }
}
