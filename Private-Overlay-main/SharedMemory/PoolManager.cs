using cs2_dma_esp;
using MemoryPack;
using MemoryPack.Compression;
using Private_Overlay.Rendering;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Private_Overlay.SharedMemory
{
    public static unsafe partial class PoolManager
    {
        private const string DLLName = "SLOBS Pool Helper.dll";

        [LibraryImport(DLLName)]
        private static partial nint GetReadPool();

        [LibraryImport(DLLName)]
        private static partial nint GetWritePool();

        [LibraryImport(DLLName)]
        private static partial nint GetIsAlive();

        private static readonly ulong* IsAlive; // DLLName+0x3670
        private static readonly byte* ReadPool; // DLLName+0x3680
        private static readonly byte* WritePool; // DLLName+0x120E0

        private const uint PoolSize = 60000;

        /// <summary>
        /// The last message index that was processed.
        /// </summary>
        private static ulong _lastReadMessageIndex;

        private static readonly Thread _socketThread;
        private static readonly Thread _isAliveThread;

        public enum PoolType
        {
            Read,
            Write
        }

        public enum MessageType : byte
        {
            ESP_RenderPacket,
        }

        [StructLayout(LayoutKind.Explicit, Size = 13)]
        private struct ReadPoolHeader
        {
            /// <summary>
            /// The current message number (used to detect new messages).
            /// </summary>
            [FieldOffset(0)]
            public ulong MessageIndex;
            /// <summary>
            /// The length of the serialized message's bytes.
            /// </summary>
            [FieldOffset(8)]
            public uint DataLength;
            /// <summary>
            /// The type of the serialized message.
            /// </summary>
            [FieldOffset(12)]
            public byte MessageType;
        }
        /// <summary>
        /// The byte size of the ReadPoolHeader type.
        /// </summary>
        private const uint ReadPoolHeaderSize = 13;

        private struct PoolHeader
        {
            /// <summary>
            /// The current message number (used to detect new messages).
            /// </summary>
            public ulong MessageIndex;
            /// <summary>
            /// The length of the serialized message's bytes.
            /// </summary>
            public uint DataLength;
            /// <summary>
            /// Whether or not this message has been read (only checked on the write pool).
            /// </summary>
            public bool Consumed;
        }

        /// <summary>
        /// The byte size of the pool header.
        /// </summary>
        private const uint PoolHeaderSize = 13;

        private readonly struct PoolMessage
        {
            /// <summary>
            /// The message header.
            /// </summary>
            public readonly ReadPoolHeader Header;
            /// <summary>
            /// The serialized message data.
            /// </summary>
            public readonly byte[] Data;

            public PoolMessage(ReadPoolHeader header, byte[] data)
            {
                Header = header;
                Data = data;
            }
        }

        static PoolManager()
        {
            IsAlive = (ulong*)GetIsAlive().ToPointer();

            // Get the read & write pools
            ReadPool = (byte*)GetReadPool().ToPointer();
            WritePool = (byte*)GetWritePool().ToPointer();

            // Init pool data w/ the default header
            ReadPoolHeader defaultHeader = new()
            {
                MessageIndex = 0,
                DataLength = 0
            };

            Write(PoolType.Read, defaultHeader);
            Write(PoolType.Write, defaultHeader);

            _lastReadMessageIndex = 0;

            _socketThread = new(PoolWorker)
            {
                IsBackground = true,
            };

            _isAliveThread = new(IsAliveWorker)
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest,
            };
        }

        public static void Run()
        {
            _socketThread.Start();
            _isAliveThread.Start();
        }

        private static void PoolWorker()
        {
            try
            {
                Logger.WriteLine($"[POOL MANAGER] -> PoolWorker(): Worker thread for pool is starting...");

                while (true)
                {
                    try
                    {
                        var message = GetMessage(PoolType.Read);

                        if (message.Data.Length == 0)
                            continue;

                        using BrotliDecompressor decompressor = new();
                        var decompressedBuffer = decompressor.Decompress(message.Data);

                        if (message.Header.MessageType == (byte)MessageType.ESP_RenderPacket)
                        {
                            ESP_RenderPacket packet = MemoryPackSerializer.Deserialize<ESP_RenderPacket>(decompressedBuffer);

                            SLOBS.RenderData = packet.RenderData;
                            SLOBS.RenderDataIndex++;
                            SLOBS.LastRenderTime = DateTime.UtcNow;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[POOL MANAGER] -> PoolWorker(): Error reading message {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[POOL MANAGER] -> PoolWorker(): Critical error on pool worker {ex}");
            }
            finally
            {
                Logger.WriteLine($"[POOL MANAGER] -> PoolWorker(): Worker thread for pool is stopping...");
            }
        }

        private static void IsAliveWorker()
        {
            try
            {
                Logger.WriteLine($"[POOL MANAGER] -> IsAliveWorker(): Worker thread for IsAlive is starting...");

                while (true)
                {
                    *IsAlive += 1;

                    if (SLOBS.LastRenderTime.AddSeconds(1).CompareTo(DateTime.UtcNow) < 0)
                        SLOBS.ShouldClearScreen = true;
                    else
                        SLOBS.ShouldClearScreen = false;

                    Thread.Sleep(350);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[POOL MANAGER] -> IsAliveWorker(): Critical error on IsAlive worker {ex}");
            }
            finally
            {
                Logger.WriteLine($"[POOL MANAGER] -> IsAliveWorker(): Worker thread for IsAlive is stopping...");
            }
        }

        private static PoolMessage GetMessage(PoolType poolType)
        {
            ReadPoolHeader header = GetMessageHeader(poolType);

            while (_lastReadMessageIndex == header.MessageIndex)
            {
                Thread.Sleep(1);
                header = GetMessageHeader(poolType);
            }

            byte[] buffer = Read(poolType, header.DataLength, ReadPoolHeaderSize);

            // "Acknowledge" this message
            _lastReadMessageIndex = header.MessageIndex;

            return new(header, buffer);
        }

        private static ReadPoolHeader GetMessageHeader(PoolType poolType)
        {
            return Read<ReadPoolHeader>(poolType);
        }

        private static void Write(PoolType poolType, byte[] data, uint offset = 0x0)
        {
            if (data.Length > PoolSize)
                throw new Exception("[POOL MANAGER] byte size is larger than pool!");

            fixed (byte* pData = data)
            {
                byte* pool;

                if (poolType == PoolType.Read)
                    pool = ReadPool;
                else if (poolType == PoolType.Write)
                    pool = WritePool;
                else
                    throw new Exception("[POOL MANAGER] Invalid pool type specified!");

                Buffer.MemoryCopy(pData, pool + offset, data.Length, data.Length);
            }
        }

        private static void Write<T>(PoolType poolType, T value, uint offset = 0x0) where T : unmanaged
        {
            uint sizeT = (uint)Unsafe.SizeOf<T>();

            if (sizeT > PoolSize)
                throw new Exception("[POOL MANAGER] byte size is larger than pool!");

            var data = new byte[sizeT];
            MemoryMarshal.Write(data, ref value);

            Write(poolType, data, offset);
        }

        private static byte[] Read(PoolType poolType, uint size, uint offset = 0x0)
        {
            if (size > PoolSize)
                throw new Exception("[POOL MANAGER] byte size is larger than pool!");

            byte[] buffer = new byte[size];

            fixed (byte* pBuffer = buffer)
            {
                byte* pool;

                if (poolType == PoolType.Read)
                    pool = ReadPool;
                else if (poolType == PoolType.Write)
                    pool = WritePool;
                else
                    throw new Exception("[POOL MANAGER] Invalid pool type specified!");

                Buffer.MemoryCopy(pool + offset, pBuffer, size, size);
            }

            return buffer;
        }

        private static T Read<T>(PoolType poolType, uint offset = 0x0) where T : unmanaged
        {
            uint sizeT = (uint)Unsafe.SizeOf<T>();

            if (sizeT > PoolSize)
                throw new Exception("[POOL MANAGER] byte size is larger than pool!");

            byte[] buffer = Read(poolType, sizeT, offset);

            return MemoryMarshal.Read<T>(buffer);
        }
    }
}
