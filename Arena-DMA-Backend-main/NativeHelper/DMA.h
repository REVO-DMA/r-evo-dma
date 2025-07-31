#pragma once

#include "vmmdll.h"
#include "leechcore.h"

#pragma comment(lib,"vmm.lib")
#pragma comment(lib,"leechcore.lib")

namespace DMA
{
	VMM_HANDLE hVMM = NULL;
	DWORD PID = NULL;

	uintptr_t MonoDLL = 0;
	uintptr_t UnityPlayerDLL = 0;

	template<typename T> std::optional<T> ReadValue(uintptr_t address, bool cache = false)
	{
		if (!PID || !hVMM || !address)
		{
			Debug::Log("[DMA]: ReadValue() -> Invalid input.");
			return std::nullopt;
		}

		DWORD size = sizeof(T);

		BOOL success;
		T output{};
		DWORD bytesRead = 0;

		if (cache)
		{
			success = VMMDLL_MemRead(hVMM, PID, (ULONG64)address, (PBYTE)&output, size);
			bytesRead = size;
		}
		else
			success = VMMDLL_MemReadEx(hVMM, PID, (ULONG64)address, (PBYTE)&output, size, &bytesRead, VMMDLL_FLAG_NOCACHE | VMMDLL_FLAG_NOPAGING | VMMDLL_FLAG_ZEROPAD_ON_FAIL | VMMDLL_FLAG_NOPAGING_IO);

		if (!success || bytesRead != size)
		{
			Debug::Log("[DMA]: ReadValue() -> Failed to read memory.");
			return std::nullopt;
		}

		return output;
	}

	inline std::optional<uintptr_t> ReadPtr(uintptr_t address, bool cache = false)
	{
		auto ptrRes = DMA::ReadValue<uintptr_t>(address, cache);
		if (ptrRes.has_value() && ptrRes.value() != 0x0)
			return ptrRes.value();

		return std::nullopt;
	}

	template<typename U, typename P> inline BOOL WriteValue(U address, P data)
	{
		if (!PID || !hVMM || !address)
		{
			Debug::Log("[DMA]: WriteValue() -> Invalid input.");
			return FALSE;
		}

		return VMMDLL_MemWrite(hVMM, PID, (ULONG64)address, (PBYTE)data, sizeof(P));
	}

	BOOL ReadBuffer(uintptr_t address, char* data, size_t size, bool cache = false)
	{
		if (!PID || !hVMM || !address || !data || size == 0)
		{
			Debug::Log("[DMA]: ReadBuffer() -> Invalid input.");
			return FALSE;
		}

		BOOL success;
		DWORD bytesRead = 0;

		if (cache)
		{
			success = VMMDLL_MemRead(hVMM, PID, address, (PBYTE)data, size);
			bytesRead = size;
		}
		else
			success = VMMDLL_MemReadEx(hVMM, PID, address, (PBYTE)data, size, &bytesRead, VMMDLL_FLAG_NOCACHE | VMMDLL_FLAG_NOPAGING | VMMDLL_FLAG_ZEROPAD_ON_FAIL | VMMDLL_FLAG_NOPAGING_IO);

		if (!success || bytesRead != size)
		{
			Debug::Log("[DMA]: ReadBuffer() -> Failed to read memory.");
			return FALSE;
		}

		return TRUE;
	}

	inline BOOL WriteBuffer(uintptr_t address, void* data, size_t size)
	{
		if (!PID || !hVMM || !address || !data || size == 0)
		{
			Debug::Log("[DMA]: WriteBuffer() -> Invalid input.");
			return FALSE;
		}

		return VMMDLL_MemWrite(hVMM, PID, address, (PBYTE)data, size);
	}

	inline uintptr_t GetModuleBase(LPSTR moduleName)
	{
		return VMMDLL_ProcessGetModuleBaseU(hVMM, PID, moduleName);
	}

	inline uintptr_t GetExport(LPSTR moduleName, LPSTR functionName)
	{
		uintptr_t result = VMMDLL_ProcessGetProcAddressU(hVMM, PID, moduleName, functionName);

		if (!result)
			Debug::Log("[DMA]: GetExport() -> Unable to get export for \"" + Debug::S(functionName) + Debug::S("\"."));

		uintptr_t moduleBase = GetModuleBase(moduleName);

		return result - moduleBase;
	}
}