using Views.Components;

namespace Views.Forms.SubViews;

public partial class ServerConfigView : Form
{
    public ServerConfigView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtPort.Text, out int port)) port = 3306;
        var server = new Model.Entities.ServerModel
        {
            Name = txtName.Text.Trim(),
            Host = txtHost.Text.Trim(),
            Port = port,
            Database = txtDatabase.Text.Trim(),
            User = txtUser.Text.Trim(),
            Password = txtPassword.Text,
            IsActive = chkActive.Checked
        };
        try
        {
            int id = await AppServices.Servers.InsertAsync(server);
            if (server.IsActive) AppServices.CurrentServer = server;
            MessageBox.Show($"Server saved with id {id}.", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save server: {ex.Message}", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
