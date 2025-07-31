namespace pubg_dma_esp.PUBG
{
    public static class GNames
    {
        private static readonly ConcurrentDictionary<uint, string> FNameCache = new();

        public static string GetFNameFromCache(uint nameIndex)
        {
            if (FNameCache.TryGetValue(nameIndex, out var name))
                return name;
            else
                return null;
        }

        public static bool AddFNameToCache(uint nameIndex, string fName)
        {
            return FNameCache.TryAdd(nameIndex, fName);
        }

        public static void Get()
        {
            try
            {
                ulong gNamesBase = Memory.ModuleBase + Offsets.GName.Base;
                if (Offsets.GName.OffsetBase) gNamesBase += 0x20;

                ulong decryptedGNames = Memory.ReadPtr(gNamesBase);
                decryptedGNames = AC.Xenuine.Decrypt(decryptedGNames);
                decryptedGNames = Memory.ReadPtr(decryptedGNames);
                decryptedGNames = AC.Xenuine.Decrypt(decryptedGNames);

                Manager.Fields.GNames = decryptedGNames;

                Logger.WriteLine($"Got GNames: 0x{Manager.Fields.GNames:X}");
            }
            catch (Exception ex)
            {
                Manager.Fields.GNames = 0x0;

                Logger.WriteLine($"[GNames] Error while getting: {ex}");
            }
        }
    }
}
