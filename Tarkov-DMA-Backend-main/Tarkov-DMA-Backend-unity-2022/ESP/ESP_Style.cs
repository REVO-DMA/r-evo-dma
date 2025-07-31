using SkiaSharp;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov;

namespace Tarkov_DMA_Backend.ESP
{
    public static class ESP_Style
    {
        public static ConcurrentDictionary<PlayerType, PlayerTypeStyle> Styles = new();
        
        private static JSON.ESPStyleSync LastSync = null;

        public readonly struct PlayerTypeStyle(
            SKPaint boxVisible,
            SKPaint boxInvisible,
            SKPaint boneVisible,
            SKPaint boneInvisible,
            SKPaint textCenter,
            SKPaint textLeft,
            SKPaint textRight)
        {
            public readonly SKPaint BoxVisible = boxVisible;
            public readonly SKPaint BoxInvisible = boxInvisible;

            public readonly SKPaint BoneVisible = boneVisible;
            public readonly SKPaint BoneInvisible = boneInvisible;

            public readonly SKPaint TextCenter = textCenter;
            public readonly SKPaint TextLeft = textLeft;
            public readonly SKPaint TextRight = textRight;
        }

        public static void SyncStyles(JSON.ESPStyleSync config)
        {
            // Use the last style sync payload if config is null
            if (config == null)
            {
                if (LastSync == null)
                    return;

                config = LastSync;
            }
            else
                LastSync = config;

            foreach (var style in config.Styles)
            {
                PlayerType playerType = (PlayerType)style.ID;

                if (!Styles.ContainsKey(playerType))
                {
                    var newStyle = CreateNew(style);
                    Styles.TryAdd(playerType, newStyle);
                }
                else if (Styles.TryGetValue(playerType, out var existingStyle))
                {
                    UpdateExisting(style, existingStyle);
                }
            }
        }

        private static PlayerTypeStyle CreateNew(JSON.PlayerTypeEspStyle style)
        {
            PlayerType playerType = (PlayerType)style.ID;

            bool isHuman = false;
            if (playerType is PlayerType.Teammate or
                PlayerType.EnemyPMC or
                PlayerType.PlayerScav)
            {
                isHuman = true;
            }

            int boxThickness;
            int boneThickness;
            float textSize;
            if (isHuman)
            {
                boxThickness = ESP_Config.PlayerBoxThickness;
                boneThickness = ESP_Config.PlayerBoneThickness;
                textSize = ESP_Config.PlayerFontSize;
            }
            else
            {
                boxThickness = ESP_Config.AIBoxThickness;
                boneThickness = ESP_Config.AIBoneThickness;
                textSize = ESP_Config.AIFontSize;
            }

            SKColor boxColorVisible = SKColor.Parse(style.BoxColorVisible);
            SKPaint boxVisible = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = boxColorVisible,
                IsStroke = true,
                StrokeWidth = boxThickness,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
            };

            SKColor boxColorInvisible = SKColor.Parse(style.BoxColorInvisible);
            SKPaint boxInvisible = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = boxColorInvisible,
                IsStroke = true,
                StrokeWidth = boxThickness,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
            };

            SKColor boneColorVisible = SKColor.Parse(style.BoneColorVisible);
            SKPaint boneVisible = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = boneColorVisible,
                IsStroke = true,
                StrokeWidth = boneThickness,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
            };

            SKColor boneColorInvisible = SKColor.Parse(style.BoneColorInvisible);
            SKPaint boneInvisible = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                Color = boneColorInvisible,
                IsStroke = true,
                StrokeWidth = boneThickness,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
            };

            SKColor fontColor = SKColor.Parse(style.FontColor);
            SKPaint textCenter = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                TextSize = textSize,
                Color = fontColor,
                TextAlign = SKTextAlign.Center,
                Typeface = PaintsManager.FontFamily,
            };
            SKPaint textLeft = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                TextSize = textSize,
                Color = fontColor,
                TextAlign = SKTextAlign.Left,
                Typeface = PaintsManager.FontFamily,
            };
            SKPaint textRight = new()
            {
                IsAntialias = ESP_Config.ESP_Antialiasing,
                TextSize = textSize,
                Color = fontColor,
                TextAlign = SKTextAlign.Right,
                Typeface = PaintsManager.FontFamily,
            };

            PlayerTypeStyle espStyle = new(boxVisible, boxInvisible, boneVisible, boneInvisible, textCenter, textLeft, textRight);
            return espStyle;
        }

        private static void UpdateExisting(JSON.PlayerTypeEspStyle style, PlayerTypeStyle existingStyle)
        {
            PlayerType playerType = (PlayerType)style.ID;

            bool isHuman = false;
            if (playerType is PlayerType.Teammate or
                PlayerType.EnemyPMC or
                PlayerType.PlayerScav)
            {
                isHuman = true;
            }

            int boxThickness;
            int boneThickness;
            float textSize;
            if (isHuman)
            {
                boxThickness = ESP_Config.PlayerBoxThickness;
                boneThickness = ESP_Config.PlayerBoneThickness;
                textSize = ESP_Config.PlayerFontSize;
            }
            else
            {
                boxThickness = ESP_Config.AIBoxThickness;
                boneThickness = ESP_Config.AIBoneThickness;
                textSize = ESP_Config.AIFontSize;
            }

            SKColor boxColorVisible = SKColor.Parse(style.BoxColorVisible);
            existingStyle.BoxVisible.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.BoxVisible.Color = boxColorVisible;
            existingStyle.BoxVisible.StrokeWidth = boxThickness;

            SKColor boxColorInvisible = SKColor.Parse(style.BoxColorInvisible);
            existingStyle.BoxInvisible.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.BoxInvisible.Color = boxColorInvisible;
            existingStyle.BoxInvisible.StrokeWidth = boxThickness;

            SKColor boneColorVisible = SKColor.Parse(style.BoneColorVisible);
            existingStyle.BoneVisible.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.BoneVisible.Color = boneColorVisible;
            existingStyle.BoneVisible.StrokeWidth = boneThickness;

            SKColor boneColorInvisible = SKColor.Parse(style.BoneColorInvisible);
            existingStyle.BoneInvisible.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.BoneInvisible.Color = boneColorInvisible;
            existingStyle.BoneInvisible.StrokeWidth = boneThickness;

            SKColor fontColor = SKColor.Parse(style.FontColor);
            existingStyle.TextCenter.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.TextCenter.TextSize = textSize;
            existingStyle.TextCenter.Color = fontColor;

            existingStyle.TextLeft.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.TextLeft.TextSize = textSize;
            existingStyle.TextLeft.Color = fontColor;

            existingStyle.TextRight.IsAntialias = ESP_Config.ESP_Antialiasing;
            existingStyle.TextRight.TextSize = textSize;
            existingStyle.TextRight.Color = fontColor;
        }
    }
}
