using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class Desync : Feature
    {
        private const string thisID = "desync";

        private bool HashesBackedUp = false;

        public Desync(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool state = ToolkitManager.FeatureState[thisID];

            BackUpHashes(localPlayer);

            if (state)
            {
                if (CurrentState == true)
                    return;

                EnableGodMode(localPlayer);
                CurrentState = true;
            }
            else
            {
                if (CurrentState == false)
                    return;

                RestoreHashes(localPlayer);
                CurrentState = false;
            }
        }

        private List<int> PlayerStateContainer_OriginalHashes = new();
        private List<int> MovementState_OriginalHashes = new();
        private void BackUpHashes(Player localPlayer)
        {
            if (HashesBackedUp)
                return;

            ulong movementStatesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._movementStates);
            MemArray<ulong> movementStates = new(movementStatesAddr, false);
            foreach (var movementState in movementStates.Items)
                PlayerStateContainer_OriginalHashes.Add(Memory.ReadValue<int>(movementState + Offsets.PlayerStateContainer.StateFullNameHash, false));

            ulong statesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states);
            MemDictionary<ulong, ulong> states = new(statesAddr, false);
            foreach (var state in states.Items)
                MovementState_OriginalHashes.Add(Memory.ReadValue<int>(state.Value + Offsets.MovementState.AnimatorStateHash, false));

            HashesBackedUp = true;
        }

        private void RestoreHashes(Player localPlayer)
        {
            ulong movementStatesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._movementStates);
            MemArray<ulong> movementStates = new(movementStatesAddr, false);
            for (int i = 0; i < movementStates.Items.Count; i++)
                Memory.WriteValue(movementStates.Items[i] + Offsets.PlayerStateContainer.StateFullNameHash, PlayerStateContainer_OriginalHashes[i]);

            ulong statesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states);
            MemDictionary<ulong, ulong> states = new(statesAddr, false);
            ulong[] statesArray = states.Items.Values.ToArray();
            for (int i = 0; i < statesArray.Length; i++)
                Memory.WriteValue(statesArray[i] + Offsets.MovementState.AnimatorStateHash, MovementState_OriginalHashes[i]);
        }

        private void EnableGodMode(Player localPlayer)
        {
            ulong movementStatesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._movementStates);
            MemArray<ulong> movementStates = new(movementStatesAddr, false);
            foreach (var movementState in movementStates.Items)
                Memory.WriteValue(movementState + Offsets.PlayerStateContainer.StateFullNameHash, 0);

            ulong statesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states);
            MemDictionary<ulong, ulong> states = new(statesAddr, false);
            foreach (var state in states.Items)
                Memory.WriteValue(state.Value + Offsets.MovementState.AnimatorStateHash, 0);
        }
    }
}
