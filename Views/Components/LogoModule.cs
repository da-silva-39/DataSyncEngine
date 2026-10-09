namespace Views.Components;

public static class LogoModule
{
    private static readonly string[] FileNames = { "logo.png", "logo.jpg", "logo.jpeg", "logo.ico", "logo.bmp" };

    public static string? FindLogoPath()
    {
        var dirs = new List<string>();
        try
        {
            string dir = AppContext.BaseDirectory;
            for (int i = 0; i < 5 && dir != null; i++)
            {
                dirs.Add(dir);
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar))!;
            }
        }
        catch
        {
        }
        foreach (string d in dirs)
        {
            foreach (string name in FileNames)
            {
                try
                {
                    string candidate = Path.Combine(d, name);
                    if (File.Exists(candidate)) return candidate;
                }
                catch
                {
                }
            }
        }
        return null;
    }

    public static Image? GetImage(int size = 64)
    {
        string? path = FindLogoPath();
        if (path == null) return null;
        try
        {
            using var src = Image.FromFile(path);
            return new Bitmap(src, new Size(size, size));
        }
        catch
        {
            return null;
        }
    }

    public static Icon? GetIcon()
    {
        string? path = FindLogoPath();
        if (path == null) return null;
        try
        {
            if (path.EndsWith(".ico", StringComparison.OrdinalIgnoreCase))
                return new Icon(path);
            using var src = Image.FromFile(path);
            using var bmp = new Bitmap(src, new Size(32, 32));
            return Icon.FromHandle(bmp.GetHicon());
        }
        catch
        {
            return null;
        }
    }
}
