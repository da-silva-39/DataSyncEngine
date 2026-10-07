using System.Security.Cryptography;
using System.Text;

namespace Controller.Security;

public class PasswordHasher
{
    public string GenerateSalt()
    {
        byte[] salt = RandomNumberGenerator.GetBytes(Core.Config.AppConstants.SaltSizeBytes);
        return Convert.ToBase64String(salt);
    }

    public string Hash(string password, string salt)
    {
        using var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(salt + password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }

    public bool Verify(string password, string salt, string expectedHash)
    {
        return Hash(password, salt) == expectedHash;
    }
}
