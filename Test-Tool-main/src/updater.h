#pragma once

namespace updater
{
    #define VERSION xorstr_("1.6")

    const std::string UPDATE_URL = xorstr_("https://updates.evodma.com/test-tool/version");

    struct UpdateData
    {
        std::string newestVersion;
        bool isOutdated;
    };

    static UpdateData Check()
    {
        printf(xorstr_("[i] Checking for updates...\n\n"));

        std::string updateURL = UPDATE_URL + xorstr_("?v=") + utils::RandomString(16);
        
        bool isOutdated = false;
        std::string newestVersion;

        HINTERNET hInternet, hConnect;
        DWORD bytesRead;
        std::string responseData;

        DWORD statusCode = 0;
        DWORD bufferSize = sizeof(statusCode);

        hInternet = InternetOpenW(xorstr_(L"EVO DMA Test Tool"), INTERNET_OPEN_TYPE_DIRECT, NULL, NULL, 0);
        if (!hInternet)
        {
            std::cout << dye::red(xorstr_("[!] Update check failed: no hInternet!")) << std::endl;
            goto fail;
        }

        hConnect = InternetOpenUrlA(hInternet, updateURL.c_str(), NULL, 0, INTERNET_FLAG_RELOAD | INTERNET_FLAG_SECURE | INTERNET_FLAG_DONT_CACHE, 0);
        if (!hConnect)
        {
            std::cout << dye::red(xorstr_("[!] Update check failed: no hConnect!")) << std::endl;
            InternetCloseHandle(hInternet);
            goto fail;
        }

        if (!HttpQueryInfoA(hConnect, HTTP_QUERY_STATUS_CODE | HTTP_QUERY_FLAG_NUMBER, &statusCode, &bufferSize, NULL))
        {
            std::cout << dye::red(xorstr_("[!] Update check failed: no status code!")) << std::endl;
            InternetCloseHandle(hConnect);
            InternetCloseHandle(hInternet);
            goto fail;
        }

        if (statusCode != 200)
        {
            std::cout << dye::red(xorstr_("[!] Update check failed: invalid status code!")) << std::endl;
            InternetCloseHandle(hConnect);
            InternetCloseHandle(hInternet);
            goto fail;
        }

        char buffer[4096];
        while (InternetReadFile(hConnect, buffer, sizeof(buffer), &bytesRead) && bytesRead > 0)
            responseData.append(buffer, bytesRead);

        InternetCloseHandle(hConnect);
        InternetCloseHandle(hInternet);

        newestVersion = responseData;

        if (version_compare2(VERSION, responseData.c_str()) == -1) isOutdated = true;

        if (!isOutdated) printf(xorstr_("[i] Application is up-to-date!\n\n"));

        return
        {
            newestVersion,
            isOutdated
        };

    fail:
        return
        {
            std::string(xorstr_("N/A")),
            false
        };
    }
}