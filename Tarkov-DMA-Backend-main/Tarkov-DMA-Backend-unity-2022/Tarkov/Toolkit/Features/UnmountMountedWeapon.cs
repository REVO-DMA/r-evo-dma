using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class UnmountMountedWeapon : Feature
    {
        private const string thisID = "unmountMountedWeapon";

        public UnmountMountedWeapon(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            ulong currentState = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext.CurrentState);

            if (newState) // enabled
            {
                writes.Add(ScatterWriteEntry.Create(currentState + Offsets.StationaryWeapon.IsMounted, true));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(currentState + Offsets.StationaryWeapon.IsMounted, false));

                CurrentState = false;
            }
        }
    }
}
