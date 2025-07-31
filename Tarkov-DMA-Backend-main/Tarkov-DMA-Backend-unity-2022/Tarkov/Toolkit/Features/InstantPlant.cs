using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class InstantPlant : Feature
    {
        private const string thisID = "instantPlant";

        public InstantPlant(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID]) return;

            ulong currentState = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext.CurrentState, false);
            writes.Add(ScatterWriteEntry.Create(currentState + Offsets.MovementState.PlantTime, 1f));
        }
    }
}
