using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class AntiAFK : AlwaysActiveFeature
    {
        private const string thisID = "antiAFK";

        private const string patchMethod = "MoveNext";
        private static readonly byte[] Signature = new byte[]
        {
            0x74, 0x0,              // je +122
            0x33, 0xC0,             // xor eax, eax
            0x4C, 0x0F, 0xB6, 0xF8, // movzx r15, al
            0xE9                    // jmp ...
        };
        private static readonly byte[] Patch = new byte[]
        {
            0x90, 0x90 // nop nop
        };
        private const string Mask = "x?xxxxxxx";

        /// <summary>
        /// Whether or not this feature is fully applied and no longer needs to run.
        /// </summary>
        private bool Applied;

        public AntiAFK(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (!ToolkitManager.AlwaysActiveFeatureState[thisID] || Applied) return;

            SignatureInfo sigInfo = new(Signature, Signature.Patch(Patch), 0x200, Mask, Mask, 0, Patch);

            PatchMethod(ClassNames.AFKMonitor.ClassName, patchMethod, sigInfo, 2, compileClass: true);

            Applied = true;
        }
    }
}
