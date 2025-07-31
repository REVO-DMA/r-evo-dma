using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class InstantADS : Feature
    {
        private const string thisID = "instantADS";

        public InstantADS(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._aimingSpeed, 9999f));

                CurrentState = true;
            }
            else
            {
                // Only disable once
                if (CurrentState == false)
                    return;

                writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._aimingSpeed, 1f));

                CurrentState = false;
            }
        }
    }
}
