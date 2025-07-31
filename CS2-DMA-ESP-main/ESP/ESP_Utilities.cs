namespace cs2_dma_esp.ESP
{
    public static class ESP_Utilities
    {
        public static Matrix4x4 ViewMatrix;

        public static bool WorldToScreen(Vector3 WorldPos, out Vector2 ScreenPos)
		{
			ScreenPos = default;

            float SightX = ESP_Config.ESP_ResolutionX / 2, SightY = ESP_Config.ESP_ResolutionY / 2;

            float View = ViewMatrix.M31 * WorldPos.X + ViewMatrix.M32 * WorldPos.Y + ViewMatrix.M33 * WorldPos.Z + ViewMatrix.M34;
		
			if (View <= 0.01)
				return false;

			ScreenPos.X = SightX + (ViewMatrix.M11 * WorldPos.X + ViewMatrix.M12 * WorldPos.Y + ViewMatrix.M13 * WorldPos.Z + ViewMatrix.M14) / View * SightX;
			ScreenPos.Y = SightY - (ViewMatrix.M21 * WorldPos.X + ViewMatrix.M22 * WorldPos.Y + ViewMatrix.M23 * WorldPos.Z + ViewMatrix.M24) / View * SightY;
		
			return true;
		}
	}
}
