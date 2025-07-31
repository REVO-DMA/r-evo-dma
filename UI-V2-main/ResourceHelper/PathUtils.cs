namespace UI_V2.ResourceHelper
{
    public static class PathUtils
    {
        private const string Namespace = "UI_V2";

        /// <summary>
        /// A .NET resource uses dots as path separators.
        /// This converts a normal path into the correct format for a .NET resource.
        /// </summary>
        public static string ToResource(string path)
        {
            string resource = path.Replace('/', '.').Replace('\\', '.');

            return resource;
        }

        /// <summary>
        /// Adds the assembly namespace to a resource identifier.
        /// </summary>
        public static string GetResourcePath(string resourceID)
        {
            return $"{Namespace}.{resourceID}";
        }
    }
}
