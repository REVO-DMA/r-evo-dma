#define WIN32_LEAN_AND_MEAN

#include <windows.h>

static BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            break;
    }
    return TRUE;
}

static char ReadPool[60000];
static char WritePool[60000];
static DWORD64 IsAlive;

extern "C"
{
    __declspec(dllexport) char* GetReadPool()
    {
        return ReadPool;
    }

    __declspec(dllexport) char* GetWritePool()
    {
        return WritePool;
    }

    __declspec(dllexport) DWORD64* GetIsAlive()
    {
        return &IsAlive;
    }
}