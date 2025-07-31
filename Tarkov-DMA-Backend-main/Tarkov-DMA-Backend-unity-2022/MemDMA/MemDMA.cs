using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.MemDMA.vmmsharp.MemRefresh;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.UI;
using Tarkov_DMA_Backend.Unity;
using TerraFX.Interop.Windows;
using Vmmsharp;

namespace Tarkov_DMA_Backend.MemDMA
{
    public static class Memory
    {
        public static Vmm HVMM => _hVMM;
        public static VmmProcess Process => _hProcess;

        private static readonly string[] BaseInitArgs = new string[] {
            "-device",
            $"fpga://algo={FpgaAlgo.Auto}",
            "-waitinitialize",
            "-disable-python",
            "-disable-symbolserver",
            "-disable-symbols",
            "-disable-infodb",
            "-norefresh"
        };

        private const string _mmapFileName = "mmap.txt";
        public static ulong UnityPlayerModuleBase;
        public static ulong MonoModuleBase;

        private static Vmm _hVMM;
        private static VmmProcess _hProcess;

        private static MemRefreshAll MemRefreshAll;
        private static MemRefreshRead MemRefreshRead;
        private static MemRefreshTLB MemRefreshTLB;

        #region Startup

        public static void Initialize()
        {
            if (!InitializeDMA())
            {
                MessageBox.ShowError("Failed to initialize DMA card.", "DMA Startup");
                Environment.Exit(1);
            }

            if (!DeployMMAP())
            {
                if (!DeployMMAP(true))
                {
                    MessageBox.ShowError("Failed to get/apply memory map.", "DMA Startup");
                    Environment.Exit(1);
                }
            }
        }

        private static bool InitializeDMA()
        {
            try
            {
                _hVMM = StartDMA(BaseInitArgs);
                if (_hVMM == null)
                    throw new Exception("Failed to initialize DMA card.");

                //SetCustomRefreshTimings();

                MemRefreshAll = new MemRefreshAll(6000, _hVMM);
                MemRefreshRead = new MemRefreshRead(1000, _hVMM);
                MemRefreshTLB = new MemRefreshTLB(500, _hVMM);

                return true;
            }
            catch (Exception ex)
            {
                _hVMM?.Dispose();
                _hVMM = null;

                Logger.WriteLine($"[DMA] -> FinalStartup(): {ex.Message}");

                return false;
            }
        }

        private static bool DeployMMAP(bool forceRegenerate = false)
        {
            try
            {
                byte[] mmap = null;

                if (!forceRegenerate && File.Exists(_mmapFileName))
                    mmap = File.ReadAllBytes(_mmapFileName);
                else
                {
                    mmap = GetMemMap();
                    if (mmap == null)
                        throw new Exception("Failed to generate memory map.");

                    File.WriteAllBytes(_mmapFileName, mmap);
                }

                if (!_hVMM.LeechCore.Command(LeechCore.LC_CMD_MEMMAP_SET, mmap, out _))
                    throw new Exception("Failed to apply memory map.");

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[DMA] -> DeployMMAP(): {ex.Message}");
                return false;
            }
        }

        private static Vmm StartDMA(string[] args)
        {
            Vmm hVMM = null;

            try
            {
                hVMM = new(args);
            }
            catch
            {
                hVMM?.Dispose();
                hVMM = null;
            }

            return hVMM;
        }

        private static byte[] GetMemMap()
        {
            string map = _hVMM.MapMemoryAsString();
            if (string.IsNullOrEmpty(map))
                return null;

            return Encoding.ASCII.GetBytes(map);
        }

        /// <summary>
        /// Sets the custom Refresh Timings as per the Vmm Config.
        /// 1 Tick = 100ms.
        /// </summary>
        private static void SetCustomRefreshTimings()
        {
            if (!_hVMM.SetConfig(Vmm.CONFIG_OPT_CONFIG_PROCCACHE_TICKS_PARTIAL, 100) ||
                !_hVMM.SetConfig(Vmm.CONFIG_OPT_CONFIG_PROCCACHE_TICKS_TOTAL, 1800) ||
                !_hVMM.SetConfig(Vmm.CONFIG_OPT_CONFIG_READCACHE_TICKS, 4) ||
                !_hVMM.SetConfig(Vmm.CONFIG_OPT_CONFIG_TLBCACHE_TICKS, 25))
            {
                throw new Exception("Failed to initialize timings.");
            }
        }

        public static bool Attach(string processName)
        {
            VmmProcess process = _hVMM.Process(processName);
            if (process == null || !process.IsValid)
                return false;

            _hProcess = process;

            return true;
        }

