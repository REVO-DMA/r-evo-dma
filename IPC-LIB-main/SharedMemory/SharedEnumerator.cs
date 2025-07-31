using System.Collections;

namespace IPC_LIB.SharedMemory
{
    internal struct SharedEnumerator<T> : IEnumerator<T> where T : unmanaged
    {
        #region Fields

        public readonly T Current => _array[_currentIndex];
        readonly object IEnumerator.Current => _array[_currentIndex];

        private readonly T[] _array;
        private readonly int _length;

        private int _currentIndex;

        #endregion

        #region .ctor()

        internal SharedEnumerator(T[] array, int length)
        {
            _array = array;
            _length = length;
            _currentIndex = -1;
        }

        #endregion

        #region Public Methods

        public bool MoveNext()
        {
            _currentIndex++;

            return _currentIndex < _length;
        }

        public void Reset()
        {
            _currentIndex = -1;
        }

        public readonly void Dispose() { }

        #endregion
    }
}
