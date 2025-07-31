using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# Array
    /// </summary>
    /// <typeparam name="T">Array Type</typeparam>
    public sealed class MemArray<T> where T : struct
    {
        private readonly T[] _items;
        /// <summary>
        /// Array Items.
        /// </summary>
        public IReadOnlyList<T> Items => _items;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addr">Base Address for this collection.</param>
        /// <param name="useCache">Perform cached reading.</param>
        public MemArray(ulong addr, bool useCache = true, int count = -1)
        {
            if (count == -1)
            {
                count = Memory.ReadValue<int>(addr + 0x18, useCache);
                if (count > 16384 || count < 0) // Bounds Safety
                    throw new ValueOutOfRangeException(nameof(count));
            }

            _items = new T[count];

            if (count == 0)
                return;

            var arrayBase = addr + 0x20;
            var tSize = Unsafe.SizeOf<T>();
            int arraySize = count * tSize;

            var buf = Memory.ReadBuffer(arrayBase, arraySize, useCache).AsSpan();

            for (int i = 0; i < count; i++)
            {
                var index = i * tSize;
                var tItem = MemoryMarshal.Read<T>(buf.Slice(index, tSize));
                _items[i] = tItem;
            }
        }
    }
}
