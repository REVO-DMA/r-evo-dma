#pragma once

namespace throughput
{
    struct Result
    {
        double throughput;
        std::string humanThroughput;
        size_t failedCount;
        std::string humanFailedPercent;
        LONG64 elapsedSeconds;
    };

    static Result Run(std::vector<ULONG64> pages, size_t pageCount)
    {
        PPMEM_SCATTER scatters;

        size_t readsFailed = 0;

        if (!LcAllocScatter1(pageCount, &scatters))
        {
            std::cout << dye::red(xorstr_("[!] Failed to allocate memory for the throughput test!")) << std::endl;
            dma::Exit(-1);
        }

        for (size_t i = 0; i < pageCount; i++)
            scatters[i]->qwA = pages[i];

        auto startTime = std::chrono::high_resolution_clock::now();

        VMMDLL_MemReadScatter(dma::hVmm, -1, scatters, pageCount, dma::READ_FLAGS);

        auto endTime = std::chrono::high_resolution_clock::now();

        // Check for failed scatters
        for (int i = 0; i < pageCount; i++)
            if (!scatters[i]->f)
                readsFailed++;

        LcMemFree(scatters);

        // Do calculations

        auto elapsedTime = std::chrono::duration_cast<std::chrono::duration<double>>(endTime - startTime).count();
        auto elapsedSeconds = duration_cast<std::chrono::seconds>(endTime - startTime).count();

        double amountRead = (static_cast<double>(pageCount) * static_cast<double>(0x1000)) / static_cast<double>(1000000);
        double throughput = amountRead / elapsedTime;

        double failedPercent = (static_cast<double>(readsFailed) / static_cast<double>(pageCount)) * static_cast<double>(100);

        return
        {
            throughput,
            utils::HumanizeNumber(throughput, 2),
            readsFailed,
            utils::HumanizeNumber(failedPercent, 2),
            elapsedSeconds
        };
    }

    static void Print(Result result)
    {
        const float godlikeThreshold = 950.f;
        const float vipThreshold = 450.f;
        const float eliteThreshold = 220.f;
        const float amazingThreshold = 200.f;
        const float goodThreshold = 150.f;
        const float warningThreshold = 125.f;

        const std::string prefix = xorstr_("- Throughput (MB/s): ");

        printf(xorstr_("Results:\n"));

        if (result.throughput >= godlikeThreshold)
            std::cout << std::fixed << prefix << dye::bright_white(result.humanThroughput) << " " << dye::on_bright_white(xorstr_("(GODLIKE)")) << std::endl;
        else if (result.throughput >= vipThreshold)
            std::cout << std::fixed << prefix << dye::blue(result.humanThroughput) << " " << dye::on_blue(xorstr_("(VIP)")) << std::endl;
        else if (result.throughput >= eliteThreshold)
            std::cout << std::fixed << prefix << dye::aqua(result.humanThroughput) << " " << dye::on_aqua(xorstr_("(ELITE)")) << std::endl;
        else if (result.throughput >= amazingThreshold)
            std::cout << std::fixed << prefix << dye::purple(result.humanThroughput) << " " << dye::on_purple(xorstr_("(AMAZING)")) << std::endl;
        else if (result.throughput >= goodThreshold)
            std::cout << std::fixed << prefix << dye::green(result.humanThroughput) << " " << dye::on_green(xorstr_("(GOOD)")) << std::endl;
        else if (result.throughput >= warningThreshold)
            std::cout << std::fixed << prefix << dye::yellow(result.humanThroughput) << " " << dye::on_yellow(xorstr_("(WARNING)")) << std::endl;
        else
            std::cout << std::fixed << prefix << dye::red(result.humanThroughput) << " " << dye::on_red(xorstr_("(LOW)")) << std::endl;

        if (result.failedCount > 0)
            std::cout << std::fixed << xorstr_("- Failed Reads: ") << dye::red(result.failedCount) << xorstr_(" (") << result.humanFailedPercent << xorstr_("%)") << std::endl;

        printf(xorstr_("\n"));
    }

    static void Test(std::vector<ULONG64> pages)
    {
        Result oneGigResult;
        Result tenGigResult;

        printf(xorstr_("[i] Running 1 GB throughput test, please be patient...\n"));
        oneGigResult = Run(pages, pages.size() / 10);
        Print(oneGigResult);

        if (oneGigResult.elapsedSeconds <= 2)
        {
            printf(xorstr_("[i] Fast DMA Detected! Running 10 GB throughput test, please be patient...\n"));
            tenGigResult = Run(pages, pages.size());
            Print(tenGigResult);
        }
    }
}