        public static void Detach()
        {
            _hProcess = null;
        }

        #endregion

        #region Scatter Read

        /// <summary>
        /// Performs multiple reads in one sequence, significantly faster than single reads.
        /// Designed to run without throwing unhandled exceptions, which will ensure the maximum amount of
        /// reads are completed OK even if a couple fail.
        /// </summary>
        public static void ReadScatter(ReadOnlySpan<IScatterEntry> entries, bool useCache = true)
        {
            var pagesToRead = new HashSet<ulong>(); // Will contain each unique page only once to prevent reading the same page multiple times
            foreach (var entry in entries) // First loop through all entries - GET INFO
            {
                // Parse Address and Size properties
                ulong addr = entry.ParseAddr();
                uint size = (uint)entry.ParseSize();

                // INTEGRITY CHECK - Make sure the read is valid and within range
                if (addr == 0x0 || size == 0 || size > (PAGE_SIZE * 10))
                {
                    entry.IsFailed = true;
                    continue;
                }
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
            var scatters = _hProcess.MemReadScatter(flags, pagesToRead.ToArray()); // execute scatter read

            foreach (var entry in entries) // Second loop through all entries - PARSE RESULTS
            {
                if (entry.IsFailed) // Skip this entry, leaves result as null
                    continue;

                ulong readAddress = (ulong)entry.Addr + entry.Offset; // location of object
                uint pageOffset = BYTE_OFFSET(readAddress); // Get object offset from the page start address

                uint size = (uint)(int)entry.Size;
                var buffer = new byte[size]; // Alloc result buffer on heap
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
                            .CopyTo(buffer.AsSpan(bytesCopied, (int)cb)); // Copy bytes to buffer
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
                entry.SetResult(buffer);
            }
        }

        #endregion

        #region Misc

        public static ulong GetModuleBase(string module)
        {
            return _hProcess.GetModuleBase(module);
        }

        #endregion

        #region Custom Read Methods

        public static string ReadUnityString(ulong addr, bool useCache = true, int length = 64)
        {
            try
            {
                if (length % 2 != 0)
                    length += 1;

                length *= 2; // Unicode 2 bytes per char

                if ((uint)length > PAGE_SIZE)
                    throw new Exception("String length outside expected bounds!");

                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                var buffer = ReadBuffer(addr + UnityOffsets.UnityString.Value, length, useCache).AsSpan();
                int nullIndex = buffer.FindUtf16NullTerminatorIndex();
                return nullIndex >= 0 ? Encoding.Unicode.GetString(buffer.Slice(0, nullIndex)) : Encoding.Unicode.GetString(buffer);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading UnityString at 0x{addr.ToString("X")}", ex);
            }
        }

        public static Dictionary<ulong, string> ScatterUnityStrings(IEnumerable<ulong> addressesRaw)
        {
            List<ulong> addresses = addressesRaw.ToList();
            int totalStrings = addresses.Count;

            // Get string lengths
            EFTScatterMap map1 = new(totalStrings);
            var round1 = map1.AddRound(false);

            for (int i = 0; i < totalStrings; i++)
            {
                ulong address = addresses[i];
                round1.AddEntry<int>(i, 1, address + UnityOffsets.UnityString.Length);
            }

            map1.Execute();

            // Get string data
            EFTScatterMap map2 = new(totalStrings);
            var round2 = map2.AddRound(false);

            for (int i = 0; i < totalStrings; i++)
            {
                ulong address = addresses[i];

                if (!map1.Results[i][1].TryGetResult(out int size))
                    continue;

                if (size % 2 != 0)
                    size += 1;

                size *= 2; // Unicode 2 bytes per char

                round2.AddEntry<byte[]>(i, 1, address + UnityOffsets.UnityString.Value, size);
            }

            map2.Execute();

            // Convert bytes to unicode strings
            Dictionary<ulong, string> strings = new();
            for (int i = 0; i < totalStrings; i++)
            {
                ulong address = addresses[i];

                if (!map2.Results[i][1].TryGetResult(out byte[] bytes))
                    continue;

                Span<byte> buffer = bytes.AsSpan();
                int nullIndex = buffer.FindUtf16NullTerminatorIndex();
                string str = nullIndex >= 0 ? Encoding.Unicode.GetString(buffer.Slice(0, nullIndex)) : Encoding.Unicode.GetString(buffer);

                strings.Add(address, str);
            }

            return strings;
        }

        public static List<string> ScatterUtf8Strings(IEnumerable<ulong> addressesRaw, int size)
        {
            List<ulong> addresses = addressesRaw.ToList();
            int totalStrings = addresses.Count;

            // Get string data
            EFTScatterMap map = new(totalStrings);
            var round = map.AddRound(false);

            for (int i = 0; i < totalStrings; i++)
            {
                ulong address = addresses[i];

                round.AddEntry<byte[]>(i, 0, address, size);
            }

            map.Execute();

            // Convert bytes to utf8 strings
            List<string> strings = new();
            for (int i = 0; i < totalStrings; i++)
            {
                ulong address = addresses[i];

                if (!map.Results[i][0].TryGetResult(out byte[] bytes))
                    strings.Add(null);
                else
                {
                    Span<byte> buffer = bytes.AsSpan();
                    int nullIndex = buffer.IndexOf((byte)0);
                    string str = nullIndex >= 0 ? Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);

                    strings.Add(str);
                }
            }

            return strings;
        }

