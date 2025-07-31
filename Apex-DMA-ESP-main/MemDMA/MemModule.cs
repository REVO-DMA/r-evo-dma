using apex_dma_esp.Misc;

namespace apex_dma_esp.MemDMA
{
    internal static class MemModule
    {
        /// <summary>
        /// DMA Memory Module.
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public static MemDMA Memory;
#pragma warning restore CS8618

        /// <summary>
        /// Startup Memory Module.
        /// ONLY CALL ONCE!
        /// </summary>
        /// <param name="isDebug">True if Vmm instance should emit debug information.</param>
        public static void StartMemoryModule(bool isDebug)
        {
            try
            {

                Logger.WriteLine("Starting memory module...");
                Memory = new(isDebug);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DMA] StartMemoryModule() -> exception: {ex}");
                MessageBox.Show($"[DMA] StartMemoryModule() -> exception: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
                Environment.Exit(1);
            }
        }
    }
}
