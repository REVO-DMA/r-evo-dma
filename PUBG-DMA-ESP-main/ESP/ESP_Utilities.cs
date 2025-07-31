using pubg_dma_esp.PUBG;

namespace pubg_dma_esp.ESP
{
    public static class ESP_Utilities
    {
        public static CameraManager.CameraInfo CameraInfo { get; private set; }

        private static Vector3 AxisX;
        private static Vector3 AxisY;
        private static Vector3 AxisZ;

        public static void UpdateW2S()
        {
            CameraInfo = CameraManager.GetCameraInfo();

            Matrix4x4 viewMatrix = UE_Math.CreateViewMatrix(CameraInfo.Rotation);
            AxisX = new(viewMatrix.M11, viewMatrix.M12, viewMatrix.M13);
            AxisY = new(viewMatrix.M21, viewMatrix.M22, viewMatrix.M23);
            AxisZ = new(viewMatrix.M31, viewMatrix.M32, viewMatrix.M33);

            //Logger.WriteLine($"Location: {CameraLocation} | FOV: {CameraFOV}");
        }

        public static bool W2S(Vector3 worldLocation, out Vector2 screenPos)
        {
            Vector3 delta = Vector3.Subtract(worldLocation, CameraInfo.Location);
            Vector3 transformed = new(Vector3.Dot(delta, AxisY), Vector3.Dot(delta, AxisZ), Vector3.Dot(delta, AxisX));
            float zDiv = MathF.Max(transformed.Z, 1f);

            screenPos.X = ESP_Config.ScreenCenter.X + transformed.X * (ESP_Config.ScreenCenter.X / MathF.Tan(CameraInfo.FOV * UE_Math.FOV_DEG_TO_RAD)) / zDiv;
            screenPos.Y = ESP_Config.ScreenCenter.Y - transformed.Y * (ESP_Config.ScreenCenter.X / MathF.Tan(CameraInfo.FOV * UE_Math.FOV_DEG_TO_RAD)) / zDiv;

            if (IsOffscreen(in screenPos))
                return false;

            return true;
        }

        private static bool IsOffscreen(in Vector2 screenPos)
        {
            return screenPos.X > ESP_Config.ScreenRenderBounds.maxX || screenPos.X < ESP_Config.ScreenRenderBounds.minX || screenPos.Y > ESP_Config.ScreenRenderBounds.maxY || screenPos.Y < ESP_Config.ScreenRenderBounds.minY;
        }
    }
}
