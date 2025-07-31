using System.Reflection;
using System.Text;

namespace UI_V2.ResourceHelper
{
    public static class ResourceLoaderUtils
    {
        /// <summary>
        /// Creates a stream to an embedded resource.
        /// </summary>
        public static Stream GetResourceStream(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();

            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
                throw new Exception($"Unable to find embedded resource \"{resourcePath}\"! Make sure the file exists and has been set as an embedded resource in the build action.");

            return stream;
        }

        /// <summary>
        /// Reads a stream as text.
        /// </summary>
        public static string ReadTextStream(Stream stream)
        {
            using StreamReader reader = new(stream, Encoding.UTF8);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads a stream as binary data.
        /// </summary>
        public static byte[] ReadBinaryStream(Stream stream)
        {
            using MemoryStream ms = new();

            stream.CopyTo(ms);

            return ms.ToArray();
        }

        /// <summary>
        /// Converts text data into a base64 string.
        /// </summary>
        public static string ToBase64(string data)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
        }

        /// <summary>
        /// Converts binary data into a base64 string.
        /// </summary>
        public static string ToBase64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }
    }
}
