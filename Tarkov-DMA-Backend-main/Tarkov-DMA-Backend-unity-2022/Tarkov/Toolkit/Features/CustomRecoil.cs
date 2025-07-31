using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class CustomRecoil : Feature
    {
        // TODO: Change method to allow setting the intensity of the recoil.
        private const string thisID = "customRecoil";

        private const int _fullExecutionInterval = 100;
        private DateTime _lastFullExecutionTime = DateTime.MinValue;

        private bool originalPWAMaskUsed = true;
        private const Enums.EProceduralAnimationMask originalPWAMask =
                    Enums.EProceduralAnimationMask.MotionReaction |
                    Enums.EProceduralAnimationMask.ForceReaction |
                    Enums.EProceduralAnimationMask.Shooting |
                    Enums.EProceduralAnimationMask.DrawDown |
                    Enums.EProceduralAnimationMask.Aiming |
                    Enums.EProceduralAnimationMask.Breathing;
        private const int newPWAMask = 1;

        private bool originalAnimatorMaskUsed = true;
        private const Enums.EAnimatorMask originalAnimatorMask =
                    Enums.EAnimatorMask.Thirdperson |
                    Enums.EAnimatorMask.Arms |
                    Enums.EAnimatorMask.Procedural |
                    Enums.EAnimatorMask.FBBIK |
                    Enums.EAnimatorMask.IK;
        private const Enums.EAnimatorMask newAnimatorMask =
                    Enums.EAnimatorMask.Thirdperson |
                    Enums.EAnimatorMask.Arms |
                    Enums.EAnimatorMask.Procedural |
                    Enums.EAnimatorMask.FBBIK;

        public CustomRecoil(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newAlwaysAim = ToolkitManager.FeatureSettings_bool["customRecoil_AlwaysAim"];
            float newShotIntensity = ToolkitManager.FeatureSettings_float["customRecoil_ShotIntensity"];
            float newBreathIntensity = ToolkitManager.FeatureSettings_float["customRecoil_BreathIntensity"];
            bool disableWeaponInertia = ToolkitManager.FeatureSettings_bool["customRecoil_DisableWeaponInertia"];

            // Ensure that there will be a full run
            if (RunImmediately)
                _lastFullExecutionTime = DateTime.MinValue;

            if (!ToolkitManager.FeatureState[thisID]) // Custom recoil is disabled
            {
                newAlwaysAim = false;
                newBreathIntensity = 1f;
                newShotIntensity = 1f;
                disableWeaponInertia = false;
            }

            TimeSpan sinceLastExecution = DateTime.UtcNow - _lastFullExecutionTime;
            if (sinceLastExecution.TotalMilliseconds < _fullExecutionInterval)
            {
                // If intensity is set to 0, remove gun movement animations
                if (disableWeaponInertia)
                {
                    // "break" the mask so we can remove all of the different "effectors"
                    writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask, newPWAMask));
                    originalPWAMaskUsed = false;
                }
                else if (!originalPWAMaskUsed && !disableWeaponInertia)
                {
                    // Restore original mask so the recoil comes back
                    writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask, (int)originalPWAMask));
                    originalPWAMaskUsed = true;
                }

                return;
            }

            // Breath intensity
            ulong breathEffector = Memory.ReadPtr(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Breath);
            writes.Add(ScatterWriteEntry.Create(breathEffector + Offsets.BreathEffector.Intensity, newBreathIntensity));

            // Recoil intensity
            ulong sEff = Memory.ReadPtr(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Shootingg);
            ulong newShotRecoil = Memory.ReadPtr(sEff + Offsets.ShotEffector.NewShotRecoil);
            writes.Add(ScatterWriteEntry.Create(newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, new Vector3(newShotIntensity, newShotIntensity, 1f)));

            // Apply always aim
            if (newAlwaysAim && originalAnimatorMaskUsed)
            {
                // Apply only third-person animations
                writes.Add(ScatterWriteEntry.Create(localPlayer.Base + Offsets.Player.EnabledAnimators, (int)newAnimatorMask));
                Logger.WriteLine($"[WEAPON] -> {thisID} -> Enabled Animators: Always Aim");
                originalAnimatorMaskUsed = false;
            }
            else if (!newAlwaysAim && !originalAnimatorMaskUsed)
            {
                // Restore original mask so the animations come back
                writes.Add(ScatterWriteEntry.Create(localPlayer.Base + Offsets.Player.EnabledAnimators, (int)originalAnimatorMask));
                Logger.WriteLine($"[WEAPON] -> {thisID} -> Enabled Animators: Original");
                originalAnimatorMaskUsed = true;
            }

            _lastFullExecutionTime = DateTime.UtcNow;
        }
    }
}
