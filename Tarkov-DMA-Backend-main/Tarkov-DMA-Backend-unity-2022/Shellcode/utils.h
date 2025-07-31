#pragma once

namespace Utils
{
    __forceinline bool strcmp(const char* str1, const char* str2)
    {
        if (str1 == nullptr && str2 == nullptr)
            return true;

        if (str1 == nullptr || str2 == nullptr)
            return false;

        while (*str1 != '\0' && *str2 != '\0')
        {
            if (*str1 != *str2)
                return false;

            ++str1;
            ++str2;
        }

        return *str1 == '\0' && *str2 == '\0';
    }
}
