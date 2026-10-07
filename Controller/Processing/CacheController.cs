using System.Collections.Concurrent;

namespace Controller.Processing;

public class CacheController
{
    private readonly ConcurrentDictionary<string, byte[]> _table = new();

    public void Set(string key, byte[] value) => _table[key] = value;

    public bool TryGet(string key, out byte[]? value) => _table.TryGetValue(key, out value);

    public bool Contains(string key) => _table.ContainsKey(key);

    public void Remove(string key) => _table.TryRemove(key, out _);

    public void Purge() => _table.Clear();

    public int Count => _table.Count;
}
