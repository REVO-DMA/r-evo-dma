using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableCulling : Feature
    {
        private const string thisID = "disableCulling";

        public DisableCulling(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = !ToolkitManager.FeatureState[thisID];

            CameraManager.SetOclusionCulling(newState, ref writes);
        }
    }
}