        /// <summary>
        /// Helper method to locate GOM Objects.
        /// </summary>
        public static ulong GetObjectFromList(ulong currentObjectPtr, ulong lastObjectPtr, string objectName, int maxIndex = -1)
        {
            var currentObject = Memory.ReadValue<BaseObject>(currentObjectPtr, false);
            var lastObject = Memory.ReadValue<BaseObject>(lastObjectPtr, false);

            if (currentObject.CurrentObject != 0x0)
            {
                int currentIndex = 0;
                while (currentObject.CurrentObject != 0x0 && currentObject.CurrentObject != lastObject.CurrentObject)
                {
                    if (maxIndex != -1 && currentIndex > maxIndex)
                        break;

                    var objectNamePtr = ReadPtr(currentObject.CurrentObject + UnityOffsets.GameObject.ObjectName, false);
                    var objectNameStr = ReadUtf8String(objectNamePtr, 64, false);
                    if (objectNameStr.Contains(objectName, StringComparison.OrdinalIgnoreCase))
                    {
                        Logger.WriteLine($"[GetObjectFromList] Found object {objectNameStr}");
                        return currentObject.CurrentObject;
                    }

                    currentObject = ReadValue<BaseObject>(currentObject.NextObjectLink, false); // Read next object
                }
            }

            if (maxIndex != -1)
                Logger.WriteLine($"[GetObjectFromList] Broke out of while loop after {maxIndex} iterations. Unable to find object {objectName}");
            else
                Logger.WriteLine($"[GetObjectFromList] Couldn't find object {objectName}");

            return 0x0;
        }

        /// <summary>
        /// Gets a child class from a parent Game Object.
        /// </summary>
        /// <param name="objectPtr">Parent object.</param>
        /// <param name="className">Name of class of child.</param>
        /// <returns>Child class component.</returns>
        public static ulong GetObjectComponent(ulong objectPtr, string className)
        {
            // component list
            var components = ReadPtr(objectPtr + 0x30);
            // might need de/increase depending on parent class
            for (uint i = 0; i < 0x1000; i++)
            {
                try
                {
                    // returning field if name matches
                    var field = ReadPtrChain(components, new uint[] { 0x8 + (i * 0x10), 0x28 });
                    // get object's classname
                    var namePtr = ReadPtrChain(field, UnityOffsets.Component.To_NativeClassName);
                    // actual class name
                    var name = ReadUtf8String(namePtr, 128);
                    // compare to arg passed
                    if (name.Equals(className, StringComparison.OrdinalIgnoreCase))
                        return field;
                }
                catch { }
            }
            throw new NullPtrException();
        }

        /// <summary>
        /// Gets Game Camera by provided name.
        /// </summary>
        /// <param name="name">Name of camera to search.</param>
        /// <returns>Camera game object.</returns>
        public static ulong GetCameraByName(string name, bool deref = true)
        {
            var temp = ReadPtrChain(UnityPlayerModuleBase + UnityOffsets.ModuleBase.AllCameras, [0x0, 0x0]);

            for (uint i = 0; i < 400; i++)
            {
                try
                {
                    var cameraComponent = ReadPtr(temp + (i * 0x8));
                    var cameraGameObj = ReadPtr(cameraComponent + UnityOffsets.Component.GameObject);
                    var cameraNamePtr = ReadPtr(cameraGameObj + UnityOffsets.GameObject.ObjectName);

                    var cameraName = ReadUtf8String(cameraNamePtr, 128);
                    if (cameraName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        if (deref) return cameraGameObj;
                        else return cameraComponent;
                    }
                }
                catch { }
            }

            Logger.WriteLine($"[DMA] Unable to find camera name: \"{name}\"!");
            throw new NullPtrException();
        }

