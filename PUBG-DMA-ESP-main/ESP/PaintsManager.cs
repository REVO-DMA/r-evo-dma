using SkiaSharp;

namespace pubg_dma_esp.ESP
{
    public static class PaintsManager
    {
        private static readonly float DefaultFontSize = 12f;
        private static readonly float DefaultStrokeWidth = 2f;
        private static readonly bool AntialiasingEnabled = false;

        static PaintsManager()
        {
            bool moonlightEnabled = Program.UserConfig.Moonlight;

            if (moonlightEnabled)
            {
                DefaultStrokeWidth = 1f;
                AntialiasingEnabled = true;
            }

            RedLine = new()
            {
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.Red,
                IsStroke = true,
                StrokeWidth = DefaultStrokeWidth,
            };

            WhiteLine = new()
            {
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.White,
                IsStroke = true,
                StrokeWidth = DefaultStrokeWidth,
            };

            WhiteText = new()
            {
                TextSize = DefaultFontSize,
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Center,
                Typeface = ESP_Window.FontFamilyRegular,
            };

            FPSText = new()
            {
                TextSize = 20f,
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.Green,
                TextAlign = SKTextAlign.Right,
                Typeface = ESP_Window.FontFamilyRegular,
            };

            SpectatorText = new()
            {
                TextSize = 20f,
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.Blue,
                TextAlign = SKTextAlign.Left,
                Typeface = ESP_Window.FontFamilyRegular,
            };

            HealthGood = new()
            {
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.Green,
            };

            HealthWarn = new()
            {
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.Yellow,
            };

            HealthLow = new()
            {
                IsAntialias = AntialiasingEnabled,
                Color = SKColors.Red,
            };
        }

        public static readonly SKPaint RedLine;

        public static readonly SKPaint WhiteLine;

        public static readonly SKPaint WhiteText;

        public static readonly SKPaint FPSText;

        public static readonly SKPaint SpectatorText;

        public static readonly SKPaint HealthGood;

        public static readonly SKPaint HealthWarn;

        public static readonly SKPaint HealthLow;
    }
}
