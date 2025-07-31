using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.UI
{
    public static class UICache
    {
#nullable enable
        /// <summary>
        /// The map that is active in the game process.
        /// </summary>
        public static JSON.MapConfig MapConfig { get; set; }
#nullable disable
        /// <summary>
        /// The map that is loaded on the Radar UI.
        /// </summary>
        public static string CurrentMap { get; set; } = string.Empty;
    }
}
