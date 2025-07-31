using System.Collections;

namespace arena_dma_backend.DMA.Collections
{
    public struct SharedMemEnumerator<T> : IEnumerator<T>
    {
        private readonly T[] _mem;
        private readonly int _count;
        private int _index;

        public SharedMemEnumerator(T[] mem, int count)
        {
            _mem = mem;
            _count = count;
            _index = -1;
        }

        public readonly T Current
        {
            get
            {
                if (_index < 0 || _index >= _count)
                    throw new IndexOutOfRangeException();
                return _mem[_index];
            }
        }

        readonly object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            _index++;
            return _index < _count;
        }

        public void Reset()
        {
            _index = -1;
        }

        public readonly void Dispose()
        {
        }
    }
}
