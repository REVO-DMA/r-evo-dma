using static gzw_dma_backend.GZW.Manager;

namespace gzw_dma_backend.GZW.ESP
{
    public static class ESP_Utilities
    {
        private const float ScopeFovMagicNumber = 11f;

        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public struct CameraInfoData
        {
            [FieldOffset((int)Offsets.FMinimalViewInfo.Location)]
            public Vector3D<double> Location;
            [FieldOffset((int)Offsets.FMinimalViewInfo.Rotation)]
            public Vector3D<double> Rotation;
            [FieldOffset((int)Offsets.FMinimalViewInfo.FOV)]
            public float FOV;
        }

        public static CameraInfoData CameraInfo { get; private set; }
        public static float ScopeFOV { get; set; } = -1f;

        private static Vector3D<double> AxisX;
        private static Vector3D<double> AxisY;
        private static Vector3D<double> AxisZ;

        public static void UpdateW2S()
        {
            CameraInfo = Memory.ReadValue<CameraInfoData>(LocalPlayerCameraManager + Offsets.APlayerCameraManager.CameraCachePrivate + Offsets.FCameraCacheEntry.POV, false);

            var viewMatrix = UE_Math.CreateViewMatrix(CameraInfo.Rotation);
            AxisX = new(viewMatrix.M11, viewMatrix.M12, viewMatrix.M13);
            AxisY = new(viewMatrix.M21, viewMatrix.M22, viewMatrix.M23);
            AxisZ = new(viewMatrix.M31, viewMatrix.M32, viewMatrix.M33);

            //Logger.WriteLine($"Location: {CameraInfo.Location} | FOV: {CameraInfo.FOV}");
        }

        public static bool W2S(Vector3D<double> worldLocation, out Vector2 screenPos)
        {
            Vector3D<double> delta = worldLocation - CameraInfo.Location;
            Vector3D<double> transformed = new(Vector3D.Dot(delta, AxisY), Vector3D.Dot(delta, AxisZ), Vector3D.Dot(delta, AxisX));
            float zDiv = (float)Math.Max(transformed.Z, 1d);

            float usedFOV;
            if (ScopeFOV == -1 || (ScopeFOV * ScopeFovMagicNumber) > CameraInfo.FOV) usedFOV = CameraInfo.FOV;
            else usedFOV = ScopeFOV * ScopeFovMagicNumber;

            screenPos.X = ESP_Config.ScreenCenter.X + (float)transformed.X * (ESP_Config.ScreenCenter.X / MathF.Tan(usedFOV * UE_Math.F_FOV_DEG_TO_RAD)) / zDiv;
            screenPos.Y = ESP_Config.ScreenCenter.Y - (float)transformed.Y * (ESP_Config.ScreenCenter.X / MathF.Tan(usedFOV * UE_Math.F_FOV_DEG_TO_RAD)) / zDiv;

            if (!float.IsNormal(screenPos.X) ||
                !float.IsNormal(screenPos.Y) ||
                IsOffscreen(in screenPos))
            {
                return false;
            }

            return true;
        }

        private static bool IsOffscreen(in Vector2 screenPos)
        {
            return screenPos.X > ESP_Config.ScreenRenderBounds.maxX || screenPos.X < ESP_Config.ScreenRenderBounds.minX || screenPos.Y > ESP_Config.ScreenRenderBounds.maxY || screenPos.Y < ESP_Config.ScreenRenderBounds.minY;
        }
    }
}
