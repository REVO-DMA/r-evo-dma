#pragma once

namespace Offsets
{
	const LPSTR monoDLL = "mono-2.0-bdwgc.dll";

	// mono-2.0-bdwgc.dll
	uintptr_t mono_class_setup_methods = 0x3F6D0;
	uintptr_t mono_marshal_free = 0x81B70;
	uintptr_t mono_compile_method;
	uintptr_t mono_object_new;
	uintptr_t mono_type_get_object;
	uintptr_t mono_gchandle_new;
	uintptr_t mono_marshal_alloc_hglobal = 0x11A540;

	// UnityPlayer.dll
	const uintptr_t AssetBundle_CUSTOM_LoadAsset_Internal = 0x3B4A40;
	const uintptr_t AssetBundle_CUSTOM_LoadFromMemory_Internal = 0x3B4E10;
	const uintptr_t AssetBundle_CUSTOM_Unload = 0x3B5110;
	const uintptr_t Behaviour_SetEnabled = 0x6500B0;
	const uintptr_t CameraScripting_GetPixelHeight = 0x5FEB10;
	const uintptr_t CameraScripting_GetPixelWidth = 0x5FEB40;
	const uintptr_t Behavior_Deactivate = 0x64F150;
	const uintptr_t Material_CUSTOM_CreateWithShader = 0x963820;
	const uintptr_t Material_CUSTOM_SetColorImpl_Injected = 0x9662D0;
	const uintptr_t Object_CUSTOM_DontDestroyOnLoad = 0x96F020;
	const uintptr_t Object_Set_Custom_PropHideFlags = 0x96FA20;
	const uintptr_t Shader_CUSTOM_PropertyToID = 0x9883C0;
	const uintptr_t mono_runtime_invoke = 0x1849710; // Search name as string to get offset

	inline void InitMonoAddrs()
	{
		mono_compile_method = DMA::GetExport(monoDLL, "mono_compile_method");
		mono_object_new = DMA::GetExport(monoDLL, "mono_object_new");
		mono_type_get_object = DMA::GetExport(monoDLL, "mono_type_get_object");
		mono_gchandle_new = DMA::GetExport(monoDLL, "mono_gchandle_new");
	}
}