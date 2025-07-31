using System.Buffers;
using System.Collections;

namespace cs2_dma_esp.MemDMA.Collections
{
    /// <summary>
    /// Wraps a simple Memory Rental via the Shared Array Pool.
    /// Implements IMemCollection <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Value Type <typeparamref name="T"/></typeparam>
    public readonly struct SharedMemory<T> : IMemCollection<T>, IDisposable
        where T : unmanaged
    {
        private readonly int _count;
        private readonly T[] _arr;

        /// <summary>
        /// Returns a Span <typeparamref name="T"/> over this instance of the requested count (trims excess).
        /// </summary>
        public readonly Span<T> Span => _arr.AsSpan(0, _count);

        /// <summary>
        /// Returns a Memory <typeparamref name="T"/> over this instance of the requested count (trims excess).
        /// </summary>
        public readonly Memory<T> Memory => _arr.AsMemory(0, _count);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="count">Number of elements in the array.</param>
        public SharedMemory(int count)
        {
            if (count > 0)
            {
                _count = count;
                _arr = ArrayPool<T>.Shared.Rent(count);
            }
            else
                this = new();
        }

        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public SharedMemory()
        {
            _count = 0;
            _arr = Array.Empty<T>();
        }

        #region IReadOnlyList
        public readonly int Count => _count;

        public readonly T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                return _arr[index];
            }
        }

        public readonly IEnumerator<T> GetEnumerator() =>
            new SharedMemEnumerator<T>(_arr, _count);

        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region IDisposable
        /// <summary>
        /// Returns the Internal Array to the Shared Array Pool.
        /// </summary>
        public readonly void Dispose()
        {
            if (_arr is null || _arr.Length == 0)
                return;
            ArrayPool<T>.Shared.Return(_arr);
        }
        #endregion
    }
}
