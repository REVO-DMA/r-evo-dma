using pubg_dma_esp.Misc;

namespace pubg_dma_esp.ESP
{
    public static class ESP_Manager
    {
        private static readonly Thread _espWorker;
        private static ESP_Window _espWindow;

        static ESP_Manager()
        {
            try
            {
                Logger.WriteLine("Instantiating ESP Manager...");

                // Init threads
                _espWorker = new Thread(RealtimeWorker)
                {
                    IsBackground = true
                };
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP MANAGER] Early fail: {ex}");
                MessageBox.Show($"[ESP MANAGER] Early fail: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
                Environment.Exit(1);
            }
        }

        public static void Start()
        {
            try
            {
                Logger.WriteLine("Starting ESP Manager...");

                _espWorker.Start();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP MANAGER] Start() -> exception: {ex}");
                MessageBox.Show($"[ESP MANAGER] Start() -> exception: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
                Environment.Exit(1);
            }
        }

        private static void RealtimeWorker()
        {
            try
            {
                _espWindow ??= new();
                _espWindow.DoRender();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP MANAGER] RealtimeWorker() -> exception: {ex}");
                MessageBox.Show($"[ESP MANAGER] RealtimeWorker() -> exception: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
                Environment.Exit(1);
            }
        }
    }
}
