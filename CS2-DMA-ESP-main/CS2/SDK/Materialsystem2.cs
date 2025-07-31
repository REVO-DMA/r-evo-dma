namespace cs2_dma_esp.SDK.Materialsystem2 
{
    public static class MaterialParamBuffer_t  // MaterialParam_t
    {
        public const uint m_value = 0x8; // CUtlBinaryBlock
    }

    public static class MaterialParamFloat_t  // MaterialParam_t
    {
        public const uint m_flValue = 0x8; // float
    }

    public static class MaterialParamInt_t  // MaterialParam_t
    {
        public const uint m_nValue = 0x8; // int32_t
    }

    public static class MaterialParamString_t  // MaterialParam_t
    {
        public const uint m_value = 0x8; // CUtlString
    }

    public static class MaterialParamTexture_t  // MaterialParam_t
    {
        public const uint m_pValue = 0x8; // CStrongHandle<InfoForResourceTypeCTextureBase>
    }

    public static class MaterialParamVector_t  // MaterialParam_t
    {
        public const uint m_value = 0x8; // Vector4D
    }

    public static class MaterialParam_t 
    {
        public const uint m_name = 0x0; // CUtlString
    }

    public static class MaterialResourceData_t 
    {
        public const uint m_materialName = 0x0; // CUtlString
        public const uint m_shaderName = 0x8; // CUtlString
        public const uint m_intParams = 0x10; // CUtlVector<MaterialParamInt_t>
        public const uint m_floatParams = 0x28; // CUtlVector<MaterialParamFloat_t>
        public const uint m_vectorParams = 0x40; // CUtlVector<MaterialParamVector_t>
        public const uint m_textureParams = 0x58; // CUtlVector<MaterialParamTexture_t>
        public const uint m_dynamicParams = 0x70; // CUtlVector<MaterialParamBuffer_t>
        public const uint m_dynamicTextureParams = 0x88; // CUtlVector<MaterialParamBuffer_t>
        public const uint m_intAttributes = 0xA0; // CUtlVector<MaterialParamInt_t>
        public const uint m_floatAttributes = 0xB8; // CUtlVector<MaterialParamFloat_t>
        public const uint m_vectorAttributes = 0xD0; // CUtlVector<MaterialParamVector_t>
        public const uint m_textureAttributes = 0xE8; // CUtlVector<MaterialParamTexture_t>
        public const uint m_stringAttributes = 0x100; // CUtlVector<MaterialParamString_t>
        public const uint m_renderAttributesUsed = 0x118; // CUtlVector<CUtlString>
    }

    public static class PostProcessingBloomParameters_t 
    {
        public const uint m_blendMode = 0x0; // BloomBlendMode_t
        public const uint m_flBloomStrength = 0x4; // float
        public const uint m_flScreenBloomStrength = 0x8; // float
        public const uint m_flBlurBloomStrength = 0xC; // float
        public const uint m_flBloomThreshold = 0x10; // float
        public const uint m_flBloomThresholdWidth = 0x14; // float
        public const uint m_flSkyboxBloomStrength = 0x18; // float
        public const uint m_flBloomStartValue = 0x1C; // float
        public const uint m_flBlurWeight = 0x20; // float[5]
        public const uint m_vBlurTint = 0x34; // Vector[5]
    }

    public static class PostProcessingLocalContrastParameters_t 
    {
        public const uint m_flLocalContrastStrength = 0x0; // float
        public const uint m_flLocalContrastEdgeStrength = 0x4; // float
        public const uint m_flLocalContrastVignetteStart = 0x8; // float
        public const uint m_flLocalContrastVignetteEnd = 0xC; // float
        public const uint m_flLocalContrastVignetteBlur = 0x10; // float
    }

    public static class PostProcessingResource_t 
    {
        public const uint m_bHasTonemapParams = 0x0; // bool
        public const uint m_toneMapParams = 0x4; // PostProcessingTonemapParameters_t
        public const uint m_bHasBloomParams = 0x40; // bool
        public const uint m_bloomParams = 0x44; // PostProcessingBloomParameters_t
        public const uint m_bHasVignetteParams = 0xB4; // bool
        public const uint m_vignetteParams = 0xB8; // PostProcessingVignetteParameters_t
        public const uint m_bHasLocalContrastParams = 0xDC; // bool
        public const uint m_localConstrastParams = 0xE0; // PostProcessingLocalContrastParameters_t
        public const uint m_nColorCorrectionVolumeDim = 0xF4; // int32_t
        public const uint m_colorCorrectionVolumeData = 0xF8; // CUtlBinaryBlock
        public const uint m_bHasColorCorrection = 0x110; // bool
    }

    public static class PostProcessingTonemapParameters_t 
    {
        public const uint m_flExposureBias = 0x0; // float
        public const uint m_flShoulderStrength = 0x4; // float
        public const uint m_flLinearStrength = 0x8; // float
        public const uint m_flLinearAngle = 0xC; // float
        public const uint m_flToeStrength = 0x10; // float
        public const uint m_flToeNum = 0x14; // float
        public const uint m_flToeDenom = 0x18; // float
        public const uint m_flWhitePoint = 0x1C; // float
        public const uint m_flLuminanceSource = 0x20; // float
        public const uint m_flExposureBiasShadows = 0x24; // float
        public const uint m_flExposureBiasHighlights = 0x28; // float
        public const uint m_flMinShadowLum = 0x2C; // float
        public const uint m_flMaxShadowLum = 0x30; // float
        public const uint m_flMinHighlightLum = 0x34; // float
        public const uint m_flMaxHighlightLum = 0x38; // float
    }

    public static class PostProcessingVignetteParameters_t 
    {
        public const uint m_flVignetteStrength = 0x0; // float
        public const uint m_vCenter = 0x4; // Vector2D
        public const uint m_flRadius = 0xC; // float
        public const uint m_flRoundness = 0x10; // float
        public const uint m_flFeather = 0x14; // float
        public const uint m_vColorTint = 0x18; // Vector
    }
}
