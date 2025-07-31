namespace cs2_dma_esp.CS2
{
    public static class Maps
    {
        public static readonly Dictionary<string, (float PosX, float PosY, float Scale)> Coordinates = new()
        {
            { "cs_italy",    (-2647f, 2592f, 4.6f) },
            { "cs_office",   (-1838f, 1858f, 4.1f) },
            { "de_ancient",  (-2953f, 2164f, 5f) },
            { "de_anubis",   (-2796f, 3328f, 5.22f) },
            { "de_dust",     (-2850f, 4073f, 6f) },
            { "de_dust2",    (-2476f, 3239f, 4.4f) },
            { "de_inferno",  (-2087f, 3870f, 4.9f) },
            { "de_mirage",   (-3230f, 1713f, 5f) },
            { "de_nuke",     (-3453f, 2887f, 7f) },
            { "de_overpass", (-4831f, 1781f, 5.2f) },
            { "de_vertigo",  (-3168f, 1762f, 4f) },
        };

        public static readonly Dictionary<string, string> Names = new()
        {
            { "cs_italy", "Italy" },
            { "cs_office", "Office" },
            { "de_ancient", "Ancient" },
            { "de_anubis", "Anubis" },
            { "de_dust", "Dust" },
            { "de_dust2", "Dust II" },
            { "de_inferno", "Inferno" },
            { "de_mirage", "Mirage" },
            { "de_nuke", "Nuke" },
            { "de_overpass", "Overpass" },
            { "de_vertigo", "Vertigo" },
            { "<empty>", "Waiting for match..." }
        };
    }
}
