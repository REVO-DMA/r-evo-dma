using eft_dma_radar.MemDMA.ScatterAPI;
using pubg_dma_esp.ESP;

namespace pubg_dma_esp.PUBG
{
    public static class Manager
    {
        public static ConcurrentDictionary<ulong, Player> Players { get; private set; } = new();
        public static ConcurrentDictionary<ulong, Vehicle> Vehicles { get; private set; } = new();
        
        public static bool IsInMatch { get; private set; }

        private static ulong _updateIteration = 0;

        public static class Fields
        {
            public static ulong GNames { get; set; }
            public static ulong GWorld { get; set; }
            public static ulong CurrentLevel { get; set; }
            public static ulong GameInstance { get; set; }
            public static ulong LocalPlayer { get; set; }
            public static ulong Actors { get; set; }
            public static ulong ActorsDeref { get; set; }
            public static int ActorsSize { get; set; }
        }

        private static readonly Thread _t1;
        private static readonly Thread _t2;

        static Manager()
        {
            _t1 = new Thread(Realtime)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _t2 = new Thread(Deferred)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };

            _t1.Start();
            _t2.Start();
        }

        public static void UpdateBaseInfo()
        {
            GNames.Get();

            GWorld.Get();
            GWorld.GetCurrentLevel();
            GWorld.GetGameInstance();
            GWorld.GetLocalPlayers();

            Actors.Get();
            Actors.GetSize();

            if (Players.IsEmpty)
            {
                IsInMatch = false;
                _updateIteration = 0;

                Logger.WriteLine("[MANAGER] -> UpdateBaseInfo(): No match detected.");
            }
            else
            {
                IsInMatch = true;
                Logger.WriteLine($"Players Count: {Players.Count}");
            }

            LocalPlayer.Update(Fields.LocalPlayer);
        }

