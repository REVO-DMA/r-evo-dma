using cs2_dma_esp.MemDMA.Collections;
using cs2_dma_esp.MemDMA.ScatterAPI;
using cs2_dma_esp.MemDMA.vmmsharp.MemRefresh;
using cs2_dma_esp.Misc;
using System.Buffers;
using vmmsharp;

namespace cs2_dma_esp.MemDMA
{
    public abstract class MemDMA : IDisposable
    {
        #region Fields/Properties/Constructor

        private const FpgaAlgo FPGAAlgorithm = FpgaAlgo.Auto;
        private const string MemoryMapFile = "mmap.txt";
        private readonly MemRefreshAll _r1;
        private readonly MemRefreshRead _r2;
        private readonly MemRefreshTLB _r3;
        /// <summary>
        /// (Base)
        /// Currently Set Process ID (PID).
        /// </summary>
        protected uint PID;
        /// <summary>
        /// (Base)
        /// Currently Set Module Base Virtual Address.
        /// </summary>
        protected ulong ModuleBase;
        /// <summary>
        /// (Base)
        /// client.dll Base Virtual Address.
        /// </summary>
        protected ulong ClientDLL;
        /// <summary>
        /// (Base)
        /// tier0.dll Base Virtual Address.
        /// </summary>
        protected ulong Tier0DLL;
        /// <summary>
        /// (Base)
        /// inputsystem.dll Base Virtual Address.
        /// </summary>
        protected ulong InputSystemDLL;

        /// <summary>
        /// (Base)
        /// MemProcFS Vmm Instance
        /// </summary>
        protected Vmm HVmm = null;

        /// <summary>
        /// (Base)
        /// Constructor.
        /// </summary>
        /// <param name="isDebug">True if debug details should be emitted.</param>
        protected MemDMA(bool isDebug)
        {
            Logger.WriteLine("Loading memory module...");
            string[] initArgs = new string[] { "-device", $"fpga://algo={FPGAAlgorithm}", "-waitinitialize", "-disable-python", "-norefresh" };
            if (isDebug)
            {
                var debugArgs = new string[] { "-printf", "-vv" };
                initArgs = initArgs.Concat(debugArgs).ToArray();
            }

            if (!File.Exists(MemoryMapFile))
            {
                try
                {
                    Logger.WriteLine("[DMA] No MemMap, attempting to generate...");
                    try // Init for Memory Map Generation
                    {
                        HVmm = new Vmm(initArgs);

                        if (!GetMemMap()) throw new DMAException($"Error creating MemMap");
                    }
                    catch (Exception ex)
                    {
                        throw new DMAException($"[DMA] Startup (MemMap init): {ex.Message}", ex);
                    }
                }
                finally
                {
                    HVmm?.Dispose(); // Close back down, re-init w/ map
                    HVmm = null; // Null ref back out

                    // Sleep for a moment before re-init
                    Thread.Sleep(1000);
                }
            }

            try // Final Init
            {
                var mapArgs = new string[] { "-memmap", MemoryMapFile };
                initArgs = initArgs.Concat(mapArgs).ToArray();
                HVmm = new Vmm(initArgs);

                if (HVmm is null) throw new DMAException($"Error initializing VMM (final init instance was null).");

                // Initialize custom cache refresh timing
                _r1 = new MemRefreshAll(3000, HVmm);
                _r2 = new MemRefreshRead(1000, HVmm);
                _r3 = new MemRefreshTLB(500, HVmm);
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] Startup (final init): {ex.Message}", ex);
            }
        }
        #endregion

