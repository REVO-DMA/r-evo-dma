using pubg_dma_esp.MemDMA.Collections;
using pubg_dma_esp.Misc;
using Silk.NET.Vulkan;
using System.Runtime.Intrinsics;

namespace pubg_dma_esp.MemDMA.ScatterAPI
{
    public sealed class ScatterReadEntry<T> : IScatterEntry
    {
        private T _result;
        /// <summary>
        /// Virtual Address to read from.
        /// </summary>
        public ulong Address { get; }
        /// <summary>
        /// Count of bytes to read.
        /// </summary>
        public int CB { get; }
        /// <summary>
        /// True if this read has failed, otherwise False.
        /// </summary>
        public bool IsFailed { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="address">Virtual Address to read from.</param>
        /// <param name="cb">Count of bytes to read.</param>
        internal ScatterReadEntry(ulong address, int cb)
        {
            Address = address;
            if (cb == 0 && typeof(T).IsValueType)
                cb = SizeChecker<T>.Size;
            CB = cb;
        }

        /// <summary>
        /// Parse the memory buffer and set the result value.
        /// Only called internally via API.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        public void SetResult(ReadOnlySpan<byte> buffer)
        {
            try
            {
                if (typeof(T).IsValueType)
                    _result = SetValueResult(buffer);
                else
                    _result = SetClassResult(buffer);
            }
            catch
            {
                IsFailed = true;
            }
        }

        /// <summary>
        /// Try to obtain the scatter read result.
        /// </summary>
        /// <typeparam name="TOut">Result Type <typeparamref name="TOut"/></typeparam>
        /// <param name="resultOut">Result field to populate.</param>
        /// <returns>True if successful, otherwise False.</returns>
        public bool TryGetResult<TOut>(out TOut resultOut)
        {
            if (IsFailed)
            {
                resultOut = default;
                return false;
            }
            else if (_result is TOut result)
            {
                resultOut = result;
                return true;
            }
            resultOut = default;
            return false;
        }

        /// <summary>
        /// Set the Result from a Value Type.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        private static T SetValueResult(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length != SizeChecker<T>.Size)
                throw new ArgumentOutOfRangeException(nameof(buffer));
            var result = Unsafe.As<byte, T>(ref MemoryMarshal.GetReference(buffer));
            if (result is MemPointer memPtrResult)
                memPtrResult.Validate();
            return result;
        }

        /// <summary>
        /// Set the Result from a Class Type.
        /// </summary>
        /// <param name="buffer">Raw memory buffer for this read.</param>
        private static T SetClassResult(ReadOnlySpan<byte> buffer)
        {
            var type = typeof(T);
            if (type == typeof(string))
            {
                var nullIndex = buffer.IndexOf((byte)0);
                var value = nullIndex >= 0 ?
                    Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
                if (value is T result) // We already know the Types match, this is to satisfy the compiler
                    return result;
            }
            else if (type == typeof(Types.AsciiString))
            {
                var nullIndex = buffer.IndexOf((byte)0);
                Types.AsciiString value = new(nullIndex >= 0 ?
                    Encoding.ASCII.GetString(buffer.Slice(0, nullIndex)) : Encoding.ASCII.GetString(buffer));
                if (value is T result) // We already know the Types match, this is to satisfy the compiler
                    return result;
            }
            else if (type == typeof(Types.UnicodeString))
            {
                var mem = Marshal.StringToCoTaskMemUni(Encoding.Unicode.GetString(buffer));
                try
                {
                    Types.UnicodeString value = new(Marshal.PtrToStringUni(mem));

                    if (value is T result) // We already know the Types match, this is to satisfy the compiler
                        return result;
                }
                finally
                {
                    Marshal.ZeroFreeCoTaskMemUnicode(mem);
                }
            }
            else
                throw new NotImplementedException(type.ToString());
            throw new Exception("SetClassResult FAIL");
        }
    }
}
