using apex_dma_esp.MemDMA.ScatterAPI;

namespace apex_dma_esp.MemDMA.ScatterAPI
{
    /// <summary>
    /// Provides mapping for a Scatter Read Operation.
    /// May contain multiple Scatter Read Rounds.
    /// </summary>
    public sealed class ScatterReadMap
    {
        private readonly List<ScatterReadRound> _rounds = new();
        private readonly Dictionary<int, Dictionary<int, IScatterEntry>> _results = new();
        /// <summary>
        /// Contains results from Scatter Read after Execute() is performed. First key is Index, Second Key ID.
        /// </summary>
        public IReadOnlyDictionary<int, Dictionary<int, IScatterEntry>> Results => _results;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="indexCount">Number of indexes in the scatter read loop.</param>
        public ScatterReadMap(int indexCount)
        {
            for (int i = 0; i < indexCount; i++)
            {
                _results.Add(i, new());
            }
        }

        /// <summary>
        /// Executes Scatter Read operation as defined per the map.
        /// </summary>
        public void Execute()
        {
            foreach (var round in _rounds)
            {
                round.Run();
            }
        }
        /// <summary>
        /// (Base)
        /// Add scatter read rounds to the operation. Each round is a successive scatter read, you may need multiple
        /// rounds if you have reads dependent on earlier scatter reads result(s).
        /// </summary>
        /// <returns>ScatterReadRound object.</returns>
        public ScatterReadRound AddRound(bool useCache = true)
        {
            var round = new ScatterReadRound(_results, useCache);
            _rounds.Add(round);
            return round;
        }
    }
}