        #region Mem Startup
        /// <summary>
        /// Generates a Physical Memory Map (mmap.txt) to enhance performance/safety.
        /// https://github.com/ufrisk/LeechCore/wiki/Device_FPGA_AMD_Thunderbolt
        /// </summary>
        private bool GetMemMap()
        {
            try
            {
                if (HVmm is null) return false;

                var map = HVmm.Map_GetPhysMem();
                if (map.Length == 0) 
                    throw new Exception("Map_GetPhysMem() returned no entries!");
                var sb = new StringBuilder();
                sb.AppendFormat("{0,4}", "#")
                    .Append(' ') // Spacer [1]
                    .AppendFormat("{0,16}", "Base")
                    .Append("   ") // Spacer [3]
                    .AppendFormat("{0,16}", "Top")
                    .AppendLine();
                sb.AppendLine("-----------------------------------------");
                for (int i = 0; i < map.Length; i++)
                {
                    sb.AppendFormat("{0,4}", $"{i.ToString("D4")}")
                        .Append(' ') // Spacer [1]
                        .AppendFormat("{0,16}", $"{map[i].pa.ToString("x")}")
                        .Append(" - ") // Spacer [3]
                        .AppendFormat("{0,16}", $"{(map[i].pa + map[i].cb - 1).ToString("x")}")
                        .AppendLine();
                }
                File.WriteAllText(MemoryMapFile, sb.ToString());

                return true;
            }
            catch (Exception ex)
            {
                throw new DMAException("[DMA] MEM MAP ERROR", ex);
            }
        }

