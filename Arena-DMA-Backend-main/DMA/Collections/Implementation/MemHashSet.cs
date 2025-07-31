using System.Collections;

namespace arena_dma_backend.DMA.Collections.Implementation
{
    public readonly struct MemHashSet<T> : IMemCollection<MemHashEntry<T>> where T : unmanaged
    {
        public const uint CountOffset = 0x3C;
        public const uint ArrOffset = 0x18;
        public const uint ArrStartOffset = 0x20;

        private readonly SharedMemory<MemHashEntry<T>> _mem;

        public MemHashSet(ulong addr, bool useCache = true)
        {
            var count = Memory.ReadValue<int>(addr + CountOffset, useCache);
            if (count > 4096 || count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                _mem = new();
                return;
            }

            var hashSetBase = Memory.ReadPtr(addr + ArrOffset, useCache) + ArrStartOffset;
            var mem = new SharedMemory<MemHashEntry<T>>(count);

            try
            {
                Memory.ReadBuffer(hashSetBase, mem.Span, useCache);
                _mem = mem;
            }
            catch
            {
                mem.Dispose();
                throw;
            }
        }

        public readonly int Count => _mem.Count;

        public readonly MemHashEntry<T> this[int index] => _mem[index];

        public readonly IEnumerator<MemHashEntry<T>> GetEnumerator() => _mem.GetEnumerator();

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly void Dispose() => _mem.Dispose();
    }

    public readonly struct MemHashEntry<T>
    {
        public static implicit operator T(MemHashEntry<T> x) => x.Value;

        private readonly int _hashCode;
        private readonly int _next;
        public readonly T Value;
    }
}
