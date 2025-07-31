using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;

namespace Tarkov_DMA_Backend
{
    public interface ILogger
    {
        void WriteLine(params object[] data);
        void Write(params object[] data);
    }

    public class DebugLogger : ILogger
    {
        public void WriteLine(params object[] data)
        {
            Debug.WriteLine(string.Join(" ", data));
        }

        public void Write(params object[] data)
        {
            Debug.Write(string.Join(" ", data));
        }
    }

    public class ReleaseLogger : ILogger
    {
        public void WriteLine(params object[] data)
        {
            Console.WriteLine(string.Join(" ", data));
        }

        public void Write(params object[] data)
        {
            Console.Write(string.Join(" ", data));
        }
    }

    public class CommercialLogger : ILogger
    {
        public void WriteLine(params object[] data)
        {
            // Release mode implementation (no logging)
        }

        public void Write(params object[] data)
        {
            // Release mode implementation (no logging)
        }
    }

    public static class Logger
    {
#if DEBUG
        private static readonly ILogger _logger = new DebugLogger();
#elif RELEASE_PUBLIC || RELEASE
        private static readonly ILogger _logger = new ReleaseLogger();
#elif COMMERCIAL_PUBLIC || COMMERCIAL
        private static readonly ILogger _logger = new CommercialLogger();
#endif

        public static void WriteLine(params object[] data)
        {
            _logger.WriteLine(data);
        }

        public static void Write(params object[] data)
        {
            _logger.Write(data);
        }

        public static void WriteBytes(ReadOnlySpan<byte> data, bool showLength = true, bool hexPrefix = false, bool commaSeperated = false)
        {
            const string hexPrefixStr = "0x";

            StringBuilder sb = new();

            if (showLength)
                sb.AppendLine($"====== Length: {data.Length} (0x{data.Length:X}) ======");

            foreach (var b in data)
            {
                if (!hexPrefix && !commaSeperated)
                    sb.Append($"{b:X2} ");
                else if (hexPrefix && commaSeperated)
                    sb.Append($"{hexPrefixStr}{b:X2}, ");
                else if (hexPrefix)
                    sb.Append($"{hexPrefixStr}{b:X2} ");
                else if (commaSeperated)
                    sb.Append($"{b:X2}, ");
            }

            string output;
            if (!commaSeperated)
                output = sb.ToString().TrimEnd();
            else
                output = sb.ToString().TrimEnd().TrimEnd(',');

            WriteLine(output);
        }

        public static void PrintLastError()
        {
            PrintError(GetLastError());
        }

        public static void PrintError(HRESULT hr)
        {
            PrintError((uint)hr);
        }

        public static unsafe void PrintError(uint hr)
        {
            const int size = 512;

            byte[] msgBuffA = new byte[size];
            fixed (byte* msgBuff = msgBuffA)
            {
                FormatMessageW(FORMAT.FORMAT_MESSAGE_FROM_SYSTEM | FORMAT.FORMAT_MESSAGE_IGNORE_INSERTS, null, hr, 0, (char*)msgBuff, size, null);
                string message = Encoding.UTF8.GetString(msgBuffA).Trim();
                if (message.Length > 0)
                    WriteLine($"{message} - {hr} (0x{hr:X})");
                else
                    WriteLine($"{hr} (0x{hr:X})");
            }
        }
    }
}
