using cs2_dma_esp.Misc;
using System.Buffers;
///
/// Contains Custom vmmsharp Code
///
namespace vmmsharp
{
    public sealed partial class Vmm : IDisposable
    {
        public IntPtr Handle => this.hVMM;

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                vmmi.VMMDLL_Close(hVMM);
                hVMM = IntPtr.Zero;
                disposed = true;
            }
        }

        public unsafe bool MemReadBuffer<T>(uint pid, ulong qwA, Span<T> buffer, uint cb, uint flags = 0, bool allowPartial = false)
            where T : unmanaged
        {
            uint cbRead;
            fixed (T* pb = buffer)
            {
                if (!vmmi.VMMDLL_MemReadEx(hVMM, pid, qwA, (byte*)pb, cb, out cbRead, flags))
                    return false;
            }
            if (!allowPartial && cbRead != cb)
                return false;
            return true;
        }

        public unsafe T MemReadValue<T>(uint pid, ulong qwA, uint flags = 0) where T : unmanaged
        {
            uint cb = (uint)SizeChecker<T>.Size;
            T value = default;
            var pb = (byte*)Unsafe.AsPointer(ref value);
            if (!vmmi.VMMDLL_MemReadEx(hVMM, pid, qwA, pb, cb, out var cbRead, flags) ||
                cbRead != cb)
                throw new Exception("Memory Read Failed!");
            return value;
        }

        public unsafe bool MemWriteValue<T>(uint pid, ulong qwA, T value) where T : unmanaged
        {
            uint cb = (uint)SizeChecker<T>.Size;
            var pb = (byte*)Unsafe.AsPointer(ref value);
            if (!vmmi.VMMDLL_MemWrite(hVMM, pid, qwA, pb, cb))
                return false;
            return true;
        }

        public unsafe MEM_SCATTER[] MemReadScatter(uint pid, uint flags, params ulong[] qwA)
        {
            int i;
            long vappMEMs, vapMEM;
            IntPtr pMEM, pMEM_qwA, pppMEMs;
            if (!lci.LcAllocScatter1((uint)qwA.Length, out pppMEMs))
            {
                return null;
            }
            vappMEMs = pppMEMs.ToInt64();
            for (i = 0; i < qwA.Length; i++)
            {
                vapMEM = Marshal.ReadIntPtr(new IntPtr(vappMEMs + i * 8)).ToInt64();
                pMEM_qwA = new IntPtr(vapMEM + 8);
                Marshal.WriteInt64(pMEM_qwA, (long)(qwA[i] & ~(ulong)0xfff));
            }
            MEM_SCATTER[] MEMs = new MEM_SCATTER[qwA.Length];
            vmmi.VMMDLL_MemReadScatter(hVMM, pid, pppMEMs, (uint)MEMs.Length, flags);
            var pool = ArrayPool<byte>.Shared;
            for (i = 0; i < MEMs.Length; i++)
            {
                pMEM = Marshal.ReadIntPtr(new IntPtr(vappMEMs + i * 8));
                lci.LC_MEM_SCATTER n = Marshal.PtrToStructure<lci.LC_MEM_SCATTER>(pMEM);
                MEMs[i].f = n.f;
                MEMs[i].qwA = n.qwA;
                MEMs[i].pb = pool.Rent(0x1000);
                Marshal.Copy(n.pb, MEMs[i].pb, 0, 0x1000);
            }
            lci.LcMemFree(pppMEMs);
            return MEMs;
        }

    }
}
