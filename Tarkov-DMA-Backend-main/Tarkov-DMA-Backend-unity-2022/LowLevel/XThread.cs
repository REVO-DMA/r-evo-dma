namespace Tarkov_DMA_Backend.LowLevel
{
    public readonly struct XThread : IDisposable
    {
        private readonly Thread _thread;
        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _ct;

        public readonly bool ShouldTerminate() => _ct.IsCancellationRequested;

        public XThread(ThreadStart start, ApartmentState apartmentState = ApartmentState.MTA)
        {
            _cts = new();
            _ct = _cts.Token;

            _thread = new(start)
            {
                IsBackground = true,
            };
            _thread.SetApartmentState(apartmentState);
            _thread.Start();
        }

        public readonly void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
