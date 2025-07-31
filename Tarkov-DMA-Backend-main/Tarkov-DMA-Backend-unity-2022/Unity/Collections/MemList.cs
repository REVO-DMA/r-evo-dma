using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# List
    /// </summary>
    /// <typeparam name="T">Collection Type</typeparam>
    public sealed class MemList<T> where T : struct
    {
        private readonly List<T> _items = new();
        /// <summary>
        /// List Items.
        /// </summary>
        public IReadOnlyList<T> Items => _items;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addr">Base Address for this collection.</param>
        /// <param name="useCache">Perform cached reading.</param>
        public MemList(ulong addr, bool useCache = true)
        {
            var count = Memory.ReadValue<int>(addr + UnityOffsets.UnityList.Count, useCache);
            if (count > 16384 || count < 0) // Bounds Safety
                throw new ValueOutOfRangeException(nameof(count));
            if (count == 0) // Collection is empty - return
                return;

            var listBase = Memory.ReadPtr(addr + UnityOffsets.UnityList.Base, useCache) + UnityOffsets.UnityListBase.Start;
            var tSize = Unsafe.SizeOf<T>();
            var listSize = tSize * count;

            var buf = Memory.ReadBuffer(listBase, listSize, useCache).AsSpan();

            for (int i = 0; i < listSize; i += tSize)
            {
                var tItem = MemoryMarshal.Read<T>(buf.Slice(i, tSize));
                _items.Add(tItem);
            }
        }
    }
}
