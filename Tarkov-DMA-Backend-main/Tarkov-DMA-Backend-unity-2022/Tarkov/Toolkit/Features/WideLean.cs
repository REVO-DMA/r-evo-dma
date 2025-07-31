using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Unity.LowLevel;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class WideLean : Feature
    {
        private const string thisID = "wideLean";

        // Wide lean vertical hotkey states
        public static bool VerticalAbove_hotkey = false;
        public static bool VerticalBelow_hotkey = false;

        public WideLean(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            bool newState = ToolkitManager.FeatureState[thisID];

            ulong EFTHardSettings = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass("EFTHardSettings"));

            if (newState) // Wide Lean is enabled
            {
                float tilt = Memory.ReadValue<float>(localPlayer.MovementContext + Offsets.MovementContext._tilt, false);

                bool noWideLean = true;

                float vertical = 0f;
                float horizontal = 0f;

                // Allow the player to AIM even when the gun is through a wall.
                writes.Add(ScatterWriteEntry.Create(EFTHardSettings + Offsets.EFTHardSettings.STOP_AIMING_AT, float.MaxValue));

                float newHorizontalDistance = ToolkitManager.FeatureSettings_float["wideLean_horizontalDistance"];
                float newVerticalDistance = ToolkitManager.FeatureSettings_float["wideLean_verticalDistance"];

                // Horizontal
                if (tilt == -5f) // Left Lean
                {
                    // Left lean has hit reg issues beyond 0.3
                    if (newHorizontalDistance > 0.3f)
                        newHorizontalDistance = 0.3f;

                    horizontal = -newHorizontalDistance;

                    noWideLean = false;
                }
                else if (tilt == 5f) // Right Lean
                {
                    horizontal = newHorizontalDistance;

                    noWideLean = false;
                }
                // Vertical
                else if (VerticalAbove_hotkey) // Above
                {
                    vertical = newVerticalDistance;

                    noWideLean = false;
                }
                else if (VerticalBelow_hotkey) // Below
                {
                    vertical = -newVerticalDistance;

                    noWideLean = false;
                }

                if (noWideLean)
                {
                    writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.CameraSmoothOut, 8f));
                    writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, new Vector3(0f, 0f, 0f)));
                }
                else
                {
                    writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.CameraSmoothOut, 32f));
                    writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, new Vector3(horizontal, 0f, vertical)));
                }
            }
            else // Wide Lean is disabled
            {
                writes.Add(ScatterWriteEntry.Create(EFTHardSettings + Offsets.EFTHardSettings.STOP_AIMING_AT, 0.1f));

                writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.CameraSmoothOut, 8f));
                writes.Add(ScatterWriteEntry.Create(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, new Vector3(0f, 0f, 0f)));
            }
        }
    }
}
