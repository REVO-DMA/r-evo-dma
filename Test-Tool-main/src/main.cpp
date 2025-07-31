#include "../globals.h"

int main(int argc, char* argv[])
{
    // Increases consistency & speeds
    SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);

    bool brokeOut = false;

    printer::AppInfo();

    dma::Start();

    auto deviceInfo = dma::GetDeviceInfo();
    std::cout << deviceInfo;

    std::vector<dma::MemoryRange> mmap;
    if (settings::usingMMAP)
        mmap = dma::ParseMMAP();
    
    std::vector<ULONG64> memoryPages;

    while (true)
    {
        int selectedAction = -1;
        while (selectedAction == -1)
            selectedAction = printer::PromptUser();

        if (selectedAction != 6 && selectedAction != 7 && selectedAction != 8 && memoryPages.size() == 0)
            memoryPages = dma::EnumeratePages(mmap);

        if (selectedAction == 1 || selectedAction == 2)
            rps::Test(memoryPages);
        else if (selectedAction == 3)
            throughput::Test(memoryPages);
        else if (selectedAction == 4)
        {
            rps::Test(memoryPages);
            throughput::Test(memoryPages);
        }
        else if (selectedAction == 5)
            rps::Test(memoryPages);
        else if (selectedAction == 6)
        {
            dma::Start();
            goto print;
        }
        else if (selectedAction == 8)
        {
            brokeOut = true;
            break;
        }

        printer::PressToContinue();

    print:
        printer::AppInfo();

        std::cout << deviceInfo;

        if (settings::usingMMAP)
            printf(xorstr_("[i] Using memory map from mmap.txt.\n\n"));
    }

    if (brokeOut) dma::Exit(0, false);
    else dma::Exit(0);
}