using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.Tarkov.Toolkit.Features;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.LowLevel;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;

namespace Tarkov_DMA_Backend.Tarkov
{
    public sealed class Game : IDisposable
    {
        private const bool DUMP_QUEST_ZONES = false;

        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _ct;
        private readonly Thread _t1;
        private readonly Thread _t2;
        private readonly Thread _t3;
        private readonly Thread _t4;
        private readonly Thread _t5;
        private readonly Thread _t6;
        private readonly Thread _t7;
        private readonly Thread _t8;
        private ulong _localGameWorld;
        private LootManager _lootManager;
        private RegisteredPlayers _rgtPlayers;
        private GrenadeManager _grenadeManager;
        private TripwireManager _tripwireManager;
        private ExfilManager _exfilManager;
        private MineManager _mineManager;
        private ToolkitManager _toolkitManager;
        private volatile bool _inRaid = false;

        private readonly Stopwatch _exfilsStopwatch;
        private readonly Stopwatch _lootStopwatch;

        public static volatile bool QuestHelperEnabled = false;
        public static bool QuestHelperShowAll = false;

        #region Getters
        /// <summary>
        /// Map ID of Current Map.
        /// </summary>
        public string MapID { get; private set; }
        public string InternalMapID { get; private set; }
        public bool InRaid
        {
            get => !_disposed && _inRaid;
        }
        public bool IsOffline { get; private set; }
        public ulong LocalGameWorld
        {
            get => _localGameWorld;
        }
        public Player LocalPlayer
        {
            get => _rgtPlayers?.LocalPlayer;
        }
        public Player[] DisplayPlayers
        {
            get => _rgtPlayers?.DisplayPlayers;
        }
        public Player[] ActiveAlivePlayers
        {
            get => _rgtPlayers?.ActiveAlivePlayers;
        }
        public IReadOnlyDictionary<ulong, Player> Players
        {
            get => _rgtPlayers?.Players;
        }
        public LootManager Loot
        {
            get => _lootManager;
        }
        public IReadOnlyDictionary<ulong, Grenade> Grenades
        {
            get => _grenadeManager?.Grenades;
        }
        public IReadOnlyDictionary<ulong, TripwireManager.Tripwire> Tripwires
        {
            get => _tripwireManager?.Tripwires;
        }
        public IReadOnlyList<Exfil> Exfils
        {
            get => _exfilManager?._exfils;
        }
        public IReadOnlyList<Claymore> Claymores
        {
            get => _mineManager?.Claymores;
        }
        public FeaturesController FeaturesController
        {
            get => _toolkitManager?.FeaturesController;
        }
        #endregion

        #region STARTUP

        public Game()
        {
            _cts = new();
            _ct = _cts.Token;

            _t1 = new Thread(RealtimePlayersWorker)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _t2 = new Thread(DeferredPlayersWorker)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            _t3 = new Thread(MiscWorker)
            {
                IsBackground = true
            };
            _t4 = new Thread(GrenadesWorker)
            {
                IsBackground = true
            };
            _t5 = new Thread(HandsDMAWorker)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            _t6 = new Thread(AimbotWorker)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _t7 = new Thread(SuperSpeedWorker)
            {
                IsBackground = true
            };
            _t8 = new Thread(CharacterManipulationWorker)
            {
                IsBackground = true
            };

            Player.Reset(); // Reset static assets for a new raid/game.

            _exfilsStopwatch = new();
            _exfilsStopwatch.Start();

            _lootStopwatch = new();
            _lootStopwatch.Start();
        }

