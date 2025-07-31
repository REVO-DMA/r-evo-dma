using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class ThermalVision : Feature
    {
        private const string thisID = "thermalVision";

        public ThermalVision(int delayMs) : base(delayMs, thisID) { }

        /// <summary>
        /// Sets/Unsets 'Thermal Vision' for the LocalPlayer.
        /// </summary>
        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            ulong thermalVision = CameraManager.GetThermalVision();
            if (thermalVision != 0x0)
            {
                writes.Add(ScatterWriteEntry.Create(thermalVision + Offsets.ThermalVision.On, newState));
                CurrentState = newState;
            }
        }
    }
}
