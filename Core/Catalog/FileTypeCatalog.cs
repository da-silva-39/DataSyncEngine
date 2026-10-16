namespace Core.Catalog;

/// <summary>
/// Lightweight file-type catalog used by the explorer and the metadata viewer to
/// classify entries by extension and report a friendly, IDE-style type name.
/// </summary>
public static class FileTypeCatalog
{
    private static readonly HashSet<string> Archives = new(StringComparer.OrdinalIgnoreCase)
        { ".zip", ".rar", ".7z", ".tar", ".gz", ".bz2", ".xz" };
    private static readonly HashSet<string> Audio = new(StringComparer.OrdinalIgnoreCase)
        { ".mp3", ".wav", ".flac", ".aac", ".ogg", ".m4a" };
    private static readonly HashSet<string> Video = new(StringComparer.OrdinalIgnoreCase)
        { ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".webm" };
    private static readonly HashSet<string> Images = new(StringComparer.OrdinalIgnoreCase)
        { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp", ".svg", ".ico", ".tif", ".tiff" };
    private static readonly HashSet<string> Documents = new(StringComparer.OrdinalIgnoreCase)
        { ".doc", ".docx", ".odt", ".rtf", ".pdf", ".txt", ".md" };
    private static readonly HashSet<string> Sheets = new(StringComparer.OrdinalIgnoreCase)
        { ".xls", ".xlsx", ".ods", ".csv" };
    private static readonly HashSet<string> Code = new(StringComparer.OrdinalIgnoreCase)
        { ".cs", ".js", ".ts", ".py", ".java", ".cpp", ".c", ".h", ".go", ".rs", ".rb", ".php",
          ".html", ".css", ".scss", ".json", ".xml", ".yml", ".yaml", ".sql", ".ps1", ".sh" };

    public static string Extension(string fileName)
    {
        string ext = Path.GetExtension(fileName);
        return string.IsNullOrEmpty(ext) ? "(none)" : ext.ToLowerInvariant();
    }

    public static string Category(string fileName) => CategoryFromExtension(Path.GetExtension(fileName));

    public static string CategoryFromExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension)) return "File";
        if (Archives.Contains(extension)) return "Archive";
        if (Audio.Contains(extension)) return "Audio";
        if (Video.Contains(extension)) return "Video";
        if (Images.Contains(extension)) return "Image";
        if (Documents.Contains(extension)) return "Document";
        if (Sheets.Contains(extension)) return "Spreadsheet";
        if (Code.Contains(extension)) return "Source code";
        return "File";
    }

    /// <summary>Compact type label, e.g. "PNG Image", "C# Source", "Folder".</summary>
    public static string TypeName(string fileName, bool isDirectory = false)
    {
        if (isDirectory) return "Folder";
        string ext = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(ext)) return "File";
        string category = CategoryFromExtension(ext);
        string shortExt = ext.TrimStart('.').ToUpperInvariant();
        return $"{shortExt} {category}";
    }

    public static string HumanSize(long bytes)
    {
        if (bytes < 0) return "-";
        string[] units = { "B", "KB", "MB", "GB", "TB" };
        double value = bytes;
        int unit = 0;
        while (value >= 1024 && unit < units.Length - 1)
        {
            value /= 1024;
            unit++;
        }
        return unit == 0 ? $"{bytes} B" : $"{value:0.##} {units[unit]}";
    }
}
