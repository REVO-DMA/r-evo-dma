namespace Tarkov_DMA_Backend.Tarkov.GameAPI
{
    public class PlayerProfile
    {
        public class RootObject
        {
            public int err { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Info info { get; set; }
            public PmcStats pmcStats { get; set; }
        }

        public class Info
        {
            public string nickname { get; set; }
            public string side { get; set; }
            public int experience { get; set; }
            public int memberCategory { get; set; }
        }

        public class PmcStats
        {
            public EFT_Stats eft { get; set; }
        }

        public class EFT_Stats
        {
            public ulong totalInGameTime { get; set; }
            public OverAllCounters overAllCounters { get; set; }
        }

        public class OverAllCounters
        {
            public List<ItemCounter> Items { get; set; }
        }

        public class ItemCounter
        {
            public List<string> Key { get; set; }
            public int Value { get; set; }
        }
    }
}
