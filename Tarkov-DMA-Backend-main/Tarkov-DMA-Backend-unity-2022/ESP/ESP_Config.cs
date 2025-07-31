using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity.Hotkey;

namespace Tarkov_DMA_Backend.ESP
{
    public static class ESP_Config
    {
        public static bool InitialSyncCompleted = false;

        public static bool Enabled = false;
        public static int Mode = 1;
        public static int FuserMonitor = 0;
        public static bool ESP_Antialiasing = false;

        // Window Properties
        public static bool Fullscreen = false;
        public static bool Borderless = false;
        public static bool Topmost = true;
        public static bool Clickthrough = false;

        // ESP Render Delay
        public static bool ESP_ShowFPS = true;
        public static int ESP_UpdateSpeed = 60;
        public static int LowLatencyMode = 1;

        // Display
        public const float DisplayTolerance = 800f;
        public const float DisplayXMin = -DisplayTolerance;
        public const float DisplayYMin = -DisplayTolerance;

        public static int ResolutionX = 1920;
        public static int ResolutionY = 1080;
        public static float DisplayXMax = 3000f;
        public static float DisplayYMax = 2000f;

        // Native ESP Window
        public static int ESP_ResolutionX = 600;
        public static int ESP_ResolutionY = 350;

        public static bool ShouldScaleESP = true;

        // Configuration

        // Utilities
        public static bool BattleModeEnabled = false;
        public static bool SmartDeclutterEnabled = true;
        public static bool ShowCrosshair = true;
        public static byte CrosshairStyle = 1;
        public static bool DynamicCrosshair = false;
        public static bool FakeLaser = false;
        public static float CrosshairScale = 1f;
        public static bool ShowAimbotFOV = true;
        public static bool ShowShowSilentAimTarget = true;
        public static bool ShowHeldWeaponInfo = true;

        // Players
        public static int PlayerRenderStyle = 1;
        public static float PlayerDrawDistance = 450f;
        public static int PlayerVisibilityCheckStyle = 1;
        public static int PlayerBoxStyle = 1;
        public static int PlayerBoxThickness = 2;
        public static bool ShowPlayerBones = true;
        public static int PlayerBoneThickness = 2;
        public static int PlayerFontSize = 12;
        public static bool PlayerShowName = true;
        public static bool PlayerShowTeamNumber = true;
        public static bool PlayerShowHandsContents = true;
        public static bool PlayerShowDistance = true;
        public static bool PlayerShowHealth = true;
        public static bool PlayerShowLevel = true;
        public static bool PlayerShowKD = true;

        // AI
        public static int AIRenderStyle = 1;
        public static float AIDrawDistance = 450f;
        public static int AIVisibilityCheckStyle = 1;
        public static int AIBoxStyle = 1;
        public static int AIBoxThickness = 2;
        public static bool ShowAIBones = true;
        public static int AIBoneThickness = 2;
        public static int AIFontSize = 12;
        public static bool AIShowName = true;
        public static bool AIShowHandsContents = true;
        public static bool AIShowDistance = true;
        public static bool AIShowHealth = true;

        // Items
        public static bool ShowLoot = true;
        public static float LootDrawDistance = 100f;
        public static int LootFontSize = 12;

        // Extractions
        public static bool ShowExtractions = true;
        public static byte ExtractionVisibility = 2;
        public static float ExtractionDrawDistance = 450f;
        public static int ExfilFontSize = 12;

        // Grenades
        public static bool ShowGrenades = true;
        public static float GrenadeDrawDistance = 150f;

        // Claymores
        public static bool ShowClaymores = true;
        public static float ClaymoreDrawDistance = 50f;
        public static float ClaymoreWarningDistance = 20f;
        public static int ClaymoreFontSize = 12;

        // Tripwires
        public static bool ShowTripwires = true;
        public static float TripwiresDrawDistance = 50f;
        public static int TripwiresFontSize = 12;

