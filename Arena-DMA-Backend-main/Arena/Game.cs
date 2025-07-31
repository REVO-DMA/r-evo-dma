using arena_dma_backend.DMA.Collections;
using arena_dma_backend.DMA.Collections.Implementation;
using arena_dma_backend.DMA.ScatterAPI;
using arena_dma_backend.Mono;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Runtime.Intrinsics;
using static arena_dma_backend.Mono.Patcher;

namespace arena_dma_backend.Arena
{
    public static class Game
    {
        private static ulong _updateIteration = 0;

        public static bool ChamsEnabled = true;

        public static ConcurrentDictionary<ulong, Player> Players { get; private set; } = new();
        public static Enums.ColorType? LocalPlayerColor { get; set; } = null;
        public static Player LocalPlayer { get; private set; } = null;

        public static string MapID { get; private set; } = "N/A";
        public static ulong LGW { get; private set; }
        public static ulong RegisteredPlayers { get; private set; }

        public static bool InMatch { get; private set; }

        public static int PlayerCount { get;  private set; }

        private static readonly Thread _tRealtime;

        static Game()
        {
            _tRealtime = new(Realtime)
            {
                IsBackground = true,
            };
            _tRealtime.Start();
        }

        public static void GetRaidInstance()
        {
            try
            {
                LGW = Memory.ReadPtr(MonoAPI.GameWorld);

                var mapPtr = Memory.ReadPtr(LGW + Offsets.ClientLocalGameWorld.LocationId);
                MapID = Memory.ReadUnityString(mapPtr);

                RegisteredPlayers = Memory.ReadPtr(LGW + Offsets.ClientLocalGameWorld.RegisteredPlayers);

                InMatch = Memory.ReadValue<bool>(LGW + Offsets.ClientLocalGameWorld.LoadBundlesAndCreatePools, false);

                PlayerCount = Memory.ReadValueUnsafe<int>(RegisteredPlayers + UnityOffsets.UnityList.Count, false);
                if (PlayerCount <= 0 || PlayerCount > 128)
                    throw new Exception("Invalid player count!");
            }
            catch (Exception ex)
            {
                MapID = "N/A";
                InMatch = false;
                CameraManager.Reset();

                Players.Clear();
                PlayerCount = 0;

                Memory.CanWrite = false;

                Logger.WriteLine($"[GAME] -> GetRaidInstance(): Unable to get raid instance: {ex}");
                return;
            }
        }

