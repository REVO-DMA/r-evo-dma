using Vmmsharp;

namespace Tarkov_DMA_Backend.MemDMA.vmmsharp.MemRefresh
{
    public class MemRefreshRead : MemRefresh
    {
        public MemRefreshRead(int intervalMS, Vmm hVMM) : base(intervalMS, hVMM) { }

        protected override void OnTimerElapsed(object state)
        {
            hVMM.SetConfig(Vmm.CONFIG_OPT_REFRESH_FREQ_MEM, 1);
        }
    }
}