        public static void SyncConfig(JSON.EspSync config)
        {
            try
            {
                if (config == null) return;

                bool shouldRecreatePaints = false;

                for (int i = 0; i < config.Settings.Length; i++)
                {
                    JSON.ESPSettingSync configSetting = config.Settings[i];

                    // General
                    if (configSetting.ID == "esp_enabled") // ESP Enabled
                        Enabled = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_displayStyle") // Display Style
                    {
                        if (configSetting.Value == "Normal")
                            Mode = 1;
                        else if (configSetting.Value == "Fuser")
                            Mode = 2;
                    }
                    else if (configSetting.ID == "esp_monitor") // Fuser Monitor
                        FuserMonitor = int.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_borderless") // Borderless
                    {
                        bool newState = bool.Parse(configSetting.Value);
                        if (Borderless != newState)
                        {
                            Borderless = newState;
                            ESP_Manager.ESPWindow?.SetWindowProperties();
                        }
                    }
                    else if (configSetting.ID == "esp_topmost") // Topmost
                    {
                        bool newState = bool.Parse(configSetting.Value);
                        if (Topmost != newState)
                        {
                            Topmost = newState;
                            ESP_Manager.ESPWindow?.SetWindowProperties();
                        }
                    }
                    else if (configSetting.ID == "esp_clickthrough") // Clickthrough
                    {
                        bool newState = bool.Parse(configSetting.Value);
                        if (Clickthrough != newState)
                        {
                            Clickthrough = newState;
                            ESP_Manager.ESPWindow?.SetWindowProperties();
                        }
                    }
                    else if (configSetting.ID == "esp_showFPS") // Low Latency Mode
                    {
                        ESP_ShowFPS = bool.Parse(configSetting.Value);
                    }
                    else if (configSetting.ID == "esp_antiAliasing") // Anti-aliasing
                    {
                        ESP_Antialiasing = bool.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_updateSpeed") // ESP Update Speed
                    {
                        ESP_UpdateSpeed = int.Parse(configSetting.Value);
                    }
                    else if (configSetting.ID == "esp_lowLatencyMode") // Low Latency Mode
                    {
                        if (configSetting.Value == "Disabled")
                            LowLatencyMode = 1;
                        else if (configSetting.Value == "Enabled")
                            LowLatencyMode = 2;
                        else if (configSetting.Value == "Enabled + Boost")
                            LowLatencyMode = 3;

                        ESP_Manager.FuserWindow?.SetWindowFramerate();
                    }
                    else if (configSetting.ID == "esp_battleModeEnabled") // Enable Battle Mode
                        BattleModeEnabled = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_battleModeHotkey") // Battle Mode Hotkey
                    {
                        int newKeyCode = int.Parse(configSetting.Value);

                        HotkeyManager.RegisterHotkey((UnityKeyCode)newKeyCode, configSetting.ID);
                    }
                    else if (configSetting.ID == "esp_smartDeclutterEnabled")
                        SmartDeclutterEnabled = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_crosshairStyle") // Crosshair Style
                    {
                        if (configSetting.Value == "Off")
                            ShowCrosshair = false;
                        else
                            ShowCrosshair = true;

                        if (configSetting.Value == "Dot")
                            CrosshairStyle = 1;
                        else if (configSetting.Value == "Cross")
                            CrosshairStyle = 2;
                    }
                    else if (configSetting.ID == "esp_dynamicCrosshair") // Dynamic Crosshair
                        DynamicCrosshair = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_fakeLaser") // Fake Laser
                        FakeLaser = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_crosshairScale") // Crosshair Scale
                        CrosshairScale = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showAimbotFOV") // Show Aimbot FOV
                        ShowAimbotFOV = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showSilentAimTarget") // Show Silent Aim Target
                        ShowShowSilentAimTarget = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showHeldWeaponInfo") // Show Held Weapon Info
                        ShowHeldWeaponInfo = bool.Parse(configSetting.Value);

                    // Players
                    else if (configSetting.ID == "esp_playerRenderStyle") // Render Style
                    {
                        if (configSetting.Value == "Always")
                            PlayerRenderStyle = 1;
                        else if (configSetting.Value == "Visible Only")
                            PlayerRenderStyle = 2;
                    }
                    else if (configSetting.ID == "esp_maxPlayerRenderDistance") // Render Distance
                        PlayerDrawDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_playerVisibilityCheckStyle") // Visibility Check Style
                    {
                        if (configSetting.Value == "Per-Limb")
                            PlayerVisibilityCheckStyle = 1;
                        else if (configSetting.Value == "Whole Body")
                            PlayerVisibilityCheckStyle = 2;
                        else if (configSetting.Value == "Box Only")
                            PlayerVisibilityCheckStyle = 3;
                    }
                    else if (configSetting.ID == "esp_playerBoxStyle") // Box Style
                    {
                        if (configSetting.Value == "Off")
                            PlayerBoxStyle = 1;
                        else if (configSetting.Value == "Corner")
                            PlayerBoxStyle = 2;
                        else if (configSetting.Value == "Full")
                            PlayerBoxStyle = 3;
                    }
                    else if (configSetting.ID == "esp_PlayerBoxThickness") // Box Thickness
                    {
                        PlayerBoxThickness = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_showPlayerBones") // Show Bones
                        ShowPlayerBones = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_PlayerBoneThickness") // Bone Thickness
                    {
                        PlayerBoneThickness = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_PlayerFontSize") // Font Size
                    {
                        PlayerFontSize = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_showPlayersName") // Show Name
                        PlayerShowName = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showPlayersTeamNumber") // Show Team Number
                        PlayerShowTeamNumber = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showPlayersHandsContents") // Show Hands Contents
                        PlayerShowHandsContents = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showPlayersDistance") // Show Distance
                        PlayerShowDistance = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showPlayerHealth") // Show Health
                        PlayerShowHealth = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showPlayerLevel") // Show Level
                        PlayerShowLevel = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showPlayerKD") // Show KD
                        PlayerShowKD = bool.Parse(configSetting.Value);

                    // AI
                    else if (configSetting.ID == "esp_aiRenderStyle") // Render Style
                    {
                        if (configSetting.Value == "Always")
                            AIRenderStyle = 1;
                        else if (configSetting.Value == "Visible Only")
                            AIRenderStyle = 2;
                    }
                    else if (configSetting.ID == "esp_maxAIRenderDistance") // Player Draw Distance
                        AIDrawDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_aiVisibilityCheckStyle") // Visibility Check Style
                    {
                        if (configSetting.Value == "Whole Body")
                            AIVisibilityCheckStyle = 2;
                        else if (configSetting.Value == "Box Only")
                            AIVisibilityCheckStyle = 3;
                    }
                    else if (configSetting.ID == "esp_aiBoxStyle") // Box Style
                    {
                        if (configSetting.Value == "Off")
                            AIBoxStyle = 1;
                        else if (configSetting.Value == "Corner")
                            AIBoxStyle = 2;
                        else if (configSetting.Value == "Full")
                            AIBoxStyle = 3;
                    }
                    else if (configSetting.ID == "esp_aiBoxThickness") // Box Thickness
                    {
                        AIBoxThickness = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_showAIBones") // Show Players
                        ShowAIBones = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_aiBoneThickness") // Bone Thickness
                    {
                        AIBoneThickness = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_aiFontSize") // Font Size
                    {
                        AIFontSize = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    else if (configSetting.ID == "esp_showAIName") // Show Name
                        AIShowName = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showAIHandsContents") // Show Hands Contents
                        AIShowHandsContents = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showAIDistance") // Show Distance
                        AIShowDistance = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_showAIHealth") // Show Health
                        AIShowHealth = bool.Parse(configSetting.Value);

                    // Items
                    else if (configSetting.ID == "esp_showLoot") // Show Loot
                        ShowLoot = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_maxLootDistance") // Loot Draw Distance
                        LootDrawDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_lootFontSize") // Font Size
                    {
                        LootFontSize = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    // Extractions
                    else if (configSetting.ID == "esp_exfilVisibility") // Extraction Visibility
                    {
                        if (configSetting.Value == "Off")
                            ShowExtractions = false;
                        else
                            ShowExtractions = true;

                        if (configSetting.Value == "All")
                            ExtractionVisibility = 1;
                        else if (configSetting.Value == "Active/Pending")
                            ExtractionVisibility = 2;
                    }
                    else if (configSetting.ID == "esp_maxExtractionDistance") // Extraction Draw Distance
                        ExtractionDrawDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_exfilFontSize") // Font Size
                    {
                        ExfilFontSize = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    // Grenades
                    else if (configSetting.ID == "esp_showGrenades") // Show Grenades
                        ShowGrenades = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_maxGrenadeDistance") // Grenade Draw Distance
                        GrenadeDrawDistance = float.Parse(configSetting.Value);
                    // Claymores
                    else if (configSetting.ID == "esp_showClaymores") // Show Claymores
                        ShowClaymores = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_maxClaymoreDistance") // Claymore Draw Distance
                        ClaymoreDrawDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_warnClaymoreDistance") // Claymore Warning Distance
                        ClaymoreWarningDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_ClaymoreFontSize") // Font Size
                    {
                        ClaymoreFontSize = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                    // Tripwiress
                    else if (configSetting.ID == "esp_showTripwires") // Show Tripwiress
                        ShowTripwires = bool.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_maxTripwiresDistance") // Tripwires Draw Distance
                        TripwiresDrawDistance = float.Parse(configSetting.Value);
                    else if (configSetting.ID == "esp_TripwiresFontSize") // Font Size
                    {
                        TripwiresFontSize = int.Parse(configSetting.Value);
                        shouldRecreatePaints = true;
                    }
                }

                if (shouldRecreatePaints)
                {
                    PaintsManager.UpdatePaints();
                    ESP_Style.SyncStyles(null);
                }

                InitialSyncCompleted = true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP Config] SyncConfig() -> exception: {ex}");
                SentrySdk.CaptureException(ex);
            }
        }
    }
}
