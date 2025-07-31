using arena_dma_backend.Arena;
using arena_dma_backend.Arena.ESP;

namespace arena_dma_backend.ESP
{
    public static class ESP_Utilities
    {
        public static Matrix4x4 ViewMatrix;

        public static void UpdateW2S()
        {
            ViewMatrix = Memory.ReadValue<Matrix4x4>(ESP_Cache.CameraAddress + UnityOffsets.Camera.ViewMatrix, false);
        }

        /// <summary>
        /// This W2S returns a bool indicating whether or not the item should be rendered.
        /// </summary>
        public static bool W2S(Vector3 inPos, bool scale, out Vector2 outPos)
        {
            outPos = default;

            Vector3 vm1 = new(ViewMatrix.M14, ViewMatrix.M24, ViewMatrix.M34);
            float w = Vector3.Dot(vm1, inPos) + ViewMatrix.M44;

            if (w < 0.098f)
                return false;

            float newX, newY;

            Vector3 vm2 = new(ViewMatrix.M11, ViewMatrix.M21, ViewMatrix.M31);
            newX = Vector3.Dot(vm2, inPos) + ViewMatrix.M41;

            Vector3 vm3 = new(ViewMatrix.M12, ViewMatrix.M22, ViewMatrix.M32);
            newY = Vector3.Dot(vm3, inPos) + ViewMatrix.M42;

            if (ESP_Cache.IsScoped)
            {
                float angleCtg = ESP_Cache.AngleCtg;
                newX /= angleCtg * ESP_Cache.AspectRatio * 0.5f;
                newY /= angleCtg * 0.5f;
            }

            // Apply game resolution
            newX = ESP_Config.GameResolutionX / 2f * (1f + newX / w);
            newY = ESP_Config.GameResolutionY / 2f * (1f - newY / w);

            Vector2 tmpOut = new(newX, newY);

            // Make sure this is not at 0,0
            if (tmpOut == Vector2.Zero) return false;

            // Make sure this is visible on screen
            if (IsOffscreen(tmpOut)) return false;

            if (scale && ESP_Config.ShouldScale) outPos = ScaleESPCoordinate(tmpOut);
            else outPos = tmpOut;

            return true;
        }

        /// <summary>
        /// This W2S always outputs the screen coord even if it would be offscreen.
        /// </summary>
        public static void W2S2(Vector3 inPos, bool scale, out Vector2 outPos)
        {
            Vector3 vm1 = new(ViewMatrix.M14, ViewMatrix.M24, ViewMatrix.M34);
            float w = Vector3.Dot(vm1, inPos) + ViewMatrix.M44;

            float newX, newY;

            Vector3 vm2 = new(ViewMatrix.M11, ViewMatrix.M21, ViewMatrix.M31);
            newX = Vector3.Dot(vm2, inPos) + ViewMatrix.M41;

            Vector3 vm3 = new(ViewMatrix.M12, ViewMatrix.M22, ViewMatrix.M32);
            newY = Vector3.Dot(vm3, inPos) + ViewMatrix.M42;

            if (ESP_Cache.IsScoped)
            {
                float angleCtg = ESP_Cache.AngleCtg;
                newX /= angleCtg * ESP_Cache.AspectRatio * 0.5f;
                newY /= angleCtg * 0.5f;
            }

            // Apply game resolution
            newX = ESP_Config.GameResolutionX / 2f * (1f + newX / w);
            newY = ESP_Config.GameResolutionY / 2f * (1f - newY / w);

            Vector2 tmpOut = new(newX, newY);

            if (scale && ESP_Config.ShouldScale) outPos = ScaleESPCoordinate(tmpOut);
            else outPos = tmpOut;
        }

        private static bool IsOffscreen(in Vector2 screenPos)
        {
            return screenPos.X > ESP_Config.ScreenRenderBounds.maxX || screenPos.X < ESP_Config.ScreenRenderBounds.minX || screenPos.Y > ESP_Config.ScreenRenderBounds.maxY || screenPos.Y < ESP_Config.ScreenRenderBounds.minY;
        }

        private static Vector2 ScaleESPCoordinate(Vector2 coord)
        {
            float transformX = (ESP_Config.GameResolutionX / 2) - (ESP_Config.ESP_ResolutionX / 2);
            float transformY = (ESP_Config.GameResolutionY / 2) - (ESP_Config.ESP_ResolutionY / 2);

            return new(coord.X - transformX, coord.Y - transformY);
        }
    }
}
