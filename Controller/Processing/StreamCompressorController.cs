using System.IO.Compression;

namespace Controller.Processing;

public class StreamCompressorController
{
    public byte[] Compress(byte[] data)
    {
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal, true))
        {
            gzip.Write(data, 0, data.Length);
        }
        return output.ToArray();
    }

    public byte[] Decompress(byte[] compressed)
    {
        using var input = new MemoryStream(compressed);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return output.ToArray();
    }

    public async Task<byte[]> CompressAsync(Stream source, CancellationToken cancellationToken = default)
    {
        using var output = new MemoryStream();
        await using (var gzip = new GZipStream(output, CompressionLevel.Optimal, true))
        {
            await source.CopyToAsync(gzip, cancellationToken);
        }
        return output.ToArray();
    }
}
