using vmmsharp;

namespace cs2_dma_esp.MemDMA.vmmsharp.MemRefresh
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
