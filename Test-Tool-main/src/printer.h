#pragma once

namespace printer
{
    static bool AppInfoInitialized = false;
    static updater::UpdateData UpdateData;

    static void AppInfo()
    {
        if (!AppInfoInitialized)
        {
            UpdateData = updater::Check();
            AppInfoInitialized = true;
        }

        if (UpdateData.isOutdated)
        {
            std::cout << dye::red(xorstr_("[!] Application is outdated! Please download version ")) << dye::green(UpdateData.newestVersion) << dye::red(xorstr_(" from the #downloads channel in discord.")) << std::endl;
            std::cout << xorstr_("[i] EVO DMA Test Tool v") << VERSION << dye::red(xorstr_(" (OUTDATED)")) << xorstr_("\n\n");
        }
        else
        {
            std::cout << xorstr_("[i] EVO DMA Test Tool v") << VERSION << xorstr_("\n\n");
        }
    }

    static void PressToContinue(bool clear = true)
    {
        printf(xorstr_("Press the space bar to continue..."));
        _getch();

        if (clear) system(xorstr_("cls"));
    }

    static void PrintTroubleshootingGuide()
    {
        printf(xorstr_("- Troubleshooting steps:\n"));

        printf(xorstr_("\n"));

        printf(xorstr_("[1] Have you installed the DMA driver?\n"));
        std::cout << dye::light_green(xorstr_("\tNOTE: To copy a link select the text and right click.")) << std::endl;
        std::cout << xorstr_("\tFor most DMA cards you need the FTDI driver: ") << dye::blue(xorstr_("https://ftdichip.com/drivers/d3xx-drivers/")) << std::endl;
        std::cout << xorstr_("\tIf you have the ZDMA card: ") << dye::blue(xorstr_("https://github.com/ufrisk/pcileech-fpga/tree/master/ZDMA")) << std::endl;

        printf(xorstr_("\n"));

        printf(xorstr_("[2] Ensure both ends of the cable are securely connected.\n"));
        std::cout << xorstr_("\tMake sure you are plugged into the") << dye::green(xorstr_(" DATA ")) << xorstr_("port, not the") << dye::red(xorstr_(" UPDATE ")) << xorstr_("port.") << std::endl;

        printf(xorstr_("\n"));

        std::cout << xorstr_("[3] If there is a ") << dye::blue(xorstr_("mmap.txt")) << xorstr_(" file in the same folder as this EXE, delete it and re-run the test tool.") << std::endl;
        printf(xorstr_("\tIf you recently installed more ram or got a new game PC this probably needs to be regenerated.\n"));
        printf(xorstr_("\tThe PC's memory layout can change for other reasons as well.\n"));
        printf(xorstr_("\tIt's always good to make sure this isn't the source of your problems.\n"));

        printf(xorstr_("\n"));

        std::cout << xorstr_("[4] Power cycle your game PC (") << dye::red(xorstr_("DO NOT REBOOT!")) << xorstr_(").") << std::endl;
        printf(xorstr_("\tTurn off the PC, wait a few seconds and then turn it back on.\n"));

        printf(xorstr_("\n"));

        printf(xorstr_("[5] Has windows recently updated on your game PC?\n"));
        printf(xorstr_("\tWe have seen windows update cause issues with DMA cards before.\n"));
        printf(xorstr_("\tIf you can, roll back the last update and pause/disable updates.\n"));
        printf(xorstr_("\tAs a last resort you may reinstall Windows. We recommend Windows 10.\n"));

        printf(xorstr_("\n"));

        std::cout << xorstr_("[6] Did you recently update your BIOS? ~ Special thanks to ") << dye::black_on_white(xorstr_("ImmortalTech")) << xorstr_(" for this info!") << std::endl;
        std::cout << dye::red(xorstr_("\tThese are only recommended BIOS settings. None of these are actually required.")) << std::endl;
        std::cout << xorstr_("\t[") << dye::light_red(xorstr_("AMD")) << xorstr_("] Disable Virtualization & SVM.") << std::endl;
        std::cout << xorstr_("\t[") << dye::light_red(xorstr_("AMD")) << xorstr_("] Disable IOMMU (AMD CBS > NBIO Common Options > NB Configuration > IOMMU).") << std::endl;
        std::cout << xorstr_("\t[") << dye::aqua(xorstr_("INTEL")) << xorstr_("] Disable Virtualization & VT-d.") << std::endl;
        std::cout << xorstr_("\t[") << dye::aqua(xorstr_("INTEL")) << xorstr_("] Disable Intel Volume Manager.") << std::endl;
        printf(xorstr_("\tDisable Quick Start & Safe Start.\n"));
        printf(xorstr_("\tDisable NX-Bit / XD-bit (CPU Settings > Secure Virtual Machine).\n"));
        printf(xorstr_("\tEnable CSM (may help in some applications).\n"));
        printf(xorstr_("\tSet PCI Slot of Card to From Auto to Gen1 (Advanced > PCI Settings).\n"));

        printf(xorstr_("\n"));

        printf(xorstr_("[6a] Additional hardware troubleshooting:\n"));
        printf(xorstr_("\tPlace the DMA card in the first and/or last PCIe slot and retest.\n"));
        printf(xorstr_("\tMake sure your BIOS is up to date. You might also find success in downgrading the BIOS.\n"));

        printf(xorstr_("\n"));
    }

