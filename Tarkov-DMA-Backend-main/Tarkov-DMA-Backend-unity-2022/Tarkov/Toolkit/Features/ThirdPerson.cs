using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class ThirdPerson : Feature
    {
        // TODO: Change method to allow setting the intensity of the recoil.
        private const string thisID = "thirdPerson";

        public enum EPointOfView
        {
            FirstPerson,
            ThirdPerson,
            FreeCamera
        }

        public ThirdPerson(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            float horizontalDistance = ToolkitManager.FeatureSettings_float["tpp_horizontalDistance"];
            float horizontalOffset = ToolkitManager.FeatureSettings_float["tpp_horizontalOffset"];
            float verticalDistance = ToolkitManager.FeatureSettings_float["tpp_verticalDistance"];

            if (newState)
            {
                VisibilityCheck.UpdateTppSettings(horizontalDistance, horizontalOffset, verticalDistance, true);

                CurrentState = true;
            }
            else
            {
                VisibilityCheck.UpdateTppSettings(horizontalDistance, horizontalOffset, verticalDistance, false);

                CurrentState = false;
            }
        }
    }
}
