using Core.Enums;
using Model.Entities;
using Views;

namespace Views.ViewModels;

public class MainViewModel
{
    public UserRole Role { get; }
    public List<FileModel> Files { get; } = new();
    public List<AuditLogModel> AuditEntries { get; } = new();
    public List<ServerModel> Servers { get; } = new();

    public List<UserModel> Users { get; } = new();

    public MainViewModel(UserRole role)
    {
        Role = role;
    }

    public async Task LoadUsersAsync()
    {
        Users.Clear();
        try
        {
            Users.AddRange(await AppServices.Users.GetAllAsync());
        }
        catch
        {
        }
    }

    public async Task ScanFolderAsync(string path, Controller.Sync.DirectoryScannerController scanner)
    {
        IReadOnlyList<FileModel> scanned = await Task.Run(() => scanner.ScanAsync(path));
        Files.Clear();
        Files.AddRange(scanned);
    }

    public async Task LoadAuditAsync()
    {
        AuditEntries.Clear();
        try
        {
            AuditEntries.AddRange(await AppServices.Audits.GetAllAsync());
        }
        catch
        {
        }
    }

    public async Task LoadServersAsync()
    {
        Servers.Clear();
        try
        {
            Servers.AddRange(await AppServices.Servers.GetAllAsync());
        }
        catch
        {
        }
    }
}
