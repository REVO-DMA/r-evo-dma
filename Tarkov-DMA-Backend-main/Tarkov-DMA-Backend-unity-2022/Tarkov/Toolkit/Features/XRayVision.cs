using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class XRayVision : Feature
    {
        private const string thisID = "xRayVision";

        private const float originalValue = 0.03f;

        public XRayVision(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            if (newState)
            {
                float newNearClipPlane = ToolkitManager.FeatureSettings_float["xRayVision_nearClipPlane"];
                CameraManager.SetNearClipPlane(newNearClipPlane);

                CurrentState = true;
            }
            else
            {
                CameraManager.SetNearClipPlane(originalValue);

                CurrentState = false;
            }
        }
    }
}
