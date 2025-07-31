namespace apex_dma_esp.Apex
{
    public static class Level
    {
        public static string Name;
        public static bool IsPlayable;
        public static bool IsFiringRange;

        public static void Get()
        {
            try
            {
                Name = Memory.ReadUtf8String(Memory.ModuleBase + Offsets.LEVEL, 32, false);
                IsPlayable = Name != string.Empty && Name != "mp_lobby";
                IsFiringRange = Name == "mp_rr_canyonlands_staging_mu1";

                Logger.WriteLine($"[LEVEL] Got: Name \"{Name}\" | Playable: {IsPlayable} | Firing Range: {IsFiringRange}");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LEVEL] Error while getting: {ex}");
            }
        }
    }
}
