namespace pubg_dma_esp.MemDMA.ScatterAPI
{
    /// <summary>
    /// Defines a Scatter Read Round. Each round will execute a single scatter read. If you have reads that
    /// are dependent on previous reads (chained pointers for example), you may need multiple rounds.
    /// </summary>
    public sealed class ScatterReadRound
    {
        private readonly Dictionary<int, ScatterReadIndex> _indexes = new();
        private readonly bool _useCache;

        /// <summary>
        /// Returns the requested ScatterReadIndex.
        /// </summary>
        /// <param name="index">Index to retrieve.</param>
        /// <returns>ScatterReadIndex object.</returns>
        public ScatterReadIndex this[int index]
        {
            get
            {
                if (_indexes.TryGetValue(index, out var existing))
                    return existing;
                return _indexes[index] = new();
            }
            set => _indexes[index] = value;
        }

        /// <summary>
        /// Constructor. Do not call directly, use .AddRound() on the map.
        /// </summary>
        /// <param name="useCache">Use caching for this read.</param>
        internal ScatterReadRound(bool useCache)
        {
            _useCache = useCache;
        }

        /// <summary>
        /// ** Internal use only do not use **
        /// </summary>
        internal void Run()
        {
            Memory.ReadScatter(_indexes.Values.SelectMany(x => x.Entries.Values).ToArray(), _useCache);
            foreach (var index in _indexes)
                index.Value.ExecuteCallback();
        }
    }
}
