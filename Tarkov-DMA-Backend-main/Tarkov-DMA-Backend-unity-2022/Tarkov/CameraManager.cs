using System.Timers;
using Tarkov_DMA_Backend.ESP;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.LowLevel;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;
using Timer = System.Timers.Timer;

namespace Tarkov_DMA_Backend.Tarkov
{
    public static class CameraManager
    {
        private const int _cacheUpdateInterval = 2000;
        private const int _checkPlayerIsInRaidInterval = 1000;

        public static bool PlayerIsInRaid = false;
        public static bool CameraIsAvailable = false;

        /// <summary>
        /// The main scene camera.
        /// </summary>
        private static ulong FPSCamera;
        /// <summary>
        /// The camera used for weapon scopes.
        /// </summary>
        private static ulong OpticCamera;
        /// <summary>
        /// The FPS Camera SSAA component.
        /// </summary>
        private static ulong SSAA;
        /// <summary>
        /// The FPS Camera ThermalVision component.
        /// </summary>
        private static ulong ThermalVision;
        /// <summary>
        /// The FPS Camera NightVision component.
        /// </summary>
        private static ulong NightVision;
        /// <summary>
        /// The FPS Camera TOD_Scattering component.
        /// </summary>
        private static ulong TOD_Scattering;
        /// <summary>
        /// The FPS Camera PrismEffects component.
        /// </summary>
        private static ulong PrismEffects;
        /// <summary>
        /// The FPS Camera BloomAndFlares component.
        /// </summary>
        private static ulong BloomAndFlares;
        /// <summary>
        /// The FPS Camera CC_Vintage component.
        /// </summary>
        private static ulong CC_Vintage;

        static CameraManager()
        {
            Timer cameraRefresherTimer = new(_cacheUpdateInterval);
            cameraRefresherTimer.Elapsed += CameraRefresher;
            cameraRefresherTimer.Start();

            Timer shouldRenderESP_Timer = new(_checkPlayerIsInRaidInterval);
            shouldRenderESP_Timer.Elapsed += CheckIsPlayerInRaid;
            shouldRenderESP_Timer.Start();
        }

        private static void CameraRefresher(object sender, ElapsedEventArgs e)
        {
            if (!EFTDMA.InRaid || !PlayerIsInRaid)
                return;

            GetScreenDimensions();
        }

        private static void CheckIsPlayerInRaid(object sender, ElapsedEventArgs e)
        {
            if (!EFTDMA.InRaid)
            {
                PlayerIsInRaid = false;
                VisibilityCheck.SetActive(false);
                return;
            }

            if (!PlayerIsInRaid)
                PlayerIsInRaid = GetPlayerIsInRaid();

            VisibilityCheck.SetActive(PlayerIsInRaid);
        }

