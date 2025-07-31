namespace arena_dma_backend.Misc
{
    public struct LoopIndexer(int startIndex = 0)
    {
        public static implicit operator int(LoopIndexer x) => x.Get();

        private readonly int _startIndex = startIndex;
        private int _index = startIndex;

        public void Set(int i) => _index = i;

        public void Reset() => _index = _startIndex;

        public void Zero() => _index = 0;

        public void Inc() => _index++;

        public void Dec() => _index--;

        public readonly int Get() => _index;
    }
}
