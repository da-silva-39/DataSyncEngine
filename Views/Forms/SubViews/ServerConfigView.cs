using Views.Components;

namespace Views.Forms.SubViews;

public partial class ServerConfigView : Form
{
    private readonly Model.Entities.ServerModel? _editing;

    public ServerConfigView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
    }

    public ServerConfigView(Model.Entities.ServerModel server) : this()
    {
        _editing = server;
        txtName.Text = server.Name;
        txtHost.Text = server.Host;
        txtPort.Text = server.Port.ToString();
        txtDatabase.Text = server.Database;
        txtUser.Text = server.User;
        txtPassword.Text = server.Password;
        chkActive.Checked = server.IsActive;
        Text = "Edit Server";
    }

    private async void btnTest_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtPort.Text, out int port)) port = 3306;
        var server = new Model.Entities.ServerModel
        {
            Host = txtHost.Text.Trim(),
            Port = port,
            Database = txtDatabase.Text.Trim(),
            User = txtUser.Text.Trim(),
            Password = txtPassword.Text
        };
        Cursor = Cursors.WaitCursor;
        try
        {
            await using var conn = await Model.DataAccess.DbConnectionFactory.OpenConnectionAsync(server);
            MessageBox.Show("Connection OK.", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection failed: {ex.Message}", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async void btnSave_Click(object? sender, EventArgs e)
    {
        if (!int.TryParse(txtPort.Text, out int port)) port = 3306;
        var server = _editing ?? new Model.Entities.ServerModel();
        server.Name = txtName.Text.Trim();
        server.Host = txtHost.Text.Trim();
        server.Port = port;
        server.Database = txtDatabase.Text.Trim();
        server.User = txtUser.Text.Trim();
        server.Password = txtPassword.Text;
        server.IsActive = chkActive.Checked;
        try
        {
            if (server.Id > 0)
            {
                await AppServices.Servers.UpdateAsync(server);
                MessageBox.Show("Server updated.", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                int id = await AppServices.Servers.InsertAsync(server);
                MessageBox.Show($"Server saved with id {id}.", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (server.IsActive) AppServices.CurrentServer = server;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save server: {ex.Message}", "ServerConfig", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
