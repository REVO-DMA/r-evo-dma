using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.MemDMA.EFT
{
    public sealed class EFTScatterEntry<T> : ScatterReadEntry<T>, IScatterEntry
    {
        /// <summary>
        /// Parses the number of bytes to read for this Scatter Read.
        /// Sets the Size property for the object.
        /// </summary>
        /// <returns>Size of read.</returns>
        public override int ParseSize()
        {
            int size = base.ParseSize(); // Check Base Implementation First
            if (size != 0)
                return size;
            if (this.Size is IScatterEntry sizeObj &&
                sizeObj.TryGetResult<int>(out var refSize))
            {
                if (this.Type == typeof(List<int>)) // Special size case for Indices
                    size = (refSize + 1) * 4; // Hardcode Multiplier
                else if (this.Type == typeof(Vector128<float>[])) // Special size case for Vertices
                    size = (3 * refSize + 3) * 16; // Hardcode Multiplier
            }
            this.Size = size;
            return size;
        }

        /// <summary>
        /// Set the Result from a Class Type.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        protected override void SetClassResult(byte[] buffer)
        {
            if (Type == typeof(List<int>)) // indices
            {
                var value = new List<int>();
                var spanBuf = buffer.AsSpan();
                for (var i = 0; i < buffer.Length; i += 4)
                {
                    value.Add(MemoryMarshal.Read<int>(spanBuf.Slice(i, 4)));
                }
                if (value is T result)
                    Result = result;
            }
            else if (Type == typeof(Vector128<float>[])) // vertices
            {
                const int chunkSize = 16;

                var value = new Vector128<float>[buffer.Length / chunkSize];
                var spanBuf = buffer.AsSpan();
                int outputPos = 0;
                for (int i = 0; i < buffer.Length; i += chunkSize)
                {
                    ReadOnlySpan<byte> span = spanBuf.Slice(i, chunkSize);
                    value[outputPos++] = Vector128.Create(span).AsSingle();
                }

                if (value is T result)
                    Result = result;
            }
            else // Check base implementation for Type
                base.SetClassResult(buffer);
        }
    }
}
