using Tarkov_DMA_Backend.MemDMA.EFT;
using Vmmsharp;

namespace Tarkov_DMA_Backend.MemDMA.vmmsharp.MemRefresh
{
    public class MemRefreshAll : MemRefresh
    {
        public MemRefreshAll(int intervalMS, Vmm hVMM) : base(intervalMS, hVMM) { }

        protected override void OnTimerElapsed(object state)
        {
            if (EFTDMA.InRaid) return;

            RefreshNow();
        }

        public void RefreshNow()
        {
            hVMM.SetConfig(Vmm.CONFIG_OPT_REFRESH_ALL, 1);
        }
    }
}
