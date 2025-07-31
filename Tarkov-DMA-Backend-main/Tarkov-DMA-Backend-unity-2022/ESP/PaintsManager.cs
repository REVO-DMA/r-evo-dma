using SkiaSharp;

namespace Tarkov_DMA_Backend.ESP
{
    public static class PaintsManager
    {
        public static readonly SKTypeface FontFamily;

        public static readonly SKPaint GenericFill_White;
        public static readonly SKPaint GenericStroke_White;
        public static readonly SKPaint GenericStroke_Red;
        public static readonly SKPaint ExfilText_Open;
        public static readonly SKPaint ExfilText_Closed;
        public static readonly SKPaint ExfilText_Pending;
        public static readonly SKPaint LootItem_Circle;
        public static readonly SKPaint LootItem_Text_WithinReach;
        public static readonly SKPaint LootItem_Text_TooFar;
        public static readonly SKPaint LootItem_Text_Default;
        public static readonly SKPaint Grenade_Circle;
        public static readonly SKPaint Claymore_WarningCircle;
        public static readonly SKPaint Claymore_Text;
        public static readonly SKPaint Tripwires_Text;
        public static readonly SKPaint AmmoCount_BulletCountText;
        public static readonly SKPaint HeldWeaponInfoText_Left;
        public static readonly SKPaint HeldWeaponInfoText_Right;

        static PaintsManager()
        {
            byte[] fontBytes = File.ReadAllBytes("Fonts\\Neo Sans Std Regular.otf");
            using MemoryStream ms = new(fontBytes);
            SKTypeface face = SKTypeface.FromStream(ms);
            SKFontStyle style = new(SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            FontFamily = SKFontManager.Default.MatchTypeface(face, style);

            // Generic

            // White fill
            GenericFill_White = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
            };

            // White stroke
            GenericStroke_White = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                IsStroke = true,
                StrokeWidth = 2f,
            };

            // White stroke
            GenericStroke_Red = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Red,
                IsStroke = true,
                StrokeWidth = 2f,
            };

            // Exfils

            // Open
            ExfilText_Open = new()
            {
                TextSize = ESP_Config.ExfilFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Green,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            // Closed
            ExfilText_Closed = new()
            {
                TextSize = ESP_Config.ExfilFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Red,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            // Pending
            ExfilText_Pending = new()
            {
                TextSize = ESP_Config.ExfilFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Yellow,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            // Loot

            // LootItem circle
            LootItem_Circle = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
            };

            // LootItem text - within reach
            LootItem_Text_WithinReach = new()
            {
                TextSize = ESP_Config.LootFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Green,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            // LootItem text - too far
            LootItem_Text_TooFar = new()
            {
                TextSize = ESP_Config.LootFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Red,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            // LootItem text - default
            LootItem_Text_Default = new()
            {
                TextSize = ESP_Config.LootFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            // Grenades
            Grenade_Circle = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Red,
            };

            // Claymores
            Claymore_WarningCircle = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.Red,
            };

            Claymore_Text = new()
            {
                TextSize = ESP_Config.ClaymoreFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };

            Tripwires_Text = new()
            {
                TextSize = ESP_Config.TripwiresFontSize,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Center,
                Typeface = FontFamily,
            };


            // AmmoCount

            // AmmoCount - BulletCountText
            AmmoCount_BulletCountText = new()
            {
                TextSize = 42f,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Left,
                Typeface = FontFamily,
            };

            HeldWeaponInfoText_Left = new()
            {
                TextSize = 16f,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Left,
                Typeface = FontFamily,
            };

            HeldWeaponInfoText_Right = new()
            {
                TextSize = 16f,
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = SKColors.White,
                TextAlign = SKTextAlign.Right,
                Typeface = FontFamily,
            };
        }

        public static void UpdatePaints()
        {
            GenericFill_White.IsAntialias = ESP_Config.ESP_Antialiasing;

            GenericStroke_White.IsAntialias = ESP_Config.ESP_Antialiasing;

            GenericStroke_Red.IsAntialias = ESP_Config.ESP_Antialiasing;

            ExfilText_Open.IsAntialias = ESP_Config.ESP_Antialiasing;
            ExfilText_Open.TextSize = ESP_Config.ExfilFontSize;

            ExfilText_Closed.IsAntialias = ESP_Config.ESP_Antialiasing;
            ExfilText_Closed.TextSize = ESP_Config.ExfilFontSize;

            ExfilText_Pending.IsAntialias = ESP_Config.ESP_Antialiasing;
            ExfilText_Pending.TextSize = ESP_Config.ExfilFontSize;

            LootItem_Circle.IsAntialias = ESP_Config.ESP_Antialiasing;

            LootItem_Text_WithinReach.IsAntialias = ESP_Config.ESP_Antialiasing;
            LootItem_Text_WithinReach.TextSize = ESP_Config.LootFontSize;

            LootItem_Text_TooFar.IsAntialias = ESP_Config.ESP_Antialiasing;
            LootItem_Text_TooFar.TextSize = ESP_Config.LootFontSize;

            LootItem_Text_Default.IsAntialias = ESP_Config.ESP_Antialiasing;
            LootItem_Text_Default.TextSize = ESP_Config.LootFontSize;

            Grenade_Circle.IsAntialias = ESP_Config.ESP_Antialiasing;

            Claymore_WarningCircle.IsAntialias = ESP_Config.ESP_Antialiasing;

            Claymore_Text.IsAntialias = ESP_Config.ESP_Antialiasing;
            Claymore_Text.TextSize = ESP_Config.ClaymoreFontSize;

            Tripwires_Text.IsAntialias = ESP_Config.ESP_Antialiasing;
            Tripwires_Text.TextSize = ESP_Config.TripwiresFontSize;

            AmmoCount_BulletCountText.IsAntialias = ESP_Config.ESP_Antialiasing;

            HeldWeaponInfoText_Left.IsAntialias = ESP_Config.ESP_Antialiasing;

            HeldWeaponInfoText_Right.IsAntialias = ESP_Config.ESP_Antialiasing;
        }
    }
}
