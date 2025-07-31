using arena_dma_backend.DMA.Collections;
using arena_dma_backend.DMA.Collections.Implementation;
using arena_dma_backend.DMA.Collections.Implementation.NoThrow;

namespace arena_dma_backend.Mono
{
    public static class MonoAPI
    {
        private static IReadOnlyDictionary<ulong, ulong> JittedFunctions;

        public static ulong GameWorld { get; private set; }
        public static ulong InertiaSettings { get; private set; }
        public static ulong GameSettings { get; private set; }
        public static ulong HardSettings { get; private set; }

        /// <summary>
        /// Initialize Mono at Game Startup.
        /// </summary>
        /// <returns>True if Init OK, otherwise False.</returns>
        public static bool Initialize()
        {
            try
            {
                Logger.WriteLine("[MONO] -> Initialize(): [...]");

                ulong[] singletons = Singleton.FindMany([
                    "GameWorld",
                    ClassNames.InertiaSettingsName,
                    ClassNames.GameSettingsName,
                ]);

                if (singletons[0] != 0x0)
                    GameWorld = singletons[0];
                else
                    throw new Exception("[MONO] Unable to find GameWorld!");

                if (singletons[1] != 0x0)
                    InertiaSettings = singletons[1];
                else
                    throw new Exception("[MONO] Unable to find InertiaSettings!");

                if (singletons[2] != 0x0)
                    GameSettings = singletons[2];
                else
                    throw new Exception("[MONO] Unable to find GameSettings!");

                Class[] staticClasses = Class.FindMany("Assembly-CSharp", [
                    "EFTHardSettings"
                ]);

                if (staticClasses[0] != 0x0)
                    HardSettings = staticClasses[0];
                else
                    throw new Exception("[MONO] Unable to find HardSettings!");

                Logger.WriteLine("[MONO] -> Initialize(): [OK]");

                return true;
            }
            catch(Exception ex)
            {
                Logger.WriteLine($"[MONO] -> Initialize(): [FAIL] ~ {ex}");

                return false;
            }
        }

        private static readonly object _initializeFunctionsLock = new();
        /// <summary>
        /// Initialize Mono Functions (should be done once per raid).
        /// </summary>
        /// <returns>True if Init OK, otherwise False.</returns>
        public static bool InitializeFunctions()
        {
            if (Monitor.TryEnter(_initializeFunctionsLock, TimeSpan.Zero))
            {
                try
                {
                    if (UpdateJittedFunctions())
                    {
                        Logger.WriteLine("[MONO] -> InitializeFunctions(): [OK]");
                        return true;
                    }
                    else
                    {
                        Logger.WriteLine("[MONO] -> InitializeFunctions(): [FAIL]");
                        return false;
                    }
                }
                catch { throw; }
                finally { Monitor.Exit(_initializeFunctionsLock); }
            }
            else
            {
                Logger.WriteLine("[MONO] -> InitializeFunctions(): [SKIPPED]");
                return false;
            }
        }

        public static ushort UTF8ToUTF16(string val)
        {
            Span<byte> utf16Bytes = stackalloc byte[2 * val.Length]; // UTF-16 can be up to 2 bytes per char

            var byteCount = Encoding.Unicode.GetBytes(val.AsSpan(), utf16Bytes);
            if (byteCount < 2)
                throw new ArgumentException("Input string is too short.", nameof(val));

            return BitConverter.ToUInt16(utf16Bytes);
        }

