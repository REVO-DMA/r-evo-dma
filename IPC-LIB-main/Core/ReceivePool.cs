using IPC_LIB.SharedMemory;
using System.IO.Pipes;
using static IPC_LIB.IPC_Types;

namespace IPC_LIB.Core
{
    internal sealed class ReceivePool
    {
        #region Fields

        private readonly NamedPipeServerStream _ipcServerInStream;
        private readonly NamedPipeClientStream _ipcClientInStream;

        /// <summary>
        /// Whether or not this instance is for the IPC Server.
        /// </summary>
        private readonly bool _isServer;

        #endregion

        #region .ctor()

        internal ReceivePool(NamedPipeServerStream ipcServerInStream)
        {
            _ipcServerInStream = ipcServerInStream;

            _isServer = true;
        }

        internal ReceivePool(NamedPipeClientStream ipcClientInStream)
        {
            _ipcClientInStream = ipcClientInStream;

            _isServer = false;
        }

        #endregion

        #region Public Methods

        public bool TryRead(int length, out SharedRaw<byte> buffer)
        {
            buffer = new(length);

            int bytesRead;
            if (_isServer)
                bytesRead = _ipcServerInStream.Read(buffer.Span);
            else
                bytesRead = _ipcClientInStream.Read(buffer.Span);

            if (bytesRead != length)
            {
                Console.WriteLine("[IPC RECEIVE POOL] -> IpcReadHeader(): bytesRead does not match length!");
                buffer.Dispose();
                return false;
            }

            return true;
        }

        public unsafe bool TryReadHeader(out MessageHeader header)
        {
            using SharedPinned<byte> buffer = new(MessageHeader.Size);

            int bytesRead;
            if (_isServer)
                bytesRead = _ipcServerInStream.Read(buffer.Span);
            else
                bytesRead = _ipcClientInStream.Read(buffer.Span);

            if (bytesRead != MessageHeader.Size)
            {
                Console.WriteLine("[IPC RECEIVE POOL] -> IpcReadHeader(): Unable to read message header!");
                header = default;
                return false;
            }

            header = *(MessageHeader*)buffer.Address;

            return true;
        }

        #endregion
    }
}
