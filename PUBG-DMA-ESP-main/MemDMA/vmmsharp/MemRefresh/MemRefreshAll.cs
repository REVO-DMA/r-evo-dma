using pubg_dma_esp.PUBG;
using vmmsharp;

namespace pubg_dma_esp.MemDMA.vmmsharp.MemRefresh
{
    public class MemRefreshAll : MemRefresh
    {
        public MemRefreshAll(int intervalMS, Vmm HVmm) : base(intervalMS, HVmm) { }

        protected override void OnTimerElapsed(object state)
        {
            if (Memory is null || Manager.IsInMatch) return;

            HVmm.ConfigSet(Vmm.OPT_REFRESH_ALL, 1);
        }
    }
}
