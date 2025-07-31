using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class NoFall : Feature
    {
        private const string thisID = "noFall";

        public NoFall(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (newState)
            {
                ulong pMovementStateDict = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states, false);
                var movementStateDict = new MemDictionary<ulong, ulong>(pMovementStateDict, false);
                if (!movementStateDict.Items.Any())
                    throw new ValueOutOfRangeException(nameof(movementStateDict));

                foreach (ulong movementState in movementStateDict.Items.Values)
                {
                    writes.Add(ScatterWriteEntry.Create(movementState + Offsets.MovementState.StickToGround, false));
                }

                CurrentState = true;
            }
            else
            {
                ulong pMovementStateDict = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states, false);
                var movementStateDict = new MemDictionary<ulong, ulong>(pMovementStateDict, false);
                if (!movementStateDict.Items.Any())
                    throw new ValueOutOfRangeException(nameof(movementStateDict));

                foreach (ulong movementState in movementStateDict.Items.Values)
                {
                    writes.Add(ScatterWriteEntry.Create(movementState + Offsets.MovementState.StickToGround, true));
                }

                CurrentState = false;
            }
        }
    }
}
