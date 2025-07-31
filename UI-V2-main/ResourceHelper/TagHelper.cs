namespace UI_V2.ResourceHelper
{
    public static class TagHelper
    {
        public const string OpeningTag = "{rl}";
        public const string ClosingTag = "{/rl}";

        /// <summary>
        /// Finds all occurrences of the resource tag inside the given text data and extracts the tag data.
        /// </summary>
        public static IReadOnlyList<string> Find(string data)
        {
            List<string> contents = new();

            int openingIndex, closingIndex = 0;
            while ((openingIndex = data.IndexOf(OpeningTag, closingIndex)) != -1)
            {
                openingIndex += OpeningTag.Length;

                closingIndex = data.IndexOf(ClosingTag, openingIndex);

                int nextOpeningIndex = data.IndexOf(OpeningTag, openingIndex);
                if (closingIndex == -1 || (nextOpeningIndex != -1 && nextOpeningIndex < closingIndex))
                {
                    string substring = data.Substring(openingIndex, LengthOrMax(128, data.Length - openingIndex));
                    throw new Exception($"Missing closing tag at \"{substring}\"");
                }

                contents.Add(data.Substring(openingIndex, closingIndex - openingIndex));
            }

            return contents;
        }

        /// <summary>
        /// Returns the desired length if it's less than or equal to the max length.
        /// </summary>
        private static int LengthOrMax(int desiredLength, int maxLength)
        {
            if (desiredLength > maxLength)
                return maxLength;
            else
                return desiredLength;
        }
    }
}
