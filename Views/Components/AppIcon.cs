namespace Views.Components;

public static class AppIcon
{
    public static Icon Create()
    {
        var bmp = new Bitmap(32, 32);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new SolidBrush(DarkThemeModule.AccentColor);
            g.FillEllipse(brush, 1, 1, 29, 29);
            using var font = new Font("Segoe UI", 11F, FontStyle.Bold);
            using var textBrush = new SolidBrush(Color.White);
            g.DrawString("DS", font, textBrush, 5, 7);
        }
        return Icon.FromHandle(bmp.GetHicon());
    }
}
