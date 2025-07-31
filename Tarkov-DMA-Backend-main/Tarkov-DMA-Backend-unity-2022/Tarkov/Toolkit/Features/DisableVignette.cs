using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableVignette : Feature
    {
        private const string thisID = "disableVignette";

        public DisableVignette(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(CameraManager.GetPrismEffects() + Offsets.PrismEffects.useVignette, false));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(CameraManager.GetPrismEffects() + Offsets.PrismEffects.useVignette, true));

                CurrentState = false;
            }
        }
    }
}
