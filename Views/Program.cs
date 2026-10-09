namespace Views;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Components.KryptonThemeModule.Initialize();
        AppServices.Initialize();
        using (var splash = new Forms.SplashView())
        {
            if (splash.ShowDialog() != DialogResult.OK) return;
        }
        while (true)
        {
            using var login = new Forms.LoginView();
            if (login.ShowDialog() != DialogResult.OK) return;
            Application.Run(new Forms.MainView());
            if (AppServices.Session.IsActive) return;
        }
    }
}
