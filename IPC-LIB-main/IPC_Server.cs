using IPC_LIB.Core;
using IPC_LIB.Crypto;
using System.IO.Pipes;

namespace IPC_LIB
{
    public sealed class IPC_Server : IDisposable
    {
        #region Fields

        private readonly string _name;
        private readonly bool _encrypted;

        private readonly NamedPipeServerStream _inServer;
        private readonly NamedPipeServerStream _outServer;

        private readonly Sender _sender;
        private readonly Receiver _receiver;

        private readonly RSA_Helper _rsaHelper;
        private readonly CryptoHandler _cryptoHandler;

        #endregion

        public IPC_Server(string name, bool encrypted, IPC_Types.OnMessage onMessage)
        {
            _name = name;
            _encrypted = encrypted;

            _inServer = new($"{name}_in", PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.WriteThrough);
            _outServer = new($"{name}_out", PipeDirection.Out, 1, PipeTransmissionMode.Message, PipeOptions.WriteThrough);

            Console.WriteLine("[IPC SERVER] .ctor(): InServer ~ Waiting for connection...");
            _inServer.WaitForConnection();

            Console.WriteLine("[IPC SERVER] .ctor(): OutServer ~ Waiting for connection...");
            _outServer.WaitForConnection();

            if (_encrypted)
            {
                _rsaHelper = new();
                _cryptoHandler = new(_rsaHelper);
            }

            _receiver = new(_inServer, onMessage, _cryptoHandler);
            _sender = new(_outServer, _receiver, _encrypted, _cryptoHandler);

            if (_encrypted)
            {
                Negotiator negotiator = new(_rsaHelper, _cryptoHandler, _receiver, _sender, true);
                negotiator.Negotiate();
            }

            Console.WriteLine("[IPC SERVER] .ctor(): Ready!");
        }

        #region Public Methods

        public void SendMessage(int messageType, ReadOnlySpan<byte> message, bool forcePlaintext = false) => _sender.SendMessage(messageType, message, forcePlaintext);
        public void SendMessage(int messageType, byte[] message, bool forcePlaintext = false) => _sender.SendMessage(messageType, message, forcePlaintext);
        public void SendResponse(int messageType, byte[] message, ulong transactionID, bool forcePlaintext = false) => _sender.SendResponse(messageType, message, transactionID, forcePlaintext);
        public void GetResponse(int messageType, byte[] message, Action<byte[]> onResponse, bool forcePlaintext = false) => _sender.GetResponse(messageType, message, onResponse, forcePlaintext);

        public void Dispose()
        {
            _inServer.Dispose();
            _outServer.Dispose();

            // TODO: Implement IDisposable in _receiver & _sender

            _cryptoHandler.Dispose();
            _rsaHelper.Dispose();
        }

        #endregion
    }
}
