using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit
{
    public class FeaturesController : IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private readonly CancellationToken _ct;
        private readonly Thread _featuresThread;

        private List<Feature> _features = new();

        public FeaturesController()
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
                Logger.WriteLine("Features Controller thread is starting...");

                Stopwatch _stopwatch = new();
                _stopwatch.Start();

                while (true)
                {
                    _ct.ThrowIfCancellationRequested();

                    try
                    {
                        if (EFTDMA.InRaid && CameraManager.CameraIsAvailable)
                        {
                            List<ScatterWriteEntry> writes = new();
                            Player LocalPlayer = EFTDMA.LocalPlayer;

                            if (LocalPlayer == null)
                            {
                                Thread.Sleep(10);
                                continue;
                            }

                            foreach (Feature feature in _features)
                                feature.TryRun(ref writes, LocalPlayer);

                            if (writes.Any())
                                Memory.WriteScatter(writes);

                            Thread.Sleep(10);
                        }
                        else
                            Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[Features Controller] ERROR: {ex}");
                    }
                }
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                Logger.WriteLine($"CRITICAL ERROR on Features Controller thread: {ex}");
                SentrySdk.CaptureException(ex);
            }
            finally
            {
                Logger.WriteLine("Features Controller thread is stopping...");
            }
        }

        public void AddFeature(Feature feature)
        {
            _features.Add(feature);
        }

        public Feature GetFeature(string id)
        {
            foreach (Feature feature in _features)
            {
                if (feature.ID == id) return feature;
            }

            return null;
        }
    }
}
