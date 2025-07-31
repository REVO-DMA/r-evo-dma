#include "global.h"
#include "shellcode.h"
#include "math.h"
#include "memory.h"
#include "utils.h"
#include "monoClass.h"
#include "transform.h"
#include "tpp.h"
#include "visibilityCheck.h"
#include "espData.h"

extern "C" void _hook(ulong gameWorld, ShellCodeData_t* data)
{
    if (IS_INVALID_ADDR(gameWorld)) return;

    ulong registeredPlayers = ReadPtr(gameWorld + data->offsets.RegisteredPlayers);
    if (IS_INVALID_ADDR(registeredPlayers)) return;

    ulong registeredPlayersBase = ReadPtr(registeredPlayers + data->offsets.ListBase);
    if (IS_INVALID_ADDR(registeredPlayersBase)) return;

    int registeredPlayersCount = ReadValue<int>(registeredPlayers + data->offsets.ListCount);
    if (registeredPlayersCount < 1) return;

    TPP::Run(data, gameWorld);
    VisCheck::Run(data, registeredPlayersBase, registeredPlayersCount, gameWorld);
    ESP_Data::Run(data, gameWorld);
}

#pragma optimize("", off)

extern "C" void _code(ulong gameWorld)
{
    ShellCodeData_t* data = (ShellCodeData_t*)0xFAFAFAFAFAFAFAFA;

    if (!data->shellcodeActive)
    {
        data->tpp_enabled = false;
        goto end;
    }

    data->executorFunction(gameWorld, data);

end:
    __movsb((PBYTE)data->gameWorld_Update, data->originalBytes, PATCH_SIZE);
    data->gameWorld_Update(gameWorld);
    __movsb((PBYTE)data->gameWorld_Update, data->patchBytes, PATCH_SIZE);
}

#pragma optimize("", on)
