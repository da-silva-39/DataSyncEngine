using Views.Components;

namespace Views.Forms.SubViews;

public partial class LockView : Krypton.Toolkit.KryptonForm
{
    private readonly System.Windows.Forms.Timer _cooldownTimer = new() { Interval = 1000 };
    private string? _username;
    private int _failures;
    private int _remaining;
    private bool _forceClose;

    public bool Unlocked { get; private set; }

    public LockView()
    {
        InitializeComponent();
        KryptonThemeModule.Apply(this);
        AcceptButton = btnUnlock;
        _cooldownTimer.Tick += (_, _) =>
        {
            _remaining--;
            if (_remaining <= 0)
            {
                _cooldownTimer.Stop();
                _failures = 0;
                btnUnlock.Enabled = true;
                lblError.Text = string.Empty;
            }
            else
            {
                lblError.Text = $"Too many attempts. Try again in {_remaining} second(s).";
            }
        };
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtPassword.Focus();
        try
        {
            _username = AppServices.Session.Username;
            string role = AppServices.Session.Role?.ToString() ?? "User";
            lblUser.Text = $"{_username}\n{role}";
            if (_username != null)
            {
                var user = await AppServices.Users.GetByUsernameAsync(_username);
                if (user?.Avatar is { Length: > 0 } avatar)
                {
                    using var ms = new MemoryStream(avatar);
                    using var img = Image.FromStream(ms);
                    Image? old = picAvatar.Image;
                    picAvatar.Image = new Bitmap(img);
                    old?.Dispose();
                }
            }
        }
        catch
        {
        }
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_forceClose && !Unlocked)
        {
            e.Cancel = true;
            return;
        }
        base.OnFormClosing(e);
    }

    internal void ForceDismiss()
    {
        _forceClose = true;
        Close();
    }

    private async void btnUnlock_Click(object? sender, EventArgs e)
    {
        if (!btnUnlock.Enabled) return;
        lblError.Text = string.Empty;
        if (_username == null)
        {
            Unlocked = true;
            DialogResult = DialogResult.OK;
            Close();
            return;
        }
        try
        {
            var user = await AppServices.Users.GetByUsernameAsync(_username);
            if (user != null && AppServices.Hasher.Verify(txtPassword.Text, user.Salt, user.PasswordHash))
            {
                Unlocked = true;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }
            _failures++;
            int left = 5 - _failures;
            if (_failures >= 5)
            {
                _remaining = 60;
                btnUnlock.Enabled = false;
                txtPassword.Clear();
                _cooldownTimer.Start();
                lblError.Text = $"Too many attempts. Try again in {_remaining} second(s).";
            }
            else
            {
                lblError.Text = $"Incorrect password. {left} attempt(s) left.";
                txtPassword.SelectAll();
                txtPassword.Focus();
            }
        }
        catch (Exception ex)
        {
            lblError.Text = $"Unlock failed: {ex.Message}";
        }
    }
}
