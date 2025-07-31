using static arena_dma_backend.Arena.Misc;

namespace arena_dma_backend.Mono
{
    public static partial class NativeHelper
    {
        private const string DLLName = "NativeHelper";

        private static readonly object _nativeHelperLock = new();

        private static void LogSuccess(string message)
        {
            Logger.WriteLine($"[NATIVE HELPER] -> [SUCCESS] -> {message}");
        }

        private static void LogFailure(string message)
        {
            Logger.WriteLine($"[NATIVE HELPER] -> [FAILURE] -> {message}");
        }

        [LibraryImport(DLLName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool NH_Initialize(nint hVmm, uint pid, [MarshalAs(UnmanagedType.Bool)] bool debug);
        public static bool Initialize(nint hVmm, uint pid, bool debug)
        {
            if (NH_Initialize(hVmm, pid, debug))
            {
                if (debug) LogSuccess("NativeHelper initialization [DEBUG]!");
                else LogSuccess("NativeHelper initialization!");

                return true;
            }
            else
            {
                LogFailure("NativeHelper initialization!");
                return false;
            }
        }

        [LibraryImport(DLLName)]
        private static partial void NH_SetCodeCave(ulong codeCave);
        public static bool SetCodeCave()
        {
            try
            {
                MonoAPI.InitializeFunctions();

                var hkClass = MonoAPI.Class.FindOne("Assembly-CSharp", "EFT.AbstractApplication");
                if ((ulong)hkClass == 0x0)
                    throw new Exception("Unable to find the EFT.AbstractApplication class!");

                ulong hkMethod = hkClass.FindJittedMethod("Awake");
                if (hkMethod == 0x0)
                    throw new Exception("Unable to find the EFT.AbstractApplication:Awake method!");

                NH_SetCodeCave(hkMethod);
                LogSuccess($"Code Cave set to 0x{hkMethod:X} successfully!");
            }
            catch
            {
                LogFailure($"Unable to set Code Cave!");
                return false;
            }

            return true;
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_CallFunction(ulong function, ulong rcx, ulong rdx, ulong r8, ulong r9);
        public static ulong CallFunction(ulong functionAddr, ulong p1 = 0, ulong p2 = 0, ulong p3 = 0, ulong p4 = 0)
        {
            lock (_nativeHelperLock)
            {
                return NH_CallFunction(functionAddr, p1, p2, p3, p4);
            }
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_GetTypeObject(ulong monoDomain, ulong monoType);
        public static ulong GetTypeObject(ulong monoDomain, ulong monoType)
        {
            lock (_nativeHelperLock)
            {
                ulong result = NH_GetTypeObject(monoDomain, monoType);

                if (result != 0x0)
                    LogSuccess($"Got type object of type at 0x{result:X}!");
                else
                    LogFailure($"Unable to get type object of type 0x{monoType:X} using mono domain at 0x{monoDomain:X}!");

                return result;
            }
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_CompileMethod(ulong monoMethod);
        public static ulong CompileMethod(ulong monoMethod)
        {
            lock (_nativeHelperLock)
            {
                ulong result = NH_CompileMethod(monoMethod);

                if (result != 0x0)
                    LogSuccess($"Method at 0x{monoMethod:X} was compiled successfully at 0x{result:X}!");
                else
                    LogFailure($"Method at 0x{monoMethod:X} was unable to be compiled!");

                return result;
            }
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_CompileClass(ulong monoClass);
        public static ulong CompileClass(ulong monoClass)
        {
            lock (_nativeHelperLock)
            {
                ulong result = NH_CompileClass(monoClass);

                if (result != 0x0)
                    LogSuccess($"Class at 0x{monoClass:X} was compiled successfully at 0x{result:X}!");
                else
                    LogFailure($"Class at 0x{monoClass:X} was unable to be compiled!");

                return result;
            }
        }

        [LibraryImport(DLLName)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool NH_LoadAssetBundle(ulong assetBundle, ulong shaderName, ulong shaderTypeObject);
        public static bool LoadAssetBundle(ulong assetBundle, ulong shaderName, ulong shaderTypeObject)
        {
            lock (_nativeHelperLock)
            {
                bool result = NH_LoadAssetBundle(assetBundle, shaderName, shaderTypeObject);

                if (result)
                    LogSuccess($"Asset bundle was loaded successfully!");
                else
                    LogFailure($"Asset bundle was unable to be loaded!");

                return result;
            }
        }

        [LibraryImport(DLLName)]
        private static partial void NH_UnloadAssetBundle();
        public static void UnloadAssetBundle()
        {
            lock (_nativeHelperLock)
            {
                NH_UnloadAssetBundle();
            }
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_CreateMaterialFromShader(ulong monoDomain, ulong materialClass);
        public static ulong CreateMaterialFromShader(ulong monoDomain, ulong materialClass)
        {
            lock (_nativeHelperLock)
            {
                ulong result = NH_CreateMaterialFromShader(monoDomain, materialClass);

                if (result != 0x0)
                    LogSuccess($"Material was created successfully!");
                else
                    LogFailure($"Material was unable to be created!");

                return result;
            }
        }

        [LibraryImport(DLLName)]
        private static partial int NH_ShaderPropertyToID(ulong propertyName);
        public static int ShaderPropertyToID(ulong propertyName)
        {
            lock (_nativeHelperLock)
            {
                return NH_ShaderPropertyToID(propertyName);
            }
        }

        [LibraryImport(DLLName)]
        private static partial void NH_SetMaterialColor(ulong material, int propertyID, ulong color);
        public static void SetMaterialColor(RemoteBytes remoteBytes, ulong material, int propertyID, UnityColor color)
        {
            lock (_nativeHelperLock)
            {
                remoteBytes.WriteValue(color);

                NH_SetMaterialColor(material, propertyID, (ulong)remoteBytes);
            }
        }
        public static void SetMaterialColor(ulong material, int propertyID, UnityColor color)
        {
            lock (_nativeHelperLock)
            {
                using (RemoteBytes remoteBytes = new(UnityColor.GetSizeU()))
                {
                    SetMaterialColor(remoteBytes, material, propertyID, color);
                }
            }
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_AllocBytes(uint size);
        public static ulong AllocBytes(uint size)
        {
            lock (_nativeHelperLock)
            {
                ulong result = NH_AllocBytes(size);

                if (result != 0x0)
                    LogSuccess($"Allocated {size} bytes 0x{result:X}!");
                else
                    LogFailure($"Failed to allocate {size} bytes!");

                return result;
            }
        }

        [LibraryImport(DLLName)]
        private static partial void NH_FreeBytes(ulong pv);
        public static void FreeBytes(ulong pv)
        {
            lock (_nativeHelperLock)
            {
                NH_FreeBytes(pv);
            }
        }

        [LibraryImport(DLLName)]
        private static partial ulong NH_SetBehaviorState(ulong behavior, [MarshalAs(UnmanagedType.Bool)] bool state);
        public static bool SetBehaviorState(ulong behavior, bool state)
        {
            lock (_nativeHelperLock)
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

        [LibraryImport(DLLName)]
        private static partial ulong NH_DisableBehavior(ulong behavior);
        public static bool DisableBehavior(ulong behavior)
        {
            lock (_nativeHelperLock)
            {
                ulong result = NH_DisableBehavior(behavior);

                if (result != 0x0)
                {
                    LogSuccess($"Behavior was disabled successfully!");
                    return true;
                }

                LogFailure($"Behavior was unable to be disabled!");
                return false;
            }
        }

        [LibraryImport(DLLName)]
        private static partial int NH_GetCameraHeight(ulong camera);
        public static int GetCameraHeight(ulong camera)
        {
            lock (_nativeHelperLock)
            {
                return NH_GetCameraHeight(camera);
            }
        }

        [LibraryImport(DLLName)]
        private static partial int NH_GetCameraWidth(ulong camera);
        public static int GetCameraWidth(ulong camera)
        {
            lock (_nativeHelperLock)
            {
                return NH_GetCameraWidth(camera);
            }
        }
    }
}