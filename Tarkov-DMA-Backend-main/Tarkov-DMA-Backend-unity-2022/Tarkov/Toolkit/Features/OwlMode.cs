using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class OwlMode : Feature
    {
        private const string thisID = "360Freelook";

        private Vector2 originalMouseLookHorizontalLimit = new(-40f, 40f);
        private Vector2 newMouseLookHorizontalLimit = new(-180f, 180f);

        public OwlMode(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            ulong hardSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass("EFTHardSettings"));

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(hardSettings + Offsets.EFTHardSettings.MOUSE_LOOK_HORIZONTAL_LIMIT, newMouseLookHorizontalLimit));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(hardSettings + Offsets.EFTHardSettings.MOUSE_LOOK_HORIZONTAL_LIMIT, originalMouseLookHorizontalLimit));

                CurrentState = false;
            }
        }
    }
}
