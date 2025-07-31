using System.Buffers;

namespace SharedMemory
{
    public unsafe readonly struct SharedPinned<T> : IDisposable where T : unmanaged
    {
        #region Fields

        public readonly Span<T> Span => new(Address, Length);
        public readonly ReadOnlySpan<T> ReadOnlySpan => Span;
        public readonly T* TAddress => (T*)Address;

        public readonly void* Address;

        public readonly T[] Array;
        public readonly int Length;

        private readonly GCHandle _gcHandle;

        #endregion

        #region .ctor()

        public SharedPinned(int length)
        {
            Length = length;
            Array = ArrayPool<T>.Shared.Rent(length);

            _gcHandle = GCHandle.Alloc(Array, GCHandleType.Pinned);
            Address = _gcHandle.AddrOfPinnedObject().ToPointer();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            if (_gcHandle.IsAllocated)
                _gcHandle.Free();

            ArrayPool<T>.Shared.Return(Array);
        }

        #endregion
    }
}
