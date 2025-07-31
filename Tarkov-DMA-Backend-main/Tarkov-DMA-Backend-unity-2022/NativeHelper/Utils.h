#pragma once

namespace utils
{
    const int DisassemblyMaxSize = 0x3000;

    /// <summary>
    /// Naive implementation - should work fine for simple functions though.
    /// Just searches for the ret mnemonic.
    /// </summary>
    ZyanUSize FindFunctionLength(void* methodAddress)
    {
        ZyanU8 data[DisassemblyMaxSize];
        ZyanU64 runtime_address = (ZyanU64)methodAddress;
        ZyanUSize offset = 0;
        ZydisDisassembledInstruction instruction;

        // Copy data to buffer
        memcpy(data, methodAddress, DisassemblyMaxSize);

        while (ZYAN_SUCCESS(ZydisDisassembleIntel(
            /* machine_mode:    */ ZYDIS_MACHINE_MODE_LONG_64,
            /* runtime_address: */ runtime_address,
            /* buffer:          */ data + offset,
            /* length:          */ sizeof(data) - offset,
            /* instruction:     */ &instruction
        ))) {
            if (instruction.info.mnemonic == ZydisMnemonic::ZYDIS_MNEMONIC_RET)
                return offset + 1;

            offset += instruction.info.length;
            runtime_address += instruction.info.length;
        }

        return -1;
    }
}