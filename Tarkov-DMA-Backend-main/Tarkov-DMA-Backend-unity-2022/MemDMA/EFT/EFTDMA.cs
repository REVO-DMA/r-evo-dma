using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.LowLevel;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures;
using Tarkov_DMA_Backend.Tarkov.Toolkit.Features;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Hotkey;
using Tarkov_DMA_Backend.Unity.LowLevel;
using Tarkov_DMA_Backend.Unity.LowLevel.PersistentCache;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;

namespace Tarkov_DMA_Backend.MemDMA.EFT
{
    public static class EFTDMA
    {
        #region Fields/Properties/Constructor
        
        private static XThread _tPrimary;

        private static volatile bool _restart = false;
        public static bool RadarReady = false;
        public static ulong HookAddress = 0x0;

        public static GameObjectManager? GOM;
        public static AlwaysActiveFeaturesController AlwaysActiveFeaturesController;
        public static PatchManager PatchManager;
        public static Classes MonoClasses;
        public static string SessionToken => NetworkContainer.GetSessionToken();
        public static byte[] AesKey = [];

        private static Game _game;
        public static string MapID => _game?.MapID;
        public static string InternalMapID => _game?.InternalMapID;
        public static bool InRaid => _game?.InRaid ?? false;
        public static Player LocalPlayer => _game?.LocalPlayer;
        public static Player[] DisplayPlayers => _game?.DisplayPlayers;
        public static Player[] ActiveAlivePlayers => _game?.ActiveAlivePlayers;
        public static IReadOnlyDictionary<ulong, Player> Players => _game?.Players;
        public static bool IsOffline => _game.IsOffline;
        public static ulong LocalGameWorld => _game.LocalGameWorld;
        public static LootManager Loot => _game?.Loot;
        public static IReadOnlyDictionary<ulong, Grenade> Grenades => _game?.Grenades;
        public static IReadOnlyDictionary<ulong, TripwireManager.Tripwire> Tripwires => _game?.Tripwires;
        public static IReadOnlyList<Exfil> Exfils => _game?.Exfils;
        public static IReadOnlyList<Claymore> Claymores => _game?.Claymores;
        public static FeaturesController FeaturesController => _game?.FeaturesController;

        public static void Initialize()
        {
            _tPrimary = new(MemoryPrimaryWorker, ApartmentState.STA);
        }

        #endregion

        #region Primary Memory Thread

        private static void MemoryPrimaryWorker()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        Server.SendRadarStatus(Constants.RadarStatuses.ProcessEnded);

                        InputManager inputManager;
                        PatchManager = new();

                        Logger.WriteLine("Attempting to locate EFT Process...");

                        while (true) // Startup loop
                        {
                            if (Memory.Attach("EscapeFromTarkov.exe"))
                            {
                                Memory.UnityPlayerModuleBase = Memory.GetModuleBase("UnityPlayer.dll");
                                Memory.MonoModuleBase = Memory.GetModuleBase("mono-2.0-bdwgc.dll");

                                Logger.WriteLine("[INITIAL STARTUP] Searching for required mono classes...");

                                var requiredClasses = MonoLibrary.FindClasses("Assembly-CSharp",
                                [
                                    ClassNames.NetworkContainer.ClassName,
                                    ClassNames.GameAPIClient.ClassName,
                                    "EFT.AbstractApplication"
                                ]);

                                foreach (MonoClass mClass in requiredClasses)
                                {
                                    if (!MemoryUtils.IsValidAddress(mClass))
                                        throw new Exception("Failed to find a required mono class!");
                                }

                                // Get the session token
                                {
                                    int tries = -1;
                                    bool success = false;
                                    while (tries <= 5)
                                    {
                                        tries++;

                                        byte[] tmpAES = null;
                                        string tmpST = null;

                                        Logger.WriteLine($"[INITIAL STARTUP] Loading network container ({tries + 1})...");
                                        if (NetworkContainer.TryLoad(requiredClasses[0]))
                                        {
                                            tmpAES = MiscUtils.GetAESKey(requiredClasses[1]);
                                            tmpST = NetworkContainer.GetSessionToken(false);
                                        }

                                        if (tmpAES != null && tmpST != null)
                                        {
                                            AesKey = tmpAES;
                                            success = true;
                                            break;
                                        }

                                        Thread.Sleep(1000);
                                    }

                                    if (!success)
                                        throw new Exception("Error initializing Network Container!");
                                }

                                {
                                    Logger.WriteLine($"[INITIAL STARTUP] Loading native helper...");

                                    if (!NativeHelper.Initialize(Memory.HVMM, Memory.Process.PID))
                                        throw new Exception("Error initializing NativeHelper");

                                    if (!NativeHelper.SetCodeCave(requiredClasses[2]))
                                        throw new Exception("Error initializing CodeCave");
                                }

                                if (!InputManager.TryLoad(Memory.UnityPlayerModuleBase, out inputManager))
                                    throw new Exception("Error initializing Input Manager");

                                Logger.WriteLine($"EFT Startup [OK]");
                                Server.SendRadarStatus(Constants.RadarStatuses.LoadingFeatures);
                                break;
                            }
                            else
                            {
                                Logger.WriteLine("EFT Startup [FAIL]");
                                Server.SendRadarStatus(Constants.RadarStatuses.ProcessEnded);
                                throw new Exception("Failed to find valid PID");
                            }
                        }

