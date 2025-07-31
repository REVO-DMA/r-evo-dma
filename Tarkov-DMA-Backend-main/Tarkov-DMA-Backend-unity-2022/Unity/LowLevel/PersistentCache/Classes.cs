using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.LowLevel.PersistentCache
{
    public sealed class Classes : IDisposable
    {
        public readonly ConcurrentDictionary<ClassesEntry, ulong> ResolvedStaticClasses = new();
        public readonly ConcurrentDictionary<ClassesEntry, ulong> ResolvedClasses = new();
        public readonly ConcurrentDictionary<ClassesEntry, ulong> ResolvedSingletons = new();

        private readonly RemoteBytes _remoteBufferNameSpace;
        private readonly RemoteBytes _remoteBufferName;

        public enum FindType
        {
            Name,
            Token,
        }

        public enum ResolveType
        {
            StaticClass,
            Class,
            Singleton,
            Method,
        }

        private static readonly List<ClassesEntry> SearchEntries = new()
        {
            // Static Classes
            new(new(FindType.Name, ResolveType.StaticClass), "Assembly-CSharp", "", "EFTHardSettings"),
            new(new(FindType.Name, ResolveType.StaticClass), "Assembly-CSharp", "EFT", "GameWorld"),
            new(new(FindType.Name, ResolveType.StaticClass), "Assembly-CSharp", "EFT.Weather", "WeatherController", canFail: true),
            new(new(FindType.Name, ResolveType.StaticClass), "Assembly-CSharp", "", "MineDirectional", canFail: true),
            new(new(FindType.Name, ResolveType.StaticClass), "Assembly-CSharp", "GPUInstancer", "GPUInstancerManager", canFail: true),
            new(new(FindType.Token, ResolveType.StaticClass), "Assembly-CSharp", classToken: ClassNames.ScreenManager.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.StaticClass), "Assembly-CSharp", classToken: ClassNames.OpticCameraManagerContainer.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.StaticClass), "Assembly-CSharp", classToken: ClassNames.LocaleManager.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.StaticClass), "Assembly-CSharp", classToken: ClassNames.LayerManager.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.StaticClass), "Assembly-CSharp", classToken: ClassNames.BallisticLayerManager.ClassName_ClassToken),

            // Classes
            new(new(FindType.Name, ResolveType.Class), "Comfort", "Comfort.Common", "Singleton`1", canSkip: false),
            new(new(FindType.Name, ResolveType.Class), "Assembly-CSharp", "EFT", "GameWorld"),
            new(new(FindType.Name, ResolveType.Class), "UnityEngine.PhysicsModule", "UnityEngine", "Physics"),
            new(new(FindType.Name, ResolveType.Class), "Assembly-CSharp", "EFT", "Player"),
            new(new(FindType.Name, ResolveType.Class), "UnityEngine.CoreModule", "UnityEngine", "Shader"),
            new(new(FindType.Name, ResolveType.Class), "UnityEngine.CoreModule", "UnityEngine", "Material"),
            new(new(FindType.Name, ResolveType.Class), "UnityEngine.CoreModule", "UnityEngine", "QualitySettings"),
            new(new(FindType.Token, ResolveType.Class), "Assembly-CSharp", classToken: ClassNames.EquipmentPenaltyComponent.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.Class), "Assembly-CSharp", classToken: ClassNames.FixWildSpawnType.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.Class), "Assembly-CSharp", classToken: ClassNames.FirearmController.ClassName_ClassToken),

            // Singletons
            new(new(FindType.Name, ResolveType.Singleton), "Assembly-CSharp", "EFT", "GameWorld", canFail: true),
            new(new(FindType.Name, ResolveType.Singleton), "Assembly-CSharp", "", "LevelSettings"),
            new(new(FindType.Token, ResolveType.Singleton), "Assembly-CSharp", classToken: ClassNames.InertiaSettings.ClassName_ClassToken),
            new(new(FindType.Token, ResolveType.Singleton), "Assembly-CSharp", classToken: ClassNames.GameSettings.ClassName_ClassToken),
        };

        public Classes()
        {
            _remoteBufferNameSpace = new(1024);
            _remoteBufferName = new(1024);
        }

        public void Dispose()
        {
            _remoteBufferNameSpace?.Dispose();
            _remoteBufferName?.Dispose();
            Reset();
        }

        public bool IsInitialized()
        {
            foreach (var entry in SearchEntries)
            {
                if (entry.CanFail)
                    continue;

                if (!ResolvedStaticClasses.ContainsKey(entry) &&
                    !ResolvedClasses.ContainsKey(entry) &&
                    !ResolvedSingletons.ContainsKey(entry))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsFullyInitialized()
        {
            int foundCount = ResolvedStaticClasses.Count + ResolvedClasses.Count + ResolvedSingletons.Count;

            if (SearchEntries.Count == foundCount)
                return true;

            return false;
        }

        public ulong GetStaticClass(string className)
        {
            var entries = SearchEntries.Where(x => x.GetFullClassName() == className);
            if (!entries.Any())
                throw new Exception($"There is no Entry in SearchEntries with class name \"{className}\"");

            ClassesEntry entry = entries.First();

            foreach (var sc in ResolvedStaticClasses)
            {
                if (sc.Key.SearchInfo.FT != FindType.Name)
                    continue;

                string fullName = "";
                if (!string.IsNullOrEmpty(sc.Key.NameSpace))
                    fullName += $"{sc.Key.NameSpace}.";

                fullName += sc.Key.ClassName;

                if (className == fullName)
                    return sc.Value;
            }

            throw new Exception($"Unable to find static class via class name \"{className}\" ~ {entry}");
        }

        public ulong GetStaticClass(uint token)
        {
            var entries = SearchEntries.Where(x => x.ClassToken == token);
            if (!entries.Any())
                throw new Exception($"There is no Entry in SearchEntries with token \"0x{token:X}\"");

            ClassesEntry entry = entries.First();

            foreach (var sc in ResolvedStaticClasses)
            {
                if (sc.Key.SearchInfo.FT != FindType.Token)
                    continue;

                if (token == sc.Key.ClassToken)
                    return sc.Value;
            }

            throw new Exception($"Unable to find static class via token \"0x{token:X}\" ~ {entry}");
        }

        public MonoAPI.MonoClass GetClass(string className)
        {
            var entries = SearchEntries.Where(x => x.GetFullClassName() == className);
            if (!entries.Any())
                throw new Exception($"There is no Entry in SearchEntries with class name \"{className}\"");

            ClassesEntry entry = entries.First();

            foreach (var sc in ResolvedClasses)
            {
                if (sc.Key.SearchInfo.FT != FindType.Name)
                    continue;

                string fullName = "";
                if (!string.IsNullOrEmpty(sc.Key.NameSpace))
                    fullName += $"{sc.Key.NameSpace}.";

                fullName += sc.Key.ClassName;

                if (className == fullName)
                    return new(sc.Value);
            }

            throw new Exception($"Unable to find class via class name \"{className}\" ~ {entry}");
        }

        public MonoAPI.MonoClass GetClass(uint token)
        {
            var entries = SearchEntries.Where(x => x.ClassToken == token);
            if (!entries.Any())
                throw new Exception($"There is no Entry in SearchEntries with token \"0x{token:X}\"");

            ClassesEntry entry = entries.First();

            foreach (var sc in ResolvedClasses)
            {
                if (sc.Key.SearchInfo.FT != FindType.Token)
                    continue;

                if (token == sc.Key.ClassToken)
                    return new(sc.Value);
            }

            throw new Exception($"Unable to find class via token \"0x{token:X}\" ~ {entry}");
        }

        public ulong GetSingleton(string className)
        {
            var entries = SearchEntries.Where(x => x.GetFullClassName() == className);
            if (!entries.Any())
                throw new Exception($"There is no Entry in SearchEntries with class name \"{className}\"");

            ClassesEntry entry = entries.First();

            foreach (var sc in ResolvedSingletons)
            {
                if (sc.Key.SearchInfo.FT != FindType.Name)
                    continue;

                string fullName = "";
                if (!string.IsNullOrEmpty(sc.Key.NameSpace))
                    fullName += $"{sc.Key.NameSpace}.";

                fullName += sc.Key.ClassName;
                if (className == fullName)
                    return sc.Value;
            }

            throw new Exception($"Unable to find singleton via class name \"{className}\" ~ {entry}");
        }

        public ulong GetSingleton(uint token)
        {
            var entries = SearchEntries.Where(x => x.ClassToken == token);
            if (!entries.Any())
                throw new Exception($"There is no Entry in SearchEntries with token \"0x{token:X}\"");

            ClassesEntry entry = entries.First();

            foreach (var sc in ResolvedSingletons)
            {
                if (sc.Key.SearchInfo.FT != FindType.Token)
                    continue;

                if (token == sc.Key.ClassToken)
                    return sc.Value;
            }

            throw new Exception($"Unable to find singleton via token \"0x{token:X}\" ~ {entry}");
        }

        public bool Refresh()
        {
            return ProcessEntries();
        }
        
        public void Reset()
        {
            ResolvedStaticClasses.Clear();
            ResolvedClasses.Clear();
            ResolvedSingletons.Clear();
        }

        private static bool ShouldSkipEntry(ClassesEntry entry, Classes classes)
        {
            if (!entry.CanSkip)
                return false;

            if (entry.SearchInfo.RT == ResolveType.StaticClass)
            {
                if (classes.ResolvedStaticClasses.ContainsKey(entry))
                    return true;
            }
            else if (entry.SearchInfo.RT == ResolveType.Class)
            {
                if (classes.ResolvedClasses.ContainsKey(entry))
                    return true;
            }
            else if (entry.SearchInfo.RT == ResolveType.Singleton)
            {
                if (classes.ResolvedSingletons.ContainsKey(entry))
                    return true;
            }

            return false;
        }

        private bool ProcessEntries()
        {
            if (IsFullyInitialized())
                return true;

            // Try to populate dictionaries from the persistent cache
            Cache.LoadClasses(this);

            MonoAPI.MonoRootDomain rootDomain = MonoAPI.MonoLibrary.GetRootDomain();
            if (rootDomain == 0x0)
            {
                Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Failed to get mono root domain.");
                return false;
            }

            Dictionary<string, MonoAPI.MonoImage> assemblies = GetAssemblies(rootDomain);
            if (assemblies == null)
            {
                Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Failed to resolve all of the required mono images.");
                return false;
            }

            assemblies.TryGetValue("Comfort", out MonoAPI.MonoImage comfortMonoImage);
            MonoAPI.MonoClass singletonClass = MonoAPI.MonoLibrary.FindClassInternal(comfortMonoImage, "Comfort.Common", "Singleton`1", _remoteBufferNameSpace, _remoteBufferName);
            if (!MemoryUtils.IsValidAddress(singletonClass))
            {
                Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Failed to get the Comfort.Common.Singleton class.");
                return false;
            }

            foreach (ClassesEntry entry in SearchEntries)
            {
                if (ShouldSkipEntry(entry, this))
                    continue;

                if (!assemblies.TryGetValue(entry.AssemblyName, out MonoAPI.MonoImage monoImage))
                {
                    Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Failed to resolve MonoClass because assembly: \"{entry.AssemblyName}\" was not found ~ {entry}");
                    continue;
                }

                // Resolve the associated MonoClass
                MonoAPI.MonoClass monoClass;
                if (entry.SearchInfo.FT == FindType.Name)
                    monoClass = MonoAPI.MonoLibrary.FindClassInternal(monoImage, entry.NameSpace, entry.ClassName, _remoteBufferNameSpace, _remoteBufferName);
                else if (entry.SearchInfo.FT == FindType.Token)
                    monoClass = MonoAPI.MonoLibrary.FindClassByToken(monoImage, entry.ClassToken);
                else
                {
                    Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Unhandled FindType: \"{entry.SearchInfo.FT}\"");
                    continue;
                }

                if (!MemoryUtils.IsValidAddress(monoClass))
                {
                    Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Invalid MonoClass found by: {entry}");
                    continue;
                }

                // Perform final resolution and/or save
                if (entry.SearchInfo.RT == ResolveType.StaticClass)
                {
                    ulong staticClass = monoClass.GetVTable(rootDomain).GetStaticFieldData();
                    if (!MemoryUtils.IsValidAddress(staticClass))
                    {
                        Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Invalid Static Class found by MonoClass @ 0x{(ulong)monoClass:X} ~ MonoClass found by: {entry}");
                        continue;
                    }

                    ResolvedStaticClasses.TryAdd(entry, staticClass);

                    Logger.WriteLine($"[CLASSES] -> Found Static Class @ 0x{(ulong)monoClass:X} ~ {entry}");
                }
                else if (entry.SearchInfo.RT == ResolveType.Class)
                {
                    ResolvedClasses.TryAdd(entry, monoClass);

                    Logger.WriteLine($"[CLASSES] -> Found Class @ 0x{(ulong)monoClass:X} ~ {entry}");
                }
                else if (entry.SearchInfo.RT == ResolveType.Singleton)
                {
                    ulong singleton = MonoAPI.Singleton.FindOne(singletonClass, monoClass);
                    if (!MemoryUtils.IsValidAddress(singleton))
                    {
                        Logger.WriteLine($"[CLASSES] -> ProcessEntries(): Invalid Singleton found by MonoClass @ 0x{monoClass:X} ~ MonoClass found by: {entry}");
                        continue;
                    }

                    ResolvedSingletons.TryAdd(entry, singleton);

                    Logger.WriteLine($"[CLASSES] -> Found Singleton Class @ 0x{singleton:X} ~ {entry}");
                }
            }

            Cache.SaveClasses(this);

            return true;
        }

        private Dictionary<string, MonoAPI.MonoImage> GetAssemblies(MonoAPI.MonoRootDomain rootDomain)
        {
            HashSet<string> assemblyNames = new();
            foreach (ClassesEntry entry in SearchEntries)
            {
                if (ShouldSkipEntry(entry, this))
                    continue;

                assemblyNames.Add(entry.AssemblyName);
            }

            Dictionary<string, MonoAPI.MonoImage> assemblies = new();
            foreach (string assemblyName in assemblyNames)
            {
                var domainAssembly = MonoAPI.MonoLibrary.DomainAssemblyOpen(rootDomain, assemblyName);
                if (domainAssembly == 0x0)
                    return null;

                var monoImage = domainAssembly.MonoImage();
                if (monoImage == 0x0 || !MemoryUtils.IsValidAddress(monoImage))
                    return null;

                assemblies.Add(assemblyName, monoImage);
            }

            return assemblies;
        }
    }
}
