using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;
using static Tarkov_DMA_Backend.Unity.LowLevel.ShellKeeper;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class NoMalfunctions : AlwaysActiveFeature
    {
        private const string thisID = "noMalfunctions";

        /// <summary>
        /// Whether or not this feature is fully applied and no longer needs to run.
        /// </summary>
        private bool Applied;

        public NoMalfunctions(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (!ToolkitManager.AlwaysActiveFeatureState[thisID] || Applied) return;

            SignatureInfo sigInfo = new(null, ReturnInt((int)Enums.EMalfunctionState.None));
            PatchMethod(ClassNames.NoMalfunctions.ClassName, ClassNames.NoMalfunctions.GetMalfunctionState, sigInfo, compileClass: true);

            Applied = true;
        }
    }
}
