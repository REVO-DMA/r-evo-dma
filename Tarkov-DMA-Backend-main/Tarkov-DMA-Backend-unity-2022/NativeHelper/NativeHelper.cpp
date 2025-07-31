#include <vector>
#include <string>
#include <sstream>
#include <iostream>
#include <algorithm>
#include <stdio.h>
#include <mutex>
#include <optional>
#include <corecrt_math.h>
#include <xmmintrin.h>

#include "../include/Zydis.h"

#include "xorstr.h"
#include "Debug.h"
#include "Utils.h"
#include "DMA.h"
#include "Offsets.h"
#include "Game.h"
#include "RemoteMethods.h"
#include "AssetFactory.h"

bool Initialized = false;

extern "C" bool NH_Initialize(VMM_HANDLE handle, UINT pid)
{
	try
	{
		if (!handle || !pid)
			return false;

		DMA::hVMM = handle;
		DMA::PID = pid;

		DMA::UnityPlayerDLL = DMA::GetModuleBase((LPSTR)xorstr_("UnityPlayer.dll"));
		LOG(xorstr_("[NH_Initialize] -> UnityPlayer.dll found at ") + Debug::AddrToHex(DMA::UnityPlayerDLL));
		if (DMA::UnityPlayerDLL == 0x0)
			return false;

		DMA::MonoDLL = DMA::GetModuleBase((LPSTR)xorstr_("mono-2.0-bdwgc.dll"));
		LOG(xorstr_("[NH_Initialize] -> mono-2.0-bdwgc.dll found at ") + Debug::AddrToHex(DMA::MonoDLL));
		if (DMA::MonoDLL == 0x0)
			return false;

		if (!Offsets::InitMonoAddrs())
			return false;

		// Allow changing data addr
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

extern "C" bool NH_SetCodeCave(uintptr_t codeCave)
{
	if (!Initialized)
	{
		Game::HookIndex = Game::GetHookIndex();

		Initialized = true;
	}

	Game::CodeCave = codeCave;
	return Game::Initialize();
}

// -_-

extern "C" uintptr_t NH_CallFunction(uintptr_t function, uintptr_t rcx, uintptr_t rdx, uintptr_t r8, uintptr_t r9)
{
	return Game::Call(function, rcx, rdx, r8, r9);
}

// Compilation

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

// Shader Stuff

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

// Memory

extern "C" uintptr_t NH_AllocBytes(uint32_t size)
{
	return RemoteMethods::AllocBytes(size);
}

extern "C" uintptr_t NH_FreeBytes(uintptr_t pv)
{
	return RemoteMethods::FreeBytes(pv);
}

extern "C" uintptr_t NH_AllocRWX()
{
	return RemoteMethods::AllocRWX();
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

// Game Object

extern "C" uintptr_t NH_FindGameObject(uintptr_t name)
{
	uintptr_t GameObject_CUSTOM_Find = DMA::UnityPlayerDLL + Offsets::GameObject_CUSTOM_Find;

	return Game::Call(GameObject_CUSTOM_Find, name);
}

extern "C" uintptr_t NH_GameObjectSetActive(uintptr_t gameObject, bool state)
{
	uintptr_t GameObject_CUSTOM_SetActive = DMA::UnityPlayerDLL + Offsets::GameObject_CUSTOM_SetActive;

	return Game::Call(GameObject_CUSTOM_SetActive, gameObject, (byte)state);
}

// Misc

extern "C" uintptr_t NH_GetTypeObject(uintptr_t monoDomain, uintptr_t monoType)
{
	if (!monoDomain || !monoType)
		return 0;

	uintptr_t mono_type_get_object = DMA::MonoDLL + Offsets::mono_type_get_object;

	return Game::Call(mono_type_get_object, monoDomain, monoType);
}

extern "C" int NH_GetMonoMethodParamCount(uintptr_t monoMethod)
{
	return RemoteMethods::GetMonoMethodParamCount(monoMethod);
}