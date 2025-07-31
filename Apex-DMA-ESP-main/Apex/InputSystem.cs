namespace apex_dma_esp.Apex
{
    public static class InputSystem
    {
        public readonly struct Keys
        {
            public const uint NONE = 0;

            public const uint A = 11;
            public const uint B = 12;
            public const uint C = 13;
            public const uint D = 14;
            public const uint E = 15;
            public const uint F = 16;
            public const uint G = 17;
            public const uint H = 18;
            public const uint I = 19;
            public const uint J = 20;
            public const uint K = 21;
            public const uint L = 22;
            public const uint M = 23;
            public const uint N = 24;
            public const uint O = 25;
            public const uint P = 26;
            public const uint Q = 27;
            public const uint R = 28;
            public const uint S = 29;
            public const uint T = 30;
            public const uint U = 31;
            public const uint V = 32;
            public const uint W = 33;
            public const uint X = 34;
            public const uint Y = 35;
            public const uint Z = 36;

            public const uint MOUSE_LEFT = 108;
            public const uint MOUSE_RIGHT = 109;
            public const uint MOUSE_MIDDLE = 110;
            public const uint MOUSE_4 = 111;
            public const uint MOUSE_5 = 112;
        }

        public static bool GetKeyState(uint button)
        {
            button = button + 1;
            uint a0 = Memory.ReadValue<uint>(Memory.ModuleBase + Offsets.INPUT_SYSTEM + ((button >> 5) * 4) + 0xB0, false);
            return ((a0 >> ((int)button & 31)) & 1) != 0;
        }
    }
}
