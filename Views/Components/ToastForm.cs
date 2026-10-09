namespace Views.Components;

public enum ToastKind
{
    Info,
    Success,
    Warning,
    Error
}

public partial class ToastForm : Form
{
    private readonly System.Windows.Forms.Timer _timer = new();
    private int _ticks;
    private const int MaxTicks = 30;

    public ToastForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = true;
        StartPosition = FormStartPosition.Manual;
        Size = new Size(320, 64);
        _timer.Interval = 100;
        _timer.Tick += (_, _) =>
        {
            _ticks++;
            if (_ticks >= MaxTicks)
            {
                _timer.Stop();
                Close();
            }
            else if (_ticks > MaxTicks - 6)
            {
                Opacity = Math.Max(0, Opacity - 0.2);
            }
        };
    }

    public static void ShowToast(Form owner, string message, ToastKind kind = ToastKind.Info)
    {
        NotificationCenter.Push(message, kind);
        try
        {
            if (!AppServices.Settings.Notifications) return;
            var toast = new ToastForm();
            Color accent = kind switch
            {
                ToastKind.Success => DarkThemeModule.SuccessColor,
                ToastKind.Warning => DarkThemeModule.WarningColor,
                ToastKind.Error => DarkThemeModule.DangerColor,
                _ => DarkThemeModule.AccentColor,
            };
            toast.BackColor = DarkThemeModule.PanelColor;
            var bar = new Panel { BackColor = accent, Dock = DockStyle.Left, Width = 6 };
            var label = new Label
            {
                Text = message,
                ForeColor = DarkThemeModule.TextColor,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 10, 0),
                AutoEllipsis = true
            };
            toast.Controls.Add(label);
            toast.Controls.Add(bar);
            Rectangle area = Screen.GetWorkingArea(owner);
            toast.Location = new Point(area.Right - toast.Width - 16, area.Bottom - toast.Height - 16);
            toast.Opacity = 0.95;
            toast.Show(owner);
            toast._timer.Start();
        }
        catch
        {
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _timer.Dispose();
        base.Dispose(disposing);
    }
}
