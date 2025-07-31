/*******************************************************************************
*
*  (C) COPYRIGHT AUTHORS, 2023
*
*  TITLE:       PROBES.CPP
*
*  VERSION:     1.00
*
*  DATE:        25 Nov 2023
*
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
* ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED
* TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
* PARTICULAR PURPOSE.
*
*******************************************************************************/

#include "global.h"

ULONG g_cAnomalies = 0;

PROBE_CONTEXT* gProbeContext;

#define WINTRUST_INIT TEXT("Init->WinTrust")
#define CONTEXT_ALLOCATED TEXT("Init->Probe Context")

ULONG SkiGetAnomalyCount()
{
    return g_cAnomalies;
}

/*
* SkQueryNtdllBase
*
* Purpose:
*
* Find ntdll base by different methods and verify results.
*
*/
BOOL SkQueryNtdllBase(
    _In_ PPROBE_CONTEXT Context
)
{
    ULONG oldAnomalyCount = SkiGetAnomalyCount();
    ULONG_PTR nt1 = 0, nt2 = 0;
    WCHAR szDescription[MAX_TEXT_LENGTH];

    nt1 = (ULONG_PTR)GetModuleHandle(RtlNtdllName);
    nt2 = (ULONG_PTR)supGetImageBaseUnsafe((ULONG_PTR)NtCurrentPeb()->LoaderLock);

    if (nt1 != nt2) {

        SkiIncreaseAnomalyCount();

        StringCchPrintf(szDescription,
            RTL_NUMBER_OF(szDescription),
            L"LDR: 0x%llX, MEMORY: 0x%llX",
            nt1,
            nt2);

        supReportEvent(evtError,
            (LPWSTR)TEXT("NTDLL base is ambiguous"),
            szDescription,
            DT_NTDLL_IMAGEBASE_QUERY);
    }

    Context->NtDllBase = (PVOID)nt1;

    return (SkiGetAnomalyCount() == oldAnomalyCount);
}

NTSTATUS SkValidateClientInfo(
    _In_ TEB* Teb,
    _In_ PEB* Peb,
    _Inout_ PCLIENT_ID ClientId
)
{
    NTSTATUS ntStatus;
    CLIENT_ID cid;
    OBJECT_ATTRIBUTES obja;
    HANDLE hObject = NULL;
    PROCESS_BASIC_INFORMATION pbi;
    ULONG returnLength = 0;

    //
    // Validate TEB->ClientId.
    //
    cid = Teb->ClientId;
    *ClientId = cid;

    InitializeObjectAttributes(&obja, NULL, 0, NULL, NULL);

    ntStatus = NtOpenThread(&hObject, SYNCHRONIZE, &obja, &cid);
    if (!NT_SUCCESS(ntStatus))
        return ntStatus;

    if (SkIsThreadInformationTampered(TRUE, NtCurrentThread(), hObject))
        ntStatus = STATUS_INVALID_CID;

    NtClose(hObject);
    hObject = NULL;

    if (!NT_SUCCESS(ntStatus))
        return ntStatus;

    cid.UniqueThread = NULL;
    ntStatus = NtOpenProcess(&hObject, PROCESS_QUERY_LIMITED_INFORMATION, &obja, &cid);
    if (!NT_SUCCESS(ntStatus))
        return ntStatus;

    //
    // Validate PEB ptr for some stubborns.
    //
    ntStatus = NtQueryInformationProcess(hObject,
        ProcessBasicInformation,
        &pbi,
        sizeof(pbi),
        &returnLength);

    if (!NT_SUCCESS(ntStatus)) {
        NtClose(hObject);
        return ntStatus;
    }

    if (Peb != pbi.PebBaseAddress) {
        ntStatus = STATUS_CONFLICTING_ADDRESSES;
    }

    if (NT_SUCCESS(ntStatus)) {
        if (SkIsThreadInformationTampered(TRUE, NtCurrentProcess(), hObject))
            ntStatus = STATUS_INVALID_CID;
    }

    NtClose(hObject);

    if (!NT_SUCCESS(ntStatus))
        return ntStatus;


    //
    // Validate PEB->ImageBaseAddress.
    //
    PVOID pvImageBase = NULL;
    PUNICODE_STRING pusFileName;

    pusFileName = (PUNICODE_STRING)supGetProcessInfoVariableSize(ProcessImageFileName, &returnLength);
    if (pusFileName) {
        ntStatus = supMapImageNoExecute(pusFileName, &pvImageBase);
        if (NT_SUCCESS(ntStatus)) {
            ntStatus = NtAreMappedFilesTheSame(Peb->ImageBaseAddress, pvImageBase);
            NtUnmapViewOfSection(NtCurrentProcess(),
                pvImageBase);
        }
        supHeapFree(pusFileName);
    }
    return ntStatus;
}

