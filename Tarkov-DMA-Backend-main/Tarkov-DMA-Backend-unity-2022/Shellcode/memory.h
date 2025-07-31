#pragma once

template <typename T> __forceinline T ReadValue(ulong addr)
{
    return *(T*)(addr);
}

template <typename T> __forceinline void WriteValue(ulong addr, T value)
{
    *(T*)(addr) = value;
}

__forceinline ulong ReadPtr(ulong ptr)
{
    return ReadValue<ulong>(ptr);
}