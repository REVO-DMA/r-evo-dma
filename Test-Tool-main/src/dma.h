#pragma once

namespace dma
{
	const DWORD READ_FLAGS = VMMDLL_FLAG_NOCACHE | VMMDLL_FLAG_NOCACHEPUT | VMMDLL_FLAG_NO_PREDICTIVE_READ | VMMDLL_FLAG_NOPAGING | VMMDLL_FLAG_NOPAGING_IO;
	
	static VMM_HANDLE hVmm = NULL;

    static size_t ONE_GIGABYTE = static_cast<size_t>(244140); // 244140 = ~1GB of pages (4096 bytes)

    static void Exit(int code, bool prompt = true)
    {
        if (hVmm)
            VMMDLL_Close(hVmm);

        if (prompt)
        {
            printf(xorstr_("\nPress the space bar to exit..."));
            _getch();
        }

        exit(code);
    }

    static bool MMAP_Exists() {
        std::ifstream file(xorstr_("mmap.txt"));
        return file.good();
    }

    static void Start(bool firstTry = true)
    {
        bool startFailed = false;

        if (!firstTry)
            std::cout << xorstr_("[i] Starting DMA without MMAP...") << std::endl;
        else if (!hVmm)
            std::cout << xorstr_("[i] Starting DMA...") << std::endl;
        else
        {
            std::cout << xorstr_("[i] Restarting DMA...") << std::endl;
            VMMDLL_Close(hVmm);
            hVmm = NULL;
        }

        std::string connectionStr = (settings::fpgaAlg == xorstr_("-1")) ? xorstr_("fpga") : (xorstr_("fpga://algo=") + settings::fpgaAlg);

        std::vector<LPCSTR> initArgs;

        // Define base args
        initArgs.push_back(xorstr_("-device"));
        initArgs.push_back((LPSTR)connectionStr.c_str());
        initArgs.push_back(xorstr_("-waitinitialize"));
        initArgs.push_back(xorstr_("-norefresh"));
        initArgs.push_back(xorstr_("-disable-python"));
        initArgs.push_back(xorstr_("-disable-symbolserver"));
        initArgs.push_back(xorstr_("-disable-symbols"));
        initArgs.push_back(xorstr_("-disable-infodb"));

        if (MMAP_Exists() && firstTry)
        {
            initArgs.push_back(xorstr_("-memmap"));
            initArgs.push_back(xorstr_("mmap.txt"));

            settings::usingMMAP = true;
        }
        else
        {
            settings::usingMMAP = false;
        }

        if (settings::isVerbose)
        {
            initArgs.push_back(xorstr_("-printf"));
            initArgs.push_back((LPCSTR)settings::verbosityString.c_str());
        }

        LPCSTR* strArray = new LPCSTR[initArgs.size()];
        std::copy(initArgs.begin(), initArgs.end(), strArray);

        hVmm = VMMDLL_Initialize(initArgs.size(), strArray);

        delete[] strArray;

        if (!hVmm)
        {
            // If startup failed, we are using a mmap, and this is the first try - retry without the mmap.
            if (firstTry && settings::usingMMAP)
            {
                std::cout << dye::red(xorstr_("[!] DMA startup failed (with MMAP)!")) << std::endl;

                printf(xorstr_("\n"));
                Start(false);
                
                return;
            }

            std::cout << dye::red(xorstr_("[!] DMA startup failed (without MMAP)!")) << std::endl;
            startFailed = true;
        }

        // If this is not the first try that means initial startup with a mmap failed.
        // It just succeeded without a mmap which means the mmap is bad!
        if (hVmm && !firstTry)
        {
            std::cout << dye::red(xorstr_("[!] Your MMAP is invalid!")) << std::endl;
            
            printf(xorstr_("\n"));
            printf(xorstr_("Would you like to delete the MMAP and retry DMA startup?\n"));
            printf(xorstr_("[1] Yes\n"));
            printf(xorstr_("[2] No\n\n"));

            int choice = utils::PromptNumber(xorstr_("Select an action"), 1, 2, 1);

            if (choice == 1)
            {
                if (!std::filesystem::remove(xorstr_("mmap.txt")))
                    std::cout << dye::red(xorstr_("[!] Unable to delete mmap.txt file!")) << std::endl;
                else // mmap.txt file is deleted - retry DMA startup normally!
                {
                    Start();

                    return;
                }
            }
            
            Exit(-1);
            
            return;
        }

        if (startFailed)
        {
            printf(xorstr_("\n"));
            printer::PrintTroubleshootingGuide();

            Exit(-1);
        }
    }

    static dye::colorful<std::string> GetDeviceInfo()
    {
        dye::colorful<std::string> deviceInfo;

        printf(xorstr_("[i] Getting DMA device info...\n\n"));

        ULONG64 qwID = 0, qwVersionMajor = 0, qwVersionMinor = 0, qwAlgoTiny = 0;

        VMMDLL_ConfigGet(hVmm, LC_OPT_FPGA_FPGA_ID, &qwID);
        VMMDLL_ConfigGet(hVmm, LC_OPT_FPGA_VERSION_MAJOR, &qwVersionMajor);
        VMMDLL_ConfigGet(hVmm, LC_OPT_FPGA_VERSION_MINOR, &qwVersionMinor);
        VMMDLL_ConfigGet(hVmm, LC_OPT_FPGA_ALGO_TINY, &qwAlgoTiny);

        std::string fpgaAlgoStr = xorstr_("");
        bool isTinyAlgo = qwAlgoTiny;
        if (isTinyAlgo) fpgaAlgoStr = xorstr_("Tiny");
        else fpgaAlgoStr = xorstr_("Normal");

        deviceInfo += xorstr_("Device Info:\n");
        std::string deviceName(hVmm->dev.szDeviceName);
        deviceInfo += xorstr_("- Device Name: ") + deviceName + xorstr_("\n");
        deviceInfo += xorstr_("- Firmware Version: ") + std::to_string(qwVersionMajor) + xorstr_(".") + std::to_string(qwVersionMinor) + xorstr_("\n");
        if (isTinyAlgo) deviceInfo += xorstr_("- FPGA Algorithm: ") + dye::red(fpgaAlgoStr) + xorstr_("\n");
        else deviceInfo += xorstr_("- FPGA Algorithm: ") + fpgaAlgoStr + xorstr_("\n\n");

        return deviceInfo;
    }

