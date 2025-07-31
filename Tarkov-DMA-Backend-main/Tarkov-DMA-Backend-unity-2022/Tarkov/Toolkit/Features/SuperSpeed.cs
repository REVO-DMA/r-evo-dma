using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class SuperSpeed : Feature
    {
        private const string thisID = "superSpeed";

        // Super speed hack state
        public static bool superSpeedHack_state = false;
        public static bool superSpeedHack_engaged = false;

        public SuperSpeed(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            superSpeedHack_state = ToolkitManager.FeatureState[thisID];
        }
    }
}
