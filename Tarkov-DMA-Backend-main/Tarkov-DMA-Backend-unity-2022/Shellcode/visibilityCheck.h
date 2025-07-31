#pragma once

namespace VisCheck
{
    __forceinline void Run(ShellCodeData_t* data, ulong registeredPlayersBase, int registeredPlayersCount, ulong gameWorld)
    {
        // Sanity check on players count
        if (registeredPlayersCount > MAX_PLAYER_COUNT || registeredPlayersCount <= 1)
            return;

        data->rayStart = GetLocalHeadPosition(data, gameWorld);
        if (V3_IS_ZERO(data->rayStart)) return;

        // ClientPlayer PlayerBody address will be the same as the PlayerBody offset if in offline
        bool isOffline = data->offsets.PlayerBodyClientPlayer == data->offsets.PlayerBody;

        for (int i = 0; i < registeredPlayersCount; i++)
        {
            ulong player = ReadPtr(registeredPlayersBase + data->offsets.ListFirstElement + (i * sizeof(ulong)));
            CONTINUE_IF_INVALID(player);

            ulong playerBody = ReadPtr(player + data->offsets.PlayerBody);
            CONTINUE_IF_INVALID(playerBody);

            ulong boneArray = GetBoneArray(data, playerBody);
            CONTINUE_IF_INVALID(boneArray);

            // Determine if this is an AI
            bool isAI = false;
            {
                if (isOffline)
                {
                    ulong aiData = ReadPtr(player + data->offsets.AIDataClientPlayer);
                    if (!IS_INVALID_ADDR(aiData))
                        isAI = ReadValue<bool>(aiData + data->offsets.IsAIClientPlayer);
                }
                else
                    isAI = ReadValue<bool>(player + data->offsets.IsAI);
            }

            // Loop through vis check bones and test visibility
            for (int ii = 0; ii < CHECK_BONE_COUNT; ii++)
            {
                int boneIndex = data->checkBones[ii];

                ulong targetBone = GetBoneTransform(data, boneArray, boneIndex);
                CONTINUE_IF_INVALID(targetBone);

                data->transform_getPosition(targetBone, data->tmpVec3);

                data->visiblePlayers[(i * CHECK_BONE_COUNT) + ii] = !data->linecastFunction(data->rayStart, data->tmpVec3, data->tmpRaycastHit, data->highPolyWithTerrainMask);

                // Only check the first bone to save performance if this is an AI player
                if (isAI)
                    break;
            }
        }
    }
}