namespace cs2_dma_esp.SDK.Scenesystem 
{
    public static class CSSDSEndFrameViewInfo 
    {
        public const uint m_nViewId = 0x0; // uint64_t
        public const uint m_ViewName = 0x8; // CUtlString
    }

    public static class CSSDSMsg_EndFrame 
    {
        public const uint m_Views = 0x0; // CUtlVector<CSSDSEndFrameViewInfo>
    }

    public static class CSSDSMsg_LayerBase 
    {
        public const uint m_viewId = 0x0; // SceneViewId_t
        public const uint m_ViewName = 0x10; // CUtlString
        public const uint m_nLayerIndex = 0x18; // int32_t
        public const uint m_nLayerId = 0x20; // uint64_t
        public const uint m_LayerName = 0x28; // CUtlString
        public const uint m_displayText = 0x30; // CUtlString
    }

    public static class CSSDSMsg_PostLayer  // CSSDSMsg_LayerBase
    {
    }

    public static class CSSDSMsg_PreLayer  // CSSDSMsg_LayerBase
    {
    }

    public static class CSSDSMsg_ViewRender 
    {
        public const uint m_viewId = 0x0; // SceneViewId_t
        public const uint m_ViewName = 0x10; // CUtlString
    }

    public static class CSSDSMsg_ViewTarget 
    {
        public const uint m_Name = 0x0; // CUtlString
        public const uint m_TextureId = 0x8; // uint64_t
        public const uint m_nWidth = 0x10; // int32_t
        public const uint m_nHeight = 0x14; // int32_t
        public const uint m_nRequestedWidth = 0x18; // int32_t
        public const uint m_nRequestedHeight = 0x1C; // int32_t
        public const uint m_nNumMipLevels = 0x20; // int32_t
        public const uint m_nDepth = 0x24; // int32_t
        public const uint m_nMultisampleNumSamples = 0x28; // int32_t
        public const uint m_nFormat = 0x2C; // int32_t
    }

    public static class CSSDSMsg_ViewTargetList 
    {
        public const uint m_viewId = 0x0; // SceneViewId_t
        public const uint m_ViewName = 0x10; // CUtlString
        public const uint m_Targets = 0x18; // CUtlVector<CSSDSMsg_ViewTarget>
    }

    public static class SceneViewId_t 
    {
        public const uint m_nViewId = 0x0; // uint64_t
        public const uint m_nFrameCount = 0x8; // uint64_t
    }
}
