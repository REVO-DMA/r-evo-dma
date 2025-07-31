using gzw_dma_backend.DMA.ScatterAPI;
using gzw_dma_backend.GZW.ESP;
using System.Collections.Concurrent;

namespace gzw_dma_backend.GZW
{
    public static class Manager
    {
        public static readonly ConcurrentDictionary<ulong, Player> Players = new();
        public static Player LocalPlayer { get; set; } = null;

        // Local Player Stuff
        public static ulong LocalPlayerBase { get; private set; }
        public static ulong LocalPlayerController { get; private set; }
        public static ulong LocalPlayerCameraManager { get; private set; }
        public static ulong LocalPlayerState { get; private set; }

        public static ulong ActorsBase {  get; private set; }
        public static uint ActorsCount { get; private set; }

        private static ulong _updateIteration = 0;

        private static readonly Thread _t1;
        private static readonly Thread _t2;
        private static readonly Thread _t3;

        static Manager()
        {
            _t1 = new Thread(Realtime)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _t1.Start();

            _t2 = new Thread(Aimbot)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _t2.Start();

            _t3 = new Thread(Toolkit)
            {
                IsBackground = true,
            };
            _t3.Start();
        }

        public static void UpdateInfo()
        {
            try
            {
                ulong world = Memory.ReadPtr(Memory.ModuleBase + Offsets.World);
                ulong gameInstance = Memory.ReadPtr(world + Offsets.UWorld.OwningGameInstance);
                ulong localPlayers = Memory.ReadPtr(gameInstance + Offsets.UGameInstance.LocalPlayers);
                LocalPlayerBase = Memory.ReadPtr(localPlayers + 0x0); // First item in tarray
            }
            catch (Exception ex)
            {
                LocalPlayerBase = 0x0;
                Logger.WriteLine($"[MANAGER] -> UpdateInfo(): Failed to get Local Player ~ {ex}");
            }

            try
            {
                LocalPlayerController = Memory.ReadPtr(LocalPlayerBase + Offsets.UPlayer.PlayerController);
                LocalPlayerCameraManager = Memory.ReadPtr(LocalPlayerController + Offsets.APlayerController.PlayerCameraManager);

                ulong acknowledgedPawn = Memory.ReadPtr(LocalPlayerController + Offsets.APlayerController.AcknowledgedPawn);
                LocalPlayerState = Memory.ReadPtr(acknowledgedPawn + Offsets.APawn.PlayerState);

                ulong viewportClient = Memory.ReadPtr(LocalPlayerBase + Offsets.ULocalPlayer.ViewportClient);
                ulong vcWorld = Memory.ReadPtr(viewportClient + Offsets.UGameViewportClient.World);
                ulong persistentLevel = Memory.ReadPtr(vcWorld + Offsets.UWorld.PersistentLevel);

                ActorsBase = Memory.ReadPtr(persistentLevel + Offsets.ULevel.Actors);

                ActorsCount = Memory.ReadValue<uint>(persistentLevel + Offsets.ULevel.ActorsCount);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[MANAGER] -> UpdateInfo(): {ex}");
            }
        }

        public static void Update()
        {
            _updateIteration++;

            var actorList = Actors.GetList(ActorsBase, (int)ActorsCount);
            var processedActors = Actors.ProcessList(actorList);

            foreach (var actor in processedActors)
            {
                try
                {
                    Player newPlayer = new(actor, _updateIteration);
                    Players.AddOrUpdate(actor.Address, (newItem) => newPlayer, (key, existing) =>
                    {
                        newPlayer.IsDead = existing.IsDead;
                        newPlayer.Health = existing.Health;
                        newPlayer.Distance = existing.Distance;
                        newPlayer.BonePositions = existing.BonePositions;

                        newPlayer.IsScoped = existing.IsScoped;
                        newPlayer.IsADS = existing.IsADS;

                        return newPlayer;
                    });
                }
                catch { }
            }

            // Purge old Players
            try
            {
                foreach (var player in Players)
                {
                    if (_updateIteration > player.Value.UpdateIteration + 3)
                        Players.TryRemove(player);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[MANAGER] -> Purge Old Players Exception: {ex}");
            }
        }

        private static Vector3D<double> ToRotator(Vector3D<double> local, Vector3D<double> target)
        {
            double radToUnrRot = 57.2957795d;

            Vector3D<double> diff = target - local;

            double X = Math.Atan2(diff.Z, Math.Sqrt((diff.X * diff.X) + (diff.Y * diff.Y))) * radToUnrRot;
            double Y = Math.Atan2(diff.Y, diff.X) * radToUnrRot;
            double Z = 0d;

            return new Vector3D<double>(X, Y, Z);
        }

        private static void Aimbot()
        {
            while (true)
            {
                if (LocalPlayer == null || !Program.UserConfig.MemoryAimbot)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    Vector2 centerScreen = ESP_Config.ScreenCenter;
                    Vector3D<double> closestHead = new();
                    float closestDistance = float.MaxValue;
                    bool foundTarget = false;
                    foreach (var rawPlayer in Players)
                    {
                        var player = rawPlayer.Value;

                        if (!player.ShouldRender())
                            continue;

                        ESP_Utilities.W2S(player.BonePositions[6], out var headPosition);

                        float crosshairDistance = Vector2.Distance(headPosition, centerScreen);

                        if (crosshairDistance < closestDistance)
                        {
                            closestDistance = crosshairDistance;
                            closestHead = player.BonePositions[6];
                            foundTarget = true;
                        }
                    }

                    if (LocalPlayer == null)
                    {
                        Thread.Sleep(16);
                        continue;
                    }

                    if (foundTarget && LocalPlayer != null && LocalPlayer.IsADS)
                    {
                        //var cameraLocation = ESP_Utilities.CameraInfo.Location;
                        var cameraLocation = LocalPlayer.BonePositions[6];

                        Memory.WriteValue(LocalPlayerController + Offsets.AController.ControlRotation, ToRotator(cameraLocation, closestHead));
                    }
                }
                catch { }

                Thread.Sleep(8);
            }
        }

