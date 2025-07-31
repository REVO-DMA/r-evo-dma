using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class CharacterManipulation : Feature
    {
        private const string thisID = "characterManipulation";

        // Super speed hack state
        public static bool characterManipulation_state = false;
        public static bool characterManipulation_engaged = false;
        public static bool characterManipulationUp_engaged = false;
        public static bool characterManipulationDown_engaged = false;

        public CharacterManipulation(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            characterManipulation_state = ToolkitManager.FeatureState[thisID];
        }
    }
}
