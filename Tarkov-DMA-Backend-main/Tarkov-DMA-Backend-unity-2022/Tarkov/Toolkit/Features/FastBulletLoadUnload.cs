using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class FastBulletLoadUnload : Feature
    {
        private const string thisID = "fastBulletLoadUnload";

        private const float newMagDrillsLoadSpeed = 85f;
        private const float newMagDrillsUnloadSpeed = 60f;

        private static readonly byte[] Signature;
        private static readonly byte[] Patch;

        static FastBulletLoadUnload()
        {
            byte[] b = BitConverter.GetBytes(Offsets.LootItemMagazine.LoadUnloadModifier);

            Signature = new byte[]
            {
                0xF3, 0x0F, 0x10, 0x80, b[0], b[1], b[2], b[3], // movss xmm0, [rax+Offsets.ItemTemplate.LoadUnloadModifier]
            };

            // Loads a 0f into the primary fp register
            Patch = new byte[]
            {
                0x0F, 0x57, 0xC0,   // xorps xmm0,xmm0
                0x90,               // nop
                0x90,               // nop
                0x90,               // nop
                0x90,               // nop
                0x90,               // nop
            };
        }

        public FastBulletLoadUnload(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            ulong profile = Memory.ReadPtr(localPlayer.Base + Offsets.Player.Profile);
            ulong skillsPtr = Memory.ReadPtr(profile + Offsets.Profile.Skills);

            ulong magDrillsLoadSpeedPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.MagDrillsLoadSpeed);
            ulong magDrillsUnloadSpeedPtr = Memory.ReadPtr(skillsPtr + Offsets.SkillManager.MagDrillsUnloadSpeed);

            if (newState)
            {
                writes.Add(ScatterWriteEntry.Create(magDrillsLoadSpeedPtr + Offsets.SkillValueContainer.Value, newMagDrillsLoadSpeed));

                writes.Add(ScatterWriteEntry.Create(magDrillsUnloadSpeedPtr + Offsets.SkillValueContainer.Value, newMagDrillsUnloadSpeed));

                if (!EFTDMA.PatchManager.GetStatus(thisID))
                {
                    SignatureInfo sigInfo = new(Signature, Patch, 0x50);
                    PatchMethod(ClassNames.AmmoTemplate.ClassName, ClassNames.AmmoTemplate.MethodName, sigInfo, compileClass: true);

                    EFTDMA.PatchManager.SetStatus(thisID, true);
                }

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(magDrillsLoadSpeedPtr + Offsets.SkillValueContainer.Value, 25f));

                writes.Add(ScatterWriteEntry.Create(magDrillsUnloadSpeedPtr + Offsets.SkillValueContainer.Value, 15f));

                CurrentState = false;
            }
        }
    }
}
