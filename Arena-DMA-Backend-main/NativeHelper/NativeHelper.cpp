#include <vector>
#include <string>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <stdio.h>
#include <mutex>
#include <optional>
#include <corecrt_math.h>

#include "Debug.h"
#include "DMA.h"
#include "Offsets.h"
#include "Game.h"
#include "AssetFactory.h"

bool Initialized = false;

extern "C" bool NH_Initialize(VMM_HANDLE handle, UINT pid, bool debug)
{
	try
	{
		if (!handle || !pid)
			return false;

		Debug::IsDebug = debug;

		DMA::hVMM = handle;
		DMA::PID = pid;

		DMA::UnityPlayerDLL = DMA::GetModuleBase((LPSTR)"UnityPlayer.dll");
		Debug::Log("[NH_Initialize] -> UnityPlayer.dll found at " + Debug::AddrToHex(DMA::UnityPlayerDLL));
		if (DMA::UnityPlayerDLL == 0x0)
			return false;

		DMA::MonoDLL = DMA::GetModuleBase((LPSTR)"mono-2.0-bdwgc.dll");
		Debug::Log("[NH_Initialize] -> mono-2.0-bdwgc.dll found at " + Debug::AddrToHex(DMA::MonoDLL));
		if (DMA::MonoDLL == 0x0)
			return false;

		Offsets::InitMonoAddrs();

		DWORD oldProtect;
		VirtualProtect((LPVOID)Game::InvokeHook, 0x100, PAGE_EXECUTE_READWRITE, &oldProtect);
	}
	catch (const std::exception& err)
	{
		std::cerr << "[NH_Initialize] -> An unhandled exception occurred: " << err.what() << "\n" << std::endl;

		return false;
	}

	return true;
}

extern "C" uintptr_t NH_CallFunction(uintptr_t function, uintptr_t rcx, uintptr_t rdx, uintptr_t r8, uintptr_t r9)
{
	return Game::Call(function, rcx, rdx, r8, r9);
}

extern "C" void NH_SetCodeCave(uintptr_t codeCave)
{
	Game::CodeCave = (void*)codeCave;

	if (!Initialized)
	{
		Game::HookIndex = Game::GetHookIndex();

		Initialized = true;
	}
}

extern "C" uintptr_t NH_GetTypeObject(uintptr_t monoDomain, uintptr_t monoType)
{
	if (!monoDomain || !monoType)
		return 0;

	uintptr_t mono_type_get_object = DMA::MonoDLL + Offsets::mono_type_get_object;

	return Game::Call(mono_type_get_object, monoDomain, monoType);
}

extern "C" uintptr_t NH_CompileMethod(uintptr_t monoMethod)
{
	if (!monoMethod)
		return 0;

	uintptr_t mono_compile_method = DMA::MonoDLL + Offsets::mono_compile_method;

	return Game::Call(mono_compile_method, monoMethod);
}

extern "C" uintptr_t NH_CompileClass(uintptr_t monoClass)
{
	if (!monoClass)
		return 0;

	uintptr_t mono_class_setup_methods = DMA::MonoDLL + Offsets::mono_class_setup_methods;

	return Game::Call(mono_class_setup_methods, monoClass);
}

extern "C" bool NH_LoadAssetBundle(uintptr_t assetBundle, uintptr_t shaderName, uintptr_t shaderTypeObject)
{
	return AssetFactory::LoadBundle(assetBundle, shaderName, shaderTypeObject);
}

extern "C" void NH_UnloadAssetBundle()
{
	AssetFactory::UnloadLoadedAssetBundle();
}

extern "C" uintptr_t NH_CreateMaterialFromShader(uintptr_t monoDomain, uintptr_t materialClass)
{
	return AssetFactory::CreateMaterial(monoDomain, materialClass);
}

extern "C" int NH_ShaderPropertyToID(uintptr_t propertyName)
{
	return AssetFactory::ShaderPropertyToID(propertyName);
}

extern "C" void NH_SetMaterialColor(uintptr_t material, int propertyID, uintptr_t color)
{
	AssetFactory::SetMaterialColor(material, propertyID, color);
}

extern "C" uintptr_t NH_AllocBytes(uint32_t size)
{
	uintptr_t mono_marshal_alloc = DMA::MonoDLL + Offsets::mono_marshal_alloc_hglobal;

	return Game::Call(mono_marshal_alloc, size);
}

extern "C" uintptr_t NH_FreeBytes(uintptr_t pv)
{
	uintptr_t mono_marshal_free = DMA::MonoDLL + Offsets::mono_marshal_free;

	return Game::Call(mono_marshal_free, pv);
}

/// <summary>
/// Sets the state of a behaviour (not to be confused with a mono behaviour). The behaviour address can be gotten from the m_CachedPtr field.
/// </summary>
extern "C" uintptr_t NH_SetBehaviorState(uintptr_t behavior, bool state)
{
	if (!behavior)
		return 0;

	uintptr_t Behaviour_SetEnabled = DMA::UnityPlayerDLL + Offsets::Behaviour_SetEnabled;

	return Game::Call(Behaviour_SetEnabled, behavior, (byte)state);
}

/// <summary>
/// Disables a behaviour (not to be confused with a mono behaviour). The behaviour address can be gotten from the m_CachedPtr field.
/// </summary>
extern "C" uintptr_t NH_DisableBehavior(uintptr_t behavior)
{
	if (!behavior)
		return 0;

	enum DeactivateOperation
	{
		kNormalDeactivate = 0ull,
		kWillDestroySingleComponentDeactivate = 1ull,
		kWillDestroyGameObjectDeactivate = 2ull
	};

	uintptr_t mono_class_setup_methods = DMA::UnityPlayerDLL + Offsets::Behavior_Deactivate;

	return Game::Call(mono_class_setup_methods, behavior, DeactivateOperation::kNormalDeactivate);
}

extern "C" int NH_GetCameraHeight(uintptr_t camera)
{
	uintptr_t CameraScripting_GetPixelHeight = DMA::UnityPlayerDLL + Offsets::CameraScripting_GetPixelHeight;

	return Game::Call(CameraScripting_GetPixelHeight, camera);
}

extern "C" int NH_GetCameraWidth(uintptr_t camera)
{
	uintptr_t CameraScripting_GetPixelWidth = DMA::UnityPlayerDLL + Offsets::CameraScripting_GetPixelWidth;

	return Game::Call(CameraScripting_GetPixelWidth, camera);
}
