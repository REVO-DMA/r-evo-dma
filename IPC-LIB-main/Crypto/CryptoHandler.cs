namespace IPC_LIB.Crypto
{
    internal sealed class CryptoHandler : IDisposable
    {
        private readonly AES_Encryptor _aesEncryptor;
        private readonly AES_Decryptor _aesDecryptor;

        internal CryptoHandler(RSA_Helper rsaHelper)
        {
            _aesEncryptor = new(rsaHelper);
            _aesDecryptor = new(rsaHelper);
        }

        public byte[] Encrypt(ReadOnlySpan<byte> data) => _aesEncryptor.Encrypt(data);

        public byte[] Decrypt(ReadOnlySpan<byte> data) => _aesDecryptor.Decrypt(data);

        public void SetAesKey(ReadOnlySpan<byte> data)
        {
            byte[] key = data.ToArray();

            _aesEncryptor.SetKey(key);
            _aesDecryptor.SetKey(key);
        }

        public void Dispose()
        {
            _aesEncryptor.Dispose();
            _aesDecryptor.Dispose();
        }
    }
}
