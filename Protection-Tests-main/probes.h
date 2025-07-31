/*******************************************************************************
*
*  (C) COPYRIGHT AUTHORS, 2023
*
*  TITLE:       PROBES.H
*
*  VERSION:     1.00
*
*  DATE:        25 Nov 2023
*
*  Common header file for the program probes.
*
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
* ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED
* TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
* PARTICULAR PURPOSE.
*
*******************************************************************************/
#pragma once

typedef struct _PROBE_CONTEXT {
    BOOL IsClientElevated;
    BOOL Win10FeaturePack;
    ULONG WindowsMajorVersion;
    ULONG WindowsMinorVersion;
    ULONG ReferenceNtBuildNumber;
    HWND MainWindow;
    PVOID NtDllBase;
    PVOID NtOsBase;
    PVOID SelfBase;
    SIZE_T SelfSize;
    CLIENT_ID ClientId;
    ULONG_PTR SystemRangeStart;
    ptrWTGetSignatureInfo WTGetSignatureInfo;
    SYSTEM_BASIC_INFORMATION SystemInfo;
} PROBE_CONTEXT, * PPROBE_CONTEXT;

typedef enum _PS_SCAN_TYPE {
    ScanTypeNative,
    ScanTypeWMI,
    ScanTypeAppCompat
} PS_SCAN_TYPE;

#define SkiInitializeAnomalyCount() {  g_cAnomalies = 0; }
#define SkiIncreaseAnomalyCount() { InterlockedIncrement((PLONG)&g_cAnomalies); }

ULONG SkiGetAnomalyCount();

//
// Reports start.
//
VOID SkReportThreadOpenError(
    _In_ HANDLE WindowHandle,
    _In_ HANDLE ThreadId,
    _In_ BOOL IsClientElevated,
    _In_ NTSTATUS NtStatus);

BOOL SkIsThreadInformationTampered(
    _In_ BOOL SuppressOutput,
    _In_ HANDLE FirstObjectHandle,
    _In_ HANDLE SecondObjectHandle);

VOID SkReportHiddenProcessWindow(
    _In_ HANDLE UniqueProcessId,
    _In_ HANDLE UniqueThreadId,
    _In_ HANDLE WindowHandle);

VOID SkReportGdiObject(
    _In_ HANDLE UniqueProcessId,
    _In_ OBJTYPE ObjectType);

VOID SkReportSuspectHandleEntry(
    _In_ BOOL IsProcess,
    _In_ PSYSTEM_HANDLE_TABLE_ENTRY_INFO_EX HandleEntry);

VOID SkReportParentProcessMismatch(
    _In_ ULONG_PTR InheritedFromUniqueProcessId,
    _In_ ULONG_PTR ParentPID);

VOID SkReportExtractionFailureEvent(
    _In_ LPCSTR lpName,
    _In_opt_ LPWSTR lpDescription,
    _In_ LPWSTR lpAnomalyType);

VOID SkReportInvalidHandleClosure(
    _In_ ULONG ConditionType);

VOID SkReportThreadCountRIP();

VOID SkReportSessionIdRIP(
    _In_ ULONG SessionId);

VOID SkReportThreadUnknownRip(
    _In_ ULONG64 Rip);

VOID SkReportInvalidExtractedSSN(
    _In_ LPWSTR lpQueryType);

VOID SkReportUnexpectedSSN(
    _In_ ULONG SsnGot,
    _In_ ULONG SsnExpected);

VOID SkReportNtdllMapRIP(
    _In_ NTDLL_MAP_METHOD Method);

VOID SkReportObTypeListCorruption(
    _In_ ULONG ReportedLength,
    _In_ ULONG ActualLength);

VOID SkReportHandleListCorruption(
    _In_ ULONG ReportedLength,
    _In_ ULONG ActualLength);

VOID SkReportProcListCorruption(
    _In_ ULONG NextEntryOffset,
    _In_ ULONG ExpectedOffset);

VOID SkReportUnknownCode(
    _In_ ULONG_PTR Address,
    _In_ KPROCESSOR_MODE Mode);

VOID SkReportNtCallRIP(
    _In_ NTSTATUS NtStatus,
    _In_ LPWSTR lpMessage,
    _In_opt_ LPWSTR lpApiName,
    _In_opt_ LPWSTR lpQueryName);

