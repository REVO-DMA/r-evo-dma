using System.Collections;

namespace pubg_dma_esp.MemDMA.Collections
{
    /// <summary>
    /// Represents a container that implements IReadOnlyList <typeparamref name="T"/>, and is
    /// backed by the Shared ArrayPool.
    /// Container must be populated by the caller.
    /// Container must be disposed when finished.
    /// </summary>
    /// <typeparam name="T">Value type <typeparamref name="T"/> for elements.</typeparam>
    public sealed class SharedContainer<T> : IMemCollection<T>
        where T : unmanaged
    {
        private readonly SharedMemory<T> _mem;

        /// <summary>
        /// Returns a Span <typeparamref name="T"/> over this instance of the requested count (trims excess).
        /// </summary>
        public Span<T> Span => _mem.Span;

        /// <summary>
        /// Returns a Memory <typeparamref name="T"/> over this instance of the requested count (trims excess).
        /// </summary>
        public Memory<T> Memory => _mem.Memory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="count">Number of <typeparamref name="T"/> elements in this container.</param>
        public SharedContainer(int count)
        {
            _mem = new(count);
        }

        #region IReadOnlyList
        public int Count => _mem.Count;

        public T this[int index] => _mem[index];

        public IEnumerator<T> GetEnumerator() => 
            _mem.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        #region IDisposable
        private bool _disposed = false;
        /// <summary>
        /// Returns the Internal Array to the Shared Array Pool.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;
            _mem.Dispose();
            _disposed = true;
        }
        #endregion
    }
}
