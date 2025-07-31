using arena_dma_backend.DMA.Collections.Implementation;
using arena_dma_backend.ESP;
using arena_dma_backend.Mono;
using System.Timers;
using Timer = System.Timers.Timer;

namespace arena_dma_backend.Arena
{
    public static class CameraManager
    {
        private const int _cacheUpdateInterval = 10000;
        private const int _checkPlayerIsInRaidInterval = 1000;

        public static bool PlayerIsInRaid = false;

        /// <summary>
        /// The main scene camera.
        /// </summary>
        private static ulong FPSCamera;
        /// <summary>
        /// The camera used for weapon scopes.
        /// </summary>
        private static ulong OpticCamera;

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
            if (!Game.InMatch || !PlayerIsInRaid)
                return;

            GetScreenDimensions();
        }

        private static void CheckIsPlayerInRaid(object sender, ElapsedEventArgs e)
        {
            if (!Game.InMatch)
            {
                PlayerIsInRaid = false;
                return;
            }

            PlayerIsInRaid = GetPlayerIsInRaid();
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
                Memory.IsCameraActive(Memory.GetCamera(GetOpticCamera(false), false));
                ulong fpsCamera = Memory.GetCamera(GetFPSCamera(false), false);
                Memory.ReadValue<Matrix4x4>(fpsCamera + UnityOffsets.Camera.ViewMatrix, false);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the current pixel width and height of the Game.
        /// </summary>
        public static void GetScreenDimensions()
        {
            try
            {
                ulong fpsCamera = GetFPSCamera(false, false);

                int resolutionX = NativeHelper.GetCameraWidth(fpsCamera);
                int resolutionY = NativeHelper.GetCameraHeight(fpsCamera);

                // Update resolution
                ESP_Config.GameResolutionX = resolutionX;
                ESP_Config.GameResolutionY = resolutionY;

                // Determine if W2S output should be scaled
                if (ESP_Config.GameResolutionX != ESP_Config.ESP_ResolutionX || ESP_Config.GameResolutionY != ESP_Config.ESP_ResolutionY) ESP_Config.ShouldScale = true;
                else ESP_Config.ShouldScale = false;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"GetScreenDimensions: ERROR getting Screen Dimensions: {ex}");
            }
        }

        public static bool IsScoped(ulong camera)
        {
            if (Game.LocalPlayer == null)
                return false;

            try
            {
                if (Memory.IsCameraActive(camera)) // Check Mono Behaviour
                {
                    var opticsPtr = Memory.ReadPtr(Game.LocalPlayer.PWA + Offsets.ProceduralWeaponAnimation._optics);
                    using var optics = new MemList<ulong>(opticsPtr);
                    if (optics.Count > 0)
                    {
                        var pSightComponent = Memory.ReadPtr(optics[0] + Offsets.SightNBone.Mod);
                        var sightComponent = Memory.ReadValue<SightComponent>(pSightComponent);

                        return sightComponent.GetZoomLevel() > 1f; // Make sure we're actually zoomed in
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"IsScoped: ERROR -> {ex}");
                return false;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private readonly struct SightComponent // (Type: EFT.InventoryLogic.SightComponent)
        {
            [FieldOffset(0x20)]
            private readonly ulong pSightInterface;

            [FieldOffset(0x30)]
            private readonly ulong pScopeSelectedModes;

            [FieldOffset(0x38)]
            private readonly int SelectedScope;

            public readonly float GetZoomLevel()
            {
                using var zoomArray = SightInterface.Zooms;
                if (SelectedScope >= zoomArray.Count || SelectedScope is < 0 or > 10)
                    return -1.0f;
                using MemArray<int> selectedScopeModes = new(pScopeSelectedModes, -1, false);
                var selectedScopeMode = SelectedScope >= selectedScopeModes.Count ? 0 : selectedScopeModes[SelectedScope];
                var zoomAddr = zoomArray[SelectedScope] + MemArray<float>.ArrBaseOffset + (uint)selectedScopeMode * 0x4;

                var zoomLevel = Memory.ReadValue<float>(zoomAddr, false);
                if (float.IsNormal(zoomLevel) && zoomLevel is >= 0f and < 100f)
                    return zoomLevel;

                return -1.0f;
            }

            public readonly SightInterface SightInterface => Memory.ReadValue<SightInterface>(pSightInterface);
        }

        [StructLayout(LayoutKind.Explicit)]
        private readonly struct SightInterface // _template (Type: -.GInterfaceBB26)
        {
            [FieldOffset(0x170)]
            private readonly ulong pZooms;

            public readonly MemArray<ulong> Zooms => new(pZooms);
        }

        /// <summary>
        /// Reset all camera addresses.
        /// </summary>
        public static void Reset()
        {
            FPSCamera = 0x0;
            OpticCamera = 0x0;

            PlayerIsInRaid = false;
        }
    }
}
