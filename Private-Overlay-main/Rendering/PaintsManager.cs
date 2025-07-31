using SkiaSharp;

namespace Private_Overlay.Rendering
{
    public static class PaintsManager
    {
        public static readonly SKPaint HealthBar = new()
        {
            IsAntialias = true,
            Color = SKColors.Black,
            Style = SKPaintStyle.StrokeAndFill,
            StrokeWidth = 2f,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round,
        };

        public static readonly SKPaint HealthGood = new()
        {
            IsAntialias = true,
            Color = SKColors.Green,
        };

        public static readonly SKPaint HealthWarn = new()
        {
            IsAntialias = true,
            Color = SKColors.Yellow,
        };

        public static readonly SKPaint HealthLow = new()
        {
            IsAntialias = true,
            Color = SKColors.Red,
        };

        public static readonly SKPaint RedBone = new()
        {
            IsAntialias = true,
            Color = SKColors.Red,
            IsStroke = true,
            StrokeWidth = 2f,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round,
        };

        public static readonly SKPaint WhiteBone = new()
        {
            IsAntialias = true,
            Color = SKColors.White,
            IsStroke = true,
            StrokeWidth = 2f,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round,
        };
    }
}
