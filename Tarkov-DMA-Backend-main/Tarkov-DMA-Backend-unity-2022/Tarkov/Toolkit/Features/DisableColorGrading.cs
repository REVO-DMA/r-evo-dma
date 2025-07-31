using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableColorGrading : Feature
    {
        private const string thisID = "disableColorGrading";

        public DisableColorGrading(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(CameraManager.GetCC_Vintage() + Offsets.CC_Vintage.amount, 0f));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(CameraManager.GetCC_Vintage() + Offsets.CC_Vintage.amount, 0.498f));

                CurrentState = false;
            }
        }
    }
}
