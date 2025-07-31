using cs2_dma_esp.MemDMA.ScatterAPI;

namespace cs2_dma_esp.MemDMA.CS2
{
    public sealed class PubgScatterMap : ScatterReadMap
    {
        public PubgScatterMap(int indexCount) : base(indexCount)  { }

        /// <summary>
        /// Add scatter read rounds to the operation. Each round is a successive scatter read, you may need multiple
        /// rounds if you have reads dependent on earlier scatter reads result(s).
        /// </summary>
        /// <returns>ScatterReadRound object.</returns>
        public override PubgScatterRound AddRound(bool useCache = true)
        {
            var round = new PubgScatterRound(_results, useCache);
            Rounds.Add(round);
            return round;
        }
    }
}
