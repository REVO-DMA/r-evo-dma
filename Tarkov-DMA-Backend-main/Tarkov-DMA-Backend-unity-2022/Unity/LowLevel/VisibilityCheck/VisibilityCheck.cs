using Tarkov_DMA_Backend.LowLevel;
using Tarkov_DMA_Backend.LowLevel.PDB;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;

namespace Tarkov_DMA_Backend.Unity.LowLevel.VisibilityCheck
{
    public static partial class VisibilityCheck
    {
        private static ulong ShellCodeDataAddress = 0x0;

        public static Result<VC_Structs.ShellCodeLocation> PlaceShellcode(VC_Structs.InitData initData)
        {
            try
            {
                byte[] shellcodeDllBytes = File.ReadAllBytes("Shellcode.dll");
                byte[] shellcodePdbBytes = File.ReadAllBytes("Shellcode.pdb");

                using XPDB_Core pdb = new(shellcodePdbBytes);

                byte[] trampolineBytes;
                ulong trampolineAddress;
                {
                    var func = pdb.GetFunction(VC_Structs.TRAMPOLINE_METHOD_NAME);
                    if (!func)
                        throw new Exception($"Failed to find function {VC_Structs.TRAMPOLINE_METHOD_NAME}");

                    ulong fOffset = PE_Parser.RvaToFileOffset(shellcodeDllBytes, func.Value.RVA);
                    trampolineBytes = shellcodeDllBytes.AsSpan((int)fOffset, (int)func.Value.Size).ToArray();
                    trampolineAddress = MemoryUtils.AlignAddress(initData.CodeCave);

                    unsafe
                    {
                        byte[] searchBytes = BitConverter.GetBytes(0xFAFAFAFAFAFAFAFA);
                        uint hookIndex = trampolineBytes.FindSignature(searchBytes);
                        if (hookIndex == uint.MaxValue)
                            throw new Exception("Failed to find ShellCodeData struct placeholder address");

                        fixed (byte* pCodeBytes = trampolineBytes)
                        {
                            *(ulong*)((ulong)pCodeBytes + hookIndex) = initData.shellCodeDataStruct;
                        }
                    }

                    if (!Memory.WriteBufferEnsure(trampolineAddress, trampolineBytes))
                        throw new Exception($"Failed to write {VC_Structs.TRAMPOLINE_METHOD_NAME} bytes to 0x{trampolineAddress:X}");
                }

                byte[] executorBytes;
                ulong executorAddress;
                {
                    var func = pdb.GetFunction(VC_Structs.EXECUTOR_METHOD_NAME);
                    if (!func)
                        throw new Exception($"Failed to find function {VC_Structs.EXECUTOR_METHOD_NAME}");

                    ulong fOffset = PE_Parser.RvaToFileOffset(shellcodeDllBytes, func.Value.RVA);
                    executorBytes = shellcodeDllBytes.AsSpan((int)fOffset, (int)func.Value.Size).ToArray();
                    executorAddress = MemoryUtils.AlignAddress(trampolineAddress + (uint)trampolineBytes.Length);
                    if (!Memory.WriteBufferEnsure(executorAddress, executorBytes))
                        throw new Exception($"Failed to write {VC_Structs.EXECUTOR_METHOD_NAME} bytes to 0x{executorAddress:X}");
                }

                return new(true, new(trampolineAddress, executorAddress));
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[VISIBILITY CHECK] -> Initialize(): {ex}");
                return Result<VC_Structs.ShellCodeLocation>.Fail;
            }
        }

        public static unsafe bool Initialize(VC_Structs.InitData initData)
        {
            ShellCodeDataAddress = initData.shellCodeDataStruct;

            bool success = false;

            try
            {
                var shellCodeLocation = PlaceShellcode(initData);
                if (!shellCodeLocation)
                    throw new();

                var shellCodeData = VC_Structs.ShellCodeData.Get(initData, shellCodeLocation);
                if (!shellCodeData)
                    throw new();

                if (!VC_Structs.ShellCodeData.Write(initData.shellCodeDataStruct, shellCodeData))
                    throw new();

                byte[] patchBytes = VC_Structs.GetPatchBytes(shellCodeLocation.Value.Trampoline);
                if (!Memory.WriteBufferEnsure(initData.GameWorldUpdate, patchBytes))
                    throw new();

                success = true;
            }
            catch { }
            finally
            {
                if (!success)
                {
                    // Don't free the bytes. If the mem write fails and we free bytes it will
                    // keep allocating the buffer in the same memory region. By not freeing them the
                    // bytes will be placed in a new region next time it tries to inject the shellcode.

                    //NativeHelper.FreeBytes(initData.shellCodeDataStruct);
                    //NativeHelper.FreeBytes(initData.CodeCave);
                }
            }

            return success;
        }

        public static void Update()
        {
            VC_Structs.VC_Offsets offsets;
            if (EFTDMA.IsOffline)
                offsets = new(Offsets.Player._playerBody);
            else
                offsets = new(Offsets.ObservedPlayerView.PlayerBody);

            VC_Structs.ShellCodeData.WriteValue(ShellCodeDataAddress, "offsets", offsets);
        }

        public static void SetActive(bool state)
        {
            VC_Structs.ShellCodeData.WriteValue(ShellCodeDataAddress, "shellcodeActive", XBOOL.Get(state));
        }