    static int PromptUser()
    {
        printf(xorstr_("Please choose the desired action:\n"));
        printf(xorstr_("[1] Quick Speed Test\n"));
        printf(xorstr_("[2] Custom Speed Test\n"));
        printf(xorstr_("[3] Throughput Test\n"));
        printf(xorstr_("[4] Mixed Test (Throughput & Speed)\n"));
        printf(xorstr_("[5] Stress Test\n"));
        printf(xorstr_("[6] Advanced Options\n"));
        printf(xorstr_("[7] Show Troubleshooting Guide\n"));
        printf(xorstr_("[8] Exit\n\n"));

        int choice = utils::PromptNumber(xorstr_("Select an action"), 1, 8);

        if (choice == 1)
        {
            printf(xorstr_("\n[i] Running a Quick Speed Test!\n\n"));

            settings::SpeedTestDuration = 10;
            settings::SpeedTestReadSize = 1;
        }
        else if (choice == 2)
        {
            printf(xorstr_("\nCustom Speed Test Settings:\n\n"));

            settings::SpeedTestDuration = utils::PromptNumber(xorstr_("Test duration (seconds)"), 1, 600, 10);
            settings::SpeedTestReadSize = utils::PromptNumber(xorstr_("Read size (bytes)"), 1, 4096, 1);

            printf(xorstr_("\n[i] Running a Custom Speed Test!\n\n"));
        }
        else if (choice == 3) printf(xorstr_("\n[i] Running a Throughput Test!\n\n"));
        else if (choice == 4) printf(xorstr_("\n[i] Running a Mixed Test (Throughput & Speed)!\n\n"));
        else if (choice == 5)
        {
            printf(xorstr_("\nStress Test Settings:\n\n"));

            settings::SpeedTestDuration = utils::PromptNumber(xorstr_("Test duration (seconds)"), 1, 600, 60);
            settings::SpeedTestReadSize = utils::PromptNumber(xorstr_("Read size (bytes)"), 1, 4096, 4096);

            printf(xorstr_("\n[i] Running a Stress Test!\n\n"));
        }
        else if (choice == 6)
        {
            printf(xorstr_("\nAdvanced Options:\n\n"));

            printf(xorstr_("Please choose the desired FPGA Algorithm:\n"));
            printf(xorstr_("[1] Auto\n"));
            printf(xorstr_("[2] Async Normal\n"));
            printf(xorstr_("[3] Async Tiny\n"));
            printf(xorstr_("[4] Old Normal\n"));
            printf(xorstr_("[5] Old Tiny\n\n"));

            settings::fpgaAlg = std::to_string(utils::PromptNumber(xorstr_("FPGA Algorithm"), 1, 5, 1) - 2);

            printf(xorstr_("\nPlease choose the desired logging mode:\n"));
            printf(xorstr_("[1] None\n"));
            printf(xorstr_("[2] Verbose\n"));
            printf(xorstr_("[3] Very Verbose\n\n"));

            int tmpVerbosity = utils::PromptNumber(xorstr_("Logging Mode"), 1, 3, 1);

            if (tmpVerbosity > 1) settings::isVerbose = true;
            else settings::isVerbose = false;

            if (tmpVerbosity == 1) settings::verbosityString = xorstr_("");
            else if (tmpVerbosity == 2) settings::verbosityString = xorstr_("-v");
            else if (tmpVerbosity == 3) settings::verbosityString = xorstr_("-vv");

            system(xorstr_("cls"));

            printf(xorstr_("\n[i] Advanced Options configured successfully!\n\n"));
        }
        else if (choice == 7)
        {
            system(xorstr_("cls"));
            PrintTroubleshootingGuide();
        }
        else if (choice == 8) printf(xorstr_("\n[i] Exiting...\n\n"));
        else
        {
            std::cout << dye::red(xorstr_("[!] Invalid choice!")) << std::endl;
            choice = -1;
        }

        return choice;
    }
}