using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.Collections
{
    /// <summary>
    /// DMA Wrapper for a C# Dictionary
    /// </summary>
    /// <typeparam name="T1">Key Type</typeparam>
    /// <typeparam name="T2">Value Type</typeparam>
    public sealed class MemDictionary<T1, T2> where T1 : struct where T2 : struct
    {
        private readonly ulong _base;
        private readonly bool _useCache;

        private readonly ulong _dictBase;
        private readonly int _dictItemsCount;
        private readonly int _dictSize;
        private readonly byte[] _dictBuff;

        private readonly int _tKeySize;
        private readonly int _tValueSize;

        private readonly Dictionary<T1, T2> _items = new();
        /// <summary>
        /// Dictionary Items.
        /// </summary>
        public IReadOnlyDictionary<T1, T2> Items => _items;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="addr">Base Address for this collection.</param>
        /// <param name="useCache">Perform cached reading.</param>
        public MemDictionary(ulong addr, bool useCache = true, int maxSize = 16384) // System.Collections.Generic.Dictionary
        {
            _base = addr;
            _useCache = useCache;

            _dictItemsCount = GetSize(maxSize);
            if (_dictItemsCount == 0) // Collection is empty - return
                return;

            _dictBase = Memory.ReadPtr(addr + 0x18, useCache) + 0x28;
            _tKeySize = Marshal.SizeOf<T1>();
            _tValueSize = Marshal.SizeOf<T2>();
            _dictSize = (_tKeySize + _tValueSize + 0x8) * _dictItemsCount;

            _dictBuff = Memory.ReadBuffer(_dictBase, _dictSize, useCache);

            var span = _dictBuff.AsSpan();
            for (int i = 0; i < _dictSize; i += _tKeySize + _tValueSize + 0x8) // parse buffer for entries
            {
                var tKey = MemoryMarshal.Read<T1>(span.Slice(i, _tKeySize));
                var tValue = MemoryMarshal.Read<T2>(span.Slice(i + _tKeySize, _tValueSize));
                _items.Add(tKey, tValue);
            }
        }

        public void OverwriteKeys<T>(T newKey) where T : struct
        {
            if (Marshal.SizeOf<T>() != _tKeySize)
                throw new Exception("[MEM DICTIONARY] The new key's type is not the same size as the original!");

            byte[] buff = new byte[_dictBuff.Length];
            System.Buffer.BlockCopy(_dictBuff, 0, buff, 0, _dictBuff.Length);

            var span = buff.AsSpan();
            for (int i = 0; i < _dictSize; i += _tKeySize + _tValueSize + 0x8) // parse buffer for entries
            {
                MemoryMarshal.Write<T>(span.Slice(i, _tKeySize), in newKey);
            }

            Memory.WriteBuffer(_dictBase, buff);
        }

        public void OverwriteKeys<T>(ref List<ScatterWriteEntry> writes, T newKey) where T : struct
        {
            if (Marshal.SizeOf<T>() != _tKeySize)
                throw new Exception("[MEM DICTIONARY] The new key's type is not the same size as the original!");

            byte[] buff = new byte[_dictBuff.Length];
            System.Buffer.BlockCopy(_dictBuff, 0, buff, 0, _dictBuff.Length);

            var span = buff.AsSpan();
            for (int i = 0; i < _dictSize; i += _tKeySize + _tValueSize + 0x8) // parse buffer for entries
            {
                MemoryMarshal.Write<T>(span.Slice(i, _tKeySize), in newKey);
            }

            writes.Add(ScatterWriteEntry.Create(_dictBase, buff));
        }

        public void OverwriteValues<T>(T newValue) where T : struct
        {
            if (Marshal.SizeOf<T>() != _tValueSize)
                throw new Exception("[MEM DICTIONARY] The new value's type is not the same size as the original!");

            byte[] buff = new byte[_dictBuff.Length];
            System.Buffer.BlockCopy(_dictBuff, 0, buff, 0, _dictBuff.Length);

            var span = buff.AsSpan();
            for (int i = 0; i < _dictSize; i += _tKeySize + _tValueSize + 0x8) // parse buffer for entries
            {
                MemoryMarshal.Write(span.Slice(i + _tKeySize, _tValueSize), in newValue);
            }

            Memory.WriteBuffer(_dictBase, buff);
        }

        public void OverwriteValues<T>(ref List<ScatterWriteEntry> writes, T newValue) where T : struct
        {
            if (Marshal.SizeOf<T>() != _tValueSize)
                throw new Exception("[MEM DICTIONARY] The new value's type is not the same size as the original!");

            byte[] buff = new byte[_dictBuff.Length];
            System.Buffer.BlockCopy(_dictBuff, 0, buff, 0, _dictBuff.Length);

            var span = buff.AsSpan();
            for (int i = 0; i < _dictSize; i += _tKeySize + _tValueSize + 0x8) // parse buffer for entries
            {
                MemoryMarshal.Write(span.Slice(i + _tKeySize, _tValueSize), in newValue);
            }

            writes.Add(ScatterWriteEntry.Create(_dictBase, buff));
        }

        private int GetSize(int maxSize = 16384)
        {
            var count = Memory.ReadValue<int>(_base + 0x40, _useCache);
            if (count > maxSize || count < 0) // Bounds Safety
                throw new ValueOutOfRangeException(nameof(count));

            return count;
        }
    }
}
