namespace Tarkov_DMA_Backend.Unity.LowLevel
{
    public sealed class PatchManager
    {
        private readonly ConcurrentDictionary<string, bool> _patchStatus;

        public PatchManager()
        {
            _patchStatus = new();
        }

        public bool GetStatus(string id)
        {
            if (_patchStatus.TryGetValue(id, out bool result))
                return result;
            else
                return false;
        }

        public void SetStatus(string id, bool value)
        {
            _patchStatus.AddOrUpdate(id, (newItem) => value, (key, existing) => value);
        }

        public void Reset()
        {
            _patchStatus.Clear();
        }
    }
}
