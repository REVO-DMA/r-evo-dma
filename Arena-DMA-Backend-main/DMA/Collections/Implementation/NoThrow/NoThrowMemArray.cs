namespace arena_dma_backend.DMA.Collections.Implementation.NoThrow
{
    public static class NoThrowMemArray<T> where T : unmanaged
    {
        public static MemArray<T> Get(ulong addr, int count = -1, bool useCache = true)
        {
            try
            {
                return new(addr, count, useCache);
            }
            catch
            {
                return default;
            }
        }
    }
}
