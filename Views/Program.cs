namespace Views;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Components.MaterialThemeModule.Initialize();
        AppServices.Initialize();
        while (true)
        {
            using var login = new Forms.LoginView();
            if (login.ShowDialog() != DialogResult.OK) return;
            Application.Run(new Forms.MainView());
            if (AppServices.Session.IsActive) return;
        }
    }
}