        protected virtual uint[] GetPidsForProcess(string process)
        {
            try
            {
                var pids = HVmm.PidList();
                List<uint> PIDs = new();
                foreach (var pid in pids)
                {
                    var procInfo = HVmm.ProcessGetInformation(pid);
                    if (HVmm.ProcessGetInformation(pid).szNameLong.Contains(process, StringComparison.OrdinalIgnoreCase))
                        PIDs.Add(pid);
                }

                return PIDs.ToArray();
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DMA] Unable to get PIDs for process \"{process}\": {ex}");
            }

            return Array.Empty<uint>();
        }

        /// <summary>
        /// (Base)
        /// Obtain the Base Address of a Process Module.
        /// </summary>
        /// <param name="module">Module Name (including file extension, ex: .dll)</param>
        protected virtual ulong GetModuleBase(string module)
        {
            try
            {
                ulong moduleBase = HVmm.ProcessGetModuleBase(PID, module);
                if (moduleBase == 0x0)
                    throw new DMAException("Module Lookup Failed");

                return moduleBase;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DMA] Unable to get Module Base for {module}: {ex}");

                return 0x0;
            }
        }

        public virtual ulong GetModuleExport(string module, string function)
        {
            try
            {
                var exports = HVmm.Map_GetEAT(PID, module, out var info);

                foreach (var export in exports)
                    if (string.Equals(function, export.wszFunction, StringComparison.OrdinalIgnoreCase))
                        return export.vaFunction;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DMA] Unable to get Module Base for {module}: {ex}");
                return 0x0;
            }

            return 0x0;
        }
        #endregion

        #region ScatterRead
        /// <summary>
        /// (Base)
        /// Performs multiple reads in one sequence, significantly faster than single reads.
        /// Designed to run without throwing unhandled exceptions, which will ensure the maximum amount of
        /// reads are completed OK even if a couple fail.
        /// </summary>
        public virtual void ReadScatter(IReadOnlyList<IScatterEntry> entries, bool useCache = true)
        {
            if (!entries.Any())
                return;
            var pagesToRead = new HashSet<ulong>(); // Will contain each unique page only once to prevent reading the same page multiple times
            int maxSize = 0;
            foreach (var entry in entries) // First loop through all entries - GET INFO
            {
                // Parse Address and Size properties
                ulong addr = entry.ParseAddr();
                uint size = (uint)entry.ParseSize();

                // INTEGRITY CHECK - Make sure the read is valid and within range
                if (addr == 0x0 || size == 0 || size > (PAGE_SIZE * 5))
                {
                    entry.IsFailed = true;
                    continue;
                }
                if (size > maxSize)
                    maxSize = (int)size;
                // location of object
                ulong readAddress = addr + entry.Offset;
                // get the number of pages
                uint numPages = ADDRESS_AND_SIZE_TO_SPAN_PAGES(readAddress, size);
                ulong basePage = PAGE_ALIGN(readAddress);

                //loop all the pages we would need
                for (int p = 0; p < numPages; p++)
                {
                    ulong page = basePage + PAGE_SIZE * (uint)p;
                    pagesToRead.Add(page);
                }
            }
            uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
            var scatters = HVmm.MemReadScatter(PID, flags, pagesToRead.ToArray())
                ?? throw new NullReferenceException("scatters"); // execute scatter read
            try
            {
                using var hBuf = new SharedMemory<byte>(maxSize);
                var buffer = hBuf.Span;
                foreach (var entry in entries) // Second loop through all entries - PARSE RESULTS
                {
                    if (entry.IsFailed) // Skip this entry, leaves result as null
                        continue;

                    ulong readAddress = (ulong)entry.Addr + entry.Offset; // location of object
                    uint pageOffset = BYTE_OFFSET(readAddress); // Get object offset from the page start address

                    uint size = (uint)(int)entry.Size;
                    int bytesCopied = 0; // track number of bytes copied to ensure nothing is missed
                    uint cb = Math.Min(size, (uint)PAGE_SIZE - pageOffset); // bytes to read this page

                    uint numPages = ADDRESS_AND_SIZE_TO_SPAN_PAGES(readAddress, size); // number of pages to read from (in case result spans multiple pages)
                    ulong basePage = PAGE_ALIGN(readAddress);

                    for (int p = 0; p < numPages; p++)
                    {
                        ulong page = basePage + PAGE_SIZE * (uint)p; // get current page addr
                        var scatter = scatters.FirstOrDefault(x => x.qwA == page); // retrieve page of mem needed
                        if (scatter.f) // read succeeded -> copy to buffer
                        {
                            scatter.pb
                                .AsSpan((int)pageOffset, (int)cb)
                                .CopyTo(buffer.Slice(bytesCopied, (int)cb)); // Copy bytes to buffer
                            bytesCopied += (int)cb;
                        }
                        else // read failed -> set failed flag
                        {
                            entry.IsFailed = true;
                            break;
                        }

                        cb = (uint)PAGE_SIZE; // set bytes to read next page
                        if (bytesCopied + cb > size) // partial chunk last page
                            cb = size - (uint)bytesCopied;

                        pageOffset = 0x0; // Next page (if any) should start at 0x0
                    }
                    if (bytesCopied != size)
                        entry.IsFailed = true;
                    entry.SetResult(buffer.Slice(0, bytesCopied));
                }
            }
            finally
            {
                var pool = ArrayPool<byte>.Shared;
                foreach (var s in scatters)
                    pool.Return(s.pb);
            }
        }
        #endregion

        #region ReadMethods
        /// <summary>
        /// (Base)
        /// Read memory into a Buffer of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Value Type <typeparamref name="T"/></typeparam>
        /// <param name="addr">Virtual Address to read from.</param>
        /// <param name="buffer">Buffer to receive memory read in.</param>
        /// <param name="useCache">Use caching for this read.</param>
        public virtual void ReadBuffer<T>(ulong addr, Span<T> buffer, bool useCache = true) where T : unmanaged
        {
            try
            {
                int size = SizeChecker<T>.Size * buffer.Length;
                if ((uint)size > PAGE_SIZE * 1500)
                    throw new Exception("Read length outside expected bounds!");
                uint flags = useCache ?
                    0 : Vmm.FLAG_NOCACHE;
                if (!HVmm.MemReadBuffer(PID, addr, buffer, (uint)size, flags))
                    throw new Exception("Memory Read Failed!");
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading Collection<{typeof(T)}> at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// (Base)
        /// Read a chain of pointers and get the final result.
        /// </summary>
        public virtual ulong ReadPtrChain(ulong addr, uint[] offsets, bool useCache = true)
        {
            ulong ptr = addr; // push ptr to first address value
            for (int i = 0; i < offsets.Length; i++)
            {
                try
                {
                    ptr = ReadPtr(ptr + offsets[i], useCache);
                }
                catch (Exception ex)
                {
                    throw new DMAException($"[DMA] ERROR reading pointer chain at index {i}, addr 0x{ptr.ToString("X")} + 0x{offsets[i].ToString("X")}", ex);
                }
            }
            return ptr;
        }
        /// <summary>
        /// (Base)
        /// Resolves a pointer and returns the memory address it points to.
        /// </summary>
        public virtual ulong ReadPtr(ulong addr, bool useCache = true)
        {
            try
            {
                var ptr = ReadValue<MemPointer>(addr, useCache);
                ptr.Validate();
                return ptr;
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading pointer at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// (Base)
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to read from.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual T ReadValue<T>(ulong addr, bool useCache = true) where T : unmanaged
        {
            try
            {
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                return HVmm.MemReadValue<T>(PID, addr, flags);
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading {typeof(T)} value at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// (Base)
        /// Write value type/struct to specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to write to.</param>
        /// <param name="value">Value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual bool WriteValue<T>(ulong addr, T value) where T : unmanaged
        {
            if (!HVmm.MemWriteValue(PID, addr, value))
                return false;

            return true;
        }

        /// <summary>
        /// (Base)
        /// Read null terminated string (utf-8/default).
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <exception cref="DMAException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual string ReadUtf8String(ulong addr, int length, bool useCache = true) // read n bytes (string)
        {
            try
            {
                if ((uint)length > PAGE_SIZE)
                    throw new Exception("String length outside expected bounds!");
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                using var hBuf = new SharedMemory<byte>(length);
                var buffer = hBuf.Span;
                if (!HVmm.MemReadBuffer(PID, addr, buffer, (uint)length, flags, true))
                    throw new Exception("Memory Read Failed!");
                var nullIndex = buffer.IndexOf((byte)0);
                return nullIndex >= 0 ?
                    Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading string at 0x{addr.ToString("X")}", ex);
            }
        }
        #endregion

        #region IDisposable
        private readonly object _disposeSync = new();
        private bool _disposed = false;
        public void Dispose() => Dispose(true); // Public Dispose Pattern

        protected virtual void Dispose(bool disposing)
        {
            lock (_disposeSync)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        _r1.Dispose();
                        _r2.Dispose();
                        _r3.Dispose();
                        HVmm.Dispose();
                    }
                    _disposed = true;
                }
            }
        }
        #endregion

        #region Memory Macros
        /// Mem Align Functions Ported from Win32 (C Macros)
        protected const ulong PAGE_SIZE = 0x1000;
        protected const int PAGE_SHIFT = 12;

        /// <summary>
        /// The PAGE_ALIGN macro takes a virtual address and returns a page-aligned
        /// virtual address for that page.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static ulong PAGE_ALIGN(ulong va)
        {
            return (va & ~(PAGE_SIZE - 1));
        }
        /// <summary>
        /// The ADDRESS_AND_SIZE_TO_SPAN_PAGES macro takes a virtual address and size and returns the number of pages spanned by the size.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static uint ADDRESS_AND_SIZE_TO_SPAN_PAGES(ulong va, uint size)
        {
            return (uint)((BYTE_OFFSET(va) + (size) + (PAGE_SIZE - 1)) >> PAGE_SHIFT);
        }

        /// <summary>
        /// The BYTE_OFFSET macro takes a virtual address and returns the byte offset
        /// of that address within the page.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static uint BYTE_OFFSET(ulong va)
        {
            return (uint)(va & (PAGE_SIZE - 1));
        }
        #endregion
    }
}

    #region Exceptions
    public sealed class DMAException : Exception
    {
        public DMAException() { }

        public DMAException(string message) : base(message) { }

        public DMAException(string message, Exception inner) : base(message, inner) { }
    }

    public sealed class NullPtrException : Exception
    {
        public NullPtrException() { }

        public NullPtrException(string message) : base(message) { }

        public NullPtrException(string message, Exception inner) : base(message, inner) { }
    }
    #endregion
