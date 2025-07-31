global using System.Text;
global using System.Diagnostics;
global using System.Numerics;
global using System.Collections.Concurrent;
global using System.Runtime.InteropServices;
global using System.Runtime.CompilerServices;
global using static pubg_dma_esp.MemDMA.MemModule;
using pubg_dma_esp.Misc;

namespace pubg_dma_esp
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
                Console.WriteLine("Config.json file created, please configure it. Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("PUBG ESP - by EVO DMA");

                Timing.MakePrecise();

                StartMemoryModule();

                Thread main = new(() =>
                {
                    while (true)
                        Thread.Sleep(100);
                });
                main.IsBackground = true;
                main.Start();
                main.Join();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"An unknown error occurred: {ex}");
                MessageBox.Show($"An unknown error occurred: {ex.Message}", "EVO DMA", MessageBox.MessageBoxButtons.Ok, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1, MessageBox.MessageBoxModal.System);
            }
        }
    }
}
