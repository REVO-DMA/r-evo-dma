using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit
{
    public abstract class AlwaysActiveFeature
    {
        /// <summary>
        /// The ID of this feature.
        /// </summary>
        public readonly string ID;
        /// <summary>
        /// Schedule this feature to run immediately upon the next FeaturesController execution loop.
        /// </summary>
        public bool RunImmediately = false;
        /// <summary>
        /// Current state of the feature. Reflects the actual state of the feature. Internally updated by implementers.
        /// </summary>
        public bool CurrentState = false;
        /// <summary>
        /// The internally forced state of this feature.
        /// </summary>
        public bool? OverriddenState = null;

        private readonly int _delayMs;
        private DateTime _lastExecutionTime;
        private readonly object[] _parameters;

        public AlwaysActiveFeature(int delayMs, string id, params object[] parameters)
        {
            ID = id;

            _delayMs = delayMs;
            _lastExecutionTime = DateTime.MinValue;
            _parameters = parameters;
        }

        public void TryRun(ref List<ScatterWriteEntry> writes)
        {
            try
            {
                if (RunImmediately)
                {
                    Run(ref writes, _parameters);
                    RunImmediately = false;
                    return;
                }

                TimeSpan sinceLastExecution = DateTime.UtcNow - _lastExecutionTime;
                if (sinceLastExecution.TotalMilliseconds >= _delayMs)
                {
                    _lastExecutionTime = DateTime.UtcNow;
                    Run(ref writes, _parameters);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[TOOLKIT FEATURE] Error while running {ID} ~ {ex}");
            }
        }

        public abstract void Run(ref List<ScatterWriteEntry> writes, params object[] parameters);
    }
}
