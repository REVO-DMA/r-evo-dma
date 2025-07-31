namespace Tarkov_DMA_Backend.Unity.LowLevel
{
    public readonly unsafe struct RemoteCharPointer : IDisposable
    {
        public static implicit operator ulong(RemoteCharPointer x) => x._chars;

        private readonly bool _shouldDispose;
        private readonly RemoteBytes _chars;

        public RemoteCharPointer(string data, RemoteBytes chars = null)
        {
            int byteCount = Encoding.UTF8.GetByteCount(data) + 1; // +1 for null terminator
            byte[] bytes = new byte[byteCount];

            Encoding.UTF8.GetBytes(data, 0, data.Length, bytes, 0);
            bytes[byteCount - 1] = 0;

            if (chars == null)
            {
                _shouldDispose = true;
                _chars = new((uint)byteCount);
            }
            else
            {
                _shouldDispose = false;
                _chars = chars;
                _chars.WriteBuffer(bytes);
            }
        }

        public void Dispose()
        {
            if (_shouldDispose)
                _chars.Dispose();
        }
    }
}
