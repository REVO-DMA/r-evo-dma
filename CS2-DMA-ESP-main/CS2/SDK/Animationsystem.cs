namespace cs2_dma_esp.SDK.Animationsystem 
{
    public static class AimMatrixOpFixedSettings_t 
    {
        public const uint m_attachment = 0x0; // CAnimAttachment
        public const uint m_damping = 0x80; // CAnimInputDamping
        public const uint m_poseCacheHandles = 0x90; // CPoseHandle[10]
        public const uint m_eBlendMode = 0xB8; // AimMatrixBlendMode
        public const uint m_fAngleIncrement = 0xBC; // float
        public const uint m_nSequenceMaxFrame = 0xC0; // int32_t
        public const uint m_nBoneMaskIndex = 0xC4; // int32_t
        public const uint m_bTargetIsPosition = 0xC8; // bool
    }

    public static class AnimComponentID 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimNodeID 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimNodeOutputID 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimParamID 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimScriptHandle 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimStateID 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimTagID 
    {
        public const uint m_id = 0x0; // uint32_t
    }

    public static class AnimationDecodeDebugDumpElement_t 
    {
        public const uint m_nEntityIndex = 0x0; // int32_t
        public const uint m_modelName = 0x8; // CUtlString
        public const uint m_poseParams = 0x10; // CUtlVector<CUtlString>
        public const uint m_decodeOps = 0x28; // CUtlVector<CUtlString>
        public const uint m_internalOps = 0x40; // CUtlVector<CUtlString>
        public const uint m_decodedAnims = 0x58; // CUtlVector<CUtlString>
    }

    public static class AnimationDecodeDebugDump_t 
    {
        public const uint m_processingType = 0x0; // AnimationProcessingType_t
        public const uint m_elems = 0x8; // CUtlVector<AnimationDecodeDebugDumpElement_t>
    }

    public static class AnimationSnapshotBase_t 
    {
        public const uint m_flRealTime = 0x0; // float
        public const uint m_rootToWorld = 0x10; // matrix3x4a_t
        public const uint m_bBonesInWorldSpace = 0x40; // bool
        public const uint m_boneSetupMask = 0x48; // CUtlVector<uint32_t>
        public const uint m_boneTransforms = 0x60; // CUtlVector<matrix3x4a_t>
        public const uint m_flexControllers = 0x78; // CUtlVector<float>
        public const uint m_SnapshotType = 0x90; // AnimationSnapshotType_t
        public const uint m_bHasDecodeDump = 0x94; // bool
        public const uint m_DecodeDump = 0x98; // AnimationDecodeDebugDumpElement_t
    }

    public static class AnimationSnapshot_t  // AnimationSnapshotBase_t
    {
        public const uint m_nEntIndex = 0x110; // int32_t
        public const uint m_modelName = 0x118; // CUtlString
    }

    public static class AttachmentHandle_t 
    {
        public const uint m_Value = 0x0; // uint8_t
    }

    public static class BlendItem_t 
    {
        public const uint m_tags = 0x0; // CUtlVector<TagSpan_t>
        public const uint m_pChild = 0x18; // CAnimUpdateNodeRef
        public const uint m_hSequence = 0x28; // HSequence
        public const uint m_vPos = 0x2C; // Vector2D
        public const uint m_flDuration = 0x34; // float
        public const uint m_bUseCustomDuration = 0x38; // bool
    }

    public static class BoneDemoCaptureSettings_t 
    {
        public const uint m_boneName = 0x0; // CUtlString
        public const uint m_flChainLength = 0x8; // float
    }

    public static class CActionComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_actions = 0x30; // CUtlVector<CSmartPtr<CAnimActionUpdater>>
    }

    public static class CAddUpdateNode  // CBinaryUpdateNode
    {
        public const uint m_footMotionTiming = 0x8C; // BinaryNodeChildOption
        public const uint m_bApplyToFootMotion = 0x90; // bool
        public const uint m_bApplyChannelsSeparately = 0x91; // bool
        public const uint m_bUseModelSpace = 0x92; // bool
    }

    public static class CAimConstraint  // CBaseConstraint
    {
        public const uint m_qAimOffset = 0x70; // Quaternion
        public const uint m_nUpType = 0x80; // uint32_t
    }

    public static class CAimMatrixUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedSettings = 0x70; // AimMatrixOpFixedSettings_t
        public const uint m_target = 0x148; // AnimVectorSource
        public const uint m_paramIndex = 0x14C; // CAnimParamHandle
        public const uint m_hSequence = 0x150; // HSequence
        public const uint m_bResetChild = 0x154; // bool
        public const uint m_bLockWhenWaning = 0x155; // bool
    }

    public static class CAnimActionUpdater 
    {
    }

    public static class CAnimActivity 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_nActivity = 0x10; // int32_t
        public const uint m_nFlags = 0x14; // int32_t
        public const uint m_nWeight = 0x18; // int32_t
    }

    public static class CAnimAttachment 
    {
        public const uint m_influenceRotations = 0x0; // Quaternion[3]
        public const uint m_influenceOffsets = 0x30; // VectorAligned[3]
        public const uint m_influenceIndices = 0x60; // int32_t[3]
        public const uint m_influenceWeights = 0x6C; // float[3]
        public const uint m_numInfluences = 0x78; // uint8_t
    }

    public static class CAnimBone 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_parent = 0x10; // int32_t
        public const uint m_pos = 0x14; // Vector
        public const uint m_quat = 0x20; // QuaternionStorage
        public const uint m_scale = 0x30; // float
        public const uint m_qAlignment = 0x34; // QuaternionStorage
        public const uint m_flags = 0x44; // int32_t
    }

    public static class CAnimBoneDifference 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_parent = 0x10; // CBufferString
        public const uint m_posError = 0x20; // Vector
        public const uint m_bHasRotation = 0x2C; // bool
        public const uint m_bHasMovement = 0x2D; // bool
    }

    public static class CAnimComponentUpdater 
    {
        public const uint m_name = 0x18; // CUtlString
        public const uint m_id = 0x20; // AnimComponentID
        public const uint m_networkMode = 0x24; // AnimNodeNetworkMode
        public const uint m_bStartEnabled = 0x28; // bool
    }

    public static class CAnimCycle  // CCycleBase
    {
    }

    public static class CAnimData 
    {
        public const uint m_name = 0x10; // CBufferString
        public const uint m_animArray = 0x20; // CUtlVector<CAnimDesc>
        public const uint m_decoderArray = 0x38; // CUtlVector<CAnimDecoder>
        public const uint m_nMaxUniqueFrameIndex = 0x50; // int32_t
        public const uint m_segmentArray = 0x58; // CUtlVector<CAnimFrameSegment>
    }

    public static class CAnimDataChannelDesc 
    {
        public const uint m_szChannelClass = 0x0; // CBufferString
        public const uint m_szVariableName = 0x10; // CBufferString
        public const uint m_nFlags = 0x20; // int32_t
        public const uint m_nType = 0x24; // int32_t
        public const uint m_szGrouping = 0x28; // CBufferString
        public const uint m_szDescription = 0x38; // CBufferString
        public const uint m_szElementNameArray = 0x48; // CUtlVector<CBufferString>
        public const uint m_nElementIndexArray = 0x60; // CUtlVector<int32_t>
        public const uint m_nElementMaskArray = 0x78; // CUtlVector<uint32_t>
    }

    public static class CAnimDecoder 
    {
        public const uint m_szName = 0x0; // CBufferString
        public const uint m_nVersion = 0x10; // int32_t
        public const uint m_nType = 0x14; // int32_t
    }

    public static class CAnimDemoCaptureSettings 
    {
        public const uint m_rangeBoneChainLength = 0x0; // Vector2D
        public const uint m_rangeMaxSplineErrorRotation = 0x8; // Vector2D
        public const uint m_flMaxSplineErrorTranslation = 0x10; // float
        public const uint m_flMaxSplineErrorScale = 0x14; // float
        public const uint m_flIkRotation_MaxSplineError = 0x18; // float
        public const uint m_flIkTranslation_MaxSplineError = 0x1C; // float
        public const uint m_flMaxQuantizationErrorRotation = 0x20; // float
        public const uint m_flMaxQuantizationErrorTranslation = 0x24; // float
        public const uint m_flMaxQuantizationErrorScale = 0x28; // float
        public const uint m_flIkRotation_MaxQuantizationError = 0x2C; // float
        public const uint m_flIkTranslation_MaxQuantizationError = 0x30; // float
        public const uint m_baseSequence = 0x38; // CUtlString
        public const uint m_nBaseSequenceFrame = 0x40; // int32_t
        public const uint m_boneSelectionMode = 0x44; // EDemoBoneSelectionMode
        public const uint m_bones = 0x48; // CUtlVector<BoneDemoCaptureSettings_t>
        public const uint m_ikChains = 0x60; // CUtlVector<IKDemoCaptureSettings_t>
    }

    public static class CAnimDesc 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_flags = 0x10; // CAnimDesc_Flag
        public const uint fps = 0x18; // float
        public const uint m_Data = 0x20; // CAnimEncodedFrames
        public const uint m_movementArray = 0xF8; // CUtlVector<CAnimMovement>
        public const uint m_eventArray = 0x110; // CUtlVector<CAnimEventDefinition>
        public const uint m_activityArray = 0x128; // CUtlVector<CAnimActivity>
        public const uint m_hierarchyArray = 0x140; // CUtlVector<CAnimLocalHierarchy>
        public const uint framestalltime = 0x158; // float
        public const uint m_vecRootMin = 0x15C; // Vector
        public const uint m_vecRootMax = 0x168; // Vector
        public const uint m_vecBoneWorldMin = 0x178; // CUtlVector<Vector>
        public const uint m_vecBoneWorldMax = 0x190; // CUtlVector<Vector>
        public const uint m_sequenceParams = 0x1A8; // CAnimSequenceParams
    }

    public static class CAnimDesc_Flag 
    {
        public const uint m_bLooping = 0x0; // bool
        public const uint m_bAllZeros = 0x1; // bool
        public const uint m_bHidden = 0x2; // bool
        public const uint m_bDelta = 0x3; // bool
        public const uint m_bLegacyWorldspace = 0x4; // bool
        public const uint m_bModelDoc = 0x5; // bool
        public const uint m_bImplicitSeqIgnoreDelta = 0x6; // bool
        public const uint m_bAnimGraphAdditive = 0x7; // bool
    }

    public static class CAnimEncodeDifference 
    {
        public const uint m_boneArray = 0x0; // CUtlVector<CAnimBoneDifference>
        public const uint m_morphArray = 0x18; // CUtlVector<CAnimMorphDifference>
        public const uint m_userArray = 0x30; // CUtlVector<CAnimUserDifference>
        public const uint m_bHasRotationBitArray = 0x48; // CUtlVector<uint8_t>
        public const uint m_bHasMovementBitArray = 0x60; // CUtlVector<uint8_t>
        public const uint m_bHasMorphBitArray = 0x78; // CUtlVector<uint8_t>
        public const uint m_bHasUserBitArray = 0x90; // CUtlVector<uint8_t>
    }

    public static class CAnimEncodedFrames 
    {
        public const uint m_fileName = 0x0; // CBufferString
        public const uint m_nFrames = 0x10; // int32_t
        public const uint m_nFramesPerBlock = 0x14; // int32_t
        public const uint m_frameblockArray = 0x18; // CUtlVector<CAnimFrameBlockAnim>
        public const uint m_usageDifferences = 0x30; // CAnimEncodeDifference
    }

    public static class CAnimEnum 
    {
        public const uint m_value = 0x0; // uint8_t
    }

    public static class CAnimEventDefinition 
    {
        public const uint m_nFrame = 0x8; // int32_t
        public const uint m_flCycle = 0xC; // float
        public const uint m_EventData = 0x10; // KeyValues3
        public const uint m_sLegacyOptions = 0x20; // CBufferString
        public const uint m_sEventName = 0x30; // CGlobalSymbol
    }

    public static class CAnimFoot 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_vBallOffset = 0x8; // Vector
        public const uint m_vHeelOffset = 0x14; // Vector
        public const uint m_ankleBoneIndex = 0x20; // int32_t
        public const uint m_toeBoneIndex = 0x24; // int32_t
    }

    public static class CAnimFrameBlockAnim 
    {
        public const uint m_nStartFrame = 0x0; // int32_t
        public const uint m_nEndFrame = 0x4; // int32_t
        public const uint m_segmentIndexArray = 0x8; // CUtlVector<int32_t>
    }

    public static class CAnimFrameSegment 
    {
        public const uint m_nUniqueFrameIndex = 0x0; // int32_t
        public const uint m_nLocalElementMasks = 0x4; // uint32_t
        public const uint m_nLocalChannel = 0x8; // int32_t
        public const uint m_container = 0x10; // CUtlBinaryBlock
    }

    public static class CAnimGraphDebugReplay 
    {
        public const uint m_animGraphFileName = 0x40; // CUtlString
        public const uint m_frameList = 0x48; // CUtlVector<CSmartPtr<CAnimReplayFrame>>
        public const uint m_startIndex = 0x60; // int32_t
        public const uint m_writeIndex = 0x64; // int32_t
        public const uint m_frameCount = 0x68; // int32_t
    }

    public static class CAnimGraphModelBinding 
    {
        public const uint m_modelName = 0x8; // CUtlString
        public const uint m_pSharedData = 0x10; // CSmartPtr<CAnimUpdateSharedData>
    }

    public static class CAnimGraphNetworkSettings  // CAnimGraphSettingsGroup
    {
        public const uint m_bNetworkingEnabled = 0x20; // bool
    }

    public static class CAnimGraphSettingsGroup 
    {
    }

    public static class CAnimGraphSettingsManager 
    {
        public const uint m_settingsGroups = 0x18; // CUtlVector<CSmartPtr<CAnimGraphSettingsGroup>>
    }

    public static class CAnimInputDamping 
    {
        public const uint m_speedFunction = 0x8; // DampingSpeedFunction
        public const uint m_fSpeedScale = 0xC; // float
    }

    public static class CAnimKeyData 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_boneArray = 0x10; // CUtlVector<CAnimBone>
        public const uint m_userArray = 0x28; // CUtlVector<CAnimUser>
        public const uint m_morphArray = 0x40; // CUtlVector<CBufferString>
        public const uint m_nChannelElements = 0x58; // int32_t
        public const uint m_dataChannelArray = 0x60; // CUtlVector<CAnimDataChannelDesc>
    }

    public static class CAnimLocalHierarchy 
    {
        public const uint m_sBone = 0x0; // CBufferString
        public const uint m_sNewParent = 0x10; // CBufferString
        public const uint m_nStartFrame = 0x20; // int32_t
        public const uint m_nPeakFrame = 0x24; // int32_t
        public const uint m_nTailFrame = 0x28; // int32_t
        public const uint m_nEndFrame = 0x2C; // int32_t
    }

    public static class CAnimMorphDifference 
    {
        public const uint m_name = 0x0; // CBufferString
    }

    public static class CAnimMotorUpdaterBase 
    {
        public const uint m_name = 0x10; // CUtlString
        public const uint m_bDefault = 0x18; // bool
    }

    public static class CAnimMovement 
    {
        public const uint endframe = 0x0; // int32_t
        public const uint motionflags = 0x4; // int32_t
        public const uint v0 = 0x8; // float
        public const uint v1 = 0xC; // float
        public const uint angle = 0x10; // float
        public const uint vector = 0x14; // Vector
        public const uint position = 0x20; // Vector
    }

    public static class CAnimNodePath 
    {
        public const uint m_path = 0x0; // AnimNodeID[11]
        public const uint m_nCount = 0x2C; // int32_t
    }

    public static class CAnimParamHandle 
    {
        public const uint m_type = 0x0; // AnimParamType_t
        public const uint m_index = 0x1; // uint8_t
    }

    public static class CAnimParamHandleMap 
    {
        public const uint m_list = 0x0; // CUtlHashtable<uint16_t,int16_t>
    }

    public static class CAnimParameterBase 
    {
        public const uint m_name = 0x18; // CGlobalSymbol
        public const uint m_group = 0x20; // CUtlString
        public const uint m_id = 0x28; // AnimParamID
        public const uint m_componentName = 0x40; // CUtlString
        public const uint m_bNetworkingRequested = 0x4C; // bool
        public const uint m_bIsReferenced = 0x4D; // bool
    }

    public static class CAnimParameterManagerUpdater 
    {
        public const uint m_parameters = 0x18; // CUtlVector<CSmartPtr<CAnimParameterBase>>
        public const uint m_idToIndexMap = 0x30; // CUtlHashtable<AnimParamID,int32_t>
        public const uint m_nameToIndexMap = 0x50; // CUtlHashtable<CUtlString,int32_t>
        public const uint m_indexToHandle = 0x70; // CUtlVector<CAnimParamHandle>
        public const uint m_autoResetParams = 0x88; // CUtlVector<CUtlPair<CAnimParamHandle,CAnimVariant>>
        public const uint m_autoResetMap = 0xA0; // CUtlHashtable<CAnimParamHandle,int16_t>
    }

    public static class CAnimReplayFrame 
    {
        public const uint m_inputDataBlocks = 0x10; // CUtlVector<CUtlBinaryBlock>
        public const uint m_instanceData = 0x28; // CUtlBinaryBlock
        public const uint m_startingLocalToWorldTransform = 0x40; // CTransform
        public const uint m_localToWorldTransform = 0x60; // CTransform
        public const uint m_timeStamp = 0x80; // float
    }

    public static class CAnimScriptComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_hScript = 0x30; // AnimScriptHandle
    }

    public static class CAnimScriptManager 
    {
        public const uint m_scriptInfo = 0x10; // CUtlVector<ScriptInfo_t>
    }

    public static class CAnimSequenceParams 
    {
        public const uint m_flFadeInTime = 0x0; // float
        public const uint m_flFadeOutTime = 0x4; // float
    }

    public static class CAnimSkeleton 
    {
        public const uint m_localSpaceTransforms = 0x10; // CUtlVector<CTransform>
        public const uint m_modelSpaceTransforms = 0x28; // CUtlVector<CTransform>
        public const uint m_boneNames = 0x40; // CUtlVector<CUtlString>
        public const uint m_children = 0x58; // CUtlVector<CUtlVector<int32_t>>
        public const uint m_parents = 0x70; // CUtlVector<int32_t>
        public const uint m_feet = 0x88; // CUtlVector<CAnimFoot>
        public const uint m_morphNames = 0xA0; // CUtlVector<CUtlString>
        public const uint m_lodBoneCounts = 0xB8; // CUtlVector<int32_t>
    }

    public static class CAnimStateMachineUpdater 
    {
        public const uint m_states = 0x8; // CUtlVector<CStateUpdateData>
        public const uint m_transitions = 0x20; // CUtlVector<CTransitionUpdateData>
        public const uint m_startStateIndex = 0x50; // int32_t
    }

    public static class CAnimTagBase 
    {
        public const uint m_name = 0x18; // CGlobalSymbol
        public const uint m_group = 0x20; // CGlobalSymbol
        public const uint m_tagID = 0x28; // AnimTagID
        public const uint m_bIsReferenced = 0x2C; // bool
    }

    public static class CAnimTagManagerUpdater 
    {
        public const uint m_tags = 0x18; // CUtlVector<CSmartPtr<CAnimTagBase>>
    }

    public static class CAnimUpdateNodeBase 
    {
        public const uint m_nodePath = 0x18; // CAnimNodePath
        public const uint m_networkMode = 0x48; // AnimNodeNetworkMode
        public const uint m_name = 0x50; // CUtlString
    }

    public static class CAnimUpdateNodeRef 
    {
        public const uint m_nodeIndex = 0x8; // int32_t
    }

    public static class CAnimUpdateSharedData 
    {
        public const uint m_nodes = 0x10; // CUtlVector<CSmartPtr<CAnimUpdateNodeBase>>
        public const uint m_nodeIndexMap = 0x28; // CUtlHashtable<CAnimNodePath,int32_t>
        public const uint m_components = 0x48; // CUtlVector<CSmartPtr<CAnimComponentUpdater>>
        public const uint m_pParamListUpdater = 0x60; // CSmartPtr<CAnimParameterManagerUpdater>
        public const uint m_pTagManagerUpdater = 0x68; // CSmartPtr<CAnimTagManagerUpdater>
        public const uint m_scriptManager = 0x70; // CSmartPtr<CAnimScriptManager>
        public const uint m_settings = 0x78; // CAnimGraphSettingsManager
        public const uint m_pStaticPoseCache = 0xA8; // CSmartPtr<CStaticPoseCacheBuilder>
        public const uint m_pSkeleton = 0xB0; // CSmartPtr<CAnimSkeleton>
        public const uint m_rootNodePath = 0xB8; // CAnimNodePath
    }

    public static class CAnimUser 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_nType = 0x10; // int32_t
    }

    public static class CAnimUserDifference 
    {
        public const uint m_name = 0x0; // CBufferString
        public const uint m_nType = 0x10; // int32_t
    }

    public static class CAnimationGraphVisualizerAxis  // CAnimationGraphVisualizerPrimitiveBase
    {
        public const uint m_xWsTransform = 0x40; // CTransform
        public const uint m_flAxisSize = 0x60; // float
    }

    public static class CAnimationGraphVisualizerLine  // CAnimationGraphVisualizerPrimitiveBase
    {
        public const uint m_vWsPositionStart = 0x40; // VectorAligned
        public const uint m_vWsPositionEnd = 0x50; // VectorAligned
        public const uint m_Color = 0x60; // Color
    }

    public static class CAnimationGraphVisualizerPie  // CAnimationGraphVisualizerPrimitiveBase
    {
        public const uint m_vWsCenter = 0x40; // VectorAligned
        public const uint m_vWsStart = 0x50; // VectorAligned
        public const uint m_vWsEnd = 0x60; // VectorAligned
        public const uint m_Color = 0x70; // Color
    }

    public static class CAnimationGraphVisualizerPrimitiveBase 
    {
        public const uint m_Type = 0x8; // CAnimationGraphVisualizerPrimitiveType
        public const uint m_OwningAnimNodePaths = 0xC; // AnimNodeID[11]
        public const uint m_nOwningAnimNodePathCount = 0x38; // int32_t
    }

    public static class CAnimationGraphVisualizerSphere  // CAnimationGraphVisualizerPrimitiveBase
    {
        public const uint m_vWsPosition = 0x40; // VectorAligned
        public const uint m_flRadius = 0x50; // float
        public const uint m_Color = 0x54; // Color
    }

    public static class CAnimationGraphVisualizerText  // CAnimationGraphVisualizerPrimitiveBase
    {
        public const uint m_vWsPosition = 0x40; // VectorAligned
        public const uint m_Color = 0x50; // Color
        public const uint m_Text = 0x58; // CUtlString
    }

    public static class CAnimationGroup 
    {
        public const uint m_nFlags = 0x10; // uint32_t
        public const uint m_name = 0x18; // CBufferString
        public const uint m_localHAnimArray_Handle = 0x60; // CUtlVector<CStrongHandle<InfoForResourceTypeCAnimData>>
        public const uint m_includedGroupArray_Handle = 0x78; // CUtlVector<CStrongHandle<InfoForResourceTypeCAnimationGroup>>
        public const uint m_directHSeqGroup_Handle = 0x90; // CStrongHandle<InfoForResourceTypeCSequenceGroupData>
        public const uint m_decodeKey = 0x98; // CAnimKeyData
        public const uint m_szScripts = 0x110; // CUtlVector<CBufferString>
    }

    public static class CAttachment 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_influenceNames = 0x8; // CUtlString[3]
        public const uint m_vInfluenceRotations = 0x20; // Quaternion[3]
        public const uint m_vInfluenceOffsets = 0x50; // Vector[3]
        public const uint m_influenceWeights = 0x74; // float[3]
        public const uint m_bInfluenceRootTransform = 0x80; // bool[3]
        public const uint m_nInfluences = 0x83; // uint8_t
        public const uint m_bIgnoreRotation = 0x84; // bool
    }

    public static class CAudioAnimTag  // CAnimTagBase
    {
        public const uint m_clipName = 0x38; // CUtlString
        public const uint m_attachmentName = 0x40; // CUtlString
        public const uint m_flVolume = 0x48; // float
        public const uint m_bStopWhenTagEnds = 0x4C; // bool
        public const uint m_bStopWhenGraphEnds = 0x4D; // bool
        public const uint m_bPlayOnServer = 0x4E; // bool
        public const uint m_bPlayOnClient = 0x4F; // bool
    }

    public static class CBaseConstraint  // CBoneConstraintBase
    {
        public const uint m_name = 0x28; // CUtlString
        public const uint m_vUpVector = 0x30; // Vector
        public const uint m_slaves = 0x40; // CUtlVector<CConstraintSlave>
        public const uint m_targets = 0x58; // CUtlVector<CConstraintTarget>
    }

    public static class CBinaryUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_pChild1 = 0x58; // CAnimUpdateNodeRef
        public const uint m_pChild2 = 0x68; // CAnimUpdateNodeRef
        public const uint m_timingBehavior = 0x78; // BinaryNodeTiming
        public const uint m_flTimingBlend = 0x7C; // float
        public const uint m_bResetChild1 = 0x80; // bool
        public const uint m_bResetChild2 = 0x81; // bool
    }

    public static class CBindPoseUpdateNode  // CLeafUpdateNode
    {
    }

    public static class CBlend2DUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_items = 0x60; // CUtlVector<BlendItem_t>
        public const uint m_tags = 0x78; // CUtlVector<TagSpan_t>
        public const uint m_paramSpans = 0x90; // CParamSpanUpdater
        public const uint m_nodeItemIndices = 0xA8; // CUtlVector<int32_t>
        public const uint m_damping = 0xC0; // CAnimInputDamping
        public const uint m_blendSourceX = 0xD0; // AnimValueSource
        public const uint m_paramX = 0xD4; // CAnimParamHandle
        public const uint m_blendSourceY = 0xD8; // AnimValueSource
        public const uint m_paramY = 0xDC; // CAnimParamHandle
        public const uint m_eBlendMode = 0xE0; // Blend2DMode
        public const uint m_playbackSpeed = 0xE4; // float
        public const uint m_bLoop = 0xE8; // bool
        public const uint m_bLockBlendOnReset = 0xE9; // bool
        public const uint m_bLockWhenWaning = 0xEA; // bool
        public const uint m_bAnimEventsAndTagsOnMostWeightedOnly = 0xEB; // bool
    }

    public static class CBlendCurve 
    {
        public const uint m_flControlPoint1 = 0x0; // float
        public const uint m_flControlPoint2 = 0x4; // float
    }

    public static class CBlendUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_children = 0x60; // CUtlVector<CAnimUpdateNodeRef>
        public const uint m_sortedOrder = 0x78; // CUtlVector<uint8_t>
        public const uint m_targetValues = 0x90; // CUtlVector<float>
        public const uint m_blendValueSource = 0xAC; // AnimValueSource
        public const uint m_paramIndex = 0xB0; // CAnimParamHandle
        public const uint m_damping = 0xB8; // CAnimInputDamping
        public const uint m_blendKeyType = 0xC8; // BlendKeyType
        public const uint m_bLockBlendOnReset = 0xCC; // bool
        public const uint m_bSyncCycles = 0xCD; // bool
        public const uint m_bLoop = 0xCE; // bool
        public const uint m_bLockWhenWaning = 0xCF; // bool
    }

    public static class CBlockSelectionMetricEvaluator  // CMotionMetricEvaluator
    {
    }

    public static class CBodyGroupAnimTag  // CAnimTagBase
    {
        public const uint m_nPriority = 0x38; // int32_t
        public const uint m_bodyGroupSettings = 0x40; // CUtlVector<CBodyGroupSetting>
    }

    public static class CBodyGroupSetting 
    {
        public const uint m_BodyGroupName = 0x0; // CUtlString
        public const uint m_nBodyGroupOption = 0x8; // int32_t
    }

    public static class CBoneConstraintBase 
    {
    }

    public static class CBoneConstraintDotToMorph  // CBoneConstraintBase
    {
        public const uint m_sBoneName = 0x28; // CUtlString
        public const uint m_sTargetBoneName = 0x30; // CUtlString
        public const uint m_sMorphChannelName = 0x38; // CUtlString
        public const uint m_flRemap = 0x40; // float[4]
    }

    public static class CBoneConstraintPoseSpaceBone  // CBaseConstraint
    {
        public const uint m_inputList = 0x70; // CUtlVector<CBoneConstraintPoseSpaceBone::Input_t>
    }

    public static class CBoneConstraintPoseSpaceBone_Input_t 
    {
        public const uint m_inputValue = 0x0; // Vector
        public const uint m_outputTransformList = 0x10; // CUtlVector<CTransform>
    }

    public static class CBoneConstraintPoseSpaceMorph  // CBoneConstraintBase
    {
        public const uint m_sBoneName = 0x28; // CUtlString
        public const uint m_sAttachmentName = 0x30; // CUtlString
        public const uint m_outputMorph = 0x38; // CUtlVector<CUtlString>
        public const uint m_inputList = 0x50; // CUtlVector<CBoneConstraintPoseSpaceMorph::Input_t>
        public const uint m_bClamp = 0x68; // bool
    }

    public static class CBoneConstraintPoseSpaceMorph_Input_t 
    {
        public const uint m_inputValue = 0x0; // Vector
        public const uint m_outputWeightList = 0x10; // CUtlVector<float>
    }

    public static class CBoneMaskUpdateNode  // CBinaryUpdateNode
    {
        public const uint m_nWeightListIndex = 0x8C; // int32_t
        public const uint m_flRootMotionBlend = 0x90; // float
        public const uint m_blendSpace = 0x94; // BoneMaskBlendSpace
        public const uint m_footMotionTiming = 0x98; // BinaryNodeChildOption
        public const uint m_bUseBlendScale = 0x9C; // bool
        public const uint m_blendValueSource = 0xA0; // AnimValueSource
        public const uint m_hBlendParameter = 0xA4; // CAnimParamHandle
    }

    public static class CBonePositionMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_nBoneIndex = 0x50; // int32_t
    }

    public static class CBoneVelocityMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_nBoneIndex = 0x50; // int32_t
    }

    public static class CBoolAnimParameter  // CConcreteAnimParameter
    {
        public const uint m_bDefaultValue = 0x60; // bool
    }

    public static class CCPPScriptComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_scriptsToRun = 0x30; // CUtlVector<CGlobalSymbol>
    }

    public static class CCachedPose 
    {
        public const uint m_transforms = 0x8; // CUtlVector<CTransform>
        public const uint m_morphWeights = 0x20; // CUtlVector<float>
        public const uint m_hSequence = 0x38; // HSequence
        public const uint m_flCycle = 0x3C; // float
    }

    public static class CChoiceUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_children = 0x58; // CUtlVector<CAnimUpdateNodeRef>
        public const uint m_weights = 0x70; // CUtlVector<float>
        public const uint m_blendTimes = 0x88; // CUtlVector<float>
        public const uint m_choiceMethod = 0xA0; // ChoiceMethod
        public const uint m_choiceChangeMethod = 0xA4; // ChoiceChangeMethod
        public const uint m_blendMethod = 0xA8; // ChoiceBlendMethod
        public const uint m_blendTime = 0xAC; // float
        public const uint m_bCrossFade = 0xB0; // bool
        public const uint m_bResetChosen = 0xB1; // bool
        public const uint m_bDontResetSameSelection = 0xB2; // bool
    }

    public static class CChoreoUpdateNode  // CUnaryUpdateNode
    {
    }

    public static class CClothSettingsAnimTag  // CAnimTagBase
    {
        public const uint m_flStiffness = 0x38; // float
        public const uint m_flEaseIn = 0x3C; // float
        public const uint m_flEaseOut = 0x40; // float
        public const uint m_nVertexSet = 0x48; // CUtlString
    }

    public static class CCompressorGroup 
    {
        public const uint m_nTotalElementCount = 0x0; // int32_t
        public const uint m_szChannelClass = 0x8; // CUtlVector<char*>
        public const uint m_szVariableName = 0x20; // CUtlVector<char*>
        public const uint m_nType = 0x38; // CUtlVector<fieldtype_t>
        public const uint m_nFlags = 0x50; // CUtlVector<int32_t>
        public const uint m_szGrouping = 0x68; // CUtlVector<CUtlString>
        public const uint m_nCompressorIndex = 0x80; // CUtlVector<int32_t>
        public const uint m_szElementNames = 0x98; // CUtlVector<CUtlVector<char*>>
        public const uint m_nElementUniqueID = 0xB0; // CUtlVector<CUtlVector<int32_t>>
        public const uint m_nElementMask = 0xC8; // CUtlVector<uint32_t>
        public const uint m_vectorCompressor = 0xF8; // CUtlVector<CCompressor<Vector>*>
        public const uint m_quaternionCompressor = 0x110; // CUtlVector<CCompressor<QuaternionStorage>*>
        public const uint m_intCompressor = 0x128; // CUtlVector<CCompressor<int32_t>*>
        public const uint m_boolCompressor = 0x140; // CUtlVector<CCompressor<bool>*>
        public const uint m_colorCompressor = 0x158; // CUtlVector<CCompressor<Color>*>
        public const uint m_vector2DCompressor = 0x170; // CUtlVector<CCompressor<Vector2D>*>
        public const uint m_vector4DCompressor = 0x188; // CUtlVector<CCompressor<Vector4D>*>
    }

    public static class CConcreteAnimParameter  // CAnimParameterBase
    {
        public const uint m_previewButton = 0x50; // AnimParamButton_t
        public const uint m_eNetworkSetting = 0x54; // AnimParamNetworkSetting
        public const uint m_bUseMostRecentValue = 0x58; // bool
        public const uint m_bAutoReset = 0x59; // bool
        public const uint m_bGameWritable = 0x5A; // bool
        public const uint m_bGraphWritable = 0x5B; // bool
    }

    public static class CConstraintSlave 
    {
        public const uint m_qBaseOrientation = 0x0; // Quaternion
        public const uint m_vBasePosition = 0x10; // Vector
        public const uint m_nBoneHash = 0x1C; // uint32_t
        public const uint m_flWeight = 0x20; // float
        public const uint m_sName = 0x28; // CUtlString
    }

    public static class CConstraintTarget 
    {
        public const uint m_qOffset = 0x20; // Quaternion
        public const uint m_vOffset = 0x30; // Vector
        public const uint m_nBoneHash = 0x3C; // uint32_t
        public const uint m_sName = 0x40; // CUtlString
        public const uint m_flWeight = 0x48; // float
        public const uint m_bIsAttachment = 0x59; // bool
    }

    public static class CCurrentRotationVelocityMetricEvaluator  // CMotionMetricEvaluator
    {
    }

    public static class CCurrentVelocityMetricEvaluator  // CMotionMetricEvaluator
    {
    }

    public static class CCycleBase 
    {
        public const uint m_flCycle = 0x0; // float
    }

    public static class CCycleControlClipUpdateNode  // CLeafUpdateNode
    {
        public const uint m_tags = 0x60; // CUtlVector<TagSpan_t>
        public const uint m_hSequence = 0x7C; // HSequence
        public const uint m_duration = 0x80; // float
        public const uint m_valueSource = 0x84; // AnimValueSource
        public const uint m_paramIndex = 0x88; // CAnimParamHandle
    }

    public static class CCycleControlUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_valueSource = 0x68; // AnimValueSource
        public const uint m_paramIndex = 0x6C; // CAnimParamHandle
    }

    public static class CDampedPathAnimMotorUpdater  // CPathAnimMotorUpdaterBase
    {
        public const uint m_flAnticipationTime = 0x2C; // float
        public const uint m_flMinSpeedScale = 0x30; // float
        public const uint m_hAnticipationPosParam = 0x34; // CAnimParamHandle
        public const uint m_hAnticipationHeadingParam = 0x36; // CAnimParamHandle
        public const uint m_flSpringConstant = 0x38; // float
        public const uint m_flMinSpringTension = 0x3C; // float
        public const uint m_flMaxSpringTension = 0x40; // float
    }

    public static class CDampedValueComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_items = 0x30; // CUtlVector<CDampedValueUpdateItem>
    }

    public static class CDampedValueUpdateItem 
    {
        public const uint m_damping = 0x0; // CAnimInputDamping
        public const uint m_hParamIn = 0x18; // CAnimParamHandle
        public const uint m_hParamOut = 0x1A; // CAnimParamHandle
    }

    public static class CDemoSettingsComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_settings = 0x30; // CAnimDemoCaptureSettings
    }

    public static class CDirectPlaybackTagData 
    {
        public const uint m_sequenceName = 0x0; // CUtlString
        public const uint m_tags = 0x8; // CUtlVector<TagSpan_t>
    }

    public static class CDirectPlaybackUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_bFinishEarly = 0x6C; // bool
        public const uint m_bResetOnFinish = 0x6D; // bool
        public const uint m_allTags = 0x70; // CUtlVector<CDirectPlaybackTagData>
    }

    public static class CDirectionalBlendUpdateNode  // CLeafUpdateNode
    {
        public const uint m_hSequences = 0x5C; // HSequence[8]
        public const uint m_damping = 0x80; // CAnimInputDamping
        public const uint m_blendValueSource = 0x90; // AnimValueSource
        public const uint m_paramIndex = 0x94; // CAnimParamHandle
        public const uint m_playbackSpeed = 0x98; // float
        public const uint m_duration = 0x9C; // float
        public const uint m_bLoop = 0xA0; // bool
        public const uint m_bLockBlendOnReset = 0xA1; // bool
    }

    public static class CDistanceRemainingMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_flMaxDistance = 0x50; // float
        public const uint m_flMinDistance = 0x54; // float
        public const uint m_flStartGoalFilterDistance = 0x58; // float
        public const uint m_flMaxGoalOvershootScale = 0x5C; // float
        public const uint m_bFilterFixedMinDistance = 0x60; // bool
        public const uint m_bFilterGoalDistance = 0x61; // bool
        public const uint m_bFilterGoalOvershoot = 0x62; // bool
    }

    public static class CDrawCullingData 
    {
        public const uint m_vConeApex = 0x0; // Vector
        public const uint m_ConeAxis = 0xC; // int8_t[3]
        public const uint m_ConeCutoff = 0xF; // int8_t
    }

    public static class CEditableMotionGraph  // CMotionGraph
    {
    }

    public static class CEmitTagActionUpdater  // CAnimActionUpdater
    {
        public const uint m_nTagIndex = 0x18; // int32_t
        public const uint m_bIsZeroDuration = 0x1C; // bool
    }

    public static class CEnumAnimParameter  // CConcreteAnimParameter
    {
        public const uint m_defaultValue = 0x68; // uint8_t
        public const uint m_enumOptions = 0x70; // CUtlVector<CUtlString>
    }

    public static class CExpressionActionUpdater  // CAnimActionUpdater
    {
        public const uint m_hParam = 0x18; // CAnimParamHandle
        public const uint m_eParamType = 0x1A; // AnimParamType_t
        public const uint m_hScript = 0x1C; // AnimScriptHandle
    }

    public static class CFingerBone 
    {
        public const uint m_boneName = 0x0; // CUtlString
        public const uint m_hingeAxis = 0x8; // Vector
        public const uint m_vCapsulePos1 = 0x14; // Vector
        public const uint m_vCapsulePos2 = 0x20; // Vector
        public const uint m_flMinAngle = 0x2C; // float
        public const uint m_flMaxAngle = 0x30; // float
        public const uint m_flRadius = 0x34; // float
    }

    public static class CFingerChain 
    {
        public const uint m_targets = 0x0; // CUtlVector<CFingerSource>
        public const uint m_bones = 0x18; // CUtlVector<CFingerBone>
        public const uint m_name = 0x30; // CUtlString
        public const uint m_tipParentBoneName = 0x38; // CUtlString
        public const uint m_vTipOffset = 0x40; // Vector
        public const uint m_metacarpalBoneName = 0x50; // CUtlString
        public const uint m_vSplayHingeAxis = 0x58; // Vector
        public const uint m_flSplayMinAngle = 0x64; // float
        public const uint m_flSplayMaxAngle = 0x68; // float
        public const uint m_flFingerScaleRatio = 0x6C; // float
    }

    public static class CFingerSource 
    {
        public const uint m_nFingerIndex = 0x0; // AnimVRFinger_t
        public const uint m_flFingerWeight = 0x4; // float
    }

    public static class CFlexController 
    {
        public const uint m_szName = 0x0; // CUtlString
        public const uint m_szType = 0x8; // CUtlString
        public const uint min = 0x10; // float
        public const uint max = 0x14; // float
    }

    public static class CFlexDesc 
    {
        public const uint m_szFacs = 0x0; // CUtlString
    }

    public static class CFlexOp 
    {
        public const uint m_OpCode = 0x0; // FlexOpCode_t
        public const uint m_Data = 0x4; // int32_t
    }

    public static class CFlexRule 
    {
        public const uint m_nFlex = 0x0; // int32_t
        public const uint m_FlexOps = 0x8; // CUtlVector<CFlexOp>
    }

    public static class CFloatAnimParameter  // CConcreteAnimParameter
    {
        public const uint m_fDefaultValue = 0x60; // float
        public const uint m_fMinValue = 0x64; // float
        public const uint m_fMaxValue = 0x68; // float
        public const uint m_bInterpolate = 0x6C; // bool
    }

    public static class CFollowAttachmentUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedData = 0x70; // FollowAttachmentSettings_t
    }

    public static class CFollowPathUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_flBlendOutTime = 0x6C; // float
        public const uint m_bBlockNonPathMovement = 0x70; // bool
        public const uint m_bStopFeetAtGoal = 0x71; // bool
        public const uint m_bScaleSpeed = 0x72; // bool
        public const uint m_flScale = 0x74; // float
        public const uint m_flMinAngle = 0x78; // float
        public const uint m_flMaxAngle = 0x7C; // float
        public const uint m_flSpeedScaleBlending = 0x80; // float
        public const uint m_turnDamping = 0x88; // CAnimInputDamping
        public const uint m_facingTarget = 0x98; // AnimValueSource
        public const uint m_hParam = 0x9C; // CAnimParamHandle
        public const uint m_flTurnToFaceOffset = 0xA0; // float
        public const uint m_bTurnToFace = 0xA4; // bool
    }

    public static class CFootAdjustmentUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_clips = 0x70; // CUtlVector<HSequence>
        public const uint m_hBasePoseCacheHandle = 0x88; // CPoseHandle
        public const uint m_facingTarget = 0x8C; // CAnimParamHandle
        public const uint m_flTurnTimeMin = 0x90; // float
        public const uint m_flTurnTimeMax = 0x94; // float
        public const uint m_flStepHeightMax = 0x98; // float
        public const uint m_flStepHeightMaxAngle = 0x9C; // float
        public const uint m_bResetChild = 0xA0; // bool
        public const uint m_bAnimationDriven = 0xA1; // bool
    }

    public static class CFootCycle  // CCycleBase
    {
    }

    public static class CFootCycleDefinition 
    {
        public const uint m_vStancePositionMS = 0x0; // Vector
        public const uint m_vMidpointPositionMS = 0xC; // Vector
        public const uint m_flStanceDirectionMS = 0x18; // float
        public const uint m_vToStrideStartPos = 0x1C; // Vector
        public const uint m_stanceCycle = 0x28; // CAnimCycle
        public const uint m_footLiftCycle = 0x2C; // CFootCycle
        public const uint m_footOffCycle = 0x30; // CFootCycle
        public const uint m_footStrikeCycle = 0x34; // CFootCycle
        public const uint m_footLandCycle = 0x38; // CFootCycle
    }

    public static class CFootCycleMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_footIndices = 0x50; // CUtlVector<int32_t>
    }

    public static class CFootDefinition 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_ankleBoneName = 0x8; // CUtlString
        public const uint m_toeBoneName = 0x10; // CUtlString
        public const uint m_vBallOffset = 0x18; // Vector
        public const uint m_vHeelOffset = 0x24; // Vector
        public const uint m_flFootLength = 0x30; // float
        public const uint m_flBindPoseDirectionMS = 0x34; // float
        public const uint m_flTraceHeight = 0x38; // float
        public const uint m_flTraceRadius = 0x3C; // float
    }

    public static class CFootFallAnimTag  // CAnimTagBase
    {
        public const uint m_foot = 0x38; // FootFallTagFoot_t
    }

    public static class CFootLockUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedSettings = 0x68; // FootLockPoseOpFixedSettings
        public const uint m_footSettings = 0xD0; // CUtlVector<FootFixedSettings>
        public const uint m_hipShiftDamping = 0xE8; // CAnimInputDamping
        public const uint m_rootHeightDamping = 0xF8; // CAnimInputDamping
        public const uint m_flStrideCurveScale = 0x108; // float
        public const uint m_flStrideCurveLimitScale = 0x10C; // float
        public const uint m_flStepHeightIncreaseScale = 0x110; // float
        public const uint m_flStepHeightDecreaseScale = 0x114; // float
        public const uint m_flHipShiftScale = 0x118; // float
        public const uint m_flBlendTime = 0x11C; // float
        public const uint m_flMaxRootHeightOffset = 0x120; // float
        public const uint m_flMinRootHeightOffset = 0x124; // float
        public const uint m_flTiltPlanePitchSpringStrength = 0x128; // float
        public const uint m_flTiltPlaneRollSpringStrength = 0x12C; // float
        public const uint m_bApplyFootRotationLimits = 0x130; // bool
        public const uint m_bApplyHipShift = 0x131; // bool
        public const uint m_bModulateStepHeight = 0x132; // bool
        public const uint m_bResetChild = 0x133; // bool
        public const uint m_bEnableVerticalCurvedPaths = 0x134; // bool
        public const uint m_bEnableRootHeightDamping = 0x135; // bool
    }

    public static class CFootMotion 
    {
        public const uint m_strides = 0x0; // CUtlVector<CFootStride>
        public const uint m_name = 0x18; // CUtlString
        public const uint m_bAdditive = 0x20; // bool
    }

    public static class CFootPinningUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_poseOpFixedData = 0x70; // FootPinningPoseOpFixedData_t
        public const uint m_eTimingSource = 0xA0; // FootPinningTimingSource
        public const uint m_params = 0xA8; // CUtlVector<CAnimParamHandle>
        public const uint m_bResetChild = 0xC0; // bool
    }

    public static class CFootPositionMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_footIndices = 0x50; // CUtlVector<int32_t>
        public const uint m_bIgnoreSlope = 0x68; // bool
    }

    public static class CFootStepTriggerUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_triggers = 0x68; // CUtlVector<FootStepTrigger>
        public const uint m_flTolerance = 0x84; // float
    }

    public static class CFootStride 
    {
        public const uint m_definition = 0x0; // CFootCycleDefinition
        public const uint m_trajectories = 0x40; // CFootTrajectories
    }

    public static class CFootTrajectories 
    {
        public const uint m_trajectories = 0x0; // CUtlVector<CFootTrajectory>
    }

    public static class CFootTrajectory 
    {
        public const uint m_vOffset = 0x0; // Vector
        public const uint m_flRotationOffset = 0xC; // float
        public const uint m_flProgression = 0x10; // float
    }

    public static class CFootstepLandedAnimTag  // CAnimTagBase
    {
        public const uint m_FootstepType = 0x38; // FootstepLandedFootSoundType_t
        public const uint m_OverrideSoundName = 0x40; // CUtlString
        public const uint m_DebugAnimSourceString = 0x48; // CUtlString
        public const uint m_BoneName = 0x50; // CUtlString
    }

    public static class CFutureFacingMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_flDistance = 0x50; // float
        public const uint m_flTime = 0x54; // float
    }

    public static class CFutureVelocityMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_flDistance = 0x50; // float
        public const uint m_flStoppingDistance = 0x54; // float
        public const uint m_flTargetSpeed = 0x58; // float
        public const uint m_eMode = 0x5C; // VelocityMetricMode
    }

    public static class CHitBox 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_sSurfaceProperty = 0x8; // CUtlString
        public const uint m_sBoneName = 0x10; // CUtlString
        public const uint m_vMinBounds = 0x18; // Vector
        public const uint m_vMaxBounds = 0x24; // Vector
        public const uint m_flShapeRadius = 0x30; // float
        public const uint m_nBoneNameHash = 0x34; // uint32_t
        public const uint m_nGroupId = 0x38; // int32_t
        public const uint m_nShapeType = 0x3C; // uint8_t
        public const uint m_bTranslationOnly = 0x3D; // bool
        public const uint m_CRC = 0x40; // uint32_t
        public const uint m_cRenderColor = 0x44; // Color
        public const uint m_nHitBoxIndex = 0x48; // uint16_t
    }

    public static class CHitBoxSet 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_nNameHash = 0x8; // uint32_t
        public const uint m_HitBoxes = 0x10; // CUtlVector<CHitBox>
        public const uint m_SourceFilename = 0x28; // CUtlString
    }

    public static class CHitBoxSetList 
    {
        public const uint m_HitBoxSets = 0x0; // CUtlVector<CHitBoxSet>
    }

    public static class CHitReactUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedSettings = 0x68; // HitReactFixedSettings_t
        public const uint m_triggerParam = 0xB4; // CAnimParamHandle
        public const uint m_hitBoneParam = 0xB6; // CAnimParamHandle
        public const uint m_hitOffsetParam = 0xB8; // CAnimParamHandle
        public const uint m_hitDirectionParam = 0xBA; // CAnimParamHandle
        public const uint m_hitStrengthParam = 0xBC; // CAnimParamHandle
        public const uint m_flMinDelayBetweenHits = 0xC0; // float
        public const uint m_bResetChild = 0xC4; // bool
    }

    public static class CInputStreamUpdateNode  // CLeafUpdateNode
    {
    }

    public static class CIntAnimParameter  // CConcreteAnimParameter
    {
        public const uint m_defaultValue = 0x60; // int32_t
        public const uint m_minValue = 0x64; // int32_t
        public const uint m_maxValue = 0x68; // int32_t
    }

    public static class CJiggleBoneUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedData = 0x68; // JiggleBoneSettingsList_t
    }

    public static class CJumpHelperUpdateNode  // CSequenceUpdateNode
    {
        public const uint m_hTargetParam = 0xA8; // CAnimParamHandle
        public const uint m_flOriginalJumpMovement = 0xAC; // Vector
        public const uint m_flOriginalJumpDuration = 0xB8; // float
        public const uint m_flJumpStartCycle = 0xBC; // float
        public const uint m_flJumpEndCycle = 0xC0; // float
        public const uint m_eCorrectionMethod = 0xC4; // JumpCorrectionMethod
        public const uint m_bTranslationAxis = 0xC8; // bool[3]
        public const uint m_bScaleSpeed = 0xCB; // bool
    }

    public static class CLODComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_nServerLOD = 0x30; // int32_t
    }

    public static class CLeafUpdateNode  // CAnimUpdateNodeBase
    {
    }

    public static class CLeanMatrixUpdateNode  // CLeafUpdateNode
    {
        public const uint m_frameCorners = 0x5C; // int32_t[3][3]
        public const uint m_poses = 0x80; // CPoseHandle[9]
        public const uint m_damping = 0xA8; // CAnimInputDamping
        public const uint m_blendSource = 0xB8; // AnimVectorSource
        public const uint m_paramIndex = 0xBC; // CAnimParamHandle
        public const uint m_verticalAxis = 0xC0; // Vector
        public const uint m_horizontalAxis = 0xCC; // Vector
        public const uint m_hSequence = 0xD8; // HSequence
        public const uint m_flMaxValue = 0xDC; // float
        public const uint m_nSequenceMaxFrame = 0xE0; // int32_t
    }

    public static class CLookAtUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedSettings = 0x70; // LookAtOpFixedSettings_t
        public const uint m_target = 0x138; // AnimVectorSource
        public const uint m_paramIndex = 0x13C; // CAnimParamHandle
        public const uint m_weightParamIndex = 0x13E; // CAnimParamHandle
        public const uint m_bResetChild = 0x140; // bool
        public const uint m_bLockWhenWaning = 0x141; // bool
    }

    public static class CLookComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_hLookHeading = 0x34; // CAnimParamHandle
        public const uint m_hLookHeadingVelocity = 0x36; // CAnimParamHandle
        public const uint m_hLookPitch = 0x38; // CAnimParamHandle
        public const uint m_hLookDistance = 0x3A; // CAnimParamHandle
        public const uint m_hLookDirection = 0x3C; // CAnimParamHandle
        public const uint m_hLookTarget = 0x3E; // CAnimParamHandle
        public const uint m_hLookTargetWorldSpace = 0x40; // CAnimParamHandle
        public const uint m_bNetworkLookTarget = 0x42; // bool
    }

    public static class CMaterialAttributeAnimTag  // CAnimTagBase
    {
        public const uint m_AttributeName = 0x38; // CUtlString
        public const uint m_AttributeType = 0x40; // MatterialAttributeTagType_t
        public const uint m_flValue = 0x44; // float
        public const uint m_Color = 0x48; // Color
    }

    public static class CMaterialDrawDescriptor 
    {
        public const uint m_nPrimitiveType = 0x0; // RenderPrimitiveType_t
        public const uint m_nBaseVertex = 0x4; // int32_t
        public const uint m_nVertexCount = 0x8; // int32_t
        public const uint m_nStartIndex = 0xC; // int32_t
        public const uint m_nIndexCount = 0x10; // int32_t
        public const uint m_flUvDensity = 0x14; // float
        public const uint m_vTintColor = 0x18; // Vector
        public const uint m_flAlpha = 0x24; // float
        public const uint m_nFirstMeshlet = 0x2C; // uint32_t
        public const uint m_nNumMeshlets = 0x30; // uint16_t
        public const uint m_indexBuffer = 0xB8; // CRenderBufferBinding
        public const uint m_material = 0xE0; // CStrongHandle<InfoForResourceTypeIMaterial2>
    }

    public static class CMeshletDescriptor 
    {
        public const uint m_PackedAABB = 0x0; // PackedAABB_t
        public const uint m_CullingData = 0x8; // CDrawCullingData
    }

    public static class CModelConfig 
    {
        public const uint m_ConfigName = 0x0; // CUtlString
        public const uint m_Elements = 0x8; // CUtlVector<CModelConfigElement*>
        public const uint m_bTopLevel = 0x20; // bool
    }

    public static class CModelConfigElement 
    {
        public const uint m_ElementName = 0x8; // CUtlString
        public const uint m_NestedElements = 0x10; // CUtlVector<CModelConfigElement*>
    }

    public static class CModelConfigElement_AttachedModel  // CModelConfigElement
    {
        public const uint m_InstanceName = 0x48; // CUtlString
        public const uint m_EntityClass = 0x50; // CUtlString
        public const uint m_hModel = 0x58; // CStrongHandle<InfoForResourceTypeCModel>
        public const uint m_vOffset = 0x60; // Vector
        public const uint m_aAngOffset = 0x6C; // QAngle
        public const uint m_AttachmentName = 0x78; // CUtlString
        public const uint m_LocalAttachmentOffsetName = 0x80; // CUtlString
        public const uint m_AttachmentType = 0x88; // ModelConfigAttachmentType_t
        public const uint m_bBoneMergeFlex = 0x8C; // bool
        public const uint m_bUserSpecifiedColor = 0x8D; // bool
        public const uint m_bUserSpecifiedMaterialGroup = 0x8E; // bool
        public const uint m_bAcceptParentMaterialDrivenDecals = 0x8F; // bool
        public const uint m_BodygroupOnOtherModels = 0x90; // CUtlString
        public const uint m_MaterialGroupOnOtherModels = 0x98; // CUtlString
    }

    public static class CModelConfigElement_Command  // CModelConfigElement
    {
        public const uint m_Command = 0x48; // CUtlString
        public const uint m_Args = 0x50; // KeyValues3
    }

    public static class CModelConfigElement_RandomColor  // CModelConfigElement
    {
        public const uint m_Gradient = 0x48; // CColorGradient
    }

    public static class CModelConfigElement_RandomPick  // CModelConfigElement
    {
        public const uint m_Choices = 0x48; // CUtlVector<CUtlString>
        public const uint m_ChoiceWeights = 0x60; // CUtlVector<float>
    }

    public static class CModelConfigElement_SetBodygroup  // CModelConfigElement
    {
        public const uint m_GroupName = 0x48; // CUtlString
        public const uint m_nChoice = 0x50; // int32_t
    }

    public static class CModelConfigElement_SetBodygroupOnAttachedModels  // CModelConfigElement
    {
        public const uint m_GroupName = 0x48; // CUtlString
        public const uint m_nChoice = 0x50; // int32_t
    }

    public static class CModelConfigElement_SetMaterialGroup  // CModelConfigElement
    {
        public const uint m_MaterialGroupName = 0x48; // CUtlString
    }

    public static class CModelConfigElement_SetMaterialGroupOnAttachedModels  // CModelConfigElement
    {
        public const uint m_MaterialGroupName = 0x48; // CUtlString
    }

    public static class CModelConfigElement_SetRenderColor  // CModelConfigElement
    {
        public const uint m_Color = 0x48; // Color
    }

    public static class CModelConfigElement_UserPick  // CModelConfigElement
    {
        public const uint m_Choices = 0x48; // CUtlVector<CUtlString>
    }

    public static class CModelConfigList 
    {
        public const uint m_bHideMaterialGroupInTools = 0x0; // bool
        public const uint m_bHideRenderColorInTools = 0x1; // bool
        public const uint m_Configs = 0x8; // CUtlVector<CModelConfig*>
    }

    public static class CMoodVData 
    {
        public const uint m_sModelName = 0x0; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_nMoodType = 0xE0; // MoodType_t
        public const uint m_animationLayers = 0xE8; // CUtlVector<MoodAnimationLayer_t>
    }

    public static class CMorphBundleData 
    {
        public const uint m_flULeftSrc = 0x0; // float
        public const uint m_flVTopSrc = 0x4; // float
        public const uint m_offsets = 0x8; // CUtlVector<float>
        public const uint m_ranges = 0x20; // CUtlVector<float>
    }

    public static class CMorphConstraint  // CBaseConstraint
    {
        public const uint m_sTargetMorph = 0x70; // CUtlString
        public const uint m_nSlaveChannel = 0x78; // int32_t
        public const uint m_flMin = 0x7C; // float
        public const uint m_flMax = 0x80; // float
    }

    public static class CMorphData 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_morphRectDatas = 0x8; // CUtlVector<CMorphRectData>
    }

    public static class CMorphRectData 
    {
        public const uint m_nXLeftDst = 0x0; // int16_t
        public const uint m_nYTopDst = 0x2; // int16_t
        public const uint m_flUWidthSrc = 0x4; // float
        public const uint m_flVHeightSrc = 0x8; // float
        public const uint m_bundleDatas = 0x10; // CUtlVector<CMorphBundleData>
    }

    public static class CMorphSetData 
    {
        public const uint m_nWidth = 0x10; // int32_t
        public const uint m_nHeight = 0x14; // int32_t
        public const uint m_bundleTypes = 0x18; // CUtlVector<MorphBundleType_t>
        public const uint m_morphDatas = 0x30; // CUtlVector<CMorphData>
        public const uint m_pTextureAtlas = 0x48; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_FlexDesc = 0x50; // CUtlVector<CFlexDesc>
        public const uint m_FlexControllers = 0x68; // CUtlVector<CFlexController>
        public const uint m_FlexRules = 0x80; // CUtlVector<CFlexRule>
    }

    public static class CMotionDataSet 
    {
        public const uint m_groups = 0x0; // CUtlVector<CMotionGraphGroup>
        public const uint m_nDimensionCount = 0x18; // int32_t
    }

    public static class CMotionGraph 
    {
        public const uint m_paramSpans = 0x10; // CParamSpanUpdater
        public const uint m_tags = 0x28; // CUtlVector<TagSpan_t>
        public const uint m_pRootNode = 0x40; // CSmartPtr<CMotionNode>
        public const uint m_nParameterCount = 0x48; // int32_t
        public const uint m_nConfigStartIndex = 0x4C; // int32_t
        public const uint m_nConfigCount = 0x50; // int32_t
        public const uint m_bLoop = 0x54; // bool
    }

    public static class CMotionGraphConfig 
    {
        public const uint m_paramValues = 0x0; // float[4]
        public const uint m_flDuration = 0x10; // float
        public const uint m_nMotionIndex = 0x14; // MotionIndex
        public const uint m_nSampleStart = 0x18; // int32_t
        public const uint m_nSampleCount = 0x1C; // int32_t
    }

    public static class CMotionGraphGroup 
    {
        public const uint m_searchDB = 0x0; // CMotionSearchDB
        public const uint m_motionGraphs = 0xB8; // CUtlVector<CSmartPtr<CMotionGraph>>
        public const uint m_motionGraphConfigs = 0xD0; // CUtlVector<CMotionGraphConfig>
        public const uint m_sampleToConfig = 0xE8; // CUtlVector<int32_t>
        public const uint m_hIsActiveScript = 0x100; // AnimScriptHandle
    }

    public static class CMotionGraphUpdateNode  // CLeafUpdateNode
    {
        public const uint m_pMotionGraph = 0x58; // CSmartPtr<CMotionGraph>
    }

    public static class CMotionMatchingUpdateNode  // CLeafUpdateNode
    {
        public const uint m_dataSet = 0x58; // CMotionDataSet
        public const uint m_metrics = 0x78; // CUtlVector<CSmartPtr<CMotionMetricEvaluator>>
        public const uint m_weights = 0x90; // CUtlVector<float>
        public const uint m_bSearchEveryTick = 0xE0; // bool
        public const uint m_flSearchInterval = 0xE4; // float
        public const uint m_bSearchWhenClipEnds = 0xE8; // bool
        public const uint m_bSearchWhenGoalChanges = 0xE9; // bool
        public const uint m_blendCurve = 0xEC; // CBlendCurve
        public const uint m_flSampleRate = 0xF4; // float
        public const uint m_flBlendTime = 0xF8; // float
        public const uint m_bLockClipWhenWaning = 0xFC; // bool
        public const uint m_flSelectionThreshold = 0x100; // float
        public const uint m_flReselectionTimeWindow = 0x104; // float
        public const uint m_bEnableRotationCorrection = 0x108; // bool
        public const uint m_bGoalAssist = 0x109; // bool
        public const uint m_flGoalAssistDistance = 0x10C; // float
        public const uint m_flGoalAssistTolerance = 0x110; // float
        public const uint m_distanceScale_Damping = 0x118; // CAnimInputDamping
        public const uint m_flDistanceScale_OuterRadius = 0x128; // float
        public const uint m_flDistanceScale_InnerRadius = 0x12C; // float
        public const uint m_flDistanceScale_MaxScale = 0x130; // float
        public const uint m_flDistanceScale_MinScale = 0x134; // float
        public const uint m_bEnableDistanceScaling = 0x138; // bool
    }

    public static class CMotionMetricEvaluator 
    {
        public const uint m_means = 0x18; // CUtlVector<float>
        public const uint m_standardDeviations = 0x30; // CUtlVector<float>
        public const uint m_flWeight = 0x48; // float
        public const uint m_nDimensionStartIndex = 0x4C; // int32_t
    }

    public static class CMotionNode 
    {
        public const uint m_name = 0x18; // CUtlString
        public const uint m_id = 0x20; // AnimNodeID
    }

    public static class CMotionNodeBlend1D  // CMotionNode
    {
        public const uint m_blendItems = 0x28; // CUtlVector<MotionBlendItem>
        public const uint m_nParamIndex = 0x40; // int32_t
    }

    public static class CMotionNodeSequence  // CMotionNode
    {
        public const uint m_tags = 0x28; // CUtlVector<TagSpan_t>
        public const uint m_hSequence = 0x40; // HSequence
        public const uint m_flPlaybackSpeed = 0x44; // float
    }

    public static class CMotionSearchDB 
    {
        public const uint m_rootNode = 0x0; // CMotionSearchNode
        public const uint m_residualQuantizer = 0x80; // CProductQuantizer
        public const uint m_codeIndices = 0xA0; // CUtlVector<MotionDBIndex>
    }

    public static class CMotionSearchNode 
    {
        public const uint m_children = 0x0; // CUtlVector<CMotionSearchNode*>
        public const uint m_quantizer = 0x18; // CVectorQuantizer
        public const uint m_sampleCodes = 0x38; // CUtlVector<CUtlVector<SampleCode>>
        public const uint m_sampleIndices = 0x50; // CUtlVector<CUtlVector<int32_t>>
        public const uint m_selectableSamples = 0x68; // CUtlVector<int32_t>
    }

    public static class CMovementComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_movementModes = 0x30; // CUtlVector<CMovementMode>
        public const uint m_motors = 0x48; // CUtlVector<CSmartPtr<CAnimMotorUpdaterBase>>
        public const uint m_facingDamping = 0x60; // CAnimInputDamping
        public const uint m_eDefaultFacingMode = 0x70; // FacingMode
        public const uint m_nDefaultMotorIndex = 0x7C; // int32_t
        public const uint m_bMoveVarsDisabled = 0x80; // bool
        public const uint m_bNetworkPath = 0x81; // bool
        public const uint m_bNetworkFacing = 0x82; // bool
        public const uint m_paramHandles = 0x83; // CAnimParamHandle[30]
    }

    public static class CMovementMode 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_flSpeed = 0x8; // float
    }

    public static class CMoverUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_damping = 0x70; // CAnimInputDamping
        public const uint m_facingTarget = 0x80; // AnimValueSource
        public const uint m_hMoveVecParam = 0x84; // CAnimParamHandle
        public const uint m_hMoveHeadingParam = 0x86; // CAnimParamHandle
        public const uint m_hTurnToFaceParam = 0x88; // CAnimParamHandle
        public const uint m_flTurnToFaceOffset = 0x8C; // float
        public const uint m_flTurnToFaceLimit = 0x90; // float
        public const uint m_bAdditive = 0x94; // bool
        public const uint m_bApplyMovement = 0x95; // bool
        public const uint m_bOrientMovement = 0x96; // bool
        public const uint m_bApplyRotation = 0x97; // bool
        public const uint m_bLimitOnly = 0x98; // bool
    }

    public static class COrientConstraint  // CBaseConstraint
    {
    }

    public static class CParamSpanUpdater 
    {
        public const uint m_spans = 0x0; // CUtlVector<ParamSpan_t>
    }

    public static class CParentConstraint  // CBaseConstraint
    {
    }

    public static class CParticleAnimTag  // CAnimTagBase
    {
        public const uint m_hParticleSystem = 0x38; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_particleSystemName = 0x40; // CUtlString
        public const uint m_configName = 0x48; // CUtlString
        public const uint m_bDetachFromOwner = 0x50; // bool
        public const uint m_bStopWhenTagEnds = 0x51; // bool
        public const uint m_bTagEndStopIsInstant = 0x52; // bool
        public const uint m_attachmentName = 0x58; // CUtlString
        public const uint m_attachmentType = 0x60; // ParticleAttachment_t
        public const uint m_attachmentCP1Name = 0x68; // CUtlString
        public const uint m_attachmentCP1Type = 0x70; // ParticleAttachment_t
    }

    public static class CPathAnimMotorUpdater  // CPathAnimMotorUpdaterBase
    {
    }

    public static class CPathAnimMotorUpdaterBase  // CAnimMotorUpdaterBase
    {
        public const uint m_bLockToPath = 0x20; // bool
    }

    public static class CPathHelperUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_flStoppingRadius = 0x68; // float
        public const uint m_flStoppingSpeedScale = 0x6C; // float
    }

    public static class CPathMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_pathTimeSamples = 0x50; // CUtlVector<float>
        public const uint m_flDistance = 0x68; // float
        public const uint m_bExtrapolateMovement = 0x6C; // bool
        public const uint m_flMinExtrapolationSpeed = 0x70; // float
    }

    public static class CPhysSurfaceProperties 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_nameHash = 0x8; // uint32_t
        public const uint m_baseNameHash = 0xC; // uint32_t
        public const uint m_bHidden = 0x18; // bool
        public const uint m_description = 0x20; // CUtlString
        public const uint m_physics = 0x28; // CPhysSurfacePropertiesPhysics
        public const uint m_audioSounds = 0x48; // CPhysSurfacePropertiesSoundNames
        public const uint m_audioParams = 0x88; // CPhysSurfacePropertiesAudio
    }

    public static class CPhysSurfacePropertiesAudio 
    {
        public const uint m_reflectivity = 0x0; // float
        public const uint m_hardnessFactor = 0x4; // float
        public const uint m_roughnessFactor = 0x8; // float
        public const uint m_roughThreshold = 0xC; // float
        public const uint m_hardThreshold = 0x10; // float
        public const uint m_hardVelocityThreshold = 0x14; // float
        public const uint m_flStaticImpactVolume = 0x18; // float
        public const uint m_flOcclusionFactor = 0x1C; // float
    }

    public static class CPhysSurfacePropertiesPhysics 
    {
        public const uint m_friction = 0x0; // float
        public const uint m_elasticity = 0x4; // float
        public const uint m_density = 0x8; // float
        public const uint m_thickness = 0xC; // float
        public const uint m_softContactFrequency = 0x10; // float
        public const uint m_softContactDampingRatio = 0x14; // float
        public const uint m_wheelDrag = 0x18; // float
    }

    public static class CPhysSurfacePropertiesSoundNames 
    {
        public const uint m_impactSoft = 0x0; // CUtlString
        public const uint m_impactHard = 0x8; // CUtlString
        public const uint m_scrapeSmooth = 0x10; // CUtlString
        public const uint m_scrapeRough = 0x18; // CUtlString
        public const uint m_bulletImpact = 0x20; // CUtlString
        public const uint m_rolling = 0x28; // CUtlString
        public const uint m_break = 0x30; // CUtlString
        public const uint m_strain = 0x38; // CUtlString
    }

    public static class CPlayerInputAnimMotorUpdater  // CAnimMotorUpdaterBase
    {
        public const uint m_sampleTimes = 0x20; // CUtlVector<float>
        public const uint m_flSpringConstant = 0x3C; // float
        public const uint m_flAnticipationDistance = 0x40; // float
        public const uint m_hAnticipationPosParam = 0x44; // CAnimParamHandle
        public const uint m_hAnticipationHeadingParam = 0x46; // CAnimParamHandle
        public const uint m_bUseAcceleration = 0x48; // bool
    }

    public static class CPointConstraint  // CBaseConstraint
    {
    }

    public static class CPoseHandle 
    {
        public const uint m_nIndex = 0x0; // uint16_t
        public const uint m_eType = 0x2; // PoseType_t
    }

    public static class CProductQuantizer 
    {
        public const uint m_subQuantizers = 0x0; // CUtlVector<CVectorQuantizer>
        public const uint m_nDimensions = 0x18; // int32_t
    }

    public static class CQuaternionAnimParameter  // CConcreteAnimParameter
    {
        public const uint m_defaultValue = 0x60; // Quaternion
        public const uint m_bInterpolate = 0x70; // bool
    }

    public static class CRagdollAnimTag  // CAnimTagBase
    {
        public const uint m_nPoseControl = 0x38; // AnimPoseControl
        public const uint m_flFrequency = 0x3C; // float
        public const uint m_flDampingRatio = 0x40; // float
        public const uint m_flDecayDuration = 0x44; // float
        public const uint m_flDecayBias = 0x48; // float
        public const uint m_bDestroy = 0x4C; // bool
    }

    public static class CRagdollComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_ragdollNodePaths = 0x30; // CUtlVector<CAnimNodePath>
        public const uint m_boneIndices = 0x48; // CUtlVector<int32_t>
        public const uint m_boneNames = 0x60; // CUtlVector<CUtlString>
        public const uint m_weightLists = 0x78; // CUtlVector<WeightList>
        public const uint m_flSpringFrequencyMin = 0x90; // float
        public const uint m_flSpringFrequencyMax = 0x94; // float
        public const uint m_flMaxStretch = 0x98; // float
    }

    public static class CRagdollUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_nWeightListIndex = 0x68; // int32_t
        public const uint m_poseControlMethod = 0x6C; // RagdollPoseControl
    }

    public static class CRenderBufferBinding 
    {
        public const uint m_hBuffer = 0x0; // uint64_t
        public const uint m_nBindOffsetBytes = 0x10; // uint32_t
    }

    public static class CRenderMesh 
    {
        public const uint m_sceneObjects = 0x10; // CUtlVectorFixedGrowable<CSceneObjectData>
        public const uint m_constraints = 0xA0; // CUtlVector<CBaseConstraint*>
        public const uint m_skeleton = 0xB8; // CRenderSkeleton
    }

    public static class CRenderSkeleton 
    {
        public const uint m_bones = 0x0; // CUtlVector<RenderSkeletonBone_t>
        public const uint m_boneParents = 0x30; // CUtlVector<int32_t>
        public const uint m_nBoneWeightCount = 0x48; // int32_t
    }

    public static class CRootUpdateNode  // CUnaryUpdateNode
    {
    }

    public static class CSceneObjectData 
    {
        public const uint m_vMinBounds = 0x0; // Vector
        public const uint m_vMaxBounds = 0xC; // Vector
        public const uint m_drawCalls = 0x18; // CUtlVector<CMaterialDrawDescriptor>
        public const uint m_drawBounds = 0x30; // CUtlVector<AABB_t>
        public const uint m_meshlets = 0x48; // CUtlVector<CMeshletDescriptor>
        public const uint m_vTintColor = 0x60; // Vector4D
    }

    public static class CSelectorUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_children = 0x58; // CUtlVector<CAnimUpdateNodeRef>
        public const uint m_tags = 0x70; // CUtlVector<int8_t>
        public const uint m_blendCurve = 0x8C; // CBlendCurve
        public const uint m_flBlendTime = 0x94; // CAnimValue<float>
        public const uint m_hParameter = 0x9C; // CAnimParamHandle
        public const uint m_eTagBehavior = 0xA0; // SelectorTagBehavior_t
        public const uint m_bResetOnChange = 0xA4; // bool
        public const uint m_bSyncCyclesOnChange = 0xA5; // bool
    }

    public static class CSeqAutoLayer 
    {
        public const uint m_nLocalReference = 0x0; // int16_t
        public const uint m_nLocalPose = 0x2; // int16_t
        public const uint m_flags = 0x4; // CSeqAutoLayerFlag
        public const uint m_start = 0xC; // float
        public const uint m_peak = 0x10; // float
        public const uint m_tail = 0x14; // float
        public const uint m_end = 0x18; // float
    }

    public static class CSeqAutoLayerFlag 
    {
        public const uint m_bPost = 0x0; // bool
        public const uint m_bSpline = 0x1; // bool
        public const uint m_bXFade = 0x2; // bool
        public const uint m_bNoBlend = 0x3; // bool
        public const uint m_bLocal = 0x4; // bool
        public const uint m_bPose = 0x5; // bool
        public const uint m_bFetchFrame = 0x6; // bool
        public const uint m_bSubtract = 0x7; // bool
    }

    public static class CSeqBoneMaskList 
    {
        public const uint m_sName = 0x0; // CBufferString
        public const uint m_nLocalBoneArray = 0x10; // CUtlVector<int16_t>
        public const uint m_flBoneWeightArray = 0x28; // CUtlVector<float>
        public const uint m_flDefaultMorphCtrlWeight = 0x40; // float
        public const uint m_morphCtrlWeightArray = 0x48; // CUtlVector<CUtlPair<CBufferString,float>>
    }

    public static class CSeqCmdLayer 
    {
        public const uint m_cmd = 0x0; // int16_t
        public const uint m_nLocalReference = 0x2; // int16_t
        public const uint m_nLocalBonemask = 0x4; // int16_t
        public const uint m_nDstResult = 0x6; // int16_t
        public const uint m_nSrcResult = 0x8; // int16_t
        public const uint m_bSpline = 0xA; // bool
        public const uint m_flVar1 = 0xC; // float
        public const uint m_flVar2 = 0x10; // float
        public const uint m_nLineNumber = 0x14; // int16_t
    }

    public static class CSeqCmdSeqDesc 
    {
        public const uint m_sName = 0x0; // CBufferString
        public const uint m_flags = 0x10; // CSeqSeqDescFlag
        public const uint m_transition = 0x1C; // CSeqTransition
        public const uint m_nFrameRangeSequence = 0x24; // int16_t
        public const uint m_nFrameCount = 0x26; // int16_t
        public const uint m_flFPS = 0x28; // float
        public const uint m_nSubCycles = 0x2C; // int16_t
        public const uint m_numLocalResults = 0x2E; // int16_t
        public const uint m_cmdLayerArray = 0x30; // CUtlVector<CSeqCmdLayer>
        public const uint m_eventArray = 0x48; // CUtlVector<CAnimEventDefinition>
        public const uint m_activityArray = 0x60; // CUtlVector<CAnimActivity>
        public const uint m_poseSettingArray = 0x78; // CUtlVector<CSeqPoseSetting>
    }

    public static class CSeqIKLock 
    {
        public const uint m_flPosWeight = 0x0; // float
        public const uint m_flAngleWeight = 0x4; // float
        public const uint m_nLocalBone = 0x8; // int16_t
        public const uint m_bBonesOrientedAlongPositiveX = 0xA; // bool
    }

    public static class CSeqMultiFetch 
    {
        public const uint m_flags = 0x0; // CSeqMultiFetchFlag
        public const uint m_localReferenceArray = 0x8; // CUtlVector<int16_t>
        public const uint m_nGroupSize = 0x20; // int32_t[2]
        public const uint m_nLocalPose = 0x28; // int32_t[2]
        public const uint m_poseKeyArray0 = 0x30; // CUtlVector<float>
        public const uint m_poseKeyArray1 = 0x48; // CUtlVector<float>
        public const uint m_nLocalCyclePoseParameter = 0x60; // int32_t
        public const uint m_bCalculatePoseParameters = 0x64; // bool
    }

    public static class CSeqMultiFetchFlag 
    {
        public const uint m_bRealtime = 0x0; // bool
        public const uint m_bCylepose = 0x1; // bool
        public const uint m_b0D = 0x2; // bool
        public const uint m_b1D = 0x3; // bool
        public const uint m_b2D = 0x4; // bool
        public const uint m_b2D_TRI = 0x5; // bool
    }

    public static class CSeqPoseParamDesc 
    {
        public const uint m_sName = 0x0; // CBufferString
        public const uint m_flStart = 0x10; // float
        public const uint m_flEnd = 0x14; // float
        public const uint m_flLoop = 0x18; // float
        public const uint m_bLooping = 0x1C; // bool
    }

    public static class CSeqPoseSetting 
    {
        public const uint m_sPoseParameter = 0x0; // CBufferString
        public const uint m_sAttachment = 0x10; // CBufferString
        public const uint m_sReferenceSequence = 0x20; // CBufferString
        public const uint m_flValue = 0x30; // float
        public const uint m_bX = 0x34; // bool
        public const uint m_bY = 0x35; // bool
        public const uint m_bZ = 0x36; // bool
        public const uint m_eType = 0x38; // int32_t
    }

    public static class CSeqS1SeqDesc 
    {
        public const uint m_sName = 0x0; // CBufferString
        public const uint m_flags = 0x10; // CSeqSeqDescFlag
        public const uint m_fetch = 0x20; // CSeqMultiFetch
        public const uint m_nLocalWeightlist = 0x88; // int32_t
        public const uint m_autoLayerArray = 0x90; // CUtlVector<CSeqAutoLayer>
        public const uint m_IKLockArray = 0xA8; // CUtlVector<CSeqIKLock>
        public const uint m_transition = 0xC0; // CSeqTransition
        public const uint m_SequenceKeys = 0xC8; // KeyValues3
        public const uint m_LegacyKeyValueText = 0xD8; // CBufferString
        public const uint m_activityArray = 0xE8; // CUtlVector<CAnimActivity>
        public const uint m_footMotion = 0x100; // CUtlVector<CFootMotion>
    }

    public static class CSeqScaleSet 
    {
        public const uint m_sName = 0x0; // CBufferString
        public const uint m_bRootOffset = 0x10; // bool
        public const uint m_vRootOffset = 0x14; // Vector
        public const uint m_nLocalBoneArray = 0x20; // CUtlVector<int16_t>
        public const uint m_flBoneScaleArray = 0x38; // CUtlVector<float>
    }

    public static class CSeqSeqDescFlag 
    {
        public const uint m_bLooping = 0x0; // bool
        public const uint m_bSnap = 0x1; // bool
        public const uint m_bAutoplay = 0x2; // bool
        public const uint m_bPost = 0x3; // bool
        public const uint m_bHidden = 0x4; // bool
        public const uint m_bMulti = 0x5; // bool
        public const uint m_bLegacyDelta = 0x6; // bool
        public const uint m_bLegacyWorldspace = 0x7; // bool
        public const uint m_bLegacyCyclepose = 0x8; // bool
        public const uint m_bLegacyRealtime = 0x9; // bool
        public const uint m_bModelDoc = 0xA; // bool
    }

    public static class CSeqSynthAnimDesc 
    {
        public const uint m_sName = 0x0; // CBufferString
        public const uint m_flags = 0x10; // CSeqSeqDescFlag
        public const uint m_transition = 0x1C; // CSeqTransition
        public const uint m_nLocalBaseReference = 0x24; // int16_t
        public const uint m_nLocalBoneMask = 0x26; // int16_t
        public const uint m_activityArray = 0x28; // CUtlVector<CAnimActivity>
    }

    public static class CSeqTransition 
    {
        public const uint m_flFadeInTime = 0x0; // float
        public const uint m_flFadeOutTime = 0x4; // float
    }

    public static class CSequenceFinishedAnimTag  // CAnimTagBase
    {
        public const uint m_sequenceName = 0x38; // CUtlString
    }

    public static class CSequenceGroupData 
    {
        public const uint m_sName = 0x10; // CBufferString
        public const uint m_nFlags = 0x20; // uint32_t
        public const uint m_localSequenceNameArray = 0x28; // CUtlVector<CBufferString>
        public const uint m_localS1SeqDescArray = 0x40; // CUtlVector<CSeqS1SeqDesc>
        public const uint m_localMultiSeqDescArray = 0x58; // CUtlVector<CSeqS1SeqDesc>
        public const uint m_localSynthAnimDescArray = 0x70; // CUtlVector<CSeqSynthAnimDesc>
        public const uint m_localCmdSeqDescArray = 0x88; // CUtlVector<CSeqCmdSeqDesc>
        public const uint m_localBoneMaskArray = 0xA0; // CUtlVector<CSeqBoneMaskList>
        public const uint m_localScaleSetArray = 0xB8; // CUtlVector<CSeqScaleSet>
        public const uint m_localBoneNameArray = 0xD0; // CUtlVector<CBufferString>
        public const uint m_localNodeName = 0xE8; // CBufferString
        public const uint m_localPoseParamArray = 0xF8; // CUtlVector<CSeqPoseParamDesc>
        public const uint m_keyValues = 0x110; // KeyValues3
        public const uint m_localIKAutoplayLockArray = 0x120; // CUtlVector<CSeqIKLock>
    }

    public static class CSequenceUpdateNode  // CLeafUpdateNode
    {
        public const uint m_paramSpans = 0x60; // CParamSpanUpdater
        public const uint m_tags = 0x78; // CUtlVector<TagSpan_t>
        public const uint m_hSequence = 0x94; // HSequence
        public const uint m_playbackSpeed = 0x98; // float
        public const uint m_duration = 0x9C; // float
        public const uint m_bLoop = 0xA0; // bool
    }

    public static class CSetFacingUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_facingMode = 0x68; // FacingMode
        public const uint m_bResetChild = 0x6C; // bool
    }

    public static class CSetParameterActionUpdater  // CAnimActionUpdater
    {
        public const uint m_hParam = 0x18; // CAnimParamHandle
        public const uint m_value = 0x1A; // CAnimVariant
    }

    public static class CSingleFrameUpdateNode  // CLeafUpdateNode
    {
        public const uint m_actions = 0x58; // CUtlVector<CSmartPtr<CAnimActionUpdater>>
        public const uint m_hPoseCacheHandle = 0x70; // CPoseHandle
        public const uint m_hSequence = 0x74; // HSequence
        public const uint m_flCycle = 0x78; // float
    }

    public static class CSkeletalInputUpdateNode  // CLeafUpdateNode
    {
        public const uint m_fixedOpData = 0x58; // SkeletalInputOpFixedSettings_t
    }

    public static class CSlopeComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_flTraceDistance = 0x34; // float
        public const uint m_hSlopeAngle = 0x38; // CAnimParamHandle
        public const uint m_hSlopeAngleFront = 0x3A; // CAnimParamHandle
        public const uint m_hSlopeAngleSide = 0x3C; // CAnimParamHandle
        public const uint m_hSlopeHeading = 0x3E; // CAnimParamHandle
        public const uint m_hSlopeNormal = 0x40; // CAnimParamHandle
        public const uint m_hSlopeNormal_WorldSpace = 0x42; // CAnimParamHandle
    }

    public static class CSlowDownOnSlopesUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_flSlowDownStrength = 0x68; // float
    }

    public static class CSolveIKChainUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_targetHandles = 0x68; // CUtlVector<CSolveIKTargetHandle_t>
        public const uint m_opFixedData = 0x80; // SolveIKChainPoseOpFixedSettings_t
    }

    public static class CSolveIKTargetHandle_t 
    {
        public const uint m_positionHandle = 0x0; // CAnimParamHandle
        public const uint m_orientationHandle = 0x2; // CAnimParamHandle
    }

    public static class CSpeedScaleUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_paramIndex = 0x68; // CAnimParamHandle
    }

    public static class CStanceOverrideUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_footStanceInfo = 0x68; // CUtlVector<StanceInfo_t>
        public const uint m_pStanceSourceNode = 0x80; // CAnimUpdateNodeRef
        public const uint m_hParameter = 0x90; // CAnimParamHandle
        public const uint m_eMode = 0x94; // StanceOverrideMode
    }

    public static class CStanceScaleUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_hParam = 0x68; // CAnimParamHandle
    }

    public static class CStateActionUpdater 
    {
        public const uint m_pAction = 0x0; // CSmartPtr<CAnimActionUpdater>
        public const uint m_eBehavior = 0x8; // StateActionBehavior
    }

    public static class CStateMachineComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_stateMachine = 0x30; // CAnimStateMachineUpdater
    }

    public static class CStateMachineUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_stateMachine = 0x68; // CAnimStateMachineUpdater
        public const uint m_stateData = 0xC0; // CUtlVector<CStateNodeStateData>
        public const uint m_transitionData = 0xD8; // CUtlVector<CStateNodeTransitionData>
        public const uint m_bBlockWaningTags = 0xF4; // bool
        public const uint m_bLockStateWhenWaning = 0xF5; // bool
    }

    public static class CStateNodeStateData 
    {
        public const uint m_pChild = 0x0; // CAnimUpdateNodeRef
        public const uint m_bExclusiveRootMotion = 0x0; // bitfield:1
    }

    public static class CStateNodeTransitionData 
    {
        public const uint m_curve = 0x0; // CBlendCurve
        public const uint m_blendDuration = 0x8; // CAnimValue<float>
        public const uint m_resetCycleValue = 0x10; // CAnimValue<float>
        public const uint m_bReset = 0x0; // bitfield:1
        public const uint m_resetCycleOption = 0x0; // bitfield:3
    }

    public static class CStateUpdateData 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_hScript = 0x8; // AnimScriptHandle
        public const uint m_transitionIndices = 0x10; // CUtlVector<int32_t>
        public const uint m_actions = 0x28; // CUtlVector<CStateActionUpdater>
        public const uint m_stateID = 0x40; // AnimStateID
        public const uint m_bIsStartState = 0x0; // bitfield:1
        public const uint m_bIsEndState = 0x0; // bitfield:1
        public const uint m_bIsPassthrough = 0x0; // bitfield:1
    }

    public static class CStaticPoseCache 
    {
        public const uint m_poses = 0x10; // CUtlVector<CCachedPose>
        public const uint m_nBoneCount = 0x28; // int32_t
        public const uint m_nMorphCount = 0x2C; // int32_t
    }

    public static class CStaticPoseCacheBuilder  // CStaticPoseCache
    {
    }

    public static class CStepsRemainingMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_footIndices = 0x50; // CUtlVector<int32_t>
        public const uint m_flMinStepsRemaining = 0x68; // float
    }

    public static class CStopAtGoalUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_flOuterRadius = 0x6C; // float
        public const uint m_flInnerRadius = 0x70; // float
        public const uint m_flMaxScale = 0x74; // float
        public const uint m_flMinScale = 0x78; // float
        public const uint m_damping = 0x80; // CAnimInputDamping
    }

    public static class CStringAnimTag  // CAnimTagBase
    {
    }

    public static class CSubtractUpdateNode  // CBinaryUpdateNode
    {
        public const uint m_footMotionTiming = 0x8C; // BinaryNodeChildOption
        public const uint m_bApplyToFootMotion = 0x90; // bool
        public const uint m_bApplyChannelsSeparately = 0x91; // bool
        public const uint m_bUseModelSpace = 0x92; // bool
    }

    public static class CTaskStatusAnimTag  // CAnimTagBase
    {
    }

    public static class CTiltTwistConstraint  // CBaseConstraint
    {
        public const uint m_nTargetAxis = 0x70; // int32_t
        public const uint m_nSlaveAxis = 0x74; // int32_t
    }

    public static class CTimeRemainingMetricEvaluator  // CMotionMetricEvaluator
    {
        public const uint m_bMatchByTimeRemaining = 0x50; // bool
        public const uint m_flMaxTimeRemaining = 0x54; // float
        public const uint m_bFilterByTimeRemaining = 0x58; // bool
        public const uint m_flMinTimeRemaining = 0x5C; // float
    }

    public static class CToggleComponentActionUpdater  // CAnimActionUpdater
    {
        public const uint m_componentID = 0x18; // AnimComponentID
        public const uint m_bSetEnabled = 0x1C; // bool
    }

    public static class CTransitionUpdateData 
    {
        public const uint m_srcStateIndex = 0x0; // uint8_t
        public const uint m_destStateIndex = 0x1; // uint8_t
        public const uint m_bDisabled = 0x0; // bitfield:1
    }

    public static class CTurnHelperUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_facingTarget = 0x6C; // AnimValueSource
        public const uint m_turnStartTimeOffset = 0x70; // float
        public const uint m_turnDuration = 0x74; // float
        public const uint m_bMatchChildDuration = 0x78; // bool
        public const uint m_manualTurnOffset = 0x7C; // float
        public const uint m_bUseManualTurnOffset = 0x80; // bool
    }

    public static class CTwistConstraint  // CBaseConstraint
    {
        public const uint m_bInverse = 0x70; // bool
        public const uint m_qParentBindRotation = 0x80; // Quaternion
        public const uint m_qChildBindRotation = 0x90; // Quaternion
    }

    public static class CTwoBoneIKUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_opFixedData = 0x70; // TwoBoneIKSettings_t
    }

    public static class CUnaryUpdateNode  // CAnimUpdateNodeBase
    {
        public const uint m_pChildNode = 0x58; // CAnimUpdateNodeRef
    }

    public static class CVPhysXSurfacePropertiesList 
    {
        public const uint m_surfacePropertiesList = 0x0; // CUtlVector<CPhysSurfaceProperties*>
    }

    public static class CVRInputComponentUpdater  // CAnimComponentUpdater
    {
        public const uint m_FingerCurl_Thumb = 0x34; // CAnimParamHandle
        public const uint m_FingerCurl_Index = 0x36; // CAnimParamHandle
        public const uint m_FingerCurl_Middle = 0x38; // CAnimParamHandle
        public const uint m_FingerCurl_Ring = 0x3A; // CAnimParamHandle
        public const uint m_FingerCurl_Pinky = 0x3C; // CAnimParamHandle
        public const uint m_FingerSplay_Thumb_Index = 0x3E; // CAnimParamHandle
        public const uint m_FingerSplay_Index_Middle = 0x40; // CAnimParamHandle
        public const uint m_FingerSplay_Middle_Ring = 0x42; // CAnimParamHandle
        public const uint m_FingerSplay_Ring_Pinky = 0x44; // CAnimParamHandle
    }

    public static class CVectorAnimParameter  // CConcreteAnimParameter
    {
        public const uint m_defaultValue = 0x60; // Vector
        public const uint m_bInterpolate = 0x6C; // bool
    }

    public static class CVectorQuantizer 
    {
        public const uint m_centroidVectors = 0x0; // CUtlVector<float>
        public const uint m_nCentroids = 0x18; // int32_t
        public const uint m_nDimensions = 0x1C; // int32_t
    }

    public static class CVirtualAnimParameter  // CAnimParameterBase
    {
        public const uint m_expressionString = 0x50; // CUtlString
        public const uint m_eParamType = 0x58; // AnimParamType_t
    }

    public static class CVrSkeletalInputSettings 
    {
        public const uint m_wristBones = 0x0; // CUtlVector<CWristBone>
        public const uint m_fingers = 0x18; // CUtlVector<CFingerChain>
        public const uint m_name = 0x30; // CUtlString
        public const uint m_outerKnuckle1 = 0x38; // CUtlString
        public const uint m_outerKnuckle2 = 0x40; // CUtlString
        public const uint m_eHand = 0x48; // AnimVRHand_t
    }

    public static class CWayPointHelperUpdateNode  // CUnaryUpdateNode
    {
        public const uint m_flStartCycle = 0x6C; // float
        public const uint m_flEndCycle = 0x70; // float
        public const uint m_bOnlyGoals = 0x74; // bool
        public const uint m_bPreventOvershoot = 0x75; // bool
        public const uint m_bPreventUndershoot = 0x76; // bool
    }

    public static class CWristBone 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_vForwardLS = 0x8; // Vector
        public const uint m_vUpLS = 0x14; // Vector
        public const uint m_vOffset = 0x20; // Vector
    }

    public static class CZeroPoseUpdateNode  // CLeafUpdateNode
    {
    }

    public static class ChainToSolveData_t 
    {
        public const uint m_nChainIndex = 0x0; // int32_t
        public const uint m_SolverSettings = 0x4; // IKSolverSettings_t
        public const uint m_TargetSettings = 0x10; // IKTargetSettings_t
        public const uint m_DebugSetting = 0x38; // SolveIKChainAnimNodeDebugSetting
        public const uint m_flDebugNormalizedValue = 0x3C; // float
        public const uint m_vDebugOffset = 0x40; // VectorAligned
    }

    public static class ConfigIndex 
    {
        public const uint m_nGroup = 0x0; // uint16_t
        public const uint m_nConfig = 0x2; // uint16_t
    }

    public static class FingerBone_t 
    {
        public const uint m_boneIndex = 0x0; // int32_t
        public const uint m_hingeAxis = 0x4; // Vector
        public const uint m_vCapsulePos1 = 0x10; // Vector
        public const uint m_vCapsulePos2 = 0x1C; // Vector
        public const uint m_flMinAngle = 0x28; // float
        public const uint m_flMaxAngle = 0x2C; // float
        public const uint m_flRadius = 0x30; // float
    }

    public static class FingerChain_t 
    {
        public const uint m_targets = 0x0; // CUtlVector<FingerSource_t>
        public const uint m_bones = 0x18; // CUtlVector<FingerBone_t>
        public const uint m_vTipOffset = 0x30; // Vector
        public const uint m_vSplayHingeAxis = 0x3C; // Vector
        public const uint m_tipParentBoneIndex = 0x48; // int32_t
        public const uint m_metacarpalBoneIndex = 0x4C; // int32_t
        public const uint m_flSplayMinAngle = 0x50; // float
        public const uint m_flSplayMaxAngle = 0x54; // float
        public const uint m_flFingerScaleRatio = 0x58; // float
    }

    public static class FingerSource_t 
    {
        public const uint m_nFingerIndex = 0x0; // AnimVRFinger_t
        public const uint m_flFingerWeight = 0x4; // float
    }

    public static class FollowAttachmentSettings_t 
    {
        public const uint m_attachment = 0x0; // CAnimAttachment
        public const uint m_boneIndex = 0x80; // int32_t
        public const uint m_bMatchTranslation = 0x84; // bool
        public const uint m_bMatchRotation = 0x85; // bool
    }

    public static class FootFixedData_t 
    {
        public const uint m_vToeOffset = 0x0; // VectorAligned
        public const uint m_vHeelOffset = 0x10; // VectorAligned
        public const uint m_nTargetBoneIndex = 0x20; // int32_t
        public const uint m_nAnkleBoneIndex = 0x24; // int32_t
        public const uint m_nIKAnchorBoneIndex = 0x28; // int32_t
        public const uint m_ikChainIndex = 0x2C; // int32_t
        public const uint m_flMaxIKLength = 0x30; // float
        public const uint m_nFootIndex = 0x34; // int32_t
        public const uint m_nTagIndex = 0x38; // int32_t
        public const uint m_flMaxRotationLeft = 0x3C; // float
        public const uint m_flMaxRotationRight = 0x40; // float
    }

    public static class FootFixedSettings 
    {
        public const uint m_traceSettings = 0x0; // TraceSettings_t
        public const uint m_vFootBaseBindPosePositionMS = 0x10; // VectorAligned
        public const uint m_flFootBaseLength = 0x20; // float
        public const uint m_flMaxRotationLeft = 0x24; // float
        public const uint m_flMaxRotationRight = 0x28; // float
        public const uint m_footstepLandedTagIndex = 0x2C; // int32_t
        public const uint m_bEnableTracing = 0x30; // bool
        public const uint m_flTraceAngleBlend = 0x34; // float
        public const uint m_nDisableTagIndex = 0x38; // int32_t
        public const uint m_nFootIndex = 0x3C; // int32_t
    }

    public static class FootLockPoseOpFixedSettings 
    {
        public const uint m_footInfo = 0x0; // CUtlVector<FootFixedData_t>
        public const uint m_hipDampingSettings = 0x18; // CAnimInputDamping
        public const uint m_nHipBoneIndex = 0x28; // int32_t
        public const uint m_ikSolverType = 0x2C; // IKSolverType
        public const uint m_bApplyTilt = 0x30; // bool
        public const uint m_bApplyHipDrop = 0x31; // bool
        public const uint m_bAlwaysUseFallbackHinge = 0x32; // bool
        public const uint m_bApplyFootRotationLimits = 0x33; // bool
        public const uint m_bApplyLegTwistLimits = 0x34; // bool
        public const uint m_flMaxFootHeight = 0x38; // float
        public const uint m_flExtensionScale = 0x3C; // float
        public const uint m_flMaxLegTwist = 0x40; // float
        public const uint m_bEnableLockBreaking = 0x44; // bool
        public const uint m_flLockBreakTolerance = 0x48; // float
        public const uint m_flLockBlendTime = 0x4C; // float
        public const uint m_bEnableStretching = 0x50; // bool
        public const uint m_flMaxStretchAmount = 0x54; // float
        public const uint m_flStretchExtensionScale = 0x58; // float
    }

    public static class FootPinningPoseOpFixedData_t 
    {
        public const uint m_footInfo = 0x0; // CUtlVector<FootFixedData_t>
        public const uint m_flBlendTime = 0x18; // float
        public const uint m_flLockBreakDistance = 0x1C; // float
        public const uint m_flMaxLegTwist = 0x20; // float
        public const uint m_nHipBoneIndex = 0x24; // int32_t
        public const uint m_bApplyLegTwistLimits = 0x28; // bool
        public const uint m_bApplyFootRotationLimits = 0x29; // bool
    }

    public static class FootStepTrigger 
    {
        public const uint m_tags = 0x0; // CUtlVector<int32_t>
        public const uint m_nFootIndex = 0x18; // int32_t
        public const uint m_triggerPhase = 0x1C; // StepPhase
    }

    public static class HSequence 
    {
        public const uint m_Value = 0x0; // int32_t
    }

    public static class HitReactFixedSettings_t 
    {
        public const uint m_nWeightListIndex = 0x0; // int32_t
        public const uint m_nEffectedBoneCount = 0x4; // int32_t
        public const uint m_flMaxImpactForce = 0x8; // float
        public const uint m_flMinImpactForce = 0xC; // float
        public const uint m_flWhipImpactScale = 0x10; // float
        public const uint m_flCounterRotationScale = 0x14; // float
        public const uint m_flDistanceFadeScale = 0x18; // float
        public const uint m_flPropagationScale = 0x1C; // float
        public const uint m_flWhipDelay = 0x20; // float
        public const uint m_flSpringStrength = 0x24; // float
        public const uint m_flWhipSpringStrength = 0x28; // float
        public const uint m_flMaxAngleRadians = 0x2C; // float
        public const uint m_nHipBoneIndex = 0x30; // int32_t
        public const uint m_flHipBoneTranslationScale = 0x34; // float
        public const uint m_flHipDipSpringStrength = 0x38; // float
        public const uint m_flHipDipImpactScale = 0x3C; // float
        public const uint m_flHipDipDelay = 0x40; // float
    }

    public static class IKBoneNameAndIndex_t 
    {
        public const uint m_Name = 0x0; // CUtlString
    }

    public static class IKDemoCaptureSettings_t 
    {
        public const uint m_parentBoneName = 0x0; // CUtlString
        public const uint m_eMode = 0x8; // IKChannelMode
        public const uint m_ikChainName = 0x10; // CUtlString
        public const uint m_oneBoneStart = 0x18; // CUtlString
        public const uint m_oneBoneEnd = 0x20; // CUtlString
    }

    public static class IKSolverSettings_t 
    {
        public const uint m_SolverType = 0x0; // IKSolverType
        public const uint m_nNumIterations = 0x4; // int32_t
    }

    public static class IKTargetSettings_t 
    {
        public const uint m_TargetSource = 0x0; // IKTargetSource
        public const uint m_Bone = 0x8; // IKBoneNameAndIndex_t
        public const uint m_AnimgraphParameterNamePosition = 0x18; // AnimParamID
        public const uint m_AnimgraphParameterNameOrientation = 0x1C; // AnimParamID
        public const uint m_TargetCoordSystem = 0x20; // IKTargetCoordinateSystem
    }

    public static class JiggleBoneSettingsList_t 
    {
        public const uint m_boneSettings = 0x0; // CUtlVector<JiggleBoneSettings_t>
    }

    public static class JiggleBoneSettings_t 
    {
        public const uint m_nBoneIndex = 0x0; // int32_t
        public const uint m_flSpringStrength = 0x4; // float
        public const uint m_flMaxTimeStep = 0x8; // float
        public const uint m_flDamping = 0xC; // float
        public const uint m_vBoundsMaxLS = 0x10; // Vector
        public const uint m_vBoundsMinLS = 0x1C; // Vector
        public const uint m_eSimSpace = 0x28; // JiggleBoneSimSpace
    }

    public static class LookAtBone_t 
    {
        public const uint m_index = 0x0; // int32_t
        public const uint m_weight = 0x4; // float
    }

    public static class LookAtOpFixedSettings_t 
    {
        public const uint m_attachment = 0x0; // CAnimAttachment
        public const uint m_damping = 0x80; // CAnimInputDamping
        public const uint m_bones = 0x90; // CUtlVector<LookAtBone_t>
        public const uint m_flYawLimit = 0xA8; // float
        public const uint m_flPitchLimit = 0xAC; // float
        public const uint m_flHysteresisInnerAngle = 0xB0; // float
        public const uint m_flHysteresisOuterAngle = 0xB4; // float
        public const uint m_bRotateYawForward = 0xB8; // bool
        public const uint m_bMaintainUpDirection = 0xB9; // bool
        public const uint m_bTargetIsPosition = 0xBA; // bool
        public const uint m_bUseHysteresis = 0xBB; // bool
    }

    public static class MaterialGroup_t 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_materials = 0x8; // CUtlVector<CStrongHandle<InfoForResourceTypeIMaterial2>>
    }

    public static class ModelBoneFlexDriverControl_t 
    {
        public const uint m_nBoneComponent = 0x0; // ModelBoneFlexComponent_t
        public const uint m_flexController = 0x8; // CUtlString
        public const uint m_flexControllerToken = 0x10; // uint32_t
        public const uint m_flMin = 0x14; // float
        public const uint m_flMax = 0x18; // float
    }

    public static class ModelBoneFlexDriver_t 
    {
        public const uint m_boneName = 0x0; // CUtlString
        public const uint m_boneNameToken = 0x8; // uint32_t
        public const uint m_controls = 0x10; // CUtlVector<ModelBoneFlexDriverControl_t>
    }

    public static class ModelSkeletonData_t 
    {
        public const uint m_boneName = 0x0; // CUtlVector<CUtlString>
        public const uint m_nParent = 0x18; // CUtlVector<int16_t>
        public const uint m_boneSphere = 0x30; // CUtlVector<float>
        public const uint m_nFlag = 0x48; // CUtlVector<uint32_t>
        public const uint m_bonePosParent = 0x60; // CUtlVector<Vector>
        public const uint m_boneRotParent = 0x78; // CUtlVector<QuaternionStorage>
        public const uint m_boneScaleParent = 0x90; // CUtlVector<float>
    }

    public static class MoodAnimationLayer_t 
    {
        public const uint m_sName = 0x0; // CUtlString
        public const uint m_bActiveListening = 0x8; // bool
        public const uint m_bActiveTalking = 0x9; // bool
        public const uint m_layerAnimations = 0x10; // CUtlVector<MoodAnimation_t>
        public const uint m_flIntensity = 0x28; // CRangeFloat
        public const uint m_flDurationScale = 0x30; // CRangeFloat
        public const uint m_bScaleWithInts = 0x38; // bool
        public const uint m_flNextStart = 0x3C; // CRangeFloat
        public const uint m_flStartOffset = 0x44; // CRangeFloat
        public const uint m_flEndOffset = 0x4C; // CRangeFloat
        public const uint m_flFadeIn = 0x54; // float
        public const uint m_flFadeOut = 0x58; // float
    }

    public static class MoodAnimation_t 
    {
        public const uint m_sName = 0x0; // CUtlString
        public const uint m_flWeight = 0x8; // float
    }

    public static class MotionBlendItem 
    {
        public const uint m_pChild = 0x0; // CSmartPtr<CMotionNode>
        public const uint m_flKeyValue = 0x8; // float
    }

    public static class MotionDBIndex 
    {
        public const uint m_nIndex = 0x0; // uint32_t
    }

    public static class MotionIndex 
    {
        public const uint m_nGroup = 0x0; // uint16_t
        public const uint m_nMotion = 0x2; // uint16_t
    }

    public static class ParamSpanSample_t 
    {
        public const uint m_value = 0x0; // CAnimVariant
        public const uint m_flCycle = 0x14; // float
    }

    public static class ParamSpan_t 
    {
        public const uint m_samples = 0x0; // CUtlVector<ParamSpanSample_t>
        public const uint m_hParam = 0x18; // CAnimParamHandle
        public const uint m_eParamType = 0x1A; // AnimParamType_t
        public const uint m_flStartCycle = 0x1C; // float
        public const uint m_flEndCycle = 0x20; // float
    }

    public static class PermModelDataAnimatedMaterialAttribute_t 
    {
        public const uint m_AttributeName = 0x0; // CUtlString
        public const uint m_nNumChannels = 0x8; // int32_t
    }

    public static class PermModelData_t 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_modelInfo = 0x8; // PermModelInfo_t
        public const uint m_ExtParts = 0x60; // CUtlVector<PermModelExtPart_t>
        public const uint m_refMeshes = 0x78; // CUtlVector<CStrongHandle<InfoForResourceTypeCRenderMesh>>
        public const uint m_refMeshGroupMasks = 0x90; // CUtlVector<uint64_t>
        public const uint m_refPhysGroupMasks = 0xA8; // CUtlVector<uint64_t>
        public const uint m_refLODGroupMasks = 0xC0; // CUtlVector<uint8_t>
        public const uint m_lodGroupSwitchDistances = 0xD8; // CUtlVector<float>
        public const uint m_refPhysicsData = 0xF0; // CUtlVector<CStrongHandle<InfoForResourceTypeCPhysAggregateData>>
        public const uint m_refPhysicsHitboxData = 0x108; // CUtlVector<CStrongHandle<InfoForResourceTypeCPhysAggregateData>>
        public const uint m_refAnimGroups = 0x120; // CUtlVector<CStrongHandle<InfoForResourceTypeCAnimationGroup>>
        public const uint m_refSequenceGroups = 0x138; // CUtlVector<CStrongHandle<InfoForResourceTypeCSequenceGroupData>>
        public const uint m_meshGroups = 0x150; // CUtlVector<CUtlString>
        public const uint m_materialGroups = 0x168; // CUtlVector<MaterialGroup_t>
        public const uint m_nDefaultMeshGroupMask = 0x180; // uint64_t
        public const uint m_modelSkeleton = 0x188; // ModelSkeletonData_t
        public const uint m_remappingTable = 0x230; // CUtlVector<int16_t>
        public const uint m_remappingTableStarts = 0x248; // CUtlVector<uint16_t>
        public const uint m_boneFlexDrivers = 0x260; // CUtlVector<ModelBoneFlexDriver_t>
        public const uint m_pModelConfigList = 0x278; // CModelConfigList*
        public const uint m_BodyGroupsHiddenInTools = 0x280; // CUtlVector<CUtlString>
        public const uint m_refAnimIncludeModels = 0x298; // CUtlVector<CStrongHandle<InfoForResourceTypeCModel>>
        public const uint m_AnimatedMaterialAttributes = 0x2B0; // CUtlVector<PermModelDataAnimatedMaterialAttribute_t>
    }

    public static class PermModelExtPart_t 
    {
        public const uint m_Transform = 0x0; // CTransform
        public const uint m_Name = 0x20; // CUtlString
        public const uint m_nParent = 0x28; // int32_t
        public const uint m_refModel = 0x30; // CStrongHandle<InfoForResourceTypeCModel>
    }

    public static class PermModelInfo_t 
    {
        public const uint m_nFlags = 0x0; // uint32_t
        public const uint m_vHullMin = 0x4; // Vector
        public const uint m_vHullMax = 0x10; // Vector
        public const uint m_vViewMin = 0x1C; // Vector
        public const uint m_vViewMax = 0x28; // Vector
        public const uint m_flMass = 0x34; // float
        public const uint m_vEyePosition = 0x38; // Vector
        public const uint m_flMaxEyeDeflection = 0x44; // float
        public const uint m_sSurfaceProperty = 0x48; // CUtlString
        public const uint m_keyValueText = 0x50; // CUtlString
    }

    public static class PhysSoftbodyDesc_t 
    {
        public const uint m_ParticleBoneHash = 0x0; // CUtlVector<uint32_t>
        public const uint m_Particles = 0x18; // CUtlVector<RnSoftbodyParticle_t>
        public const uint m_Springs = 0x30; // CUtlVector<RnSoftbodySpring_t>
        public const uint m_Capsules = 0x48; // CUtlVector<RnSoftbodyCapsule_t>
        public const uint m_InitPose = 0x60; // CUtlVector<CTransform>
        public const uint m_ParticleBoneName = 0x78; // CUtlVector<CUtlString>
    }

    public static class RenderSkeletonBone_t 
    {
        public const uint m_boneName = 0x0; // CUtlString
        public const uint m_parentName = 0x8; // CUtlString
        public const uint m_invBindPose = 0x10; // matrix3x4_t
        public const uint m_bbox = 0x40; // SkeletonBoneBounds_t
        public const uint m_flSphereRadius = 0x58; // float
    }

    public static class SampleCode 
    {
        public const uint m_subCode = 0x0; // uint8_t[8]
    }

    public static class ScriptInfo_t 
    {
        public const uint m_code = 0x0; // CUtlString
        public const uint m_paramsModified = 0x8; // CUtlVector<CAnimParamHandle>
        public const uint m_proxyReadParams = 0x20; // CUtlVector<int32_t>
        public const uint m_proxyWriteParams = 0x38; // CUtlVector<int32_t>
        public const uint m_eScriptType = 0x50; // AnimScriptType
    }

    public static class SkeletalInputOpFixedSettings_t 
    {
        public const uint m_wristBones = 0x0; // CUtlVector<WristBone_t>
        public const uint m_fingers = 0x18; // CUtlVector<FingerChain_t>
        public const uint m_outerKnuckle1 = 0x30; // int32_t
        public const uint m_outerKnuckle2 = 0x34; // int32_t
        public const uint m_eHand = 0x38; // AnimVRHand_t
        public const uint m_eMotionRange = 0x3C; // AnimVRHandMotionRange_t
        public const uint m_eTransformSource = 0x40; // AnimVrBoneTransformSource_t
        public const uint m_bEnableIK = 0x44; // bool
        public const uint m_bEnableCollision = 0x45; // bool
    }

    public static class SkeletonBoneBounds_t 
    {
        public const uint m_vecCenter = 0x0; // Vector
        public const uint m_vecSize = 0xC; // Vector
    }

    public static class SolveIKChainPoseOpFixedSettings_t 
    {
        public const uint m_ChainsToSolveData = 0x0; // CUtlVector<ChainToSolveData_t>
        public const uint m_bMatchTargetOrientation = 0x18; // bool
    }

    public static class StanceInfo_t 
    {
        public const uint m_vPosition = 0x0; // Vector
        public const uint m_flDirection = 0xC; // float
    }

    public static class TagSpan_t 
    {
        public const uint m_tagIndex = 0x0; // int32_t
        public const uint m_startCycle = 0x4; // float
        public const uint m_endCycle = 0x8; // float
    }

    public static class TraceSettings_t 
    {
        public const uint m_flTraceHeight = 0x0; // float
        public const uint m_flTraceRadius = 0x4; // float
    }

    public static class TwoBoneIKSettings_t 
    {
        public const uint m_endEffectorType = 0x0; // IkEndEffectorType
        public const uint m_endEffectorAttachment = 0x10; // CAnimAttachment
        public const uint m_targetType = 0x90; // IkTargetType
        public const uint m_targetAttachment = 0xA0; // CAnimAttachment
        public const uint m_targetBoneIndex = 0x120; // int32_t
        public const uint m_hPositionParam = 0x124; // CAnimParamHandle
        public const uint m_hRotationParam = 0x126; // CAnimParamHandle
        public const uint m_bAlwaysUseFallbackHinge = 0x128; // bool
        public const uint m_vLsFallbackHingeAxis = 0x130; // VectorAligned
        public const uint m_nFixedBoneIndex = 0x140; // int32_t
        public const uint m_nMiddleBoneIndex = 0x144; // int32_t
        public const uint m_nEndBoneIndex = 0x148; // int32_t
        public const uint m_bMatchTargetOrientation = 0x14C; // bool
        public const uint m_bConstrainTwist = 0x14D; // bool
        public const uint m_flMaxTwist = 0x150; // float
    }

    public static class VPhysXAggregateData_t 
    {
        public const uint m_nFlags = 0x0; // uint16_t
        public const uint m_nRefCounter = 0x2; // uint16_t
        public const uint m_bonesHash = 0x8; // CUtlVector<uint32_t>
        public const uint m_boneNames = 0x20; // CUtlVector<CUtlString>
        public const uint m_indexNames = 0x38; // CUtlVector<uint16_t>
        public const uint m_indexHash = 0x50; // CUtlVector<uint16_t>
        public const uint m_bindPose = 0x68; // CUtlVector<matrix3x4a_t>
        public const uint m_parts = 0x80; // CUtlVector<VPhysXBodyPart_t>
        public const uint m_constraints2 = 0x98; // CUtlVector<VPhysXConstraint2_t>
        public const uint m_joints = 0xB0; // CUtlVector<VPhysXJoint_t>
        public const uint m_pFeModel = 0xC8; // PhysFeModelDesc_t*
        public const uint m_boneParents = 0xD0; // CUtlVector<uint16_t>
        public const uint m_surfacePropertyHashes = 0xE8; // CUtlVector<uint32_t>
        public const uint m_collisionAttributes = 0x100; // CUtlVector<VPhysXCollisionAttributes_t>
        public const uint m_debugPartNames = 0x118; // CUtlVector<CUtlString>
        public const uint m_embeddedKeyvalues = 0x130; // CUtlString
    }

    public static class VPhysXBodyPart_t 
    {
        public const uint m_nFlags = 0x0; // uint32_t
        public const uint m_flMass = 0x4; // float
        public const uint m_rnShape = 0x8; // VPhysics2ShapeDef_t
        public const uint m_nCollisionAttributeIndex = 0x80; // uint16_t
        public const uint m_nReserved = 0x82; // uint16_t
        public const uint m_flInertiaScale = 0x84; // float
        public const uint m_flLinearDamping = 0x88; // float
        public const uint m_flAngularDamping = 0x8C; // float
        public const uint m_bOverrideMassCenter = 0x90; // bool
        public const uint m_vMassCenterOverride = 0x94; // Vector
    }

    public static class VPhysXCollisionAttributes_t 
    {
        public const uint m_CollisionGroup = 0x0; // uint32_t
        public const uint m_InteractAs = 0x8; // CUtlVector<uint32_t>
        public const uint m_InteractWith = 0x20; // CUtlVector<uint32_t>
        public const uint m_InteractExclude = 0x38; // CUtlVector<uint32_t>
        public const uint m_CollisionGroupString = 0x50; // CUtlString
        public const uint m_InteractAsStrings = 0x58; // CUtlVector<CUtlString>
        public const uint m_InteractWithStrings = 0x70; // CUtlVector<CUtlString>
        public const uint m_InteractExcludeStrings = 0x88; // CUtlVector<CUtlString>
    }

    public static class VPhysXConstraint2_t 
    {
        public const uint m_nFlags = 0x0; // uint32_t
        public const uint m_nParent = 0x4; // uint16_t
        public const uint m_nChild = 0x6; // uint16_t
        public const uint m_params = 0x8; // VPhysXConstraintParams_t
    }

    public static class VPhysXConstraintParams_t 
    {
        public const uint m_nType = 0x0; // int8_t
        public const uint m_nTranslateMotion = 0x1; // int8_t
        public const uint m_nRotateMotion = 0x2; // int8_t
        public const uint m_nFlags = 0x3; // int8_t
        public const uint m_anchor = 0x4; // Vector[2]
        public const uint m_axes = 0x1C; // QuaternionStorage[2]
        public const uint m_maxForce = 0x3C; // float
        public const uint m_maxTorque = 0x40; // float
        public const uint m_linearLimitValue = 0x44; // float
        public const uint m_linearLimitRestitution = 0x48; // float
        public const uint m_linearLimitSpring = 0x4C; // float
        public const uint m_linearLimitDamping = 0x50; // float
        public const uint m_twistLowLimitValue = 0x54; // float
        public const uint m_twistLowLimitRestitution = 0x58; // float
        public const uint m_twistLowLimitSpring = 0x5C; // float
        public const uint m_twistLowLimitDamping = 0x60; // float
        public const uint m_twistHighLimitValue = 0x64; // float
        public const uint m_twistHighLimitRestitution = 0x68; // float
        public const uint m_twistHighLimitSpring = 0x6C; // float
        public const uint m_twistHighLimitDamping = 0x70; // float
        public const uint m_swing1LimitValue = 0x74; // float
        public const uint m_swing1LimitRestitution = 0x78; // float
        public const uint m_swing1LimitSpring = 0x7C; // float
        public const uint m_swing1LimitDamping = 0x80; // float
        public const uint m_swing2LimitValue = 0x84; // float
        public const uint m_swing2LimitRestitution = 0x88; // float
        public const uint m_swing2LimitSpring = 0x8C; // float
        public const uint m_swing2LimitDamping = 0x90; // float
        public const uint m_goalPosition = 0x94; // Vector
        public const uint m_goalOrientation = 0xA0; // QuaternionStorage
        public const uint m_goalAngularVelocity = 0xB0; // Vector
        public const uint m_driveSpringX = 0xBC; // float
        public const uint m_driveSpringY = 0xC0; // float
        public const uint m_driveSpringZ = 0xC4; // float
        public const uint m_driveDampingX = 0xC8; // float
        public const uint m_driveDampingY = 0xCC; // float
        public const uint m_driveDampingZ = 0xD0; // float
        public const uint m_driveSpringTwist = 0xD4; // float
        public const uint m_driveSpringSwing = 0xD8; // float
        public const uint m_driveSpringSlerp = 0xDC; // float
        public const uint m_driveDampingTwist = 0xE0; // float
        public const uint m_driveDampingSwing = 0xE4; // float
        public const uint m_driveDampingSlerp = 0xE8; // float
        public const uint m_solverIterationCount = 0xEC; // int32_t
        public const uint m_projectionLinearTolerance = 0xF0; // float
        public const uint m_projectionAngularTolerance = 0xF4; // float
    }

    public static class VPhysXJoint_t 
    {
        public const uint m_nType = 0x0; // uint16_t
        public const uint m_nBody1 = 0x2; // uint16_t
        public const uint m_nBody2 = 0x4; // uint16_t
        public const uint m_nFlags = 0x6; // uint16_t
        public const uint m_Frame1 = 0x10; // CTransform
        public const uint m_Frame2 = 0x30; // CTransform
        public const uint m_bEnableCollision = 0x50; // bool
        public const uint m_bEnableLinearLimit = 0x51; // bool
        public const uint m_LinearLimit = 0x54; // VPhysXRange_t
        public const uint m_bEnableLinearMotor = 0x5C; // bool
        public const uint m_vLinearTargetVelocity = 0x60; // Vector
        public const uint m_flMaxForce = 0x6C; // float
        public const uint m_bEnableSwingLimit = 0x70; // bool
        public const uint m_SwingLimit = 0x74; // VPhysXRange_t
        public const uint m_bEnableTwistLimit = 0x7C; // bool
        public const uint m_TwistLimit = 0x80; // VPhysXRange_t
        public const uint m_bEnableAngularMotor = 0x88; // bool
        public const uint m_vAngularTargetVelocity = 0x8C; // Vector
        public const uint m_flMaxTorque = 0x98; // float
        public const uint m_flLinearFrequency = 0x9C; // float
        public const uint m_flLinearDampingRatio = 0xA0; // float
        public const uint m_flAngularFrequency = 0xA4; // float
        public const uint m_flAngularDampingRatio = 0xA8; // float
        public const uint m_flFriction = 0xAC; // float
    }

    public static class VPhysXRange_t 
    {
        public const uint m_flMin = 0x0; // float
        public const uint m_flMax = 0x4; // float
    }

    public static class VPhysics2ShapeDef_t 
    {
        public const uint m_spheres = 0x0; // CUtlVector<RnSphereDesc_t>
        public const uint m_capsules = 0x18; // CUtlVector<RnCapsuleDesc_t>
        public const uint m_hulls = 0x30; // CUtlVector<RnHullDesc_t>
        public const uint m_meshes = 0x48; // CUtlVector<RnMeshDesc_t>
        public const uint m_CollisionAttributeIndices = 0x60; // CUtlVector<uint16_t>
    }

    public static class WeightList 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_weights = 0x8; // CUtlVector<float>
    }

    public static class WristBone_t 
    {
        public const uint m_xOffsetTransformMS = 0x0; // CTransform
        public const uint m_boneIndex = 0x20; // int32_t
    }
}
