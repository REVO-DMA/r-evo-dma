using arena_dma_backend.DMA.Collections;
using arena_dma_backend.Misc;
using Silk.NET.Vulkan;
using System.Buffers.Binary;
using System.Runtime.Intrinsics;

namespace arena_dma_backend.DMA.ScatterAPI
{
    /// <summary>
    /// Single scatter read.
    /// Use ScatterReadRound.AddEntry() to construct this class.
    /// </summary>
    public sealed class ScatterReadEntry<T> : IScatterEntry
    {
        #region Properties

        /// <summary>
        /// Entry Index.
        /// </summary>
        public int Index { get; init; }
        /// <summary>
        /// Entry ID.
        /// </summary>
        public int Id { get; init; }
        /// <summary>
        /// Can be a ulong or another ScatterReadEntry.
        /// </summary>
        public object Addr { get; set; }
        /// <summary>
        /// Offset to the Base Address.
        /// </summary>
        public object Offset { get; set; }
        /// <summary>
        /// Defines the type based on <typeparamref name="T"/>
        /// </summary>
        public Type Type { get; } = typeof(T);
        /// <summary>
        /// Can be an int32 or another ScatterReadEntry.
        /// </summary>
        public object Size { get; set; }
        /// <summary>
        /// True if the Scatter Read has failed.
        /// </summary>
        public bool IsFailed { get; set; }
        /// <summary>
        /// Scatter Read Result.
        /// </summary>
        private T Result { get; set; }
        #endregion

        #region Read Prep
        /// <summary>
        /// Parses the address to read for this Scatter Read.
        /// Sets the Addr property for the object.
        /// </summary>
        /// <returns>Virtual address to read.</returns>
        public ulong ParseAddr()
        {
            ulong addr = 0x0;
            if (this.Addr is ulong p1)
                addr = p1;
            else if (this.Addr is MemPointer p2)
                addr = p2;
            else if (this.Addr is IScatterEntry ptrObj) // Check if the addr references another ScatterRead Result
            {
                if (ptrObj.TryGetResult<MemPointer>(out var p3))
                    addr = p3;
                else
                    ptrObj.TryGetResult(out addr);
            }
            this.Addr = addr;
            return addr;
        }

        /// <summary>
        /// Parses the number of bytes to read for this Scatter Read.
        /// Sets the Size property for the object.
        /// Derived classes should call upon this Base.
        /// </summary>
        /// <returns>Size of read.</returns>
        public int ParseSize()
        {
            int size = 0;
            if (this.Type.IsValueType)
                size = SizeChecker<T>.Size;
            else if (this.Size is int sizeInt)
                size = sizeInt;

            this.Size = size;
            return size;
        }

        /// <summary>
        /// Parses the address's offset to read for this Scatter Read.
        /// Sets the Offset property for the object.
        /// </summary>
        /// <returns>Offset of read.</returns>
        public uint ParseOffset()
        {
            uint offset = 0x0;
            if (this.Offset is uint p1)
                offset = p1;
            else if (this.Offset is IScatterEntry offsetObj) // Check if the offset references another ScatterRead Result
                offsetObj.TryGetResult(out offset);
            this.Offset = offset;
            return offset;
        }
        #endregion

        #region Set Result
        /// <summary>
        /// Sets the Result for this Scatter Read.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        public void SetResult(ReadOnlySpan<byte> buffer)
        {
            try
            {
                if (IsFailed)
                    return;
                if (Type.IsValueType) /// Ref Type
                    SetValueResult(buffer);
                else /// Value Type
                    SetClassResult(buffer);

            }
            catch
            {
                IsFailed = true;
            }
        }

        /// <summary>
        /// Set the Result from a Value Type.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        private void SetValueResult(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length != SizeChecker<T>.Size) // Safety Check
                throw new ArgumentOutOfRangeException(nameof(buffer));

            Result = Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(buffer));

            if (Result is MemPointer memPtrResult)
                memPtrResult.Validate();
        }

        /// <summary>
        /// Set the Result from a Class Type.
        /// Derived classes should call upon this Base.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        private void SetClassResult(ReadOnlySpan<byte> buffer)
        {
            if (Type == typeof(int[])) // indices
            {
                const int chunkSize = 4;

                var value = new int[buffer.Length / chunkSize];
                int outputPos = 0;
                for (var i = 0; i < buffer.Length; i += chunkSize)
                {
                    var span = buffer.Slice(i, chunkSize);
                    value[outputPos++] = BinaryPrimitives.ReadInt32LittleEndian(span);
                }

                if (value is T result)
                    Result = result;
            }
            else if (Type == typeof(SharedContainer<Vector128<float>>)) // vertices
            {
                int size = SizeChecker<Vector128<float>>.Size;
                ArgumentOutOfRangeException.ThrowIfNotEqual(buffer.Length % size, 0, nameof(buffer));
                int count = buffer.Length / size;

                SharedContainer<Vector128<float>> value = new(count);
                try
                {
                    var casted = MemoryMarshal.Cast<byte, Vector128<float>>(buffer);
                    casted.CopyTo(value.Span);
                    
                    if (value is T result)
                        Result = result;
                }
                catch
                {
                    value.Dispose();
                    throw;
                }
            }
            else if (Type == typeof(string))
            {
                var nullIndex = buffer.IndexOf((byte)0);
                var value = nullIndex >= 0 ?
                    Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
                if (value is T result) // We already know the Types match, this is to satisfy the compiler
                    Result = result;
            }
            else
                throw new NotImplementedException(Type.ToString());
        }
        #endregion

        #region Get Result
        /// <summary>
        /// Tries to return the Scatter Read Result.
        /// </summary>
        /// <typeparam name="TOut">Type to return.</typeparam>
        /// <param name="result">Result to populate.</param>
        /// <returns>True if successful, otherwise False.</returns>
        public bool TryGetResult<TOut>(out TOut result)
        {
            try
            {
                if (!IsFailed && Result is TOut tResult)
                {
                    result = tResult;
                    return true;
                }
                result = default;
                return false;
            }
            catch
            {
                result = default;
                return false;
            }
        }
        #endregion
    }
}