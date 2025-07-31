using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableShadows : Feature
    {
        private const string thisID = "disableShadows";

        private enum ShadowQuality
        {
            Disable,
            HardOnly,
            All
        }

        public DisableShadows(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            var qualitySettings = EFTDMA.MonoClasses.GetClass("UnityEngine.QualitySettings");
            NativeHelper.CompileClass((ulong)qualitySettings);

            ulong setShadows = (ulong)qualitySettings.FindMethod("set_shadows");
            if (setShadows == 0x0)
                throw new Exception("Unable to find the \"set_shadows\" method of the static \"shadows\" property!");

            ulong setShadowsMethod = NativeHelper.CompileMethod(setShadows);

            if (newState)
            {
                NativeHelper.CallFunction(setShadowsMethod, (ulong)ShadowQuality.Disable);

                CurrentState = true;
            }
            else
            {
                NativeHelper.CallFunction(setShadowsMethod, (ulong)ShadowQuality.All);

                CurrentState = false;
            }
        }
    }
}
