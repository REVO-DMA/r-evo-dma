using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Toolkit.Features;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.Hotkey;
using static Tarkov_DMA_Backend.Tarkov.Toolkit.Features.AimbotSettings;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit
{
    public class ToolkitManager
    {
        public static readonly Dictionary<string, bool> AlwaysActiveFeatureState = new()
        {
            { "hideRaidCode", false },
            { "streamerMode", false },
            { "antiAFK", false },
            { "noPenalties", false },
            { "removableAttachments", false },
            { "unlimitedSearch", false },
            { "noMalfunctions", false },
            { "showOwnDogTag", false },
            { "keybindFromAnywhere", false },
            { "harmlessAI", false },
            { "noFallDamage", false },
            { "pingBypass", false },
        };

        public static readonly Dictionary<string, bool> FeatureState = new()
        {
            { "noFall", false },
            { "infiniteStamina", false },
            { "enhancedJump", false },
            { "enhancedThrow", false },
            { "muleMode", false },
            { "alwaysSprint", false },
            { "360Freelook", false },
            { "noInertia", false },
            { "wideLean", false },
            { "thirdPerson", false },
            { "xRayVision", false },
            { "noVisor", false },
            { "nightVision", false },
            { "thermalVision", false },
            { "aimbot", false },
            { "instantADS", false },
            { "customRecoil", false },
            { "noOverheating", false },
            { "fastBulletLoadUnload", false },
            { "speedHack", false },
            { "lootThroughWalls", false },
            { "silentLoot", false },
            { "chams", false },
            { "disableCulling", false },
            { "fullBright", false },
            { "alwaysDay", false },
            { "sunnyWeather", false },
            { "disableGrass", false },
            { "disableInventoryBlur", false },
            { "fovChanger", false },
            { "noHeadBobbing", false },
            { "disableVignette", false },
            { "disableExposure", false },
            { "disableBloom", false },
            { "disableColorGrading", false },
            { "disableShadows", false },
            { "disableScreenEffects", false },
            { "gatherAI", false },
            { "instantZoom", false },
            { "instantPlant", false },

            // Ban me features
            { "unmountMountedWeapon", false },
            { "superSpeed", false },
            { "characterManipulation", false },
            { "desync", false },
        };

        public static readonly Dictionary<string, int> FeatureSettings_int = new()
        {
            { "aimbot_hotkey", 306 }, // UnityKeyCode
            { "aimbot_boneHuman", 37 }, // Bones
            { "aimbot_boneAI", 133 }, // Bones
            { "aimbot_targetingMode", 1 },
            { "aimbot_lockMode", 1 },
            { "aimbot_missChance", 0 },
            { "wideLean_verticalAbove_hotkey", 0 }, // UnityKeyCode
            { "wideLean_verticalBelow_hotkey", 0 }, // UnityKeyCode
            { "characterManipulationUp_hotkey", 0 }, // UnityKeyCode
            { "characterManipulationDown_hotkey", 0 }, // UnityKeyCode
            { "silentLoot_hotkey", 0 }, // UnityKeyCode
            { "superSpeed_hotkey", 324 }, // UnityKeyCode
            { "chams_Mode", 1 },
            { "fovChanger_FOV", 75 },
            { "fovChanger_AimFOV", 75 },
            { "fovChanger_TppFOV", 75 },
            { "instantZoom_hotkey", 0 }, // UnityKeyCode
            { "instantZoom_FOV", 45 },
            { "gatherAI_hotkey", 0 }, // UnityKeyCode
            { "superSpeed_OnTime", 150 },
            { "superSpeed_OffTime", 120 },
        };

        public static readonly Dictionary<string, float> FeatureSettings_float = new()
        {
            { "aimbot_maxDistance", 450f },
            { "aimbot_fov", 30f },
            { "aimbot_SmoothAmount_X", 35.0f },
            { "aimbot_SmoothAmount_Y", 47.0f },
            { "aimbot_SmoothMagnitude", 1.0f },
            { "aimbot_DeadZone", 0.1f },
            { "normalSpeedHack_speed", 1.0f },
            { "enhancedJump_StrengthBuffJumpHeightInc", 0.1f },
            { "enhancedThrow_StrengthBuffThrowDistanceInc", 0.1f },
            { "wideLean_horizontalDistance", 0.1f },
            { "wideLean_verticalDistance", 0.1f },
            { "tpp_horizontalDistance", 5f },
            { "tpp_verticalDistance", 5f },
            { "tpp_horizontalOffset", 5f },
            { "xRayVision_nearClipPlane", 0.03f },
            { "customRecoil_ShotIntensity", 1.0f },
            { "customRecoil_BreathIntensity", 1.0f },
            { "silentLoot_distance", 0.3f },
            { "fullBright_brightness", 1.0f },
            { "alwaysDay_Hour", 12f },
            { "superSpeed_Speed", 1f },
            { "characterManipulation_Distance", 1f },
        };

        public static readonly Dictionary<string, bool> FeatureSettings_bool = new()
        {
            { "aimbot_alwaysOn", false },
            { "aimbot_TargetTeammates", false },
            { "aimbot_SmoothEnabled", false },
            { "customRecoil_AlwaysAim", false },
            { "customRecoil_DisableWeaponInertia", true },
            { "chams_AimbotTarget", false },

            { "disableScreenEffects_NoFlash", false },
            { "disableScreenEffects_NoBlood", false },
            { "disableScreenEffects_NoSharpen", false },
            { "disableScreenEffects_NoBlur", false },

            { "streamerMode_SpoofName", false },
            { "streamerMode_SpoofLevel", false },
            { "streamerMode_SpoofDogtags", false },
            { "streamerMode_HideOverallInfo", false },
            { "streamerMode_DisableNotifications", false },
        };

        public static readonly Dictionary<string, string> ChamsColors = new()
        {
            { "chams_AimbotLocked_visible", "#ff0000" },
            { "chams_AimbotLocked_invisible", "#000000" },
            { "chams_Teammate_visible", "#00ff00" },
            { "chams_Teammate_invisible", "#00ff00" },
            { "chams_EnemyPMC_visible", "#000000" },
            { "chams_EnemyPMC_invisible", "#ff0000" },
            { "chams_PlayerScav_visible", "#000000" },
            { "chams_PlayerScav_invisible", "#ffff00" },
            { "chams_ScavBoss_visible", "#000000" },
            { "chams_ScavBoss_invisible", "#e617c8" },
            { "chams_Scav_visible", "#000000" },
            { "chams_Scav_invisible", "#ff6900" },
            { "chams_Default_visible", "#000000" },
            { "chams_Default_invisible", "#0000ff" },
            { "chams_Corpse_visible", "#999999" },
            { "chams_Corpse_invisible", "#999999" },
        };

        public readonly FeaturesController FeaturesController;

        public ToolkitManager()
        {
            FeaturesController = new();

#if COMMERCIAL || RELEASE

            // Private Only Features
            FeaturesController.AddFeature(new NoFall(1000));
            FeaturesController.AddFeature(new WideLean(100));
            FeaturesController.AddFeature(new SpeedHack(1000));
            FeaturesController.AddFeature(new DisableVignette(1000));
            FeaturesController.AddFeature(new DisableExposure(1000));
            FeaturesController.AddFeature(new DisableBloom(1000));
            FeaturesController.AddFeature(new DisableColorGrading(1000));
            FeaturesController.AddFeature(new GatherAI(1000));
            FeaturesController.AddFeature(new OwlMode(1000));

            // Ban me features
            FeaturesController.AddFeature(new UnmountMountedWeapon(500));
            FeaturesController.AddFeature(new SuperSpeed(10000));
            FeaturesController.AddFeature(new CharacterManipulation(10000));
            FeaturesController.AddFeature(new Desync(1000));

#endif

            FeaturesController.AddFeature(new AlwaysSprint(3000));
            FeaturesController.AddFeature(new InstantZoom(10000));
            FeaturesController.AddFeature(new CustomRecoil(50));
            FeaturesController.AddFeature(new InfiniteStamina(500));
            FeaturesController.AddFeature(new XRayVision(1000));
            FeaturesController.AddFeature(new ThirdPerson(500));
            FeaturesController.AddFeature(new AimbotSettings(3000));
            FeaturesController.AddFeature(new MuleMode(1000));
            FeaturesController.AddFeature(new NightVision(1000));
            FeaturesController.AddFeature(new ThermalVision(1000));
            FeaturesController.AddFeature(new InstantADS(1000));
            FeaturesController.AddFeature(new NoInertia(1000));
            FeaturesController.AddFeature(new FOVChanger(50));
            FeaturesController.AddFeature(new NoHeadBobbing(3000));
            FeaturesController.AddFeature(new EnhancedJump(3000));
            FeaturesController.AddFeature(new EnhancedThrow(3000));
            FeaturesController.AddFeature(new NoVisor(1000));
            FeaturesController.AddFeature(new FastBulletLoadUnload(1000));
            FeaturesController.AddFeature(new Chams(1000));
            FeaturesController.AddFeature(new LootThroughWalls(1000));
            FeaturesController.AddFeature(new SilentLoot(800));
            FeaturesController.AddFeature(new DisableInventoryBlur(1000));
            FeaturesController.AddFeature(new FullBright(1000));
            FeaturesController.AddFeature(new AlwaysDay(1000));
            FeaturesController.AddFeature(new SunnyWeather(1000));
            FeaturesController.AddFeature(new DisableGrass(1000));
            FeaturesController.AddFeature(new DisableCulling(3000));
            FeaturesController.AddFeature(new DisableShadows(1000));
            FeaturesController.AddFeature(new DisableScreenEffects(1000));
            FeaturesController.AddFeature(new InstantPlant(1000));
        }

        public void Dispose()
        {
            FeaturesController?.Dispose();
        }

        public static void SyncFeatures(JSON.FeatureSync[] features)
        {
            if (features == null) return;

            bool chamsUpdated = false;
            List<PlayerType> typesToUpdate = new();

            // Sync feature state
            for (int i = 0; i < features.Length; i++)
            {
                JSON.FeatureSync feature = features[i];

                // Check if this is an always active feature
                if (AlwaysActiveFeatureState.TryGetValue(feature.ID, out _))
                {
                    AlwaysActiveFeatureState[feature.ID] = feature.Enabled;

                    // Sync this feature's settings
                    for (int ii = 0; ii < feature.Settings.Length; ii++)
                    {
                        JSON.FeatureSettingSync featureSetting = feature.Settings[ii];

                        if (featureSetting.Type == "int")
                            FeatureSettings_int[featureSetting.ID] = int.Parse(featureSetting.Value);
                        else if (featureSetting.Type == "float")
                            FeatureSettings_float[featureSetting.ID] = float.Parse(featureSetting.Value);
                        else if (featureSetting.Type == "bool")
                            FeatureSettings_bool[featureSetting.ID] = bool.Parse(featureSetting.Value);
                    }

                    // Queue this feature to run immediately
                    var aa_updatedFeature = EFTDMA.AlwaysActiveFeaturesController?.GetFeature(feature.ID);
                    if (aa_updatedFeature != null)
                        aa_updatedFeature.RunImmediately = true;

                    continue;
                }

                FeatureState[feature.ID] = feature.Enabled;

                // Sync this feature's settings
                for (int ii = 0; ii < feature.Settings.Length; ii++)
                {
                    JSON.FeatureSettingSync featureSetting = feature.Settings[ii];

                    if (featureSetting.Type == Constants.FeatureSettingTypes.UnityKeyCode)
                    {
                        int newKeyCode = int.Parse(featureSetting.Value);

                        FeatureSettings_int[featureSetting.ID] = newKeyCode;

                        HotkeyManager.RegisterHotkey((UnityKeyCode)newKeyCode, featureSetting.ID);
                    }
                    else if (featureSetting.Type == "int")
                        FeatureSettings_int[featureSetting.ID] = int.Parse(featureSetting.Value);
                    else if (featureSetting.Type == "float")
                        FeatureSettings_float[featureSetting.ID] = float.Parse(featureSetting.Value);
                    else if (featureSetting.Type == "bool")
                        FeatureSettings_bool[featureSetting.ID] = bool.Parse(featureSetting.Value);
                    else if (featureSetting.Type == "VisCheckColor")
                    {
                        var visibleKey = $"{featureSetting.ID}_visible";
                        var invisibleKey = $"{featureSetting.ID}_invisible";

                        var colors = featureSetting.Value.Split('_');
                        var visibleColor = colors[0];
                        var invisibleColor = colors[1];

                        // If any chams colors were changed mark chams for re-init
                        if (visibleColor != ChamsColors[visibleKey] || invisibleColor != ChamsColors[invisibleKey])
                        {
                            chamsUpdated = true;

                            ChamsColors[visibleKey] = visibleColor;
                            ChamsColors[invisibleKey] = invisibleColor;

                            var playerTypeStr = featureSetting.ID.Split("_")[1];
                            PlayerType playerType;

                            if (playerTypeStr == "AimbotLocked")
                                playerType = PlayerType.AimbotLocked;
                            else if (playerTypeStr == "EnemyPMC")
                                playerType = PlayerType.EnemyPMC;
                            else if (playerTypeStr == "Teammate")
                                playerType = PlayerType.Teammate;
                            else if (playerTypeStr == "ScavBoss")
                                playerType = PlayerType.Boss;
                            else if (playerTypeStr == "PlayerScav")
                                playerType = PlayerType.PlayerScav;
                            else if (playerTypeStr == "Scav")
                                playerType = PlayerType.Scav;
                            else if (playerTypeStr == "Default")
                                playerType = PlayerType.Default;
                            else if (playerTypeStr == "Corpse")
                                playerType = PlayerType.Corpse;
                            else
                            {
                                Logger.WriteLine($"Skipped updating player type \"{playerTypeStr}\". It has no handler.");
                                continue;
                            }

                            typesToUpdate.Add(playerType);
                        }
                    }
                    else if (featureSetting.Type == "hitboxes")
                    {
                        bool isPlayer = featureSetting.ID == "aimbot_playerHitboxes";

                        var hitboxSettings = JSON.DeserializeHitboxSettings(featureSetting.Value);
                        Dictionary<Bone, HitboxSettings> newSettings = new();
                        foreach (var setting in hitboxSettings.Values)
                        {
                            Bone bone = Player.BoneNameToBone(setting.SelectedBone, setting.Side);
                            HitboxSettings settings = new(setting.Chance, setting.SmartTargeting);

                            newSettings.Add(bone, settings);
                        }

                        if (isPlayer)
                            PlayerHitboxSettings = newSettings;
                        else
                            AIHitboxSettings = newSettings;
                    }
                }

                // Queue this feature to run immediately
                var updatedFeature = EFTDMA.FeaturesController?.GetFeature(feature.ID);
                if (updatedFeature != null)
                    updatedFeature.RunImmediately = true;
            }

            // Run on thread pool since this can take a while
            Task.Run(() =>
            {
                lock (Chams._chamsLock)
                {
                    if (chamsUpdated)
                        RedoChams(typesToUpdate.ToArray());
                }
            });
        }

        private static void RedoChams(PlayerType[] typesToUpdate)
        {
            Player[] Players = EFTDMA.DisplayPlayers;

            if (Players != null)
            {
                // Mark chams as unset so everyone gets reapplied
                foreach (Player player in Players)
                {
                    if (player == null) continue;

                    player.ChamsSet = false;
                }
            }

            // Recreate chams materials
            if (ChamsManager.ChamsStatus is ChamsManager.ChamsLoadStatus.FullyLoaded)
                ChamsManager.SyncRemoteChamsColors(typesToUpdate);
        }

        public static void SyncFeatureToggleHotkeys(JSON.FeatureToggleHotkeySync[] featureToggleHotkeys)
        {
            if (featureToggleHotkeys == null) return;

            for (int i = 0; i < featureToggleHotkeys.Length; i++)
            {
                var feature = featureToggleHotkeys[i];

                if (feature == null) continue;

                HotkeyManager.RegisterHotkey((UnityKeyCode)feature.Hotkey, feature.ID, true);
            }
        }
    }
}
