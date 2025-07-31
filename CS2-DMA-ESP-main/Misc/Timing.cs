namespace cs2_dma_esp.Misc
{
    public static partial class Timing
    {
        [LibraryImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        private static partial uint TimeBeginPeriod(uint uMilliseconds);

        [LibraryImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
        private static partial uint TimeEndPeriod(uint uMilliseconds);

        public static void MakePrecise()
        {
            try
            {
                TimeBeginPeriod(1);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[TIMING] Early fail: {ex}");
                MessageBox.Show($"[TIMING] Early fail: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
                Environment.Exit(1);
            }
        }
    }
}
