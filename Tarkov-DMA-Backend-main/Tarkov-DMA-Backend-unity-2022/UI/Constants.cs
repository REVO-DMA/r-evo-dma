namespace Tarkov_DMA_Backend.UI
{
    public static class Constants
    {
        public static class IPC
        {
            public static readonly byte[] Delimiter = new byte[] { 0x5c, 0x72, 0x5c, 0x6e }; // \r\n
            public const string RadarPipeName = "57617fb3-3424-4ac7-8598-e2206398acda";
        }

        public static class MessageTypes
        {
            public const byte StaticPlayer = 1;
            public const byte RealtimePlayer = 2;
            public const byte DeferredPlayer = 3;
            public const byte RadarStatus = 20;
            public const byte PlayerStatus = 21;
            public const byte RadarLoot = 22;
            public const byte RadarFeature = 23;
            public const byte MapConfigs = 24;
            public const byte AllocateGrenade = 25;
            public const byte DestroyGrenade = 26;
            public const byte RadarExfils = 27;
            public const byte ESPRealtime = 28;
            public const byte ESPConfig = 29;
            public const byte LootItems = 30;
            public const byte ItemFilter = 31;
            public const byte AuthLogin = 32;
            public const byte AuthStatus = 33;
            public const byte RadarUpdateRate = 34;
            public const byte LootFilter = 35;
            public const byte AllLootFilters = 36;
            public const byte ReloadRadar = 37;
            public const byte FeatureToggleHotkeysSync = 38;
            public const byte FeatureState = 39;
            public const byte RadarReadyForRaid = 40;
            public const byte QuestLocations = 41;
            public const byte PlayerStats = 42;
            public const byte EspPlayerTypeStyles = 43;
        }

        public static class RadarStatuses
        {
            public const byte RaidBegan = 1;
            public const byte RaidEnded = 2;
            // Reserve up to byte 20 for maps on the IPC Client
            public const byte ProcessStarted = 21;
            public const byte ProcessEnded = 22;
            public const byte LoadingFeatures = 23;
            public const byte RestartGame = 24;
            public const byte DisableAutoRamCleaner = 25;
            public const byte TeleportingAICountdown = 26;
            public const byte InitializingDMA = 27;
            public const byte MembershipExpired = 28;
        }

        public static class PlayerStatuses
        {
            public const byte Alive = 1;
            public const byte Die = 2;
            public const byte Exfil = 3;
            public const byte Destroy = 4;
        }

        public static class FeatureSettingTypes
        {
            public const string UnityKeyCode = "UnityKeyCode";
            public const string Bones = "Bones";
        }
    }
}
