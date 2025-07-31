#pragma once

namespace Offsets
{
	// mono-2.0-bdwgc.dll
	uintptr_t mono_mprotect = 0x72B60;
	uintptr_t mono_class_setup_methods = 0xCFC60;
	uintptr_t mono_marshal_free = 0x16ABE0;
	uintptr_t mono_compile_method;
	uintptr_t mono_object_new;
	uintptr_t mono_type_get_object;
	uintptr_t mono_gchandle_new;
	uintptr_t mono_method_signature;
	uintptr_t mono_signature_get_param_count;
	uintptr_t mono_marshal_alloc_hglobal = 0x274700;

	// UnityPlayer.dll
	const uintptr_t AssetBundle_CUSTOM_LoadAsset_Internal = 0x1CD440;
	const uintptr_t AssetBundle_CUSTOM_LoadFromMemory_Internal = 0x1CC810;
	const uintptr_t AssetBundle_CUSTOM_Unload = 0x1CE410;
	const uintptr_t Behaviour_SetEnabled = 0x4499E0; // Behaviour::SetEnabled
	const uintptr_t GameObject_CUSTOM_Find = 0x1006C0;
	const uintptr_t GameObject_CUSTOM_SetActive = 0xFD8E0;
	const uintptr_t Material_CUSTOM_CreateWithShader = 0xAFCA0;
	const uintptr_t Material_CUSTOM_SetColorImpl_Injected = 0xB7CC0;
	const uintptr_t Object_CUSTOM_DontDestroyOnLoad = 0x105EC0;
	const uintptr_t Object_Set_Custom_PropHideFlags = 0x106130;
	const uintptr_t Shader_CUSTOM_PropertyToID = 0xAB260;
	const uintptr_t mono_runtime_invoke = 0x1CFA000; // Search name as string to get offset

	inline bool InitMonoAddrs()
	{
		const LPCSTR monoDLL = (LPCSTR)xorstr_("mono-2.0-bdwgc.dll");

		mono_compile_method = DMA::GetExport(monoDLL, (LPCSTR)xorstr_("mono_compile_method"));
		mono_object_new = DMA::GetExport(monoDLL, (LPCSTR)xorstr_("mono_object_new"));
		mono_type_get_object = DMA::GetExport(monoDLL, (LPCSTR)xorstr_("mono_type_get_object"));
		mono_gchandle_new = DMA::GetExport(monoDLL, (LPCSTR)xorstr_("mono_gchandle_new"));
		mono_method_signature = DMA::GetExport(monoDLL, (LPCSTR)xorstr_("mono_method_signature"));
		mono_signature_get_param_count = DMA::GetExport(monoDLL, (LPCSTR)xorstr_("mono_signature_get_param_count"));

		if (mono_compile_method == 0x0 ||
			mono_object_new == 0x0 ||
			mono_type_get_object == 0x0 ||
			mono_gchandle_new == 0x0 ||
			mono_method_signature == 0x0 ||
			mono_signature_get_param_count == 0x0)
		{
			return false;
		}

		return true;
	}
}