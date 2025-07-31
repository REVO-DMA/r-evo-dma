namespace arena_dma_backend.DMA.ScatterAPI
{
    public readonly struct ScatterWriteEntry
    {
        /// <summary>
        /// Virtual address to write to.
        /// </summary>
        public readonly ulong Va;
        /// <summary>
        /// Value to write.
        /// </summary>
        public readonly byte[] Value;

        public ScatterWriteEntry(ulong va, byte[] value)
        {
            Va = va;
            Value = value;
        }

        public static ScatterWriteEntry Create<T>(ulong va, T value) where T : unmanaged
        {
            var valueSpan = MemoryMarshal.CreateSpan(ref value, 1);
            var bytes = MemoryMarshal.AsBytes(valueSpan).ToArray();
            
            return new ScatterWriteEntry(va, bytes);
        }

        public static ScatterWriteEntry Create<T>(ulong va, Span<T> buffer) where T : unmanaged
        {
            var bytes = MemoryMarshal.AsBytes(buffer).ToArray();
            
            return new ScatterWriteEntry(va, bytes);
        }

        public static ScatterWriteEntry Create(ulong va, byte[] bytes)
        {
            return new ScatterWriteEntry(va, bytes);
        }
    }
}
