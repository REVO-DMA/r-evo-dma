using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Tarkov;
using Tarkov_DMA_Backend.Tarkov.Toolkit;
using Tarkov_DMA_Backend.Unity.LowLevel;
using Tarkov_DMA_Backend.Unity.LowLevel.PersistentCache;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;

namespace Tarkov_DMA_Backend.Unity
{
    public static class ChamsManager
    {
        private const string AssetBundleShaderName = "visibilitycheck.shader";
        private const string ShaderProperty_VisibleColor = "_ColorVisible";
        private const string ShaderProperty_InvisibleColor = "_ColorInvisible";

        private static readonly byte[] AssetBundleBytes = File.ReadAllBytes("visibilitycheck.bundle");

        public static ChamsLoadStatus ChamsStatus = ChamsLoadStatus.NotLoaded;

        public enum ChamsLoadStatus
        {
            SkippedLoading,
            NotLoaded,
            FullyLoaded,
            FailedToLoad,
        }

        private static readonly HashSet<PlayerType> ChamsPlayerTypes =
        [
            PlayerType.EnemyPMC,
            PlayerType.Teammate,
            PlayerType.Boss,
            PlayerType.PlayerScav,
            PlayerType.Scav,
            PlayerType.Default,
            PlayerType.AimbotLocked,
            PlayerType.Corpse,
        ];

        private static ConcurrentDictionary<PlayerType, ChamsMaterial> ChamsMaterials = new();

        public static ChamsLoadStatus Initialize()
        {
            // Try to populate dict from the persistent cache
            Cache.LoadChams();

            // If chams are disabled or vis check is not active skip initialization
            if (!ToolkitManager.FeatureState["chams"] || ToolkitManager.FeatureSettings_int["chams_Mode"] != 3)
                return ChamsLoadStatus.SkippedLoading;

            // See if this is already loaded
            bool fullyLoaded = true;
            foreach (var type in ChamsPlayerTypes)
            {
                if (!ChamsMaterials.ContainsKey(type))
                {
                    fullyLoaded = false;
                    break;
                }
            }

            if (fullyLoaded)
                return ChamsLoadStatus.FullyLoaded;
            else
                ChamsMaterials.Clear();

            try
            {
                ulong monoDomain = (ulong)MonoLibrary.GetRootDomain();
                if (monoDomain == 0x0)
                    throw new Exception("Failed to get mono domain!");

                ulong shaderType = EFTDMA.MonoClasses.GetClass("UnityEngine.Shader").GetType();

                ulong shaderTypeObject = NativeHelper.GetTypeObject(monoDomain, shaderType);
                if (shaderTypeObject == 0x0)
                    throw new Exception("Failed to get UnityEngine.Shader Type Object!");

                ulong materialClass = EFTDMA.MonoClasses.GetClass("UnityEngine.Material");
                if (materialClass == 0x0)
                    throw new Exception("Failed to get UnityEngine.Material class!");

                var assetBundleMonoByteArr = RemoteBytes.MonoByteArray.Get(AssetBundleBytes);
                using RemoteBytes assetBundleMem = new(assetBundleMonoByteArr.GetSizeU());
                assetBundleMem.WriteBytes(assetBundleMonoByteArr);

                var assetBundleMonoStr = RemoteBytes.MonoString.Get(AssetBundleShaderName);
                using RemoteBytes shaderNameMem = new(assetBundleMonoStr.GetSizeU());
                shaderNameMem.WriteString(assetBundleMonoStr);

                if (!NativeHelper.LoadAssetBundle((ulong)assetBundleMem, (ulong)shaderNameMem, shaderTypeObject))
                    throw new Exception("Failed to load the asset bundle!");

                var shaderProperty_VisibleColorMonoStr = RemoteBytes.MonoString.Get(ShaderProperty_VisibleColor);
                using RemoteBytes shaderProperty_VisibleColorMem = new(shaderProperty_VisibleColorMonoStr.GetSizeU());
                shaderProperty_VisibleColorMem.WriteString(shaderProperty_VisibleColorMonoStr);

                var shaderProperty_InvisibleColorMonoStr = RemoteBytes.MonoString.Get(ShaderProperty_InvisibleColor);
                using RemoteBytes shaderProperty_InvisibleColorMem = new(shaderProperty_InvisibleColorMonoStr.GetSizeU());
                shaderProperty_InvisibleColorMem.WriteString(shaderProperty_InvisibleColorMonoStr);

                using RemoteBytes chamsColorMem = new(UnityColor.GetSizeU());

                foreach (PlayerType playerType in ChamsPlayerTypes)
                {
                    var tmpMaterial = CreateChamsMaterial(playerType, monoDomain, materialClass, (ulong)shaderProperty_InvisibleColorMem, (ulong)shaderProperty_VisibleColorMem);
                    if (tmpMaterial == null)
                        throw new Exception("Failed to create chams material!");

                    ChamsMaterial material = (ChamsMaterial)tmpMaterial;

                    if (!GetChamsColorsFromPlayerType(playerType, out var visibleColor, out var invisibleColor))
                    {
                        Logger.WriteLine($"Skipping chams for player type: \"{playerType}\" due to being unable to get the chams color!");
                        continue;
                    }

                    NativeHelper.SetMaterialColor(chamsColorMem, material.Address, material._ColorVisible, visibleColor);
                    NativeHelper.SetMaterialColor(chamsColorMem, material.Address, material._ColorInvisible, invisibleColor);

                    ChamsMaterials.AddOrUpdate(playerType, (newItem) => material, (key, existing) => material);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Error initializing chams {ex}");
                SentrySdk.CaptureException(ex);
                return ChamsLoadStatus.FailedToLoad;
            }
            finally
            {
                NativeHelper.UnloadAssetBundle();
            }

            Cache.SaveChams(ChamsMaterials);

            return ChamsLoadStatus.FullyLoaded;
        }

        private static ChamsMaterial? CreateChamsMaterial(PlayerType playerType, ulong monoDomain, ulong materialClass, ulong invisibleColorMem, ulong visibleColorMem)
        {
            ulong materialAddress = NativeHelper.CreateMaterialFromShader(monoDomain, materialClass);
            if (materialAddress == 0x0)
            {
                Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Failed to create the material for {playerType} from the loaded shader!");
                return null;
            }

            int instanceID = int.MaxValue;

            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < 5000)
            {
                ulong materialInstance = Memory.ReadPtrUnsafe(materialAddress + UnityOffsets.Material.Instance, false);
                if (materialInstance == 0x0)
                {
                    Thread.Sleep(10);
                    continue;
                }

                var tmpInstanceID = Memory.ReadValueEnsure<int>(materialInstance + UnityOffsets.UnityObject.InstanceID);
                // The instance ID of an object acts like a handle to the in-memory instance. It is always unique, and never has the value 0.
                // Objects loaded from file will be assigned a positive Instance ID. Newly created objects will have a negative Instance ID.
                // https://docs.unity3d.com/ScriptReference/Object.GetInstanceID.html
                if (tmpInstanceID == null || (int)tmpInstanceID >= 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                else
                {
                    instanceID = (int)tmpInstanceID;
                    Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Got material instance for {playerType}: \"{instanceID}\"!");
                }

                break;
            }

            if (instanceID == int.MaxValue)
            {
                Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Failed to get the material instance for {playerType} from the created material!");
                return null;
            }

            return new()
            {
                Address = materialAddress,
                InstanceID = instanceID,
                _ColorInvisible = NativeHelper.ShaderPropertyToID(invisibleColorMem),
                _ColorVisible = NativeHelper.ShaderPropertyToID(visibleColorMem)
            };
        }

