using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class EnhancedJump : Feature
    {
        private const string thisID = "enhancedJump";

        public EnhancedJump(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            if (!CameraManager.PlayerIsInRaid)
                return;

            ulong profile = Memory.ReadPtr(localPlayer.Base + Offsets.Player.Profile);
            ulong skillsPtr = Memory.ReadPtr(profile + Offsets.Profile.Skills);
            ulong jumpHeightPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.StrengthBuffJumpHeightInc);

            var newValue = ToolkitManager.FeatureSettings_float["enhancedJump_StrengthBuffJumpHeightInc"];

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(jumpHeightPtr + Offsets.SkillValueContainer.Value, newValue));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(jumpHeightPtr + Offsets.SkillValueContainer.Value, 0.1f));

                CurrentState = false;
            }
        }
    }
}
