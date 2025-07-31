using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Ballistics;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;
using static Tarkov_DMA_Backend.Tarkov.Toolkit.Features.AimbotSettings;

namespace Tarkov_DMA_Backend.Tarkov
{
    public static class Aimbot
    {
        // Aimbot Settings
        public static bool Enabled = false;
        public static bool Engaged = false;
        public static bool AlwaysOn = false;
        public static bool Dirty = false;
        public static float MaxDistance = 450f;
        public static float FOV = 30f;
        public static bool TargetingVisibilityCheck = true;
        public static bool TargetTeammates = false;
        public static int TargetingMode = 1;

        /// <summary>
        /// Address of the getter method for WeaponDirection. This method returns the weapon direction in world space. This is byte patched to allow silent aim to work.
        /// </summary>
        public static ulong WeaponDirectionGetter = 0x0;

        private static ulong LastHandsVersion = ulong.MaxValue;
        private static int PositionArrayIndex = -1;

        // Misc
        public static Player LockedPlayer = null;
        public static Vector3 LastTargetPosition = Vector3.Zero;

        /// <summary>
        /// The original bytes of the WeaponDirection getter method.
        /// </summary>
        private static readonly byte[] WeaponDirectionGetterOriginalBytes = new byte[]
        {
            0x55,                    					// push rbp
            0x48, 0x8B, 0xEC,              				// mov rbp,rsp
            0x48, 0x81, 0xEC, 0x90, 0x00, 0x00, 0x00,   // sub rsp,00000090
            0x48, 0x89, 0x7D, 0xF8,           			// mov [rbp-08],rdi
            0x48, 0x89, 0x55, 0xF0,           			// mov [rbp-10],rdx
            0x48, 0x8B, 0xF9,              				// mov rdi,rcx
            0x49, 0xBB, 								// mov r11
        };
        /// <summary>
        /// The silent aim bytes of the WeaponDirection getter method.
        /// </summary>
        private static readonly byte[] WeaponDirectionGetterSilentBytes = new byte[]
        {
            0xC7, 0x02, // mov [rdx], xBytes
            0x0, 0x0, 0x0, 0x0, // X

            0xC7, 0x42, 0x04, // mov [rdx+4], yBytes
            0x0, 0x0, 0x0, 0x0, // Y

            0xC7, 0x42, 0x08, // mov [rdx+8], zBytes
            0x0, 0x0, 0x0, 0x0, // Z

            0x48, 0x89, 0xD0, // mov rax, rdx

            0xC3 // ret
        };

        public static void SetAimbot()
        {
            try
            {
                Player localPlayer = EFTDMA.LocalPlayer;
                if (localPlayer == null)
                {
                    ResetAimbot();
                    return;
                }

                if (WeaponDirectionGetter == 0x0)
                {
                    GetWeaponDirectionGetter();
                    return;
                }

                // Update target and bone
                Player newTarget = GetBestAimbotTarget();
                bool gotNewTarget = false;
                if (LockedPlayer != newTarget)
                {
                    gotNewTarget = true;

                    // Restore chams on last player
                    LockedPlayer?.SetAimbotChams(true);
                }

                // Update targeting data if the target or weapon changed
                if (LastHandsVersion != LocalHandsManager.HandsVersion ||
                    PositionArrayIndex == -1 ||
                    gotNewTarget)
                {
                    PositionArrayIndex = GetPositionArrayIndex(newTarget);
                    
                    LastHandsVersion = LocalHandsManager.HandsVersion;

                    LockedPlayer = newTarget;

                    // Apply aimbot chams on new player
                    LockedPlayer?.SetAimbotChams(false);
                }

                if (LockedPlayer == null)
                {
                    ResetAimbot();
                    return;
                }

                AimbotExecute(localPlayer);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Aimbot [FAIL] {ex}");
                ResetAimbot();
            }
        }

        public static bool ResetAimbot()
        {
            // Restore chams on last player
            LockedPlayer?.SetAimbotChams(true);

            LockedPlayer = null;
            LastTargetPosition = Vector3.Zero;
            LastHandsVersion = ulong.MaxValue;
            PositionArrayIndex = -1;

            return RestoreWeaponDirectionGetter();
        }

        private readonly struct AimbotTarget(Player player, Vector2[] bones)
        {
            public readonly Player Player = player;
            public readonly Vector2[] Bones = bones;
        }