        private static bool GetChamsColorsFromPlayerType(PlayerType playerType, out UnityColor visibleColor, out UnityColor invisibleColor)
        {
            visibleColor = default;
            invisibleColor = default;

            if (playerType == PlayerType.AimbotLocked)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_AimbotLocked_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_AimbotLocked_invisible"]);
            }
            else if (playerType == PlayerType.EnemyPMC)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_EnemyPMC_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_EnemyPMC_invisible"]);
            }
            else if (playerType == PlayerType.Teammate)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_Teammate_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_Teammate_invisible"]);
            }
            else if (playerType == PlayerType.Boss)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_ScavBoss_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_ScavBoss_invisible"]);
            }
            else if (playerType == PlayerType.PlayerScav)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_PlayerScav_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_PlayerScav_invisible"]);
            }
            else if (playerType == PlayerType.Scav)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_Scav_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_Scav_invisible"]);
            }
            else if (playerType == PlayerType.Default)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_Default_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_Default_invisible"]);
            }
            else if (playerType == PlayerType.Corpse)
            {
                visibleColor = new(ToolkitManager.ChamsColors["chams_Corpse_visible"]);
                invisibleColor = new(ToolkitManager.ChamsColors["chams_Corpse_invisible"]);
            }
            else return false;

            return true;
        }

        public static ChamsMaterial? GetMaterialForPlayerType(PlayerType? playerType)
        {
            if (ChamsMaterials.Count == 0 || playerType == null)
                return null;

            if (!ChamsMaterials.TryGetValue((PlayerType)playerType, out var material))
                ChamsMaterials.TryGetValue(PlayerType.Default, out material); // Fall back to the default

            return material;
        }

        public static void SyncRemoteChamsColors(PlayerType[] playerTypes)
        {
            if (ChamsMaterials.IsEmpty || playerTypes.Length == 0)
                return;

            using RemoteBytes chamsColorMem = new(16);

            foreach (var playerType in playerTypes)
            {
                if (!ChamsMaterials.TryGetValue(playerType, out var material))
                {
                    Logger.WriteLine($"Skipping sync remote chams colors for player type: \"{playerType}\" due to being unable to get the chams material!");
                    continue;
                }

                if (!GetChamsColorsFromPlayerType(playerType, out var visibleColor, out var invisibleColor))
                {
                    Logger.WriteLine($"Skipping sync remote chams colors for player type: \"{playerType}\" due to being unable to get the chams color!");
                    continue;
                }

                NativeHelper.SetMaterialColor(chamsColorMem, material.Address, material._ColorVisible, visibleColor);
                NativeHelper.SetMaterialColor(chamsColorMem, material.Address, material._ColorInvisible, invisibleColor);
            }
        }

        public static void LoadChamsMaterialFromPersistentCache(PlayerType playerType, ChamsMaterial chamsMaterial)
        {
            if (ChamsMaterials.TryAdd(playerType, chamsMaterial))
                Logger.WriteLine($"[CHAMS MANAGER]: From Cache -> Got material instance for {playerType}: \"{chamsMaterial.InstanceID}\"!");
        }

        public static void Reset()
        {
            ChamsStatus = ChamsLoadStatus.NotLoaded;
            ChamsMaterials.Clear();
        }
    }
}
