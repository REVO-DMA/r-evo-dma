#include "global.h"

INT EntryPoint()
{
    NTSTATUS ntStatus;
    DWORD exitProcessCode;

    RtlSetUnhandledExceptionFilter(supUnhandledExceptionFilter);
    HeapSetInformation(NtCurrentPeb()->ProcessHeap, HeapEnableTerminationOnCorruption, NULL, 0);
    supSetMitigationPolicies();
    supCacheKnownDllsEntries();
    ntStatus = supInitializeKnownSids();
    if (!NT_SUCCESS(ntStatus)) {
        exitProcessCode = ntStatus;
    }
    else {
        exitProcessCode = 0;
        SkStartProbe();
    }
    return exitProcessCode;
}

int main()
{
    return EntryPoint();
}