        public static ulong GetCamera(ulong camera, bool useCache = true)
        {
            ulong cameraClass = GetObjectComponent(camera, "Camera");
            return ReadPtr(cameraClass + 0x10, useCache);
        }

        public static bool IsCameraActive(ulong camera)
        {
            return ReadValue<bool>(camera + UnityOffsets.Behaviour.IsAdded, false);
        }

        #endregion

        #region Read Methods

        /// <summary>
        /// Reads a list of pointers.
        /// </summary>
        public static List<ulong> ReadPointers(List<ulong> pointers, bool useCache = true)
        {
            EFTScatterMap map = new(pointers.Count);
            var round = map.AddRound(useCache);

            for (int i = 0; i < pointers.Count; i++)
            {
                ulong pointer = pointers[i];

                round.AddEntry<ulong>(i, 0, pointers[i]);
            }

            map.Execute();

            List<ulong> output = new();
            for (int i = 0; i < pointers.Count; i++)
            {
                if (!map.Results[i][0].TryGetResult(out ulong addr))
                    output.Add(0x0);
                else
                    output.Add(addr);
            }

            return output;
        }

        public static List<ulong> AreAny(List<ulong> objects, string compareType, bool useCache = true)
        {
            List<ulong> output = new();

            int totalCount = objects.Count;

            List<ulong> p1 = ReadPointers(objects, false);
            Logger.WriteLine("[QUEST ZONE DUMPER] Stage 1X");

            List<ulong> p2 = ReadPointers(p1, false);
            Logger.WriteLine("[QUEST ZONE DUMPER] Stage 2X");

            for (int i = 0; i < 5; i++)
            {
                List<ulong> p3Raw = new();
                for (int ii = 0; ii < totalCount; ii++)
                    p3Raw.Add(p2[ii] + 0x30);
                List<ulong> p3 = ReadPointers(p3Raw, false);
                Logger.WriteLine($"[QUEST ZONE DUMPER] Stage 3:{i}");

                List<ulong> p4Raw = new();
                for (int ii = 0; ii < totalCount; ii++)
                    p4Raw.Add(p3[ii] + 0x48);
                List<ulong> p4 = ReadPointers(p4Raw, false);
                Logger.WriteLine($"[QUEST ZONE DUMPER] Stage 4:{i}");

                List<string> strings = ScatterUtf8Strings(p4, 128);
                for (int ii = 0; ii < strings.Count; ii++)
                {
                    string item = strings[ii];

                    if (item == null)
                        continue;

                    if (item.Contains(compareType, StringComparison.OrdinalIgnoreCase))
                        output.Add(objects[ii]);

                    p2[ii] = p3[ii];
                }
            }

            return output;
        }

        /// <summary>
        /// Read memory into a buffer.
        /// </summary>
        public static byte[] ReadBuffer(ulong addr, int size, bool useCache = true, bool allowIncompleteRead = false)
        {
            try
            {
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                var buf = _hProcess.MemRead(addr, (uint)size, flags);
                if (!allowIncompleteRead && buf.Length != size)
                    throw new DMAException("Incomplete memory read!");
                return buf;
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading buffer at 0x{addr:X}", ex);
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
                byte[][] buffers = new byte[ValidationCount][];
                for (int i = 0; i < ValidationCount; i++)
                {
                    buffers[i] = _hProcess.MemRead(addr, (uint)size, Vmm.FLAG_NOCACHE);
                    
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
                    throw new DMAException($"[DMA] ERROR reading pointer chain at index {i}, addr 0x{ptr:X} + 0x{offsets[i]:X}", ex);
                }
            }
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
                throw new DMAException($"[DMA] ERROR reading pointer at 0x{addr:X}", ex);
            }
        }

        /// <summary>
        /// Resolves a pointer and returns the memory address it points to.
        /// </summary>
        public static ulong ReadPtrUnsafe(ulong addr, bool useCache = true)
        {
            try
            {
                return ReadValue<ulong>(addr, useCache);
            }
            catch
            {
                return 0x0;
            }
        }

        /// <summary>
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to read from.</param>
        public static T ReadValue<T>(ulong addr, bool useCache = true) where T : struct
        {
            try
            {
                int size = Unsafe.SizeOf<T>();
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                var buf = _hProcess.MemRead(addr, (uint)size, flags);
                if (buf.Length != size)
                    throw new Exception("Incomplete Memory Read!");
                return Unsafe.As<byte, T>(ref buf[0]);
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR reading {typeof(T)} value at 0x{addr:X}", ex);
            }
        }

