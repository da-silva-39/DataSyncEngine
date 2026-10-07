namespace Views;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        AppServices.Initialize();
        using var login = new Forms.LoginView();
        if (login.ShowDialog() == DialogResult.OK)
        {
            Application.Run(new Forms.MainView());
        }
    }
}
