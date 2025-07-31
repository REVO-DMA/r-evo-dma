namespace arena_dma_backend.ESP
{
    public static class ESP_Config
    {
        public static bool ESP_Enabled = true;
        public static bool ESP_FuserMode = false;
        public static int ESP_MonitorIndex = 0;

        public static bool ShouldScale = false;

        private static int _gameResolutionX;
        private static int _gameResolutionY;

        /// <summary>
        /// The screen center coordinates.
        /// </summary>
        public static Vector2 ScreenCenter { get; private set; }

        public struct RenderBounds
        {
            public short minX;
            public short maxX;

            public short minY;
            public short maxY;
        }
        /// <summary>
        /// The "safe area" to render in.
        /// </summary>
        public static RenderBounds ScreenRenderBounds { get; private set; }

        public static int ESP_ResolutionX { get; set; }
        public static int ESP_ResolutionY { get; set; }

        public static int GameResolutionX
        {
            get
            {
                return _gameResolutionX;
            }
            set
            {
                _gameResolutionX = value;
                ScreenCenter = new(value / 2f, ScreenCenter.Y);
                ScreenRenderBounds = new()
                {
                    minX = -400,
                    maxX = (short)(value + 400),

                    minY = ScreenRenderBounds.minY,
                    maxY = ScreenRenderBounds.maxY,
                };
            }
        }
        public static int GameResolutionY
        {
            get
            {
                return _gameResolutionY;
            }
            set
            {
                _gameResolutionY = value;
                ScreenCenter = new(ScreenCenter.X, value / 2f);
                ScreenRenderBounds = new()
                {
                    minX = ScreenRenderBounds.minX,
                    maxX = ScreenRenderBounds.maxX,

                    minY = -400,
                    maxY = (short)(value + 400),
                };
            }
        }
    }
}