        public static string ReadWideChar(ulong address, int size)
        {
            try
            {
                return Memory.ReadString(address, size);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ReadName(ulong addr, int length)
        {
            try
            {
                if (length % 2 != 0)
                    length++;

                using var hBuf = new SharedMemory<byte>(length);
                var buffer = hBuf.Span;
                Memory.ReadBuffer(addr, buffer);
                if (buffer[0] >= 0xE0)
                {
                    var nullIndex = buffer.IndexOf((byte)0);
                    var value = nullIndex >= 0 ? Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
                    return $"\\u{UTF8ToUTF16(value):X4}";
                }
                else
                {
                    var nullIndex = buffer.IndexOf((byte)0);
                    return nullIndex >= 0 ? Encoding.UTF8.GetString(buffer.Slice(0, nullIndex)) : Encoding.UTF8.GetString(buffer);
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static MonoRootDomain GetRootDomain()
        {
            return new MonoRootDomain(Memory.ReadPtrUnsafe(Memory.MonoModuleBase + 0x499C78));
        }

        public static bool UpdateJittedFunctions()
        {
            Dictionary<ulong, ulong> functions = new();

            var rootDomain = GetRootDomain();
            if (rootDomain == 0x0)
                return false;

            var jittedTable = rootDomain.JittedFunctionTable();
            if (jittedTable == 0x0)
                return false;

            int count = Memory.ReadValueUnsafe<int>(jittedTable + 0x8);
            if (count > 5000)
            {
                Logger.WriteLine($"[MONO] InitializeFunctions(): count is out of bounds!");
                return false;
            }

            ulong entryArrayBase = jittedTable + 0x10;
            using MemArray<ulong> entryArray = new(entryArrayBase, count);

            foreach (ulong entry in entryArray)
            {
                if (entry == 0x0)
                    continue;

                int count2 = Memory.ReadValueUnsafe<int>(entry + 0x4);
                if (count2 > 5000)
                {
                    Logger.WriteLine($"[MONO] InitializeFunctions(): count2 is out of bounds!");
                    return false;
                }

                ulong functionArrayBase = entry + 0x18;
                using MemArray<ulong> functionArray = new(functionArrayBase, count2);

                foreach (ulong function in functionArray)
                {
                    if (function == 0x0)
                        continue;

                    ulong monoPtr = Memory.ReadPtrUnsafe(function + 0x0);
                    if (monoPtr == 0x0)
                        continue;

                    ulong jittedPtr = Memory.ReadPtrUnsafe(function + 0x10);
                    if (jittedPtr == 0x0)
                        continue;

                    functions[monoPtr] = jittedPtr;
                }
            }

            JittedFunctions = functions;

            return true;
        }

        public static MonoAssembly DomainAssemblyOpen(MonoRootDomain domain, string name)
        {
            var domainAssemblies = domain.DomainAssemblies();
            if (domainAssemblies == 0x0)
                return default;

            ulong data;
            while (true)
            {
                data = domainAssemblies.Data();
                if (data == 0x0)
                    continue;

                var dataName = ReadWideChar(Memory.ReadPtrUnsafe(data + 0x10), 128);
                if (dataName == name)
                    break;

                domainAssemblies = new GList(domainAssemblies.Next());
                if (domainAssemblies == 0x0)
                    break;
            }

            return new MonoAssembly(data);
        }

        public static class Singleton
        {
            [StructLayout(LayoutKind.Explicit, Pack = 1)]
            private struct SingletonHashTable
            {
                [FieldOffset(0x0)]
                public int table_size;
                [FieldOffset(0x8)]
                public ulong kvs; // key_value_pair
            }

            [StructLayout(LayoutKind.Explicit, Pack = 1)]
            private struct GenericClassPtrEntry
            {
                [FieldOffset(0x8)]
                public ulong Ptr;
            }

            [StructLayout(LayoutKind.Explicit, Pack = 1)]
            private struct GenericClassPtrData
            {
                [FieldOffset(0x20)]
                public byte Inited;
                [FieldOffset(0x28)]
                public uint Flags;
            }

            public static ulong FindOne(string className)
            {
                return FindMany(new[] { className })[0];
            }

            public static ulong[] FindMany(string[] classNames)
            {
                ulong[] results = new ulong[classNames.Length];
                int foundCount = 0;

                ulong monoImageSetPtrBase = Memory.MonoModuleBase + 0x49A400; // img_set_cache (MonoImageSet)

                using var monoImageSetPtrArray = NoThrowMemArray<ulong>.Get(monoImageSetPtrBase, 1103);
                foreach (ulong monoImageSetPtr in monoImageSetPtrArray)
                {
                    if (monoImageSetPtr == 0x0)
                        continue;

                    ulong gclassCache = Memory.ReadPtrUnsafe(monoImageSetPtr + 0x28); // gclass_cache (_MonoConcurrentHashTable)
                    if (gclassCache == 0x0)
                        continue;

                    ulong table = Memory.ReadPtrUnsafe(gclassCache); // _MonoConcurrentHashTable -> table (conc_table)
                    if (table == 0x0)
                        continue;

                    var tableData = Memory.ReadValueUnsafe<SingletonHashTable>(table);
                    if (tableData.table_size > 100000 || tableData.kvs == 0x0)
                        continue;

                    using var genericClassPtrArray = NoThrowMemArray<GenericClassPtrEntry>.Get(tableData.kvs, tableData.table_size);
                    foreach (GenericClassPtrEntry genericClassPtr in genericClassPtrArray)
                    {
                        if (genericClassPtr.Ptr == 0x0)
                            continue;

                        ulong classPtr = Memory.ReadPtrUnsafe(genericClassPtr.Ptr + 0x20);
                        if (classPtr == 0x0)
                            continue;

                        var classPtrData = Memory.ReadValueUnsafe<GenericClassPtrData>(classPtr);

                        if ((classPtrData.Inited & 1) != 1 ||           // !class->inited
                            (classPtrData.Flags & 0x800000) != 0 ||     // class->exception_type != MONO_EXCEPTION_NONE
                            (classPtrData.Flags & 0x70000) != 0x30000)  // class->class_kind != MONO_CLASS_GINST
                        {
                            continue;
                        }

                        Class monoClass = new(classPtr);

                        if (monoClass.IsSingleton())
                        {
                            int index = Array.IndexOf(classNames, monoClass.SingletonName());
                            if (index != -1)
                            {
                                ulong staticDataPtr = monoClass.GetVTable(GetRootDomain()).GetStaticFieldData();
                                results[index] = staticDataPtr;
                                foundCount++;
                            }

                            if (foundCount == classNames.Length)
                            {
                                if (classNames.Length > 1)
                                    Logger.WriteLine($"[MONO] -> FindSingletons(): Found all {classNames.Length} singletons!");
                                else
                                    Logger.WriteLine($"[MONO] -> FindSingletons(): Found the singleton!");

                                return results;
                            }
                        }
                    }
                }

                return results;
            }
        }

        public readonly struct GList(ulong baseAddress)
        {
            public static implicit operator ulong(GList x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly ulong Data() => Memory.ReadPtrUnsafe(this + 0x0);
            public readonly ulong Next() => Memory.ReadPtrUnsafe(this + 0x8);
        }

        public readonly struct MonoRootDomain(ulong baseAddress)
        {
            public static implicit operator ulong(MonoRootDomain x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly GList DomainAssemblies() => new(Memory.ReadPtrUnsafe(this + 0xC8));
            public readonly int DomainID() => Memory.ReadValueUnsafe<int>(this + 0xBC);
            public readonly ulong JittedFunctionTable() => Memory.ReadPtrUnsafe(this + 0x148);
        }

        public readonly struct MonoTableInfo(ulong baseAddress)
        {
            public static implicit operator ulong(MonoTableInfo x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly int GetRows() => Memory.ReadValueUnsafe<int>(this + 0x8) & 0xFFFFFF;
        }

        public readonly struct MonoMethod(ulong baseAddress)
        {
            public static implicit operator ulong(MonoMethod x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly string Name()
            {
                ulong address = Memory.ReadPtrUnsafe(this + 0x18);
                if (address == 0x0)
                    return string.Empty;

                return ReadName(address, 128);
            }
        }

        public readonly struct MonoClassField(ulong baseAddress)
        {
            public static implicit operator ulong(MonoClassField x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly string Name()
            {
                ulong address = Memory.ReadPtrUnsafe(this + 0x8);
                if (address == 0x0)
                    return string.Empty;

                return ReadName(address, 128);
            }

            public readonly int Offset()
            {
                return Memory.ReadValueUnsafe<int>(this + 0x18);
            }
        }

        public readonly struct MonoClassRuntimeInfo(ulong baseAddress)
        {
            public static implicit operator ulong(MonoClassRuntimeInfo x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly int MaxDomain() => Memory.ReadValueUnsafe<int>(this + 0x0);
        }

        public readonly struct MonoVTable(ulong baseAddress)
        {
            public static implicit operator ulong(MonoVTable x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly ulong GetClassType() => Memory.ReadValueUnsafe<ulong>(this + 0x18);

            public readonly byte Flags() => Memory.ReadValueUnsafe<byte>(this + 0x30);

            public readonly ulong GetStaticFieldData()
            {
                if ((Flags() & 4) != 0)
                    return Memory.ReadPtrUnsafe(this + 0x40 + 8 * (uint)Memory.ReadValueUnsafe<int>(Memory.ReadPtrUnsafe(this + 0x0) + 0x5C));

                return 0x0;
            }
        }

        public readonly struct Class(ulong baseAddress)
        {
            public static implicit operator ulong(Class x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly bool IsSingleton() => NamespaceName().Contains("Comfort.Common", StringComparison.OrdinalIgnoreCase) && Name().Contains("Singleton", StringComparison.OrdinalIgnoreCase);
            public readonly int NumFields() => Memory.ReadValueUnsafe<int>(this + 0x100);
            public readonly MonoClassRuntimeInfo RuntimeInfo() => new(Memory.ReadPtrUnsafe(this + 0xD0));

            public readonly string Name()
            {
                ulong address = Memory.ReadPtrUnsafe(this + 0x48);
                if (address == 0x0)
                    return string.Empty;

                return ReadName(address, 128);
            }

            public readonly string NamespaceName()
            {
                ulong address = Memory.ReadPtrUnsafe(this + 0x50);
                if (address == 0x0)
                    return string.Empty;

                return ReadName(address, 128);
            }

            public readonly string SingletonName()
            {
                try
                {
                    ulong genericClassPtr = Memory.ReadPtr(this + 0xF0);
                    ulong genericClassContextPtr = Memory.ReadPtr(genericClassPtr + 0x8);
                    ulong argumentClassTypePtr = Memory.ReadPtr(genericClassContextPtr + 0x8);
                    ulong argumentClassPtr = Memory.ReadPtr(argumentClassTypePtr);
                    ulong monoClassNamePtr = Memory.ReadPtr(argumentClassPtr + 0x48);

                    return ReadName(monoClassNamePtr, 64);
                }
                catch { return string.Empty; }
            }

            public readonly int GetNumMethods()
            {
                var v2 = (Memory.ReadValueUnsafe<int>(this + 0x2A) & 7) - 1;
                switch (v2)
                {
                    case 0:
                    case 1:
                        return Memory.ReadValueUnsafe<int>(this + 0xFC);

                    case 3:
                    case 5:
                        return 0;

                    case 4:
                        return Memory.ReadValueUnsafe<int>(this + 0xF0);

                    default: break;
                }

                return 0;
            }

            public readonly MonoMethod GetMethod(int i) => new(Memory.ReadPtrUnsafe(Memory.ReadPtrUnsafe(this + 0xA0) + 0x8 * (uint)i));

            public readonly MonoClassField GetField(int i)
            {
                var address = Memory.ReadPtrUnsafe(this + 0x98);
                if (address == 0x0)
                    return default;

                return new MonoClassField(address + (ulong)(0x20 * i));
            }

            public readonly MonoVTable GetVTable(MonoRootDomain domain)
            {
                var runtimeInfo = new MonoClassRuntimeInfo(RuntimeInfo());
                if (runtimeInfo == 0x0)
                    return default;

                var domainID = domain.DomainID();
                if (runtimeInfo.MaxDomain() < domainID)
                    return default;

                return new MonoVTable(Memory.ReadPtrUnsafe(runtimeInfo + 8 * (uint)domainID + 8));
            }

            new public readonly ulong GetType() => this + 0xB8;

            public readonly MonoMethod FindMethod(string methodName)
            {
                ulong monoPtr = 0x0;

                int methodCount = GetNumMethods();
                if (methodCount > 5000)
                {
                    Logger.WriteLine($"[MONO] FindMethod(): methodCount is out of bounds!");
                    return default;
                }

                for (int i = 0; i < methodCount; i++)
                {
                    var method = GetMethod(i);

                    if (method == 0x0)
                        continue;

                    if (method.Name() == methodName)
                        monoPtr = method;
                }

                return new MonoMethod(monoPtr);
            }

            public readonly ulong FindJittedMethod(string methodName)
            {
                MonoMethod method = FindMethod(methodName);

                if (JittedFunctions == null)
                    return 0x0;

                if (JittedFunctions.TryGetValue((ulong)method, out var jittedMethod))
                    return jittedMethod;
                else
                    return 0x0;
            }

            public readonly MonoClassField FindField(string fieldName)
            {
                int fieldCount = NumFields();
                if (fieldCount > 5000)
                {
                    Logger.WriteLine($"[MONO] FindField(): fieldCount is out of bounds!");
                    return default;
                }

                for (int i = 0; i < fieldCount; i++)
                {
                    var field = GetField(i);

                    if (field == 0x0)
                        continue;

                    if (field.Name() == fieldName)
                        return new MonoClassField(field);
                }

                return default;
            }

            private readonly struct FindClassLeadup(int rowCount, MonoHashTable hashTable)
            {
                public readonly int RowCount = rowCount;
                public readonly MonoHashTable HashTable = hashTable;
            }

            private static FindClassLeadup? GetFindClassLeadup(string assemblyName)
            {
                var rootDomain = GetRootDomain();
                if (rootDomain == 0x0)
                    return null;

                var domainAssembly = DomainAssemblyOpen(rootDomain, assemblyName);
                if (domainAssembly == 0x0)
                    return null;

                var monoImage = domainAssembly.MonoImage();
                if (monoImage == 0x0)
                    return null;

                var tableInfo = monoImage.GetTableInfo(2);
                if (tableInfo == 0x0)
                    return null;

                int rowCount = tableInfo.GetRows();
                if (rowCount > 25000)
                {
                    Logger.WriteLine($"[MONO] -> GetFindClassLeadup(): rowCount is out of bounds!");
                    return null;
                }

                MonoHashTable monoImageHashTable = new(monoImage + 0x4C0);

                return new(rowCount, monoImageHashTable);
            }

            public static Class FindOne(string assemblyName, string className)
            {
                return FindMany(assemblyName, new[] { className })[0];
            }

            public static Class[] FindMany(string assemblyName, string[] classNames)
            {
                Class[] results = new Class[classNames.Length];
                int foundCount = 0;

                FindClassLeadup? tmpFindClassLeadup = GetFindClassLeadup(assemblyName);
                if (tmpFindClassLeadup == null)
                    return default;

                FindClassLeadup findClassLeadup = (FindClassLeadup)tmpFindClassLeadup;

                bool mainClassFound = false;
                bool findSubclass = classNames[0].Contains('+');
                for (int i = 0; i < findClassLeadup.RowCount; i++)
                {
                    var ptr = new Class(findClassLeadup.HashTable.Lookup((ulong)(0x02000000 | i + 1)));
                    if (ptr == 0x0)
                        continue;

                    var name = ptr.Name();
                    var ns = ptr.NamespaceName();

                    if (ns.Length != 0)
                        name = ns + "." + name;

                    int index = Array.IndexOf(classNames, name);

                    if (mainClassFound && findSubclass)
                    {
                        if (name.Contains(classNames[0].Split('+')[1]))
                        {
                            results[0] = ptr;
                            foundCount++;
                        }
                    }
                    else if (index != -1)
                    {
                        results[index] = ptr;
                        foundCount++;
                    }
                    else if (classNames.Length == 1 && findSubclass) // Only works on FindOne() calls
                    {
                        if (name == classNames[0].Split('+')[0])
                            mainClassFound = true;
                    }

                    if (foundCount == classNames.Length)
                    {
                        if (classNames.Length > 1)
                            Logger.WriteLine($"[MONO] -> FindClasses(): Found all {classNames.Length} classes!");
                        else
                            Logger.WriteLine($"[MONO] -> FindClasses(): Found the class!");

                        return results;
                    }
                }

                return results;
            }
        }

        public readonly struct MonoHashTable(ulong baseAddress)
        {
            public static implicit operator ulong(MonoHashTable x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly int Size() => Memory.ReadValueUnsafe<int>(this + 0x18);
            public readonly ulong Data() => Memory.ReadPtrUnsafe(this + 0x20);
            public readonly ulong NextValue() => Memory.ReadPtrUnsafe(this + 0x108);
            public readonly uint KeyExtract() => Memory.ReadValueUnsafe<uint>(this + 0x58);

            public readonly ulong Lookup(ulong key)
            {
                var v4 = new MonoHashTable(Memory.ReadPtrUnsafe(Data() + 0x8 * (ulong)((uint)key % Size())));
                if (v4 == 0x0)
                    return default;

                while (v4.KeyExtract() != key)
                {
                    v4 = new MonoHashTable(v4.NextValue());
                    if (v4 == 0x0)
                        return default;
                }

                return v4;
            }
        }

        public readonly struct MonoImage(ulong baseAddress)
        {
            public static implicit operator ulong(MonoImage x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly int Flags() => Memory.ReadValueUnsafe<int>(this + 0x1C);

            public readonly MonoTableInfo GetTableInfo(int tableID)
            {
                if (tableID > 55) return default;

                return new MonoTableInfo(this + 0x10 * ((uint)tableID + 0xE));
            }

            public readonly Class Get(int typeID)
            {
                if ((Flags() & 0x20) != 0)
                    return default;

                if ((typeID & 0xFF000000) != 0x2000000)
                    return default;

                return new Class(new MonoHashTable(this + 0x4C0).Lookup((ulong)typeID));
            }
        }

        public readonly struct MonoAssembly(ulong baseAddress)
        {
            public static implicit operator ulong(MonoAssembly x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly MonoImage MonoImage() => new(Memory.ReadPtrUnsafe(this + 0x60));
        }
    }
}
