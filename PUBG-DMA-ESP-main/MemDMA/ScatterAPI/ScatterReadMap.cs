using pubg_dma_esp.MemDMA;
using pubg_dma_esp.MemDMA.ScatterAPI;

namespace eft_dma_radar.MemDMA.ScatterAPI
{
    /// <summary>
    /// Provides mapping for a Scatter Read Operation. May contain multiple Scatter Read Rounds.
    /// This API is *NOT* Thread Safe! Keep operations synchronous.
    /// </summary>
    public sealed class ScatterReadMap
    {
        private readonly List<ScatterReadRound> _rounds = new();
        /// <summary>
        /// [Optional] Callback(s) to be executed on completion of *all* scatter read executions.
        /// NOTE: Be sure to handle exceptions!
        /// </summary>
        public List<Action> CompletionCallbacks { get; } = new();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ScatterReadMap()
        {
        }

        /// <summary>
        /// Executes Scatter Read operation as defined per the map.
        /// </summary>
        public void Execute()
        {
            foreach (var round in _rounds)
                round.Run();
            foreach (var cb in CompletionCallbacks)
                cb.Invoke();
        }
        /// <summary>
        /// (Base)
        /// Add scatter read rounds to the operation. Each round is a successive scatter read, you may need multiple
        /// rounds if you have reads dependent on earlier scatter reads result(s).
        /// </summary>
        /// <returns>ScatterReadRound object.</returns>
        public ScatterReadRound AddRound(bool useCache = true)
        {
            var round = new ScatterReadRound(useCache);
            _rounds.Add(round);
            return round;
        }
    }
}