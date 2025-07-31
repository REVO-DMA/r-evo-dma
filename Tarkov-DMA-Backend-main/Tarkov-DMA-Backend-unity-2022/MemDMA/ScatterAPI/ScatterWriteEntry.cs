namespace Tarkov_DMA_Backend.MemDMA.ScatterAPI
{
    /// <summary>
    /// Defines a single Write in a Scatter Operation.
    /// </summary>
    public readonly struct ScatterWriteEntry
    {
        /// <summary>
        /// Virtual address to write to.
        /// </summary>
        public readonly ulong Va { get; init; }
        /// <summary>
        /// Value to write (in bytes).
        /// </summary>
        public readonly byte[] Value { get; init; }

        public static ScatterWriteEntry Create<T>(ulong va, T value) where T : struct
        {
            var bytes = new byte[Unsafe.SizeOf<T>()];
            MemoryMarshal.Write(bytes, in value);
            return new ScatterWriteEntry()
            {
                Va = va,
                Value = bytes
            };
        }

        public static ScatterWriteEntry Create(ulong va, byte[] bytes)
        {
            return new ScatterWriteEntry()
            {
                Va = va,
                Value = bytes
            };
        }
    }
}
