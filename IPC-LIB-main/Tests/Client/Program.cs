using IPC_LIB;
using System.Buffers;
using System.Security.Cryptography;

namespace Client
{
    internal class Program
    {
        private static readonly IPC_Client _ipc;

        static Program()
        {
            _ipc = new("test_ipc_lib", true, HandleData);
        }

        static void Main(string[] args)
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                while (true)
                {
                    int size = Random.Shared.Next(1600);
                    byte[] buffer = ArrayPool<byte>.Shared.Rent(size);
                    Span<byte> span = buffer.AsSpan();

                    rng.GetBytes(span);

                    _ipc.SendMessage(69420, span);
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
        }

        private static ulong totalLength = 0;
        private static ulong totalMessages = 0;
        private static readonly Timer timer = new(Tracker, null, 0, 1000);

        private static void Tracker(object state)
        {
            if (totalMessages == 0)
                return;

            Console.WriteLine($"Avg size: {totalLength / totalMessages} | Msg/s {totalMessages}");
            totalLength = 0;
            totalMessages = 0;
        }

        private static void HandleData(IPC_Types.MessageHeader header, ReadOnlySpan<byte> data)
        {
            totalLength += (ulong)data.Length;
            totalMessages++;
        }
    }
}
