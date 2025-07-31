using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class NoFallDamage : AlwaysActiveFeature
    {
        private const string thisID = "noFallDamage";

        /// <summary>
        /// Whether or not this feature is fully applied and no longer needs to run.
        /// </summary>
        private bool Applied;

        public NoFallDamage(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (!ToolkitManager.AlwaysActiveFeatureState[thisID] || Applied) return;

            SignatureInfo sigInfo = new(null, ShellKeeper.PatchReturn);
            PatchMethod(ClassNames.ActiveHealthController.ClassName, ClassNames.ActiveHealthController.MethodName, sigInfo, compileClass: true);
            Applied = true;
        }
    }
}
