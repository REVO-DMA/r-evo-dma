namespace apex_dma_esp.ESP
{
    public static class ESP_Config
    {
        private static int _espResolutionX;
        private static int _espResolutionY;

        public static Vector2 ScreenCenter { get; private set; }

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
            }
        }
    }
}
