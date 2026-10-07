using Controller.System;

namespace Views.ViewModels;

public class LoginViewModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsBusy { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public bool LoginSucceeded { get; private set; }

    public async Task<bool> LoginAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        try
        {
            bool ok = await AuthController.Instance.LoginAsync(Username, Password);
            if (!ok) ErrorMessage = "Invalid username or password.";
            LoginSucceeded = ok;
            return ok;
        }
        catch (TimeoutException)
        {
            ErrorMessage = "Connection to the database timed out. Check the server configuration.";
            return false;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
