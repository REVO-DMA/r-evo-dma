using apex_dma_esp.Apex;
using static apex_dma_esp.Apex.ApexMath;

namespace apex_dma_esp.ESP
{
    public static class ESP_Utilities
    {
        public static ViewMatrix ViewMatrix;

        public static void Update()
        {
            ulong RenderPtr = Memory.ReadPtrUnsafe(Memory.ModuleBase + Offsets.VIEWRENDER);
            ulong MatrixPtr = Memory.ReadPtrUnsafe(RenderPtr + Offsets.VIEWMATRIX);
            ViewMatrix = Memory.ReadValue<ViewMatrix>(MatrixPtr, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool W2S(Vector3 worldLocation, out Vector2 screenPos)
        {
            screenPos = default;

            Vector3 transformed = ViewMatrix.Transform(worldLocation);

            if (transformed.Z < 0.001f)
                return false;

            transformed.X *= 1f / transformed.Z;
            transformed.Y *= 1f / transformed.Z;

            screenPos.X = ESP_Config.ESP_ResolutionX / 2f + transformed.X * (ESP_Config.ESP_ResolutionX / 2f);
            screenPos.Y = ESP_Config.ESP_ResolutionY / 2f - transformed.Y * (ESP_Config.ESP_ResolutionY / 2f);

            if (IsOffscreen(screenPos))
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsOffscreen(Vector2 screenPos)
        {
            return screenPos.X > ESP_Config.ESP_ResolutionX || screenPos.X < 0f || screenPos.Y > ESP_Config.ESP_ResolutionY || screenPos.Y < 0f;
        }
    }
}
