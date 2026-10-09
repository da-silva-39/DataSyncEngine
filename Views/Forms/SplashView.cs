using Views.Components;

namespace Views.Forms;

public partial class SplashView : Form
{
    private int _ticks;
    private const int TotalTicks = 140;

    public SplashView()
    {
        InitializeComponent();
        DarkThemeModule.Apply(this);
        if (!DarkThemeModule.IsDesignTime)
        {
            Image? logo = LogoModule.GetImage(128);
            picLogo.Image = logo ?? AppIcon.Create().ToBitmap();
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        loadTimer.Start();
    }

    private void loadTimer_Tick(object? sender, EventArgs e)
    {
        _ticks++;
        int percent = Math.Min(100, _ticks * 100 / TotalTicks);
        loader.Progress = percent;
        lblPercent.Text = percent + "%";
        if (percent >= 100)
        {
            loadTimer.Stop();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
