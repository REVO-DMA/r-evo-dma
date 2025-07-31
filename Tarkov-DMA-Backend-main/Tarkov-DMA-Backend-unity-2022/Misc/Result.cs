namespace Tarkov_DMA_Backend.Misc
{
    public readonly struct Result<T>
    {
        public static readonly Result<T> Fail = new();

        public static implicit operator bool(Result<T> x) => x.Success;
        public static implicit operator T(Result<T> x) => x.Value;

        public readonly bool Success;
        public readonly T Value;

        public Result(bool success = false, T value = default)
        {
            Success = success;
            Value = value;
        }
    }
}
