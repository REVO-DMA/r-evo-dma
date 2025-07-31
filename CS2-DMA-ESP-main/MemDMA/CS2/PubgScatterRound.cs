using cs2_dma_esp.MemDMA.ScatterAPI;

namespace cs2_dma_esp.MemDMA.CS2
{
    public sealed class PubgScatterRound : ScatterReadRound
    {
        public PubgScatterRound(Dictionary<int, Dictionary<int, IScatterEntry>> results, bool useCache) : base(results, useCache) { }

        /// <summary>
        /// Adds a single Scatter Read 
        /// </summary>
        /// <param name="index">For loop index this is associated with.</param>
        /// <param name="id">Random ID number to identify the entry's purpose.</param>
        /// <param name="addr">Address to read from (you can pass a ScatterReadEntry from an earlier round, 
        /// and it will use the result).</param>
        /// <param name="size">Size of oject to read (ONLY for reference types, value types get size from
        /// Type). You canc pass a ScatterReadEntry from an earlier round and it will use the Result.</param>
        /// <param name="offset">Optional offset to add to address (usually in the event that you pass a
        /// ScatterReadEntry to the Addr field).</param>
        /// <returns></returns>
        public override PubgScatterEntry<T> AddEntry<T>(int index, int id, object addr, object? size = null, uint offset = 0x0)
        {
            var entry = new PubgScatterEntry<T>()
            {
                Index = index,
                Id = id,
                Addr = addr,
                Size = size,
                Offset = offset
            };
            Results[index].Add(id, entry);
            Entries.Add(entry);
            return entry;
        }
    }
}
