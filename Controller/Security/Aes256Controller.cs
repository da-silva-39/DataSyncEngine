using System.Security.Cryptography;
using System.Text;

namespace Controller.Security;

public class Aes256Controller
{
    public byte[] Encrypt(byte[] plainData, byte[] key)
    {
        if (key.Length != 32) throw new ArgumentException("Key must be 256 bits.", nameof(key));
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
        if (key.Length != 32) throw new ArgumentException("Key must be 256 bits.", nameof(key));
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