        private static int GetPositionArrayIndex(Player player)
        {
            if (player == null)
                return -1;

            Dictionary<Bone, HitboxSettings> settings;
            if (player.IsAI)
                settings = AIHitboxSettings;
            else
            {
                settings = PlayerHitboxSettings.Where(x =>
                {
                    if (!x.Value.SmartTargeting)
                        return true;

                    int boneIndex = Array.IndexOf(Player.BoneIndices, (int)x.Key);

                    int visCheckIndex = Player.BoneIndexToVisCheckIndex(Player.BoneIndices[boneIndex]);
                    if (visCheckIndex == -1)
                        return false;

                    return player.VisibilityInfo[visCheckIndex];
                }).ToDictionary(x => x.Key, x => x.Value);
            }

            float totalChance = settings.Values.Sum(x => x.Chance);
            float randomValue = (float)(Random.Shared.NextDouble() * totalChance);

            float cumulative = 0f;
            Bone chosenBone = Bone.None;
            foreach (var item in settings)
            {
                cumulative += item.Value.Chance;
                if (randomValue < cumulative)
                {
                    chosenBone = item.Key;
                    break;
                }
            }

            int arrayIndex = Array.IndexOf(Player.BoneIndices, (int)chosenBone);
            return arrayIndex;
        }

        private static Player GetBestAimbotTarget()
        {
            Player localPlayer = EFTDMA.LocalPlayer;
            Player[] players = EFTDMA.DisplayPlayers;

            if (localPlayer == null || players == null)
                return null;

            int playersLength = players.Length;
            Span<int> boneLinkIndices = Player.BoneLinkIndices.AsSpan();
            List<AimbotTarget> aimbotTargets = new();

            // Loop through all players and generate targeting information
            for (int i = 0; i < playersLength; i++)
            {
                Player player = players[i];

                if (player == null ||
                    !player.IsActive ||
                    !player.IsAlive ||
                    player.Distance > MaxDistance ||
                    (!TargetTeammates && player.Type == PlayerType.Teammate) ||
                    (TargetingVisibilityCheck && !player.IsVisible))
                {
                    continue;
                }

                Vector3[] BonePositions = player.BonePositions;
                Vector2[] espPositions = new Vector2[boneLinkIndices.Length];
                // Get all bone screen positions
                for (int ii = 0; ii < boneLinkIndices.Length; ii++)
                {
                    int boneIndex = boneLinkIndices[ii];

                    // Get bone screen position
                    if (!ESP_Utilities.W2S(BonePositions[boneIndex], out Vector2 espPosition, false)) continue;

                    espPositions[ii] = espPosition;
                }

                // Add player to targets array
                aimbotTargets.Add(new(player, espPositions));
            }

            Vector2 centerScreen = new(ESP_Config.ResolutionX / 2, ESP_Config.ResolutionY / 2);
            Result<AimbotTarget> closestTarget = Result<AimbotTarget>.Fail;
            float closestDistance = float.MaxValue;
            // Get the closest AimbotTarget based on configured targeting mode
            foreach (AimbotTarget aimbotTarget in aimbotTargets)
            {
                int bonesCount = aimbotTarget.Bones.Length;
                for (int ii = 0; ii < bonesCount; ii++)
                {
                    float crosshairDistance = Vector2.Distance(aimbotTarget.Bones[ii], centerScreen);

                    if (crosshairDistance > FOV) continue;

                    if (TargetingMode == 1) // Smart
                    {
                        if (LocalHandsManager.IsAiming) // Crosshair
                        {
                            if (crosshairDistance < closestDistance)
                            {
                                closestDistance = crosshairDistance;
                                closestTarget = new(true, aimbotTarget);
                            }
                        }
                        else // CQB
                        {
                            float playerDistance = aimbotTarget.Player.Distance;

                            if (playerDistance < closestDistance)
                            {
                                closestDistance = playerDistance;
                                closestTarget = new(true, aimbotTarget);
                            }
                        }
                    }
                    else if (TargetingMode == 2) // CQB
                    {
                        float playerDistance = aimbotTarget.Player.Distance;

                        if (playerDistance < closestDistance)
                        {
                            closestDistance = playerDistance;
                            closestTarget = new(true, aimbotTarget);
                        }
                    }
                    else if (TargetingMode == 3) // Crosshair
                    {
                        if (crosshairDistance < closestDistance)
                        {
                            closestDistance = crosshairDistance;
                            closestTarget = new(true, aimbotTarget);
                        }
                    }
                }
            }

            if (closestTarget)
                return closestTarget.Value.Player;
            else
                return null;
        }