                        if (!Cache.TryLoad())
                        {
                            Cache.SaveHookAddress();
                        }

                        // Wait for mono to initialize
                        {
                            int tries = -1;
                            bool success = false;
                            while (tries <= 6)
                            {
                                tries++;

                                MonoClasses = new();
                                MonoClasses.Refresh();

                                bool status = MonoClasses.IsInitialized();
                                if (status)
                                {
                                    success = true;
                                    break;
                                }

                                Thread.Sleep(2000);
                            }

                            if (!success)
                                throw new Exception("Error initializing Mono API!");
                        }

                        // Load locale
                        {
                            int tries = -1;
                            bool success = false;
                            while (tries <= 6)
                            {
                                tries++;

                                bool status = LocaleManager.TryLoad();
                                if (status)
                                {
                                    success = true;
                                    break;
                                }

                                Thread.Sleep(2000);
                            }

                            if (!success)
                                throw new Exception("Error initializing Locale!");
                        }

                        using (AlwaysActiveFeaturesController = new())
                        {
#if COMMERCIAL || RELEASE
                            
                            AlwaysActiveFeaturesController.AddFeature(new KeybindFromAnywhere(1000));
                            AlwaysActiveFeaturesController.AddFeature(new ShowOwnDogTag(1000));
                            AlwaysActiveFeaturesController.AddFeature(new HarmlessAI(1000));
                            AlwaysActiveFeaturesController.AddFeature(new NoFallDamage(1000));
                            //AlwaysActiveFeaturesController.AddFeature(new StreamerMode(1000));
                            
#endif

                            AlwaysActiveFeaturesController.AddFeature(new FixWildSpawnType(1000));
                            AlwaysActiveFeaturesController.AddFeature(new RamCleanerMonitor(1000));
                            AlwaysActiveFeaturesController.AddFeature(new HideRaidCode(1000));
                            AlwaysActiveFeaturesController.AddFeature(new AntiAFK(1000));
                            AlwaysActiveFeaturesController.AddFeature(new NoPenalties(1000));
                            AlwaysActiveFeaturesController.AddFeature(new RemovableAttachments(1000));
                            AlwaysActiveFeaturesController.AddFeature(new NoMalfunctions(1000));

                            var fixWildSpawnType = AlwaysActiveFeaturesController.GetFeature(FixWildSpawnType.featureID);
                            // Wait for WildSpawnType to be fixed
                            {
                                int tries = -1;
                                while (!fixWildSpawnType.CurrentState && tries <= 5)
                                {
                                    tries++;

                                    Thread.Sleep(1000);
                                }

                                if (!fixWildSpawnType.CurrentState)
                                    throw new Exception("Error initializing fixWildSpawnType!");
                            }

                            // Wait for vis check to be injected
                            {
                                int tries = -1;
                                bool success = false;
                                while (tries <= 5)
                                {
                                    tries++;

                                    var initData = VisibilityCheck.GetInitData();
                                    if (initData && VisibilityCheck.Initialize(initData))
                                    {
                                        success = true;
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                if (!success)
                                    throw new Exception("Error initializing visibility check!");
                            }

                            Server.SendRadarStatus(Constants.RadarStatuses.ProcessStarted);

                            using (inputManager)
                            {
                                while (true) // Game is running
                                {
                                    using (_game = new())
                                    {
                                        try
                                        {
                                            while (true)
                                            {
                                                // Refresh the GOM & check if the game is still running
                                                try { GOM = GameObjectManager.Get(Memory.UnityPlayerModuleBase); }
                                                catch { throw new GameNotRunningException(); }

                                                ulong gameWorldSingleton = 0x0;
                                                try { gameWorldSingleton = MonoClasses.GetSingleton("EFT.GameWorld"); }
                                                catch { }

                                                // Only keep refreshing if the gameworld singleton hasn't been found
                                                if (gameWorldSingleton == 0x0)
                                                    MonoClasses.Refresh();

                                                // Check if a raid has begun
                                                if (_game.GetRaidInstance() && InitializeNative())
                                                {
                                                    Thread.Sleep(2000);
                                                    break;
                                                }
                                                else Thread.Sleep(1500);
                                            }

                                            _game.PrepareGame();
                                            Thread.Sleep(500);

                                            while (_game.InRaid)
                                            {
                                                if (IsPendingRestart()) break;
                                                _game.GameLoop();
                                                Thread.Sleep(32);
                                            }
                                        }
                                        catch (GameNotRunningException) { break; }
                                        catch (Exception ex)
                                        {
                                            Logger.WriteLine($"CRITICAL ERROR in Game Loop: {ex}");
                                            SentrySdk.CaptureException(ex);
                                        }
                                    }

                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"ERROR on Memory Thread: {ex}");
                    }
                    finally
                    {
                        try
                        {
                            Thread.Sleep(500);

                            ChamsManager.Reset();
                            PatchManager?.Reset();
                            Aimbot.WeaponDirectionGetter = 0x0;
                            SilentLoot.MonoMethod_SaveInteractionRayInfo = null;
                            SilentLoot.InteractionRaycastAddr = 0x0;
                            SilentLoot.InteractionRaycastPatchOffset = uint.MaxValue;
                            MonoClasses?.Dispose();
                            GOM = null;
                            Memory.Detach();

                            Logger.WriteLine("Game is no longer running! Attempting to restart...");
                            Server.SendRadarStatus(Constants.RadarStatuses.ProcessEnded);
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"FATAL ERROR on Memory Thread: {ex}"); // Log fatal error
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

#endregion

        #region Misc

        /// <summary>
        /// Checks if the radar is pending a Restart.
        /// </summary>
        private static bool IsPendingRestart()
        {
            if (_restart)
            {
                Logger.WriteLine("Restarting game... getting fresh GameWorld instance");
                _restart = false;

                return true;
            }

            else return false;
        }

        /// <summary>
        /// Sets restart flag to re-initialize the game/pointers from the bottom up.
        /// </summary>
        public static void Restart()
        {
            if (InRaid)
            {
                _restart = true;
            }
        }

        public static bool InitializeNative()
        {
            if (ChamsManager.ChamsStatus is ChamsManager.ChamsLoadStatus.FullyLoaded)
                return true;

            Server.SendRadarStatus(Constants.RadarStatuses.LoadingFeatures);

            Server.SendRadarStatus(Constants.RadarStatuses.RaidEnded);

            return true;
        }

        public static bool InitChams()
        {
            if (ChamsManager.ChamsStatus == ChamsManager.ChamsLoadStatus.FullyLoaded)
                return true;

            ChamsManager.ChamsStatus = ChamsManager.Initialize();

            if (ChamsManager.ChamsStatus == ChamsManager.ChamsLoadStatus.FailedToLoad)
            {
                Logger.WriteLine("[InitChams] Failed to init chams!");
                return false;
            }
            
            return true;
        }

        #endregion
    }
}
