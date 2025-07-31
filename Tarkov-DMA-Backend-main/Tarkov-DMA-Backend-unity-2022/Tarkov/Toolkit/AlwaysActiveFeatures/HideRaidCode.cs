using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class HideRaidCode : AlwaysActiveFeature
    {
        private const string thisID = "hideRaidCode";

        public HideRaidCode(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            bool newState = ToolkitManager.AlwaysActiveFeatureState[thisID];

            if (newState == CurrentState) return;

            ulong alphaLabel = NativeHelper.FindGameObject("Preloader UI/Preloader UI/BottomPanel/Content/UpperPart/AlphaLabel");
            if (alphaLabel == 0x0)
                return;

            if (newState)
            {
                NativeHelper.GameObjectSetActive(alphaLabel, false);

                CurrentState = true;
            }
            else
            {
                NativeHelper.GameObjectSetActive(alphaLabel, true);

                CurrentState = false;
            }
        }
    }
}
