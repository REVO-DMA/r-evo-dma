using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class FOVChanger : Feature
    {
        private const string thisID = "fovChanger";

        public FOVChanger(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            static ulong GetSettingValueClass()
            {
                ulong GameSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetSingleton(ClassNames.GameSettings.ClassName_ClassToken));

                ulong Game = Memory.ReadPtr(GameSettings + Offsets.GameSettingsContainer.Game);
                ulong Settings = Memory.ReadPtr(Game + Offsets.GameSettingsInnerContainer.Settings);
                ulong FieldOfView = Memory.ReadPtr(Settings + Offsets.GameSettings.FieldOfView);
                ulong SettingValueClass = Memory.ReadPtr(FieldOfView + Offsets.BSGGameSetting.ValueClass);

                return SettingValueClass;
            }

            static void WriteFOV(float fov)
            {
                ulong fpsCamera = Memory.GetCamera(CameraManager.GetFPSCamera());
                Memory.WriteValue(fpsCamera + UnityOffsets.Camera.FOV, fov);
            }

            if (ToolkitManager.FeatureState[thisID])
            {
                if (!EFTDMA.PatchManager.GetStatus(thisID))
                {
                    SignatureInfo sigInfoTrue = new(null, ShellKeeper.PatchReturn);
                    PatchMethod(ClassNames.FovChanger.ClassName, ClassNames.FovChanger.MethodName, sigInfoTrue, compileClass: true);

                    EFTDMA.PatchManager.SetStatus(thisID, true);
                }

                int newFOV = ToolkitManager.FeatureSettings_int["fovChanger_FOV"];
                int newAimFOV = ToolkitManager.FeatureSettings_int["fovChanger_AimFOV"];

                // Handle if TPP is enabled
                if (ToolkitManager.FeatureState["thirdPerson"])
                {
                    int tppFOV = ToolkitManager.FeatureSettings_int["fovChanger_TppFOV"];

                    newFOV = tppFOV;
                }

                // Handle if instant zoom is enabled
                if (InstantZoom.instantZoom_engaged)
                {
                    int instantZoomFOV = ToolkitManager.FeatureSettings_int["instantZoom_FOV"];

                    newFOV = instantZoomFOV;
                    newAimFOV = instantZoomFOV;
                }

                // Only apply when initally changed
                if (RunImmediately)
                {
                    writes.Add(ScatterWriteEntry.Create(GetSettingValueClass() + Offsets.BSGGameSettingValueClass.Value, newFOV));
                    WriteFOV(newFOV);
                }

                bool isAiming = Memory.ReadValue<bool>(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._isAiming, false);
                if (isAiming)
                    WriteFOV(newAimFOV);
                else
                    WriteFOV(newFOV);

                CurrentState = true;
            }
            else
            {
                // Only disable once
                if (CurrentState == false)
                    return;

                // Only apply when initally changed
                if (RunImmediately)
                {
                    writes.Add(ScatterWriteEntry.Create(GetSettingValueClass() + Offsets.BSGGameSettingValueClass.Value, 75));
                    WriteFOV(75);
                }

                CurrentState = false;
            }
        }
    }
}