        /// <summary>
        /// Read value type/struct from specified address and validate the right bytes were received.
        /// </summary>
        public static T? ReadValueEnsure<T>(ulong addr) where T : struct
        {
            const int ValidationCount = 3;

            try
            {
                int size = Unsafe.SizeOf<T>();

                T[] buffers = new T[ValidationCount];
                for (int i = 0; i < ValidationCount; i++)
                {
                    var buf = _hProcess.MemRead(addr, (uint)size, Vmm.FLAG_NOCACHE);
                    if (buf.Length != size)
                        throw new Exception("Incomplete Memory Read!");

                    buffers[i] = Unsafe.As<byte, T>(ref buf[0]);
                }

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
        /// Read value type/struct from specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="address">Address to read from.</param>
        public static T MonoRead<T>(ulong address, bool useCache = true) where T : struct
        {
            try
            {
                int size = Unsafe.SizeOf<T>();
                uint flags = useCache ? 0 : Vmm.FLAG_NOCACHE;
                var buf = _hProcess.MemRead(address, (uint)size, flags);
                if (buf.Length != size)
                    throw new Exception("Incomplete Memory Read!");
                return Unsafe.As<byte, T>(ref buf[0]);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Resolves a pointer and returns the memory address it points to.
        /// </summary>
        public static ulong MonoReadPtr(ulong addr, bool useCache = true)
        {
            try
            {
                var ptr = ReadValue<MemPointer>(addr, useCache);

                return ptr;
            }
            catch
            {
                return 0x0;
            }
        }

        /// <summary>
        /// Read null terminated utf-8 string.
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <exception cref="DMAException"></exception>
        public static string ReadUtf8String(ulong addr, int length, bool useCache = true)
        {
            try
            {
                if ((uint)length > PAGE_SIZE)
                    throw new Exception("String length outside expected bounds!");

                var buffer = ReadBuffer(addr, length, useCache).AsSpan();
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
        /// Read null terminated string (utf-8/default).
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        public static string ReadString(ulong addr, int length, bool useCache = true)
        {
            try
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(length, (int)PAGE_SIZE, nameof(length));
                var buffer = ReadBuffer(addr, length, useCache).AsSpan();
                var nullIndex = buffer.IndexOf((byte)0);
                return nullIndex >= 0 ? Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception($"[DMA] ERROR reading string at 0x{addr.ToString("X")}", ex);
            }
        }

        #endregion

        #region Write Methods

        /// <summary>
        /// Write value type/struct to specified address.
        /// </summary>
        /// <typeparam name="T">Specified Value Type.</typeparam>
        /// <param name="addr">Address to write to.</param>
        /// <param name="value">Value to write.</param>
        public static void WriteValue<T>(ulong addr, T value) where T : struct
        {
            try
            {
                var data = new byte[Unsafe.SizeOf<T>()];
                MemoryMarshal.Write(data, in value);
                if (!_hProcess.MemWrite(addr, data))
                    throw new Exception("Memory Write Failed!");
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR writing {typeof(T)} value at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// Write a buffer to the specified address.
        /// </summary>
        /// <param name="addr">Address to write to.</param>
        /// <param name="buffer">Buffer to write.</param>
        public static void WriteBuffer(ulong addr, byte[] buffer)
        {
            try
            {
                if (!_hProcess.MemWrite(addr, buffer))
                    throw new Exception("Memory Write Failed!");
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR writing bytes at 0x{addr:X)}", ex);
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
                    if (!_hProcess.MemWrite(addr, buffer))
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
                throw new DMAException($"[DMA] ERROR writing bytes at 0x{addr:X}", ex);
            }
        }

        /// <summary>
        /// Perform a Scatter Write Operation.
        /// </summary>
        /// <param name="entries">Write entries.</param>
        public static void WriteScatter(IReadOnlyList<ScatterWriteEntry> entries)
        {
            try
            {
                using var hScatter = _hProcess.Scatter_Initialize(Vmm.FLAG_NOCACHE);
                foreach (var entry in entries)
                {
                    if (!hScatter.PrepareWrite(entry.Va, entry.Value))
                        throw new DMAException($"ERROR preparing Scatter Write for entry 0x{entry.Va.ToString("X")}");
                }
                if (!hScatter.Execute())
                    throw new DMAException("Scatter Write Failed!");
            }
            catch (Exception ex)
            {
                throw new DMAException($"[DMA] ERROR executing Scatter Write!", ex);
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
