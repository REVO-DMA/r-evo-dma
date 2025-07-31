#pragma once

namespace rps
{
    static void Print(int rps, bool full = false)
    {
        const int godlikeThreshold = 45000;
        const int vipThreshold = 16000;
        const int eliteThreshold = 7600;
        const int amazingThreshold = 6300;
        const int goodThreshold = 5000;
        const int warningThreshold = 3600;

        const std::string prefix = xorstr_("- Reads Per Second (RPS): ");

        std::string humanRPS = utils::HumanizeNumber(rps);

        if (rps >= godlikeThreshold)
            if (full) std::cout << prefix << dye::bright_white(humanRPS) << " " << dye::on_bright_white(xorstr_("(GODLIKE)")) << std::endl;
            else std::cout << dye::bright_white(humanRPS) << std::endl;
        else if (rps >= vipThreshold)
            if (full) std::cout << prefix << dye::blue(humanRPS) << " " << dye::on_blue(xorstr_("(VIP)")) << std::endl;
            else std::cout << dye::blue(humanRPS) << std::endl;
        else if (rps >= eliteThreshold)
            if (full) std::cout << prefix << dye::aqua(humanRPS) << " " << dye::on_aqua(xorstr_("(ELITE)")) << std::endl;
            else std::cout << dye::aqua(humanRPS) << std::endl;
        else if (rps >= amazingThreshold)
            if (full) std::cout << prefix << dye::purple(humanRPS) << " " << dye::on_purple(xorstr_("(AMAZING)")) << std::endl;
            else std::cout << dye::purple(humanRPS) << std::endl;
        else if (rps >= goodThreshold)
            if (full) std::cout << prefix << dye::green(humanRPS) << " " << dye::on_green(xorstr_("(GOOD)")) << std::endl;
            else std::cout << dye::green(humanRPS) << std::endl;
        else if (rps >= warningThreshold)
            if (full) std::cout << prefix << dye::yellow(humanRPS) << " " << dye::on_yellow(xorstr_("(WARNING)")) << std::endl;
            else std::cout << dye::yellow(humanRPS) << std::endl;
        else
            if (full) std::cout << prefix << dye::red(humanRPS) << " " << dye::on_red(xorstr_("(LOW)")) << std::endl;
            else std::cout << dye::red(humanRPS) << std::endl;
    }

    static void Test(std::vector<ULONG64> pages, bool stressTest = false)
    {
        using namespace std::chrono;

        nanoseconds fastestRead = (nanoseconds::max)();
        nanoseconds slowestRead = (nanoseconds::min)();

        nanoseconds totalTime(0);

        ULONG64 speedTestTotalReads = 0;
        ULONG64 speedTestFailedReads = 0;

        ULONG64 totalMemSec = 0;

        if (stressTest)
            std::cout << xorstr_("[i] Running stress test for ") << settings::SpeedTestDuration << xorstr_(" seconds...\n") << std::endl;
        else
            std::cout << xorstr_("[i] Running speed test for ") << settings::SpeedTestDuration << xorstr_(" seconds...\n") << std::endl;

        std::random_device rd;
        std::mt19937 gen(rd());
        std::uniform_int_distribution<size_t> dis(0, pages.size() - 1);

        PBYTE pBuffer = new BYTE[settings::SpeedTestReadSize];
        DWORD cbRead;

        for (int i = 0; i < settings::SpeedTestDuration; i++)
        {
            auto start = high_resolution_clock::now();
            auto end = start + seconds(1);

            UINT memSec = 0;

            while (true)
            {
                auto start = high_resolution_clock::now();
                if (start >= end)
                    break;

                ULONG64 randomMemoryPage = pages[dis(gen)];

                if (VMMDLL_MemReadEx(dma::hVmm, -1, randomMemoryPage, pBuffer, settings::SpeedTestReadSize, &cbRead, dma::READ_FLAGS) && cbRead > 0)
                    memSec++;
                else
                    speedTestFailedReads++;

                speedTestTotalReads++;

                auto duration = high_resolution_clock::now() - start;

                if (duration < fastestRead) fastestRead = duration;
                if (duration > slowestRead) slowestRead = duration;

                totalTime += duration;
            }

            totalMemSec += memSec;

            std::cout << xorstr_("[");
            std::cout << std::setw(std::to_string(settings::SpeedTestDuration).size()) << std::setfill('0') << i + 1;
            std::cout << xorstr_("/") << settings::SpeedTestDuration << xorstr_("s]: ");
            Print(memSec);
        }

        delete[] pBuffer;

        // Perform calculations

        ULONG64 averageSpeed = totalMemSec / settings::SpeedTestDuration;

        auto averageLatency = totalTime / speedTestTotalReads;
        
        double failedReadsPercentage = (static_cast<double>(speedTestFailedReads) / static_cast<double>(speedTestTotalReads)) * static_cast<double>(100);

        // Print results

        printf(xorstr_("\nResults:\n"));

        std::cout << xorstr_("- Total Reads: ") << utils::HumanizeNumber(speedTestTotalReads) << std::endl;

        if (speedTestFailedReads > 0)
            std::cout << xorstr_("- Failed Reads: ") << dye::red(speedTestFailedReads) << xorstr_(" (") << utils::HumanizeNumber(failedReadsPercentage, 2) << xorstr_("%)") << std::endl;

        printf(xorstr_("\n"));

        const auto us = xorstr_(" \xE6s");

        std::cout << xorstr_("- Slowest Read: ") << utils::HumanizeNumber(duration_cast<microseconds>(slowestRead).count()) << us << std::endl;
        std::cout << xorstr_("- Fastest Read: ") << utils::HumanizeNumber(duration_cast<microseconds>(fastestRead).count()) << us << std::endl;
        std::cout << xorstr_("- AVG. Latency: ") << utils::HumanizeNumber(duration_cast<microseconds>(averageLatency).count()) << us << std::endl;

        printf(xorstr_("\n"));

        Print(averageSpeed, true);

        printf(xorstr_("\n"));
    }
}