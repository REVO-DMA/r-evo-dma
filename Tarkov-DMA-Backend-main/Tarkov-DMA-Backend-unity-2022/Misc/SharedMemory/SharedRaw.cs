using System.Buffers;

namespace SharedMemory
{
    public readonly struct SharedRaw<T> : IDisposable where T : unmanaged
    {
        #region Fields

        public readonly T[] Array;
        public readonly Span<T> Span => Array.AsSpan(0, _length);
        public readonly ReadOnlySpan<T> ReadOnlySpan => Span;

        private readonly int _length;

        #endregion

        #region .ctor()

        public SharedRaw(int length)
        {
            _length = length;
            Array = ArrayPool<T>.Shared.Rent(length);
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            ArrayPool<T>.Shared.Return(Array);
        }

        #endregion
    }
}
