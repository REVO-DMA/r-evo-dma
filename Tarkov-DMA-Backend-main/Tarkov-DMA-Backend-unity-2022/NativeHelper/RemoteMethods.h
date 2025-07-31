#pragma once

namespace RemoteMethods
{
	uintptr_t AllocBytes(uint32_t size)
	{
		uintptr_t mono_marshal_alloc = DMA::MonoDLL + Offsets::mono_marshal_alloc_hglobal;

		return Game::Call(mono_marshal_alloc, size);
	}

	uintptr_t FreeBytes(uintptr_t pv)
	{
		uintptr_t mono_marshal_free = DMA::MonoDLL + Offsets::mono_marshal_free;

		return Game::Call(mono_marshal_free, pv);
	}

	uintptr_t AllocRWX()
	{
		const uint32_t rwxSize = 64000; // This is the safest size for an RWX region based on research

		uintptr_t mono_mprotect = DMA::MonoDLL + Offsets::mono_mprotect;

		uintptr_t addr = RemoteMethods::AllocBytes(rwxSize);
		if (addr == 0x0)
		{
			LOG(xorstr_("[AllocRWX] -> Failed to allocate memory at ") + Debug::AddrToHex(addr));
			return 0x0;
		}

		// 7 = rwx (check mono_mmap_win_prot_from_flags)
		// VirtualProtect return value is inverted in mono_mprotect
		if (Game::Call(mono_mprotect, addr, rwxSize, 7))
		{
			LOG(xorstr_("[AllocRWX] -> Failed to convert memory at ") + Debug::AddrToHex(addr) + xorstr_(" to RWX."));
			return 0x0;
		}

		LOG(xorstr_("[AllocRWX] -> Allocated RWX memory at ") + Debug::AddrToHex(addr));

		return addr;
	}

	int GetMonoMethodParamCount(uintptr_t monoMethod)
	{
		uintptr_t monoMethodSignature = Game::Call(DMA::MonoDLL + Offsets::mono_method_signature, monoMethod);
		uintptr_t paramCount = Game::Call(DMA::MonoDLL + Offsets::mono_signature_get_param_count, monoMethodSignature);
		
		return (int)paramCount;
	}
}