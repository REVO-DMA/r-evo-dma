using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class NoVisor : Feature
    {
        private const string thisID = "noVisor";

        private const float disableVisor = 0f;
        private const float enableVisor = 1f;

        public NoVisor(int delayMs) : base(delayMs, thisID) { }

        /// <summary>
        /// Sets 'No Visor' for LocalPlayer.
        /// </summary>
        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            static ulong GetVisorEffect()
            {
                return Memory.GetObjectComponent(CameraManager.GetFPSCamera(), "VisorEffect");
            }

            if (newState)
            {
                ulong visorEffect = GetVisorEffect();
                if (visorEffect == 0x0)
                    return;

                writes.Add(ScatterWriteEntry.Create(visorEffect + Offsets.VisorEffect.Intensity, disableVisor));

                CurrentState = true;
            }
            else
            {
                // Only disable once
                if (CurrentState == false)
                    return;

                ulong visorEffect = GetVisorEffect();
                if (visorEffect == 0x0)
                {
                    CurrentState = false;
                    return;
                }

                writes.Add(ScatterWriteEntry.Create(visorEffect + Offsets.VisorEffect.Intensity, enableVisor));

                CurrentState = false;
            }
        }
    }
}