        public static void UpdateActors()
        {
            _updateIteration++;

            var actors = Actors.GetList();
            var processedActors = Actors.ProcessList(actors);

            // Update all Vehicles
            foreach (var vehicle in processedActors.Vehicles)
            {
                Vehicle newVehicle = new(vehicle.Address, vehicle.RootComponent, vehicle.Name, _updateIteration);
                Vehicles.AddOrUpdate(vehicle.Address, (newItem) => newVehicle, (key, existing) =>
                {
                    newVehicle.Position = existing.Position;
                    newVehicle.Distance = existing.Distance;

                    return newVehicle;
                });
            }

            // Update all Players
            foreach (var player in processedActors.Players)
            {
                Player newPlayer = new(player.Address, player.RootComponent, player.Mesh, player.BoneArray, player.Name, player.Team, player.IsHuman, _updateIteration);
                Players.AddOrUpdate(player.Address, (newItem) => newPlayer, (key, existing) =>
                {
                    newPlayer.BonePositions = existing.BonePositions;
                    newPlayer.Position = existing.Position;
                    newPlayer.Distance = existing.Distance;

                    newPlayer.Health = existing.Health;
                    newPlayer.GroggyHealth = existing.GroggyHealth;
                    newPlayer.Knocked = existing.Knocked;

                    newPlayer.IsVisible = existing.IsVisible;
                    newPlayer.ShouldRender = existing.ShouldRender;

                    return newPlayer;
                });
            }

            // Purge old Vehicles
            try
            {
                foreach (var vehicle in Vehicles)
                {
                    if (vehicle.Value.UpdateIteration != _updateIteration)
                        Vehicles.TryRemove(vehicle);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[MANAGER] -> Purge Old Vehicles Exception: {ex}");
            }

            // Purge old Players
            try
            {
                foreach (var player in Players)
                {
                    if (player.Value.UpdateIteration != _updateIteration)
                        Players.TryRemove(player);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[MANAGER] -> Purge Old Players Exception: {ex}");
            }
        }

        private static void Realtime()
        {
            while (true)
            {
                if (!IsInMatch)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var players = Players.ToArray();

                    var vehicles = Vehicles.ToArray();

                    Vector3 cameraLocation = ESP_Utilities.CameraInfo.Location;

                    var map = new ScatterReadMap();
                    var round1 = map.AddRound(false);

                    // Add Players
                    for (int ix = 0; ix < players.Length; ix++)
                    {
                        int i = ix;
                        var player = players[i].Value;

                        if (player is null || player.IsLocalPlayer)
                            continue;

                        /// Vis check
                        // LastSubmitTime
                        round1[i].AddEntry<float>(0, player.Mesh + Offsets.Mesh.LastSubmitTime);
                        // LastRenderTimeOnScreen
                        round1[i].AddEntry<float>(1, player.Mesh + Offsets.Mesh.LastRenderTimeOnScreen);

                        // Location
                        round1[i].AddEntry<Vector3>(2, player.RootComponent + (Offsets.SceneComponent.ComponentToWorld + Offsets.SceneComponent.Translation));
                        // Mesh C2W (component to world)
                        round1[i].AddEntry<UE_Math.FTransform>(3, player.Mesh + Offsets.SceneComponent.ComponentToWorld);

                        // Get all bone positions
                        for (int iix = 0; iix < Player.BonesCount; iix++)
                        {
                            int ii = iix;
                            round1[i].AddEntry<UE_Math.FTransform>(10000 + ii, player.GetBoneIndexAddress(Player.BoneIndices[ii]));
                        }

                        round1[i].Callback = (index) =>
                        {
                            if (index.TryGetResult<float>(0, out var lastSubmitTime) && // Vis Check
                            index.TryGetResult<float>(1, out var lastRenderTime))
                            {
                                player.IsVisible = lastRenderTime + Player.VisionTick >= lastSubmitTime;
                            }
                            if (index.TryGetResult<Vector3>(2, out var location)) // Location
                            {
                                player.SetPosition(location);
                                
                                if (Offsets.Util.PositionDebug)
                                    Logger.WriteLine($"{player.Name} -> {player.Position}");
                            }
                            if (index.TryGetResult<UE_Math.FTransform>(3, out var c2w)) // C2W
                            {
                                player.C2W = c2w.ToMatrixWithScale();
                                for (int iix = 0; iix < Player.BonesCount; iix++) // Bone Positions
                                {
                                    int ii = iix;
                                    if (index.TryGetResult<UE_Math.FTransform>(10000 + ii, out var boneTransform))
                                    {
                                        Vector3 bonePos = player.GetBonePosition(boneTransform);
                                        player.SetBonePosition(ii, bonePos);
                                    }
                                }
                            }
                            else
                                Logger.WriteLine($"[MANAGER] Error getting C2W!");

                            player.Distance = (int)(Vector3.Distance(cameraLocation, player.Position) / 100f);

                            player.SetShouldRender();
                        };
                    }

                    // Add Vehicles
                    for (int ix = 0; ix < vehicles.Length; ix++)
                    {
                        int i = ix;
                        var vehicle = vehicles[i].Value;

                        if (vehicle is null)
                            continue;

                        // Location
                        round1[i + 10000].AddEntry<Vector3>(0, vehicle.RootComponent + (Offsets.SceneComponent.ComponentToWorld + Offsets.SceneComponent.Translation));

                        round1[i + 10000].Callback = (index) =>
                        {
                            if (index.TryGetResult<Vector3>(0, out var location))
                                vehicle.SetPosition(location);
                            vehicle.Distance = (int)(Vector3.Distance(cameraLocation, vehicle.Position) / 100f);
                        };
                    }
                    // Execute scatter Read
                    map.Execute();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MANAGER] -> Realtime() Exception: {ex}");
                }

                // Update @ approx. 120 FPS
                Thread.Sleep(8);
            }
        }

        private static void Deferred()
        {
            while (true)
            {
                if (!IsInMatch)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var players = Players.ToArray();
                    var map = new ScatterReadMap();
                    var round1 = map.AddRound(false);

                    for (int i = 0; i < players.Length; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        // Health
                        round1[i].AddEntry<float>(0, player.HealthAddress);
                        // GroggyHealth
                        round1[i].AddEntry<float>(1, player.Base + Offsets.Character.GroggyHealth);

                        // SpectatedCount (LocalPlayer only)
                        if (player.IsLocalPlayer)
                            round1[i].AddEntry<int>(2, player.Base + Offsets.Character.SpectatedCount);

                        round1[i].Callback = (index) =>
                        {
                            if (index.TryGetResult<float>(0, out var healthRaw))
                            {
                                float health;

                                if (player.HealthNeedsDecryption)
                                {
                                    Span<byte> healthSpan = BitConverter.GetBytes(healthRaw).AsSpan();
                                    health = player.DecryptHealth(healthSpan);
                                }
                                else
                                    health = healthRaw;

                                player.Health = health;

                                // Set knocked status
                                if (player.Health > 0f)
                                    player.Knocked = false;
                                else
                                    player.Knocked = true;
                            }

                            if (index.TryGetResult<float>(1, out var groggyHealth))
                                player.GroggyHealth = groggyHealth;
                            if (player.IsLocalPlayer && index.TryGetResult<int>(2, out var spectatedCount))
                                LocalPlayer.SpectatorCount = spectatedCount;
                        };
                    }
                    // Execute Scatter Read
                    map.Execute();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MANAGER] -> Deferred() Exception: {ex}");
                }

                Thread.Sleep(100);
            }
        }
    }
}
