using arena_dma_backend.ESP;
using arena_dma_backend.Misc;

namespace arena_dma_backend.Arena
{
    public static class Aimbot
    {
        private static Thread _tMain;

        public static bool Engaged { get; set; }
        public static Vector3 SilentAimPosition { get; set; }
        public static bool SilentAimClean { get; set; }

        public static void Start()
        {
            _tMain = new(Main)
            {
                IsBackground = true,
            };

            _tMain.Start();
        }

        private static void Main()
        {
            while (true)
            {
                if (!Memory.CanWrite || !Engaged)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    bool forceUnlock = false;

                    ulong targetAddress = GetTarget();
                    if (targetAddress == 0x0) continue;

                    ulong saGetter = AimbotPrewarmer.WeaponDirectionGetter;
                    if (saGetter == 0x0) continue;

                    try
                    {
                        if (!Game.Players.TryGetValue(targetAddress, out Player target)) continue;

                        target.IsAimbotLocked = true;

                        Task.Run(() =>
                        {
                            target.SetChams(ChamsManager.AimbotMaterial.InstanceID);
                        });

                        using SilentAim sa = new(saGetter, targetAddress);
                        while (Memory.CanWrite && Engaged)
                        {
                            if (!sa.Update()) break;

                            ulong newTargetAddress = GetTarget();
                            if (newTargetAddress == 0x0)
                            {
                                forceUnlock = true;
                                break;
                            }
                            else sa.UpdateTarget(newTargetAddress);

                            Thread.Sleep(2);
                        }
                    }
                    finally
                    {
                        if (Game.Players.TryGetValue(targetAddress, out Player finalTarget))
                        {
                            finalTarget.IsAimbotLocked = false;
                            finalTarget.ChamsDirty = true;
                            if (!forceUnlock) Engaged = false;
                        }

                        // Trap the loop until this is clean
                        while (!SilentAimClean)
                        {
                            SilentAimClean = SilentAim.RestoreWeaponDirectionGetter(saGetter);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[AIMBOT] -> Main(): {ex}");
                }
            }
        }

        private readonly struct AimbotTarget(KeyValuePair<ulong, Player> playerEntry, Vector2[] bones)
        {
            public readonly KeyValuePair<ulong, Player> PlayerEntry = playerEntry;
            public readonly Vector2[] Bones = bones;
        }

        private static ulong GetTarget()
        {
            try
            {
                Player LocalPlayer = Game.LocalPlayer;
                var Players = Game.Players;

                if (LocalPlayer == null) return 0x0;
                if (Players == null) return 0x0;

                if (!Program.UserConfig.EnableESP) ESP_Utilities.UpdateW2S();

                int playersLength = Players.Count;
                int[] BoneLinkIndices = Player.BoneLinkIndices;
                AimbotTarget[] aimbotTargets = new AimbotTarget[playersLength];
                bool[] skipIndex = new bool[playersLength];

                // Loop through all players and generate targeting information
                LoopIndexer i = new(-1);
                foreach (var pEntry in Players)
                {
                    Player player = pEntry.Value;

                    i.Inc();

                    if (!player.CanRender() || player.IsTeammate)
                    {
                        skipIndex[i] = true;
                        continue;
                    }

                    Vector3[] BonePositions = player.BonePositions;
                    Vector2[] espPositions = new Vector2[BoneLinkIndices.Length];
                    LoopIndexer ii = new(-1);
                    // Get all bone screen positions
                    foreach (var boneIndex in BoneLinkIndices)
                    {
                        ii.Inc();

                        // Get bone screen position
                        if (!ESP_Utilities.W2S(BonePositions[boneIndex], false, out var espPosition)) continue;

                        espPositions[ii] = espPosition;
                    }

                    // Add player to targets array
                    aimbotTargets[i] = new(pEntry, espPositions);
                }

                i.Reset();
                AimbotTarget closestTarget = default;
                float closestDistance = float.MaxValue;
                // Get the closest AimbotTarget based on configured targeting mode
                foreach (AimbotTarget aimbotTarget in aimbotTargets)
                {
                    i.Inc();

                    if (skipIndex[i]) continue;

                    LoopIndexer ii = new(-1);
                    foreach (Vector2 bone in aimbotTarget.Bones)
                    {
                        ii.Inc();

                        float crosshairDistance = Vector2.Distance(aimbotTarget.Bones[ii], ESP_Config.ScreenCenter);

                        if (crosshairDistance > Program.UserConfig.AimFOV) continue;

                        if (Program.UserConfig.CQB_Targeting)
                        {
                            // Check if the distance is closer than the current closest target
                            float playerDistance = aimbotTarget.PlayerEntry.Value.Distance;

                            if (playerDistance < closestDistance)
                            {
                                closestDistance = playerDistance;
                                closestTarget = aimbotTarget;
                            }
                        }
                        else
                        {
                            // Check if the crosshair distance is closer than the current closest target
                            if (crosshairDistance < closestDistance)
                            {
                                closestDistance = crosshairDistance;
                                closestTarget = aimbotTarget;
                            }
                        }
                    }
                }

                return closestTarget.PlayerEntry.Key;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[AIMBOT] -> GetTarget(): {ex}");
            }

            return 0x0;
        }
    }
}
