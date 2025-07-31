namespace UI_V2.ResourceHelper
{
    public static class ResourceLoader
    {
        /// <summary>
        /// Returns an embedded resource as text.
        /// </summary>
        public static string LoadText(string resourceName)
        {
            using Stream stream = ResourceLoaderUtils.GetResourceStream(PathUtils.GetResourcePath(resourceName));

            return ResourceLoaderUtils.ReadTextStream(stream);
        }

        /// <summary>
        /// Returns an embedded resource in binary format.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static byte[] LoadBinary(string resourceName)
        {
            using Stream stream = ResourceLoaderUtils.GetResourceStream(PathUtils.GetResourcePath(resourceName));

            return ResourceLoaderUtils.ReadBinaryStream(stream);
        }
    }
}
