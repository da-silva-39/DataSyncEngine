using System.Security.Cryptography;
using System.Text;

namespace Controller.Security;

public class Sha256Controller
{
    public string ComputeHash(byte[] data)
    {
        byte[] hash = SHA256.HashData(data);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public async Task<string> ComputeHashAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var sha = SHA256.Create();
        byte[] hash = await sha.ComputeHashAsync(stream, cancellationToken);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public string ComputeHash(string text)
    {
        return ComputeHash(Encoding.UTF8.GetBytes(text));
    }
}
