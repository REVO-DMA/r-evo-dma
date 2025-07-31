#pragma once

namespace MonoClass
{
    __forceinline ulong GetSuperClass(ulong classAddr)
    {
        ulong superClass = ReadPtr(classAddr + 0x30);
        RETURN_0_IF_INVALID(superClass);

        return superClass;
    }

    __forceinline ulong GetClass(ulong addr)
    {
        ulong p1 = ReadPtr(addr + 0x0);
        RETURN_0_IF_INVALID(p1);

        ulong p2 = ReadPtr(p1 + 0x0);
        RETURN_0_IF_INVALID(p2);

        return p2;
    }

    __forceinline const char* GetName(ulong superClass)
    {
        ulong pClassName = ReadPtr(superClass + 0x48);
        if (IS_INVALID_ADDR(pClassName)) return nullptr;

        return (char*)pClassName;
    }

    __forceinline bool IsA(ulong addr, const char* className)
    {
        int loop_iter = 0;

        for (ulong super = GetClass(addr); super; super = GetSuperClass(super))
        {
            if (loop_iter++ == 5)
                break;

            if (Utils::strcmp(GetName(super), className))
                return true;
        }

        return false;
    }
}