VOID SkReportComCallRIP(
    _In_ HRESULT Hresult,
    _In_ LPWSTR lpMessage,
    _In_opt_ LPWSTR lpApiName,
    _In_opt_ LPWSTR lpQueryName);

VOID SkReportWrongWinVersion(
    _In_ LPWSTR lpMessage,
    _In_ ULONG dwVersionMajor,
    _In_ ULONG dwVersionMinor,
    _In_ ULONG dwBuildNumber,
    _In_ LPWSTR lpType);

VOID SkReportVersionResourceBuildNumber(
    _In_ LPCWSTR DllName,
    _In_ ULONG RefBuildNumber,
    _In_ ULONG BuildNumber);

VOID SkReportDebugObjectHandleMismatch(
    _In_ ULONG NumberOfObjects,
    _In_ ULONG NumberOfObjectsThroughQuery);

VOID SkReportDebugObject(
    _In_ ULONG NumberOfObjects,
    _In_ BOOL IsHandlde);

VOID SkReportDeviceObject(
    _In_ LPWSTR DeviceName);

VOID SkReportDriverListModification(
    _In_ ULONG ReportedLength,
    _In_ ULONG ExpectedLength);

VOID SkReportDebugDetected(
    _In_ ULONG Type,
    _In_ LPWSTR RoutineName,
    _In_opt_ LPWSTR InformationClass);

VOID SkReportBcdProbeMismatch(
    _In_ ULONG ApiQueryData,
    _In_ LPWSTR BcdProbeDescription,
    _In_ ULONG BcdProbeValue);

VOID SkReportSuspectRegion(
    _In_ PMEMORY_BASIC_INFORMATION Information);

//
// Reports end.
//

ULONG SkiQueryAndValidateSSN(
    _In_ PROBE_CONTEXT* Context,
    _In_ LPCSTR lpName,
    _In_ PVOID ImageBase,
    _In_ BOOL IsNtDll,
    _In_ BOOL bValidate);

VOID SkStartProbe();

BOOL SkIsCustomKernelSignersPolicyEnabled();
BOOL SkCheckSystemDebugControl();
BOOL SkCheckDebugPrivileges();

BOOL SkCheckBadProcess(
    _In_ ULONG ProcessId,
    _In_ PUNICODE_STRING ProcessName,
    _In_ PS_SCAN_TYPE ScanType);

BOOL SkVerifyWinVersion(
    _In_ PROBE_CONTEXT* Context);

BOOL SkiSetSyscallIndex(
    _In_ PVOID ImageBase,
    _In_ LPCSTR lpName);

BOOL SkWalkPEB(
    _In_ PPROBE_CONTEXT Context);

BOOL SkWalkLoadedDrivers(
    _In_ PPROBE_CONTEXT Context);

BOOL SkLoadNtDllCopies();

BOOL SkTestSyscalls(
    _In_ PPROBE_CONTEXT Context);

BOOL SkTestVectoredCall(
    _In_ ULONG SystemCallNumber,
    _In_ ULONG_PTR SystemCallAddress,
    _In_ SYSTEM_KERNEL_DEBUGGER_INFORMATION_EX* ReferenceInfo);

BOOL SkWsSetWalk(
    VOID);

BOOL SkWsSetWatch(
    _In_ PPROBE_CONTEXT Context);
BOOL SkStackWalk(
    _In_ PPROBE_CONTEXT Context);

BOOL SkNoKernelWubbaboos();

BOOL SkHandleTracing(
    _In_ PPROBE_CONTEXT Context);

BOOL SkCheckHandles(
    _In_ PROBE_CONTEXT* Context);

BOOL SkCheckDebug(
    _In_ PVOID NtDllBase);

BOOL SkValidateProcessList(
    _In_ PROBE_CONTEXT* Context);

BOOL SkValidateThreadList(
    _In_ PROBE_CONTEXT* Context);

BOOL SkValidateWin32uSyscalls(
    _In_ PROBE_CONTEXT* Context);

BOOL SkUserHandleTableWalk(
    _In_ PROBE_CONTEXT* Context);

BOOL SkGdiSharedHandleTableWalk(
    _In_ PROBE_CONTEXT* Context);

BOOL SkTestBootConfiguration();

BOOL SkCheckProcessMemory(
    _In_ PPROBE_CONTEXT Context);
