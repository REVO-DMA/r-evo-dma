global using System.Text;
global using System.Diagnostics;
global using System.Numerics;
global using System.Runtime.InteropServices;
global using System.Runtime.CompilerServices;
global using arena_dma_backend.DMA;
global using SDK;
using arena_dma_backend.Misc;
using Kokuban;
using arena_dma_backend.Arena;
using arena_dma_backend.ESP;

namespace arena_dma_backend
{
    internal class Program
    {
        public static UserConfig UserConfig;

        public static readonly string SessionToken;

        static Program()
        {
            Console.WriteLine(Chalk.Blue + "Please check the \"Arena Help Files\" folder for information on how to set some options in the config file.");

            Directory.CreateDirectory("Arena Help Files");

            File.WriteAllBytes("Arena Help Files\\bones.txt", File.ReadAllBytes("Help Files\\bones.txt"));
            File.WriteAllBytes("Arena Help Files\\hotkeys.txt", File.ReadAllBytes("Help Files\\hotkeys.txt"));

            if (!UserConfig.TryLoadConfig(out UserConfig))
            {
                UserConfig = new();
                UserConfig.SaveConfig(UserConfig);
                Console.WriteLine(Chalk.Blue + "Config.json file created, please configure it. Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            // These are immutable settings
            ESP_Config.ESP_Enabled = UserConfig.EnableESP;
            ESP_Config.ESP_FuserMode = UserConfig.FuserMode;
            ESP_Config.ESP_MonitorIndex = UserConfig.ESP_MonitorIndex;
            Game.ChamsEnabled = UserConfig.EnableChams;
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

                Console.WriteLine("EFT: Arena - by EVO DMA");

                UserConfig.LogConfig(UserConfig);

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
