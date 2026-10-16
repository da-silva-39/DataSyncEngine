using System.Security.Cryptography;
using System.Text;

namespace Controller.Security;

/// <summary>
/// Symmetric cryptography for the DataSyncEngine vault.
///
/// New payloads use AES-256 in GCM mode (authenticated encryption) with a key
/// derived from the master passphrase through PBKDF2-HMAC-SHA256. Every payload
/// is self-contained and begins with a versioned header, so a file sealed by the
/// application can only be opened by the application (or anyone holding the
/// master passphrase). The legacy AES-CBC format is still accepted when reading,
/// which keeps previously stored blobs usable.
/// </summary>
public class Aes256Controller
{
    public const int KeySize = 32;          // 256-bit key
    public const int SaltSize = 16;         // 128-bit salt
    public const int NonceSize = 12;        // 96-bit GCM nonce
    public const int TagSize = 16;          // 128-bit authentication tag
    public const int Iterations = 200_000;  // PBKDF2 work factor
    public const byte FormatVersion = 1;

    // "DSC1" magic marks a DataSyncEngine vault payload.
    private static readonly byte[] Magic = { (byte)'D', (byte)'S', (byte)'C', (byte)'1' };

    public static int HeaderSize => Magic.Length + 1 + SaltSize + NonceSize + TagSize;

    // ---- Authenticated vault format (AES-256-GCM) ---------------------------

    public byte[] Seal(byte[] plainData, string passphrase)
    {
        ArgumentNullException.ThrowIfNull(plainData);
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] key = DeriveKeyPbkdf2(passphrase, salt, Iterations);

        byte[] cipher = new byte[plainData.Length];
        byte[] tag = new byte[TagSize];
        byte[] header = BuildHeader(salt, nonce);

        using (var gcm = new AesGcm(key, TagSize))
        {
            gcm.Encrypt(nonce, plainData, cipher, tag, header);
        }

        byte[] result = new byte[header.Length + TagSize + cipher.Length];
        Buffer.BlockCopy(header, 0, result, 0, header.Length);
        Buffer.BlockCopy(tag, 0, result, header.Length, TagSize);
        Buffer.BlockCopy(cipher, 0, result, header.Length + TagSize, cipher.Length);
        CryptographicOperations.ZeroMemory(key);
        return result;
    }

    public byte[] OpenVault(byte[] payload, string passphrase)
    {
        if (!IsVaultFormat(payload))
            throw new CryptographicException("Payload is not a DataSync vault container.");

        Span<byte> span = payload;
        byte[] salt = span.Slice(Magic.Length + 1, SaltSize).ToArray();
        byte[] nonce = span.Slice(Magic.Length + 1 + SaltSize, NonceSize).ToArray();
        byte[] header = span.Slice(0, Magic.Length + 1 + SaltSize).ToArray();
        byte[] tag = span.Slice(Magic.Length + 1 + SaltSize + NonceSize, TagSize).ToArray();
        byte[] cipher = span.Slice(HeaderSize).ToArray();

        byte[] key = DeriveKeyPbkdf2(passphrase, salt, Iterations);
        byte[] plain = new byte[cipher.Length];
        using (var gcm = new AesGcm(key, TagSize))
        {
            gcm.Decrypt(nonce, cipher, tag, plain, header);
        }
        CryptographicOperations.ZeroMemory(key);
        return plain;
    }

    private static byte[] BuildHeader(byte[] salt, byte[] nonce)
    {
        byte[] header = new byte[Magic.Length + 1 + SaltSize + NonceSize];
        Buffer.BlockCopy(Magic, 0, header, 0, Magic.Length);
        header[Magic.Length] = FormatVersion;
        Buffer.BlockCopy(salt, 0, header, Magic.Length + 1, SaltSize);
        Buffer.BlockCopy(nonce, 0, header, Magic.Length + 1 + SaltSize, NonceSize);
        return header;
    }

    public static bool IsVaultFormat(byte[]? payload)
    {
        if (payload == null || payload.Length < HeaderSize) return false;
        for (int i = 0; i < Magic.Length; i++)
            if (payload[i] != Magic[i]) return false;
        return true;
    }

    public VaultHeader? ReadHeader(byte[]? payload)
    {
        if (!IsVaultFormat(payload)) return null;
        byte[] salt = new byte[SaltSize];
        byte[] nonce = new byte[NonceSize];
        Buffer.BlockCopy(payload!, Magic.Length + 1, salt, 0, SaltSize);
        Buffer.BlockCopy(payload!, Magic.Length + 1 + SaltSize, nonce, 0, NonceSize);
        return new VaultHeader
        {
            Algorithm = "AES-256-GCM",
            KeyDerivation = $"PBKDF2-HMAC-SHA256 ({Iterations:N0} iterations)",
            SaltHex = Convert.ToHexString(salt),
            NonceHex = Convert.ToHexString(nonce),
            Version = payload![Magic.Length],
            PayloadBytes = payload.Length
        };
    }

    public byte[] DeriveKeyPbkdf2(string passphrase, byte[] salt, int iterations = Iterations)
    {
        return Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(passphrase ?? string.Empty),
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            KeySize);
    }

    // ---- Legacy AES-256-CBC (read/write, kept for older blobs) ---------------

    public byte[] Encrypt(byte[] plainData, byte[] key)
    {
        if (key.Length != KeySize) throw new ArgumentException("Key must be 256 bits.", nameof(key));
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = key;
        aes.GenerateIV();
        using var encryptor = aes.CreateEncryptor();
        byte[] cipher = encryptor.TransformFinalBlock(plainData, 0, plainData.Length);
        byte[] result = new byte[aes.IV.Length + cipher.Length];
        Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
        Buffer.BlockCopy(cipher, 0, result, aes.IV.Length, cipher.Length);
        return result;
    }

    public byte[] Decrypt(byte[] cipherData, byte[] key)
    {
        if (key.Length != KeySize) throw new ArgumentException("Key must be 256 bits.", nameof(key));
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = key;
        byte[] iv = new byte[aes.BlockSize / 8];
        Buffer.BlockCopy(cipherData, 0, iv, 0, iv.Length);
        aes.IV = iv;
        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(cipherData, iv.Length, cipherData.Length - iv.Length);
    }

    public byte[] DeriveKey(string masterKey)
    {
        using var sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(masterKey));
    }
}

public sealed class VaultHeader
{
    public string Algorithm { get; init; } = "AES-256-GCM";
    public string KeyDerivation { get; init; } = string.Empty;
    public string SaltHex { get; init; } = string.Empty;
    public string NonceHex { get; init; } = string.Empty;
    public byte Version { get; init; }
    public int PayloadBytes { get; init; }
}
