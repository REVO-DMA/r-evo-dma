using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class LootThroughWalls : Feature
    {
        private const string thisID = "lootThroughWalls";

        public const float newLootRaycastDistance = 3.801f;
        private const float newDoorRaycastDistance = 3.801f;
        private const int newLinecastMask = 0;

        private static float originalLootRaycastDistance = 0.666667f;
        private static float originalDoorRaycastDistance = 1.3f;
        private static int originalLinecastMask = -1;

        public LootThroughWalls(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            ulong EFTHardSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass("EFTHardSettings"));
            ulong GameWorld = EFTDMA.MonoClasses.GetStaticClass("EFT.GameWorld");

            var currentLootRaycastDistance = Memory.ReadValue<float>(EFTHardSettings + Offsets.EFTHardSettings.LOOT_RAYCAST_DISTANCE, false);
            var currentDoorRaycastDistance = Memory.ReadValue<float>(EFTHardSettings + Offsets.EFTHardSettings.DOOR_RAYCAST_DISTANCE, false);
            var currentLinecastMask = Memory.ReadValue<int>(GameWorld + Offsets.GameWorld.LootMaskObstruction, false);

            // Store originals so they can be restored if user disables LTW
            if (originalLootRaycastDistance == 0.666667f && currentLootRaycastDistance < newLootRaycastDistance)
                originalLootRaycastDistance = currentLootRaycastDistance;

            if (originalDoorRaycastDistance == 1.3f && currentDoorRaycastDistance < newDoorRaycastDistance)
                originalDoorRaycastDistance = currentDoorRaycastDistance;

            if (originalLinecastMask == -1 && currentLinecastMask != newLinecastMask)
                originalLinecastMask = currentLinecastMask;

            if (newState)
            {
                if (currentLootRaycastDistance != newLootRaycastDistance)
                    writes.Add(ScatterWriteEntry.Create(EFTHardSettings + Offsets.EFTHardSettings.LOOT_RAYCAST_DISTANCE, newLootRaycastDistance));

                if (currentDoorRaycastDistance != newDoorRaycastDistance)
                    writes.Add(ScatterWriteEntry.Create(EFTHardSettings + Offsets.EFTHardSettings.DOOR_RAYCAST_DISTANCE, newDoorRaycastDistance));

                if (currentLinecastMask != newLinecastMask)
                    writes.Add(ScatterWriteEntry.Create(GameWorld + Offsets.GameWorld.LootMaskObstruction, newLinecastMask));

                CurrentState = true;
            }
            else
            {
                if (currentLootRaycastDistance != originalLootRaycastDistance)
                    writes.Add(ScatterWriteEntry.Create(EFTHardSettings + Offsets.EFTHardSettings.LOOT_RAYCAST_DISTANCE, originalLootRaycastDistance));

                if (currentDoorRaycastDistance != originalDoorRaycastDistance)
                    writes.Add(ScatterWriteEntry.Create(EFTHardSettings + Offsets.EFTHardSettings.DOOR_RAYCAST_DISTANCE, originalDoorRaycastDistance));

                if (currentLinecastMask != originalLinecastMask)
                {
                    // If this was unable to be restored, set it to a working default
                    if (originalLinecastMask == -1) originalLinecastMask = 0;

                    writes.Add(ScatterWriteEntry.Create(GameWorld + Offsets.GameWorld.LootMaskObstruction, originalLinecastMask));
                }

                CurrentState = false;
            }
        }
    }
}
