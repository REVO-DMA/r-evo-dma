namespace Tarkov_DMA_Backend.Misc
{
    public static class EnumHelper
    {
        /// <summary>
        /// Tries to convert a string representing an enum value to the matching enum value.
        /// </summary>
        public static Result<T> TryGetValue<T>(string value) where T : Enum
        {
            if (Enum.TryParse(typeof(T), value, true, out var res))
                return new(true, (T)res);

            return Result<T>.Fail;
        }
    }
}
