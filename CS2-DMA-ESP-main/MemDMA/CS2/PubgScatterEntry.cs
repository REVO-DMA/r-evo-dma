using cs2_dma_esp.MemDMA.ScatterAPI;

namespace cs2_dma_esp.MemDMA.CS2
{
    public sealed class PubgScatterEntry<T> : ScatterReadEntry<T>, IScatterEntry
    {
        /// <summary>
        /// Parses the number of bytes to read for this Scatter Read.
        /// Sets the Size property for the object.
        /// </summary>
        /// <returns>Size of read.</returns>
        public override int ParseSize()
        {
            int size = base.ParseSize(); // Check Base Implementation First
            if (size != 0)
                return size;
            this.Size = size;
            return size;
        }
    }
}
