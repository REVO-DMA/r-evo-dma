using IPC_LIB.Core;
using IPC_LIB.Crypto;
using System.IO.Pipes;

namespace IPC_LIB
{
    public sealed class IPC_Client : IDisposable
    {
        #region Fields

        private readonly string _name;
        private readonly bool _encrypted;

        private readonly NamedPipeClientStream _inClient;
        private readonly NamedPipeClientStream _outClient;

        private readonly Sender _sender;
        private readonly Receiver _receiver;

        private readonly RSA_Helper _rsaHelper;
        private readonly CryptoHandler _cryptoHandler;

        #endregion

        public IPC_Client(string name, bool encrypted, IPC_Types.OnMessage onMessage)
        {
            _name = name;
            _encrypted = encrypted;

            _inClient = new(".", $"{name}_out", PipeDirection.In, PipeOptions.WriteThrough);
            _outClient = new(".", $"{name}_in", PipeDirection.Out, PipeOptions.WriteThrough);

            Console.WriteLine("[IPC CLIENT] .ctor(): InClient ~ Connecting...");
            _inClient.Connect();

            Console.WriteLine("[IPC CLIENT] .ctor(): OutClient ~ Connecting...");
            _outClient.Connect();

            if (_encrypted)
            {
                _rsaHelper = new();
                _cryptoHandler = new(_rsaHelper);
            }

            _receiver = new(_inClient, onMessage, _cryptoHandler);
            _sender = new(_outClient, _receiver, _encrypted, _cryptoHandler);

            if (_encrypted)
            {
                Negotiator negotiator = new(_rsaHelper, _cryptoHandler, _receiver, _sender, false);
                negotiator.Negotiate();
            }

            Console.WriteLine("[IPC CLIENT] .ctor(): Ready!");
        }

        #region Public Methods

        public void SendMessage(int messageType, ReadOnlySpan<byte> message, bool forcePlaintext = false) => _sender.SendMessage(messageType, message, forcePlaintext);
        public void SendMessage(int messageType, byte[] message, bool forcePlaintext = false) => _sender.SendMessage(messageType, message, forcePlaintext);
        public void SendResponse(int messageType, byte[] message, ulong transactionID, bool forcePlaintext = false) => _sender.SendResponse(messageType, message, transactionID, forcePlaintext);
        public void GetResponse(int messageType, byte[] message, Action<byte[]> onResponse, bool forcePlaintext = false) => _sender.GetResponse(messageType, message, onResponse, forcePlaintext);

        public void Dispose()
        {
            _inClient.Dispose();
            _outClient.Dispose();

            // TODO: Implement IDisposable in _receiver & _sender

            _cryptoHandler.Dispose();
            _rsaHelper.Dispose();
        }

        #endregion
    }
}
