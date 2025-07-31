using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class DisableScreenEffects : Feature
    {
        private const string thisID = "disableScreenEffects";

        public DisableScreenEffects(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            static void SetComponentState(string componentName, bool newState)
            {
                ulong component = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), componentName);
                Behaviour behaviour = new(Memory.ReadPtr(component + 0x10));
                if (behaviour.GetState() != newState)
                {
                    if (!behaviour.SetState(newState))
                        throw new Exception($"\"{componentName}\" state could not be set to \"{newState}\"!");
                }
            }

            if (newState)
            {
                bool noFlash = ToolkitManager.FeatureSettings_bool["disableScreenEffects_NoFlash"];
                bool noBlood = ToolkitManager.FeatureSettings_bool["disableScreenEffects_NoBlood"];
                bool noSharpen = ToolkitManager.FeatureSettings_bool["disableScreenEffects_NoSharpen"];
                bool noBlur = ToolkitManager.FeatureSettings_bool["disableScreenEffects_NoBlur"];

                if (noFlash)
                {
                    ulong effectsController = Memory.GetObjectComponent(CameraManager.GetFPSCamera(), "EffectsController");

                    if (!EFTDMA.PatchManager.GetStatus(thisID))
                    {
                        // Disables the screen going dark
                        SignatureInfo sigInfoTrue = new(null, Unity.LowLevel.ShellKeeper.PatchReturn);
                        PatchMethod(ClassNames.GrenadeFlashScreenEffect.ClassName, ClassNames.GrenadeFlashScreenEffect.MethodName, sigInfoTrue, compileClass: true);

                        EFTDMA.PatchManager.SetStatus(thisID, true);
                    }

                    SetComponentState("EyeBurn", false);
                    SetComponentState("CC_Wiggle", false);
                    SetComponentState("CC_DoubleVision", false);
                }

                if (noBlood)
                {
                    SetComponentState("BloodOnScreen", false);
                }

                if (noSharpen)
                {
                    SetComponentState("CC_Sharpen", false);
                    SetComponentState("CC_FastVignette", false);
                }

                if (noBlur)
                {
                    SetComponentState("CC_RadialBlur", false);
                    SetComponentState("DistortCameraFX", false);
                }
            }
        }
    }
}
