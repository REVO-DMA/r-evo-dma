global using System.Text;
global using System.Diagnostics;
global using System.Numerics;
global using Silk.NET.Maths;
global using System.Runtime.InteropServices;
global using System.Runtime.CompilerServices;
global using gzw_dma_backend.DMA;
using gzw_dma_backend.Misc;
using Kokuban;

namespace gzw_dma_backend
{
    internal class Program
    {
        public static UserConfig UserConfig;

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
                Timing.MakePrecise();

                if (GarbageLicensing.IsExpired())
                {
                    Console.WriteLine(Chalk.Red + "Cheat expired. Please ping CodeNulls!");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
                GarbageLicensing.TrialHelper();

                Console.WriteLine("GZW - by EVO DMA");

                // Force close program after being open for 12 hours
                System.Timers.Timer timer = new(TimeSpan.FromHours(12).TotalMilliseconds);
                timer.Elapsed += (sender, e) =>
                {
                    Environment.FailFast(null);
                };
                timer.Start();
                
                Memory.Initialize();

                Thread main = new(() =>
                {
                    while (true)
                        Thread.Sleep(1000);
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
            finally
            {
                Memory.Dispose();
            }
        }
    }
}
