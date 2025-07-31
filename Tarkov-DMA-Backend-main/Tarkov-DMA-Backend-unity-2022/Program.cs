global using SDK;
global using System.Collections.Concurrent;
global using System.Diagnostics;
global using System.Net;
global using System.Numerics;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Runtime.Intrinsics;
global using System.Runtime.Intrinsics.X86;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using Tarkov_DMA_Backend.MemDMA;
global using static Tarkov_DMA_Backend.Tarkov.Loot.ItemManager;
using Sentry.Profiling;
using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend
{
    internal class Program
    {
        public static DateTime AuthCloseTS = DateTime.Now + TimeSpan.FromMinutes(3);

        [STAThread]
        static void Main(string[] args)
        {
            using (SentrySdk.Init(options =>
            {
                options.Dsn = "https://957eb543965b7d02e024ab8a0fdf5e5c@o4507923059376128.ingest.us.sentry.io/4507923061932032";
                options.Debug = false;
                options.AutoSessionTracking = true;
                options.TracesSampleRate = 0.2;
                options.ProfilesSampleRate = 0.2;
                options.SendDefaultPii = true;
                options.StackTraceMode = StackTraceMode.Enhanced;
                options.IsGlobalModeEnabled = true;
                options.ShutdownTimeout = TimeSpan.FromSeconds(3);
                options.AddIntegration(new ProfilingIntegration(
                    TimeSpan.FromMilliseconds(500)
                ));
            }))
            {
                Timing.MakePrecise();

                // Force close program after being open for 12 hours
                System.Timers.Timer timer = new System.Timers.Timer(TimeSpan.FromHours(12).TotalMilliseconds);
                timer.Elapsed += (sender, e) =>
                {
                    Environment.FailFast(null);
                };
                timer.Start();

                // Block until auth is complete
                Server.Start();

                Server.SendRadarStatus(Constants.RadarStatuses.InitializingDMA);
                Memory.Initialize();
                EFTDMA.Initialize();

                UI_Manager.Start();
                ESP_Manager.Start();
                LocalLootManager.Initialize();
                QuestManager.Initialize();

                Thread main = new(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(1000);

                        if (DateTime.Now > AuthCloseTS)
                        {
                            Server.SendRadarStatus(Constants.RadarStatuses.MembershipExpired);
                            Thread.Sleep(5000);
                            Environment.FailFast(null);
                        }
                    }
                });
                main.IsBackground = true;
                main.Start();
                main.Join();
            }
        }
    }
}
