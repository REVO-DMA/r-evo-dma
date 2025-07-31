using Tarkov_DMA_Backend.Misc;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;

namespace Tarkov_DMA_Backend.Unity.LowLevel
{
    public static class Patcher
    {
        public readonly struct SignatureInfo(byte[] signature, byte[] patch, int readSize = 0, string signatureMask = null, string patchMask = null, int patchOffset = 0x0, byte[] realPatch = null)
        {
            public readonly byte[] Signature = signature;
            public readonly string SignatureMask = signatureMask;

            public readonly byte[] Patch = patch;
            public readonly byte[] RealPatch = realPatch;
            public readonly string PatchMask = patchMask;

            public readonly int ReadSize = readSize;
            public readonly int PatchOffset = patchOffset;
        }

        public static void PatchMethod(in string className, in string methodName, in SignatureInfo sigInfo, in int subclass = -1, in string assemblyName = "Assembly-CSharp", in bool compileClass = false, in bool compileMethod = true)
        {
            static string GetName(string className)
            {
                return $"({className})";
            }

            static string FormatEx(string exception)
            {
                return $"[PATCH METHOD]: {exception}";
            }

            var mClass = MonoLibrary.FindClass(assemblyName, className, subclass);
            if ((ulong)mClass == 0x0)
                throw new Exception(FormatEx($"Unable to find class {GetName(className)}!"));

            if (compileClass)
            {
                ulong compiledClass = NativeHelper.CompileClass((ulong)mClass);
                if (compiledClass == 0x0)
                    throw new Exception(FormatEx($"Unable to compile class {GetName(className)}!"));
            }

            var mMethod = mClass.FindMethod(methodName);

            PatchMethod(mMethod, className, methodName, sigInfo, compileMethod);
        }

        public static void PatchMethod(in MonoClass mClass, in string className, in string methodName, in SignatureInfo sigInfo, in bool compileClass = false, in bool compileMethod = true)
        {
            static string GetName(string className)
            {
                return $"({className})";
            }

            static string FormatEx(string exception)
            {
                return $"[PATCH METHOD]: {exception}";
            }

            if (compileClass)
            {
                ulong compiledClass = NativeHelper.CompileClass((ulong)mClass);
                if (compiledClass == 0x0)
                    throw new Exception(FormatEx($"Unable to compile class {GetName(className)}!"));
            }

            var mMethod = mClass.FindMethod(methodName);

            PatchMethod(mMethod, className, methodName, sigInfo, compileMethod);
        }

        public static void PatchMethod(in MonoMethod mMethod, in string className, in string methodName, in SignatureInfo sigInfo, in bool compile = true)
        {
            static string GetName(string className, string methodName)
            {
                return $"({className} -> {methodName})";
            }

            static string FormatEx(string exception)
            {
                return $"[PATCH METHOD]: {exception}";
            }

            static void LogAlreadyPatched(string className, string methodName)
            {
                return; // Only needed for debugging

                Logger.WriteLine(FormatEx($"{GetName(className, methodName)} has already been patched!"));
            }

            static void LogSuccessfullyPatched(string className, string methodName)
            {
                return; // Only needed for debugging

                Logger.WriteLine(FormatEx($"Successfully patched {GetName(className, methodName)}!"));
            }

            if (mMethod == 0x0)
                throw new Exception(FormatEx($"Unable to find method {GetName(className, methodName)}!"));

            ulong methodAddr;

            if (compile)
            {
                methodAddr = NativeHelper.CompileMethod((ulong)mMethod);
                if (methodAddr == 0x0)
                    throw new Exception(FormatEx($"Unable to compile method {GetName(className, methodName)}!"));
            }
            else
                methodAddr = (ulong)mMethod;

            // Check if this is a simple patch (overwrites the method starting at the beginning)
            if (sigInfo.Signature == null)
            {
                if (!Memory.WriteBufferEnsure(methodAddr, sigInfo.Patch))
                    throw new Exception(FormatEx($"Unable to patch method bytes for {GetName(className, methodName)}!"));

                LogSuccessfullyPatched(className, methodName);
                return;
            }

            // This is a complex patch

            byte[] methodBytes = Memory.ReadBufferEnsure(methodAddr, sigInfo.ReadSize);
            if (methodBytes == null)
                throw new Exception(FormatEx($"Unable to read method bytes for {GetName(className, methodName)}!"));

            // The first sig to check.
            byte[] sig1;
            string mask1;

            // The second sig to check.
            byte[] sig2;
            string mask2;

            // Whether or not to treat sig1 as the patch sig.
            bool sig1AsPatch;

            // If the patch has a negative offset the original sig might still be there.
            // Check the patch sig first in this case.
            if (sigInfo.PatchOffset < 0x0)
            {
                sig1 = sigInfo.Patch;
                mask1 = sigInfo.PatchMask;

                sig2 = sigInfo.Signature;
                mask2 = sigInfo.SignatureMask;

                sig1AsPatch = true;
            }
            else
            {
                sig1 = sigInfo.Signature;
                mask1 = sigInfo.SignatureMask;

                sig2 = sigInfo.Patch;
                mask2 = sigInfo.PatchMask;

                sig1AsPatch = false;
            }

            uint offset = methodBytes.FindSignature(in sig1, mask1);
            if (offset == uint.MaxValue)
            {
                if (methodBytes.FindSignature(in sig2, mask2) == uint.MaxValue)
                    throw new Exception(FormatEx($"Unable to find patch signature for {GetName(className, methodName)}!"));
                else
                {
                    LogAlreadyPatched(className, methodName);
                    return;
                }
            }
            else if (sig1AsPatch)
            {
                LogAlreadyPatched(className, methodName);
                return;
            }

            ulong finalAddr = (ulong)((long)(methodAddr + offset) + sigInfo.PatchOffset);

            byte[] usedPatch;
            if (sigInfo.RealPatch != null)
                usedPatch = sigInfo.RealPatch;
            else
                usedPatch = sigInfo.Patch;

            if (!Memory.WriteBufferEnsure(finalAddr, usedPatch))
                throw new Exception(FormatEx($"Unable to patch method bytes for {GetName(className, methodName)}!"));

            LogSuccessfullyPatched(className, methodName);
        }
    }
}
