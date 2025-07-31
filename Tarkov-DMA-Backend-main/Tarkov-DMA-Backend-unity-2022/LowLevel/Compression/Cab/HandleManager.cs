namespace Tarkov_DMA_Backend.LowLevel.Compression.Cab
{
    internal sealed class HandleManager<T> where T : class
	{
        private readonly List<T> Handles;

        public HandleManager()
		{
			Handles = new();
		}

		public T this[int handle]
		{
			get
			{
				if (handle > 0 && handle <= Handles.Count)
                    return Handles[handle - 1];
                
				return default;
			}
		}

		public int AllocHandle(T obj)
		{
			Handles.Add(obj);
			return Handles.Count;
		}

		public void FreeHandle(int handle)
		{
			if (handle > 0 && handle <= Handles.Count)
                Handles[handle - 1] = default;
        }
	}
}
