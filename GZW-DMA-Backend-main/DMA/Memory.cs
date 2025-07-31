using gzw_dma_backend.DMA.Collections;
using gzw_dma_backend.DMA.ScatterAPI;
using gzw_dma_backend.DMA.vmmsharp.MemRefresh;
using gzw_dma_backend.Misc;
using System.Buffers;
using vmmsharp;
using TerraFX.Interop.Windows;
using gzw_dma_backend.GZW;
using gzw_dma_backend.GZW.ESP;

namespace gzw_dma_backend.DMA
{
    public static class Memory
    {

        #region Fields/Properties/Constructor

        private const string MemoryMapFile = "mmap.txt";
        private static Vmm _hVMM;

        public static MemRefreshAll MemRefreshAll;
        public static MemRefreshRead MemRefreshRead;
        public static MemRefreshTLB MemRefreshTLB;
        
        private static readonly Thread _tPrimary;

        /// <summary>
        /// Whether or not the game process is running.
        /// </summary>
        public static bool GameRunning {  get; private set; }
        /// <summary>
        /// Game process ID.
        /// </summary>
        public static uint PID { get; private set; }
        /// <summary>
        /// UnityPlayer.dll base virtual address.
        /// </summary>
        public static ulong ModuleBase { get; private set; }

        static Memory()
        {
            _tPrimary = new Thread(MemoryPrimaryWorker)
            {
                IsBackground = true
            };
        }

        public static void Initialize()
        {
            Logger.WriteLine("Loading memory module...");

            string[] initArgs = new string[] { "-device", "fpga", "-waitinitialize", "-disable-python", "-disable-symbolserver", "-disable-symbols", "-disable-infodb", "-norefresh" };

            if (!File.Exists(MemoryMapFile))
            {
                try
                {
                    Logger.WriteLine("[DMA] No MemMap, attempting to generate...");
                    try // Init for Memory Map Generation
                    {
                        _hVMM = new Vmm(initArgs);

                        if (!GetMemMap()) throw new DMAException($"Error creating MemMap");
                    }
                    catch (Exception ex)
                    {
                        throw new DMAException($"[DMA] Startup (MemMap init): {ex.Message}", ex);
                    }
                }
                finally
                {
                    _hVMM?.Dispose(); // Close back down, re-init w/ map
                    _hVMM = null; // Null ref back out

                    // Sleep for a moment before re-init
                    Thread.Sleep(1000);
                }
            }

            try // Final Init
            {
                var mapArgs = new string[] { "-memmap", MemoryMapFile };
                initArgs = initArgs.Concat(mapArgs).ToArray();
                _hVMM = new Vmm(initArgs);

                if (_hVMM is null)
                    throw new DMAException($"Error initializing VMM (final init instance was null).");

                // Initialize custom cache refresh timing
                MemRefreshAll = new MemRefreshAll(3000, _hVMM);
                MemRefreshRead = new MemRefreshRead(1000, _hVMM);
                MemRefreshTLB = new MemRefreshTLB(500, _hVMM);

                // Start threads
                _tPrimary.Start();
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] Startup (final init): {ex.Message}", ex);
            }
        }

        #endregion

        #region Primary Memory Thread

