namespace cs2_dma_esp.CS2
{
    public static class Input
    {
        public static ulong PreviousXY { get; set; }
        public static ulong Sensitivity { get; set; }

        public readonly struct MousePosition
        {
            public readonly float X;
            public readonly float Y;

            public MousePosition(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        public static void MouseMove(MousePosition pos)
        {
            Memory.WriteValue(PreviousXY, pos);
        }

        public static float GetSensitivity()
        {
            return Memory.ReadValue<float>(Sensitivity + 0x40, false);
        }
    }
}
