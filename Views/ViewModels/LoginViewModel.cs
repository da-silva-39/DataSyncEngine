using Controller.System;

namespace Views.ViewModels;

public class LoginViewModel
{
    private const int MaxAttempts = 5;
    private static readonly TimeSpan LockDuration = TimeSpan.FromSeconds(60);
    private static readonly Dictionary<string, (int Failures, DateTime LastFailure, DateTime LockedUntil)> _attempts = new();

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsBusy { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;
    public bool LoginSucceeded { get; private set; }

    public async Task<bool> LoginAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        string key = Username.Trim().ToLowerInvariant();
        try
        {
            if (key.Length > 0 && _attempts.TryGetValue(key, out var state))
            {
                if (state.LockedUntil > DateTime.UtcNow)
                {
                    int seconds = (int)Math.Ceiling((state.LockedUntil - DateTime.UtcNow).TotalSeconds);
                    ErrorMessage = $"Too many failed attempts. Try again in {seconds} second(s).";
                    return false;
                }
                if (state.LockedUntil != DateTime.MinValue) _attempts.Remove(key);
            }

            bool ok = await AuthController.Instance.LoginAsync(Username, Password);
            if (ok)
            {
                _attempts.Remove(key);
            }
            else
            {
                _attempts.TryGetValue(key, out var current);
                int failures = DateTime.UtcNow - current.LastFailure > TimeSpan.FromMinutes(5) ? 1 : current.Failures + 1;
                var lastFailure = DateTime.UtcNow;
                var lockedUntil = failures >= MaxAttempts ? lastFailure.Add(LockDuration) : DateTime.MinValue;
                _attempts[key] = (failures, lastFailure, lockedUntil);
                ErrorMessage = failures >= MaxAttempts
                    ? $"Account locked for {LockDuration.TotalSeconds:0} seconds after {MaxAttempts} failed attempts."
                    : "Invalid username or password.";
            }
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
