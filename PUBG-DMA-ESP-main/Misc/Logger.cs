namespace pubg_dma_esp
{
    public interface ILogger
    {
        void WriteLine(params object[] data);
    }

    public class DebugLogger : ILogger
    {
        public void WriteLine(params object[] data)
        {
            Debug.WriteLine(string.Join(" ", data));
        }
    }

    public class ReleaseLogger : ILogger
    {
        public void WriteLine(params object[] data)
        {
            Console.WriteLine(string.Join(" ", data));
        }
    }

    public class CommercialLogger : ILogger
    {
        public void WriteLine(params object[] data)
        {
            // Release mode implementation (no logging)
        }
    }

    public static class Logger
    {
#if DEBUG
        private static readonly ILogger _logger = new DebugLogger();
#endif
#if RELEASE
        private static readonly ILogger _logger = new ReleaseLogger();
#endif
#if COMMERCIAL
        private static readonly ILogger _logger = new CommercialLogger();
#endif

        public static void WriteLine(params object[] data)
        {
            _logger.WriteLine(data);
        }
    }
}
