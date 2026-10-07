using Controller.Processing;
using Xunit;

namespace DataSyncEngine.Tests;

public class ProcessingTests
{
    [Fact]
    public void Compressor_RoundTrip()
    {
        var compressor = new StreamCompressorController();
        byte[] data = System.Text.Encoding.UTF8.GetBytes(new string('x', 5000));
        byte[] compressed = compressor.Compress(data);
        Assert.True(compressed.Length < data.Length);
        Assert.Equal(data, compressor.Decompress(compressed));
    }

    [Fact]
    public void Cache_SetGetPurge()
    {
        var cache = new CacheController();
        cache.Set("k", new byte[] { 1, 2, 3 });
        Assert.True(cache.TryGet("k", out byte[]? value));
        Assert.Equal(new byte[] { 1, 2, 3 }, value);
        Assert.True(cache.Contains("k"));
        cache.Purge();
        Assert.Equal(0, cache.Count);
        Assert.False(cache.TryGet("k", out _));
    }
}
