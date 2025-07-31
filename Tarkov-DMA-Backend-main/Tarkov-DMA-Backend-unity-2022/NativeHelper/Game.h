#pragma once

namespace Game
{
    using Generic4Param_t = uintptr_t(*)(uintptr_t, uintptr_t, uintptr_t, uintptr_t);
    
    std::mutex mtx;

    uintptr_t CodeCave = 0x0;
    int HookIndex = -1;
    uintptr_t ShellCodeAddress;
    uintptr_t MonoRuntimeInvokeAddress = 0x0;
    Generic4Param_t OriginalMonoRuntimeInvokeFunction = 0x0;

    struct ShellCodeData_t
    {
        Generic4Param_t calledFunction;
        uintptr_t rcx;
        uintptr_t rdx;
        uintptr_t r8;
        uintptr_t r9;

        bool executed;
        uintptr_t result;

        uintptr_t monoInvokeAddress;
        Generic4Param_t monoInvokeOriginalFunc;
    };

#pragma optimize("", off)

    uintptr_t InvokeHook(uintptr_t rcx, uintptr_t rdx, uintptr_t r8, uintptr_t r9)
    {
        ShellCodeData_t* data = (ShellCodeData_t*)0xFAFAFAFAFAFAFAFA;

        if (!data->executed)
        {
            *(uintptr_t*)(data->monoInvokeAddress) = (uintptr_t)data->monoInvokeOriginalFunc;

            data->result = data->calledFunction(data->rcx, data->rdx, data->r8, data->r9);
            data->executed = true;
        }

        return data->monoInvokeOriginalFunc(rcx, rdx, r8, r9);
    }

#pragma optimize("", on)

    int GetHookIndex()
    {
        for (int i = 0; i < 0x120; i++)
        {
            auto& qw = *(uintptr_t*)((uintptr_t)InvokeHook + i);

            if (qw == 0xFAFAFAFAFAFAFAFA)
                return i;
        }

        return -1;
    }

    BOOL Initialize()
    {
        if (!CodeCave || HookIndex == -1)
        {
            LOG(xorstr_("[GAME]: Initialize() -> Invalid CodeCave and/or HookIndex."));
            return false;
        }

        MonoRuntimeInvokeAddress = DMA::UnityPlayerDLL + Offsets::mono_runtime_invoke;

        auto& qw = *(uintptr_t*)((uintptr_t)InvokeHook + HookIndex);
        qw = CodeCave;

        ShellCodeAddress = CodeCave + 100;

        auto origMonoInvoke = DMA::ReadPtr(MonoRuntimeInvokeAddress);
        if (origMonoInvoke.has_value())
            OriginalMonoRuntimeInvokeFunction = (Generic4Param_t)origMonoInvoke.value();
        else
            return false;

        if (!DMA::WriteBuffer(ShellCodeAddress, InvokeHook, 0x100))
            return false;

        return true;
    }

    uintptr_t Call(uintptr_t function, uintptr_t rcx = 0, uintptr_t rdx = 0, uintptr_t r8 = 0, uintptr_t r9 = 0)
    {
        std::lock_guard<std::mutex> lock(mtx);

        if (!CodeCave || HookIndex == -1 || !function)
        {
            LOG(xorstr_("[GAME]: Call() -> Invalid input."));
            return 0;
        }

        ShellCodeData_t data = {};
        data.calledFunction = (Generic4Param_t)function;
        data.rcx = rcx;
        data.rdx = rdx;
        data.r8 = r8;
        data.r9 = r9;

        data.monoInvokeAddress = MonoRuntimeInvokeAddress;
        data.monoInvokeOriginalFunc = OriginalMonoRuntimeInvokeFunction;

        if (!DMA::WriteBuffer(CodeCave, &data, sizeof(ShellCodeData_t)))
            return 0;

        if (!DMA::WriteBuffer(MonoRuntimeInvokeAddress, &ShellCodeAddress, sizeof(uintptr_t)))
            return 0;

        int tries = 0;
        while (true)
        {
            Sleep(8);

            auto inputRes = DMA::ReadValue<ShellCodeData_t>(CodeCave);
            if (inputRes.has_value())
                data = inputRes.value();
            
            if (data.executed)
                break;

            if (tries++ == 600) // Wait for up to 4.8 seconds
            {
                LOG(xorstr_("[GAME]: Call() -> Method was never executed."));
                break;
            }
        }

        return data.result;
    }
}