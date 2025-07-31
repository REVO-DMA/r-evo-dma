using arena_dma_backend.DMA.ScatterAPI;
using System.Collections;

namespace arena_dma_backend.DMA.Collections.Implementation
{
    public readonly struct MemArray<T> : IMemCollection<T> where T : unmanaged
    {
        public const uint CountOffset = 0x18;
        public const uint ArrBaseOffset = 0x20;

        private readonly ulong _base;

        private readonly SharedMemory<T> _mem;

        public MemArray(ulong addr, int count = -1, bool useCache = true)
        {
            if (count == -1)
            {
                count = Memory.ReadValue<int>(addr + CountOffset, useCache);

                if (count > 4096 || count < 0) // Bounds Safety
                    throw new ArgumentOutOfRangeException(nameof(count));

                _base = addr + ArrBaseOffset;
            }
            else
            {
                _base = addr;
            }

            if (count == 0) // Collection is empty - return
            {
                _mem = new();
                return;
            }

            var mem = new SharedMemory<T>(count);
            try
            {
                Memory.ReadBuffer(_base, mem.Span, useCache);
                _mem = mem;
            }
            catch
            {
                mem.Dispose();
                throw;
            }
        }

        public readonly void OverwriteValues(T newValue)
        {
            InternalOverwriteValues(newValue);

            Memory.WriteBuffer(_base, _mem.Span);
        }

        public readonly void OverwriteValues(ref List<ScatterWriteEntry> writes, T newValue)
        {
            InternalOverwriteValues(newValue);

            writes.Add(ScatterWriteEntry.Create(_base, _mem.Span));
        }

        public readonly int Count => _mem.Count;

        public readonly T this[int index] => _mem[index];

        public readonly IEnumerator<T> GetEnumerator() => _mem.GetEnumerator();

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly void Dispose() => _mem.Dispose();

        private readonly void InternalOverwriteValues(T newValue)
        {
            for (int i = 0; i < _mem.Count; i++)
                _mem.Span[i] = newValue;
        }
    }
}
