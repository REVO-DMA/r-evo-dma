using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Buffers;
using System.Security.Cryptography;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Aes = System.Security.Cryptography.Aes;

namespace Tarkov_DMA_Backend.Tarkov.GameAPI
{
    public static class GameApiClient
    {
        public static string Post(string backend, string endpoint, Dictionary<string, string> headers, string content)
        {
            try
            {
                HttpClientHandler handler = new()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                };

                using HttpClient httpClient = new(handler);

                httpClient.Timeout = TimeSpan.FromSeconds(5);

                httpClient.DefaultRequestHeaders.Clear();

                httpClient.BaseAddress = new Uri($"https://{backend}");
                httpClient.DefaultRequestHeaders.Host = backend;

                // Accept Encoding
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new("deflate"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new("gzip"));

                // Add headers
                foreach (var header in headers)
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

                byte[] deflated = Deflate(Encoding.UTF8.GetBytes(content));

                HttpRequestMessage request = new(HttpMethod.Post, endpoint)
                {
                    Content = new ByteArrayContent(deflated)
                };
                request.Content.Headers.ContentType = new("application/json");
                request.Content.Headers.ContentLength = deflated.Length;

                HttpResponseMessage response = httpClient.Send(request);

                if (!response.IsSuccessStatusCode)
                {
                    Logger.WriteLine($"[GAME API]: RemotePost() -> Non-successful status code received: {response.StatusCode}");
                    return null;
                }

                using MemoryStream outputStream = new();
                response.Content.ReadAsStream().CopyTo(outputStream);
                byte[] result = outputStream.ToArray();

                var decrypted = Decrypt(result);
                var inflated = Inflate(decrypted);

                return Encoding.UTF8.GetString(inflated);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME API]: RemotePost() -> Error while performing request: {ex}");
            }

            return null;
        }

        private static byte[] Decrypt(byte[] data)
        {
            try
            {
                using Aes aes = Aes.Create();

                aes.IV = data.Take(16).ToArray();
                aes.Key = EFTDMA.AesKey;
                aes.Padding = PaddingMode.Zeros;

                using ICryptoTransform decryptor = aes.CreateDecryptor();
                byte[] cipherBytes = data.Skip(16).ToArray();

                return decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME API]: Decrypt() -> Error during decryption: {ex}");
            }

            return null;
        }

        private static byte[] Inflate(byte[] data)
        {
            try
            {
                using MemoryStream outputStream = new();
                using MemoryStream dataStream = new(data);

                Inflater inflater = new(false);
                using InflaterInputStream inStream = new(dataStream, inflater);

                inStream.CopyTo(outputStream);

                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME API]: Inflate() -> Error during decompression: {ex}");
            }

            return null;
        }

        private static byte[] Deflate(byte[] data)
        {
            try
            {
                using MemoryStream outputStream = new();

                Deflater deflater = new(9, false);
                using DeflaterOutputStream outStream = new(outputStream, deflater);

                outStream.Write(data);
                outStream.Flush();
                outStream.Finish();

                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[GAME API]: Deflate() -> Error during compression: {ex}");
            }

            return null;
        }
    }
}
