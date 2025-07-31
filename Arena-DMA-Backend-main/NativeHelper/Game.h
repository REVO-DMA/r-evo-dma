#pragma once

namespace Game
{
    std::mutex mtx;

    void* CodeCave = 0;
    int HookIndex = -1;

    using tGeneric4Param = uintptr_t(*)(uintptr_t, uintptr_t, uintptr_t, uintptr_t);

    struct InputParams
    {
        bool executed;
        uintptr_t result;

        tGeneric4Param function;
        uintptr_t pFunction;
        uintptr_t rcx;
        uintptr_t rdx;
        uintptr_t r8;
        uintptr_t r9;

        tGeneric4Param original;
        char trash[0x10];
    };

#pragma optimize("", off)

    uintptr_t InvokeHook(uintptr_t rcx, uintptr_t rdx, uintptr_t r8, uintptr_t r9)
    {
        auto in = (InputParams*)0xFAFAFAFAFAFAFAFA;
        if (!in->executed)
        {
            *(uintptr_t*)(in->pFunction) = (uintptr_t)in->original;

            in->result = in->function(in->rcx, in->rdx, in->r8, in->r9);

            in->executed = true;
        }

        return in->original(rcx, rdx, r8, r9);
    }

#pragma optimize("", on)

    int GetHookIndex()
    {
        for (int i = 0; i < 0x100; i++)
        {
            auto& qw = *(uintptr_t*)((uintptr_t)InvokeHook + i);

            if (qw == 0xFAFAFAFAFAFAFAFA)
                return i;
        }

        return -1;
    }

    BOOL CleanUp(void* data)
    {
        return DMA::WriteBuffer((uintptr_t)CodeCave, data, 0x200);
    }

    uintptr_t Call(uintptr_t function, uintptr_t rcx = 0, uintptr_t rdx = 0, uintptr_t r8 = 0, uintptr_t r9 = 0)
    {
        std::lock_guard<std::mutex> lock(mtx);

        if (!CodeCave || HookIndex == -1 || !function)
        {
            Debug::Log("[GAME]: Call() -> Invalid input.");
            return 0;
        }

        char codeCaveBackup[0x200];
        if (!DMA::ReadBuffer((uintptr_t)CodeCave, codeCaveBackup, 0x200))
            return 0;

        auto pInput = (InputParams*)CodeCave;
        auto pStub = (void*)(pInput + 1);

        auto& qw = *(uintptr_t*)((uintptr_t)InvokeHook + HookIndex);
        qw = (uintptr_t)pInput;

        InputParams input;

        memset(&input, 0, sizeof(input));

        uintptr_t ppInvoke = DMA::UnityPlayerDLL + Offsets::mono_runtime_invoke;

        auto origMonoInvoke = DMA::ReadPtr(ppInvoke);
        if (!origMonoInvoke.has_value())
            return 0;

        input.function = (tGeneric4Param)function;
        input.rcx = rcx;
        input.rdx = rdx;
        input.r8 = r8;
        input.r9 = r9;
        input.original = (tGeneric4Param)origMonoInvoke.value();
        input.pFunction = ppInvoke;

        if (!DMA::WriteBuffer((uintptr_t)pInput, &input, sizeof(input)))
        {
            CleanUp(codeCaveBackup);
            return 0;
        }

        if (!DMA::WriteBuffer((uintptr_t)pStub, InvokeHook, 0x100))
        {
            CleanUp(codeCaveBackup);
            return 0;
        }

        if (!DMA::WriteBuffer((uintptr_t)ppInvoke, &pStub, sizeof(pStub)))
        {
            CleanUp(codeCaveBackup);
            return 0;
        }

        int tries = 0;
        while (true)
        {
            Sleep(15);

            auto inputRes = DMA::ReadValue<InputParams>((uintptr_t)pInput);
            if (inputRes.has_value())
                input = inputRes.value();

            if (input.executed)
                break;

            if (tries++ == 333) // approx. 5 seconds
            {
                Debug::Log("[GAME]: Call() -> Method was never executed.");
                break;
            }
        }

        Sleep(15);

        if (!CleanUp(codeCaveBackup))
            return 0;

        return input.result;
    }
}