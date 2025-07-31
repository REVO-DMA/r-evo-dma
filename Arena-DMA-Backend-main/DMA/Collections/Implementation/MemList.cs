using System.Collections;

namespace arena_dma_backend.DMA.Collections.Implementation
{
    public readonly struct MemList<T> : IMemCollection<T> where T : unmanaged
    {
        public const uint CountOffset = 0x18;
        public const uint ArrOffset = 0x10;
        public const uint ArrStartOffset = 0x20;

        private readonly SharedMemory<T> _mem;

        public MemList(ulong addr, bool useCache = true)
        {
            var count = Memory.ReadValue<int>(addr + CountOffset, useCache);
            if (count > 4096 || count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                _mem = new();
                return;
            }

            var listBase = Memory.ReadPtr(addr + ArrOffset, useCache) + ArrStartOffset;
            var mem = new SharedMemory<T>(count);

            try
            {
                Memory.ReadBuffer(listBase, mem.Span, useCache);
                _mem = mem;
            }
            catch
            {
                mem.Dispose();
                throw;
            }
        }

        public readonly int Count => _mem.Count;

        public readonly T this[int index] => _mem[index];

        public readonly IEnumerator<T> GetEnumerator() => _mem.GetEnumerator();

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly void Dispose() => _mem.Dispose();
    }
}
