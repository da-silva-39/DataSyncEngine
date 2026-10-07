using Controller.Governance;
using Controller.Processing;
using Controller.Security;
using Controller.System;
using Model.Entities;
using Model.Repositories;

namespace Views;

public static class AppServices
{
    public static ServerModel CurrentServer { get; set; } = new()
    {
        Name = "default",
        Host = "localhost",
        Port = 3306,
        Database = "datasync_db",
        User = "root",
        Password = string.Empty
    };

    public static string MasterKey { get; set; } = "DataSyncEngine-MasterKey-2026";

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

    public static void Initialize()
    {
        Auth = AuthController.Initialize(Session, Hasher, () => Users);
        HotSwap = new HotSwapController(CurrentServer, Cache);
        HotSwap.ServerChanged += server => CurrentServer = server;
    }
}
