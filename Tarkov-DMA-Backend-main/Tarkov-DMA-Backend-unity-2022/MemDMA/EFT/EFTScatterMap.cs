using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.MemDMA.EFT
{
    public sealed class EFTScatterMap : ScatterReadMap
    {
        public EFTScatterMap(int indexCount) : base(indexCount) 
        { 
        }

        /// <summary>
        /// Add scatter read rounds to the operation. Each round is a successive scatter read, you may need multiple
        /// rounds if you have reads dependent on earlier scatter reads result(s).
        /// </summary>
        /// <returns>ScatterReadRound object.</returns>
        public override EFTScatterRound AddRound(bool useCache = true)
        {
            var round = new EFTScatterRound(_results, useCache);
            Rounds.Add(round);
            return round;
        }
    }
}
