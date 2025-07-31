using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class NoInertia : Feature
    {
        private const string thisID = "noInertia";

        public NoInertia(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (ToolkitManager.FeatureState[thisID] == false) return;

            ulong HardSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass("EFTHardSettings"));
            ulong InertiaSettingsSingleton = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton(ClassNames.InertiaSettings.ClassName_ClassToken));

            ulong movementContext = localPlayer.MovementContext;
            writes.Add(ScatterWriteEntry.Create(movementContext + Offsets.MovementContext.WalkInertia, 0f));
            writes.Add(ScatterWriteEntry.Create(movementContext + Offsets.MovementContext.SprintBrakeInertia, 0f));

            ulong inertiaSettings = Memory.ReadPtr(InertiaSettingsSingleton + Offsets.GlobalConfigs.Inertia);
            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.FallThreshold, 99999f));
            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.BaseJumpPenalty, 0f));
            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.BaseJumpPenaltyDuration, 0f));
            writes.Add(ScatterWriteEntry.Create(inertiaSettings + Offsets.InertiaSettings.MoveTimeRange, new Vector2(0f, 0f)));

            writes.Add(ScatterWriteEntry.Create(HardSettings + Offsets.EFTHardSettings.DecelerationSpeed, 100f));
        }
    }
}
