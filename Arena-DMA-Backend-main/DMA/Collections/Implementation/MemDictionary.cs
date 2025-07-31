using arena_dma_backend.DMA.ScatterAPI;
using System.Collections;

namespace arena_dma_backend.DMA.Collections.Implementation
{
    public readonly struct MemDictionary<TKey, TValue> : IMemCollection<MemDictEntry<TKey, TValue>> where TKey : unmanaged where TValue : unmanaged
    {
        public const uint CountOffset = 0x40;
        public const uint ArrOffset = 0x18;
        public const uint ArrStartOffset = 0x20;

        private readonly ulong _base;

        private readonly SharedMemory<MemDictEntry<TKey, TValue>> _mem;

        public MemDictionary(ulong addr, bool useCache = true)
        {
            var count = Memory.ReadValue<int>(addr + CountOffset, useCache);
            if (count > 4096 || count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
            {
                _mem = new();
                return;
            }

            _base = Memory.ReadPtr(addr + ArrOffset, useCache) + ArrStartOffset;
            var mem = new SharedMemory<MemDictEntry<TKey, TValue>>(count);
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

        public readonly void OverwriteValues(TValue newValue)
        {
            InternalOverwriteValues(newValue);

            Memory.WriteBuffer(_base, _mem.Span);
        }

        public readonly void OverwriteValues(ref List<ScatterWriteEntry> writes, TValue newValue)
        {
            InternalOverwriteValues(newValue);

            writes.Add(ScatterWriteEntry.Create(_base, _mem.Span));
        }

        public readonly void OverwriteKeys(TKey newKey)
        {
            InternalOverwriteKeys(newKey);

            Memory.WriteBuffer(_base, _mem.Span);
        }

        public readonly void OverwriteKeys(ref List<ScatterWriteEntry> writes, TKey newKey)
        {
            InternalOverwriteKeys(newKey);

            writes.Add(ScatterWriteEntry.Create(_base, _mem.Span));
        }

        public readonly int Count => _mem.Count;

        public readonly MemDictEntry<TKey, TValue> this[int index] => _mem[index];

        public readonly IEnumerator<MemDictEntry<TKey, TValue>> GetEnumerator() => _mem.GetEnumerator();

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public readonly void Dispose() => _mem.Dispose();

        private readonly void InternalOverwriteKeys(TKey newKey)
        {
            for (int i = 0; i < _mem.Count; i++)
            {
                var curr = _mem.Span[i];

                _mem.Span[i] = new(newKey, curr.Value);
            }
        }

        private readonly void InternalOverwriteValues(TValue newValue)
        {
            for (int i = 0; i < _mem.Count; i++)
            {
                var curr = _mem.Span[i];

                _mem.Span[i] = new(curr.Key, newValue);
            }
        }
    }

    public readonly struct MemDictEntry<TKey, TValue>
    {
        private readonly uint _hashCode;
        private readonly int _next;
        public readonly TKey Key;
        public readonly TValue Value;

        public MemDictEntry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}