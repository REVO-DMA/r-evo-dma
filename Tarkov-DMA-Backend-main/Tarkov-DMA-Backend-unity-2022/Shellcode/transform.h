#pragma once

__forceinline ulong GetBoneArray(ShellCodeData_t* data, ulong playerBody)
{
    ulong skeleton = ReadPtr(playerBody + data->offsets.SkeletonRootJoint);
    RETURN_0_IF_INVALID(skeleton);

    ulong values = ReadPtr(skeleton + data->offsets.SkeletonValues);
    RETURN_0_IF_INVALID(values);

    ulong valuesBase = ReadPtr(values + data->offsets.ListBase);
    RETURN_0_IF_INVALID(valuesBase);

    return valuesBase;
}

__forceinline ulong GetBoneTransform(ShellCodeData_t* data, ulong boneArray, int boneIndex)
{
    ulong transform = ReadPtr(boneArray + data->offsets.ListFirstElement + (boneIndex * sizeof(ulong)));
    RETURN_0_IF_INVALID(transform);

    ulong transformInternal = ReadPtr(transform + 0x10);
    RETURN_0_IF_INVALID(transformInternal);

    // Simple checks to make sure the transform is valid
    {
        ulong checkOne = ReadPtr(transform);
        RETURN_0_IF_INVALID(checkOne);

        ulong checkTwo = ReadPtr(checkOne);
        RETURN_0_IF_INVALID(checkTwo);
    }

    return transformInternal;
}

__forceinline Vector3 GetLocalHeadPosition(ShellCodeData_t* data, ulong gameWorld)
{
    ulong localPlayer = ReadPtr(gameWorld + data->offsets.MainPlayer);
    if (IS_INVALID_ADDR(localPlayer)) goto fail;

    ulong body = ReadPtr(localPlayer + data->offsets.PlayerBodyClientPlayer);
    if (IS_INVALID_ADDR(body)) goto fail;

    ulong boneArray = GetBoneArray(data, body);
    if (IS_INVALID_ADDR(boneArray)) goto fail;

    ulong headBone = GetBoneTransform(data, boneArray, (int)BoneNames::Head);
    if (IS_INVALID_ADDR(headBone)) goto fail;

    data->transform_getPosition(headBone, data->tmpVec3);

    return data->tmpVec3;

fail:
    return { 0.0f, 0.0f, 0.0f };
}

__forceinline void UpdateWeaponPositionalData(ShellCodeData_t* data, ulong gameWorld)
{
    ulong localPlayer = ReadPtr(gameWorld + data->offsets.MainPlayer);
    if (IS_INVALID_ADDR(localPlayer)) goto fail;

    ulong handsController = ReadPtr(localPlayer + data->offsets.HandsController);
    if (IS_INVALID_ADDR(handsController)) goto fail;

    if (!MonoClass::IsA(handsController, xorstr_("FirearmController")))
        goto fail;

    ulong firePort = ReadPtr(handsController + data->offsets.FirePort);
    if (IS_INVALID_ADDR(firePort)) goto fail;

    ulong original = ReadPtr(firePort + 0x10);
    if (IS_INVALID_ADDR(original)) goto fail;

    ulong transformInternal = ReadPtr(original + 0x10);
    if (IS_INVALID_ADDR(transformInternal)) goto fail;

    // Simple checks to make sure the transform is valid
    {
        ulong checkOne = ReadPtr(original);
        if (IS_INVALID_ADDR(checkOne)) goto fail;

        ulong checkTwo = ReadPtr(checkOne);
        if (IS_INVALID_ADDR(checkTwo)) goto fail;
    }

    data->transform_getPosition(transformInternal, data->espData.firePortPos);
    
    // Get fireport direction
    data->tmpVec3 = { 0.0f, -1.0f, 0.0f };
    data->transform_transformDirection(transformInternal, data->espData.firePortDirection, data->tmpVec3);

    data->eft_player_firearmController_adjustShotVectors(handsController, data->espData.firePortPos, data->espData.firePortDirection);

    // Get world intersection point
    data->tmpVec3 = V3_Add(data->espData.firePortPos, V3_Multiply(data->espData.firePortDirection, data->espData.intersectionTestDistance));
    
    // Check world intersection
    data->linecastFunction(data->espData.firePortPos, data->tmpVec3, data->tmpRaycastHit, data->highPolyWithTerrainMask);
    Vector3 worldIntersection = data->tmpRaycastHit.point;
    float worldIntersectionDistance = V3_Distance(data->espData.firePortPos, worldIntersection);

    // Check player intersection
    data->linecastFunction(data->espData.firePortPos, data->tmpVec3, data->tmpRaycastHit, data->hitMask);
    Vector3 playerIntersection = data->tmpRaycastHit.point;
    float playerIntersectionDistance = V3_Distance(data->espData.firePortPos, playerIntersection);

    // Get the closest intersection
    Vector3 closestIntersection;
    if (worldIntersectionDistance < playerIntersectionDistance)
        closestIntersection = worldIntersection;
    else
        closestIntersection = playerIntersection;
    
    data->espData.firePortIntersectionPoint = closestIntersection;

    return;

fail:
    data->espData.firePortPos = { 0.0f, 0.0f, 0.0f };
    data->espData.firePortDirection = { 0.0f, 0.0f, 0.0f };
    data->espData.firePortIntersectionPoint = { 0.0f, 0.0f, 0.0f };
}

__forceinline ulong GetTransformInternal(ulong addr)
{
    addr = ReadPtr(addr + 0x10);
    RETURN_0_IF_INVALID(addr);

    addr = ReadPtr(addr + 0x30);
    RETURN_0_IF_INVALID(addr);

    addr = ReadPtr(addr + 0x30);
    RETURN_0_IF_INVALID(addr);

    addr = ReadPtr(addr + 0x8);
    RETURN_0_IF_INVALID(addr);

    return addr;
}