using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class KeybindFromAnywhere : AlwaysActiveFeature
    {
        private const string thisID = "keybindFromAnywhere";

        private static readonly byte[] Signature = new byte[]
        {
            0x0F, 0x84, 0x0, 0x0, 0x0, 0x0, // je this+0x30A -> this.Inventory.GetItemsInSlots(Inventory.BindAvailableSlotsExtended).Contains(item)
            0x48, 0x8B, 0xCF,               // mov rcx, rdi
            0x48, 0x8B, 0xD6,               // mov rdx, rsi
        };
        private static readonly byte[] Patch = new byte[]
        {
            0x90, 0x90, 0x90, 0x90, 0x90, 0x90 // nop x6
        };
        private static readonly string Mask = "xx????xxxxxx";

        /// <summary>
        /// Whether or not this feature is fully applied and no longer needs to run.
        /// </summary>
        private bool Applied;

        public KeybindFromAnywhere(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (!ToolkitManager.AlwaysActiveFeatureState[thisID] || Applied) return;

            SignatureInfo sigInfoA = new(Signature, Patch, 0x200, Mask);
            PatchMethod(ClassNames.InventoryController.ClassName, ClassNames.InventoryController.KeybindFromAnywhereMethodA, sigInfoA, compileClass: true);

            SignatureInfo sigInfoB = new(null, ShellKeeper.PatchTrue);
            PatchMethod(ClassNames.InventoryController.ClassName, ClassNames.InventoryController.KeybindFromAnywhereMethodB, sigInfoB, compileClass: true);

            Applied = true;
        }
    }
}
