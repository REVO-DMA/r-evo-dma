using System.Security.Cryptography;

namespace Tarkov_DMA_Backend.Guardian
{
    public class ObfuscatedBool
    {
        private const int _minSize = 32;
        private const int _maxSize = 128;

        private readonly object _lock = new();
        private readonly Random _random;

        private readonly int _size;
        private readonly byte[] _data;

        private string _hash = string.Empty;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ObfuscatedBool(bool initialValue)
        {
            _random = new();

            _size = _random.Next(_minSize, _maxSize);
            _data = new byte[_size];

            Set(initialValue, false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(bool value, bool validate = true)
        {
            VirtualizerSDK.VIRTUALIZER_LION_RED_START();

            lock (_lock)
            {
                if (validate && !_hash.Equals(ComputeHash(_data)))
                {
                    Logger.WriteLine("[ObfuscatedBool] Tampering detected, log and ban...");
                }

                _data[0] = (byte)(value ? 1 : 0);

                for (int i = 1; i < _size; i++)
                    _data[i] = (byte)_random.Next(256);

                for (int i = 1; i < _size; i++)
                    _data[0] ^= _data[i];

                _hash = ComputeHash(_data);
            }

            VirtualizerSDK.VIRTUALIZER_LION_RED_END();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Get()
        {
            VirtualizerSDK.VIRTUALIZER_LION_RED_START();

            bool value;

            lock (_lock)
            {
                if (!_hash.Equals(ComputeHash(_data)))
                {
                    Logger.WriteLine("[ObfuscatedBool] Tampering detected, log and ban...");
                }

                byte result = _data[0];

                for (int i = 1; i < _size; i++)
                    result ^= _data[i];

                value = result != 0;
            }

            VirtualizerSDK.VIRTUALIZER_LION_RED_END();

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string ComputeHash(byte[] data)
        {
            VirtualizerSDK.VIRTUALIZER_LION_RED_START();

            byte[] hashBytes = SHA256.HashData(data);

            string hash = Convert.ToBase64String(hashBytes);

            VirtualizerSDK.VIRTUALIZER_LION_RED_END();

            return hash;
        }
    }
}
