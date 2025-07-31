using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;
using static Tarkov_DMA_Backend.Unity.LowLevel.ShellKeeper;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class RemovableAttachments : AlwaysActiveFeature
    {
        private const string thisID = "removableAttachments";

        /// <summary>
        /// Whether or not this feature is fully applied and no longer needs to run.
        /// </summary>
        private bool Applied;

        public RemovableAttachments(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (!ToolkitManager.AlwaysActiveFeatureState[thisID] || Applied) return;

            SignatureInfo sigInfoFalse = new(null, PatchFalse);
            SignatureInfo sigInfoTrue = new(null, PatchTrue);

            PatchMethod(ClassNames.VitalParts.ClassName, ClassNames.VitalParts.MethodName, sigInfoFalse, 0, compileClass: true);

            PatchMethod(ClassNames.InventoryLogic_Mod.ClassName, ClassNames.InventoryLogic_Mod.MethodName, sigInfoTrue, compileClass: true);

            Applied = true;
        }
    }
}