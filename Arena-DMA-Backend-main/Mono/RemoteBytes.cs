using arena_dma_backend.DMA.Collections;

namespace arena_dma_backend.Mono
{
    public class RemoteBytes : IDisposable
    {
        public static implicit operator ulong(RemoteBytes x) => x.AllocBase;

        private readonly ulong AllocBase;
        private readonly uint AllocSize;

        public RemoteBytes(uint size)
        {
            AllocSize = GetSize(size);
            AllocBase = NativeHelper.AllocBytes(AllocSize);
        }

        private const int MonoString_p1 = 0x10;
        public readonly struct MonoString
        {
            private readonly byte[] _p1;
            private readonly byte[] Length;
            private readonly byte[] Data;

            public MonoString(string data)
            {
                _p1 = new byte[MonoString_p1];

                Length = BitConverter.GetBytes(data.Length);
                Data = Encoding.Unicode.GetBytes(data);
            }

            public readonly int GetSize() => MonoString_p1 + Length.Length + Data.Length;
            public readonly uint GetSizeU() => (uint)GetSize();

            public readonly byte[] GetBytes()
            {
                byte[] bytes = new byte[GetSize()];

                MemoryStream memoryStream = new(bytes);
                BinaryWriter writer = new(memoryStream);

                writer.Write(_p1);
                writer.Write(Length);
                writer.Write(Data);

                return bytes;
            }

            public static MonoString Get(string str)
            {
                return new(str);
            }
        }

        public readonly struct RemoteMonoString(int length, byte[] data)
        {
            public readonly int Length = length;
            public readonly byte[] Data = data;

            public static RemoteMonoString Get(ulong addr)
            {
                int length = Memory.ReadValue<int>(addr + 0x18);

                using var hBuf = new SharedMemory<byte>(length);
                var buffer = hBuf.Span;
                Memory.ReadBuffer(addr + 0x20, buffer);

                return new(length, buffer.ToArray());
            }
        }

        public void WriteString(MonoString monoString)
        {
            int byteSize = monoString.GetSize();

            ThrowIfTooBig(byteSize, AllocSize);

            if (!Memory.WriteBufferEnsure(AllocBase, monoString.GetBytes()))
                throw new Exception($"[REMOTE BYTES]: WriteString -> Unable to write string!");
        }

        private const int MonoByteArray_p1 = 0x18;
        private const int MonoByteArray_p2 = 0x4;
        public readonly struct MonoByteArray
        {
            private readonly byte[] _p1;
            private readonly byte[] Length;
            private readonly byte[] _p2;
            private readonly byte[] Data;

            public MonoByteArray(byte[] data)
            {
                _p1 = new byte[MonoByteArray_p1];
                _p2 = new byte[MonoByteArray_p2];

                Length = BitConverter.GetBytes(data.Length);
                Data = data;
            }

            public readonly int GetSize() => MonoByteArray_p1 + MonoByteArray_p2 + Length.Length + Data.Length;
            public readonly uint GetSizeU() => (uint)GetSize();

            public readonly byte[] GetBytes()
            {
                byte[] bytes = new byte[GetSize()];

                MemoryStream memoryStream = new(bytes);
                BinaryWriter writer = new(memoryStream);

                writer.Write(_p1);
                writer.Write(Length);
                writer.Write(_p2);
                writer.Write(Data);

                return bytes;
            }

            public static MonoByteArray Get(byte[] arr)
            {
                return new(arr);
            }
        }

        public void WriteBytes(MonoByteArray monoByteArray)
        {
            int byteSize = monoByteArray.GetSize();

            ThrowIfTooBig(byteSize, AllocSize);

            var bytes = monoByteArray.GetBytes();
            if (!Memory.WriteBufferEnsure(AllocBase, bytes))
                throw new Exception($"[REMOTE BYTES]: WriteBytes -> Unable to write bytes!");
        }

        public void WriteValue<T>(T value) where T : struct
        {
            int byteSize = Unsafe.SizeOf<T>();

            ThrowIfTooBig(byteSize, AllocSize);

            var data = new byte[byteSize];
            MemoryMarshal.Write(data, in value);

            if (!Memory.WriteBufferEnsure(AllocBase, data))
                throw new Exception($"[REMOTE BYTES]: WriteValue<T> -> Unable to write bytes!");
        }

        public void Dispose()
        {
            NativeHelper.FreeBytes(AllocBase);
        }

        private static void ThrowIfTooBig(int dataSize, uint maxSize)
        {
            if (dataSize > maxSize)
                throw new Exception($"The data byte size {dataSize} is larger than the allocated memory of size {maxSize} bytes!");
        }

        /// <summary>
        /// Returns the closest larger multiple of eight.
        /// </summary>
        private static uint GetSize(uint size)
        {
            uint remainder = size % 8;

            if (remainder == 0)
                return size;

            return size + (8 - remainder);
        }
    }
}
