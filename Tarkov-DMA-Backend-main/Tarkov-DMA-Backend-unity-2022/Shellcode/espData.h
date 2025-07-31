#pragma once

namespace ESP_Data
{
	__forceinline ulong GetOCMContainer(ShellCodeData_t* data)
	{
		return ReadPtr(data->ocmContainer + data->offsets.OCMContainerInstance);
	}

	__forceinline ulong GetOCM(ShellCodeData_t* data, ulong ocmContainer)
	{
		return ReadPtr(ocmContainer + data->offsets.OCM);
	}

	__forceinline ulong GetScopeCamera(ShellCodeData_t* data, ulong ocm)
	{
		ulong camera = ReadPtr(ocm + data->offsets.OCMCamera);
		RETURN_0_IF_INVALID(camera);

		return ReadPtr(camera + 0x10);
	}

	__forceinline ulong GetFPSCamera(ShellCodeData_t* data, ulong ocmContainer)
	{
		ulong camera = ReadPtr(ocmContainer + data->offsets.FPSCamera);
		RETURN_0_IF_INVALID(camera);

		return ReadPtr(camera + 0x10);
	}

	__forceinline ulong GetLensRenderer(ShellCodeData_t* data, ulong ocm)
	{
		ulong currentOpticSight = ReadPtr(ocm + data->offsets.CurrentOpticSight);
		RETURN_0_IF_INVALID(currentOpticSight);

		return ReadPtr(currentOpticSight + data->offsets.LensRenderer);
	}

	__forceinline bool IsCameraActive(ShellCodeData_t* data, ulong camera)
	{
		return ReadValue<bool>(camera + data->offsets.IsAdded);
	}

	__forceinline void Run(ShellCodeData_t* data, uintptr_t gameWorld)
	{
		UpdateWeaponPositionalData(data, gameWorld);

		ulong ocmContainer = GetOCMContainer(data);
		if (IS_INVALID_ADDR(ocmContainer)) goto fail;

		// Get FPS Camera view matrix
		ulong fpsCamera = GetFPSCamera(data, ocmContainer);
		if (IS_INVALID_ADDR(fpsCamera)) goto fail;
		data->espData.CameraMatrix = ReadValue<Matrix4x4>(fpsCamera + data->offsets.ViewMatrix);

		ulong ocm = GetOCM(data, ocmContainer);
		if (IS_INVALID_ADDR(ocm)) goto fail;

		ulong scopeCamera = GetScopeCamera(data, ocm);
		if (IS_INVALID_ADDR(scopeCamera)) goto fail;

		ulong lensRenderer = GetLensRenderer(data, ocm);
		if (IS_INVALID_ADDR(lensRenderer)) goto fail;

		ulong lensRendererNative = ReadPtr(lensRenderer + 0x10);
		if (IS_INVALID_ADDR(lensRendererNative)) goto fail;

		// Get lens AABB
		data->espData.AABB = ReadValue<AABB>(lensRendererNative + data->offsets.LocalAABB);

		// Get scope camera position
		data->espData.LensPosition = ReadValue<Vector3>(scopeCamera + data->offsets.LastPosition);

		// Get scope view matrix
		data->espData.ScopeMatrix = ReadValue<Matrix4x4>(scopeCamera + data->offsets.ViewMatrix);
		data->espData.ScopeFOV = ReadValue<float>(scopeCamera + data->offsets.FOV);

		// Get scoped status
		data->espData.IsScoped = IsCameraActive(data, scopeCamera);

		return;
		
	fail:
		data->espData.IsScoped = false;
	}
}