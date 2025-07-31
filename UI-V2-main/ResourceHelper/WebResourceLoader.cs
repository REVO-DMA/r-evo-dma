using System.Text;

namespace UI_V2.ResourceHelper
{
    public sealed class WebResourceLoader
    {
        private readonly string _resourceName;

        public WebResourceLoader(string resourceName)
        {
            _resourceName = resourceName;
        }

        #region Public Methods

        /// <summary>
        /// Gets the processed version of the embedded resource.
        /// </summary>
        public string GetText()
        {
            return Load(_resourceName).tData;
        }

        /// <summary>
        /// Gets the raw binary embedded resource.
        /// </summary>
        public byte[] GetBinary()
        {
            return Load(_resourceName).bData;
        }

        #endregion

        #region Class Types

        private enum ResourceType
        {
            text,
            binary
        }

        private readonly struct LoadedResource(ResourceType type, string tData = null, byte[] bData = null)
        {
            public readonly ResourceType Type = type;
            public readonly string tData = tData;
            public readonly byte[] bData = bData;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Recursively loads an embedded web resource.
        /// </summary>
        private static LoadedResource Load(string resourceName)
        {
            using Stream stream = ResourceLoaderUtils.GetResourceStream(PathUtils.GetResourcePath(resourceName));

            var resourceType = GetType(resourceName);
            if (resourceType == ResourceType.text)
            {
                string resourceData = ResourceLoaderUtils.ReadTextStream(stream);
                string parsedResource = Parse(resourceData);

                return new(resourceType, tData: parsedResource);
            }
            else if (resourceType == ResourceType.binary)
            {
                byte[] resourceData = ResourceLoaderUtils.ReadBinaryStream(stream);

                return new(resourceType, bData: resourceData);
            }
            else
                throw new Exception($"Unhandled resource type \"{resourceType}\"!");
        }

        /// <summary>
        /// Returns the type of a given resource based on it's file extension.
        /// </summary>
        private static ResourceType GetType(string resourceName)
        {
            string ext = Path.GetExtension(resourceName);

            if (ext == ".html")
                return ResourceType.text;
            else if (ext == ".css")
                return ResourceType.text;
            else if (ext == ".js")
                return ResourceType.text;
            else if (ext == ".svg")
                return ResourceType.text;
            else if (ext == ".woff2")
                return ResourceType.binary;
            else
                throw new Exception($"Unhandled file extension \"{resourceName}\"!");
        }

        /// <summary>
        /// Inlines referenced resource data into the parent resource.
        /// </summary>
        private static string Parse(string data)
        {
            StringBuilder sb = new(data);

            var tags = TagHelper.Find(data);
            foreach (var tag in tags)
            {
                string tagPath = PathUtils.ToResource(tag);
                var tagData = Load(tagPath);

                string replacementTarget = TagHelper.OpeningTag + tag + TagHelper.ClosingTag;

                if (tagData.Type == ResourceType.text)
                    sb.Replace(replacementTarget, ToBase64(tag, tagData.tData));
                else if (tagData.Type == ResourceType.binary)
                    sb.Replace(replacementTarget, ToBase64(tag, tagData.bData));
                else
                    throw new Exception($"Unhandled resource type \"{tagData.Type}\"!");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a text resource into a web-formatted base64 string.
        /// </summary>
        private static string ToBase64(string tag, string tagData)
        {
            string ext = Path.GetExtension(tag);

            string base64 = ResourceLoaderUtils.ToBase64(tagData);

            if (ext == ".svg")
                return "data:image/svg+xml;base64," + base64;
            else if (ext == ".css")
                return "data:text/css;base64," + base64;
            else if (ext == ".js")
                return "data:text/javascript;base64," + base64;
            else
                throw new Exception($"Unhandled file extension \"{ext}\"!");
        }

        /// <summary>
        /// Converts a binary resource into a web-formatted base64 string.
        /// </summary>
        private static string ToBase64(string tag, byte[] tagData)
        {
            string ext = Path.GetExtension(tag);

            string base64 = ResourceLoaderUtils.ToBase64(tagData);

            if (ext == ".woff2")
                return "data:font/woff2;base64," + base64;
            else
                throw new Exception($"Unhandled file extension \"{ext}\"!");
        }

        #endregion
    }
}