        /// <summary>
        /// Get the main scene camera.
        /// </summary>
        public static ulong GetFPSCamera(bool cached = true, bool deref = true)
        {
            try
            {
                if (FPSCamera == 0x0 || cached == false)
                {
                    // Only some things can use the raw camera ptr. So don't cache the non-deref'd version
                    if (deref) FPSCamera = Memory.GetCameraByName("FPS Camera");
                    else return Memory.GetCameraByName("FPS Camera", deref);
                }

                return FPSCamera;
            }
            catch (Exception ex)
            {
                FPSCamera = 0x0;
                Logger.WriteLine($"GetFPSCamera: ERROR getting camera information: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Disables Oclusion Culling for FPSCamera.
        /// </summary>
        public static void SetOclusionCulling(bool newState, ref List<ScatterWriteEntry> writes)
        {
            try
            {
                ulong fpsCamera = Memory.GetCamera(GetFPSCamera());

                bool currentState = Memory.ReadValue<bool>(fpsCamera + UnityOffsets.Camera.OcclusionCulling, false);
                if (currentState != newState)
                    Memory.WriteValue(fpsCamera + UnityOffsets.Camera.OcclusionCulling, newState);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"SetOcclusionCulling: ERROR setting occlusion culling: {ex}");
            }
        }

        /// <summary>
        /// Sets the near clip plane of the FPSCamera for the X-Ray Vision effect.
        /// </summary>
        public static ulong SetNearClipPlane(float newNearClipPlane)
        {
            try
            {
                ulong fpsCamera = Memory.GetCamera(GetFPSCamera());
                Memory.WriteValue(fpsCamera + UnityOffsets.Camera.NearClip, newNearClipPlane);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"SetNearClipPlane: ERROR setting NearClipPlane: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera SSAA component.
        /// </summary>
        public static ulong GetSSAA(bool cached = true)
        {
            try
            {
                if (SSAA == 0x0 || cached == false)
                    SSAA = Memory.GetObjectComponent(GetFPSCamera(cached), "SSAA");

                return SSAA;
            }
            catch (Exception ex)
            {
                SSAA = 0x0;
                Logger.WriteLine($"GetSSAA: ERROR getting FPS Camera SSAA component: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera ThermalVision component.
        /// </summary>
        public static ulong GetThermalVision(bool cached = true)
        {
            try
            {
                if (ThermalVision == 0x0 || cached == false)
                    ThermalVision = Memory.GetObjectComponent(GetFPSCamera(cached), "ThermalVision");

                return ThermalVision;
            }
            catch (Exception ex)
            {
                ThermalVision = 0x0;
                Logger.WriteLine($"GetThermalVision: ERROR getting FPS Camera ThermalVision component: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera NightVision component.
        /// </summary>
        public static ulong GetNightVision(bool cached = true)
        {
            try
            {
                if (NightVision == 0x0 || cached == false)
                    NightVision = Memory.GetObjectComponent(GetFPSCamera(cached), "NightVision");

                return NightVision;
            }
            catch (Exception ex)
            {
                NightVision = 0x0;
                Logger.WriteLine($"GetNightVision: ERROR getting FPS Camera NightVision component: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera TOD_Scattering component.
        /// </summary>
        public static ulong GetTOD_Scattering(bool cached = true)
        {
            try
            {
                if (TOD_Scattering == 0x0 || cached == false)
                    TOD_Scattering = Memory.GetObjectComponent(GetFPSCamera(cached), "TOD_Scattering");

                return TOD_Scattering;
            }
            catch (Exception ex)
            {
                TOD_Scattering = 0x0;
                Logger.WriteLine($"GetTOD_Scattering: ERROR getting FPS Camera TOD_Scattering component: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera PrismEffects component.
        /// </summary>
        public static ulong GetPrismEffects(bool cached = true)
        {
            try
            {
                if (PrismEffects == 0x0 || cached == false)
                    PrismEffects = Memory.GetObjectComponent(GetFPSCamera(cached), "PrismEffects");

                return PrismEffects;
            }
            catch (Exception ex)
            {
                PrismEffects = 0x0;
                Logger.WriteLine($"GetPrismEffects: ERROR getting FPS Camera PrismEffects component: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera BloomAndFlares component.
        /// </summary>
        public static ulong GetBloomAndFlares(bool cached = true)
        {
            try
            {
                if (BloomAndFlares == 0x0 || cached == false)
                    BloomAndFlares = Memory.GetObjectComponent(GetFPSCamera(cached), "BloomAndFlares");

                return BloomAndFlares;
            }
            catch (Exception ex)
            {
                BloomAndFlares = 0x0;
                Logger.WriteLine($"GetBloomAndFlares: ERROR getting FPS Camera BloomAndFlares component: {ex}");
            }

            return 0x0;
        }

        /// <summary>
        /// Get the FPS Camera CC_Vintage component.
        /// </summary>
        public static ulong GetCC_Vintage(bool cached = true)
        {
            try
            {
                if (CC_Vintage == 0x0 || cached == false)
                    CC_Vintage = Memory.GetObjectComponent(GetFPSCamera(cached), "CC_Vintage");

                return CC_Vintage;
            }
            catch (Exception ex)
            {
                CC_Vintage = 0x0;
                Logger.WriteLine($"GetCC_Vintage: ERROR getting FPS Camera CC_Vintage component: {ex}");
            }

            return 0x0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct Vector2i
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        struct DisplaySettings
        {
            [FieldOffset(0x8)]
            public Vector2i Resolution;
        }

        /// <summary>
        /// Gets the current pixel width and height of the Game.
        /// </summary>
        public static void GetScreenDimensions()
        {
            try
            {
                if (!ESP_Config.Enabled)
                    return;

                ulong GameSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton(ClassNames.GameSettings.ClassName_ClassToken));

                ulong Graphics = Memory.ReadPtr(GameSettings + Offsets.GameSettingsContainer.Graphics);
                ulong Settings = Memory.ReadPtr(Graphics + Offsets.GameSettingsInnerContainer.Settings);
                ulong DisplaySettings = Memory.ReadPtr(Settings + Offsets.GraphicsSettings.DisplaySettings);
                ulong SettingValueClass = Memory.ReadPtr(DisplaySettings + Offsets.BSGGameSetting.ValueClass);
                var DisplaySettingsValues = Memory.ReadValue<DisplaySettings>(SettingValueClass + Offsets.BSGGameSettingValueClass.Value);

                int resolutionX = DisplaySettingsValues.Resolution.X;
                int resolutionY = DisplaySettingsValues.Resolution.Y;

                // Make sure a valid resolution was read
                if (resolutionX == 0 || resolutionY == 0 ||
                    resolutionX < 400 || resolutionY < 400 ||
                    resolutionX > 8000 || resolutionY > 5000)
                    return;

                // Update resolution
                ESP_Config.ResolutionX = resolutionX;
                ESP_Config.ResolutionY = resolutionY;

                // Update tolerances
                ESP_Config.DisplayXMax = resolutionX + ESP_Config.DisplayTolerance;
                ESP_Config.DisplayYMax = resolutionY + ESP_Config.DisplayTolerance;

                // Determine if W2S output should be scaled
                if (ESP_Config.ResolutionX != ESP_Config.ESP_ResolutionX || ESP_Config.ResolutionY != ESP_Config.ESP_ResolutionY) ESP_Config.ShouldScaleESP = true;
                else ESP_Config.ShouldScaleESP = false;

                //ESP_Manager._fuserWindow.SetTitle();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"GetScreenDimensions: ERROR getting Screen Dimensions: {ex}");
            }
        }

        /// <summary>
        /// Get the camera used for weapon scopes.
        /// </summary>
        public static ulong GetOpticCamera(bool cached = true)
        {
            try
            {
                if (OpticCamera == 0x0 || cached == false)
                    OpticCamera = Memory.GetCameraByName("BaseOpticCamera(Clone)");

                return OpticCamera;
            }
            catch (Exception ex)
            {
                OpticCamera = 0x0;
                Logger.WriteLine($"GetOpticCamera: ERROR getting camera information: {ex}");
            }

            return 0x0;
        }

        private static bool GetPlayerIsInRaid()
        {
            try
            {
                ulong fpsCamera = Memory.GetCamera(GetFPSCamera(false), false);
                Memory.ReadValue<Matrix4x4>(fpsCamera + UnityOffsets.Camera.ViewMatrix, false);
                CameraIsAvailable = true;

                // If we can get the camera but the screen name is MatchmakerFinalCountdown the raid hasn't fully begun yet
                ulong screenManager = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass(ClassNames.ScreenManager.ClassName_ClassToken) + Offsets.ScreenManager.Instance, false);
                ulong sc = Memory.ReadPtrChain(screenManager, new uint[] { Offsets.ScreenManager.CurrentScreenController, Offsets.CurrentScreenController.Generic }, false);
                string name = ObjectClass.ReadName(sc, 128, false);
                if (name == "MatchmakerFinalCountdown")
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reset all camera addresses.
        /// </summary>
        public static void Reset()
        {
            FPSCamera = 0x0;
            OpticCamera = 0x0;
            SSAA = 0x0;
            ThermalVision = 0x0;
            NightVision = 0x0;
            TOD_Scattering = 0x0;
            PrismEffects = 0x0;
            BloomAndFlares = 0x0;
            CC_Vintage = 0x0;

            PlayerIsInRaid = false;
            CameraIsAvailable = false;
        }
    }
}
