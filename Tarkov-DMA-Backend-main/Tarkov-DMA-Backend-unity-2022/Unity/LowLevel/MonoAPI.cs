using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.LowLevel
{
    public static class MonoAPI
    {
        public static class Singleton
        {
            public static ulong FindOne(MonoClass comfortCommonSingleton, MonoClass monoClass)
            {
                ulong[] singletons = FindManyInternal(comfortCommonSingleton,
                [
                    monoClass
                ]);

                return singletons[0];
            }

            public static ulong[] FindManyInternal(MonoClass comfortCommonSingleton, MonoClass[] classes)
            {
                ulong[] results = new ulong[classes.Length];
                
                using RemoteBytes types = new(sizeof(ulong));

                MonoRootDomain rootDomain = MonoLibrary.GetRootDomain();

                for (int i = 0; i < classes.Length; i++)
                {
                    if (!MemoryUtils.IsValidAddress(classes[i]))
                        continue;

                    ulong type = classes[i].GetType();

                    types.WriteValue(type);

                    const ulong mono_class_bind_generic_parameters = 0x23AEA0;
                    ulong generic = NativeHelper.CallFunction(Memory.MonoModuleBase + mono_class_bind_generic_parameters, comfortCommonSingleton, 1, types, XBOOL.Get(false));
                    ulong singleton = new MonoClass(generic).GetVTable(rootDomain).GetStaticFieldData();

                    results[i] = singleton;
                }

                return results;
            }
        }

        public static class MonoLibrary
        {
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

                    var buffer = Memory.ReadBuffer(addr, length).AsSpan();
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
                return new MonoRootDomain(Memory.MonoReadPtr(Memory.MonoModuleBase + 0x751020));
            }

            public static ulong FindJittedFunctionAddress(MonoMethod monoMethod)
            {
                var rootDomain = GetRootDomain();
                if (rootDomain == 0x0)
                    return 0x0;

                var jittedTable = rootDomain.JittedFunctionTable();
                if (jittedTable == 0x0)
                    return 0x0;

                int count = Memory.MonoRead<int>(jittedTable + 0x8, false);
                if (count > 20000)
                {
                    Logger.WriteLine($"[MONO] FindJittedFunctionAddress(): count is out of bounds!");
                    return 0x0;
                }

                for (int i = 0; i < count; i++)
                {
                    ulong entry = Memory.ReadPtrUnsafe(jittedTable + 0x10 + (uint)i * 0x8, false);
                    if (entry == 0x0)
                        continue;

                    int innerCount = Memory.MonoRead<int>(entry + 0x4, false);
                    if (innerCount > 20000)
                    {
                        Logger.WriteLine($"[MONO] FindJittedFunctionAddress(): innerCount is out of bounds!");
                        return 0x0;
                    }

                    for (int ii = 0; ii < innerCount; ii++)
                    {
                        ulong function = Memory.ReadPtrUnsafe(entry + 0x18 + (uint)ii * 0x8, false);
                        if (function == 0x0)
                            continue;

                        ulong monoPtr = Memory.ReadPtrUnsafe(function, false);
                        if (monoPtr == 0x0)
                            continue;

                        if ((ulong)monoMethod == monoPtr)
                        {
                            ulong jittedPtr = Memory.ReadPtrUnsafe(function + 0x10, false);
                            return jittedPtr;
                        }
                    }
                }

                return 0x0;
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

                    var dataName = ReadWideChar(Memory.MonoReadPtr(data + 0x10), 128);
                    if (dataName == name)
                        break;

                    domainAssemblies = new GSList(domainAssemblies.Next());
                    if (domainAssemblies == 0x0)
                        break;
                }

                return new MonoAssembly(data);
            }

            public static MonoClass FindClass(string assemblyName, string className, int subclass = -1)
            {
                var rootDomain = GetRootDomain();
                if (rootDomain == 0x0)
                    return default;

                var domainAssembly = DomainAssemblyOpen(rootDomain, assemblyName);
                if (domainAssembly == 0x0)
                    return default;

                var monoImage = domainAssembly.MonoImage();
                if (monoImage == 0x0)
                    return default;

                var tableInfo = monoImage.GetTableInfo(2);
                if (tableInfo == 0x0)
                    return default;

                int rowCount = tableInfo.GetRows();
                if (rowCount > 25000)
                {
                    Logger.WriteLine($"[MONO] -> FindClass(): rowCount is out of bounds!");
                    return default;
                }

                // Subclass stuff
                int currSubclassOffset = 0;
                bool mainClassFound = false;

                bool findSubclass = className.Contains('+');
                bool findSubclassAlt = subclass > -1;

                List<string> subclassParts = new();
                if (findSubclass)
                    subclassParts = className.Split('+').ToList();

                if (findSubclass && subclassParts.Count < 2)
                    Logger.WriteLine($"[MONO] -> FindClass(): Invalid subclass markup! The definition must have the main class name and at least one subclass.");

                for (int i = 0; i < rowCount; i++)
                {
                    var ptr = new MonoClass(new MonoHashTable(monoImage + 0x4D0).Lookup((ulong)(0x02000000 | i + 1)));
                    if (ptr == 0x0)
                        continue;

                    var name = ptr.Name();
                    var ns = ptr.NamespaceName();

                    if (ns.Length != 0)
                        name = ns + "." + name;

                    // Clean mangled names
                    if (name.Contains('`'))
                        name = name.Split('`')[0];

                    bool found = false;
                    if (findSubclass)
                        found = name.Equals(subclassParts[0], StringComparison.OrdinalIgnoreCase);

                    if (mainClassFound)
                    {
                        if (findSubclassAlt)
                        {
                            if (currSubclassOffset == subclass)
                                return ptr;
                        }
                        else if (findSubclass)
                        {
                            if (found && subclassParts.Count == 1) // If there is only one item left we found the target subclass!
                                return ptr;
                            else if (found) // Remove this entry since we just found it. Begin looking for the next one!
                                subclassParts.RemoveRange(0, 1);
                        }

                        currSubclassOffset++;
                    }
                    else if (name == className)
                    {
                        if (findSubclassAlt)
                            mainClassFound = true;
                        else return ptr;
                    }
                    else if (findSubclass && found)
                    {
                        mainClassFound = true;
                        subclassParts.RemoveRange(0, 1); // Remove the main class since we found it.
                    }
                }

                return default;
            }

            public static MonoClass[] FindClasses(string assemblyName, string[] classNames)
            {
                MonoClass[] results = new MonoClass[classNames.Length];
                int foundCount = 0;

                var rootDomain = GetRootDomain();
                if (rootDomain == 0x0)
                    return results;

                var domainAssembly = DomainAssemblyOpen(rootDomain, assemblyName);
                if (domainAssembly == 0x0)
                    return results;

                var monoImage = domainAssembly.MonoImage();
                if (monoImage == 0x0)
                    return results;

                var tableInfo = monoImage.GetTableInfo(2);
                if (tableInfo == 0x0)
                    return results;

                int rowCount = tableInfo.GetRows();
                if (rowCount > 25000)
                {
                    Logger.WriteLine($"[MONO] -> FindClasses(): rowCount is out of bounds!");
                    return Array.Empty<MonoClass>();
                }

                for (int i = 0; i < rowCount; i++)
                {
                    var ptr = new MonoClass(new MonoHashTable(monoImage + 0x4D0).Lookup((ulong)(0x02000000 | i + 1)));
                    if (ptr == 0x0)
                        continue;

                    var name = ptr.Name();
                    var ns = ptr.NamespaceName();

                    if (ns.Length != 0)
                        name = ns + "." + name;

                    int index = Array.IndexOf(classNames, name);
                    if (index != -1)
                    {
                        results[index] = ptr;
                        foundCount++;
                    }

                    if (foundCount == classNames.Length)
                    {
                        Logger.WriteLine($"[MONO] -> FindClasses(): Found all {classNames.Length} classes!");
                        break;
                    }
                }

                return results;
            }

            public static MonoClass FindClassInternal(MonoImage monoImage, string nameSpace, string className, RemoteBytes remoteBufferNameSpace, RemoteBytes remoteBufferName)
            {
                MonoClass[] monoClasses = FindClassesInternal(monoImage,
                [
                    new(nameSpace, className)
                ], remoteBufferNameSpace, remoteBufferName);

                return monoClasses[0];
            }

            public readonly struct FindClassInternalData(string nameSpace, string className)
            {
                public readonly string NameSpace = nameSpace;
                /// <summary>
                /// Must include type argument count if applicable (ex. Singleton`1).
                /// </summary>
                public readonly string ClassName = className;
            }

            public static MonoClass[] FindClassesInternal(MonoImage monoImage, FindClassInternalData[] fcData, RemoteBytes remoteBufferNameSpace, RemoteBytes remoteBufferName)
            {
                MonoClass[] results = new MonoClass[fcData.Length];

                for (int i = 0; i < fcData.Length; i++)
                {
                    FindClassInternalData data = fcData[i];

                    try
                    {
                        using RemoteCharPointer pNameSpace = new(data.NameSpace, remoteBufferNameSpace);
                        using RemoteCharPointer pName = new(data.ClassName, remoteBufferName);

                        const ulong mono_class_from_name = 0xC02F0;
                        ulong fClass = NativeHelper.CallFunction(Memory.MonoModuleBase + mono_class_from_name, monoImage, pNameSpace, pName);

                        results[i] = new(fClass);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteLine($"[MONO] -> FindClassesInternal(): NameSpace: {data.NameSpace} Name: {data.ClassName} ~ {ex}");
                    }
                }

                return results;
            }

            public static MonoClass FindClassByToken(MonoImage monoImage, uint token)
            {
                MonoClass[] fClasses = FindClassesByTokens(monoImage,
                [
                    token
                ]);

                return fClasses[0];
            }

            public static MonoClass[] FindClassesByTokens(MonoImage monoImage, uint[] tokens)
            {
                MonoClass[] results = new MonoClass[tokens.Length];

                for (int i = 0; i < tokens.Length; i++)
                {
                    uint token = tokens[i];

                    const ulong mono_class_get = 0xBE830;
                    ulong fClass = NativeHelper.CallFunction(Memory.MonoModuleBase + mono_class_get, monoImage, token);

                    results[i] = new(fClass);
                }

                return results;
            }
        }

        public readonly struct GSList
        {
            public static implicit operator ulong(GSList x) => x.Base;
            private readonly ulong Base;

            public GSList(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly ulong Data() => Memory.MonoReadPtr(this + 0x0);
            public readonly ulong Next() => Memory.MonoReadPtr(this + 0x8);
        }

        public readonly struct MonoRootDomain
        {
            public static implicit operator ulong(MonoRootDomain x) => x.Base;
            private readonly ulong Base;

            public MonoRootDomain(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly GSList DomainAssemblies() => new(Memory.MonoReadPtr(this + 0xA0));
            public readonly int DomainID() => Memory.MonoRead<int>(this + 0x94);
            public readonly ulong JittedFunctionTable() => Memory.MonoReadPtr(this + 0x120);
        }

        public readonly struct MonoTableInfo
        {
            public static implicit operator ulong(MonoTableInfo x) => x.Base;
            private readonly ulong Base;

            public MonoTableInfo(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly int GetRows() => Memory.MonoRead<int>(this + 0x8) & 0xFFFFFF;
        }

        public readonly struct MonoMethod
        {
            public static implicit operator ulong(MonoMethod x) => x.Base;
            private readonly ulong Base;

            public readonly int GetParamCount() => NativeHelper.GetMonoMethodParamCount(Base);

            public MonoMethod(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly string Name()
            {
                var address = Memory.MonoReadPtr(this + 0x18);
                return MonoLibrary.ReadName(address, 128);
            }
        }

        public readonly struct MonoClassField
        {
            public static implicit operator ulong(MonoClassField x) => x.Base;
            private readonly ulong Base;

            public MonoClassField(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly string Name()
            {
                var address = Memory.ReadValue<ulong>(this + 0x8);
                return MonoLibrary.ReadName(address, 128);
            }

            public readonly int Offset()
            {
                return Memory.ReadValue<int>(this + 0x18);
            }
        }

        public readonly struct MonoClassRuntimeInfo
        {
            public static implicit operator ulong(MonoClassRuntimeInfo x) => x.Base;
            private readonly ulong Base;

            public MonoClassRuntimeInfo(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly int MaxDomain() => Memory.MonoRead<ushort>(this + 0x0);
        }

        public readonly struct MonoVTable
        {
            public static implicit operator ulong(MonoVTable x) => x.Base;
            private readonly ulong Base;

            public MonoVTable(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly ulong GetClassType() => Memory.MonoRead<ulong>(this + 0x18);

            public readonly byte Flags() => Memory.MonoRead<byte>(this + 0x30);

            public readonly ulong GetStaticFieldData()
            {
                if ((Flags() & 4) != 0)
                    return Memory.MonoReadPtr(this + 0x48 + 8 * (uint)Memory.MonoRead<int>(Memory.MonoReadPtr(this + 0x0) + 0x5C));

                return 0x0;
            }
        }

        public readonly struct MonoClass(ulong baseAddress)
        {
            public static implicit operator ulong(MonoClass x) => x.Base;
            private readonly ulong Base = baseAddress;

            public readonly int NumFields() => Memory.MonoRead<int>(this + 0x100);
            public readonly MonoClassRuntimeInfo RuntimeInfo() => new(Memory.MonoReadPtr(this + 0xD0));

            public readonly string Name()
            {
                ulong address = Memory.ReadPtrUnsafe(this + 0x48);
                if (address == 0x0)
                    return string.Empty;

                return MonoLibrary.ReadName(address, 128);
            }

            public readonly string NamespaceName()
            {
                ulong address = Memory.ReadPtrUnsafe(this + 0x50);
                if (address == 0x0)
                    return string.Empty;

                return MonoLibrary.ReadName(address, 128);
            }

            private static uint HIDWORD(ulong value)
            {
                return (uint)((value >> 32) & 0xFFFFFFFF);
            }

            private static uint LODWORD(ulong value)
            {
                return (uint)(value & 0xFFFFFFFF);
            }

            public readonly uint GetNumMethods()
            {
                var v2 = Memory.MonoRead<byte>(this + 0x1B) - 1;
                switch (v2)
                {
                    case 0:
                    case 1:
                        return HIDWORD(Memory.MonoRead<ulong>(this + 0xF8)); // element_class[1].cast_class -> (sizeof(_MonoClass) + 0x8)
                    case 3:
                    case 5:
                        return 0;
                    case 4:
                        return LODWORD(Memory.MonoRead<ulong>(this + 0xF8)); // element_class[1].cast_class -> (sizeof(_MonoClass) + 0x8)
                    default: break;
                }

                return 0;
            }

            public readonly MonoMethod GetMethod(int i) => new(Memory.MonoReadPtr(Memory.MonoReadPtr(this + 0xA0) + 0x8 * (uint)i));

            public readonly MonoClassField GetField(int i)
            {
                var fieldsPtr = Memory.ReadValue<ulong>(this + 0x98);
                return new MonoClassField(fieldsPtr + (ulong)(0x20 * i));
            }

            public readonly MonoVTable GetVTable(MonoRootDomain domain)
            {
                var runtimeInfo = new MonoClassRuntimeInfo(RuntimeInfo());
                if (runtimeInfo == 0x0)
                    return default;

                var domainID = domain.DomainID();
                if (runtimeInfo.MaxDomain() < domainID)
                    return default;

                return new MonoVTable(Memory.MonoReadPtr(runtimeInfo + 8 * (uint)domainID + 8));
            }

            public new readonly ulong GetType() => this + 0xB8;

            public readonly MonoMethod FindMethod(string methodName, int paramCount = -1)
            {
                ulong monoPtr = 0x0;

                uint methodCount = GetNumMethods();
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
                    {
                        if (paramCount != -1)
                        {
                            if (method.GetParamCount() == paramCount)
                                monoPtr = method;
                        }
                        else
                            monoPtr = method;
                    }
                }

                return new MonoMethod(monoPtr);
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
        }

        public readonly struct MonoHashTable
        {
            public static implicit operator ulong(MonoHashTable x) => x.Base;
            private readonly ulong Base;

            public MonoHashTable(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly int Size() => Memory.MonoRead<int>(this + 0x18);
            public readonly ulong Data() => Memory.MonoReadPtr(this + 0x20);
            public readonly ulong NextValue() => Memory.MonoReadPtr(this + 0x108);
            public readonly uint KeyExtract() => Memory.MonoRead<uint>(this + 0x58);

            public readonly ulong Lookup(ulong key)
            {
                var v4 = new MonoHashTable(Memory.MonoReadPtr(Data() + 0x8 * (ulong)((uint)key % Size())));
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

        public readonly struct MonoImage
        {
            public static implicit operator ulong(MonoImage x) => x.Base;
            private readonly ulong Base;

            public MonoImage(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly int Flags() => Memory.MonoRead<int>(this + 0x1C);

            public readonly MonoTableInfo GetTableInfo(int tableID)
            {
                if (tableID > 55) return default;

                return new MonoTableInfo(this + 0xF0 + ((uint)tableID * 0x10));
            }

            public readonly MonoClass Get(int typeID)
            {
                if ((Flags() & 0x20) != 0)
                    return default;

                if ((typeID & 0xFF000000) != 0x2000000)
                    return default;

                return new MonoClass(new MonoHashTable(this + 0x4C0).Lookup((ulong)typeID));
            }

            public readonly byte[] GetImage()
            {
                try
                {
                    var rawDataLength = Memory.ReadValue<uint>(this + 0x18);

                    ulong rawData = Memory.ReadPtr(this + 0x10);

                    byte[] imageBytes = Memory.ReadBufferEnsure(rawData, (int)rawDataLength);

                    return imageBytes;
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MONO] -> GetImage(): Failed to get image ~ {ex}");
                    return null;
                }
            }
        }

        public readonly struct MonoAssembly
        {
            public static implicit operator ulong(MonoAssembly x) => x.Base;
            private readonly ulong Base;

            public MonoAssembly(ulong baseAddress)
            {
                Base = baseAddress;
            }

            public readonly ulong pMonoImage() => this + 0x60;

            public readonly MonoImage MonoImage() => new(Memory.MonoReadPtr(pMonoImage()));
        }
    }
}
