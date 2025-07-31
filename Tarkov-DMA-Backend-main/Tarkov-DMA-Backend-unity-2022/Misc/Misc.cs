namespace Tarkov_DMA_Backend.Misc
{
    public static class Misc
    {
        public static float GetRandomFloat(float minValue, float maxValue)
        {
            float range = maxValue - minValue;
            float sample = (float)Random.Shared.NextDouble();
            
            return minValue + sample * range;
        }
    }

    /// <summary>
    /// The ValueOutOfRangeException is thrown when a value/collection
    /// is outside the nominal range for that value.
    /// </summary>
    public sealed class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException() { }

        public ValueOutOfRangeException(string message) : base(message) { }

        public ValueOutOfRangeException(string message, Exception inner) : base(message, inner) { }
    }
}
