using System.Runtime.InteropServices;

namespace XPDB.Misc
{
    public struct PinnedObject<T> : IDisposable
    {
        public static implicit operator IntPtr(PinnedObject<T> x) => x.GetPtr();

        private readonly GCHandle _handle;
        private bool _disposed = false;

        public PinnedObject(T obj)
        {
            _handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
        }

        public readonly IntPtr GetPtr()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(PinnedObject<T>));

            return _handle.AddrOfPinnedObject();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _handle.Free();
                _disposed = true;
            }
        }
    }
}
