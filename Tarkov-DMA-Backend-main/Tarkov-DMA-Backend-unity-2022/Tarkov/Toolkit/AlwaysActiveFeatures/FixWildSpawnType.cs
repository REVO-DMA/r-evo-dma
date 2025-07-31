using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class FixWildSpawnType : AlwaysActiveFeature
    {
        public const string featureID = "fixWildSpawnType";

        const int ReadSize = 0x100;

        private static readonly byte[] Signature;
        private static readonly byte[] SignaturePatched;

        static FixWildSpawnType()
        {
            byte o1 = (byte)Offsets.PlayerSpawnInfo.Side;
            byte o2 = (byte)Offsets.InfoContainer.Side;

            byte p1 = (byte)Offsets.PlayerSpawnInfo.WildSpawnType;

            Signature = new byte[]
            {
                0x48, 0x63, 0x40, o1,   // movsxd rax, dword ptr [rax+SIDE_OFFSET_PlayerSpawnInfo]
                0x89, 0x46, o2,         // mov [rsi+SIDE_OFFSET_InfoContainer], eax
            };

            // The idea here is to change the SetUpSpawnInfo() method to set the "Side"
            // field to the value of "WildSpawnType" instead of "Side".
            SignaturePatched = new byte[]
            {
                0x48, 0x63, 0x40, p1,   // movsxd rax, dword ptr [rax+SIDE_OFFSET_PlayerSpawnInfo]
                0x89, 0x46, o2,         // mov [rsi+SIDE_OFFSET_InfoContainer], eax
            };
        }

        public FixWildSpawnType(int delayMs) : base(delayMs, featureID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            if (CurrentState) return;

            var infoContainer = EFTDMA.MonoClasses.GetClass(ClassNames.FixWildSpawnType.ClassName_ClassToken);

            ulong compiledClass = NativeHelper.CompileClass((ulong)infoContainer);
            if (compiledClass == 0x0)
                throw new Exception($"Unable to compile the \"{ClassNames.FixWildSpawnType.ClassName}\" class!");

            SignatureInfo sigInfo = new(Signature, SignaturePatched, ReadSize);

            PatchMethod(infoContainer, ClassNames.FixWildSpawnType.ClassName, ClassNames.FixWildSpawnType.MethodName, sigInfo);

            CurrentState = true;
        }
    }
}
