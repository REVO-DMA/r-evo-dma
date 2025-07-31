using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.Collections;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class SpeedHack : Feature
    {
        private const string thisID = "speedHack";

        public SpeedHack(int delayMs) : base(delayMs, thisID) { }

        public static ulong GetAnimatorSpeedHackAddress(ulong localPlayer)
        {
            ulong animatorsArrPtr = Memory.ReadPtr(localPlayer + Offsets.Player._animators);
            var animatorsArray = new MemArray<ulong>(animatorsArrPtr);
            ulong playerAnimatorPtr = animatorsArray.Items[0]; // -.GClass1141 : Object, IAnimator
            ulong playerAnimator = Memory.ReadPtr(playerAnimatorPtr + 0x10); // animator_0 : UnityEngine.Animator
            ulong anim_m_CachedPtr = Memory.ReadPtr(playerAnimator + 0x10); // m_CachedPtr
            ulong anim_gameObject = Memory.ReadPtr(anim_m_CachedPtr + 0x30);
            ulong anim_noClue = Memory.ReadPtr(anim_gameObject + 0x30);
            return Memory.ReadPtr(anim_noClue + 0x18) + UnityOffsets.Animator.Speed; // UnityEngine.Animator
        }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];
            if (newState == CurrentState && OverriddenState != false && RunImmediately == false) return;

            if (!CameraManager.PlayerIsInRaid)
                return;

            float normalSpeedHackSpeed = ToolkitManager.FeatureSettings_float["normalSpeedHack_speed"];

            // Handle super speed hotkey override
            if (OverriddenState == false)
            {
                // Animator is used in offline raids
                if (EFTDMA.IsOffline)
                    return;

                writes.Add(ScatterWriteEntry.Create(GetAnimatorSpeedHackAddress(localPlayer.Base), 1f));
                return;
            }

            if (ToolkitManager.FeatureState[thisID])
            {
                writes.Add(ScatterWriteEntry.Create(GetAnimatorSpeedHackAddress(localPlayer.Base), normalSpeedHackSpeed));

                CurrentState = true;
            }
            else
            {
                writes.Add(ScatterWriteEntry.Create(GetAnimatorSpeedHackAddress(localPlayer.Base), 1f));

                CurrentState = false;
            }
        }
    }
}