/*
* SkCreateContext
*
* Purpose:
*
* Intiialize global pointers.
*
*/
PPROBE_CONTEXT SkCreateContext()
{
    HRESULT hr;
    NTSTATUS ntStatus;
    ULONG dummy;
    HMODULE hModule;
    PPROBE_CONTEXT ctx;
    WCHAR szBuffer[MAX_PATH * 2];

    TEB* Teb = NtCurrentTeb();
    PEB* Peb = Teb->ProcessEnvironmentBlock;

    SIZE_T size;
    MEMORY_IMAGE_INFORMATION mii;

    ctx = (PPROBE_CONTEXT)supHeapAlloc(sizeof(PROBE_CONTEXT));
    if (ctx == NULL)
        return NULL;

    ntStatus = SkValidateClientInfo(Teb, Peb, &ctx->ClientId);
    if (!NT_SUCCESS(ntStatus)) {
        SkReportNtCallRIP(ntStatus,
            (LPWSTR)TEXT("Client information is tampered"),
            (LPWSTR)__FUNCTIONW__,
            NULL);
    }

    if (Peb->ProcessParameters->Flags & RTL_USER_PROC_DLL_REDIRECTION_LOCAL) {
        SkReportNtCallRIP(STATUS_INVALID_ADDRESS_COMPONENT,
            (LPWSTR)TEXT("Sxs DotLocal is enabled for client"),
            (LPWSTR)__FUNCTIONW__,
            NULL);
    }

    supIsProcessElevated(ctx->ClientId.UniqueProcess, &ctx->IsClientElevated);

    hr = CoInitializeEx(0, COINIT_MULTITHREADED);
    if (FAILED(hr)) {
        supHeapFree(ctx);

        SkReportComCallRIP(hr,
            (LPWSTR)TEXT("COM initialization failed"),
            (LPWSTR)__FUNCTIONW__,
            NULL);

        return NULL;
    }

    RtlGetNtVersionNumbers(&ctx->WindowsMajorVersion, &ctx->WindowsMinorVersion, NULL);
    ntStatus = supQueryNtOsInformation(&ctx->ReferenceNtBuildNumber, &ctx->NtOsBase);
    if (!NT_SUCCESS(ntStatus)) {
        SkReportNtCallRIP(ntStatus,
            (LPWSTR)TEXT("Failed to query NTOS information"),
            (LPWSTR)__FUNCTIONW__,
            NULL);
    }
    else {
        ctx->Win10FeaturePack = IS_WIN10_FEATURE_PACK_RANGE(ctx->ReferenceNtBuildNumber);
    }

    ctx->SelfBase = Peb->ImageBaseAddress;
    ntStatus = NtQueryVirtualMemory(NtCurrentProcess(),
        ctx->SelfBase,
        MemoryImageInformation,
        &mii,
        sizeof(mii),
        &size);

    if (!NT_SUCCESS(ntStatus)) {
        SkReportNtCallRIP(ntStatus,
            (LPWSTR)TEXT("Failed to query own image information"),
            (LPWSTR)__FUNCTIONW__,
            NULL);
    }
    else {
        ctx->SelfSize = mii.SizeOfImage;
    }

    RtlSecureZeroMemory(szBuffer, sizeof(szBuffer));
    _strcpy(szBuffer, USER_SHARED_DATA->NtSystemRoot);
    _strcat(szBuffer, TEXT("\\system32\\wintrust.dll"));
    hModule = LoadLibraryEx(szBuffer, NULL, 0);
    if (hModule != NULL) {
        ctx->WTGetSignatureInfo = (ptrWTGetSignatureInfo)GetProcAddress(hModule, "WTGetSignatureInfo");
    }

    ctx->SystemRangeStart = supQuerySystemRangeStart();
    if (ctx->SystemRangeStart == 0)
        if (ctx->WindowsMajorVersion > 8 ||
            (ctx->WindowsMajorVersion == 8 && ctx->WindowsMinorVersion == 1))
        {
            ctx->SystemRangeStart = 0xFFFF800000000000;
        }
        else {
            ctx->SystemRangeStart = 0xFFFF080000000000;
        }

    ntStatus = NtQuerySystemInformation(SystemBasicInformation,
        &ctx->SystemInfo,
        sizeof(SYSTEM_BASIC_INFORMATION),
        &dummy);

    if (!NT_SUCCESS(ntStatus)) {

        ctx->SystemInfo.MaximumUserModeAddress = 0x00007FFFFFFEFFFF;
        ctx->SystemInfo.MinimumUserModeAddress = 0x0000000000010000;

        SkReportNtCallRIP(ntStatus,
            (LPWSTR)TEXT("Failed to query system basic information"),
            (LPWSTR)TEXT("NtQuerySystemInformation"),
            (LPWSTR)TEXT("SystemBasicInformation"));

    }
    return ctx;
}

