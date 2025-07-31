namespace pubg_dma_esp.MemDMA.ScatterAPI
{
    /// <summary>
    /// Single scatter read index. May contain multiple child entries.
    /// </summary>
    public sealed class ScatterReadIndex
    {
        /// <summary>
        /// All read entries for this index.
        /// [KEY] = ID
        /// [VALUE] = IScatterEntry
        /// </summary>
        internal Dictionary<int, IScatterEntry> Entries { get; } = new();
        /// <summary>
        /// Callback to execute on completion.
        /// NOTE: Exceptions will be automatically handled.
        /// </summary>
        public Action<ScatterReadIndex> Callback { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        internal ScatterReadIndex()
        {
        }

        /// <summary>
        /// Execute the User Specified Callback.
        /// </summary>
        internal void ExecuteCallback()
        {
            try 
            {
                Callback?.Invoke(this); 
            }
            catch { } // Handle Unhandled Exceptions to prevent crashing mid-operation
        }

        /// <summary>
        /// Add a scatter read entry to this index.
        /// </summary>
        /// <typeparam name="T">Type to read.</typeparam>
        /// <param name="id">Unique ID for this entry.</param>
        /// <param name="address">Virtual Address to read from.</param>
        /// <param name="cb">Count of bytes to read.</param>
        public ScatterReadEntry<T> AddEntry<T>(int id, ulong address, int cb = 0)
        {
            var entry = new ScatterReadEntry<T>(address, cb);
            Entries.Add(id, entry);
            return entry;
        }

        /// <summary>
        /// Try obtain a result from the requested Entry ID.
        /// </summary>
        /// <typeparam name="TOut">Result Type <typeparamref name="TOut"/></typeparam>
        /// <param name="id">ID for entry to lookup.</param>
        /// <param name="result">Result field to populate.</param>
        /// <returns>True if successful, otherwise False.</returns>
        public bool TryGetResult<TOut>(int id, out TOut result)
        {
            if (Entries.TryGetValue(id, out var entry))
                return entry.TryGetResult<TOut>(out result);
            result = default;
            return false;
        }
    }
}
