// ******************************************************************************
// Header: VirtualizerSDK.cs
// Description: .NET Native AOT macros definitions
//
// Author/s: Oreans Technologies 
// (c) 2023 Oreans Technologies
//
// --- File generated automatically from Oreans VM Generator (9/8/2023) ---
// ******************************************************************************

public static partial class VirtualizerSDK
{

#if WIN32

        public const string VIRTUALIZER_SDK_DLL = "VirtualizerSDK.dll";

        public const string VIRTUALIZER_TIGER_WHITE_START_ENTRYPOINT = "CustomVM00000100_Start";
        public const string VIRTUALIZER_TIGER_WHITE_END_ENTRYPOINT = "CustomVM00000100_End";
        public const string VIRTUALIZER_TIGER_RED_START_ENTRYPOINT = "CustomVM00000101_Start";
        public const string VIRTUALIZER_TIGER_RED_END_ENTRYPOINT = "CustomVM00000101_End";
        public const string VIRTUALIZER_TIGER_BLACK_START_ENTRYPOINT = "CustomVM00000102_Start";
        public const string VIRTUALIZER_TIGER_BLACK_END_ENTRYPOINT = "CustomVM00000102_End";
        public const string VIRTUALIZER_FISH_WHITE_START_ENTRYPOINT = "CustomVM00000106_Start";
        public const string VIRTUALIZER_FISH_WHITE_END_ENTRYPOINT = "CustomVM00000106_End";
        public const string VIRTUALIZER_FISH_RED_START_ENTRYPOINT = "CustomVM00000108_Start";
        public const string VIRTUALIZER_FISH_RED_END_ENTRYPOINT = "CustomVM00000108_End";
        public const string VIRTUALIZER_FISH_BLACK_START_ENTRYPOINT = "CustomVM00000110_Start";
        public const string VIRTUALIZER_FISH_BLACK_END_ENTRYPOINT = "CustomVM00000110_End";
        public const string VIRTUALIZER_PUMA_WHITE_START_ENTRYPOINT = "CustomVM00000112_Start";
        public const string VIRTUALIZER_PUMA_WHITE_END_ENTRYPOINT = "CustomVM00000112_End";
        public const string VIRTUALIZER_PUMA_RED_START_ENTRYPOINT = "CustomVM00000114_Start";
        public const string VIRTUALIZER_PUMA_RED_END_ENTRYPOINT = "CustomVM00000114_End";
        public const string VIRTUALIZER_PUMA_BLACK_START_ENTRYPOINT = "CustomVM00000116_Start";
        public const string VIRTUALIZER_PUMA_BLACK_END_ENTRYPOINT = "CustomVM00000116_End";
        public const string VIRTUALIZER_SHARK_WHITE_START_ENTRYPOINT = "CustomVM00000118_Start";
        public const string VIRTUALIZER_SHARK_WHITE_END_ENTRYPOINT = "CustomVM00000118_End";
        public const string VIRTUALIZER_SHARK_RED_START_ENTRYPOINT = "CustomVM00000120_Start";
        public const string VIRTUALIZER_SHARK_RED_END_ENTRYPOINT = "CustomVM00000120_End";
        public const string VIRTUALIZER_SHARK_BLACK_START_ENTRYPOINT = "CustomVM00000122_Start";
        public const string VIRTUALIZER_SHARK_BLACK_END_ENTRYPOINT = "CustomVM00000122_End";
        public const string VIRTUALIZER_DOLPHIN_WHITE_START_ENTRYPOINT = "CustomVM00000134_Start";
        public const string VIRTUALIZER_DOLPHIN_WHITE_END_ENTRYPOINT = "CustomVM00000134_End";
        public const string VIRTUALIZER_DOLPHIN_RED_START_ENTRYPOINT = "CustomVM00000136_Start";
        public const string VIRTUALIZER_DOLPHIN_RED_END_ENTRYPOINT = "CustomVM00000136_End";
        public const string VIRTUALIZER_DOLPHIN_BLACK_START_ENTRYPOINT = "CustomVM00000138_Start";
        public const string VIRTUALIZER_DOLPHIN_BLACK_END_ENTRYPOINT = "CustomVM00000138_End";
        public const string VIRTUALIZER_EAGLE_WHITE_START_ENTRYPOINT = "CustomVM00000146_Start";
        public const string VIRTUALIZER_EAGLE_WHITE_END_ENTRYPOINT = "CustomVM00000146_End";
        public const string VIRTUALIZER_EAGLE_RED_START_ENTRYPOINT = "CustomVM00000148_Start";
        public const string VIRTUALIZER_EAGLE_RED_END_ENTRYPOINT = "CustomVM00000148_End";
        public const string VIRTUALIZER_EAGLE_BLACK_START_ENTRYPOINT = "CustomVM00000150_Start";
        public const string VIRTUALIZER_EAGLE_BLACK_END_ENTRYPOINT = "CustomVM00000150_End";
        public const string VIRTUALIZER_LION_WHITE_START_ENTRYPOINT = "CustomVM00000160_Start";
        public const string VIRTUALIZER_LION_WHITE_END_ENTRYPOINT = "CustomVM00000160_End";
        public const string VIRTUALIZER_LION_RED_START_ENTRYPOINT = "CustomVM00000162_Start";
        public const string VIRTUALIZER_LION_RED_END_ENTRYPOINT = "CustomVM00000162_End";
        public const string VIRTUALIZER_LION_BLACK_START_ENTRYPOINT = "CustomVM00000164_Start";
        public const string VIRTUALIZER_LION_BLACK_END_ENTRYPOINT = "CustomVM00000164_End";
        public const string VIRTUALIZER_COBRA_WHITE_START_ENTRYPOINT = "CustomVM00000166_Start";
        public const string VIRTUALIZER_COBRA_WHITE_END_ENTRYPOINT = "CustomVM00000166_End";
        public const string VIRTUALIZER_COBRA_RED_START_ENTRYPOINT = "CustomVM00000168_Start";
        public const string VIRTUALIZER_COBRA_RED_END_ENTRYPOINT = "CustomVM00000168_End";
        public const string VIRTUALIZER_COBRA_BLACK_START_ENTRYPOINT = "CustomVM00000170_Start";
        public const string VIRTUALIZER_COBRA_BLACK_END_ENTRYPOINT = "CustomVM00000170_End";
        public const string VIRTUALIZER_WOLF_WHITE_START_ENTRYPOINT = "CustomVM00000172_Start";
        public const string VIRTUALIZER_WOLF_WHITE_END_ENTRYPOINT = "CustomVM00000172_End";
        public const string VIRTUALIZER_WOLF_RED_START_ENTRYPOINT = "CustomVM00000174_Start";
        public const string VIRTUALIZER_WOLF_RED_END_ENTRYPOINT = "CustomVM00000174_End";
        public const string VIRTUALIZER_WOLF_BLACK_START_ENTRYPOINT = "CustomVM00000176_Start";
        public const string VIRTUALIZER_WOLF_BLACK_END_ENTRYPOINT = "CustomVM00000176_End";
        public const string VIRTUALIZER_MUTATE_ONLY_START_ENTRYPOINT = "Mutate_Start";
        public const string VIRTUALIZER_MUTATE_ONLY_END_ENTRYPOINT = "Mutate_End";
    
#else

