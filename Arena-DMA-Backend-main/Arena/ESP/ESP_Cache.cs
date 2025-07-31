using arena_dma_backend.ESP;

namespace arena_dma_backend.Arena.ESP
{
    /// <summary>
    /// Cached Values for the ESP.
    /// </summary>
    public static class ESP_Cache
    {
        /// <summary>
        /// The second address that can be used to get the camera's view matrix.
        /// </summary>
        public static ulong CameraAddress { get; private set; }
        /// <summary>
        /// Whether or not the player is looking through a scope.
        /// </summary>
        public static bool IsScoped {  get; private set; }
        /// <summary>
        /// FPS camera aspect ratio.
        /// </summary>
        public static float AspectRatio { get; private set; }
        /// <summary>
        /// FPS camera AngleCtg property.
        /// </summary>
        public static float AngleCtg { get; private set; }

        public static void Update()
        {
            try
            {
                if (!CameraManager.PlayerIsInRaid || !Game.InMatch)
                    return;

                ulong opticCamera = Memory.GetCamera(CameraManager.GetOpticCamera());

                // Update the ESP scoped status
                IsScoped = IsADS() && CameraManager.IsScoped(opticCamera);

                if (IsScoped)
                {
                    CameraAddress = opticCamera;

                    ulong fpsCamera = Memory.GetCamera(CameraManager.GetFPSCamera());
                    float FOV = Memory.ReadValue<float>(fpsCamera + UnityOffsets.Camera.FOV, false);
                    AspectRatio = Memory.ReadValue<float>(fpsCamera + UnityOffsets.Camera.AspectRatio, false);

                    float AngleRadHalf = (float)(Math.PI / 180f) * FOV * 0.5f;
                    AngleCtg = (float)(Math.Cos(AngleRadHalf) / Math.Sin(AngleRadHalf));
                }
                else
                    CameraAddress = Memory.GetCamera(CameraManager.GetFPSCamera());

                ESP_Utilities.UpdateW2S();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP CACHE] Update() -> ERROR: {ex}");
            }
        }

        /// <summary>
        /// Checks if LocalPlayer is Aiming (ADS).
        /// </summary>
        /// <returns>True if aiming (ADS), otherwise False.</returns>
        private static bool IsADS()
        {
            try
            {
                return Memory.ReadValue<bool>(Game.LocalPlayer.PWA + Offsets.ProceduralWeaponAnimation._isAiming, false);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[ESP CACHE] -> IsADS(): {ex}");
                return false;
            }
        }
    }
}
