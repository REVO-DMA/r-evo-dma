///
/// Contains Custom vmmsharp Code
///
namespace vmmsharp
{
    /// <summary>
    /// Encapsulates the results of a Custom VMM Scatter Read.
    /// Must be disposed!
    /// </summary>
    public sealed class SCATTER_HANDLE : IDisposable
    {
        private readonly IntPtr _pppMEMs;
        /// <summary>
        /// Scatter Read Results.
        /// </summary>
        public IReadOnlyDictionary<ulong, SCATTER_PAGE> Scatters { get; }

        public SCATTER_HANDLE(Dictionary<ulong, SCATTER_PAGE> scatters, IntPtr pppMEMs)
        {
            Scatters = scatters;
            _pppMEMs = pppMEMs;
        }

        #region IDisposable
        private bool _disposed = false;
        /// <summary>
        /// Calls LcMemFree on native memory resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            Lci.LcMemFree(_pppMEMs);
            _disposed = true;
        }

        ~SCATTER_HANDLE() => Dispose(false);
        #endregion
    }

    /// <summary>
    /// Defines a Scatter Read Entry (Page).
    /// Validation should be done in calling code that constructs this value.
    /// </summary>
    public readonly struct SCATTER_PAGE
    {
        private readonly IntPtr _pb;

        public SCATTER_PAGE(IntPtr pb)
        {
            _pb = pb;
        }

        /// <summary>
        /// Page for this scatter read entry.
        /// </summary>
        public unsafe ReadOnlySpan<byte> Page =>
            new ReadOnlySpan<byte>(_pb.ToPointer(), 0x1000);
    }

    public sealed partial class Vmm : IDisposable
    {
        /// <summary>
        /// Custom scatter read method.
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <param name="flags">VMM Flags</param>
        /// <param name="qwA">Array of virtual addresses (page aligned) to scatter read. You MUST ensure there are no duplicate pages! (Call .Distinct() on the source).</param>
        /// <returns>Scatter Handle object containing results.</returns>
        public unsafe SCATTER_HANDLE MemReadScatterCustom(uint pid, uint flags, params ulong[] qwA)
        {
            if (!Lci.LcAllocScatter1((uint)qwA.Length, out IntPtr pppMEMs))
                return null;
            var ppMEMs = (LC_MEM_SCATTER_CUSTOM**)pppMEMs.ToPointer();
            for (int i = 0; i < qwA.Length; i++)
            {
                var pMEM = ppMEMs[i];
                pMEM->qwA = qwA[i] & ~(ulong)0xfff;
            }
            var MEMs = new Dictionary<ulong, SCATTER_PAGE>(qwA.Length);
            Vmmi.VMMDLL_MemReadScatter(hVMM, pid, pppMEMs, (uint)qwA.Length, flags);
            for (int i = 0; i < qwA.Length; i++)
            {
                var pMEM = ppMEMs[i];
                if (pMEM->f)
                    MEMs.Add(pMEM->qwA, new(pMEM->pb));
            }
            return new(MEMs, pppMEMs);
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct LC_MEM_SCATTER_CUSTOM
        {
            private readonly uint version;
            public readonly bool f;
            public ulong qwA;
            public readonly IntPtr pb;
        }
    }
}