using Controller.Processing;
using Model.Entities;

namespace Controller.System;

public class HotSwapController
{
    private readonly CacheController _cache;
    public ServerModel CurrentServer { get; private set; }
    public event Action<ServerModel>? ServerChanged;

    public HotSwapController(ServerModel initialServer, CacheController cache)
    {
        CurrentServer = initialServer;
        _cache = cache;
    }

    public void SwitchServer(ServerModel newServer)
    {
        CurrentServer = newServer;
        _cache.Purge();
        ServerChanged?.Invoke(newServer);
    }
}
