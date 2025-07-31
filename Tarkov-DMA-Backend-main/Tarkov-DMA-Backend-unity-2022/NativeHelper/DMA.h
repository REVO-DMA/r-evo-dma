#pragma once

#pragma warning(push, 0)
#include "vmmdll.h"
#include "leechcore.h"
#pragma warning(pop)

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
			LOG(xorstr_("[DMA]: ReadValue() -> Invalid input."));
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
			LOG(xorstr_("[DMA]: ReadValue() -> Failed to read memory."));
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

	BOOL ReadBuffer(uintptr_t address, char* data, DWORD size, bool cache = false)
	{
		if (!PID || !hVMM || !address || !data || size == 0)
		{
			LOG(xorstr_("[DMA]: ReadBuffer() -> Invalid input."));
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
			LOG(xorstr_("[DMA]: ReadBuffer() -> Failed to read memory."));
			return FALSE;
		}

		return TRUE;
	}

	inline BOOL WriteBuffer(uintptr_t address, void* data, DWORD size)
	{
		if (!PID || !hVMM || !address || !data || size == 0)
		{
			LOG(xorstr_("[DMA]: WriteBuffer() -> Invalid input."));
			return FALSE;
		}

		return VMMDLL_MemWrite(hVMM, PID, address, (PBYTE)data, size);
	}

	inline uintptr_t GetModuleBase(LPCSTR moduleName)
	{
		return VMMDLL_ProcessGetModuleBaseU(hVMM, PID, moduleName);
	}

	inline uintptr_t GetExport(LPCSTR moduleName, LPCSTR functionName)
	{
		uintptr_t result = VMMDLL_ProcessGetProcAddressU(hVMM, PID, moduleName, functionName);

		if (!result)
		{
			LOG(xorstr_("[DMA]: GetExport() -> Unable to get export for \"") + Debug::S(functionName) + Debug::S(xorstr_("\".")));
			return 0x0;
		}

		uintptr_t moduleBase = GetModuleBase(moduleName);

		return result - moduleBase;
	}
}