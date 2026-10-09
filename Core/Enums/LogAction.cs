namespace Core.Enums;

public enum LogAction
{
    Login,
    Logout,
    FileUpload,
    FileDownload,
    FileDelete,
    SyncStarted,
    SyncCompleted,
    SyncFailed,
    ServerSwitch,
    MasterKeyPrompted,
    CachePurged,
    UserUpdated,
    SessionLocked,
    SessionUnlocked,
    Error
}
