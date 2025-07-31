using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;
using Tarkov_DMA_Backend.MemDMA.EFT;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class NoPenalties : AlwaysActiveFeature
    {
        private const string thisID = "noPenalties";

        const int type1ReadSize = 0x200;
        const int type2ReadSize = 0x200;

        private static readonly byte[] SignatureType1 = new byte[]
        {
            0xF3, 0x0F, 0x5A, 0xC0, // cvtss2sd xmm0,xmm0
            0xF2, 0x0F, 0x5A, 0xC0, // cvtsd2ss xmm0,xmm0
        };
        private static readonly byte[] SignatureType1Patched = new byte[]
        {
            0x0F, 0x57, 0xC0, // xorps xmm0, xmm0
            0x90, // nop
            0x90, // nop
        };

        private static readonly byte[] SignatureType2 = new byte[]
        {
            0xF2, 0x0F, 0x58, 0xC1, // addsd xmm0,xmm1
            0xF2, 0x0F, 0x5A, 0xC0, // cvtsd2ss xmm0,xmm0
        };
        private static readonly byte[] SignatureType2Patched = new byte[]
        {
            0x0F, 0x57, 0xC0, // xorps xmm0, xmm0
            0x90, // nop
        };

        /// <summary>
        /// Whether or not this feature is fully applied and no longer needs to run.
        /// </summary>
        private bool Applied;

        public NoPenalties(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (!ToolkitManager.AlwaysActiveFeatureState[thisID] || Applied) return;

            var penaltyComponent = EFTDMA.MonoClasses.GetClass(ClassNames.EquipmentPenaltyComponent.ClassName_ClassToken);

            ulong compiledClass = NativeHelper.CompileClass((ulong)penaltyComponent);
            if (compiledClass == 0x0)
                throw new Exception($"Unable to compile the \"{ClassNames.EquipmentPenaltyComponent.ClassName}\" class!");

            // Type 1 sig -  all of the type 2 methods call this method to get their base value
            SignatureInfo type1SigInfo = new(SignatureType1, SignatureType1Patched, type1ReadSize, null, null, -0x5);

            PatchMethod(penaltyComponent, ClassNames.EquipmentPenaltyComponent.ClassName, ClassNames.EquipmentPenaltyComponent.BaseCalculationMethod, type1SigInfo);

            // Type 2 sig - these return floats subsequently used on the UI
            SignatureInfo type2SigInfo = new(SignatureType2, SignatureType2Patched, type2ReadSize);

            PatchMethod(penaltyComponent, ClassNames.EquipmentPenaltyComponent.ClassName, ClassNames.EquipmentPenaltyComponent.SpeedPenaltyPercent, type2SigInfo);
            PatchMethod(penaltyComponent, ClassNames.EquipmentPenaltyComponent.ClassName, ClassNames.EquipmentPenaltyComponent.MousePenalty, type2SigInfo);
            PatchMethod(penaltyComponent, ClassNames.EquipmentPenaltyComponent.ClassName, ClassNames.EquipmentPenaltyComponent.WeaponErgonomicPenalty, type2SigInfo);

            Applied = true;
        }
    }
}