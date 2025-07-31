namespace pubg_dma_esp.MemDMA.ScatterAPI
{
    public interface IScatterEntry
    {
        /// <summary>
        /// Virtual Address to read from.
        /// </summary>
        ulong Address { get; }
        /// <summary>
        /// Count of bytes to read.
        /// </summary>
        int CB { get; }
        /// <summary>
        /// True if this read has failed, otherwise False.
        /// </summary>
        bool IsFailed { get; set; }

        /// <summary>
        /// Parse the memory buffer and set the result value.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        void SetResult(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Try to obtain the scatter read result.
        /// </summary>
        /// <typeparam name="TOut">Result Type <typeparamref name="TOut"/></typeparam>
        /// <param name="result">Result field to populate.</param>
        /// <returns>True if successful, otherwise False.</returns>
        bool TryGetResult<TOut>(out TOut result);
    }
}
