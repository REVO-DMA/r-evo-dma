using Vmmsharp;

namespace Tarkov_DMA_Backend.MemDMA.vmmsharp.MemRefresh
{
    public class MemRefresh : IDisposable
    {
        private readonly Timer _timer;
        protected Vmm hVMM;

        public MemRefresh(int intervalMS, Vmm HVMM)
        {
            _timer = new(new TimerCallback(OnTimerElapsed), null, Timeout.Infinite, Timeout.Infinite);
            hVMM = HVMM;

            _timer.Change(0, intervalMS);
        }

        protected virtual void OnTimerElapsed(object state)
        {
            throw new NotImplementedException();
        }

        private bool _disposed = false;
        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _timer.Dispose();
            _disposed = true;
        }
    }
}
