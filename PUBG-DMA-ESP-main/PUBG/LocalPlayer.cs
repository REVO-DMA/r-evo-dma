namespace pubg_dma_esp.PUBG
{
    public static class LocalPlayer
    {
        #region Fields

        public static ulong Base { get; private set; }
        public static ulong PlayerController { get; private set; }
        public static ulong CameraManager { get; private set; }

        public static ulong AcknowledgedPawn { get; private set; }

        public static int SpectatorCount { get; set; }

        public static int Team { get; private set; }

        #endregion

        public static void Update(ulong baseAddr)
        {
            try
            {
                Base = baseAddr;

                GetPlayerController();
                GetCameraManager();
                GetAcknowledgedPawn();

                Team = GetTeam(AcknowledgedPawn);

                if (Offsets.Util.OffsetDebug)
                {
                    Logger.WriteLine("LOCAL PLAYER=================================");
                    Logger.WriteLine($"baseAddr -> 0x{baseAddr:X}");
                    Logger.WriteLine($"PlayerController -> 0x{PlayerController:X}");
                    Logger.WriteLine($"CameraManager -> 0x{CameraManager:X}");
                    Logger.WriteLine($"AcknowledgedPawn -> 0x{AcknowledgedPawn:X}");
                    Logger.WriteLine($"Team -> {Team}");
                    Logger.WriteLine("LOCAL PLAYER=================================");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LOCAL PLAYER] Error allocating the LocalPlayer! -> {ex}");
            }
        }

        public static bool IsValid()
        {
            if (Base == 0x0)
                return false;

            return true;
        }

        private static void GetPlayerController()
        {
            PlayerController = AC.Xenuine.Decrypt(Memory.ReadPtr(Base + Offsets.Player.Controller));
        }

        private static void GetAcknowledgedPawn()
        {
            AcknowledgedPawn = AC.Xenuine.Decrypt(Memory.ReadPtr(PlayerController + Offsets.PlayerController.AcknowledgedPawn));
        }

        private static void GetCameraManager()
        {
            CameraManager = Memory.ReadPtr(PlayerController + Offsets.PlayerController.CameraManager);
        }

        private static int GetTeam(ulong playerBase)
        {
            return Memory.ReadValue<int>(playerBase + Offsets.Character.Team);
        }
    }
}
