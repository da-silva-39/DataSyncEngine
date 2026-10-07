using Controller.Security;
using Xunit;

namespace DataSyncEngine.Tests;

public class SecurityTests
{
    [Fact]
    public void PasswordHasher_Verify_RoundTrip()
    {
        var hasher = new PasswordHasher();
        string salt = hasher.GenerateSalt();
        string hash = hasher.Hash("secret123", salt);
        Assert.True(hasher.Verify("secret123", salt, hash));
        Assert.False(hasher.Verify("wrong", salt, hash));
    }

    [Fact]
    public void Aes256_EncryptDecrypt_RoundTrip()
    {
        var aes = new Aes256Controller();
        byte[] key = aes.DeriveKey("master-key");
        byte[] data = System.Text.Encoding.UTF8.GetBytes("sync payload test");
        byte[] cipher = aes.Encrypt(data, key);
        byte[] plain = aes.Decrypt(cipher, key);
        Assert.Equal(data, plain);
    }

    [Fact]
    public void Aes256_RejectsInvalidKeySize()
    {
        var aes = new Aes256Controller();
        Assert.Throws<ArgumentException>(() => aes.Encrypt(new byte[] { 1 }, new byte[16]));
    }

    [Fact]
    public void Sha256_KnownVector()
    {
        var sha = new Sha256Controller();
        Assert.Equal("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", sha.ComputeHash("abc"));
    }
}
