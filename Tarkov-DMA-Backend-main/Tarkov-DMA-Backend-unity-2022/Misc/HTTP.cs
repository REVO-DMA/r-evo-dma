namespace Tarkov_DMA_Backend.Misc
{
    public static class HTTP
    {
        public static bool TryGetRemoteBytes(string url, out byte[] response)
        {
            HttpClient httpClient = new();

            try
            {

                response = httpClient.GetByteArrayAsync(url).GetAwaiter().GetResult();
                return true;
            }
            catch
            {
                response = null;
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
