using arena_dma_backend.Mono;
using static arena_dma_backend.Arena.Misc;

namespace arena_dma_backend.Arena
{
    public static class ChamsManager
    {
        private const string AssetBundleShaderName = "visibilitycheck.shader";
        private const string ShaderProperty_VisibleColor = "_ColorVisible";
        private const string ShaderProperty_InvisibleColor = "_ColorInvisible";

        private static readonly byte[] AssetBundleBytes = File.ReadAllBytes("visibilitycheck.bundle");

        public struct ChamsMaterial
        {
            public ulong Address;
            public int InstanceID;
            public int _ColorVisible;
            public int _ColorInvisible;
        }

        public static ChamsMaterial TeammateMaterial { get; private set; }
        public static ChamsMaterial EnemyMaterial { get; private set; }
        public static ChamsMaterial CorpseMaterial { get; private set; }
        public static ChamsMaterial AimbotMaterial { get; private set; }

        public static bool Initialize()
        {
            try
            {
                ulong monoDomain = (ulong)MonoAPI.GetRootDomain();
                if (monoDomain == 0x0)
                    throw new Exception("Failed to get mono domain!");

                ulong shaderType = MonoAPI.Class.FindOne("UnityEngine.CoreModule", "UnityEngine.Shader").GetType();
                if (shaderType == 0x0)
                    throw new Exception("Failed to find UnityEngine.Shader type!");

                ulong shaderTypeObject = NativeHelper.GetTypeObject(monoDomain, shaderType);
                if (shaderTypeObject == 0x0)
                    throw new Exception("Failed to get UnityEngine.Shader Type Object!");

                ulong materialClass = (ulong)MonoAPI.Class.FindOne("UnityEngine.CoreModule", "UnityEngine.Material");
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

                // Create Teammate chams
                var tmpTeammateMaterial = CreateChamsMaterial(monoDomain, materialClass, (ulong)shaderProperty_InvisibleColorMem, (ulong)shaderProperty_VisibleColorMem);
                if (tmpTeammateMaterial == null) return false;

                ChamsMaterial teammateMaterial = (ChamsMaterial)tmpTeammateMaterial;
                NativeHelper.SetMaterialColor(chamsColorMem, teammateMaterial.Address, teammateMaterial._ColorVisible, new("#00ff00"));
                NativeHelper.SetMaterialColor(chamsColorMem, teammateMaterial.Address, teammateMaterial._ColorInvisible, new("#00ff00"));

                TeammateMaterial = teammateMaterial;

                // Create Enemy chams
                var tmpEnemyMaterial = CreateChamsMaterial(monoDomain, materialClass, (ulong)shaderProperty_InvisibleColorMem, (ulong)shaderProperty_VisibleColorMem);
                if (tmpEnemyMaterial == null) return false;

                ChamsMaterial enemyMaterial = (ChamsMaterial)tmpEnemyMaterial;
                NativeHelper.SetMaterialColor(chamsColorMem, enemyMaterial.Address, enemyMaterial._ColorVisible, new("#ff0000"));
                NativeHelper.SetMaterialColor(chamsColorMem, enemyMaterial.Address, enemyMaterial._ColorInvisible, new("#ff9b9b"));
                
                EnemyMaterial = enemyMaterial;

                // Create Corpse chams
                var tmpCorpseMaterial = CreateChamsMaterial(monoDomain, materialClass, (ulong)shaderProperty_InvisibleColorMem, (ulong)shaderProperty_VisibleColorMem);
                if (tmpCorpseMaterial == null) return false;

                ChamsMaterial corpseMaterial = (ChamsMaterial)tmpCorpseMaterial;
                NativeHelper.SetMaterialColor(chamsColorMem, corpseMaterial.Address, corpseMaterial._ColorVisible, new("#000000"));
                NativeHelper.SetMaterialColor(chamsColorMem, corpseMaterial.Address, corpseMaterial._ColorInvisible, new("#000000"));

                CorpseMaterial = corpseMaterial;

                // Create Aimbot chams
                var tmpAimbotMaterial = CreateChamsMaterial(monoDomain, materialClass, (ulong)shaderProperty_InvisibleColorMem, (ulong)shaderProperty_VisibleColorMem);
                if (tmpAimbotMaterial == null) return false;

                ChamsMaterial aimbotMaterial = (ChamsMaterial)tmpAimbotMaterial;
                NativeHelper.SetMaterialColor(chamsColorMem, aimbotMaterial.Address, aimbotMaterial._ColorVisible, new("#005bff"));
                NativeHelper.SetMaterialColor(chamsColorMem, aimbotMaterial.Address, aimbotMaterial._ColorInvisible, new("#98bdff"));

                AimbotMaterial = aimbotMaterial;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Error initializing chams {ex}");
                return false;
            }
            finally
            {
                NativeHelper.UnloadAssetBundle();
            }

            return true;
        }

        private static ChamsMaterial? CreateChamsMaterial(ulong monoDomain, ulong materialClass, ulong invisibleColorMem, ulong visibleColorMem)
        {
            ulong materialAddress = NativeHelper.CreateMaterialFromShader(monoDomain, materialClass);
            if (materialAddress == 0x0)
            {
                Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Failed to create the material from the loaded shader!");
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
                    Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Got material instance: \"{instanceID}\"!");
                }

                break;
            }

            if (instanceID == int.MaxValue)
            {
                Logger.WriteLine($"[CHAMS MANAGER]: Initialize() -> Failed to get the material instance from the created material!");
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
    }
}
