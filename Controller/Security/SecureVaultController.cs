using Controller.Processing;

namespace Controller.Security;

/// <summary>
/// The DataSyncEngine vault. Everything that leaves the application towards the
/// server passes through <see cref="Seal"/>: the content is compressed and then
/// sealed with authenticated AES-256-GCM, producing a self-describing container
/// that only this application (holding the master passphrase) can open.
///
/// <see cref="Open"/> transparently accepts both the new GCM container and the
/// legacy CBC+compression layout, so blobs created by earlier versions remain
/// readable.
/// </summary>
public class SecureVaultController
{
    private readonly Aes256Controller _aes;
    private readonly StreamCompressorController _compressor;

    public SecureVaultController(Aes256Controller aes, StreamCompressorController compressor)
    {
        _aes = aes;
        _compressor = compressor;
    }

    /// <summary>Compression level used when sealing payloads (delegates to the compressor).</summary>
    public global::System.IO.Compression.CompressionLevel CompressionLevel
    {
        get => _compressor.Level;
        set => _compressor.Level = value;
    }

    public byte[] Seal(byte[] plaintext, string passphrase)
    {
        byte[] compressed = _compressor.Compress(plaintext);
        return _aes.Seal(compressed, passphrase);
    }

    public byte[] Open(byte[] stored, string passphrase)
    {
        if (stored == null || stored.Length == 0)
            throw new InvalidOperationException("Empty vault payload.");

        if (Aes256Controller.IsVaultFormat(stored))
        {
            byte[] compressed = _aes.OpenVault(stored, passphrase);
            return _compressor.Decompress(compressed);
        }

        // Legacy layout: [gzip(IV + AES-CBC ciphertext)]
        byte[] legacyBlob = _compressor.Decompress(stored);
        return _aes.Decrypt(legacyBlob, _aes.DeriveKey(passphrase));
    }

    public VaultDescription Describe(byte[]? stored)
    {
        if (stored == null || stored.Length == 0)
            return new VaultDescription { Format = "empty" };

        VaultHeader? header = _aes.ReadHeader(stored);
        if (header != null)
        {
            return new VaultDescription
            {
                Format = "AES-256-GCM vault (v" + header.Version + ")",
                Algorithm = header.Algorithm,
                KeyDerivation = header.KeyDerivation,
                SaltHex = header.SaltHex,
                NonceHex = header.NonceHex,
                PayloadBytes = header.PayloadBytes,
                IsAuthenticated = true
            };
        }

        return new VaultDescription
        {
            Format = "Legacy AES-256-CBC",
            Algorithm = "AES-256-CBC",
            KeyDerivation = "SHA-256 (no salt)",
            PayloadBytes = stored.Length,
            IsAuthenticated = false
        };
    }
}

public sealed class VaultDescription
{
    public string Format { get; init; } = "unknown";
    public string Algorithm { get; init; } = string.Empty;
    public string KeyDerivation { get; init; } = string.Empty;
    public string SaltHex { get; init; } = string.Empty;
    public string NonceHex { get; init; } = string.Empty;
    public int PayloadBytes { get; init; }
    public bool IsAuthenticated { get; init; }
}
