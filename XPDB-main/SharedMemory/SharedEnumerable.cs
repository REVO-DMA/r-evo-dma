using System.Buffers;
using System.Collections;

namespace SharedMemory
{
    internal readonly struct SharedEnumerable<T> : IReadOnlyList<T>, IDisposable where T : unmanaged
    {
        #region Fields

        public readonly IEnumerator<T> GetEnumerator() => new SharedEnumerator<T>(_array, _length);
        readonly IEnumerator IEnumerable.GetEnumerator() => new SharedEnumerator<T>(_array, _length);

        public readonly int Count => _length;
        public readonly T this[int index] => _array[index];

        public readonly Span<T> Span => _array.AsSpan(0, _length);
        public readonly Memory<T> Memory => _array.AsMemory(0, _length);

        private readonly T[] _array;
        private readonly int _length;

        #endregion

        #region .ctor()

        internal SharedEnumerable(int length)
        {
            _length = length;
            _array = ArrayPool<T>.Shared.Rent(_length);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(_array);
        }

        #endregion
    }
}
