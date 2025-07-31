#pragma once

namespace AssetFactory
{
    uintptr_t LoadedBundle = 0;
    uintptr_t LoadedShader = 0;

    void DontDestroyOnLoad(uintptr_t target)
    {
        Game::Call(DMA::UnityPlayerDLL + Offsets::Object_CUSTOM_DontDestroyOnLoad, target);
    }

    void UnloadLoadedAssetBundle()
    {
        if (!LoadedBundle)
            return;

        Game::Call(DMA::UnityPlayerDLL + Offsets::AssetBundle_CUSTOM_Unload, LoadedBundle, FALSE);

        LoadedBundle = 0;
    }

    bool LoadBundle(uintptr_t assetBundle, uintptr_t shaderName, uintptr_t shaderTypeObject)
    {
        uintptr_t AssetBundle_LoadFromMemory = DMA::UnityPlayerDLL + Offsets::AssetBundle_CUSTOM_LoadFromMemory_Internal;
        uintptr_t AssetBundle_LoadAsset = DMA::UnityPlayerDLL + Offsets::AssetBundle_CUSTOM_LoadAsset_Internal;

        LoadedBundle = Game::Call(AssetBundle_LoadFromMemory, assetBundle, 0ull);
        if (!LoadedBundle)
        {
            Debug::Log("[LoadBundle] -> Unable to load asset bundle from " + Debug::AddrToHex(assetBundle));
            return false;
        }
        else
            Debug::Log("[LoadBundle] -> Asset Bundle created at " + Debug::AddrToHex(LoadedBundle));

        LoadedShader = Game::Call(AssetBundle_LoadAsset, LoadedBundle, shaderName, shaderTypeObject);
        if (!LoadedShader)
        {
            Debug::Log("[LoadBundle] -> Unable to load shader from name at " + Debug::AddrToHex(shaderName) + Debug::S(" from asset bundle at ") + Debug::AddrToHex(LoadedBundle));
            return false;
        }
        else
            Debug::Log("[LoadBundle] -> Shader created at " + Debug::AddrToHex(LoadedShader));

        return true;
    }

    /// <summary>
    /// Creates a material from the loaded asset bundle.
    /// </summary>
    /// <returns>The created material's instance ID.</returns>
    uintptr_t CreateMaterial(uintptr_t monoDomain, uintptr_t materialClass)
    {
        if (!LoadedShader)
            return 0;

        uintptr_t mono_object_new = DMA::MonoDLL + Offsets::mono_object_new;
        uintptr_t Material_CreateWithShader = DMA::UnityPlayerDLL + Offsets::Material_CUSTOM_CreateWithShader;
        uintptr_t mono_gchandle_new = DMA::MonoDLL + Offsets::mono_gchandle_new;

        uintptr_t material = Game::Call(mono_object_new, monoDomain, materialClass);

        // Create the new material from the shader from the asset bundle
        Game::Call(Material_CreateWithShader, material, LoadedShader);

        // Prevent GC
        DontDestroyOnLoad(material);

        Game::Call(mono_gchandle_new, material, TRUE);
        Game::Call(DMA::UnityPlayerDLL + Offsets::Object_Set_Custom_PropHideFlags, material, 61);

        return material;
    }

    int ShaderPropertyToID(uintptr_t propertyName)
    {
        uintptr_t Shader_PropertyToID = DMA::UnityPlayerDLL + Offsets::Shader_CUSTOM_PropertyToID;

        auto id = Game::Call(Shader_PropertyToID, propertyName);

        return id;
    }

    void SetMaterialColor(uintptr_t material, int propertyID, uintptr_t color)
    {
        uintptr_t Material_SetColorImpl = DMA::UnityPlayerDLL + Offsets::Material_CUSTOM_SetColorImpl_Injected;

        Game::Call(Material_SetColorImpl, material, propertyID, color);
    }
}