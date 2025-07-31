using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class NightVision : Feature
    {
        private const string thisID = "nightVision";

        public NightVision(int delayMs) : base(delayMs, thisID) { }

        /// <summary>
        /// Sets/Unsets 'Night Vision' for the LocalPlayer.
        /// </summary>
        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            ulong nightVision = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), "NightVision");
            if (nightVision != 0x0)
            {
                writes.Add(ScatterWriteEntry.Create(nightVision + Offsets.NightVision._on, newState));
                CurrentState = newState;
            }
        }
    }
}
