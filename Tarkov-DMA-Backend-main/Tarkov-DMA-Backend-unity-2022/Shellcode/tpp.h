#pragma once

namespace TPP
{
    enum EPointOfView
    {
        FirstPerson,
        ThirdPerson,
        FreeCamera
    };

    __forceinline void Run(ShellCodeData_t* data, ulong gameWorld)
    {
        ulong localPlayer = ReadPtr(gameWorld + data->offsets.MainPlayer);
        if (IS_INVALID_ADDR(localPlayer)) return;

        ulong pwa = ReadPtr(localPlayer + data->offsets.ProceduralWeaponAnimation);
        if (IS_INVALID_ADDR(pwa)) return;

        if (data->tpp_enabled == true)
        {
            ulong playerBody = ReadPtr(localPlayer + data->offsets.PlayerBodyClientPlayer);
            if (IS_INVALID_ADDR(playerBody)) return;

            ulong pointOfView = ReadPtr(playerBody + data->offsets.PointOfView);
            if (IS_INVALID_ADDR(pointOfView)) return;

            EPointOfView pov = (EPointOfView)ReadValue<int>(pointOfView + data->offsets.POV);
            if (pov != EPointOfView::ThirdPerson)
            {
                data->eft_player_set_PointOfView(localPlayer, (int)EPointOfView::FirstPerson);
                data->eft_player_set_PointOfView(localPlayer, (int)EPointOfView::ThirdPerson);
            }

            ulong handsContainer = ReadPtr(pwa + data->offsets.HandsContainer);
            if (IS_INVALID_ADDR(handsContainer)) return;

            ulong cameraTransform = ReadPtr(handsContainer + data->offsets.CameraTransform);
            if (IS_INVALID_ADDR(cameraTransform)) return;

            ulong transformInternal = ReadPtr(cameraTransform + 0x10);
            if (IS_INVALID_ADDR(transformInternal)) return;

            ulong movementCtx = ReadPtr(localPlayer + data->offsets.MovementContext);
            if (IS_INVALID_ADDR(movementCtx)) return;

            Vector3 lookDir = V3_Normalize(ReadValue<Vector3>(movementCtx + data->offsets.LookDirection));
            Vector3 lpHeadPos = GetLocalHeadPosition(data, gameWorld);
            if (V3_IS_ZERO(lpHeadPos)) return;

            Vector3 backwardOffset = {
                -(lookDir.X * data->tpp_horizontalDistance),
                -(lookDir.Y),
                -(lookDir.Z * data->tpp_horizontalDistance)
            };
            Vector3 horizontalOffset = {
                lookDir.Z * data->tpp_horizontalOffset,
                0.0f,
                -(lookDir.X * data->tpp_horizontalOffset)
            };

            // Current position + the backwards offset + the horizontal offset + the vertical offset
            Vector3 tppCameraPos = {
                lpHeadPos.X + backwardOffset.X + horizontalOffset.X,
                lpHeadPos.Y + backwardOffset.Y + horizontalOffset.Y + data->tpp_verticalDistance,
                lpHeadPos.Z + backwardOffset.Z + horizontalOffset.Z
            };

            data->tmpVec3 = tppCameraPos;
            data->transform_setPosition(transformInternal, data->tmpVec3);

            data->tpp_currentState = true;
        }
        else if (data->tpp_enabled == false)
        {
            if (data->tpp_currentState == true)
            {
                data->eft_player_set_PointOfView(localPlayer, (int)EPointOfView::FirstPerson);
            }

            data->tpp_currentState = false;
        }
    }
}