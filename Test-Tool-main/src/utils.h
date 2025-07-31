#pragma once

namespace utils
{
    static std::string RandomString(size_t length)
    {
        const std::string characters = xorstr_("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789");

        std::random_device randomDevice;
        std::mt19937 generator(randomDevice());
        std::uniform_int_distribution<size_t> distribution(0, characters.size() - 1);

        std::string randomString;
        for (size_t i = 0; i < length; ++i)
            randomString += characters[distribution(generator)];

        return randomString;
    }

    static bool HasMoreThanOneDash(const std::string& s)
    {
        int count = 0;
        for (char c : s)
        {
            if (c == '-')
            {
                count++;
                if (count > 1) return true;
            }
        }

        return false;
    }

    template<typename T> std::string HumanizeNumber(T number, int decimalPlaces = -1)
    {
        std::ostringstream stream;

        if constexpr (std::is_floating_point<T>::value) {
            if (decimalPlaces >= 0) {
                stream << std::fixed << std::setprecision(decimalPlaces);
            }
            stream << number;
        }
        else {
            stream << number;
        }

        std::string numberStr = stream.str();
        int insertPosition = numberStr.find('.');
        if (insertPosition == std::string::npos) insertPosition = numberStr.length();

        insertPosition -= 3;
        while (insertPosition > 0)
        {
            numberStr.insert(insertPosition, xorstr_(","));
            insertPosition -= 3;
        }

        return numberStr;
    }

    static int PromptNumber(std::string prompt, int min, int max, int defaultVal = -1)
    {
        int output = -1;

        while (true)
        {
            std::string input;

            if (defaultVal != -1) std::cout << xorstr_("[?] ") << prompt << xorstr_(" [default: ") << defaultVal << xorstr_("]") << xorstr_(": ");
            else std::cout << xorstr_("[?] ") << prompt << xorstr_(": ");

            std::getline(std::cin, input);

            if (input.empty())
            {
                if (defaultVal == -1)
                {
                    std::cout << dye::red(xorstr_("[!] Empty response!")) << std::endl;
                    continue;
                }
                else return defaultVal;
            }

            bool wasAllDigits = true;
            for (char ch : input)
            {
                if (!isdigit(ch))
                {
                    wasAllDigits = false;
                    break;
                }
            }
            if (!wasAllDigits)
            {
                std::cout << dye::red(xorstr_("[!] Invalid input! Only digits are allowed.")) << std::endl;
                continue;
            }

            int tmpInt = std::stoi(input);

            if (tmpInt > max)
            {
                std::cout << dye::red(xorstr_("[!] Invalid input! The maximum value is ")) << dye::red(max) << dye::red(xorstr_(".")) << std::endl;
                continue;
            }

            if (tmpInt < min)
            {
                std::cout << dye::red(xorstr_("[!] Invalid input! The minimum value is ")) << dye::red(min) << dye::red(xorstr_(".")) << std::endl;
                continue;
            }

            output = tmpInt;
            break;
        }

        return output;
    }
}