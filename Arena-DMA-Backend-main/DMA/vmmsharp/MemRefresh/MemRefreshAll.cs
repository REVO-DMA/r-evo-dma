using arena_dma_backend.Arena;
using vmmsharp;

namespace arena_dma_backend.DMA.vmmsharp.MemRefresh
{
    public class MemRefreshAll : MemRefresh
    {
        public MemRefreshAll(int intervalMS, Vmm HVmm) : base(intervalMS, HVmm) { }

        protected override void OnTimerElapsed(object state)
        {
            if (Game.InMatch) return;

            RefreshNow();
        }

        public void RefreshNow()
        {
            HVmm.ConfigSet(Vmm.OPT_REFRESH_ALL, 1);
        }
    }
}
