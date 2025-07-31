using SkiaSharp;

namespace arena_dma_backend.ESP
{
    public static class PaintsManager
    {
        private const bool _antialiasEnabled = false;

        private const float _defaultFontSize = 12f;
        private const float _defaultStrokeWidth = 2f;

        public readonly static SKPaint RedLine = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Red,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };

        public static readonly SKPaint WhiteLine = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.White,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };

        public static readonly SKPaint WhiteText = new()
        {
            TextSize = _defaultFontSize,
            IsAntialias = _antialiasEnabled,
            Color = SKColors.White,
            TextAlign = SKTextAlign.Center,
            Typeface = ESP_Window.FontFamilyBold,
        };

        public static readonly SKPaint FPSText = new()
        {
            TextSize = 20f,
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Green,
            TextAlign = SKTextAlign.Right,
            Typeface = ESP_Window.FontFamilyRegular,
        };

        public static readonly SKPaint HealthGood = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Green,
        };

        public static readonly SKPaint HealthWarn = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Yellow,
        };

        public static readonly SKPaint HealthLow = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Red,
        };

        public static readonly SKPaint PlayerBone_Teammate = new ()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Green,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };

        public static readonly SKPaint PlayerBone_Enemy = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Red,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };
    }
}