        private static void Toolkit()
        {
            while (true)
            {
                if (LocalPlayer == null)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    if (Program.UserConfig.SpeedHack)
                        Memory.WriteValue(LocalPlayer.Base + Offsets.AActor.CustomTimeDilation, (float)Program.UserConfig.SpeedMultiplier);
                    else
                        Memory.WriteValue(LocalPlayer.Base + Offsets.AActor.CustomTimeDilation, 1f);

                    if (Program.UserConfig.InfiniteSprint)
                        Memory.WriteValue(LocalPlayer.CachedMovementComponent + Offsets.UMFGMovementComponent.CurrentMovementType, (byte)Player.MovementType.Sprinting);

                    var bReplicateMovementBitfield = Memory.ReadValue<byte>(LocalPlayer.Base + Offsets.AActor.bReplicateMovementBitfield, false);
                    if (Program.UserConfig.GodMode)
                        Memory.WriteValue(LocalPlayer.Base + Offsets.AActor.bReplicateMovementBitfield, UnsetBit(bReplicateMovementBitfield, Offsets.AActor.bReplicateMovementBitOffset));
                    else
                        Memory.WriteValue(LocalPlayer.Base + Offsets.AActor.bReplicateMovementBitfield, SetBit(bReplicateMovementBitfield, Offsets.AActor.bReplicateMovementBitOffset));
                }
                catch { }

                Thread.Sleep(8);
            }
        }

        public static byte SetBit(byte bitfield, int offset)
        {
            byte mask = (byte)(1 << offset);
            return (byte)(bitfield | mask);
        }

        public static byte UnsetBit(byte bitfield, int offset)
        {
            byte mask = (byte)(~(1 << offset));
            return (byte)(bitfield & mask);
        }

        private static void Realtime()
        {
            while (true)
            {
                try
                {
                    var players = Players.ToArray();

                    var map = new ScatterReadMap(players.Length);
                    var round1 = map.AddRound(false);

                    for (int i = 0; i < players.Length; i++)
                    {
                        var player = players[i].Value;

                        int currentScatterId = 0;

                        // LocalPlayer only
                        if (player.IsLocalPlayer)
                        {
                            round1.AddEntry<bool>(i, currentScatterId++, player.Base + Offsets.AMFGGameCharacter.WantsAiming);

                            round1.AddEntry<byte>(i, currentScatterId++, player.ScopeRenderComponent + Offsets.USceneComponent.Bitfield);
                            round1.AddEntry<float>(i, currentScatterId++, player.ScopeRenderComponent + Offsets.USceneCaptureComponent2D.FOVAngle);
                        }

                        round1.AddEntry<bool>(i, currentScatterId++, player.Base + Offsets.AMFGGameCharacter.IsDead);

                        round1.AddEntry<byte>(i, currentScatterId++, player.HealthSystemComponent + Offsets.UMFGHealthSystem.OverallHealthStatus);

                        round1.AddEntry<UE_Math.FTransform>(i, currentScatterId++, player.ComponentToWorld);

                        for (int ii = 0; ii < Player.BonesCount; ii++)
                        {
                            round1.AddEntry<UE_Math.FTransform>(i, currentScatterId++, player.BoneArray + ((uint)Player.BoneIndices[ii] * 0x60));
                        }
                    }

                    // Execute scatter Read
                    map.Execute();

                    for (int i = 0; i < players.Length; i++)
                    {
                        var player = players[i].Value;

                        int currentScatterId = 0;

                        // LocalPlayer only
                        if (player.IsLocalPlayer)
                        {
                            if (map.Results[i][currentScatterId++].TryGetResult<bool>(out var isADS) &&
                                map.Results[i][currentScatterId++].TryGetResult<byte>(out var bitfield))
                            {
                                bool isScope = ((bitfield >> Offsets.USceneComponent.bVisible) & 1) == 1;

                                if (isADS && isScope)
                                    player.IsScoped = isADS;
                                else
                                    player.IsScoped = false;

                                player.IsADS = isADS;
                            }

                            if (map.Results[i][currentScatterId++].TryGetResult<float>(out var scopeFOV) && player.IsScoped)
                                ESP_Utilities.ScopeFOV = scopeFOV;
                            else
                                ESP_Utilities.ScopeFOV = -1f;
                        }

                        if (map.Results[i][currentScatterId++].TryGetResult<bool>(out var isDead))
                            player.IsDead = isDead;

                        if (map.Results[i][currentScatterId++].TryGetResult<byte>(out var overallHealthStatus))
                            player.Health = (Player.HealthStatus)overallHealthStatus;

                        if (map.Results[i][currentScatterId++].TryGetResult<UE_Math.FTransform>(out var c2w))
                        {
                            player.C2W = c2w.ToMatrixWithScale();

                            for (int ii = 0; ii < Player.BonesCount; ii++)
                            {
                                if (map.Results[i][currentScatterId++].TryGetResult<UE_Math.FTransform>(out var boneTransform))
                                {
                                    player.SetBonePosition(boneTransform.ToMatrixWithScale(), ii);
                                }
                            }

                            player.Distance = (int)(Vector3D.Distance(LocalPlayer.Position, player.Position) / 100d);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MANAGER] -> Realtime() Exception: {ex}");
                }

                // Update @ approx. 120 FPS
                Thread.Sleep(8);
            }
        }
    }
}
