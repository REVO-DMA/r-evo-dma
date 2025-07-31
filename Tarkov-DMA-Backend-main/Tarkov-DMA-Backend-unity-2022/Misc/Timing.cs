namespace Tarkov_DMA_Backend.Misc
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
                MessageBox.ShowError($"[TIMING] Early fail: {ex.Message}", "EVO DMA");
                Environment.Exit(1);
            }
        }
    }
}
