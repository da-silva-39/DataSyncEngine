using Timers = global::System.Timers;

namespace Controller.Sync;

public class DirectoryWatchController : IDisposable
{
    private FileSystemWatcher? _watcher;
    private readonly Timers.Timer _debounceTimer;
    private readonly HashSet<string> _pending = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _sync = new();
    private bool _disposed;

    public event Action<string>? FileChanged;

    public string? WatchedFolder { get; private set; }

    public bool IsWatching => _watcher != null;

    public int DebounceMilliseconds { get; set; } = 2000;

    public DirectoryWatchController()
    {
        _debounceTimer = new Timers.Timer { AutoReset = false };
        _debounceTimer.Elapsed += (_, _) => Flush();
    }

    public void Start(string folder)
    {
        Stop();
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder)) return;
        _debounceTimer.Interval = DebounceMilliseconds;
        _watcher = new FileSystemWatcher(folder)
        {
            IncludeSubdirectories = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
            EnableRaisingEvents = true
        };
        _watcher.Created += (_, e) => Enqueue(e.FullPath);
        _watcher.Changed += (_, e) => Enqueue(e.FullPath);
        _watcher.Renamed += (_, e) => Enqueue(e.FullPath);
        WatchedFolder = folder;
    }

    public void Stop()
    {
        lock (_sync)
        {
            _pending.Clear();
        }
        _debounceTimer.Stop();
        if (_watcher != null)
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _watcher = null;
        }
        WatchedFolder = null;
    }

    private void Enqueue(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        lock (_sync)
        {
            _pending.Add(path);
        }
        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private void Flush()
    {
        string[] batch;
        lock (_sync)
        {
            batch = _pending.ToArray();
            _pending.Clear();
        }
        foreach (string path in batch)
        {
            try
            {
                FileChanged?.Invoke(path);
            }
            catch
            {
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Stop();
        _debounceTimer.Dispose();
        GC.SuppressFinalize(this);
    }
}
