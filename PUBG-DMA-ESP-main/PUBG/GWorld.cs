namespace pubg_dma_esp.PUBG
{
    public static class GWorld
    {
        public static void Get()
        {
            try
            {
                ulong gWorld = Memory.ReadPtr(Memory.ModuleBase + Offsets.GWorld.Base);
                ulong decryptedGWorld = AC.Xenuine.Decrypt(gWorld);

                Manager.Fields.GWorld = decryptedGWorld;

                Logger.WriteLine($"Got GWorld: 0x{Manager.Fields.GWorld:X}");
            }
            catch (Exception ex)
            {
                Manager.Fields.GWorld = 0x0;

                Logger.WriteLine($"[GWorld] Error while getting: {ex}");
            }
        }

        public static void GetCurrentLevel()
        {
            try
            {
                ulong currentLevel = Memory.ReadPtr(Manager.Fields.GWorld + Offsets.GWorld.CurrentLevel);
                ulong decryptedCurrentLevel = AC.Xenuine.Decrypt(currentLevel);

                Manager.Fields.CurrentLevel = decryptedCurrentLevel;

                Logger.WriteLine($"Got CurrentLevel: 0x{Manager.Fields.CurrentLevel:X}");
            }
            catch (Exception ex)
            {
                Manager.Fields.CurrentLevel = 0x0;

                Logger.WriteLine($"[GetCurrentLevel] Error while getting: {ex}");
            }
        }

        public static void GetGameInstance()
        {
            try
            {
                ulong gameInstance = Memory.ReadPtr(Manager.Fields.GWorld + Offsets.GWorld.GameInstance);
                ulong decryptedGameInstance = AC.Xenuine.Decrypt(gameInstance);

                Manager.Fields.GameInstance = decryptedGameInstance;

                Logger.WriteLine($"Got GameInstance: 0x{Manager.Fields.GameInstance:X}");
            }
            catch (Exception ex)
            {
                Manager.Fields.GameInstance = 0x0;

                Logger.WriteLine($"[GetGameInstance] Error while getting: {ex}");
            }
        }

        public static void GetLocalPlayers()
        {
            try
            {
                ulong localPlayers = Memory.ReadPtr(Manager.Fields.GameInstance + Offsets.GameInstance.LocalPlayers);
                ulong p1 = Memory.ReadPtr(localPlayers);
                ulong decryptedLocalPlayer = AC.Xenuine.Decrypt(p1);

                Manager.Fields.LocalPlayer = decryptedLocalPlayer;

                Logger.WriteLine($"Got LocalPlayers: 0x{Manager.Fields.LocalPlayer:X}");
            }
            catch (Exception ex)
            {
                Manager.Fields.LocalPlayer = 0x0;

                Logger.WriteLine($"[GetLocalPlayers] Error while getting: {ex}");
            }
        }
    }
}
