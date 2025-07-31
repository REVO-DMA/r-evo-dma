
namespace apex_dma_esp.MemDMA.Collections
{
    /// <summary>
    /// Defines a Custom Memory-Based List <typeparamref name="T"/> Collection.
    /// </summary>
    /// <typeparam name="T">Value Type <typeparamref name="T"/></typeparam>
    public interface IMemCollection<T> : IReadOnlyList<T>, IDisposable
        where T : unmanaged
    {
    }
}
