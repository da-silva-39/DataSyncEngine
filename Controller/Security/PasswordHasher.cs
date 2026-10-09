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

    private const int Iterations = 100000;
    private const int KeySizeBytes = 32;

    public string Hash(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, HashAlgorithmName.SHA256, KeySizeBytes);
        return $"PBKDF2${Iterations}${Convert.ToBase64String(key)}";
    }

    public bool Verify(string password, string salt, string expectedHash)
    {
        if (expectedHash.StartsWith("PBKDF2$", StringComparison.Ordinal))
        {
            string[] parts = expectedHash.Split('$');
            if (parts.Length != 3 || !int.TryParse(parts[1], out int iterations)) return false;
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, iterations, HashAlgorithmName.SHA256, KeySizeBytes);
            return CryptographicOperations.FixedTimeEquals(key, Convert.FromBase64String(parts[2]));
        }
        return HashLegacy(password, salt) == expectedHash;
    }

    private static string HashLegacy(string password, string salt)
    {
        using var sha = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(salt + password);
        return Convert.ToBase64String(sha.ComputeHash(bytes));
    }
}
