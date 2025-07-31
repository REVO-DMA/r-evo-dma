using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# HashSet
    /// </summary>
    /// <typeparam name="T">Collection Type</typeparam>
    public sealed class MemHashSet<T> where T : struct
    {
        private readonly HashSet<T> _items = new();
        /// <summary>
        /// HashSet Items.
        /// </summary>
        public IReadOnlyCollection<T> Items => _items;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addr">Base Address for this collection.</param>
        /// <param name="useCache">Perform cached reading.</param>
        public MemHashSet(ulong addr, bool useCache = true)
        {
            var count = Memory.ReadValue<int>(addr + 0x3C, useCache);
            if (count > 16384 || count < 0) // Bounds Safety
                throw new ValueOutOfRangeException(nameof(count));
            if (count == 0) // Collection is empty - return
                return;

            var hashSetBase = Memory.ReadPtr(addr + 0x18, useCache) + 0x20;
            var tSize = Unsafe.SizeOf<T>();
            var hashSetSize = (8 + tSize) * count;

            var buf = Memory.ReadBuffer(hashSetBase, hashSetSize, useCache).AsSpan();

            for (int i = 8; i < hashSetSize; i += tSize + 8)
            {
                var tItem = MemoryMarshal.Read<T>(buf.Slice(i, tSize));
                _items.Add(tItem);
            }
        }
    }
}