    public const string VIRTUALIZER_SDK_DLL = "VirtualizerSDK64.dll";

    public const string VIRTUALIZER_TIGER_WHITE_START_ENTRYPOINT = "CustomVM00000103_Start";
    public const string VIRTUALIZER_TIGER_WHITE_END_ENTRYPOINT = "CustomVM00000103_End";
    public const string VIRTUALIZER_TIGER_RED_START_ENTRYPOINT = "CustomVM00000104_Start";
    public const string VIRTUALIZER_TIGER_RED_END_ENTRYPOINT = "CustomVM00000104_End";
    public const string VIRTUALIZER_TIGER_BLACK_START_ENTRYPOINT = "CustomVM00000105_Start";
    public const string VIRTUALIZER_TIGER_BLACK_END_ENTRYPOINT = "CustomVM00000105_End";
    public const string VIRTUALIZER_FISH_WHITE_START_ENTRYPOINT = "CustomVM00000107_Start";
    public const string VIRTUALIZER_FISH_WHITE_END_ENTRYPOINT = "CustomVM00000107_End";
    public const string VIRTUALIZER_FISH_RED_START_ENTRYPOINT = "CustomVM00000109_Start";
    public const string VIRTUALIZER_FISH_RED_END_ENTRYPOINT = "CustomVM00000109_End";
    public const string VIRTUALIZER_FISH_BLACK_START_ENTRYPOINT = "CustomVM00000111_Start";
    public const string VIRTUALIZER_FISH_BLACK_END_ENTRYPOINT = "CustomVM00000111_End";
    public const string VIRTUALIZER_PUMA_WHITE_START_ENTRYPOINT = "CustomVM00000113_Start";
    public const string VIRTUALIZER_PUMA_WHITE_END_ENTRYPOINT = "CustomVM00000113_End";
    public const string VIRTUALIZER_PUMA_RED_START_ENTRYPOINT = "CustomVM00000115_Start";
    public const string VIRTUALIZER_PUMA_RED_END_ENTRYPOINT = "CustomVM00000115_End";
    public const string VIRTUALIZER_PUMA_BLACK_START_ENTRYPOINT = "CustomVM00000117_Start";
    public const string VIRTUALIZER_PUMA_BLACK_END_ENTRYPOINT = "CustomVM00000117_End";
    public const string VIRTUALIZER_SHARK_WHITE_START_ENTRYPOINT = "CustomVM00000119_Start";
    public const string VIRTUALIZER_SHARK_WHITE_END_ENTRYPOINT = "CustomVM00000119_End";
    public const string VIRTUALIZER_SHARK_RED_START_ENTRYPOINT = "CustomVM00000121_Start";
    public const string VIRTUALIZER_SHARK_RED_END_ENTRYPOINT = "CustomVM00000121_End";
    public const string VIRTUALIZER_SHARK_BLACK_START_ENTRYPOINT = "CustomVM00000123_Start";
    public const string VIRTUALIZER_SHARK_BLACK_END_ENTRYPOINT = "CustomVM00000123_End";
    public const string VIRTUALIZER_DOLPHIN_WHITE_START_ENTRYPOINT = "CustomVM00000135_Start";
    public const string VIRTUALIZER_DOLPHIN_WHITE_END_ENTRYPOINT = "CustomVM00000135_End";
    public const string VIRTUALIZER_DOLPHIN_RED_START_ENTRYPOINT = "CustomVM00000137_Start";
    public const string VIRTUALIZER_DOLPHIN_RED_END_ENTRYPOINT = "CustomVM00000137_End";
    public const string VIRTUALIZER_DOLPHIN_BLACK_START_ENTRYPOINT = "CustomVM00000139_Start";
    public const string VIRTUALIZER_DOLPHIN_BLACK_END_ENTRYPOINT = "CustomVM00000139_End";
    public const string VIRTUALIZER_EAGLE_WHITE_START_ENTRYPOINT = "CustomVM00000147_Start";
    public const string VIRTUALIZER_EAGLE_WHITE_END_ENTRYPOINT = "CustomVM00000147_End";
    public const string VIRTUALIZER_EAGLE_RED_START_ENTRYPOINT = "CustomVM00000149_Start";
    public const string VIRTUALIZER_EAGLE_RED_END_ENTRYPOINT = "CustomVM00000149_End";
    public const string VIRTUALIZER_EAGLE_BLACK_START_ENTRYPOINT = "CustomVM00000151_Start";
    public const string VIRTUALIZER_EAGLE_BLACK_END_ENTRYPOINT = "CustomVM00000151_End";
    public const string VIRTUALIZER_CAT_BLACK_START_ENTRYPOINT = "CustomVM00000157_Start";
    public const string VIRTUALIZER_CAT_BLACK_END_ENTRYPOINT = "CustomVM00000157_End";
    public const string VIRTUALIZER_LION_WHITE_START_ENTRYPOINT = "CustomVM00000161_Start";
    public const string VIRTUALIZER_LION_WHITE_END_ENTRYPOINT = "CustomVM00000161_End";
    public const string VIRTUALIZER_LION_RED_START_ENTRYPOINT = "CustomVM00000163_Start";
    public const string VIRTUALIZER_LION_RED_END_ENTRYPOINT = "CustomVM00000163_End";
    public const string VIRTUALIZER_LION_BLACK_START_ENTRYPOINT = "CustomVM00000165_Start";
    public const string VIRTUALIZER_LION_BLACK_END_ENTRYPOINT = "CustomVM00000165_End";
    public const string VIRTUALIZER_COBRA_WHITE_START_ENTRYPOINT = "CustomVM00000167_Start";
    public const string VIRTUALIZER_COBRA_WHITE_END_ENTRYPOINT = "CustomVM00000167_End";
    public const string VIRTUALIZER_COBRA_RED_START_ENTRYPOINT = "CustomVM00000169_Start";
    public const string VIRTUALIZER_COBRA_RED_END_ENTRYPOINT = "CustomVM00000169_End";
    public const string VIRTUALIZER_COBRA_BLACK_START_ENTRYPOINT = "CustomVM00000171_Start";
    public const string VIRTUALIZER_COBRA_BLACK_END_ENTRYPOINT = "CustomVM00000171_End";
    public const string VIRTUALIZER_WOLF_WHITE_START_ENTRYPOINT = "CustomVM00000173_Start";
    public const string VIRTUALIZER_WOLF_WHITE_END_ENTRYPOINT = "CustomVM00000173_End";
    public const string VIRTUALIZER_WOLF_RED_START_ENTRYPOINT = "CustomVM00000175_Start";
    public const string VIRTUALIZER_WOLF_RED_END_ENTRYPOINT = "CustomVM00000175_End";
    public const string VIRTUALIZER_WOLF_BLACK_START_ENTRYPOINT = "CustomVM00000177_Start";
    public const string VIRTUALIZER_WOLF_BLACK_END_ENTRYPOINT = "CustomVM00000177_End";
    public const string VIRTUALIZER_MUTATE_ONLY_START_ENTRYPOINT = "Mutate_Start";
    public const string VIRTUALIZER_MUTATE_ONLY_END_ENTRYPOINT = "Mutate_End";

#endif

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_TIGER_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_TIGER_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_TIGER_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_TIGER_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_TIGER_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_TIGER_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_TIGER_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_TIGER_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_TIGER_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_TIGER_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_TIGER_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_TIGER_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_FISH_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_FISH_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_FISH_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_FISH_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_FISH_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_FISH_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_FISH_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_FISH_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_FISH_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_FISH_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_FISH_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_FISH_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_PUMA_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_PUMA_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_PUMA_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_PUMA_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_PUMA_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_PUMA_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_PUMA_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_PUMA_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_PUMA_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_PUMA_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_PUMA_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_PUMA_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_SHARK_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_SHARK_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_SHARK_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_SHARK_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_SHARK_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_SHARK_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_SHARK_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_SHARK_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_SHARK_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_SHARK_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_SHARK_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_SHARK_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_DOLPHIN_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_DOLPHIN_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_DOLPHIN_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_DOLPHIN_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_DOLPHIN_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_DOLPHIN_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_DOLPHIN_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_DOLPHIN_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_DOLPHIN_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_DOLPHIN_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_DOLPHIN_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_DOLPHIN_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_EAGLE_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_EAGLE_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_EAGLE_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_EAGLE_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_EAGLE_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_EAGLE_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_EAGLE_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_EAGLE_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_EAGLE_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_EAGLE_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_EAGLE_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_EAGLE_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_CAT_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_CAT_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_CAT_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_CAT_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_LION_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_LION_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_LION_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_LION_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_LION_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_LION_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_LION_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_LION_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_LION_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_LION_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_LION_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_LION_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_COBRA_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_COBRA_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_COBRA_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_COBRA_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_COBRA_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_COBRA_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_COBRA_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_COBRA_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_COBRA_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_COBRA_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_COBRA_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_COBRA_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_WOLF_WHITE_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_WOLF_WHITE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_WOLF_WHITE_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_WOLF_WHITE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_WOLF_RED_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_WOLF_RED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_WOLF_RED_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_WOLF_RED_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_WOLF_BLACK_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_WOLF_BLACK_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_WOLF_BLACK_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_WOLF_BLACK_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_MUTATE_ONLY_START_ENTRYPOINT)]
    public static partial void VIRTUALIZER_MUTATE_ONLY_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = VIRTUALIZER_MUTATE_ONLY_END_ENTRYPOINT)]
    public static partial void VIRTUALIZER_MUTATE_ONLY_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerStart")]
    public static partial void VIRTUALIZER_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerEnd")]
    public static partial void VIRTUALIZER_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "Mutate_Start")]
    public static partial void MUTATE_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "Mutate_End")]
    public static partial void MUTATE_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerStrEncryptStart")]
    public static partial void STR_ENCRYPT_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerStrEncryptEnd")]
    public static partial void STR_ENCRYPT_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerStrEncryptWStart")]
    public static partial void STR_ENCRYPTW_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerStrEncryptWEnd")]
    public static partial void STR_ENCRYPTW_END();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerUnprotectedStart")]
    public static partial void VIRTUALIZER_UNPROTECTED_START();

    [LibraryImport(VIRTUALIZER_SDK_DLL, EntryPoint = "VirtualizerUnprotectedEnd")]
    public static partial void VIRTUALIZER_UNPROTECTED_END();
}
