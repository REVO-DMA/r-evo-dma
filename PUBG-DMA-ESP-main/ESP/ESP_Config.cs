namespace pubg_dma_esp.ESP
{
    public static class ESP_Config
    {
        private static int _espResolutionX;
        private static int _espResolutionY;

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

        public static int ESP_ResolutionX
        {
            get
            {
                return _espResolutionX;
            }
            set
            {
                _espResolutionX = value;
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
        public static int ESP_ResolutionY
        {
            get
            {
                return _espResolutionY;
            }
            set
            {
                _espResolutionY = value;
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
