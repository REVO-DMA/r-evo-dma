namespace cs2_dma_esp.SDK.Worldrenderer 
{
    public static class AggregateLODSetup_t 
    {
        public const uint m_vLODOrigin = 0x0; // Vector
        public const uint m_fMaxObjectScale = 0xC; // float
        public const uint m_fSwitchDistances = 0x10; // CUtlVectorFixedGrowable<float>
    }

    public static class AggregateMeshInfo_t 
    {
        public const uint m_nVisClusterMemberOffset = 0x0; // uint32_t
        public const uint m_nVisClusterMemberCount = 0x4; // uint8_t
        public const uint m_bHasTransform = 0x5; // bool
        public const uint m_nDrawCallIndex = 0x6; // int16_t
        public const uint m_nLODSetupIndex = 0x8; // int16_t
        public const uint m_nLODGroupMask = 0xA; // uint8_t
        public const uint m_vTintColor = 0xB; // Color
        public const uint m_objectFlags = 0x10; // ObjectTypeFlags_t
        public const uint m_nLightProbeVolumePrecomputedHandshake = 0x14; // int32_t
    }

    public static class AggregateSceneObject_t 
    {
        public const uint m_allFlags = 0x0; // ObjectTypeFlags_t
        public const uint m_anyFlags = 0x4; // ObjectTypeFlags_t
        public const uint m_nLayer = 0x8; // int16_t
        public const uint m_aggregateMeshes = 0x10; // CUtlVector<AggregateMeshInfo_t>
        public const uint m_lodSetups = 0x28; // CUtlVector<AggregateLODSetup_t>
        public const uint m_visClusterMembership = 0x40; // CUtlVector<uint16_t>
        public const uint m_fragmentTransforms = 0x58; // CUtlVector<matrix3x4_t>
        public const uint m_renderableModel = 0x70; // CStrongHandle<InfoForResourceTypeCModel>
    }

    public static class BakedLightingInfo_t 
    {
        public const uint m_nLightmapVersionNumber = 0x0; // uint32_t
        public const uint m_nLightmapGameVersionNumber = 0x4; // uint32_t
        public const uint m_vLightmapUvScale = 0x8; // Vector2D
        public const uint m_bHasLightmaps = 0x10; // bool
        public const uint m_lightMaps = 0x18; // CUtlVector<CStrongHandle<InfoForResourceTypeCTextureBase>>
    }

    public static class BaseSceneObjectOverride_t 
    {
        public const uint m_nSceneObjectIndex = 0x0; // uint32_t
    }

    public static class CEntityComponent 
    {
    }

    public static class CEntityIdentity 
    {
        public const uint m_nameStringableIndex = 0x14; // int32_t
        public const uint m_name = 0x18; // CUtlSymbolLarge
        public const uint m_designerName = 0x20; // CUtlSymbolLarge
        public const uint m_flags = 0x30; // uint32_t
        public const uint m_worldGroupId = 0x38; // WorldGroupId_t
        public const uint m_fDataObjectTypes = 0x3C; // uint32_t
        public const uint m_PathIndex = 0x40; // ChangeAccessorFieldPathIndex_t
        public const uint m_pPrev = 0x58; // CEntityIdentity*
        public const uint m_pNext = 0x60; // CEntityIdentity*
        public const uint m_pPrevByClass = 0x68; // CEntityIdentity*
        public const uint m_pNextByClass = 0x70; // CEntityIdentity*
    }

    public static class CEntityInstance 
    {
        public const uint m_iszPrivateVScripts = 0x8; // CUtlSymbolLarge
        public const uint m_pEntity = 0x10; // CEntityIdentity*
        public const uint m_CScriptComponent = 0x28; // CScriptComponent*
    }

    public static class CScriptComponent  // CEntityComponent
    {
        public const uint m_scriptClassName = 0x30; // CUtlSymbolLarge
    }

    public static class CVoxelVisibility 
    {
        public const uint m_nBaseClusterCount = 0x40; // uint32_t
        public const uint m_nPVSBytesPerCluster = 0x44; // uint32_t
        public const uint m_vMinBounds = 0x48; // Vector
        public const uint m_vMaxBounds = 0x54; // Vector
        public const uint m_flGridSize = 0x60; // float
        public const uint m_nSkyVisibilityCluster = 0x64; // uint32_t
        public const uint m_nSunVisibilityCluster = 0x68; // uint32_t
        public const uint m_NodeBlock = 0x6C; // VoxelVisBlockOffset_t
        public const uint m_RegionBlock = 0x74; // VoxelVisBlockOffset_t
        public const uint m_EnclosedClusterListBlock = 0x7C; // VoxelVisBlockOffset_t
        public const uint m_EnclosedClustersBlock = 0x84; // VoxelVisBlockOffset_t
        public const uint m_MasksBlock = 0x8C; // VoxelVisBlockOffset_t
        public const uint m_nVisBlocks = 0x94; // VoxelVisBlockOffset_t
    }

    public static class ClutterSceneObject_t 
    {
        public const uint m_Bounds = 0x0; // AABB_t
        public const uint m_flags = 0x18; // ObjectTypeFlags_t
        public const uint m_nLayer = 0x1C; // int16_t
        public const uint m_instancePositions = 0x20; // CUtlVector<Vector>
        public const uint m_instanceScales = 0x50; // CUtlVector<float>
        public const uint m_instanceTintSrgb = 0x68; // CUtlVector<Color>
        public const uint m_tiles = 0x80; // CUtlVector<ClutterTile_t>
        public const uint m_renderableModel = 0x98; // CStrongHandle<InfoForResourceTypeCModel>
    }

    public static class ClutterTile_t 
    {
        public const uint m_nFirstInstance = 0x0; // uint32_t
        public const uint m_nLastInstance = 0x4; // uint32_t
        public const uint m_BoundsWs = 0x8; // AABB_t
    }

    public static class EntityIOConnectionData_t 
    {
        public const uint m_outputName = 0x0; // CUtlString
        public const uint m_targetType = 0x8; // uint32_t
        public const uint m_targetName = 0x10; // CUtlString
        public const uint m_inputName = 0x18; // CUtlString
        public const uint m_overrideParam = 0x20; // CUtlString
        public const uint m_flDelay = 0x28; // float
        public const uint m_nTimesToFire = 0x2C; // int32_t
    }

    public static class EntityKeyValueData_t 
    {
        public const uint m_connections = 0x8; // CUtlVector<EntityIOConnectionData_t>
        public const uint m_keyValuesData = 0x20; // CUtlBinaryBlock
    }

    public static class ExtraVertexStreamOverride_t  // BaseSceneObjectOverride_t
    {
        public const uint m_nSubSceneObject = 0x4; // uint32_t
        public const uint m_nDrawCallIndex = 0x8; // uint32_t
        public const uint m_nAdditionalMeshDrawPrimitiveFlags = 0xC; // MeshDrawPrimitiveFlags_t
        public const uint m_extraBufferBinding = 0x10; // CRenderBufferBinding
    }

    public static class InfoForResourceTypeVMapResourceData_t 
    {
    }

    public static class InfoOverlayData_t 
    {
        public const uint m_transform = 0x0; // matrix3x4_t
        public const uint m_flWidth = 0x30; // float
        public const uint m_flHeight = 0x34; // float
        public const uint m_flDepth = 0x38; // float
        public const uint m_vUVStart = 0x3C; // Vector2D
        public const uint m_vUVEnd = 0x44; // Vector2D
        public const uint m_pMaterial = 0x50; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_nRenderOrder = 0x58; // int32_t
        public const uint m_vTintColor = 0x5C; // Vector4D
        public const uint m_nSequenceOverride = 0x6C; // int32_t
    }

    public static class MaterialOverride_t  // BaseSceneObjectOverride_t
    {
        public const uint m_nSubSceneObject = 0x4; // uint32_t
        public const uint m_nDrawCallIndex = 0x8; // uint32_t
        public const uint m_pMaterial = 0x10; // CStrongHandle<InfoForResourceTypeIMaterial2>
    }

    public static class NodeData_t 
    {
        public const uint m_nParent = 0x0; // int32_t
        public const uint m_vOrigin = 0x4; // Vector
        public const uint m_vMinBounds = 0x10; // Vector
        public const uint m_vMaxBounds = 0x1C; // Vector
        public const uint m_flMinimumDistance = 0x28; // float
        public const uint m_ChildNodeIndices = 0x30; // CUtlVector<int32_t>
        public const uint m_worldNodePrefix = 0x48; // CUtlString
    }

    public static class PermEntityLumpData_t 
    {
        public const uint m_name = 0x8; // CUtlString
        public const uint m_hammerUniqueId = 0x10; // CUtlString
        public const uint m_childLumps = 0x18; // CUtlVector<CStrongHandleCopyable<InfoForResourceTypeCEntityLump>>
        public const uint m_entityKeyValues = 0x30; // CUtlLeanVector<EntityKeyValueData_t>
    }

    public static class SceneObject_t 
    {
        public const uint m_nObjectID = 0x0; // uint32_t
        public const uint m_vTransform = 0x4; // Vector4D[3]
        public const uint m_flFadeStartDistance = 0x34; // float
        public const uint m_flFadeEndDistance = 0x38; // float
        public const uint m_vTintColor = 0x3C; // Vector4D
        public const uint m_skin = 0x50; // CUtlString
        public const uint m_nObjectTypeFlags = 0x58; // ObjectTypeFlags_t
        public const uint m_vLightingOrigin = 0x5C; // Vector
        public const uint m_nOverlayRenderOrder = 0x68; // int16_t
        public const uint m_nLODOverride = 0x6A; // int16_t
        public const uint m_nCubeMapPrecomputedHandshake = 0x6C; // int32_t
        public const uint m_nLightProbeVolumePrecomputedHandshake = 0x70; // int32_t
        public const uint m_renderableModel = 0x78; // CStrongHandle<InfoForResourceTypeCModel>
        public const uint m_renderable = 0x80; // CStrongHandle<InfoForResourceTypeCRenderMesh>
    }

    public static class VMapResourceData_t 
    {
    }

    public static class VoxelVisBlockOffset_t 
    {
        public const uint m_nOffset = 0x0; // uint32_t
        public const uint m_nElementCount = 0x4; // uint32_t
    }

    public static class WorldBuilderParams_t 
    {
        public const uint m_flMinDrawVolumeSize = 0x0; // float
        public const uint m_bBuildBakedLighting = 0x4; // bool
        public const uint m_vLightmapUvScale = 0x8; // Vector2D
        public const uint m_nCompileTimestamp = 0x10; // uint64_t
        public const uint m_nCompileFingerprint = 0x18; // uint64_t
    }

    public static class WorldNodeOnDiskBufferData_t 
    {
        public const uint m_nElementCount = 0x0; // int32_t
        public const uint m_nElementSizeInBytes = 0x4; // int32_t
        public const uint m_inputLayoutFields = 0x8; // CUtlVector<RenderInputLayoutField_t>
        public const uint m_pData = 0x20; // CUtlVector<uint8_t>
    }

    public static class WorldNode_t 
    {
        public const uint m_sceneObjects = 0x0; // CUtlVector<SceneObject_t>
        public const uint m_infoOverlays = 0x18; // CUtlVector<InfoOverlayData_t>
        public const uint m_visClusterMembership = 0x30; // CUtlVector<uint16_t>
        public const uint m_aggregateSceneObjects = 0x48; // CUtlVector<AggregateSceneObject_t>
        public const uint m_clutterSceneObjects = 0x60; // CUtlVector<ClutterSceneObject_t>
        public const uint m_extraVertexStreamOverrides = 0x78; // CUtlVector<ExtraVertexStreamOverride_t>
        public const uint m_materialOverrides = 0x90; // CUtlVector<MaterialOverride_t>
        public const uint m_extraVertexStreams = 0xA8; // CUtlVector<WorldNodeOnDiskBufferData_t>
        public const uint m_layerNames = 0xC0; // CUtlVector<CUtlString>
        public const uint m_sceneObjectLayerIndices = 0xD8; // CUtlVector<uint8_t>
        public const uint m_overlayLayerIndices = 0xF0; // CUtlVector<uint8_t>
        public const uint m_grassFileName = 0x108; // CUtlString
        public const uint m_nodeLightingInfo = 0x110; // BakedLightingInfo_t
    }

    public static class World_t 
    {
        public const uint m_builderParams = 0x0; // WorldBuilderParams_t
        public const uint m_worldNodes = 0x20; // CUtlVector<NodeData_t>
        public const uint m_worldLightingInfo = 0x38; // BakedLightingInfo_t
        public const uint m_entityLumps = 0x68; // CUtlVector<CStrongHandleCopyable<InfoForResourceTypeCEntityLump>>
    }
}
