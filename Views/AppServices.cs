using Controller.Governance;
using Controller.Processing;
using Controller.Security;
using Controller.System;
using Model.Entities;
using Model.Repositories;

namespace Views;

public static class AppServices
{
    public static string MasterKey { get; set; } = "DataSyncEngine-MasterKey-2026";

    public static ServerModel CurrentServer { get; set; } = LoadServer();

    private static ServerModel LoadServer()
    {
        var s = AppSettings.Load();
        MasterKey = s.MasterKey;
        return new ServerModel
        {
            Name = s.ServerName,
            Host = s.ServerHost,
            Port = s.ServerPort,
            Database = s.Database,
            User = s.User,
            Password = s.Password
        };
    }

    public static SessionController Session { get; } = new();
    public static PasswordHasher Hasher { get; } = new();
    public static CacheController Cache { get; } = new();
    public static Sha256Controller Sha256 { get; } = new();
    public static Aes256Controller Aes { get; } = new();
    public static StreamCompressorController Compressor { get; } = new();
    public static HotSwapController HotSwap { get; private set; } = null!;
    public static AuthController Auth { get; private set; } = null!;

    public static UserRepository Users => new(() => CurrentServer);
    public static ServerRepository Servers => new(() => CurrentServer);
    public static FileRepository Files => new(() => CurrentServer);
    public static AuditRepository Audits => new(() => CurrentServer);

    public static AuditLoggerController AuditLogger => new(() => Audits, () => Session.Username);

    public static Controller.Sync.SyncEngineController SyncEngine { get; private set; } = null!;

    public static void Initialize()
    {
        Auth = AuthController.Initialize(Session, Hasher, () => Users);
        HotSwap = new HotSwapController(CurrentServer, Cache);
        HotSwap.ServerChanged += server => CurrentServer = server;
        SyncEngine = new Controller.Sync.SyncEngineController(Aes, Compressor, Files, () => Aes.DeriveKey(MasterKey));
    }
}
