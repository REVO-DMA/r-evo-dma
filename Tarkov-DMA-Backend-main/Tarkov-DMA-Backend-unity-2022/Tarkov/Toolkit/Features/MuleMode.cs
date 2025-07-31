using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class MuleMode : Feature
    {
        private const string thisID = "muleMode";

        private const float newOverweight = 0f;
        private const float newWalkOverweight = 0f;
        private const float newWalkSpeedLimit = 1f;
        private const float newInertia = 0.01f;
        private const float newFloat_3 = 0f;
        private const float newSprintAcceleration = 1f;
        private const float newPreSprintAcceleration = 3f;
        private const byte zeroByte = 0;

        public MuleMode(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID]) return;

            ulong physical = Memory.ReadPtr(localPlayer.Base + Offsets.Player.Physical);

            var currentBaseOverweightLimits = Memory.ReadValue<Vector2>(physical + Offsets.Physical.BaseOverweightLimits);

            var newOverweightLimits = new Vector2(currentBaseOverweightLimits.Y - 1f, currentBaseOverweightLimits.Y);

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.Overweight, newOverweight));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.WalkOverweight, newWalkOverweight));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.WalkSpeedLimit, newWalkSpeedLimit));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.Inertia, newInertia));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.SprintWeightFactor, newFloat_3));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.SprintAcceleration, newSprintAcceleration));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.PreSprintAcceleration, newPreSprintAcceleration));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.BaseOverweightLimits, newOverweightLimits));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.SprintOverweightLimits, newOverweightLimits));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.IsOverweightA, zeroByte));

            writes.Add(ScatterWriteEntry.Create(physical + Offsets.Physical.IsOverweightB, zeroByte));
        }
    }
}
