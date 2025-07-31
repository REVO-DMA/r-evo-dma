using SkiaSharp;

namespace apex_dma_esp.ESP
{
    public static class PaintsManager
    {
        private const bool _antialiasEnabled = false;

        private const float _defaultFontSize = 12f;
        private const float _defaultStrokeWidth = 2f;

        public static SKPaint RedBox = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Red,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };

        public static SKPaint BlueBox = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Blue,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };

        public static SKPaint WhiteBox = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.White,
            IsStroke = true,
            StrokeWidth = _defaultStrokeWidth,
        };

        public static SKPaint RedText = new()
        {
            TextSize = _defaultFontSize,
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Red,
            TextAlign = SKTextAlign.Center,
            Typeface = ESP_Window.FontFamilyBold,
        };

        public static SKPaint WhiteText = new()
        {
            TextSize = _defaultFontSize,
            IsAntialias = _antialiasEnabled,
            Color = SKColors.White,
            TextAlign = SKTextAlign.Center,
            Typeface = ESP_Window.FontFamilyBold,
        };

        public static SKPaint FPSText = new()
        {
            TextSize = 20f,
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Green,
            TextAlign = SKTextAlign.Right,
            Typeface = ESP_Window.FontFamilyRegular,
        };

        public static SKPaint SpectatorText = new()
        {
            TextSize = 20f,
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Blue,
            TextAlign = SKTextAlign.Left,
            Typeface = ESP_Window.FontFamilyBold,
        };

        public static SKPaint HelpText = new()
        {
            TextSize = 32f,
            IsAntialias = false,
            Color = SKColors.White,
            TextAlign = SKTextAlign.Center,
            Typeface = ESP_Window.FontFamilyRegular,
        };

        public static SKPaint HealthGood = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Green,
        };

        public static SKPaint HealthWarn = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Yellow,
        };

        public static SKPaint HealthLow = new()
        {
            IsAntialias = _antialiasEnabled,
            Color = SKColors.Red,
        };
    }
}
