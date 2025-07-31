global using System.Text;
global using System.Diagnostics;
global using System.Numerics;
global using System.Collections.Concurrent;
global using System.Runtime.InteropServices;
global using System.Runtime.CompilerServices;
global using static cs2_dma_esp.MemDMA.CS2.MemModule;
using cs2_dma_esp.Misc;

namespace cs2_dma_esp
{
    internal class Program
    {
        public static readonly UserConfig UserConfig;

        static Program()
        {
            if (!UserConfig.TryLoadConfig(out UserConfig))
            {
                UserConfig = new();
                UserConfig.SaveConfig(UserConfig);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("CS2 ESP - by EVO DMA");

                Timing.MakePrecise();

                if (GarbageLicensing.IsExpired()) Environment.Exit(0);
                GarbageLicensing.TrialHelper();

                // Force close program after being open for 12 hours
                System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromHours(12).TotalMilliseconds);
                timer.Elapsed += (sender, e) =>
                {
                    Environment.FailFast(null);
                };
                timer.Start();

                bool isDebug = args?.Any(x => x.Trim().Equals("-debug", StringComparison.OrdinalIgnoreCase)) ?? false;

                if (isDebug)
                    Logger.WriteLine("[INFO] Started in debug mode.");

                StartMemoryModule(isDebug);

                Thread main = new(() =>
                {
                    while (true)
                        Thread.Sleep(100);
                });
                main.IsBackground = true;
                main?.Start();
                main?.Join();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An unknown error occurred: {ex}");
                MessageBox.Show($"An unknown error occurred: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
            }
        }
    }
}