    struct MemoryRange {
        ULONG64 start;
        ULONG64 end;
    };

    static std::vector<MemoryRange> ParseMMAP()
    {
        std::vector<MemoryRange> memoryRanges;
        std::ifstream file(xorstr_("mmap.txt"));

        if (!file.is_open())
        {
            printf(xorstr_("[i] Unable to find mmap.txt file. Falling back to DMA memory enumeration.\n\n"));
            return memoryRanges;
        }

        std::string line;
        bool isSecondFormat = false;

        // Determine mmap format
        while (std::getline(file, line))
        {
            if (!line.empty())
            {
                if (line.find('#') != std::string::npos)
                    isSecondFormat = true;

                break;
            }
        }

        file.clear();
        file.seekg(0, std::ios::beg);

        if (!isSecondFormat)
            printf(xorstr_("[i] Parsing mmap.txt (format 1)...\n\n"));
        else
            printf(xorstr_("[i] Parsing mmap.txt (format 2)...\n\n"));

        while (std::getline(file, line)) {
            if (line.find('#') != std::string::npos || utils::HasMoreThanOneDash(line))
                continue;

            size_t dashPos = line.find('-');
            if (dashPos != std::string::npos)
            {
                std::string startStr, endStr;

                if (isSecondFormat)
                {
                    std::vector<std::string> segments;
                    std::stringstream ss(line);
                    std::string segment;

                    while (std::getline(ss, segment, ' '))
                        if (!segment.empty())
                            segments.push_back(segment);

                    if (segments.size() >= 4)
                    {
                        startStr = segments[1];
                        endStr = segments[3];
                    }
                }
                else
                {
                    startStr = line.substr(0, dashPos);
                    endStr = line.substr(dashPos + 1);
                }

                // Remove any whitespace and non-hexadecimal characters
                startStr.erase(std::remove_if(startStr.begin(), startStr.end(),
                    [](char c) { return !std::isxdigit(c); }), startStr.end());

                endStr.erase(std::remove_if(endStr.begin(), endStr.end(),
                    [](char c) { return !std::isxdigit(c); }), endStr.end());

                ULONG64 start, end;
                std::stringstream ss1(startStr);
                ss1 >> std::hex >> start;

                std::stringstream ss2(endStr);
                ss2 >> std::hex >> end;

                memoryRanges.push_back({ start, end });

                printf(xorstr_("[+] Adding memory range: %llX - %llX\n"), start, end);
            }
        }

        printf(xorstr_("\n[i] Successfully parsed mmap.txt!\n\n"));

        return memoryRanges;
    }

    static std::vector<ULONG64> EnumeratePages(std::vector<MemoryRange> memoryRanges)
    {
        std::vector<ULONG64> memoryPages;

        const size_t maxPages = ONE_GIGABYTE * 10;

        bool enumerate = false;
        if (memoryRanges.size() == 0) enumerate = true;

        if (enumerate)
        {
            printf(xorstr_("[i] Enumerating memory ranges...\n\n"));

            PVMMDLL_MAP_PHYSMEM memMap;
            if (!VMMDLL_Map_GetPhysMem(hVmm, &memMap))
            {
                std::cout << dye::red(xorstr_("[!] Error while enumerating memory ranges!")) << std::endl;
                Exit(-1);
            }

            for (int i = 0; i < memMap->cMap; i++)
            {
                if (memoryPages.size() >= maxPages)
                    break;

                auto pMapEntry = memMap->pMap[i];
                ULONG64 sectionTop = pMapEntry.pa + pMapEntry.cb;

                printf(xorstr_("[+] Adding memory range: %llX - %llX\n"), pMapEntry.pa, sectionTop);

                for (ULONG64 p = pMapEntry.pa; p + 0x1000 < sectionTop && memoryPages.size() < maxPages; p += 0x1000)
                    memoryPages.push_back(p);
            }

            VMMDLL_MemFree(memMap);
        }
        else
        {
            printf(xorstr_("[i] Building memory map from mmap.txt...\n\n"));

            for (int i = 0; i < memoryRanges.size(); i++)
            {
                if (memoryPages.size() >= maxPages)
                    break;

                auto& pMapEntry = memoryRanges[i];
                for (ULONG64 p = pMapEntry.start; p + 0x1000 < pMapEntry.end && memoryPages.size() < maxPages; p += 0x1000)
                    memoryPages.push_back(p);
            }
        }

        if (memoryPages.size() == 0)
        {
            if (MMAP_Exists())
                std::cout << dye::red(xorstr_("[!] No memory ranges were added!")) << std::endl;
            else
            {
                std::cout << dye::red(xorstr_("[!] No memory ranges were added!")) << std::endl;
                std::cout << dye::red(xorstr_("\tPlease try adding a memory map (mmap.txt) file to the same folder as this EXE and re-run this tool.")) << std::endl;
            }

            Exit(-1);
        }
        else
        {
            if (enumerate)
                printf(xorstr_("\n[i] Successfully enumerated memory ranges!\n\n"));
            else
                printf(xorstr_("[i] Successfully built memory map from mmap.txt!\n\n"));
        }

        return memoryPages;
    }
}