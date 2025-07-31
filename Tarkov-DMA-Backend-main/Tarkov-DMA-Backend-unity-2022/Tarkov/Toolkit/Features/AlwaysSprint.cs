using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;
using static Tarkov_DMA_Backend.Unity.LowLevel.ShellKeeper;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class AlwaysSprint : Feature
    {
        private const string thisID = "alwaysSprint";

        public AlwaysSprint(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            if (newState)
            {
                if (!EFTDMA.PatchManager.GetStatus(thisID))
                {
                    SignatureInfo sigInfo = new(null, PatchTrue);
                    PatchMethod(ClassNames.MovementContext.ClassName, ClassNames.MovementContext.MethodName, sigInfo, compileClass: true);

                    EFTDMA.PatchManager.SetStatus(thisID, true);
                }

                writes.Add(ScatterWriteEntry.Create(localPlayer.MovementContext + Offsets.MovementContext._physicalCondition, (int)Enums.EPhysicalCondition.None));

                CurrentState = true;
            }
            else
            {
                // Only disable once
                if (CurrentState == false)
                    return;

                CurrentState = false;
            }
        }
    }
}