        /// <summary>
        /// Main worker thread to perform DMA Reads on.
        /// </summary>
        private static void MemoryPrimaryWorker()
        {
            try
            {
                ESP_Manager.Start();

                Logger.WriteLine("Memory thread starting...");

                while (true)
                {
                    Thread.Sleep(1000);

                    MemRefreshAll.RefreshNow();

                    Logger.WriteLine("Searching for the GZW Process...");

                    int startupTries = 0;

                    while (true)
                    {
                        if (startupTries >= 3)
                        {
                            Console.WriteLine("Unable to load cheat, please restart your game.");
                            Thread.Sleep(5000);
                            startupTries = 0;
                        }

                        Thread.Sleep(1000);

                        MemRefreshAll.RefreshNow();

                        uint[] pids = GetPidsForProcess("GZWClientSteam-Win64-Shipping.exe");
                        bool validPidFound = false;
                        foreach (uint pid in pids)
                        {
                            PID = pid;

                            ModuleBase = GetModuleBase("GZWClientSteam-Win64-Shipping.exe");

                            if (ModuleBase != 0x0)
                            {
                                validPidFound = true;
                                break;
                            }
                        }

                        if (validPidFound)
                        {
                            GameRunning = true;

                            Console.WriteLine($"GZW Startup [OK]");
                            Logger.WriteLine($"PID: {PID} \n GZWClientSteam-Win64-Shipping.exe: 0x{ModuleBase:X}");

                            break;
                        }
                        else
                        {
                            GameRunning = false;

                            Console.WriteLine("GZW Startup [FAIL]");
                            Thread.Sleep(3000);
                        }
                    }

                    Console.WriteLine($"Cheat ready!");

                    while (true)
                    {
                        try
                        {
                            Manager.UpdateInfo();
                            Manager.Update();
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteLine($"Error in main loop: {ex}");
                        }

                        Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"FATAL ERROR on Memory Thread: {ex}"); // Log fatal error
                throw; // State is corrupt, program will need to restart
            }
        }

        #endregion

        #region Mem Startup

        /// <summary>
        /// Generates a Physical Memory Map (mmap.txt) to enhance performance/safety.
        /// https://github.com/ufrisk/LeechCore/wiki/Device_FPGA_AMD_Thunderbolt
        /// </summary>
        private static bool GetMemMap()
        {
            try
            {
                if (_hVMM is null) return false;

                var map = _hVMM.Map_GetPhysMem();
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

        public static uint[] GetPidsForProcess(string process)
        {
            try
            {
                var pids = _hVMM.PidList();
                List<uint> PIDs = new();
                foreach (var pid in pids)
                {
                    var procInfo = _hVMM.ProcessGetInformation(pid);
                    if (procInfo.szNameLong.Contains(process, StringComparison.OrdinalIgnoreCase))
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

        public static uint GetPidForProcess(string process)
        {
            uint[] results = GetPidsForProcess(process);
            if (results.Length == 0)
                return uint.MaxValue;

            return results[0];
        }

        /// <summary>
        /// Obtain the Base Address of a Process Module from a specific PID.
        /// </summary>
        /// <param name="module">Module Name (including file extension, ex: .dll)</param>
        public static ulong GetModuleBase(uint pid, string module)
        {
            try
            {
                ulong moduleBase = _hVMM.ProcessGetModuleBase(pid, module);
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

        /// <summary>
        /// Obtain the Base Address of a Process Module.
        /// </summary>
        /// <param name="module">Module Name (including file extension, ex: .dll)</param>
        public static ulong GetModuleBase(string module)
        {
            return GetModuleBase(PID, module);
        }

        /// <summary>
        /// Obtain a specific export from a given module.
        /// </summary>
        public static ulong GetModuleExport(string module, string function)
        {
            try
            {
                var exports = _hVMM.Map_GetEAT(PID, module, out var info);

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
        /// Performs multiple reads in one sequence, significantly faster than single reads.
        /// Designed to run without throwing unhandled exceptions, which will ensure the maximum amount of
        /// reads are completed OK even if a couple fail.
        /// </summary>
        public static void ReadScatter(IReadOnlyList<IScatterEntry> entries, bool useCache = true)
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
                uint offset = entry.ParseOffset();

                // INTEGRITY CHECK - Make sure the read is valid and within range
                if (addr == 0x0 || size == 0 || size > (PAGE_SIZE * 100))
                {
                    entry.IsFailed = true;
                    continue;
                }
                if (size > maxSize)
                    maxSize = (int)size;
                // location of object
                ulong readAddress = addr + offset;
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
            using var hScatter = _hVMM.MemReadScatterCustom(PID, flags, pagesToRead.ToArray())
                ?? throw new NullReferenceException("hScatter"); // execute scatter read
            using var hBuf = new SharedMemory<byte>(maxSize);
            var buffer = hBuf.Span;
            foreach (var entry in entries) // Second loop through all entries - PARSE RESULTS
            {
                if (entry.IsFailed) // Skip this entry, leaves result as null
                    continue;

                ulong readAddress = (ulong)entry.Addr + (uint)entry.Offset; // location of object
                uint pageOffset = BYTE_OFFSET(readAddress); // Get object offset from the page start address

                uint size = (uint)(int)entry.Size;
                int bytesCopied = 0; // track number of bytes copied to ensure nothing is missed
                uint cb = Math.Min(size, (uint)PAGE_SIZE - pageOffset); // bytes to read this page

                uint numPages = ADDRESS_AND_SIZE_TO_SPAN_PAGES(readAddress, size); // number of pages to read from (in case result spans multiple pages)
                ulong basePageAddr = PAGE_ALIGN(readAddress);

                for (int p = 0; p < numPages; p++)
                {
                    ulong pageAddr = basePageAddr + PAGE_SIZE * (uint)p; // get current page addr
                    if (hScatter.Scatters.TryGetValue(pageAddr, out var scatter)) // retrieve page of mem needed
                    {
                        scatter.Page
                            .Slice((int)pageOffset, (int)cb)
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

        #endregion

        #region ReadMethods

        /// <summary>
        /// Read memory into a Buffer of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Value Type <typeparamref name="T"/></typeparam>
        /// <param name="addr">Virtual Address to read from.</param>
        /// <param name="buffer">Buffer to receive memory read in.</param>
        /// <param name="useCache">Use caching for this read.</param>
        public static unsafe void ReadBuffer<T>(uint pid, ulong addr, Span<T> buffer, bool useCache = true, bool allowPartialRead = false) where T : unmanaged
        {
            try
            {
                uint cb = (uint)(SizeChecker<T>.Size * buffer.Length);
                if (cb > PAGE_SIZE * 1500)
                    throw new Exception("Read length outside expected bounds!");
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                uint cbRead;
                fixed (T* pb = buffer)
                {
                    cbRead = _hVMM.MemRead(pid, addr, cb, pb, flags);
                }
                if (cbRead == 0)
                    throw new Exception("Memory Read Failed!");
                if (!allowPartialRead && cbRead != cb)
                    throw new Exception("Partial Memory Read!");
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading Collection<{typeof(T)}> at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// Read memory into a Buffer of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Value Type <typeparamref name="T"/></typeparam>
        /// <param name="addr">Virtual Address to read from.</param>
        /// <param name="buffer">Buffer to receive memory read in.</param>
        /// <param name="useCache">Use caching for this read.</param>
        public static void ReadBuffer<T>(ulong addr, Span<T> buffer, bool useCache = true, bool allowPartialRead = false) where T : unmanaged
        {
            ReadBuffer(PID, addr, buffer, useCache, allowPartialRead);
        }

        /// <summary>
        /// Read memory into a Buffer of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">Value Type <typeparamref name="T"/></typeparam>
        /// <param name="addr">Virtual Address to read from.</param>
        /// <param name="buffer">Buffer to receive memory read in.</param>
        /// <param name="useCache">Use caching for this read.</param>
        public static unsafe void ReadBufferUnsafe<T>(ulong addr, Span<T> buffer, bool useCache = true, bool allowPartialRead = false) where T : unmanaged
        {
            uint cb = (uint)(SizeChecker<T>.Size * buffer.Length);
            uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
            uint cbRead;
            fixed (T* pb = buffer)
            {
                cbRead = _hVMM.MemRead(PID, addr, cb, pb, flags);
            }
        }

        /// <summary>
        /// Read memory into a buffer and validate the right bytes were received.
        /// </summary>
        public static byte[] ReadBufferEnsure(ulong addr, int size)
        {
            const int ValidationCount = 3;

            try
            {
                if ((uint)size > PAGE_SIZE * 1500)
                    throw new DMAException("Buffer length outside expected bounds!");

                byte[][] buffers = new byte[ValidationCount][];
                for (int i = 0; i < ValidationCount; i++)
                {
                    buffers[i] = _hVMM.MemRead(PID, addr, (uint)size, Vmm.FLAG_NOCACHE);

                    if (buffers[i].Length != size)
                        throw new DMAException("Incomplete memory read!");
                }

                // Check that all arrays have the same contents
                for (int i = 1; i < ValidationCount; i++) // Start checking with second item in the array
                    if (!buffers[i].SequenceEqual(buffers[0])) // Compare against the first item in the array
                    {
                        Logger.WriteLine($"[WARN] ReadBufferEnsure() -> 0x{addr:X} did not pass validation!");
                        return null;
                    }

                return buffers[0];
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading buffer at 0x{addr:X}", ex);
            }
        }

        /// <summary>
        /// Read a chain of pointers and get the final result.
        /// </summary>
        public static ulong ReadPtrChain(ulong addr, uint[] offsets, bool useCache = true)
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
        /// Read a chain of pointers and get the final result.
        /// </summary>
        public static ulong ReadPtrChain(uint pid, ulong addr, uint[] offsets, bool useCache = true)
        {
            ulong ptr = addr;

            for (int i = 0; i < offsets.Length; i++)
            {
                try
                {
                    ptr = ReadPtr(pid, ptr + offsets[i], useCache);
                }
                catch (Exception ex)
                {
                    throw new DMAException($"[DMA] ERROR reading pointer chain at index {i}, addr 0x{ptr.ToString("X")} + 0x{offsets[i].ToString("X")}", ex);
                }
            }

            return ptr;
        }

        /// <summary>
        /// Read a chain of pointers and get the final result without throwing.
        /// </summary>
        public static ulong ReadPtrChainUnsafe(ulong addr, uint[] offsets, bool useCache = true)
        {
            return ReadPtrChainUnsafe(PID, addr, offsets, useCache);
        }

        /// <summary>
        /// Read a chain of pointers and get the final result without throwing.
        /// </summary>
        public static ulong ReadPtrChainUnsafe(uint pid, ulong addr, uint[] offsets, bool useCache = true)
        {
            ulong ptr = addr;

            for (int i = 0; i < offsets.Length; i++)
                ptr = ReadPtrUnsafe(pid, ptr + offsets[i], useCache);

            return ptr;
        }

        /// <summary>
        /// Resolves a pointer and returns the memory address it points to.
        /// </summary>
        public static ulong ReadPtr(ulong addr, bool useCache = true)
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
        /// Resolves a pointer and returns the memory address it points to.
        /// </summary>
        public static ulong ReadPtr(uint pid, ulong addr, bool useCache = true)
        {
            try
            {
                var ptr = ReadValue<MemPointer>(pid, addr, useCache);
                ptr.Validate();
                return ptr;
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading pointer at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// Resolves a pointer and returns the memory address it points to without throwing.
        /// </summary>
        public static ulong ReadPtrUnsafe(ulong addr, bool useCache = true)
        {
            try
            {
                return ReadValue<ulong>(addr, useCache);
            }
            catch { return 0x0; }
        }

        /// <summary>
        /// Resolves a pointer and returns the memory address it points to without throwing.
        /// </summary>
        public static ulong ReadPtrUnsafe(uint pid, ulong addr, bool useCache = true)
        {
            try
            {
                return ReadValue<ulong>(pid, addr, useCache);
            }
            catch { return 0x0; }
        }

        /// <summary>
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to read from.</param>
        public static T ReadValue<T>(uint pid, ulong addr, bool useCache = true) where T : unmanaged
        {
            try
            {
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                if (!_hVMM.MemReadStruct<T>(pid, addr, out var result, flags))
                    throw new Exception("Memory Read Failed!");
                return result;
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading {typeof(T)} value at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to read from.</param>
        public static T ReadValueUnsafe<T>(ulong addr, bool useCache = true) where T : unmanaged
        {
            try
            {
                return ReadValue<T>(PID, addr, useCache);
            }
            catch { return default; }
        }

        /// <summary>
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to read from.</param>
        public static T ReadValue<T>(ulong addr, bool useCache = true) where T : unmanaged
        {
            return ReadValue<T>(PID, addr, useCache);
        }

        /// <summary>
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to read from.</param>
        public static T? ReadValueEnsure<T>(ulong addr) where T : unmanaged
        {
            const int ValidationCount = 3;

            try
            {
                T[] buffers = new T[ValidationCount];
                for (int i = 0; i < ValidationCount; i++)
                    buffers[i] = ReadValue<T>(PID, addr, false);

                // Check that all values are the same
                for (int i = 1; i < ValidationCount; i++) // Start checking with second item in the array
                    if (!buffers[i].Equals(buffers[0])) // Compare against the first item in the array
                    {
                        Logger.WriteLine($"[WARN] ReadValueEnsure() -> 0x{addr:X} did not pass validation!");
                        return null;
                    }

                return buffers[0];
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading value at 0x{addr:X}", ex);
            }
        }

        /// <summary>
        /// Read null terminated string (utf-8/default).
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <exception cref="DMAException"></exception>
        public static string ReadUtf8String(ulong addr, int length, bool useCache = true)
        {
            try
            {
                if ((uint)length > PAGE_SIZE)
                    throw new Exception("String length outside expected bounds!");
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                using var hBuf = new SharedMemory<byte>(length);
                var buffer = hBuf.Span;
                ReadBuffer(addr, buffer, useCache, true);
                var nullIndex = buffer.IndexOf((byte)0);
                return nullIndex >= 0 ?
                    Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading string at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// Read null terminated string (utf-8/default) without throwing.
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <exception cref="DMAException"></exception>
        public static string ReadUtf8StringUnsafe(ulong addr, int length, bool useCache = true)
        {
            try
            {
                return ReadUtf8String(addr, length, useCache);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// Read null terminated string (utf-8/default).
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <exception cref="Exception"></exception>
        public static string ReadString(ulong addr, int length, bool useCache = true) // read n bytes (string)
        {
            try
            {
                if ((uint)length > PAGE_SIZE)
                    throw new Exception("String length outside expected bounds!");
                using var hBuf = new SharedMemory<byte>(length);
                var buffer = hBuf.Span;
                ReadBuffer(addr, buffer, useCache);
                var nullIndex = buffer.IndexOf((byte)0);
                return nullIndex >= 0 ? Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception($"[DMA] ERROR reading string at 0x{addr.ToString("X")}", ex);
            }
        }

        public static Vmm.MAP_EATENTRY[] GetEAT(uint pid, string module, out Vmm.MAP_EATINFO eatInfo)
        {
            return _hVMM.Map_GetEAT(pid, module, out eatInfo);
        }

        public static Vmm.MAP_EATENTRY[] GetEAT(string module, out Vmm.MAP_EATINFO eatInfo)
        {
            return GetEAT(PID, module, out eatInfo);
        }

        #endregion

        #region Windows Registry

        public static class Registry
        {
            public enum ERegistryType : uint
            {
                none = REG.REG_NONE,
                sz = REG.REG_SZ,
                expand_sz = REG.REG_EXPAND_SZ,
                binary = REG.REG_BINARY,
                dword = REG.REG_DWORD,
                dword_little_endian = REG.REG_DWORD_LITTLE_ENDIAN,
                dword_big_endian = REG.REG_DWORD_BIG_ENDIAN,
                link = REG.REG_LINK,
                multi_sz = REG.REG_MULTI_SZ,
                resource_list = REG.REG_RESOURCE_LIST,
                full_resource_descriptor = REG.REG_FULL_RESOURCE_DESCRIPTOR,
                resource_requirements_list = REG.REG_RESOURCE_REQUIREMENTS_LIST,
                qword = REG.REG_QWORD,
                qword_little_endian = REG.REG_QWORD_LITTLE_ENDIAN
            }

            public static string QueryRegistryValue(string path, ERegistryType type)
            {
                var result = _hVMM.RegValueRead(path, out uint resultType);
                if (result == null)
                    throw new Exception("Unable to get value from registry!");

                if ((ERegistryType)resultType != type)
                    throw new Exception("Registry Types do not match!");

                return Encoding.Unicode.GetString(result);
            }
        }

        #endregion

        #region Write Methods

        /// <summary>
        /// Write a buffer to the specified address inside of the specified PID.
        /// </summary>
        /// <param name="addr">Address to write to.</param>
        /// <param name="buffer">Buffer to write.</param>
        public static bool WriteBuffer(uint pid, ulong addr, byte[] buffer)
        {
            if (!_hVMM.MemWrite(pid, addr, buffer))
                return false;

            return true;
        }

        /// <summary>
        /// Write a buffer to the specified address.
        /// </summary>
        /// <param name="addr">Address to write to.</param>
        /// <param name="buffer">Buffer to write.</param>
        public static bool WriteBuffer(ulong addr, byte[] buffer)
        {
            return WriteBuffer(PID, addr, buffer);
        }

        /// <summary>
        /// Write a buffer to the specified address inside of the specified PID.
        /// </summary>
        /// <param name="addr">Address to write to.</param>
        /// <param name="buffer">Buffer to write.</param>
        public static unsafe bool WriteBuffer<T>(uint pid, ulong addr, Span<T> buffer) where T : unmanaged
        {
            try
            {
                uint cb = (uint)(SizeChecker<T>.Size * buffer.Length);
                if (cb > PAGE_SIZE * 1500)
                    throw new Exception("Write length outside expected bounds!");

                bool success;
                fixed (T* pb = buffer)
                {
                    success = _hVMM.MemWrite(PID, addr, cb, pb);
                }

                return success;
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR writing Collection<{typeof(T)}> at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// Write a buffer to the specified address.
        /// </summary>
        /// <param name="addr">Address to write to.</param>
        /// <param name="buffer">Buffer to write.</param>
        public static bool WriteBuffer<T>(ulong addr, Span<T> buffer) where T : unmanaged
        {
            return WriteBuffer(PID, addr, buffer);
        }

        /// <summary>
        /// Write value type/struct to the specified address inside of the specified PID.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to write to.</param>
        /// <param name="value">Value to write.</param>
        public static bool WriteValue<T>(uint pid, ulong addr, T value) where T : unmanaged
        {
            if (!_hVMM.MemWriteStruct(pid, addr, value))
                return false;

            return true;
        }

        /// <summary>
        /// Write value type/struct to the specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to write to.</param>
        /// <param name="value">Value to write.</param>
        public static bool WriteValue<T>(ulong addr, T value) where T : unmanaged
        {
            return WriteValue<T>(PID, addr, value);
        }

        /// <summary>
        /// Perform a Scatter Write Operation.
        /// </summary>
        /// <param name="entries">Write entries.</param>
        public static void WriteScatter(IReadOnlyList<ScatterWriteEntry> entries)
        {
            try
            {
                using var hScatter = _hVMM.Scatter_Initialize(PID, Vmm.FLAG_NOCACHE);
                foreach (var entry in entries)
                {
                    if (!hScatter.PrepareWrite(entry.Va, entry.Value))
                        throw new DMAException($"ERROR preparing Scatter Write for entry 0x{entry.Va:X}");
                }

                if (!hScatter.Execute())
                    throw new DMAException("Scatter Write Failed!");
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR executing Scatter Write!", ex);
            }
        }

        /// <summary>
        /// Write a buffer to the specified address and validate the right bytes were written.
        /// </summary>
        /// <param name="addr">Address to write to.</param>
        /// <param name="buffer">Buffer to write.</param>
        public static bool WriteBufferEnsure(ulong addr, byte[] buffer)
        {
            const int RetryCount = 3;

            try
            {
                bool success = false;
                for (int i = 0; i < RetryCount; i++)
                {
                    if (!_hVMM.MemWrite(PID, addr, buffer))
                        throw new Exception("Memory Write Failed!");

                    // Validate the bytes were written properly
                    var validateBytes = ReadBufferEnsure(addr, buffer.Length);

                    if (validateBytes is null || !validateBytes.SequenceEqual(buffer))
                    {
                        Logger.WriteLine($"[WARN] WriteBufferEnsure() -> 0x{addr:X} did not pass validation on try {i + 1}!");
                        success = false;
                        continue;
                    }

                    success = true;
                    break;
                }

                return success;
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR writing bytes at 0x{addr:X)}", ex);
            }
        }

        #endregion

        #region Memory Macros

        /// Mem Align Functions Ported from Win32 (C Macros)
        private const ulong PAGE_SIZE = 0x1000;
        private const int PAGE_SHIFT = 12;

        /// <summary>
        /// The PAGE_ALIGN macro takes a virtual address and returns a page-aligned
        /// virtual address for that page.
        /// </summary>
        private static ulong PAGE_ALIGN(ulong va)
        {
            return (va & ~(PAGE_SIZE - 1));
        }

        /// <summary>
        /// The ADDRESS_AND_SIZE_TO_SPAN_PAGES macro takes a virtual address and size and returns the number of pages spanned by the size.
        /// </summary>
        private static uint ADDRESS_AND_SIZE_TO_SPAN_PAGES(ulong va, uint size)
        {
            return (uint)((BYTE_OFFSET(va) + (size) + (PAGE_SIZE - 1)) >> PAGE_SHIFT);
        }

        /// <summary>
        /// The BYTE_OFFSET macro takes a virtual address and returns the byte offset
        /// of that address within the page.
        /// </summary>
        private static uint BYTE_OFFSET(ulong va)
        {
            return (uint)(va & (PAGE_SIZE - 1));
        }

        #endregion

        public static void Dispose()
        {
            MemRefreshAll.Dispose();
            MemRefreshRead.Dispose();
            MemRefreshTLB.Dispose();
            _hVMM.Dispose();
        }
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
