using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;
using Tarkov_DMA_Backend.MemDMA.EFT;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class FullBright : Feature
    {
        private const string thisID = "fullBright";

        private enum AmbientMode
        {
            Skybox,
            Trilight,
            Flat = 3,
            Custom
        }

        public FullBright(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            ulong LevelSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton("LevelSettings"));

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(LevelSettings + Offsets.LevelSettings.AmbientMode, (int)AmbientMode.Trilight));

                float brightness = ToolkitManager.FeatureSettings_float["fullBright_brightness"];
                writes.Add(ScatterWriteEntry.Create(LevelSettings + Offsets.LevelSettings.EquatorColor, new UnityColor(brightness, brightness, brightness)));

                writes.Add(ScatterWriteEntry.Create(LevelSettings + Offsets.LevelSettings.GroundColor, new UnityColor(0f, 0f, 0f)));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(LevelSettings + Offsets.LevelSettings.AmbientMode, (int)AmbientMode.Flat));

                CurrentState = false;
            }
        }
    }
}
