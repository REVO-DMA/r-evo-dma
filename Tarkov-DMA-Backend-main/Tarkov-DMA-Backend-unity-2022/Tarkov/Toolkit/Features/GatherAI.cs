using Tarkov_DMA_Backend.IPC;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.UI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class GatherAI : Feature
    {
        private const string thisID = "gatherAI";

        private volatile bool running = false;

        public GatherAI(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID]) return;

            if (OverriddenState == true)
            {
                OverriddenState = null;

                if (running)
                    return;

                Task.Run(async () =>
                {
                    try
                    {
                        running = true;

                        Server.SendRadarStatus(Constants.RadarStatuses.TeleportingAICountdown);

                        // Save the localPlayer position
                        var savedLocalPos = Memory.ReadValue<Vector3>(localPlayer.Bones[0].HierarchyAddr + 0x90, false);

                        await Task.Delay(3000);

                        Player[] Players = EFTDMA.DisplayPlayers;
                        if (Players == null) return;

                        List<ScatterWriteEntry> tpWrites = new();

                        foreach (Player player in Players)
                        {
                            if (player == null || !player.FullyAllocated || !player.IsAI)
                                continue;

                            tpWrites.Add(ScatterWriteEntry.Create(player.Bones[0].HierarchyAddr + 0x90, savedLocalPos));
                        }

                        if (tpWrites.Count != 0)
                            Memory.WriteScatter(tpWrites);
                    }
                    finally
                    {
                        running = false;
                    }
                });
            }
        }
    }
}
