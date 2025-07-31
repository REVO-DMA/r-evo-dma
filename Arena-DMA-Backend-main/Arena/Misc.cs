using System.Drawing;

namespace arena_dma_backend.Arena
{
    public static class Misc
    {
        public enum Bone
        {
            HumanBase = 0,
            HumanPelvis = 14,
            HumanLThigh1 = 15,
            HumanLThigh2 = 16,
            HumanLCalf = 17,
            HumanLFoot = 18,
            HumanLToe = 19,
            HumanRThigh1 = 20,
            HumanRThigh2 = 21,
            HumanRCalf = 22,
            HumanRFoot = 23,
            HumanRToe = 24,
            HumanSpine1 = 29,
            HumanSpine2 = 36,
            HumanSpine3 = 37,
            HumanLCollarbone = 89,
            HumanLUpperarm = 90,
            HumanLForearm1 = 91,
            HumanLForearm2 = 92,
            HumanLForearm3 = 93,
            HumanLPalm = 94,
            HumanRCollarbone = 110,
            HumanRUpperarm = 111,
            HumanRForearm1 = 112,
            HumanRForearm2 = 113,
            HumanRForearm3 = 114,
            HumanRPalm = 115,
            HumanNeck = 132,
            HumanHead = 133
        };

        public readonly struct UnityColor
        {
            public readonly float R;
            public readonly float G;
            public readonly float B;
            public readonly float A;

            public UnityColor(float r, float g, float b, float a = 1f)
            {
                R = r;
                G = g;
                B = b;
                A = a;
            }

            public UnityColor(byte r, byte g, byte b, byte a = 255)
            {
                R = r / 255f;
                G = g / 255f;
                B = b / 255f;
                A = a / 255f;
            }

            public UnityColor(string hex)
            {
                var color = ColorTranslator.FromHtml(hex);

                R = color.R / 255f;
                G = color.G / 255f;
                B = color.B / 255f;
                A = color.A / 255f;
            }

            public UnityColor(Color color)
            {
                R = color.R / 255f;
                G = color.G / 255f;
                B = color.B / 255f;
                A = color.A / 255f;
            }

            public readonly override string ToString() => $"({R * 255}, {G * 255}, {B * 255}, {A * 255})";

            public static int GetSize()
            {
                return Unsafe.SizeOf<UnityColor>();
            }

            public static uint GetSizeU()
            {
                return (uint)GetSize();
            }
        }
    }
}
