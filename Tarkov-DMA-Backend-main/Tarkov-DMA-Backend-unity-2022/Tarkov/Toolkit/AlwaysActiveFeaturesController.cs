using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit
{
    public class AlwaysActiveFeaturesController : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _ct;
        private readonly Thread _featuresThread;

        private List<AlwaysActiveFeature> _features = new();

        public AlwaysActiveFeaturesController()
        {
            // Init thread
            _cts = new();
            _ct = _cts.Token;

            _featuresThread = new Thread(FeaturesWorker)
            {
                IsBackground = true
            };

            _featuresThread.Start();
        }

        public void Dispose()
        {
            // Stop threads
            _cts.Cancel();
            _cts.Dispose();
        }

        private void FeaturesWorker()
        {
            try
            {
                Logger.WriteLine("Always Active Features Controller thread is starting...");

                while (true)
                {
                    _ct.ThrowIfCancellationRequested();

                    try
                    {
                        List<ScatterWriteEntry> writes = [];

                        foreach (AlwaysActiveFeature feature in _features)
                            feature.TryRun(ref writes);

                        if (writes.Count > 0)
                            Memory.WriteScatter(writes);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[Always Active Features Controller] ERROR: {ex}");
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Always Active Features Controller thread: {ex}");
            }
            finally
            {
                Logger.WriteLine("Always Active Features Controller thread is stopping...");
            }
        }

        public void AddFeature(AlwaysActiveFeature feature)
        {
            _features.Add(feature);
        }

        public AlwaysActiveFeature GetFeature(string id)
        {
            foreach (AlwaysActiveFeature feature in _features)
            {
                if (feature.ID == id) return feature;
            }

            return null;
        }
    }
}
