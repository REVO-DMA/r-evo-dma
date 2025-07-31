using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class InfiniteStamina : Feature
    {
        private const string thisID = "infiniteStamina";

        private const Enums.EPlayerState originalPlayerState = Enums.EPlayerState.Sprint;
        private const Enums.EPlayerState patchPlayerState = Enums.EPlayerState.Transition;

        private const float maxStamina = 100f;
        private const float maxHandsStamina = 100f;
        private const float maxOxygen = 100f;

        private bool fatigueBypassed = false;

        public InfiniteStamina(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID]) return;

            if (!fatigueBypassed)
            {
                ulong currentState = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext.CurrentState, false);
                var movementStateName = (Enums.EPlayerState)Memory.ReadValue<byte>(currentState + Offsets.MovementState.Name, false);
                if (movementStateName == originalPlayerState)
                {
                    ApplyFatigueBypass(localPlayer, currentState);

                    fatigueBypassed = true;
                }
            }

            ulong physical = Memory.ReadPtr(localPlayer + Offsets.Player.Physical);

            ulong staminaObj = Memory.ReadPtr(physical + Offsets.Physical.Stamina);
            float currentStamina = Memory.ReadValue<float>(staminaObj + Offsets.PhysicalValue.Current);
            if (currentStamina <= (maxStamina / 3))
                writes.Add(ScatterWriteEntry.Create(staminaObj + Offsets.PhysicalValue.Current, maxStamina));

            ulong handsStaminaObj = Memory.ReadPtr(physical + Offsets.Physical.HandsStamina);
            float currentHandsStamina = Memory.ReadValue<float>(handsStaminaObj + Offsets.PhysicalValue.Current);
            if (currentHandsStamina <= (maxHandsStamina / 3))
                writes.Add(ScatterWriteEntry.Create(handsStaminaObj + Offsets.PhysicalValue.Current, maxHandsStamina));

            ulong oxygenObj = Memory.ReadPtr(physical + Offsets.Physical.Oxygen);
            float currentOxygen = Memory.ReadValue<float>(oxygenObj + Offsets.PhysicalValue.Current);
            if (currentOxygen <= (maxOxygen / 3))
                writes.Add(ScatterWriteEntry.Create(oxygenObj + Offsets.PhysicalValue.Current, maxOxygen));
        }

        private static void ApplyFatigueBypass(Player localPlayer, ulong currentState)
        {
            ulong movementStatesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._movementStates);
            MemArray<ulong> movementStates = new(movementStatesAddr, false);
            ulong originalState = 0x0;
            foreach (var movementState in movementStates.Items)
            {
                var stateName = (Enums.EPlayerState)Memory.ReadValue<byte>(movementState + Offsets.PlayerStateContainer.Name, false);
                if (stateName == originalPlayerState)
                {
                    originalState = movementState;
                    break;
                }
            }

            ulong statesAddr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states);
            MemDictionary<ulong, ulong> states = new(statesAddr, false);
            ulong patchState = 0x0;
            foreach (var state in states.Items)
            {
                var stateName = (Enums.EPlayerState)Memory.ReadValue<byte>(state.Value + Offsets.MovementState.Name, false);
                if (stateName == patchPlayerState)
                {
                    patchState = state.Value;
                    break;
                }
            }

            int targetHash = Memory.ReadValue<int>(patchState + Offsets.MovementState.AnimatorStateHash, false);
            Memory.WriteValue(originalState + Offsets.PlayerStateContainer.StateFullNameHash, targetHash);
            Memory.WriteValue(currentState + Offsets.MovementState.AnimatorStateHash, targetHash);
            Memory.WriteValue(currentState + Offsets.MovementState.Name, patchPlayerState);
        }
    }
}