        public static Result<VC_Structs.InitData> GetInitData()
        {
            ulong linecast = 0x0;
            ulong gameWorldUpdate = 0x0;
            ulong setPOV = 0x0;
            ulong adjustShotVectors = 0x0;
            ulong shellCodeDataStruct = 0x0;
            ulong codeCave = 0x0;

            try
            {
                // GameWorld
                {
                    const string className = "EFT.GameWorld";
                    const string methodName = "Update";

                    var fClass = EFTDMA.MonoClasses.GetClass(className);

                    var fMethod = fClass.FindMethod(methodName);
                    if (!MemoryUtils.IsValidAddress(fMethod))
                        throw new Exception($"Unable to find {className}:{methodName}()");

                    gameWorldUpdate = NativeHelper.CompileMethod(fMethod);
                    if (!MemoryUtils.IsValidAddress(gameWorldUpdate))
                        throw new Exception($"Unable to compile {className}:{methodName}()");
                }

                // Linecast
                {
                    const string className = "UnityEngine.Physics";
                    const string methodName = "Linecast";

                    var fClass = EFTDMA.MonoClasses.GetClass(className);
                    NativeHelper.CompileClass(fClass);

                    var fMethod = fClass.FindMethod(methodName, 4);
                    if (!MemoryUtils.IsValidAddress(fMethod))
                        throw new Exception($"Unable to find {className}:{methodName}()");

                    linecast = NativeHelper.CompileMethod(fMethod);
                    if (!MemoryUtils.IsValidAddress(linecast))
                        throw new Exception($"Unable to compile {className}:{methodName}()");
                }

                {
                    const string className = "EFT.Player";
                    const string methodName = "set_PointOfView";

                    var fClass = EFTDMA.MonoClasses.GetClass(className);

                    ulong fMethod = (ulong)fClass.FindMethod(methodName);
                    if (!MemoryUtils.IsValidAddress(fMethod))
                        throw new Exception($"Unable to find {className}:{methodName}()");

                    setPOV = NativeHelper.CompileMethod(fMethod);
                    if (!MemoryUtils.IsValidAddress(setPOV))
                        throw new Exception($"Unable to compile {className}:{methodName}()");
                }

                {
                    const uint classToken = ClassNames.FirearmController.ClassName_ClassToken;
                    const string methodName = "AdjustShotVectors";

                    var fClass = EFTDMA.MonoClasses.GetClass(classToken);

                    ulong fMethod = (ulong)fClass.FindMethod(methodName);
                    if (!MemoryUtils.IsValidAddress(fMethod))
                        throw new Exception($"Unable to find {classToken}:{methodName}()");

                    adjustShotVectors = NativeHelper.CompileMethod(fMethod);
                    if (!MemoryUtils.IsValidAddress(adjustShotVectors))
                        throw new Exception($"Unable to compile {classToken}:{methodName}()");
                }

                {
                    shellCodeDataStruct = VC_Structs.ShellCodeData.Allocate();
                    if (!MemoryUtils.IsValidAddress(shellCodeDataStruct))
                        throw new Exception("The allocated ShellCodeData region had an invalid address");
                }

                {
                    codeCave = NativeHelper.AllocRWX();
                    if (!MemoryUtils.IsValidAddress(codeCave))
                        throw new Exception("The allocated RWX region had an invalid address");
                }

                return new(true, new(shellCodeDataStruct, codeCave, gameWorldUpdate, linecast, setPOV, adjustShotVectors));
            }
            catch (Exception ex)
            {
                if (MemoryUtils.IsValidAddress(shellCodeDataStruct))
                    NativeHelper.FreeBytes(shellCodeDataStruct);

                if (MemoryUtils.IsValidAddress(codeCave))
                    NativeHelper.FreeBytes(codeCave);

                Logger.WriteLine($"[VISIBILITY CHECK] -> GetInitData(): {ex}");
                return Result<VC_Structs.InitData>.Fail;
            }
        }

        public static void UpdateTppSettings(float horizontalDistance, float horizontalOffset, float verticalDistance, bool enabled)
        {
            VC_Structs.ShellCodeData.WriteValue(ShellCodeDataAddress, "tpp_enabled", enabled);
            VC_Structs.ShellCodeData.WriteValue(ShellCodeDataAddress, "tpp_horizontalDistance", horizontalDistance);
            VC_Structs.ShellCodeData.WriteValue(ShellCodeDataAddress, "tpp_horizontalOffset", horizontalOffset);
            VC_Structs.ShellCodeData.WriteValue(ShellCodeDataAddress, "tpp_verticalDistance", verticalDistance);
        }

        public static VC_Structs.ESPData GetESPData()
        {
            return VC_Structs.ShellCodeData.ReadValue<VC_Structs.ESPData>(ShellCodeDataAddress, "espData");
        }

        public static bool[][] GetVisibilityStateBuffer()
        {
            const int BYTE_LENGTH = VC_Structs.MAX_PLAYER_COUNT * VC_Structs.BONE_COUNT;

            byte[] arrayTmp = Memory.ReadBuffer(ShellCodeDataAddress + offset<VC_Structs.ShellCodeData>.Of("visiblePlayers"), BYTE_LENGTH, false);

            // Convert from byte[] to bool[]
            bool[] tmpArray = new bool[BYTE_LENGTH];
            for (int i = 0; i < BYTE_LENGTH; i++)
                tmpArray[i] = XBOOL.Get(arrayTmp[i]);

            // Convert to jagged array
            bool[][] outputArray = new bool[VC_Structs.MAX_PLAYER_COUNT][];
            for (int i = 0; i < VC_Structs.MAX_PLAYER_COUNT; i++)
            {
                for (int ii = 0; ii < VC_Structs.BONE_COUNT; ii++)
                {
                    if (ii == 0)
                        outputArray[i] = new bool[VC_Structs.BONE_COUNT];

                    outputArray[i][ii] = tmpArray[(i * VC_Structs.BONE_COUNT) + ii];
                }
            }

            return outputArray;
        }
    }
}