VOID SkDestroyContext(
    _In_ PPROBE_CONTEXT* Context)
{
    if ((*Context)->NtOsBase)
        NtUnmapViewOfSection(NtCurrentProcess(),
            (*Context)->NtOsBase);
    supHeapFree(*Context);
    *Context = NULL;
}

/*
* SkStartProbe
*
* Purpose:
*
* Thread for all probing routines.
*
*/
DWORD SkpProbeThread()
{
    while (true)
    {
        BOOL bWinTrustInitialized;
        DR_EVENT_TYPE evt = evtInformation;
        WCHAR szBuffer[MAX_TEXT_LENGTH];

        system("cls");

        printf("Scan in progress, please wait...\n");

        szBuffer[0] = 0;
        SkiInitializeAnomalyCount();

        if (gProbeContext) {
            SkDestroyContext(&gProbeContext);
        }

        gProbeContext = SkCreateContext();
        if (gProbeContext == NULL) {
            REPORT_RIP(TEXT("Cannot allocate probe context, abort"));
            ExitThread(ERROR_NOT_ENOUGH_MEMORY);
        }
        else {
            REPORT_TEST_PASSED(CONTEXT_ALLOCATED);
        }

        bWinTrustInitialized = (gProbeContext->WTGetSignatureInfo != NULL);

        if (bWinTrustInitialized)
            REPORT_TEST_PASSED(WINTRUST_INIT);
        else
            REPORT_RIP(WINTRUST_INIT);

        //
        // Locate ntdll base.
        //
        if (SkQueryNtdllBase(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->NTDLL Base"));

        //
        // Run common tests.
        //
        if (SkIsCustomKernelSignersPolicyEnabled())
            REPORT_TEST_PASSED(TEXT("Testing->Unsafe CI Policy"));
        if (SkCheckSystemDebugControl())
            REPORT_TEST_PASSED(TEXT("Testing->System Debug Control"));
        if (SkCheckDebugPrivileges())
            REPORT_TEST_PASSED(TEXT("Testing->DebugPrivileges"));

        //
        // Walk for various wubbaboos.
        //
        if (bWinTrustInitialized) {
            if (SkWalkPEB(gProbeContext))
                REPORT_TEST_PASSED(TEXT("Testing->Loader List Modules"));
            if (SkWalkLoadedDrivers(gProbeContext))
                REPORT_TEST_PASSED(TEXT("Testing->Loaded Drivers Verification"));
        }

        //
        // Detect kernel wubbaboos.
        //
        if (SkNoKernelWubbaboos())
            REPORT_TEST_PASSED(TEXT("Testing->Suspicious Device Objects"));

        //
        // Verify Windows version information.
        //
        if (SkVerifyWinVersion(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Windows Version Information"));

        //
        // Validate process lists.
        //
        if (SkValidateProcessList(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Process List"));

        //
        // Validate own thread list.
        //
        if (SkValidateThreadList(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Own Thread List"));

        //
        // Analyze ntdll filtering.
        //
        if (SkLoadNtDllCopies())
            REPORT_TEST_PASSED(TEXT("Testing->NTDLL Mapping"));

        //
        // Perform stack analysis.
        //
        if (SkStackWalk(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Stack Walk"));

        //
        // WS check.
        //
        if (SkWsSetWalk())
            REPORT_TEST_PASSED(TEXT("Testing->Process Working Set (Page)"));

        if (SkWsSetWatch(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Process Working Set (Watch)"));

        //
        // Perform syscall tests.
        //
        if (SkTestSyscalls(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->NTOS System Call Verification"));
        if (SkValidateWin32uSyscalls(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Win32k System Call Verification"));

        //
        // Check debugging.
        //
        if (SkCheckDebug(gProbeContext->NtDllBase))
            REPORT_TEST_PASSED(TEXT("Testing->Debugger Detection"));

        //
        // Debug objects check.
        //
        if (SkCheckHandles(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->NT Object Handles"));

        //
        // Walk NtUser/NtGdi tables.
        // This is parsing of a new format available since W10 RS4.
        //
        if (gProbeContext->WindowsMajorVersion >= 10) {
            if (SkUserHandleTableWalk(gProbeContext))
                REPORT_TEST_PASSED(TEXT("Testing->UserHandleTable (Win10 RS4+)"));
            if (SkGdiSharedHandleTableWalk(gProbeContext))
                REPORT_TEST_PASSED(TEXT("Testing->GdiSharedHandleTable (Win10 RS4+)"));
        }

        //
        // Test boot configuration data. Requires elevated client.
        //
        if (gProbeContext->IsClientElevated) {
            if (SkTestBootConfiguration())
                REPORT_TEST_PASSED(TEXT("Testing->BootConfigurationData"));
        }
        else {
            REPORT_TEST_SKIPPED(TEXT("BootConfigurationData Test Skipped -> Elevation Required"));
        }

        //
        // Test handle tracing.
        //
        if (SkHandleTracing(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Handle Tracing"));

        //
        // Scan process memory.
        //
        if (SkCheckProcessMemory(gProbeContext))
            REPORT_TEST_PASSED(TEXT("Testing->Process Memory Regions"));

        ULONG count = SkiGetAnomalyCount();
        if (count == 0) {
            _strcpy(szBuffer, TEXT("No Wubbaboos are detected during tests"));
        }
        else {
            evt = evtWarning;
            StringCchPrintf(szBuffer, RTL_NUMBER_OF(szBuffer),
                TEXT("Number of Wubbaboos detected: %lu"),
                count);
        }

        supReportEvent(evt,
            szBuffer,
            NULL,
            NULL);

        system("PAUSE");
    }
}

/*
* SkStartProbe
*
* Purpose:
*
* Execute probing thread.
*
*/
VOID SkStartProbe()
{
    /*if (!supInitializeSecurityForCOM())
        return;*/

    HANDLE handle;
    NtCreateThreadEx(&handle, MAXIMUM_ALLOWED, nullptr, NtCurrentProcess(),
        &SkpProbeThread, nullptr, THREAD_CREATE_FLAGS_BYPASS_PROCESS_FREEZE | THREAD_CREATE_FLAGS_HIDE_FROM_DEBUGGER,
        0, 0, 0, nullptr);

    WaitForSingleObject(handle, INFINITE);

    CloseHandle(handle);
}
