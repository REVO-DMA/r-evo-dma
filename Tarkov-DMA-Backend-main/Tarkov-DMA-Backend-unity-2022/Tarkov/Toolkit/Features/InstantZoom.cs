using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class InstantZoom : Feature
    {
        private const string thisID = "instantZoom";

        public static bool instantZoom_engaged = false;
        private float savedFOV = 75f;

        public InstantZoom(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID]) return;

            if (instantZoom_engaged && CurrentState == false)
            {
                // Enabled

                int newFOV = ToolkitManager.FeatureSettings_int["instantZoom_FOV"];

                ulong fpsCamera = Memory.GetCamera(CameraManager.GetFPSCamera());
                savedFOV = Memory.ReadValue<float>(fpsCamera + UnityOffsets.Camera.FOV);
                Memory.WriteValue(fpsCamera + UnityOffsets.Camera.FOV, (float)newFOV);

                CurrentState = true;
            }
            else if (!instantZoom_engaged && CurrentState == true)
            {
                // Disabled

                ulong fpsCamera = Memory.GetCamera(CameraManager.GetFPSCamera());
                Memory.WriteValue(fpsCamera + UnityOffsets.Camera.FOV, savedFOV);

                CurrentState = false;
            }
        }
    }
}
