using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class AimbotSettings : Feature
    {
        private const string thisID = "aimbot";

        public static Dictionary<Bone, HitboxSettings> PlayerHitboxSettings = new();
        public static Dictionary<Bone, HitboxSettings> AIHitboxSettings = new();

        public struct HitboxSettings(float chance, bool smartTargeting)
        {
            public float Chance = chance;
            public bool SmartTargeting = smartTargeting;
        }

        public AimbotSettings(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (ToolkitManager.FeatureState[thisID])
            {
                // Aimbot State
                if (!Aimbot.Enabled)
                {
                    Aimbot.Enabled = true;
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> AimbotEnabled: false -> true");
                }

                // Always On
                bool newAimbotAlwaysOn = ToolkitManager.FeatureSettings_bool["aimbot_alwaysOn"];
                if (Aimbot.AlwaysOn != newAimbotAlwaysOn)
                {
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> Always On: {Aimbot.AlwaysOn} -> {newAimbotAlwaysOn}");
                    Aimbot.AlwaysOn = newAimbotAlwaysOn;
                    Thread.Sleep(50);
                    Aimbot.Dirty = true;
                }

                // Visibility Check
                bool newAimbotVisibilityCheck = ToolkitManager.FeatureSettings_bool["aimbot_visibilityCheck"];
                if (Aimbot.TargetingVisibilityCheck != newAimbotVisibilityCheck)
                {
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> Visibility Check: {Aimbot.TargetingVisibilityCheck} -> {newAimbotVisibilityCheck}");
                    Aimbot.TargetingVisibilityCheck = newAimbotVisibilityCheck;
                }

                // Targeting Mode
                int newAimbotTargetingMode = ToolkitManager.FeatureSettings_int["aimbot_targetingMode"];
                if (Aimbot.TargetingMode != newAimbotTargetingMode)
                {
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> Targeting Mode: {Aimbot.TargetingMode} -> {newAimbotTargetingMode}");
                    Aimbot.TargetingMode = newAimbotTargetingMode;
                }

                // Max Distance
                float newAimbotMaxDistance = ToolkitManager.FeatureSettings_float["aimbot_maxDistance"];
                if (Aimbot.MaxDistance != newAimbotMaxDistance)
                {
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> Max Distance: {Aimbot.MaxDistance} -> {newAimbotMaxDistance}");
                    Aimbot.MaxDistance = newAimbotMaxDistance;
                }

                // FOV
                float newAimbotFOV = ToolkitManager.FeatureSettings_float["aimbot_fov"] * 2f;
                if (Aimbot.FOV != newAimbotFOV)
                {
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> FOV: {Aimbot.FOV} -> {newAimbotFOV}");
                    Aimbot.FOV = newAimbotFOV;
                }

                // Target Teammates
                bool newAimbotTargetTeammates = ToolkitManager.FeatureSettings_bool["aimbot_TargetTeammates"];
                if (Aimbot.TargetTeammates != newAimbotTargetTeammates)
                {
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> Target Teammates: {Aimbot.TargetTeammates} -> {newAimbotTargetTeammates}");
                    Aimbot.TargetTeammates = newAimbotTargetTeammates;
                }

                if (!EFTDMA.PatchManager.GetStatus(thisID))
                {
                    SignatureInfo sigInfo = new(null, ShellKeeper.PatchFalse);
                    PatchMethod(ClassNames.ProceduralWeaponAnimation.ClassName, ClassNames.ProceduralWeaponAnimation.MethodName, sigInfo, compileClass: true);

                    EFTDMA.PatchManager.SetStatus(thisID, true);
                }
            }
            else
            {
                // Update aimbot configuration based on UI settings
                if (Aimbot.Enabled)
                {
                    Aimbot.Enabled = false;
                    Logger.WriteLine($"[WEAPON] -> {thisID} -> AimbotEnabled: true -> false");
                }
            }
        }
    }
}
