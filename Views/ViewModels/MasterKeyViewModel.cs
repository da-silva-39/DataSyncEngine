namespace Views.ViewModels;

public class MasterKeyViewModel
{
    public string EnteredKey { get; set; } = string.Empty;
    public bool IsAuthorized { get; private set; }
    public string ErrorMessage { get; private set; } = string.Empty;

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(EnteredKey))
        {
            ErrorMessage = "Master key is required.";
            IsAuthorized = false;
            return false;
        }
        if (EnteredKey == AppServices.MasterKey)
        {
            IsAuthorized = true;
            ErrorMessage = string.Empty;
            return true;
        }
        ErrorMessage = "Invalid master key.";
        IsAuthorized = false;
        return false;
    }
}
