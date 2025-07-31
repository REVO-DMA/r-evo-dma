using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;
using UnmanagedType = System.Runtime.InteropServices.UnmanagedType;

namespace Tarkov_DMA_Backend.Unity
{
    public static partial class NativeHelper
    {
        public const string LibName = "NativeHelper";

        public static readonly object Lock = new();

        private static void LogSuccess(string message)
        {
            return; // Only needed for debugging

            Logger.WriteLine($"[NATIVE HELPER] -> [SUCCESS] -> {message}");
        }

        private static void LogFailure(string message)
        {
            Logger.WriteLine($"[NATIVE HELPER] -> [FAILURE] -> {message}");
        }

        [LibraryImport(LibName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool NH_Initialize(IntPtr hVmm, uint pid);
        public static bool Initialize(IntPtr hVmm, uint pid)
        {
            if (NH_Initialize(hVmm, pid))
            {
                LogSuccess("NativeHelper initialization!");
                return true;
            }
            else
            {
                LogFailure("NativeHelper initialization!");
                return false;
            }
        }

        [LibraryImport(LibName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool NH_SetCodeCave(ulong codeCave);
        public static bool SetCodeCave(MonoClass hkClass)
        {
            try
            {
                var fMethod = hkClass.FindMethod("Awake");
                EFTDMA.HookAddress = MonoLibrary.FindJittedFunctionAddress(fMethod);
                if (EFTDMA.HookAddress == 0x0)
                    throw new Exception("Unable to find the EFT.AbstractApplication:Awake method!");

                bool setCC = NH_SetCodeCave(EFTDMA.HookAddress);
                if (!setCC)
                    return false;

                LogSuccess($"Code Cave set to 0x{EFTDMA.HookAddress:X} successfully!");
            }
            catch(Exception ex)
            {
                LogFailure($"Unable to set Code Cave ~ {ex}");
                return false;
            }

            return true;
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_CallFunction(ulong function, ulong rcx, ulong rdx, ulong r8, ulong r9);
        public static ulong CallFunction(ulong functionAddr, ulong p1 = 0, ulong p2 = 0, ulong p3 = 0, ulong p4 = 0)
        {
            lock (Lock)
            {
                return NH_CallFunction(functionAddr, p1, p2, p3, p4);
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_GetTypeObject(ulong monoDomain, ulong monoType);
        public static ulong GetTypeObject(ulong monoDomain, ulong monoType)
        {
            lock (Lock)
            {
                ulong result = NH_GetTypeObject(monoDomain, monoType);

                if (result != 0x0)
                    LogSuccess($"Got type object of type at 0x{result:X}!");
                else
                    LogFailure($"Unable to get type object of type 0x{monoType:X} using mono domain at 0x{monoDomain:X}!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_CompileMethod(ulong monoMethod);
        public static ulong CompileMethod(ulong monoMethod)
        {
            lock (Lock)
            {
                ulong result = NH_CompileMethod(monoMethod);

                if (result != 0x0)
                    LogSuccess($"Method at 0x{monoMethod:X} was compiled successfully at 0x{result:X}!");
                else
                    LogFailure($"Method at 0x{monoMethod:X} was unable to be compiled!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_CompileClass(ulong monoClass);
        public static ulong CompileClass(ulong monoClass)
        {
            lock (Lock)
            {
                ulong result = NH_CompileClass(monoClass);

                if (result != 0x0)
                    LogSuccess($"Class at 0x{monoClass:X} was compiled successfully at 0x{result:X}!");
                else
                    LogFailure($"Class at 0x{monoClass:X} was unable to be compiled!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool NH_LoadAssetBundle(ulong assetBundle, ulong shaderName, ulong shaderTypeObject);
        public static bool LoadAssetBundle(ulong assetBundle, ulong shaderName, ulong shaderTypeObject)
        {
            lock (Lock)
            {
                bool result = NH_LoadAssetBundle(assetBundle, shaderName, shaderTypeObject);

                if (result)
                    LogSuccess($"Asset bundle was loaded successfully!");
                else
                    LogFailure($"Asset bundle was unable to be loaded!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial void NH_UnloadAssetBundle();
        public static void UnloadAssetBundle()
        {
            lock (Lock)
            {
                NH_UnloadAssetBundle();
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_CreateMaterialFromShader(ulong monoDomain, ulong materialClass);
        public static ulong CreateMaterialFromShader(ulong monoDomain, ulong materialClass)
        {
            lock (Lock)
            {
                ulong result = NH_CreateMaterialFromShader(monoDomain, materialClass);

                if (result != 0x0)
                    LogSuccess($"Material was created successfully!");
                else
                    LogFailure($"Material was unable to be created!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial int NH_ShaderPropertyToID(ulong propertyName);
        public static int ShaderPropertyToID(ulong propertyName)
        {
            lock (Lock)
            {
                return NH_ShaderPropertyToID(propertyName);
            }
        }

        [LibraryImport(LibName)]
        private static partial void NH_SetMaterialColor(ulong material, int propertyID, ulong color);
        public static void SetMaterialColor(RemoteBytes remoteBytes, ulong material, int propertyID, UnityColor color)
        {
            lock (Lock)
            {
                remoteBytes.WriteValue(color);

                NH_SetMaterialColor(material, propertyID, (ulong)remoteBytes);
            }
        }
        public static void SetMaterialColor(ulong material, int propertyID, UnityColor color)
        {
            lock (Lock)
            {
                using (RemoteBytes remoteBytes = new(UnityColor.GetSizeU()))
                {
                    SetMaterialColor(remoteBytes, material, propertyID, color);
                }
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_AllocBytes(uint size);
        public static ulong AllocBytes(uint size)
        {
            lock (Lock)
            {
                ulong result = NH_AllocBytes(size);

                if (result != 0x0)
                    LogSuccess($"Allocated {size} bytes 0x{result:X}!");
                else
                    LogFailure($"Failed to allocate {size} bytes!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial void NH_FreeBytes(ulong pv);
        public static void FreeBytes(ulong pv)
        {
            lock (Lock)
            {
                NH_FreeBytes(pv);
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_AllocRWX();
        public static ulong AllocRWX()
        {
            lock (Lock)
            {
                ulong result = NH_AllocRWX();

                if (result != 0x0)
                    LogSuccess($"Allocated RWX bytes at 0x{result:X}!");
                else
                    LogFailure($"Failed to allocate RWX bytes!");

                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_SetBehaviorState(ulong behavior, [MarshalAs(UnmanagedType.Bool)] bool state);
        public static bool SetBehaviorState(ulong behavior, bool state)
        {
            lock (Lock)
            {
                ulong result = NH_SetBehaviorState(behavior, state);

                if (result != 0x0)
                {
                    LogSuccess($"Behavior was disabled successfully!");
                    return true;
                }

                LogFailure($"Behavior was unable to be disabled!");
                return false;
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_FindGameObject(ulong name);
        public static ulong FindGameObject(string name)
        {
            lock (Lock)
            {
                var nameMonoStr = RemoteBytes.MonoString.Get(name);
                using RemoteBytes nameMonoStrMem = new(nameMonoStr.GetSizeU());
                nameMonoStrMem.WriteString(nameMonoStr);

                ulong result = NH_FindGameObject((ulong)nameMonoStrMem);

                if (result != 0x0)
                    LogSuccess($"Game object \"{name}\" was found at 0x{result:X}!");
                else
                    LogFailure($"Game object \"{name}\" could not be found!");
                
                return result;
            }
        }

        [LibraryImport(LibName)]
        private static partial ulong NH_GameObjectSetActive(ulong gameObject, [MarshalAs(UnmanagedType.Bool)] bool state);
        /// <param name="state">True = Active | False = Inactive</param>
        public static void GameObjectSetActive(ulong gameObject, bool state)
        {
            lock (Lock)
            {
                NH_GameObjectSetActive(gameObject, state);
            }
        }

        [LibraryImport(LibName)]
        private static partial int NH_GetMonoMethodParamCount(ulong monoMethod);
        public static int GetMonoMethodParamCount(ulong monoMethod)
        {
            lock (Lock)
            {
                return NH_GetMonoMethodParamCount(monoMethod);
            }
        }
    }
}
