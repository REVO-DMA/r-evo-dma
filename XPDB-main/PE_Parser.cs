using PeNet;
using PeNet.Header.Pe;
using PeNet.Header.Resource;
using System.Runtime.InteropServices;
using TerraFX.Interop.Windows;
using XPDB.Misc;

namespace XPDB
{
    public static unsafe class PE_Parser
    {
        private const uint CV_SIGNATURE_RSDS = 0x53445352;
        public const string NT_SYMBOL_PATH = "https://msdl.microsoft.com/download/symbols";
        public const string UNITY_SYMBOL_PATH = "http://symbolserver.unity3d.com";

        private struct CV_INFO_PDB70
        {
            public uint CvSignature;
            public Guid Signature;
            public uint Age;
            public fixed char PdbFileName[512];
        }

        public readonly struct UrlDebugData(string signature, string age, string pdbFileName)
        {
            public readonly string Signature = signature;
            public readonly string Age = age;
            public readonly string PdbFileName = pdbFileName;
        }

        public readonly struct DebugData(Guid signature, uint age, string pdbFileName)
        {
            public readonly Guid Signature = signature;
            public readonly uint Age = age;
            public readonly string PdbFileName = pdbFileName;

            public readonly UrlDebugData GetUrlFormat()
            {
                string signature = Signature.ToString("N").ToUpperInvariant();
                string age = Age.ToString("X");
                string pdbFileName = Path.GetFileName(PdbFileName);

                return new(signature, age, pdbFileName);
            }

            public readonly string ToURL(string symbolServer = NT_SYMBOL_PATH)
            {
                var f = GetUrlFormat();
                return $"{symbolServer}/{f.PdbFileName}/{f.Signature}{f.Age}/{f.PdbFileName}";
            }

            public readonly string ToURLCompressed(string symbolServer = NT_SYMBOL_PATH)
            {
                var f = GetUrlFormat();
                return $"{symbolServer}/{f.PdbFileName}/{f.Signature}{f.Age}/{f.PdbFileName.TrimEnd('b')}_";
            }
        }

        /// <summary>
        /// Requires a memory backend.
        /// </summary>
        //public static Result<DebugData> GetDebugData(ulong peBase)
        //{
        //    try
        //    {
        //        var dosHeader = Memory.ReadValue<IMAGE_DOS_HEADER>(peBase);
        //        if (dosHeader.e_magic != IMAGE.IMAGE_DOS_SIGNATURE)
        //            throw new Exception("Invalid DOS header");

        //        var ntHeaders = Memory.ReadValue<IMAGE_NT_HEADERS64>(peBase + (uint)dosHeader.e_lfanew);
        //        if (ntHeaders.Signature != IMAGE.IMAGE_NT_SIGNATURE)
        //            throw new Exception("Invalid NT headers");

        //        var debugDirectoryEntry = ntHeaders.OptionalHeader.DataDirectory[IMAGE.IMAGE_DIRECTORY_ENTRY_DEBUG];
        //        if (debugDirectoryEntry.VirtualAddress == 0x0 || debugDirectoryEntry.Size == 0)
        //            throw new Exception("Debug directory not found");

        //        byte[] debugDirRaw = Memory.ReadBuffer(peBase + debugDirectoryEntry.VirtualAddress, (int)debugDirectoryEntry.Size);
        //        fixed (byte* pDebugDirRaw = debugDirRaw)
        //        {
        //            var debugDirectory = (IMAGE_DEBUG_DIRECTORY*)pDebugDirRaw;
        //            for (int i = 0; i < debugDirectoryEntry.Size / sizeof(IMAGE_DEBUG_DIRECTORY); ++i)
        //            {
        //                if (debugDirectory[i].Type == IMAGE.IMAGE_DEBUG_TYPE_CODEVIEW)
        //                {
        //                    var pdbInfo = Memory.ReadValue<CV_INFO_PDB70>(peBase + debugDirectory[i].AddressOfRawData);
        //                    if (pdbInfo.CvSignature == CV_SIGNATURE_RSDS)
        //                    {
        //                        string fileName = Marshal.PtrToStringAnsi((nint)pdbInfo.PdbFileName);
        //                        return new(true, new(pdbInfo.Signature, pdbInfo.Age, fileName));
        //                    }
        //                }
        //            }
        //        }

        //        throw new Exception("Unable to find IMAGE_DEBUG_TYPE_CODEVIEW");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLine($"[PE PARSER] -> GetDebugData(): Error ~ {ex}");
        //        return Result<DebugData>.Fail;
        //    }
        //}

        public static Result<DebugData> GetDebugData(string peLocation)
        {
            if (!File.Exists(peLocation))
                return Result<DebugData>.Fail;

            byte[] peBytes = File.ReadAllBytes(peLocation);
            return GetDebugData(peBytes);
        }

        public static Result<DebugData> GetDebugData(byte[] peBytes)
        {
            PeFile peHeader = new(peBytes);
            ImageDebugDirectory[] debugDir = peHeader.ImageDebugDirectory;
            foreach (ImageDebugDirectory section in debugDir)
            {
                CvInfoPdb70 pdbInfo = section.CvInfoPdb70;
                if (pdbInfo == null || pdbInfo.CvSignature != CV_SIGNATURE_RSDS)
                    continue;

                return new(true, new(pdbInfo.Signature, pdbInfo.Age, pdbInfo.PdbFileName));
            }

            return Result<DebugData>.Fail;
        }

        public static Result<ImageSectionHeader> GetSectionHeader(byte[] peBytes, ulong rva)
        {
            PeFile pe = new(peBytes);
            for (int i = 0; i < pe.ImageNtHeaders.FileHeader.NumberOfSections; i++)
            {
                uint sectionStartRVA = pe.ImageSectionHeaders[i].VirtualAddress;
                uint sectionEndRVA = sectionStartRVA + pe.ImageSectionHeaders[i].VirtualSize;

                if (rva >= sectionStartRVA && rva < sectionEndRVA)
                    return new(true, pe.ImageSectionHeaders[i]);
            }

            return Result<ImageSectionHeader>.Fail;
        }

        public static ulong RvaToFileOffset(byte[] peBytes, ulong rva)
        {
            var sectionHeader = GetSectionHeader(peBytes, rva);
            if (!sectionHeader)
                return 0x0;

            uint sectionStartRVA = sectionHeader.Value.VirtualAddress;
            uint sectionStartRawData = sectionHeader.Value.PointerToRawData;
            return (rva - sectionStartRVA) + sectionStartRawData;
        }
    }
}
