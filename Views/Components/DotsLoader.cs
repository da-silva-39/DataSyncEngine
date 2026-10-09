using System.Drawing.Drawing2D;

namespace Views.Components;

public class DotsLoader : Control
{
    private int _progress;

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Progress
    {
        get => _progress;
        set
        {
            _progress = Math.Clamp(value, 0, 100);
            Invalidate();
        }
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public Color TrailColor { get; set; } = Color.FromArgb(0x40, 0xC4, 0xFF);

    public DotsLoader()
    {
        SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        Size = new Size(400, 60);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Parent?.BackColor ?? Color.Black);

        float left = 20f;
        float right = Width - 20f;
        float y = Height / 2f;

        using (var trackPen = new Pen(Color.FromArgb(35, 35, 38), 6))
        {
            trackPen.StartCap = LineCap.Round;
            trackPen.EndCap = LineCap.Round;
            g.DrawLine(trackPen, left, y, right, y);
        }

        float headX = left + (right - left) * _progress / 100f;
        const int dots = 9;
        const float spacing = 14f;
        for (int i = dots - 1; i >= 0; i--)
        {
            float x = headX - i * spacing;
            if (x < left - 8) continue;
            float t = 1f - (float)i / dots;
            int alpha = (int)(50 + 205 * t);
            float r = 3.5f + 4.5f * t;
            using var brush = new SolidBrush(Color.FromArgb(alpha, TrailColor));
            g.FillEllipse(brush, x - r, y - r, r * 2f, r * 2f);
        }
    }
}
