using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableInventoryBlur : Feature
    {
        private const string thisID = "disableInventoryBlur";

        public DisableInventoryBlur(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            ulong inventoryBlur = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), "InventoryBlur");

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(inventoryBlur + Offsets.InventoryBlur._blurCount, 0));
                writes.Add(ScatterWriteEntry.Create(inventoryBlur + Offsets.InventoryBlur._upsampleTexDimension, (int)Enums.InventoryBlurDimensions._2048));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(inventoryBlur + Offsets.InventoryBlur._blurCount, 5));
                writes.Add(ScatterWriteEntry.Create(inventoryBlur + Offsets.InventoryBlur._upsampleTexDimension, (int)Enums.InventoryBlurDimensions._256));

                CurrentState = false;
            }
        }
    }
}