        public static void UpdatePlayers()
        {
            if (!InMatch)
                return;

            _updateIteration++;

            // Update players
            try
            {
                using MemList<ulong> playersList = new(RegisteredPlayers, false);

                foreach (ulong player in playersList)
                {
                    Player newPlayer = new(player, _updateIteration);
                    if (!newPlayer.IsSafe())
                        continue;

                    Players.AddOrUpdate(player, (newItem) => newPlayer, (key, existing) =>
                    {
                        newPlayer.IsAlive = existing.IsAlive;
                        newPlayer.IsTeammate = existing.IsTeammate;
                        newPlayer.IsAimbotLocked = existing.IsAimbotLocked;
                        newPlayer.ChamsDirty = existing.ChamsDirty;
                        newPlayer.BonePositions = existing.BonePositions;
                        newPlayer.Velocity = existing.Velocity;
                        newPlayer.Distance = existing.Distance;
                        newPlayer.EPurgeStatus = existing.EPurgeStatus;

                        return newPlayer;
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[UpdatePlayers] Error updating players: {ex}");
            }

            // Purge old Players
            try
            {
                foreach (var player in Players)
                {
                    var purgeStatus = player.Value.EPurgeStatus;

                    if (player.Value.UpdateIteration != _updateIteration || purgeStatus == Player.PurgeStatus.CanPurge)
                    {
                        if (purgeStatus == Player.PurgeStatus.CanPurge)
                        {
                            if (!Players.TryRemove(player))
                                Logger.WriteLine($"Failed to purge player: \"{player.Value.Name}\"!");
                        }
                        else
                            player.Value.EPurgeStatus = Player.PurgeStatus.Queued;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME] -> UpdatePlayers(): Error while purging old players: {ex}");
            }

            // Assign teammates
            try
            {
                bool enemiesAlive = false;
                
                foreach (var player in Players)
                {
                    if (player.Value.Color == null)
                    {
                        player.Value.IsTeammate = false;
                        enemiesAlive = true;
                    }
                    else if (player.Value.Color == LocalPlayerColor)
                    {
                        player.Value.IsTeammate = true;
                    }
                    else
                    {
                        player.Value.IsTeammate = false;
                        enemiesAlive = true;
                    }
                }

                LocalPlayer = Players.Where(x => x.Value.IsLocal).FirstOrDefault().Value;

                if (LocalPlayer == null || !enemiesAlive)
                    Memory.CanWrite = false;
                else
                    Memory.CanWrite = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME] -> UpdatePlayers(): Error while assigning teammates: {ex}");
            }
        }

        public static void ApplyChams()
        {
            if (!Memory.CanWrite)
                return;

            try
            {
                foreach (var player in Players.Values)
                {
                    if (player.IsAimbotLocked || !player.CanRender()) continue;

                    player.SetChams();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME] -> ApplyChams(): Error while applying chams: {ex}");
            }
        }

        public static bool NoVisorState = false;
        public static float SetFOV = 75f;
        public static bool AlwaysSprintPatched = false;
        public static bool NoFlashPatched = false;

        public static void Toolkit()
        {
            while (true)
            {
                try
                {
                    if (LocalPlayer == null || !LocalPlayer.IsSafe() || !InMatch || !Memory.CanWrite)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    List<ScatterWriteEntry> writes = new();

                    // No Visor
                    try
                    {
                        if (Program.UserConfig.NoVisor)
                        {
                            ulong visorEffect = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), "VisorEffect");
                            writes.Add(ScatterWriteEntry.Create(visorEffect + Offsets.VisorEffect.Intensity, 0f));
                            NoVisorState = true;
                        }
                        else
                        {
                            if (NoVisorState != false)
                            {
                                ulong visorEffect = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), "VisorEffect");
                                writes.Add(ScatterWriteEntry.Create(visorEffect + Offsets.VisorEffect.Intensity, 1f));
                                NoVisorState = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply No Visor ~ {ex}");
                    }

                    // Custom Sway
                    try
                    {
                        ulong bEff = Memory.ReadPtr(LocalPlayer.PWA + Offsets.ProceduralWeaponAnimation.Breath);
                        writes.Add(ScatterWriteEntry.Create(bEff + Offsets.BreathEffector.Intensity, Program.UserConfig.CustomSway / 100f));
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply Custom Sway ~ {ex}");
                    }

                    // Custom Recoil
                    try
                    {
                        float customRecoilValue = Program.UserConfig.CustomRecoil / 100f;
                        Vector3 newRecoilValue = new(customRecoilValue, customRecoilValue, 1f);
                        ulong sEff = Memory.ReadPtr(LocalPlayer.PWA + Offsets.ProceduralWeaponAnimation.Shootingg);
                        ulong newShotRecoil = Memory.ReadPtr(sEff + Offsets.ShotEffector.NewShotRecoil);
                        writes.Add(ScatterWriteEntry.Create(newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, newRecoilValue));

                        if (Program.UserConfig.DisableExtraWeaponMotion)
                        {
                            // Remove weapon motion effects (walk animation, mouse move animation)
                            ulong motionReact = Memory.ReadPtr(LocalPlayer.PWA + Offsets.ProceduralWeaponAnimation.MotionReact);

                            // Set array counts to 0 so the processors don't get executed
                            ulong mouseProcessors = Memory.ReadPtr(motionReact + Offsets.MotionEffector._mouseProcessors);
                            writes.Add(ScatterWriteEntry.Create(mouseProcessors + UnityOffsets.UnityList.Count, 0));

                            ulong movementProcessors = Memory.ReadPtr(motionReact + Offsets.MotionEffector._movementProcessors);
                            writes.Add(ScatterWriteEntry.Create(movementProcessors + UnityOffsets.UnityList.Count, 0));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply Custom Recoil ~ {ex}");
                    }

                    // No Inertia
                    try
                    {
                        if (Program.UserConfig.NoInertia)
                        {
                            ulong HardSettings = Memory.ReadPtr(MonoAPI.HardSettings);
                            ulong InertiaSettingsSingleton = Memory.ReadPtr(MonoAPI.InertiaSettings);
                            ulong physical = Memory.ReadPtr(LocalPlayer.Base + Offsets.Player.Physical);

                            // Mule Mode
                            {
                                const float newOverweight = 0f;
                                const float newWalkOverweight = 0f;
                                const float newWalkSpeedLimit = 1f;
                                const float newInertia = 0.01f;
                                const float newFloat_3 = 0f;
                                const float newSprintAcceleration = 1f;
                                const float newPreSprintAcceleration = 3f;
                                const byte zeroByte = 0;
                                Vector2 bigVector2 = new(9000f, 10000f);

                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.Overweight, newOverweight));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.WalkOverweight, newWalkOverweight));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.WalkSpeedLimit, newWalkSpeedLimit));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.Inertia, newInertia));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.SprintWeightFactor, newFloat_3));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.SprintAcceleration, newSprintAcceleration));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.PreSprintAcceleration, newPreSprintAcceleration));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.WalkOverweightLimits, bigVector2));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.BaseOverweightLimits, bigVector2));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.SprintOverweightLimits, bigVector2));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.IsOverweightA, zeroByte));
                                writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.IsOverweightB, zeroByte));
                            }

                            ulong movementContext = LocalPlayer.MovementContext;
                            writes.Add(ScatterWriteEntry.Create(movementContext + Offsets.MovementContext.WalkInertia, 0f));
                            writes.Add(ScatterWriteEntry.Create(movementContext + Offsets.MovementContext.SprintBrakeInertia, 0f));

                            ulong inertiaSettings = Memory.ReadPtr(InertiaSettingsSingleton + Offsets.GlobalConfigs.Inertia);
                            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.FallThreshold, 99999f));
                            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.BaseJumpPenalty, 0f));
                            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.BaseJumpPenaltyDuration, 0f));
                            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.MoveTimeRange, new Vector2(0f, 0f)));

                            writes.Add(ScatterWriteEntry.Create(HardSettings + Offsets.EFTHardSettings.DecelerationSpeed, 100f));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply No Inertia ~ {ex}");
                    }

                    // FOV Changer
                    try
                    {
                        static ulong GetSettingValueClass()
                        {
                            ulong GameSettings = Memory.ReadPtr(MonoAPI.GameSettings);

                            ulong Game = Memory.ReadPtr(GameSettings + Offsets.GameSettingsContainer.Game);
                            ulong Settings = Memory.ReadPtr(Game + Offsets.GameSettingsInnerContainer.Settings);
                            ulong FieldOfView = Memory.ReadPtr(Settings + Offsets.GameSettings.FieldOfView);
                            ulong SettingValueClass = Memory.ReadPtr(FieldOfView + Offsets.BSGGameSetting.ValueClass);

                            return SettingValueClass;
                        }

                        static void WriteFOV(float fov)
                        {
                            ulong fpsCamera = Memory.GetCamera(CameraManager.GetFPSCamera());
                            Memory.WriteValue(fpsCamera + UnityOffsets.Camera.FOV, fov);
                        }

                        int newFOV = (int)Program.UserConfig.CustomFOV;
                        if (SetFOV != newFOV)
                        {
                            writes.Add(ScatterWriteEntry.Create(GetSettingValueClass() + Offsets.BSGGameSettingValueClass.Value, newFOV));
                            WriteFOV(newFOV);
                            SetFOV = newFOV;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply FOV Changer ~ {ex}");
                    }

                    // Always Sprint
                    try
                    {
                        if (Program.UserConfig.AlwaysSprint)
                        {
                            if (!AlwaysSprintPatched)
                            {
                                SignatureInfo sigInfo = new(null, ShellKeeper.PatchTrue);
                                PatchMethod("EFT.MovementContext", "SetPhysicalCondition", sigInfo, compileClass: true);
                                AlwaysSprintPatched = true;
                            }

                            writes.Add(ScatterWriteEntry.Create(LocalPlayer.MovementContext + Offsets.MovementContext._physicalCondition, (int)Enums.EPhysicalCondition.None));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply Always Sprint ~ {ex}");
                    }

                    // No Flash
                    try
                    {
                        static void SetComponentState(string componentName, bool newState)
                        {
                            ulong component = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), componentName);
                            Behaviour behaviour = new(Memory.ReadPtr(component + 0x10));
                            if (behaviour.GetState() != newState)
                            {
                                if (!behaviour.SetState(newState))
                                    throw new Exception($"\"{componentName}\" state could not be set to \"{newState}\"!");
                            }
                        }

                        if (Program.UserConfig.NoFlash)
                        {
                            if (!NoFlashPatched)
                            {
                                // Disables the screen going dark
                                SignatureInfo sigInfoTrue = new(null, ShellKeeper.PatchReturn);
                                PatchMethod("GrenadeFlashScreenEffect", "Update", sigInfoTrue, compileClass: true);

                                NoFlashPatched = true;
                            }

                            SetComponentState("EyeBurn", false);
                            SetComponentState("CC_Wiggle", false);
                            SetComponentState("CC_DoubleVision", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply No Flash ~ {ex}");
                    }

                    // Speed Hack
                    try
                    {
                        static ulong GetAnimatorSpeedHackAddress(ulong localPlayer)
                        {
                            ulong animatorsArrPtr = Memory.ReadPtr(localPlayer + Offsets.Player._animators);
                            using MemArray<ulong> animatorsArray = new(animatorsArrPtr);
                            ulong playerAnimatorPtr = animatorsArray[0]; // -.GClass1141 : Object, IAnimator
                            ulong playerAnimator = Memory.ReadPtr(playerAnimatorPtr + 0x10); // animator_0 : UnityEngine.Animator
                            ulong anim_m_CachedPtr = Memory.ReadPtr(playerAnimator + 0x10); // m_CachedPtr
                            ulong anim_gameObject = Memory.ReadPtr(anim_m_CachedPtr + 0x30);
                            ulong anim_noClue = Memory.ReadPtr(anim_gameObject + 0x30);
                            return Memory.ReadPtr(anim_noClue + 0x18) + UnityOffsets.Animator.Speed; // UnityEngine.Animator
                        }

                        if (Program.UserConfig.SpeedHack)
                        {
                            writes.Add(ScatterWriteEntry.Create(GetAnimatorSpeedHackAddress(LocalPlayer.Base), 1.2f));
                        }
                        else
                        {
                            writes.Add(ScatterWriteEntry.Create(GetAnimatorSpeedHackAddress(LocalPlayer.Base), 1f));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"Failed to apply Speed Hack ~ {ex}");
                    }

                    if (writes.Count > 0)
                        Memory.WriteScatter(writes);
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[Toolkit] -> Toolkit exception: {ex}");
                }

                Thread.Sleep(30);
            }
        }

        private static void Realtime()
        {
            while (true)
            {
                if (!InMatch)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var players = Players.Values.ToArray();

                    if (players.Length == 0)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    var scatterMap = new ScatterReadMap(players.Length);
                    var round1 = scatterMap.AddRound(false);

                    int BonesCount = Player.BonesCount;

                    for (int i = 0; i < players.Length; i++)
                    {
                        Player player = players[i];

                        int currentScatterId = 0;

                        for (int ii = 0; ii < BonesCount; ii++)
                            round1.AddEntry<SharedContainer<Vector128<float>>>(i, currentScatterId++, player.Bones[ii].VerticesAddr, (3 * player.Bones[ii].HierarchyIndex + 3) * 16);

                        round1.AddEntry<ulong>(i, currentScatterId++, player.CorpseAddress);

                        round1.AddEntry<Vector3>(i, currentScatterId++, player.VelocityAddress);
                    }

                    scatterMap.Execute();

                    for (int i = 0; i < players.Length; i++)
                    {
                        Player player = players[i];

                        int currentScatterId = 0;

                        try
                        {
                            for (int ii = 0; ii < BonesCount; ii++)
                            {
                                if (scatterMap.Results[i][currentScatterId++].TryGetResult<SharedContainer<Vector128<float>>>(out var vertices))
                                {
                                    using (vertices)
                                    {
                                        player.BonePositions[ii] = player.Bones[ii].GetPosition(vertices);
                                    }
                                }
                            }

                            if (scatterMap.Results[i][currentScatterId++].TryGetResult<ulong>(out var corpse))
                            {
                                bool IsAlive = corpse == 0x0;

                                if (Memory.CanWrite)
                                {
                                    if (!IsAlive && player.IsAlive != IsAlive)
                                    {
                                        // Player just died, apply corpse chams.
                                        // TODO: run async but prevent race conditions
                                        player.SetChams(ChamsManager.CorpseMaterial.InstanceID);
                                        
                                        if (player.IsLocal)
                                            Memory.CanWrite = false;
                                    }

                                    if (IsAlive && player.ChamsDirty)
                                    {
                                        player.ChamsDirty = false;

                                        // Aimbot was just disengaged. Apply normal chams.
                                        // TODO: run async but prevent race conditions
                                        player.SetChams();
                                    }
                                }

                                player.IsAlive = IsAlive;
                            }
                            else // If we fail to get the corpse addr, mark as dead
                            {
                                player.IsAlive = false;
                            }

                            if (LocalPlayer != null)
                                player.Distance = Vector3.Distance(LocalPlayer.Position, player.Position);
                            else
                                player.Distance = 0f;

                            if (scatterMap.Results[i][currentScatterId++].TryGetResult<Vector3>(out var velocity))
                                player.Velocity = velocity;
                        }
                        catch { player.EPurgeStatus = Player.PurgeStatus.Queued; } // Queue for purging on error
                        finally
                        {
                            if (player.EPurgeStatus == Player.PurgeStatus.Queued)
                                player.EPurgeStatus = Player.PurgeStatus.CanPurge;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[GAME] -> Realtime(): Error: {ex}");
                }

                Thread.Sleep(8);
            }
        }
    }
}
