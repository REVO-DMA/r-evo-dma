using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity.LowLevel;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class SunnyWeather : Feature
    {
        private const string thisID = "sunnyWeather";

        public SunnyWeather(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            string currentMap = UICache.CurrentMap;
            if (currentMap == string.Empty ||
                currentMap == "laboratory" ||
                currentMap == "factory")
            {
                return;
            }

            ulong WeatherController = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass("EFT.Weather.WeatherController"));

            var WeatherDebug = Memory.ReadPtr(WeatherController + Offsets.WeatherController.WeatherDebug);

            if (newState)
            {
                // Enable weather debug
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.isEnabled, true));

                // Set weather to sunny
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.WindMagnitude, 0f));
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.CloudDensity, 0f));
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.Fog, 0.001f));
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.Rain, 0f));
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.LightningThunderProbability, 0f));

                CurrentState = true;
            }
            else
            {
                // Disable weather debug
                writes.Add(ScatterWriteEntry.Create(WeatherDebug + Offsets.WeatherDebug.isEnabled, false));

                CurrentState = false;
            }
        }
    }
}
