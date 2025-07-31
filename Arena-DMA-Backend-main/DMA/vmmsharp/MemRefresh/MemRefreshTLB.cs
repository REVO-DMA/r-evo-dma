using vmmsharp;

namespace arena_dma_backend.DMA.vmmsharp.MemRefresh
{
    public class MemRefreshTLB : MemRefresh
    {
        public MemRefreshTLB(int intervalMS, Vmm HVmm) : base(intervalMS, HVmm) { }

        protected override void OnTimerElapsed(object state)
        {
            HVmm.ConfigSet(Vmm.OPT_REFRESH_FREQ_TLB, 1);
        }
    }
}
