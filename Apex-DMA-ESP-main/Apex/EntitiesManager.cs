using apex_dma_esp.MemDMA.ScatterAPI;
using apex_dma_esp.MemDMA;

namespace apex_dma_esp.Apex
{
    public static class EntitiesManager
    {
        private const uint FiringRangeLoopCount = 15000;
        private const uint MatchLoopCount = 70;

        public static IReadOnlyList<ulong> Get()
        {
            uint usedCount;
            if (Level.IsFiringRange)
                usedCount = FiringRangeLoopCount;
            else
                usedCount = MatchLoopCount;

            ScatterReadMap scatterMap = new((int)usedCount);
            var scatterRound = scatterMap.AddRound();

            // Populate scatter read
            for (uint i = 0; i < usedCount; i++)
                scatterRound.AddEntry<MemPointer>((int)i, 0, Memory.ModuleBase + Offsets.ENTITY_LIST + ((i + 1) << 5));

            scatterMap.Execute();

            List<ulong> entities = new();

            for (uint i = 0; i < usedCount; i++)
            {
                if (!scatterMap.Results[(int)i][0].TryGetResult<MemPointer>(out var entity) || !entity.ValidateEx())
                    continue;

                entities.Add((ulong)entity);
            }

            Logger.WriteLine($"[ENT MANAGER] Got {entities.Count} entities!");

            return entities;
        }
    }
}
