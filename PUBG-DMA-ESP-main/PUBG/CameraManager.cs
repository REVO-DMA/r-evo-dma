namespace pubg_dma_esp.PUBG
{
    public static class CameraManager
    {
        [StructLayout(LayoutKind.Explicit, Pack = 1)]
        public unsafe struct CameraInfo
        {
            [FieldOffset(0x0)]
            public Vector3 Location;
            [FieldOffset(0x18)]
            public UE_Math.FRotator Rotation;
            [FieldOffset(0x34)]
            public float FOV;
        }

        public static CameraInfo GetCameraInfo()
        {
            if (!LocalPlayer.IsValid())
                return new();

            var cameraInfo = Memory.ReadValue<CameraInfo>(LocalPlayer.CameraManager + Offsets.CameraManager.InfoBase, false);

            if (Offsets.Util.CameraDebug)
            {
                Logger.WriteLine("CAMERA INFO==================================");
                Logger.WriteLine($"Location -> {cameraInfo.Location}");
                Logger.WriteLine($"FOV -> {cameraInfo.FOV}");
                Logger.WriteLine($"Rotation -> {cameraInfo.Rotation}");
                Logger.WriteLine("CAMERA INFO==================================");
            }

            return cameraInfo;
        }
    }
}
