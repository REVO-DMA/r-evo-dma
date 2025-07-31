using IPC_LIB.SharedMemory;
using static IPC_LIB.IPC_Types;

namespace IPC_LIB.Core
{
    internal readonly struct Transaction : IDisposable
    {
        #region Fields

        public readonly SharedPinned<byte> Payload;

        #endregion

        #region .ctor()

        internal Transaction(ulong transactionID, int messageType, bool encrypted, ReadOnlySpan<byte> message)
        {
            int totalLength = MessageHeader.Size + message.Length;

            Payload = new(totalLength);

            CreatePayload(transactionID, messageType, encrypted, message);
        }

        #endregion

        #region Public Methods

        public readonly void Dispose()
        {
            Payload.Dispose();
        }

        #endregion

        #region Private Methods

        private unsafe void CreatePayload(ulong transactionID, int messageType, bool encrypted, ReadOnlySpan<byte> message)
        {
            MessageHeader header = new(message.Length, transactionID, messageType, encrypted);

            *(MessageHeader*)Payload.Address = header;

            fixed (byte* pMessage = message)
            {
                Buffer.MemoryCopy(pMessage, Payload.Address + MessageHeader.Size, message.Length, message.Length);
            }
        }

        #endregion
    }
}