        private static void AimbotExecute(Player localPlayer)
        {
            if (PositionArrayIndex == -1)
                return;

            Vector3 fireportPosition = VC_Structs.Vector3.ToSystem(ESP_Utilities.LastESPData.firePortPos);
            if (fireportPosition == Vector3.Zero)
                return;

            Vector3 bonePosition = LockedPlayer.BonePositions[PositionArrayIndex];

            Vector3 targetVelocity = Memory.ReadValue<Vector3>(LockedPlayer.VelocityAddress, false);

            BallisticSimulationOutput? simulationResult = null;

            float shotDistance = Vector3.Distance(fireportPosition, bonePosition);
            if (shotDistance >= 5f)
            {
                var ammoData = LocalHandsManager.WeaponAmmoData;

                if (ammoData.IsOK())
                    simulationResult = BallisticsSimulation.Run(fireportPosition, bonePosition, LocalHandsManager.WeaponAmmoData);
                else
                    Logger.WriteLine($"[AIMBOT] Error: Invalid Ammo Ballistics! Running without prediction");
            }

            Vector3 newWeaponDirection = FormSilentTrajectory(fireportPosition, bonePosition, targetVelocity, simulationResult);
            PatchWeaponDirectionGetter(newWeaponDirection);

            LastTargetPosition = bonePosition;
        }

        public static void GetWeaponDirectionGetter()
        {
            try
            {
                var fClass = EFTDMA.MonoClasses.GetClass(ClassNames.FirearmController.ClassName_ClassToken);

                ulong fMethod = (ulong)fClass.FindMethod("get_WeaponDirection");
                if (fMethod == 0x0)
                    throw new Exception("Unable to find the EFT.Player+FirearmController:get_WeaponDirection method!");

                WeaponDirectionGetter = NativeHelper.CompileMethod(fMethod);
                if (WeaponDirectionGetter == 0x0)
                    throw new Exception("Unable to compile the EFT.Player+FirearmController:get_WeaponDirection method!");
            }
            catch (Exception ex)
            {
                WeaponDirectionGetter = 0x0;
                Logger.WriteLine($"[AIMBOT] Error getting WeaponDirection getter method address: {ex}");
            }
        }

        private static void PatchWeaponDirectionGetter(Vector3 newWeaponDirection)
        {
            if (WeaponDirectionGetter == 0x0)
                return;

            byte[] xBytes = BitConverter.GetBytes(newWeaponDirection.X);
            byte[] yBytes = BitConverter.GetBytes(newWeaponDirection.Y);
            byte[] zBytes = BitConverter.GetBytes(newWeaponDirection.Z);

            byte[] silentBytes = new byte[]
            {
                0xC7, 0x02, // mov [rdx], xBytes
                xBytes[0], xBytes[1], xBytes[2], xBytes[3],

                0xC7, 0x42, 0x04, // mov [rdx+4], yBytes
                yBytes[0], yBytes[1], yBytes[2], yBytes[3],

                0xC7, 0x42, 0x08, // mov [rdx+8], zBytes
                zBytes[0], zBytes[1], zBytes[2], zBytes[3],

                0x48, 0x89, 0xD0, // mov rax, rdx

                0xC3 // ret
            };

            // Patch getter
            Memory.WriteBuffer(WeaponDirectionGetter, silentBytes);

            WeaponDirectionGetterVanilla = false;
        }

        private static bool WeaponDirectionGetterVanilla = true;
        private static bool RestoreWeaponDirectionGetter()
        {
            if (WeaponDirectionGetterVanilla)
                return true;

            try
            {
                if (WeaponDirectionGetter == 0x0)
                    return true;

                bool success = Memory.WriteBufferEnsure(WeaponDirectionGetter, WeaponDirectionGetterOriginalBytes);
                WeaponDirectionGetterVanilla = success;

                return success;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[AIMBOT] RestoreWeaponDirectionGetter(): {ex}");
            }

            return false;
        }

        /// <summary>
        /// Modifies the bone (target) position based on target velocity, target distance, and bullet velocity and returns the new WeaponDirection needed to impact the target.
        /// </summary>
        private static Vector3 FormSilentTrajectory(Vector3 fireportPosition, Vector3 targetPosition, Vector3 targetVelocity, BallisticSimulationOutput? simulationResult)
        {
            if (simulationResult != null && simulationResult is BallisticSimulationOutput sim)
            {
                if (Math.Abs(targetVelocity.X) > 25f || Math.Abs(targetVelocity.Y) > 25f || Math.Abs(targetVelocity.Z) > 25f)
                    Logger.WriteLine("[AIMBOT] -> FormSilentTrajectory(): Running without prediction.");
                else
                {
                    targetVelocity.X *= sim.TravelTime;
                    targetVelocity.Y *= sim.TravelTime;
                    targetVelocity.Z *= sim.TravelTime;

                    targetPosition.Y += targetVelocity.Y + sim.DropCompensation;

                    targetPosition.X += targetVelocity.X;
                    targetPosition.Z += targetVelocity.Z;
                }
            }

            return Vector3.Normalize(targetPosition - fireportPosition);
        }
    }
}