        public bool GetRaidInstance()
        {
            try
            {
                _localGameWorld = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton("EFT.GameWorld"));

                // Check if this is an offline raid
                ulong classNamePtr = Memory.ReadPtrChain(_localGameWorld, UnityOffsets.Component.To_NativeClassName, false);
                string className = Memory.ReadUtf8String(classNamePtr, 64, false);
                if (className == "ClientLocalGameWorld")
                    IsOffline = true;
                else
                    IsOffline = false;

                var mapPtr = Memory.ReadPtrUnsafe(_localGameWorld + Offsets.ClientLocalGameWorld.LocationId, false);
                if (mapPtr == 0x0) // Offline Mode
                {
                    var localPlayer = Memory.ReadPtr(_localGameWorld + Offsets.ClientLocalGameWorld.MainPlayer, false);
                    mapPtr = Memory.ReadPtr(localPlayer + Offsets.Player.Location, false);
                }

                var map = Memory.ReadUnityString(mapPtr, false, 32);
                if (!MapsManager.MapNames.TryGetValue(map, out string mapID))
                    throw new Exception("Invalid Map ID!");

                MapID = mapID;

                // Fix map IDs
                if (map.Contains("Sandbox"))
                    map = "Sandbox";

                InternalMapID = map;

                var inRaid = Memory.ReadValue<bool>(_localGameWorld + Offsets.ClientLocalGameWorld.LoadBundlesAndCreatePools, false);
                if (!inRaid)
                    throw new Exception("Player is not in raid!");

                var rgtPlayersAddr = Memory.ReadPtr(_localGameWorld + Offsets.ClientLocalGameWorld.RegisteredPlayers, false);
                var rgtPlayers = new RegisteredPlayers(rgtPlayersAddr);
                if (rgtPlayers.PlayerCount >= 1) // Make sure LocalPlayer is found
                {
                    _rgtPlayers = rgtPlayers; // update ref
                    return true;
                }
                throw new Exception("RegisteredPlayers does not contain players!");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"ERROR getting Local Game World: {ex}");
                return false;
            }
            finally
            {
                Server.SendRadarStatus(Constants.RadarStatuses.RaidEnded);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private readonly struct HandleData
        {
            [FieldOffset(0x8)]
            public readonly ulong Entries;
            [FieldOffset(0x10)]
            public readonly uint Size;
        }

        public void PrepareGame()
        {
            Server.SendRadarStatus(Constants.RadarStatuses.RaidBegan);

            if (DUMP_QUEST_ZONES)
                DumpQuestZones();

            _inRaid = true;

            _toolkitManager = new();

            EFTDMA.MonoClasses.Refresh();

            VisibilityCheck.Update();

            Logger.WriteLine("[GAME] Waiting for UI ready status...");
            while (!EFTDMA.RadarReady)
                Thread.Sleep(32);

            // Start Game Threads -> Return to caller
            _t1.Start();
            _t2.Start();
            _t3.Start();
            _t4.Start();
            _t5.Start();
            _t6.Start();
            _t7.Start();
            _t8.Start();
        }
        #endregion

        #region GameLoop / Threads

        public void GameLoop()
        {
            try
            {
                _rgtPlayers?.UpdateList(); // Check for new players, add to list
            }
            catch (RaidEnded)
            {
                Logger.WriteLine("Raid has ended!");

                _inRaid = false;

                Server.SendRadarStatus(Constants.RadarStatuses.RaidEnded);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR - Raid ended due to unhandled exception: {ex}");
                _inRaid = false;
                Server.SendRadarStatus(Constants.RadarStatuses.RaidEnded);
                throw;
            }
        }

        private void RealtimePlayersWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("Realtime Players thread starting...");
                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    _rgtPlayers?.UpdateAllPlayersRealtime();

                    Thread.Sleep(16);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Realtime Players Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Realtime Players thread stopping...");
            }
        }

        private void DeferredPlayersWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("Deferred Players thread starting...");
                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    _rgtPlayers?.UpdateAllPlayersDeferred();
                    
                    Thread.Sleep(300);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Deferred Players Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Deferred Players thread stopping...");
            }
        }

        private void MiscWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("Misc thread starting...");
                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    UpdateMisc();
                    
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Misc Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Misc thread stopping...");
            }
        }

        private void GrenadesWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Stopwatch sw_grenades = Stopwatch.StartNew();
                Stopwatch sw_tripwires = Stopwatch.StartNew();

                Logger.WriteLine("Grenades thread starting...");
                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    if (sw_grenades.ElapsedMilliseconds >= 32)
                    {
                        UpdateGrenades();
                        sw_grenades.Restart();
                    }

                    if (sw_tripwires.ElapsedMilliseconds >= 500)
                    {
                        if (_tripwireManager == null)
                            _tripwireManager = new(_localGameWorld);

                        _tripwireManager.Update();

                        sw_tripwires.Restart();
                    }

                    Thread.Sleep(1);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Grenades Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Grenades thread stopping...");
            }
        }

        private void HandsDMAWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("HandsDMA thread starting...");
                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    _rgtPlayers?.RefreshHands();
                    
                    Thread.Sleep(650);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on HandsDMA Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("HandsDMA thread stopping...");
            }
        }

        private void AimbotWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("Aimbot thread starting...");

                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    try
                    {
                        if (Aimbot.Enabled && (Aimbot.Engaged || Aimbot.AlwaysOn)) // Only if Aimbot Enabled
                        {
                            if (Aimbot.AlwaysOn)
                                Aimbot.Dirty = false;

                            if (EFTDMA.LocalPlayer == null ||
                                !LocalHandsManager.IsHoldingGun)
                            {
                                Aimbot.ResetAimbot();
                                Thread.Sleep(16);
                                continue;
                            }

                            Aimbot.SetAimbot();
                            Thread.Sleep(4);
                        }
                        else
                        {
                            Aimbot.Dirty = !Aimbot.ResetAimbot();

                            if (!Aimbot.Dirty)
                                Thread.Sleep(16);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[GAME] -> AimbotWorker(): {ex}");
                    }
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Aimbot Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Aimbot thread stopping...");
            }
        }

        enum SuperSpeedState
        {
            Running,
            Idle,
            Off,
        }

        private void SuperSpeedWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("Super Speed Worker thread starting...");

                SuperSpeedState state = SuperSpeedState.Off;
                DateTime cycleEndDT = DateTime.MinValue;

                float speedLimit = Misc.Misc.GetRandomFloat(90f, 120f);
                bool animatorSpeedRestored = true;

                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    try
                    {
                        Player localPlayer = EFTDMA.LocalPlayer;
                        if (SuperSpeed.superSpeedHack_state == false ||
                            SuperSpeed.superSpeedHack_engaged == false ||
                            localPlayer == null)
                        {
                            // Reset state
                            state = SuperSpeedState.Off;

                            if (!animatorSpeedRestored)
                            {
                                Memory.WriteValue(SpeedHack.GetAnimatorSpeedHackAddress(localPlayer), 1f);
                                animatorSpeedRestored = true;
                            }

                            Thread.Sleep(16);
                            continue;
                        }

                        float superSpeed_Speed = ToolkitManager.FeatureSettings_float["superSpeed_Speed"];

                        if (IsOffline)
                        {
                            ulong movementContext = localPlayer.MovementContext;
                            ulong characterController = localPlayer.CharacterController;

                            Memory.WriteValue(characterController + Offsets.SimpleCharacterController._speedLimit, speedLimit);
                            Memory.WriteValue(characterController + Offsets.SimpleCharacterController._sqrSpeedLimit, speedLimit * speedLimit);

                            Memory.WriteValue(movementContext + Offsets.MovementContext.StateSpeedLimit, speedLimit);
                            Memory.WriteValue(movementContext + Offsets.MovementContext.StateSprintSpeedLimit, speedLimit);

                            Memory.WriteValue(SpeedHack.GetAnimatorSpeedHackAddress(localPlayer), superSpeed_Speed);
                            animatorSpeedRestored = false;

                            Thread.Sleep(32);
                        }
                        else
                        {
                            if (state == SuperSpeedState.Off)
                            {
                                cycleEndDT = DateTime.UtcNow.AddMilliseconds(ToolkitManager.FeatureSettings_int["superSpeed_OnTime"]);
                                state = SuperSpeedState.Running;
                            }
                            else if (state == SuperSpeedState.Running)
                            {
                                if (DateTime.UtcNow >= cycleEndDT)
                                {
                                    state = SuperSpeedState.Idle;
                                    cycleEndDT = DateTime.UtcNow.AddMilliseconds(ToolkitManager.FeatureSettings_int["superSpeed_OffTime"]);
                                    continue;
                                }

                                var basePosition = Memory.ReadValue<Vector3>(localPlayer.Bones[0].HierarchyAddr + 0x90, false);
                                var velocity = Memory.ReadValue<Vector3>(localPlayer.VelocityAddress, false);

                                Memory.WriteValue(localPlayer.Bones[0].HierarchyAddr + 0x90, basePosition + velocity * (0.002f * superSpeed_Speed));
                            }
                            else if (state == SuperSpeedState.Idle)
                            {
                                if (DateTime.UtcNow >= cycleEndDT)
                                {
                                    state = SuperSpeedState.Off;
                                    continue;
                                }

                                // Do nothing
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[GAME] Error on super speed thread: {ex}");
                    }

                    Thread.Sleep(4);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Super Speed Worker: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Super Speed Worker stopping...");
            }
        }

        private void CharacterManipulationWorker()
        {
            if (_disposed) return;
            try
            {
                // Wait for raid to begin before starting thread logic
                while (!CameraManager.PlayerIsInRaid)
                {
                    _ct.ThrowIfCancellationRequested();
                    Thread.Sleep(100);
                }

                Logger.WriteLine("Character Manipulation Worker thread starting...");

                while (InRaid)
                {
                    _ct.ThrowIfCancellationRequested();

                    try
                    {
                        Player localPlayer = EFTDMA.LocalPlayer;
                        if (CharacterManipulation.characterManipulation_state == false || localPlayer == null)
                        {
                            Thread.Sleep(100);
                            continue;
                        }

                        float characterManipulation_Distance = ToolkitManager.FeatureSettings_float["characterManipulation_Distance"];

                        if (CharacterManipulation.characterManipulationUp_engaged)
                        {
                            var basePosition = Memory.ReadValue<Vector3>(localPlayer.Bones[0].HierarchyAddr + 0x90, false);

                            Vector3 newPosition = new(basePosition.X, basePosition.Y + (0.02f * characterManipulation_Distance), basePosition.Z);
                            Memory.WriteValue(localPlayer.Bones[0].HierarchyAddr + 0x90, newPosition);
                        }
                        else if (CharacterManipulation.characterManipulationDown_engaged)
                        {
                            var basePosition = Memory.ReadValue<Vector3>(localPlayer.Bones[0].HierarchyAddr + 0x90, false);

                            Vector3 newPosition = new(basePosition.X, basePosition.Y - (0.02f * characterManipulation_Distance), basePosition.Z);
                            Memory.WriteValue(localPlayer.Bones[0].HierarchyAddr + 0x90, newPosition);
                        }

                        Thread.Sleep(4);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[GAME] Error on character manipulation thread: {ex}");
                    }
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Character Manipulation Worker: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                Logger.WriteLine("Character Manipulation Worker stopping...");
            }
        }

        #endregion

        #region Thread Helpers
        /// <summary>
        /// Validates Player Transforms -> Checks Exfils -> Checks Loot -> Checks Quests
        /// </summary>
        private void UpdateMisc()
        {
            // Init the Mine Manager if we have the static instance
            if (_mineManager == null)
            {
                try
                {
                    var mineManager = new MineManager(EFTDMA.MonoClasses.GetStaticClass("MineDirectional"), _localGameWorld);
                    _mineManager = mineManager; // update ref
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[Mine Manager] Startup error: {ex}");
                }
            }

            Player localPlayer = EFTDMA.LocalPlayer;
            if (localPlayer == null || !localPlayer.FullyAllocated) return;

            if (_exfilsStopwatch.ElapsedMilliseconds > 3000)
            {
                if (_exfilManager == null)
                {
                    try
                    {

                        var exfils = new ExfilManager(_localGameWorld, localPlayer.IsPMC);
                        _exfilManager = exfils; // update ref
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[Exfil Manager] Startup error: {ex}");
                    }
                }
                else _exfilManager.Refresh(); // Refresh exfils

                _exfilsStopwatch.Restart();
            }

            if (_lootStopwatch.ElapsedMilliseconds > 1500)
            {
                if (_lootManager == null)
                {
                    try
                    {
                        var loot = new LootManager(_localGameWorld, _ct);
                        _lootManager = loot; // update ref
                    }
                    catch (OperationCanceledException) { throw; }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[Loot Manager] Startup error: {ex}");
                    }
                }
                else _lootManager.Refresh(); // Update loot

                //_rgtPlayers?.RefreshGear(); // Update gear

                _lootStopwatch.Restart();
            }
        }

        /// <summary>
        /// Checks 'hot' grenades in Local Game World.
        /// </summary>
        private void UpdateGrenades()
        {
            if (_grenadeManager == null)
            {
                try
                {
                    var grenadeManager = new GrenadeManager(_localGameWorld);
                    _grenadeManager = grenadeManager; // update ref
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"ERROR loading GrenadeManager: {ex}");
                }
            }
            else _grenadeManager.Refresh();
        }
        #endregion

        #region Quest Zone Dumper

        private static void DumpQuestZones()
        {
            Logger.WriteLine("[QUEST ZONE DUMPER] Waiting 10 seconds before continuing...");
            Thread.Sleep(10000);

            Logger.WriteLine("[QUEST ZONE DUMPER] Beginning dump...");

            for (int i = 0; i < 4; i++)
            {
                try
                {
                    ulong gcHandles = Memory.MonoModuleBase + 0x495A10;
                    var hd = Memory.ReadValue<HandleData>(gcHandles + (40 * (uint)i), false);
                    List<ulong> objRaw = new();
                    for (int ii = 0; ii < hd.Size; ii++)
                        objRaw.Add(hd.Entries + ((uint)ii * 0x8));
                    List<ulong> obj = Memory.ReadPointers(objRaw, false).Where(x => MemoryUtils.IsValidAddress(x)).ToList();
                    Logger.WriteLine($"[QUEST ZONE DUMPER] Stage 0:{i}");

                    List<ulong> triggers = Memory.AreAny(obj, "TriggerWithId", false);
                    if (triggers.Count == 0)
                        continue;

                    StringBuilder sb = new();
                    sb.AppendLine("{");
                    sb.AppendLine($"\t\"{EFTDMA.InternalMapID}\", new Dictionary<string, Vector3>(StringComparer.OrdinalIgnoreCase)");
                    sb.AppendLine("\t{");

                    static void AddTrigger(List<ulong> triggers, int index, HashSet<string> addedTriggers, StringBuilder sb)
                    {
                        ulong trigger = triggers[index];

                        ulong triggerIdAddr = Memory.ReadPtr(trigger + 0x18, false);
                        string triggerID = Memory.ReadUnityString(triggerIdAddr, false, 128);

                        if (addedTriggers.Contains(triggerID))
                            return;

                        ulong transformInternal = Memory.ReadPtrChain(trigger, UnityOffsets.GameObject.To_TransformInternal, false);
                        Vector3 triggerPos = new UnityTransform(transformInternal, UnityTransform.TransformType.Normal).GetPosition();

                        sb.AppendLine($"\t\t{{ \"{triggerID}\", new Vector3({triggerPos.X}f, {triggerPos.Y}f, {triggerPos.Z}f) }},");

                        addedTriggers.Add(triggerID);
                    }

                    HashSet<string> addedTriggers = new();
                    for (int ii = 0; ii < triggers.Count; ii++)
                    {
                        try
                        {
                            AddTrigger(triggers, ii, addedTriggers, sb);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                AddTrigger(triggers, ii, addedTriggers, sb);
                            }
                            catch { }

                            Logger.WriteLine($"[QUEST ZONE DUMPER] -> Failed to dump quest zone ~ {ex}");
                        }
                    }

                    sb.AppendLine("\t}.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase)");
                    sb.AppendLine("},");

                    Logger.WriteLine(sb.ToString());
                    Logger.WriteLine("[QUEST ZONE DUMPER] Press enter to continue...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex);
                }
            }
        }

        #endregion

        #region IDisposable

        private readonly object _disposeSyncRoot = new();

        private volatile bool _disposed = false;
        public void Dispose()
        {
            lock (_disposeSyncRoot)
            {
                if (_disposed) return;

                EFTDMA.RadarReady = false;

                _inRaid = false;
                _cts.Cancel();
                _cts.Dispose();
                _disposed = true;

                _toolkitManager?.Dispose();

                CameraManager.Reset();

                // Notify client
                Server.SendRadarStatus(Constants.RadarStatuses.RaidEnded);
            }
        }

        #endregion
    }

    #region Exceptions
    public sealed class GameNotRunningException : Exception
    {
        public GameNotRunningException() { }

        public GameNotRunningException(string message) : base(message) { }

        public GameNotRunningException(string message, Exception inner) : base(message, inner) { }
    }

    public sealed class RaidEnded : Exception
    {
        public RaidEnded() { }

        public RaidEnded(string message) : base(message) { }

        public RaidEnded(string message, Exception inner) : base(message, inner) { }
    }
    #endregion
}
