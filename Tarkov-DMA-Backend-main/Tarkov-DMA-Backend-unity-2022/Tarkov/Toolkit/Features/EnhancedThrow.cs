using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class EnhancedThrow : Feature
    {
        private const string thisID = "enhancedThrow";

        public EnhancedThrow(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            if (!CameraManager.PlayerIsInRaid)
                return;

            ulong profile = Memory.ReadPtr(localPlayer.Base + Offsets.Player.Profile);
            ulong skillsPtr = Memory.ReadPtr(profile + Offsets.Profile.Skills);
            ulong throwDistancePtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.StrengthBuffThrowDistanceInc);

            var newValue = ToolkitManager.FeatureSettings_float["enhancedThrow_StrengthBuffThrowDistanceInc"];

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(throwDistancePtr + Offsets.SkillValueContainer.Value, newValue));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(throwDistancePtr + Offsets.SkillValueContainer.Value, 0.1f));

                CurrentState = false;
            }
        }
    }
}
