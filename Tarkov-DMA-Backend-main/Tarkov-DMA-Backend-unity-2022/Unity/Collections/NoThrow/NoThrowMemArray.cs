namespace Tarkov_DMA_Backend.Unity.Collections.NoThrow
{
    public static class NoThrowMemArray<T> where T : unmanaged
    {
        public static MemArray<T> Get(ulong addr, int count = -1, bool useCache = true)
        {
            try
            {
                return new(addr, useCache, count);
            }
            catch
            {
                return default;
            }
        }
    }
}
