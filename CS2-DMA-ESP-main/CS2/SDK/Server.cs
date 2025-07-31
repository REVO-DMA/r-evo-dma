namespace cs2_dma_esp.SDK.Server 
{
    public static class ActiveModelConfig_t 
    {
        public const uint m_Handle = 0x28; // ModelConfigHandle_t
        public const uint m_Name = 0x30; // CUtlSymbolLarge
        public const uint m_AssociatedEntities = 0x38; // CNetworkUtlVectorBase<CHandle<CBaseModelEntity>>
        public const uint m_AssociatedEntityNames = 0x50; // CNetworkUtlVectorBase<CUtlSymbolLarge>
    }

    public static class AmmoIndex_t 
    {
        public const uint m_Value = 0x0; // int8_t
    }

    public static class AmmoTypeInfo_t 
    {
        public const uint m_nMaxCarry = 0x10; // int32_t
        public const uint m_nSplashSize = 0x1C; // CRangeInt
        public const uint m_nFlags = 0x24; // AmmoFlags_t
        public const uint m_flMass = 0x28; // float
        public const uint m_flSpeed = 0x2C; // CRangeFloat
    }

    public static class AnimationUpdateListHandle_t 
    {
        public const uint m_Value = 0x0; // uint32_t
    }

    public static class CAISound  // CPointEntity
    {
        public const uint m_iSoundType = 0x4B0; // int32_t
        public const uint m_iSoundContext = 0x4B4; // int32_t
        public const uint m_iVolume = 0x4B8; // int32_t
        public const uint m_iSoundIndex = 0x4BC; // int32_t
        public const uint m_flDuration = 0x4C0; // float
        public const uint m_iszProxyEntityName = 0x4C8; // CUtlSymbolLarge
    }

    public static class CAI_ChangeHintGroup  // CBaseEntity
    {
        public const uint m_iSearchType = 0x4B0; // int32_t
        public const uint m_strSearchName = 0x4B8; // CUtlSymbolLarge
        public const uint m_strNewHintGroup = 0x4C0; // CUtlSymbolLarge
        public const uint m_flRadius = 0x4C8; // float
    }

    public static class CAI_ChangeTarget  // CBaseEntity
    {
        public const uint m_iszNewTarget = 0x4B0; // CUtlSymbolLarge
    }

    public static class CAI_Expresser 
    {
        public const uint m_flStopTalkTime = 0x38; // GameTime_t
        public const uint m_flStopTalkTimeWithoutDelay = 0x3C; // GameTime_t
        public const uint m_flBlockedTalkTime = 0x40; // GameTime_t
        public const uint m_voicePitch = 0x44; // int32_t
        public const uint m_flLastTimeAcceptedSpeak = 0x48; // GameTime_t
        public const uint m_bAllowSpeakingInterrupts = 0x4C; // bool
        public const uint m_bConsiderSceneInvolvementAsSpeech = 0x4D; // bool
        public const uint m_nLastSpokenPriority = 0x50; // int32_t
        public const uint m_pOuter = 0x58; // CBaseFlex*
    }

    public static class CAI_ExpresserWithFollowup  // CAI_Expresser
    {
        public const uint m_pPostponedFollowup = 0x60; // ResponseFollowup*
    }

    public static class CAK47  // CCSWeaponBaseGun
    {
    }

    public static class CAmbientGeneric  // CPointEntity
    {
        public const uint m_radius = 0x4B0; // float
        public const uint m_flMaxRadius = 0x4B4; // float
        public const uint m_iSoundLevel = 0x4B8; // soundlevel_t
        public const uint m_dpv = 0x4BC; // dynpitchvol_t
        public const uint m_fActive = 0x520; // bool
        public const uint m_fLooping = 0x521; // bool
        public const uint m_iszSound = 0x528; // CUtlSymbolLarge
        public const uint m_sSourceEntName = 0x530; // CUtlSymbolLarge
        public const uint m_hSoundSource = 0x538; // CHandle<CBaseEntity>
        public const uint m_nSoundSourceEntIndex = 0x53C; // CEntityIndex
    }

    public static class CAnimEventListener  // CAnimEventListenerBase
    {
    }

    public static class CAnimEventListenerBase 
    {
    }

    public static class CAnimEventQueueListener  // CAnimEventListenerBase
    {
    }

    public static class CAnimGraphControllerBase 
    {
    }

    public static class CAnimGraphNetworkedVariables 
    {
        public const uint m_PredNetBoolVariables = 0x8; // CNetworkUtlVectorBase<uint32_t>
        public const uint m_PredNetByteVariables = 0x20; // CNetworkUtlVectorBase<uint8_t>
        public const uint m_PredNetUInt16Variables = 0x38; // CNetworkUtlVectorBase<uint16_t>
        public const uint m_PredNetIntVariables = 0x50; // CNetworkUtlVectorBase<int32_t>
        public const uint m_PredNetUInt32Variables = 0x68; // CNetworkUtlVectorBase<uint32_t>
        public const uint m_PredNetUInt64Variables = 0x80; // CNetworkUtlVectorBase<uint64_t>
        public const uint m_PredNetFloatVariables = 0x98; // CNetworkUtlVectorBase<float>
        public const uint m_PredNetVectorVariables = 0xB0; // CNetworkUtlVectorBase<Vector>
        public const uint m_PredNetQuaternionVariables = 0xC8; // CNetworkUtlVectorBase<Quaternion>
        public const uint m_OwnerOnlyPredNetBoolVariables = 0xE0; // CNetworkUtlVectorBase<uint32_t>
        public const uint m_OwnerOnlyPredNetByteVariables = 0xF8; // CNetworkUtlVectorBase<uint8_t>
        public const uint m_OwnerOnlyPredNetUInt16Variables = 0x110; // CNetworkUtlVectorBase<uint16_t>
        public const uint m_OwnerOnlyPredNetIntVariables = 0x128; // CNetworkUtlVectorBase<int32_t>
        public const uint m_OwnerOnlyPredNetUInt32Variables = 0x140; // CNetworkUtlVectorBase<uint32_t>
        public const uint m_OwnerOnlyPredNetUInt64Variables = 0x158; // CNetworkUtlVectorBase<uint64_t>
        public const uint m_OwnerOnlyPredNetFloatVariables = 0x170; // CNetworkUtlVectorBase<float>
        public const uint m_OwnerOnlyPredNetVectorVariables = 0x188; // CNetworkUtlVectorBase<Vector>
        public const uint m_OwnerOnlyPredNetQuaternionVariables = 0x1A0; // CNetworkUtlVectorBase<Quaternion>
        public const uint m_nBoolVariablesCount = 0x1B8; // int32_t
        public const uint m_nOwnerOnlyBoolVariablesCount = 0x1BC; // int32_t
        public const uint m_nRandomSeedOffset = 0x1C0; // int32_t
        public const uint m_flLastTeleportTime = 0x1C4; // float
    }

    public static class CAnimGraphTagRef 
    {
        public const uint m_nTagIndex = 0x0; // int32_t
        public const uint m_tagName = 0x10; // CGlobalSymbol
    }

    public static class CAttributeContainer  // CAttributeManager
    {
        public const uint m_Item = 0x50; // CEconItemView
    }

    public static class CAttributeList 
    {
        public const uint m_Attributes = 0x8; // CUtlVectorEmbeddedNetworkVar<CEconItemAttribute>
        public const uint m_pManager = 0x58; // CAttributeManager*
    }

    public static class CAttributeManager 
    {
        public const uint m_Providers = 0x8; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_iReapplyProvisionParity = 0x20; // int32_t
        public const uint m_hOuter = 0x24; // CHandle<CBaseEntity>
        public const uint m_bPreventLoopback = 0x28; // bool
        public const uint m_ProviderType = 0x2C; // attributeprovidertypes_t
        public const uint m_CachedResults = 0x30; // CUtlVector<CAttributeManager::cached_attribute_float_t>
    }

    public static class CAttributeManager_cached_attribute_float_t 
    {
        public const uint flIn = 0x0; // float
        public const uint iAttribHook = 0x8; // CUtlSymbolLarge
        public const uint flOut = 0x10; // float
    }

    public static class CBarnLight  // CBaseModelEntity
    {
        public const uint m_bEnabled = 0x700; // bool
        public const uint m_nColorMode = 0x704; // int32_t
        public const uint m_Color = 0x708; // Color
        public const uint m_flColorTemperature = 0x70C; // float
        public const uint m_flBrightness = 0x710; // float
        public const uint m_flBrightnessScale = 0x714; // float
        public const uint m_nDirectLight = 0x718; // int32_t
        public const uint m_nBakedShadowIndex = 0x71C; // int32_t
        public const uint m_nLuminaireShape = 0x720; // int32_t
        public const uint m_flLuminaireSize = 0x724; // float
        public const uint m_flLuminaireAnisotropy = 0x728; // float
        public const uint m_LightStyleString = 0x730; // CUtlString
        public const uint m_flLightStyleStartTime = 0x738; // GameTime_t
        public const uint m_QueuedLightStyleStrings = 0x740; // CNetworkUtlVectorBase<CUtlString>
        public const uint m_LightStyleEvents = 0x758; // CNetworkUtlVectorBase<CUtlString>
        public const uint m_LightStyleTargets = 0x770; // CNetworkUtlVectorBase<CHandle<CBaseModelEntity>>
        public const uint m_StyleEvent = 0x788; // CEntityIOOutput[4]
        public const uint m_hLightCookie = 0x848; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_flShape = 0x850; // float
        public const uint m_flSoftX = 0x854; // float
        public const uint m_flSoftY = 0x858; // float
        public const uint m_flSkirt = 0x85C; // float
        public const uint m_flSkirtNear = 0x860; // float
        public const uint m_vSizeParams = 0x864; // Vector
        public const uint m_flRange = 0x870; // float
        public const uint m_vShear = 0x874; // Vector
        public const uint m_nBakeSpecularToCubemaps = 0x880; // int32_t
        public const uint m_vBakeSpecularToCubemapsSize = 0x884; // Vector
        public const uint m_nCastShadows = 0x890; // int32_t
        public const uint m_nShadowMapSize = 0x894; // int32_t
        public const uint m_nShadowPriority = 0x898; // int32_t
        public const uint m_bContactShadow = 0x89C; // bool
        public const uint m_nBounceLight = 0x8A0; // int32_t
        public const uint m_flBounceScale = 0x8A4; // float
        public const uint m_flMinRoughness = 0x8A8; // float
        public const uint m_vAlternateColor = 0x8AC; // Vector
        public const uint m_fAlternateColorBrightness = 0x8B8; // float
        public const uint m_nFog = 0x8BC; // int32_t
        public const uint m_flFogStrength = 0x8C0; // float
        public const uint m_nFogShadows = 0x8C4; // int32_t
        public const uint m_flFogScale = 0x8C8; // float
        public const uint m_flFadeSizeStart = 0x8CC; // float
        public const uint m_flFadeSizeEnd = 0x8D0; // float
        public const uint m_flShadowFadeSizeStart = 0x8D4; // float
        public const uint m_flShadowFadeSizeEnd = 0x8D8; // float
        public const uint m_bPrecomputedFieldsValid = 0x8DC; // bool
        public const uint m_vPrecomputedBoundsMins = 0x8E0; // Vector
        public const uint m_vPrecomputedBoundsMaxs = 0x8EC; // Vector
        public const uint m_vPrecomputedOBBOrigin = 0x8F8; // Vector
        public const uint m_vPrecomputedOBBAngles = 0x904; // QAngle
        public const uint m_vPrecomputedOBBExtent = 0x910; // Vector
        public const uint m_bPvsModifyEntity = 0x91C; // bool
    }

    public static class CBaseAnimGraph  // CBaseModelEntity
    {
        public const uint m_bInitiallyPopulateInterpHistory = 0x700; // bool
        public const uint m_bShouldAnimateDuringGameplayPause = 0x701; // bool
        public const uint m_pChoreoServices = 0x708; // IChoreoServices*
        public const uint m_bAnimGraphUpdateEnabled = 0x710; // bool
        public const uint m_flMaxSlopeDistance = 0x714; // float
        public const uint m_vLastSlopeCheckPos = 0x718; // Vector
        public const uint m_bAnimGraphDirty = 0x724; // bool
        public const uint m_vecForce = 0x728; // Vector
        public const uint m_nForceBone = 0x734; // int32_t
        public const uint m_pRagdollPose = 0x748; // PhysicsRagdollPose_t*
        public const uint m_bClientRagdoll = 0x750; // bool
    }

    public static class CBaseAnimGraphController  // CSkeletonAnimationController
    {
        public const uint m_baseLayer = 0x18; // CNetworkedSequenceOperation
        public const uint m_animGraphNetworkedVars = 0x40; // CAnimGraphNetworkedVariables
        public const uint m_bSequenceFinished = 0x218; // bool
        public const uint m_flLastEventCycle = 0x21C; // float
        public const uint m_flLastEventAnimTime = 0x220; // float
        public const uint m_flPlaybackRate = 0x224; // CNetworkedQuantizedFloat
        public const uint m_flPrevAnimTime = 0x22C; // float
        public const uint m_bClientSideAnimation = 0x230; // bool
        public const uint m_bNetworkedAnimationInputsChanged = 0x231; // bool
        public const uint m_nNewSequenceParity = 0x234; // int32_t
        public const uint m_nResetEventsParity = 0x238; // int32_t
        public const uint m_nAnimLoopMode = 0x23C; // AnimLoopMode_t
        public const uint m_hAnimationUpdate = 0x2DC; // AnimationUpdateListHandle_t
    }

    public static class CBaseButton  // CBaseToggle
    {
        public const uint m_angMoveEntitySpace = 0x780; // QAngle
        public const uint m_fStayPushed = 0x78C; // bool
        public const uint m_fRotating = 0x78D; // bool
        public const uint m_ls = 0x790; // locksound_t
        public const uint m_sUseSound = 0x7B0; // CUtlSymbolLarge
        public const uint m_sLockedSound = 0x7B8; // CUtlSymbolLarge
        public const uint m_sUnlockedSound = 0x7C0; // CUtlSymbolLarge
        public const uint m_bLocked = 0x7C8; // bool
        public const uint m_bDisabled = 0x7C9; // bool
        public const uint m_flUseLockedTime = 0x7CC; // GameTime_t
        public const uint m_bSolidBsp = 0x7D0; // bool
        public const uint m_OnDamaged = 0x7D8; // CEntityIOOutput
        public const uint m_OnPressed = 0x800; // CEntityIOOutput
        public const uint m_OnUseLocked = 0x828; // CEntityIOOutput
        public const uint m_OnIn = 0x850; // CEntityIOOutput
        public const uint m_OnOut = 0x878; // CEntityIOOutput
        public const uint m_nState = 0x8A0; // int32_t
        public const uint m_hConstraint = 0x8A4; // CEntityHandle
        public const uint m_hConstraintParent = 0x8A8; // CEntityHandle
        public const uint m_bForceNpcExclude = 0x8AC; // bool
        public const uint m_sGlowEntity = 0x8B0; // CUtlSymbolLarge
        public const uint m_glowEntity = 0x8B8; // CHandle<CBaseModelEntity>
        public const uint m_usable = 0x8BC; // bool
        public const uint m_szDisplayText = 0x8C0; // CUtlSymbolLarge
    }

    public static class CBaseCSGrenade  // CCSWeaponBase
    {
        public const uint m_bRedraw = 0xE28; // bool
        public const uint m_bIsHeldByPlayer = 0xE29; // bool
        public const uint m_bPinPulled = 0xE2A; // bool
        public const uint m_bJumpThrow = 0xE2B; // bool
        public const uint m_eThrowStatus = 0xE2C; // EGrenadeThrowState
        public const uint m_fThrowTime = 0xE30; // GameTime_t
        public const uint m_flThrowStrength = 0xE34; // float
        public const uint m_flThrowStrengthApproach = 0xE38; // float
        public const uint m_fDropTime = 0xE3C; // GameTime_t
        public const uint m_nNextHoldTick = 0xE40; // GameTick_t
        public const uint m_flNextHoldFrac = 0xE44; // float
    }

    public static class CBaseCSGrenadeProjectile  // CBaseGrenade
    {
        public const uint m_vInitialPosition = 0x9C8; // Vector
        public const uint m_vInitialVelocity = 0x9D4; // Vector
        public const uint m_nBounces = 0x9E0; // int32_t
        public const uint m_nExplodeEffectIndex = 0x9E8; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_nExplodeEffectTickBegin = 0x9F0; // int32_t
        public const uint m_vecExplodeEffectOrigin = 0x9F4; // Vector
        public const uint m_flSpawnTime = 0xA00; // GameTime_t
        public const uint m_unOGSExtraFlags = 0xA04; // uint8_t
        public const uint m_bDetonationRecorded = 0xA05; // bool
        public const uint m_flDetonateTime = 0xA08; // GameTime_t
        public const uint m_nItemIndex = 0xA0C; // uint16_t
        public const uint m_vecOriginalSpawnLocation = 0xA10; // Vector
        public const uint m_flLastBounceSoundTime = 0xA1C; // GameTime_t
        public const uint m_vecGrenadeSpin = 0xA20; // RotationVector
        public const uint m_vecLastHitSurfaceNormal = 0xA2C; // Vector
        public const uint m_nTicksAtZeroVelocity = 0xA38; // int32_t
        public const uint m_bHasEverHitPlayer = 0xA3C; // bool
        public const uint m_bClearFromPlayers = 0xA3D; // bool
    }

    public static class CBaseClientUIEntity  // CBaseModelEntity
    {
        public const uint m_bEnabled = 0x700; // bool
        public const uint m_DialogXMLName = 0x708; // CUtlSymbolLarge
        public const uint m_PanelClassName = 0x710; // CUtlSymbolLarge
        public const uint m_PanelID = 0x718; // CUtlSymbolLarge
        public const uint m_CustomOutput0 = 0x720; // CEntityIOOutput
        public const uint m_CustomOutput1 = 0x748; // CEntityIOOutput
        public const uint m_CustomOutput2 = 0x770; // CEntityIOOutput
        public const uint m_CustomOutput3 = 0x798; // CEntityIOOutput
        public const uint m_CustomOutput4 = 0x7C0; // CEntityIOOutput
        public const uint m_CustomOutput5 = 0x7E8; // CEntityIOOutput
        public const uint m_CustomOutput6 = 0x810; // CEntityIOOutput
        public const uint m_CustomOutput7 = 0x838; // CEntityIOOutput
        public const uint m_CustomOutput8 = 0x860; // CEntityIOOutput
        public const uint m_CustomOutput9 = 0x888; // CEntityIOOutput
    }

    public static class CBaseCombatCharacter  // CBaseFlex
    {
        public const uint m_bForceServerRagdoll = 0x920; // bool
        public const uint m_hMyWearables = 0x928; // CNetworkUtlVectorBase<CHandle<CEconWearable>>
        public const uint m_flFieldOfView = 0x940; // float
        public const uint m_impactEnergyScale = 0x944; // float
        public const uint m_LastHitGroup = 0x948; // HitGroup_t
        public const uint m_bApplyStressDamage = 0x94C; // bool
        public const uint m_bloodColor = 0x950; // int32_t
        public const uint m_navMeshID = 0x9B0; // int32_t
        public const uint m_iDamageCount = 0x9B4; // int32_t
        public const uint m_pVecRelationships = 0x9B8; // CUtlVector<RelationshipOverride_t>*
        public const uint m_strRelationships = 0x9C0; // CUtlSymbolLarge
        public const uint m_eHull = 0x9C8; // Hull_t
        public const uint m_nNavHullIdx = 0x9CC; // uint32_t
    }

    public static class CBaseDMStart  // CPointEntity
    {
        public const uint m_Master = 0x4B0; // CUtlSymbolLarge
    }

    public static class CBaseDoor  // CBaseToggle
    {
        public const uint m_angMoveEntitySpace = 0x790; // QAngle
        public const uint m_vecMoveDirParentSpace = 0x79C; // Vector
        public const uint m_ls = 0x7A8; // locksound_t
        public const uint m_bForceClosed = 0x7C8; // bool
        public const uint m_bDoorGroup = 0x7C9; // bool
        public const uint m_bLocked = 0x7CA; // bool
        public const uint m_bIgnoreDebris = 0x7CB; // bool
        public const uint m_eSpawnPosition = 0x7CC; // FuncDoorSpawnPos_t
        public const uint m_flBlockDamage = 0x7D0; // float
        public const uint m_NoiseMoving = 0x7D8; // CUtlSymbolLarge
        public const uint m_NoiseArrived = 0x7E0; // CUtlSymbolLarge
        public const uint m_NoiseMovingClosed = 0x7E8; // CUtlSymbolLarge
        public const uint m_NoiseArrivedClosed = 0x7F0; // CUtlSymbolLarge
        public const uint m_ChainTarget = 0x7F8; // CUtlSymbolLarge
        public const uint m_OnBlockedClosing = 0x800; // CEntityIOOutput
        public const uint m_OnBlockedOpening = 0x828; // CEntityIOOutput
        public const uint m_OnUnblockedClosing = 0x850; // CEntityIOOutput
        public const uint m_OnUnblockedOpening = 0x878; // CEntityIOOutput
        public const uint m_OnFullyClosed = 0x8A0; // CEntityIOOutput
        public const uint m_OnFullyOpen = 0x8C8; // CEntityIOOutput
        public const uint m_OnClose = 0x8F0; // CEntityIOOutput
        public const uint m_OnOpen = 0x918; // CEntityIOOutput
        public const uint m_OnLockedUse = 0x940; // CEntityIOOutput
        public const uint m_bLoopMoveSound = 0x968; // bool
        public const uint m_bCreateNavObstacle = 0x980; // bool
        public const uint m_isChaining = 0x981; // bool
        public const uint m_bIsUsable = 0x982; // bool
    }

    public static class CBaseEntity  // CEntityInstance
    {
        public const uint m_CBodyComponent = 0x30; // CBodyComponent*
        public const uint m_NetworkTransmitComponent = 0x38; // CNetworkTransmitComponent
        public const uint m_aThinkFunctions = 0x228; // CUtlVector<thinkfunc_t>
        public const uint m_iCurrentThinkContext = 0x240; // int32_t
        public const uint m_nLastThinkTick = 0x244; // GameTick_t
        public const uint m_isSteadyState = 0x250; // CBitVec<64>
        public const uint m_lastNetworkChange = 0x258; // float
        public const uint m_ResponseContexts = 0x268; // CUtlVector<ResponseContext_t>
        public const uint m_iszResponseContext = 0x280; // CUtlSymbolLarge
        public const uint m_iHealth = 0x2A8; // int32_t
        public const uint m_iMaxHealth = 0x2AC; // int32_t
        public const uint m_lifeState = 0x2B0; // uint8_t
        public const uint m_flDamageAccumulator = 0x2B4; // float
        public const uint m_bTakesDamage = 0x2B8; // bool
        public const uint m_nTakeDamageFlags = 0x2BC; // TakeDamageFlags_t
        public const uint m_MoveCollide = 0x2C1; // MoveCollide_t
        public const uint m_MoveType = 0x2C2; // MoveType_t
        public const uint m_nWaterTouch = 0x2C3; // uint8_t
        public const uint m_nSlimeTouch = 0x2C4; // uint8_t
        public const uint m_bRestoreInHierarchy = 0x2C5; // bool
        public const uint m_target = 0x2C8; // CUtlSymbolLarge
        public const uint m_flMoveDoneTime = 0x2D0; // float
        public const uint m_hDamageFilter = 0x2D4; // CHandle<CBaseFilter>
        public const uint m_iszDamageFilterName = 0x2D8; // CUtlSymbolLarge
        public const uint m_nSubclassID = 0x2E0; // CUtlStringToken
        public const uint m_flAnimTime = 0x2F0; // float
        public const uint m_flSimulationTime = 0x2F4; // float
        public const uint m_flCreateTime = 0x2F8; // GameTime_t
        public const uint m_bClientSideRagdoll = 0x2FC; // bool
        public const uint m_ubInterpolationFrame = 0x2FD; // uint8_t
        public const uint m_vPrevVPhysicsUpdatePos = 0x300; // Vector
        public const uint m_iTeamNum = 0x30C; // uint8_t
        public const uint m_iGlobalname = 0x310; // CUtlSymbolLarge
        public const uint m_iSentToClients = 0x318; // int32_t
        public const uint m_flSpeed = 0x31C; // float
        public const uint m_sUniqueHammerID = 0x320; // CUtlString
        public const uint m_spawnflags = 0x328; // uint32_t
        public const uint m_nNextThinkTick = 0x32C; // GameTick_t
        public const uint m_nSimulationTick = 0x330; // int32_t
        public const uint m_OnKilled = 0x338; // CEntityIOOutput
        public const uint m_fFlags = 0x360; // uint32_t
        public const uint m_vecAbsVelocity = 0x364; // Vector
        public const uint m_vecVelocity = 0x370; // CNetworkVelocityVector
        public const uint m_vecBaseVelocity = 0x3A0; // Vector
        public const uint m_nPushEnumCount = 0x3AC; // int32_t
        public const uint m_pCollision = 0x3B0; // CCollisionProperty*
        public const uint m_hEffectEntity = 0x3B8; // CHandle<CBaseEntity>
        public const uint m_hOwnerEntity = 0x3BC; // CHandle<CBaseEntity>
        public const uint m_fEffects = 0x3C0; // uint32_t
        public const uint m_hGroundEntity = 0x3C4; // CHandle<CBaseEntity>
        public const uint m_flFriction = 0x3C8; // float
        public const uint m_flElasticity = 0x3CC; // float
        public const uint m_flGravityScale = 0x3D0; // float
        public const uint m_flTimeScale = 0x3D4; // float
        public const uint m_flWaterLevel = 0x3D8; // float
        public const uint m_bSimulatedEveryTick = 0x3DC; // bool
        public const uint m_bAnimatedEveryTick = 0x3DD; // bool
        public const uint m_bDisableLowViolence = 0x3DE; // bool
        public const uint m_nWaterType = 0x3DF; // uint8_t
        public const uint m_iEFlags = 0x3E0; // int32_t
        public const uint m_OnUser1 = 0x3E8; // CEntityIOOutput
        public const uint m_OnUser2 = 0x410; // CEntityIOOutput
        public const uint m_OnUser3 = 0x438; // CEntityIOOutput
        public const uint m_OnUser4 = 0x460; // CEntityIOOutput
        public const uint m_iInitialTeamNum = 0x488; // int32_t
        public const uint m_flNavIgnoreUntilTime = 0x48C; // GameTime_t
        public const uint m_vecAngVelocity = 0x490; // QAngle
        public const uint m_bNetworkQuantizeOriginAndAngles = 0x49C; // bool
        public const uint m_bLagCompensate = 0x49D; // bool
        public const uint m_flOverriddenFriction = 0x4A0; // float
        public const uint m_pBlocker = 0x4A4; // CHandle<CBaseEntity>
        public const uint m_flLocalTime = 0x4A8; // float
        public const uint m_flVPhysicsUpdateLocalTime = 0x4AC; // float
    }

    public static class CBaseFilter  // CLogicalEntity
    {
        public const uint m_bNegated = 0x4B0; // bool
        public const uint m_OnPass = 0x4B8; // CEntityIOOutput
        public const uint m_OnFail = 0x4E0; // CEntityIOOutput
    }

    public static class CBaseFire  // CBaseEntity
    {
        public const uint m_flScale = 0x4B0; // float
        public const uint m_flStartScale = 0x4B4; // float
        public const uint m_flScaleTime = 0x4B8; // float
        public const uint m_nFlags = 0x4BC; // uint32_t
    }

    public static class CBaseFlex  // CBaseAnimGraph
    {
        public const uint m_flexWeight = 0x890; // CNetworkUtlVectorBase<float>
        public const uint m_vLookTargetPosition = 0x8A8; // Vector
        public const uint m_blinktoggle = 0x8B4; // bool
        public const uint m_flAllowResponsesEndTime = 0x908; // GameTime_t
        public const uint m_flLastFlexAnimationTime = 0x90C; // GameTime_t
        public const uint m_nNextSceneEventId = 0x910; // uint32_t
        public const uint m_bUpdateLayerPriorities = 0x914; // bool
    }

    public static class CBaseFlexAlias_funCBaseFlex  // CBaseFlex
    {
    }

    public static class CBaseGrenade  // CBaseFlex
    {
        public const uint m_OnPlayerPickup = 0x928; // CEntityIOOutput
        public const uint m_OnExplode = 0x950; // CEntityIOOutput
        public const uint m_bHasWarnedAI = 0x978; // bool
        public const uint m_bIsSmokeGrenade = 0x979; // bool
        public const uint m_bIsLive = 0x97A; // bool
        public const uint m_DmgRadius = 0x97C; // float
        public const uint m_flDetonateTime = 0x980; // GameTime_t
        public const uint m_flWarnAITime = 0x984; // float
        public const uint m_flDamage = 0x988; // float
        public const uint m_iszBounceSound = 0x990; // CUtlSymbolLarge
        public const uint m_ExplosionSound = 0x998; // CUtlString
        public const uint m_hThrower = 0x9A4; // CHandle<CCSPlayerPawn>
        public const uint m_flNextAttack = 0x9BC; // GameTime_t
        public const uint m_hOriginalThrower = 0x9C0; // CHandle<CCSPlayerPawn>
    }

    public static class CBaseIssue 
    {
        public const uint m_szTypeString = 0x20; // char[64]
        public const uint m_szDetailsString = 0x60; // char[260]
        public const uint m_iNumYesVotes = 0x164; // int32_t
        public const uint m_iNumNoVotes = 0x168; // int32_t
        public const uint m_iNumPotentialVotes = 0x16C; // int32_t
        public const uint m_pVoteController = 0x170; // CVoteController*
    }

    public static class CBaseModelEntity  // CBaseEntity
    {
        public const uint m_CRenderComponent = 0x4B0; // CRenderComponent*
        public const uint m_CHitboxComponent = 0x4B8; // CHitboxComponent
        public const uint m_flDissolveStartTime = 0x4E0; // GameTime_t
        public const uint m_OnIgnite = 0x4E8; // CEntityIOOutput
        public const uint m_nRenderMode = 0x510; // RenderMode_t
        public const uint m_nRenderFX = 0x511; // RenderFx_t
        public const uint m_bAllowFadeInView = 0x512; // bool
        public const uint m_clrRender = 0x513; // Color
        public const uint m_vecRenderAttributes = 0x518; // CUtlVectorEmbeddedNetworkVar<EntityRenderAttribute_t>
        public const uint m_bRenderToCubemaps = 0x568; // bool
        public const uint m_Collision = 0x570; // CCollisionProperty
        public const uint m_Glow = 0x620; // CGlowProperty
        public const uint m_flGlowBackfaceMult = 0x678; // float
        public const uint m_fadeMinDist = 0x67C; // float
        public const uint m_fadeMaxDist = 0x680; // float
        public const uint m_flFadeScale = 0x684; // float
        public const uint m_flShadowStrength = 0x688; // float
        public const uint m_nObjectCulling = 0x68C; // uint8_t
        public const uint m_nAddDecal = 0x690; // int32_t
        public const uint m_vDecalPosition = 0x694; // Vector
        public const uint m_vDecalForwardAxis = 0x6A0; // Vector
        public const uint m_flDecalHealBloodRate = 0x6AC; // float
        public const uint m_flDecalHealHeightRate = 0x6B0; // float
        public const uint m_ConfigEntitiesToPropagateMaterialDecalsTo = 0x6B8; // CNetworkUtlVectorBase<CHandle<CBaseModelEntity>>
        public const uint m_vecViewOffset = 0x6D0; // CNetworkViewOffsetVector
    }

    public static class CBaseMoveBehavior  // CPathKeyFrame
    {
        public const uint m_iPositionInterpolator = 0x510; // int32_t
        public const uint m_iRotationInterpolator = 0x514; // int32_t
        public const uint m_flAnimStartTime = 0x518; // float
        public const uint m_flAnimEndTime = 0x51C; // float
        public const uint m_flAverageSpeedAcrossFrame = 0x520; // float
        public const uint m_pCurrentKeyFrame = 0x528; // CPathKeyFrame*
        public const uint m_pTargetKeyFrame = 0x530; // CPathKeyFrame*
        public const uint m_pPreKeyFrame = 0x538; // CPathKeyFrame*
        public const uint m_pPostKeyFrame = 0x540; // CPathKeyFrame*
        public const uint m_flTimeIntoFrame = 0x548; // float
        public const uint m_iDirection = 0x54C; // int32_t
    }

    public static class CBasePlatTrain  // CBaseToggle
    {
        public const uint m_NoiseMoving = 0x780; // CUtlSymbolLarge
        public const uint m_NoiseArrived = 0x788; // CUtlSymbolLarge
        public const uint m_volume = 0x798; // float
        public const uint m_flTWidth = 0x79C; // float
        public const uint m_flTLength = 0x7A0; // float
    }

    public static class CBasePlayerController  // CBaseEntity
    {
        public const uint m_nInButtonsWhichAreToggles = 0x4B8; // uint64_t
        public const uint m_nTickBase = 0x4C0; // uint32_t
        public const uint m_hPawn = 0x4F0; // CHandle<CBasePlayerPawn>
        public const uint m_nSplitScreenSlot = 0x4F4; // CSplitScreenSlot
        public const uint m_hSplitOwner = 0x4F8; // CHandle<CBasePlayerController>
        public const uint m_hSplitScreenPlayers = 0x500; // CUtlVector<CHandle<CBasePlayerController>>
        public const uint m_bIsHLTV = 0x518; // bool
        public const uint m_iConnected = 0x51C; // PlayerConnectedState
        public const uint m_iszPlayerName = 0x520; // char[128]
        public const uint m_szNetworkIDString = 0x5A0; // CUtlString
        public const uint m_fLerpTime = 0x5A8; // float
        public const uint m_bLagCompensation = 0x5AC; // bool
        public const uint m_bPredict = 0x5AD; // bool
        public const uint m_bAutoKickDisabled = 0x5AE; // bool
        public const uint m_bIsLowViolence = 0x5AF; // bool
        public const uint m_bGamePaused = 0x5B0; // bool
        public const uint m_nUsecTimestampLastUserCmdReceived = 0x648; // int64_t
        public const uint m_iIgnoreGlobalChat = 0x660; // ChatIgnoreType_t
        public const uint m_flLastPlayerTalkTime = 0x664; // float
        public const uint m_flLastEntitySteadyState = 0x668; // float
        public const uint m_nAvailableEntitySteadyState = 0x66C; // int32_t
        public const uint m_bHasAnySteadyStateEnts = 0x670; // bool
        public const uint m_steamID = 0x680; // uint64_t
        public const uint m_iDesiredFOV = 0x688; // uint32_t
    }

    public static class CBasePlayerPawn  // CBaseCombatCharacter
    {
        public const uint m_pWeaponServices = 0x9D0; // CPlayer_WeaponServices*
        public const uint m_pItemServices = 0x9D8; // CPlayer_ItemServices*
        public const uint m_pAutoaimServices = 0x9E0; // CPlayer_AutoaimServices*
        public const uint m_pObserverServices = 0x9E8; // CPlayer_ObserverServices*
        public const uint m_pWaterServices = 0x9F0; // CPlayer_WaterServices*
        public const uint m_pUseServices = 0x9F8; // CPlayer_UseServices*
        public const uint m_pFlashlightServices = 0xA00; // CPlayer_FlashlightServices*
        public const uint m_pCameraServices = 0xA08; // CPlayer_CameraServices*
        public const uint m_pMovementServices = 0xA10; // CPlayer_MovementServices*
        public const uint m_ServerViewAngleChanges = 0xA20; // CUtlVectorEmbeddedNetworkVar<ViewAngleServerChange_t>
        public const uint m_nHighestGeneratedServerViewAngleChangeIndex = 0xA70; // uint32_t
        public const uint v_angle = 0xA74; // QAngle
        public const uint v_anglePrevious = 0xA80; // QAngle
        public const uint m_iHideHUD = 0xA8C; // uint32_t
        public const uint m_skybox3d = 0xA90; // sky3dparams_t
        public const uint m_fTimeLastHurt = 0xB20; // GameTime_t
        public const uint m_flDeathTime = 0xB24; // GameTime_t
        public const uint m_fNextSuicideTime = 0xB28; // GameTime_t
        public const uint m_fInitHUD = 0xB2C; // bool
        public const uint m_pExpresser = 0xB30; // CAI_Expresser*
        public const uint m_hController = 0xB38; // CHandle<CBasePlayerController>
        public const uint m_fHltvReplayDelay = 0xB40; // float
        public const uint m_fHltvReplayEnd = 0xB44; // float
        public const uint m_iHltvReplayEntity = 0xB48; // CEntityIndex
    }

    public static class CBasePlayerVData  // CEntitySubclassVDataBase
    {
        public const uint m_sModelName = 0x28; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_flHeadDamageMultiplier = 0x108; // CSkillFloat
        public const uint m_flChestDamageMultiplier = 0x118; // CSkillFloat
        public const uint m_flStomachDamageMultiplier = 0x128; // CSkillFloat
        public const uint m_flArmDamageMultiplier = 0x138; // CSkillFloat
        public const uint m_flLegDamageMultiplier = 0x148; // CSkillFloat
        public const uint m_flHoldBreathTime = 0x158; // float
        public const uint m_flDrowningDamageInterval = 0x15C; // float
        public const uint m_nDrowningDamageInitial = 0x160; // int32_t
        public const uint m_nDrowningDamageMax = 0x164; // int32_t
        public const uint m_nWaterSpeed = 0x168; // int32_t
        public const uint m_flUseRange = 0x16C; // float
        public const uint m_flUseAngleTolerance = 0x170; // float
        public const uint m_flCrouchTime = 0x174; // float
    }

    public static class CBasePlayerWeapon  // CEconEntity
    {
        public const uint m_nNextPrimaryAttackTick = 0xC18; // GameTick_t
        public const uint m_flNextPrimaryAttackTickRatio = 0xC1C; // float
        public const uint m_nNextSecondaryAttackTick = 0xC20; // GameTick_t
        public const uint m_flNextSecondaryAttackTickRatio = 0xC24; // float
        public const uint m_iClip1 = 0xC28; // int32_t
        public const uint m_iClip2 = 0xC2C; // int32_t
        public const uint m_pReserveAmmo = 0xC30; // int32_t[2]
        public const uint m_OnPlayerUse = 0xC38; // CEntityIOOutput
    }

    public static class CBasePlayerWeaponVData  // CEntitySubclassVDataBase
    {
        public const uint m_szWorldModel = 0x28; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_bBuiltRightHanded = 0x108; // bool
        public const uint m_bAllowFlipping = 0x109; // bool
        public const uint m_bIsFullAuto = 0x10A; // bool
        public const uint m_nNumBullets = 0x10C; // int32_t
        public const uint m_sMuzzleAttachment = 0x110; // CUtlString
        public const uint m_szMuzzleFlashParticle = 0x118; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_iFlags = 0x1F8; // ItemFlagTypes_t
        public const uint m_nPrimaryAmmoType = 0x1F9; // AmmoIndex_t
        public const uint m_nSecondaryAmmoType = 0x1FA; // AmmoIndex_t
        public const uint m_iMaxClip1 = 0x1FC; // int32_t
        public const uint m_iMaxClip2 = 0x200; // int32_t
        public const uint m_iDefaultClip1 = 0x204; // int32_t
        public const uint m_iDefaultClip2 = 0x208; // int32_t
        public const uint m_iWeight = 0x20C; // int32_t
        public const uint m_bAutoSwitchTo = 0x210; // bool
        public const uint m_bAutoSwitchFrom = 0x211; // bool
        public const uint m_iRumbleEffect = 0x214; // RumbleEffect_t
        public const uint m_aShootSounds = 0x218; // CUtlMap<WeaponSound_t,CSoundEventName>
        public const uint m_iSlot = 0x238; // int32_t
        public const uint m_iPosition = 0x23C; // int32_t
    }

    public static class CBaseProp  // CBaseAnimGraph
    {
        public const uint m_bModelOverrodeBlockLOS = 0x890; // bool
        public const uint m_iShapeType = 0x894; // int32_t
        public const uint m_bConformToCollisionBounds = 0x898; // bool
        public const uint m_mPreferredCatchTransform = 0x89C; // matrix3x4_t
    }

    public static class CBasePropDoor  // CDynamicProp
    {
        public const uint m_flAutoReturnDelay = 0xB18; // float
        public const uint m_hDoorList = 0xB20; // CUtlVector<CHandle<CBasePropDoor>>
        public const uint m_nHardwareType = 0xB38; // int32_t
        public const uint m_bNeedsHardware = 0xB3C; // bool
        public const uint m_eDoorState = 0xB40; // DoorState_t
        public const uint m_bLocked = 0xB44; // bool
        public const uint m_closedPosition = 0xB48; // Vector
        public const uint m_closedAngles = 0xB54; // QAngle
        public const uint m_hBlocker = 0xB60; // CHandle<CBaseEntity>
        public const uint m_bFirstBlocked = 0xB64; // bool
        public const uint m_ls = 0xB68; // locksound_t
        public const uint m_bForceClosed = 0xB88; // bool
        public const uint m_vecLatchWorldPosition = 0xB8C; // Vector
        public const uint m_hActivator = 0xB98; // CHandle<CBaseEntity>
        public const uint m_SoundMoving = 0xBA8; // CUtlSymbolLarge
        public const uint m_SoundOpen = 0xBB0; // CUtlSymbolLarge
        public const uint m_SoundClose = 0xBB8; // CUtlSymbolLarge
        public const uint m_SoundLock = 0xBC0; // CUtlSymbolLarge
        public const uint m_SoundUnlock = 0xBC8; // CUtlSymbolLarge
        public const uint m_SoundLatch = 0xBD0; // CUtlSymbolLarge
        public const uint m_SoundPound = 0xBD8; // CUtlSymbolLarge
        public const uint m_SoundJiggle = 0xBE0; // CUtlSymbolLarge
        public const uint m_SoundLockedAnim = 0xBE8; // CUtlSymbolLarge
        public const uint m_numCloseAttempts = 0xBF0; // int32_t
        public const uint m_nPhysicsMaterial = 0xBF4; // CUtlStringToken
        public const uint m_SlaveName = 0xBF8; // CUtlSymbolLarge
        public const uint m_hMaster = 0xC00; // CHandle<CBasePropDoor>
        public const uint m_OnBlockedClosing = 0xC08; // CEntityIOOutput
        public const uint m_OnBlockedOpening = 0xC30; // CEntityIOOutput
        public const uint m_OnUnblockedClosing = 0xC58; // CEntityIOOutput
        public const uint m_OnUnblockedOpening = 0xC80; // CEntityIOOutput
        public const uint m_OnFullyClosed = 0xCA8; // CEntityIOOutput
        public const uint m_OnFullyOpen = 0xCD0; // CEntityIOOutput
        public const uint m_OnClose = 0xCF8; // CEntityIOOutput
        public const uint m_OnOpen = 0xD20; // CEntityIOOutput
        public const uint m_OnLockedUse = 0xD48; // CEntityIOOutput
        public const uint m_OnAjarOpen = 0xD70; // CEntityIOOutput
    }

    public static class CBaseToggle  // CBaseModelEntity
    {
        public const uint m_toggle_state = 0x700; // TOGGLE_STATE
        public const uint m_flMoveDistance = 0x704; // float
        public const uint m_flWait = 0x708; // float
        public const uint m_flLip = 0x70C; // float
        public const uint m_bAlwaysFireBlockedOutputs = 0x710; // bool
        public const uint m_vecPosition1 = 0x714; // Vector
        public const uint m_vecPosition2 = 0x720; // Vector
        public const uint m_vecMoveAng = 0x72C; // QAngle
        public const uint m_vecAngle1 = 0x738; // QAngle
        public const uint m_vecAngle2 = 0x744; // QAngle
        public const uint m_flHeight = 0x750; // float
        public const uint m_hActivator = 0x754; // CHandle<CBaseEntity>
        public const uint m_vecFinalDest = 0x758; // Vector
        public const uint m_vecFinalAngle = 0x764; // QAngle
        public const uint m_movementType = 0x770; // int32_t
        public const uint m_sMaster = 0x778; // CUtlSymbolLarge
    }

    public static class CBaseTrigger  // CBaseToggle
    {
        public const uint m_bDisabled = 0x780; // bool
        public const uint m_iFilterName = 0x788; // CUtlSymbolLarge
        public const uint m_hFilter = 0x790; // CHandle<CBaseFilter>
        public const uint m_OnStartTouch = 0x798; // CEntityIOOutput
        public const uint m_OnStartTouchAll = 0x7C0; // CEntityIOOutput
        public const uint m_OnEndTouch = 0x7E8; // CEntityIOOutput
        public const uint m_OnEndTouchAll = 0x810; // CEntityIOOutput
        public const uint m_OnTouching = 0x838; // CEntityIOOutput
        public const uint m_OnNotTouching = 0x860; // CEntityIOOutput
        public const uint m_hTouchingEntities = 0x888; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_bClientSidePredicted = 0x8A0; // bool
    }

    public static class CBaseViewModel  // CBaseAnimGraph
    {
        public const uint m_vecLastFacing = 0x898; // Vector
        public const uint m_nViewModelIndex = 0x8A4; // uint32_t
        public const uint m_nAnimationParity = 0x8A8; // uint32_t
        public const uint m_flAnimationStartTime = 0x8AC; // float
        public const uint m_hWeapon = 0x8B0; // CHandle<CBasePlayerWeapon>
        public const uint m_sVMName = 0x8B8; // CUtlSymbolLarge
        public const uint m_sAnimationPrefix = 0x8C0; // CUtlSymbolLarge
        public const uint m_hOldLayerSequence = 0x8C8; // HSequence
        public const uint m_oldLayer = 0x8CC; // int32_t
        public const uint m_oldLayerStartTime = 0x8D0; // float
        public const uint m_hControlPanel = 0x8D4; // CHandle<CBaseEntity>
    }

    public static class CBeam  // CBaseModelEntity
    {
        public const uint m_flFrameRate = 0x700; // float
        public const uint m_flHDRColorScale = 0x704; // float
        public const uint m_flFireTime = 0x708; // GameTime_t
        public const uint m_flDamage = 0x70C; // float
        public const uint m_nNumBeamEnts = 0x710; // uint8_t
        public const uint m_hBaseMaterial = 0x718; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_nHaloIndex = 0x720; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_nBeamType = 0x728; // BeamType_t
        public const uint m_nBeamFlags = 0x72C; // uint32_t
        public const uint m_hAttachEntity = 0x730; // CHandle<CBaseEntity>[10]
        public const uint m_nAttachIndex = 0x758; // AttachmentHandle_t[10]
        public const uint m_fWidth = 0x764; // float
        public const uint m_fEndWidth = 0x768; // float
        public const uint m_fFadeLength = 0x76C; // float
        public const uint m_fHaloScale = 0x770; // float
        public const uint m_fAmplitude = 0x774; // float
        public const uint m_fStartFrame = 0x778; // float
        public const uint m_fSpeed = 0x77C; // float
        public const uint m_flFrame = 0x780; // float
        public const uint m_nClipStyle = 0x784; // BeamClipStyle_t
        public const uint m_bTurnedOff = 0x788; // bool
        public const uint m_vecEndPos = 0x78C; // Vector
        public const uint m_hEndEntity = 0x798; // CHandle<CBaseEntity>
        public const uint m_nDissolveType = 0x79C; // int32_t
    }

    public static class CBlood  // CPointEntity
    {
        public const uint m_vecSprayAngles = 0x4B0; // QAngle
        public const uint m_vecSprayDir = 0x4BC; // Vector
        public const uint m_flAmount = 0x4C8; // float
        public const uint m_Color = 0x4CC; // int32_t
    }

    public static class CBodyComponent  // CEntityComponent
    {
        public const uint m_pSceneNode = 0x8; // CGameSceneNode*
        public const uint __m_pChainEntity = 0x20; // CNetworkVarChainer
    }

    public static class CBodyComponentBaseAnimGraph  // CBodyComponentSkeletonInstance
    {
        public const uint m_animationController = 0x480; // CBaseAnimGraphController
        public const uint __m_pChainEntity = 0x760; // CNetworkVarChainer
    }

    public static class CBodyComponentBaseModelEntity  // CBodyComponentSkeletonInstance
    {
        public const uint __m_pChainEntity = 0x480; // CNetworkVarChainer
    }

    public static class CBodyComponentPoint  // CBodyComponent
    {
        public const uint m_sceneNode = 0x50; // CGameSceneNode
        public const uint __m_pChainEntity = 0x1A0; // CNetworkVarChainer
    }

    public static class CBodyComponentSkeletonInstance  // CBodyComponent
    {
        public const uint m_skeletonInstance = 0x50; // CSkeletonInstance
        public const uint __m_pChainEntity = 0x450; // CNetworkVarChainer
    }

    public static class CBombTarget  // CBaseTrigger
    {
        public const uint m_OnBombExplode = 0x8A8; // CEntityIOOutput
        public const uint m_OnBombPlanted = 0x8D0; // CEntityIOOutput
        public const uint m_OnBombDefused = 0x8F8; // CEntityIOOutput
        public const uint m_bIsBombSiteB = 0x920; // bool
        public const uint m_bIsHeistBombTarget = 0x921; // bool
        public const uint m_bBombPlantedHere = 0x922; // bool
        public const uint m_szMountTarget = 0x928; // CUtlSymbolLarge
        public const uint m_hInstructorHint = 0x930; // CHandle<CBaseEntity>
        public const uint m_nBombSiteDesignation = 0x934; // int32_t
    }

    public static class CBot 
    {
        public const uint m_pController = 0x10; // CCSPlayerController*
        public const uint m_pPlayer = 0x18; // CCSPlayerPawn*
        public const uint m_bHasSpawned = 0x20; // bool
        public const uint m_id = 0x24; // uint32_t
        public const uint m_isRunning = 0xB8; // bool
        public const uint m_isCrouching = 0xB9; // bool
        public const uint m_forwardSpeed = 0xBC; // float
        public const uint m_leftSpeed = 0xC0; // float
        public const uint m_verticalSpeed = 0xC4; // float
        public const uint m_buttonFlags = 0xC8; // uint64_t
        public const uint m_jumpTimestamp = 0xD0; // float
        public const uint m_viewForward = 0xD4; // Vector
        public const uint m_postureStackIndex = 0xF0; // int32_t
    }

    public static class CBreachCharge  // CCSWeaponBase
    {
    }

    public static class CBreachChargeProjectile  // CBaseGrenade
    {
    }

    public static class CBreakable  // CBaseModelEntity
    {
        public const uint m_Material = 0x710; // Materials
        public const uint m_hBreaker = 0x714; // CHandle<CBaseEntity>
        public const uint m_Explosion = 0x718; // Explosions
        public const uint m_iszSpawnObject = 0x720; // CUtlSymbolLarge
        public const uint m_flPressureDelay = 0x728; // float
        public const uint m_iMinHealthDmg = 0x72C; // int32_t
        public const uint m_iszPropData = 0x730; // CUtlSymbolLarge
        public const uint m_impactEnergyScale = 0x738; // float
        public const uint m_nOverrideBlockLOS = 0x73C; // EOverrideBlockLOS_t
        public const uint m_OnBreak = 0x740; // CEntityIOOutput
        public const uint m_OnHealthChanged = 0x768; // CEntityOutputTemplate<float>
        public const uint m_flDmgModBullet = 0x790; // float
        public const uint m_flDmgModClub = 0x794; // float
        public const uint m_flDmgModExplosive = 0x798; // float
        public const uint m_flDmgModFire = 0x79C; // float
        public const uint m_iszPhysicsDamageTableName = 0x7A0; // CUtlSymbolLarge
        public const uint m_iszBasePropData = 0x7A8; // CUtlSymbolLarge
        public const uint m_iInteractions = 0x7B0; // int32_t
        public const uint m_PerformanceMode = 0x7B4; // PerformanceMode_t
        public const uint m_hPhysicsAttacker = 0x7B8; // CHandle<CBasePlayerPawn>
        public const uint m_flLastPhysicsInfluenceTime = 0x7BC; // GameTime_t
    }

    public static class CBreakableProp  // CBaseProp
    {
        public const uint m_OnBreak = 0x8E0; // CEntityIOOutput
        public const uint m_OnHealthChanged = 0x908; // CEntityOutputTemplate<float>
        public const uint m_OnTakeDamage = 0x930; // CEntityIOOutput
        public const uint m_impactEnergyScale = 0x958; // float
        public const uint m_iMinHealthDmg = 0x95C; // int32_t
        public const uint m_preferredCarryAngles = 0x960; // QAngle
        public const uint m_flPressureDelay = 0x96C; // float
        public const uint m_hBreaker = 0x970; // CHandle<CBaseEntity>
        public const uint m_PerformanceMode = 0x974; // PerformanceMode_t
        public const uint m_flDmgModBullet = 0x978; // float
        public const uint m_flDmgModClub = 0x97C; // float
        public const uint m_flDmgModExplosive = 0x980; // float
        public const uint m_flDmgModFire = 0x984; // float
        public const uint m_iszPhysicsDamageTableName = 0x988; // CUtlSymbolLarge
        public const uint m_iszBasePropData = 0x990; // CUtlSymbolLarge
        public const uint m_iInteractions = 0x998; // int32_t
        public const uint m_flPreventDamageBeforeTime = 0x99C; // GameTime_t
        public const uint m_bHasBreakPiecesOrCommands = 0x9A0; // bool
        public const uint m_explodeDamage = 0x9A4; // float
        public const uint m_explodeRadius = 0x9A8; // float
        public const uint m_explosionDelay = 0x9B0; // float
        public const uint m_explosionBuildupSound = 0x9B8; // CUtlSymbolLarge
        public const uint m_explosionCustomEffect = 0x9C0; // CUtlSymbolLarge
        public const uint m_explosionCustomSound = 0x9C8; // CUtlSymbolLarge
        public const uint m_explosionModifier = 0x9D0; // CUtlSymbolLarge
        public const uint m_hPhysicsAttacker = 0x9D8; // CHandle<CBasePlayerPawn>
        public const uint m_flLastPhysicsInfluenceTime = 0x9DC; // GameTime_t
        public const uint m_bOriginalBlockLOS = 0x9E0; // bool
        public const uint m_flDefaultFadeScale = 0x9E4; // float
        public const uint m_hLastAttacker = 0x9E8; // CHandle<CBaseEntity>
        public const uint m_hFlareEnt = 0x9EC; // CHandle<CBaseEntity>
        public const uint m_bUsePuntSound = 0x9F0; // bool
        public const uint m_iszPuntSound = 0x9F8; // CUtlSymbolLarge
        public const uint m_noGhostCollision = 0xA00; // bool
    }

    public static class CBreakableStageHelper 
    {
        public const uint m_nCurrentStage = 0x8; // int32_t
        public const uint m_nStageCount = 0xC; // int32_t
    }

    public static class CBtActionAim  // CBtNode
    {
        public const uint m_szSensorInputKey = 0x68; // CUtlString
        public const uint m_szAimReadyKey = 0x80; // CUtlString
        public const uint m_flZoomCooldownTimestamp = 0x88; // float
        public const uint m_bDoneAiming = 0x8C; // bool
        public const uint m_flLerpStartTime = 0x90; // float
        public const uint m_flNextLookTargetLerpTime = 0x94; // float
        public const uint m_flPenaltyReductionRatio = 0x98; // float
        public const uint m_NextLookTarget = 0x9C; // QAngle
        public const uint m_AimTimer = 0xA8; // CountdownTimer
        public const uint m_SniperHoldTimer = 0xC0; // CountdownTimer
        public const uint m_FocusIntervalTimer = 0xD8; // CountdownTimer
        public const uint m_bAcquired = 0xF0; // bool
    }

    public static class CBtActionCombatPositioning  // CBtNode
    {
        public const uint m_szSensorInputKey = 0x68; // CUtlString
        public const uint m_szIsAttackingKey = 0x80; // CUtlString
        public const uint m_ActionTimer = 0x88; // CountdownTimer
        public const uint m_bCrouching = 0xA0; // bool
    }

    public static class CBtActionMoveTo  // CBtNode
    {
        public const uint m_szDestinationInputKey = 0x60; // CUtlString
        public const uint m_szHidingSpotInputKey = 0x68; // CUtlString
        public const uint m_szThreatInputKey = 0x70; // CUtlString
        public const uint m_vecDestination = 0x78; // Vector
        public const uint m_bAutoLookAdjust = 0x84; // bool
        public const uint m_bComputePath = 0x85; // bool
        public const uint m_flDamagingAreasPenaltyCost = 0x88; // float
        public const uint m_CheckApproximateCornersTimer = 0x90; // CountdownTimer
        public const uint m_CheckHighPriorityItem = 0xA8; // CountdownTimer
        public const uint m_RepathTimer = 0xC0; // CountdownTimer
        public const uint m_flArrivalEpsilon = 0xD8; // float
        public const uint m_flAdditionalArrivalEpsilon2D = 0xDC; // float
        public const uint m_flHidingSpotCheckDistanceThreshold = 0xE0; // float
        public const uint m_flNearestAreaDistanceThreshold = 0xE4; // float
    }

    public static class CBtActionParachutePositioning  // CBtNode
    {
        public const uint m_ActionTimer = 0x58; // CountdownTimer
    }

    public static class CBtNode 
    {
    }

    public static class CBtNodeComposite  // CBtNode
    {
    }

    public static class CBtNodeCondition  // CBtNodeDecorator
    {
        public const uint m_bNegated = 0x58; // bool
    }

    public static class CBtNodeConditionInactive  // CBtNodeCondition
    {
        public const uint m_flRoundStartThresholdSeconds = 0x78; // float
        public const uint m_flSensorInactivityThresholdSeconds = 0x7C; // float
        public const uint m_SensorInactivityTimer = 0x80; // CountdownTimer
    }

    public static class CBtNodeDecorator  // CBtNode
    {
    }

    public static class CBubbling  // CBaseModelEntity
    {
        public const uint m_density = 0x700; // int32_t
        public const uint m_frequency = 0x704; // int32_t
        public const uint m_state = 0x708; // int32_t
    }

    public static class CBumpMine  // CCSWeaponBase
    {
    }

    public static class CBumpMineProjectile  // CBaseGrenade
    {
    }

    public static class CBuoyancyHelper 
    {
        public const uint m_flFluidDensity = 0x18; // float
    }

    public static class CBuyZone  // CBaseTrigger
    {
        public const uint m_LegacyTeamNum = 0x8A8; // int32_t
    }

    public static class CC4  // CCSWeaponBase
    {
        public const uint m_vecLastValidPlayerHeldPosition = 0xE28; // Vector
        public const uint m_vecLastValidDroppedPosition = 0xE34; // Vector
        public const uint m_bDoValidDroppedPositionCheck = 0xE40; // bool
        public const uint m_bStartedArming = 0xE41; // bool
        public const uint m_fArmedTime = 0xE44; // GameTime_t
        public const uint m_bBombPlacedAnimation = 0xE48; // bool
        public const uint m_bIsPlantingViaUse = 0xE49; // bool
        public const uint m_entitySpottedState = 0xE50; // EntitySpottedState_t
        public const uint m_nSpotRules = 0xE68; // int32_t
        public const uint m_bPlayedArmingBeeps = 0xE6C; // bool[7]
        public const uint m_bBombPlanted = 0xE73; // bool
        public const uint m_bDroppedFromDeath = 0xE74; // bool
    }

    public static class CCSBot  // CBot
    {
        public const uint m_lastCoopSpawnPoint = 0xF8; // CHandle<SpawnPointCoopEnemy>
        public const uint m_eyePosition = 0x108; // Vector
        public const uint m_name = 0x114; // char[64]
        public const uint m_combatRange = 0x154; // float
        public const uint m_isRogue = 0x158; // bool
        public const uint m_rogueTimer = 0x160; // CountdownTimer
        public const uint m_diedLastRound = 0x17C; // bool
        public const uint m_safeTime = 0x180; // float
        public const uint m_wasSafe = 0x184; // bool
        public const uint m_blindFire = 0x18C; // bool
        public const uint m_surpriseTimer = 0x190; // CountdownTimer
        public const uint m_bAllowActive = 0x1A8; // bool
        public const uint m_isFollowing = 0x1A9; // bool
        public const uint m_leader = 0x1AC; // CHandle<CCSPlayerPawn>
        public const uint m_followTimestamp = 0x1B0; // float
        public const uint m_allowAutoFollowTime = 0x1B4; // float
        public const uint m_hurryTimer = 0x1B8; // CountdownTimer
        public const uint m_alertTimer = 0x1D0; // CountdownTimer
        public const uint m_sneakTimer = 0x1E8; // CountdownTimer
        public const uint m_panicTimer = 0x200; // CountdownTimer
        public const uint m_stateTimestamp = 0x4D0; // float
        public const uint m_isAttacking = 0x4D4; // bool
        public const uint m_isOpeningDoor = 0x4D5; // bool
        public const uint m_taskEntity = 0x4DC; // CHandle<CBaseEntity>
        public const uint m_goalPosition = 0x4EC; // Vector
        public const uint m_goalEntity = 0x4F8; // CHandle<CBaseEntity>
        public const uint m_avoid = 0x4FC; // CHandle<CBaseEntity>
        public const uint m_avoidTimestamp = 0x500; // float
        public const uint m_isStopping = 0x504; // bool
        public const uint m_hasVisitedEnemySpawn = 0x505; // bool
        public const uint m_stillTimer = 0x508; // IntervalTimer
        public const uint m_bEyeAnglesUnderPathFinderControl = 0x518; // bool
        public const uint m_pathIndex = 0x6610; // int32_t
        public const uint m_areaEnteredTimestamp = 0x6614; // GameTime_t
        public const uint m_repathTimer = 0x6618; // CountdownTimer
        public const uint m_avoidFriendTimer = 0x6630; // CountdownTimer
        public const uint m_isFriendInTheWay = 0x6648; // bool
        public const uint m_politeTimer = 0x6650; // CountdownTimer
        public const uint m_isWaitingBehindFriend = 0x6668; // bool
        public const uint m_pathLadderEnd = 0x6694; // float
        public const uint m_mustRunTimer = 0x66E0; // CountdownTimer
        public const uint m_waitTimer = 0x66F8; // CountdownTimer
        public const uint m_updateTravelDistanceTimer = 0x6710; // CountdownTimer
        public const uint m_playerTravelDistance = 0x6728; // float[64]
        public const uint m_travelDistancePhase = 0x6828; // uint8_t
        public const uint m_hostageEscortCount = 0x69C0; // uint8_t
        public const uint m_hostageEscortCountTimestamp = 0x69C4; // float
        public const uint m_desiredTeam = 0x69C8; // int32_t
        public const uint m_hasJoined = 0x69CC; // bool
        public const uint m_isWaitingForHostage = 0x69CD; // bool
        public const uint m_inhibitWaitingForHostageTimer = 0x69D0; // CountdownTimer
        public const uint m_waitForHostageTimer = 0x69E8; // CountdownTimer
        public const uint m_noisePosition = 0x6A00; // Vector
        public const uint m_noiseTravelDistance = 0x6A0C; // float
        public const uint m_noiseTimestamp = 0x6A10; // float
        public const uint m_noiseSource = 0x6A18; // CCSPlayerPawn*
        public const uint m_noiseBendTimer = 0x6A30; // CountdownTimer
        public const uint m_bentNoisePosition = 0x6A48; // Vector
        public const uint m_bendNoisePositionValid = 0x6A54; // bool
        public const uint m_lookAroundStateTimestamp = 0x6A58; // float
        public const uint m_lookAheadAngle = 0x6A5C; // float
        public const uint m_forwardAngle = 0x6A60; // float
        public const uint m_inhibitLookAroundTimestamp = 0x6A64; // float
        public const uint m_lookAtSpot = 0x6A6C; // Vector
        public const uint m_lookAtSpotDuration = 0x6A7C; // float
        public const uint m_lookAtSpotTimestamp = 0x6A80; // float
        public const uint m_lookAtSpotAngleTolerance = 0x6A84; // float
        public const uint m_lookAtSpotClearIfClose = 0x6A88; // bool
        public const uint m_lookAtSpotAttack = 0x6A89; // bool
        public const uint m_lookAtDesc = 0x6A90; // char*
        public const uint m_peripheralTimestamp = 0x6A98; // float
        public const uint m_approachPointCount = 0x6C20; // uint8_t
        public const uint m_approachPointViewPosition = 0x6C24; // Vector
        public const uint m_viewSteadyTimer = 0x6C30; // IntervalTimer
        public const uint m_tossGrenadeTimer = 0x6C48; // CountdownTimer
        public const uint m_isAvoidingGrenade = 0x6C68; // CountdownTimer
        public const uint m_spotCheckTimestamp = 0x6C88; // float
        public const uint m_checkedHidingSpotCount = 0x7090; // int32_t
        public const uint m_lookPitch = 0x7094; // float
        public const uint m_lookPitchVel = 0x7098; // float
        public const uint m_lookYaw = 0x709C; // float
        public const uint m_lookYawVel = 0x70A0; // float
        public const uint m_targetSpot = 0x70A4; // Vector
        public const uint m_targetSpotVelocity = 0x70B0; // Vector
        public const uint m_targetSpotPredicted = 0x70BC; // Vector
        public const uint m_aimError = 0x70C8; // QAngle
        public const uint m_aimGoal = 0x70D4; // QAngle
        public const uint m_targetSpotTime = 0x70E0; // GameTime_t
        public const uint m_aimFocus = 0x70E4; // float
        public const uint m_aimFocusInterval = 0x70E8; // float
        public const uint m_aimFocusNextUpdate = 0x70EC; // GameTime_t
        public const uint m_ignoreEnemiesTimer = 0x70F8; // CountdownTimer
        public const uint m_enemy = 0x7110; // CHandle<CCSPlayerPawn>
        public const uint m_isEnemyVisible = 0x7114; // bool
        public const uint m_visibleEnemyParts = 0x7115; // uint8_t
        public const uint m_lastEnemyPosition = 0x7118; // Vector
        public const uint m_lastSawEnemyTimestamp = 0x7124; // float
        public const uint m_firstSawEnemyTimestamp = 0x7128; // float
        public const uint m_currentEnemyAcquireTimestamp = 0x712C; // float
        public const uint m_enemyDeathTimestamp = 0x7130; // float
        public const uint m_friendDeathTimestamp = 0x7134; // float
        public const uint m_isLastEnemyDead = 0x7138; // bool
        public const uint m_nearbyEnemyCount = 0x713C; // int32_t
        public const uint m_bomber = 0x7348; // CHandle<CCSPlayerPawn>
        public const uint m_nearbyFriendCount = 0x734C; // int32_t
        public const uint m_closestVisibleFriend = 0x7350; // CHandle<CCSPlayerPawn>
        public const uint m_closestVisibleHumanFriend = 0x7354; // CHandle<CCSPlayerPawn>
        public const uint m_attentionInterval = 0x7358; // IntervalTimer
        public const uint m_attacker = 0x7368; // CHandle<CCSPlayerPawn>
        public const uint m_attackedTimestamp = 0x736C; // float
        public const uint m_burnedByFlamesTimer = 0x7370; // IntervalTimer
        public const uint m_lastVictimID = 0x7380; // int32_t
        public const uint m_isAimingAtEnemy = 0x7384; // bool
        public const uint m_isRapidFiring = 0x7385; // bool
        public const uint m_equipTimer = 0x7388; // IntervalTimer
        public const uint m_zoomTimer = 0x7398; // CountdownTimer
        public const uint m_fireWeaponTimestamp = 0x73B0; // GameTime_t
        public const uint m_lookForWeaponsOnGroundTimer = 0x73B8; // CountdownTimer
        public const uint m_bIsSleeping = 0x73D0; // bool
        public const uint m_isEnemySniperVisible = 0x73D1; // bool
        public const uint m_sawEnemySniperTimer = 0x73D8; // CountdownTimer
        public const uint m_enemyQueueIndex = 0x7490; // uint8_t
        public const uint m_enemyQueueCount = 0x7491; // uint8_t
        public const uint m_enemyQueueAttendIndex = 0x7492; // uint8_t
        public const uint m_isStuck = 0x7493; // bool
        public const uint m_stuckTimestamp = 0x7494; // GameTime_t
        public const uint m_stuckSpot = 0x7498; // Vector
        public const uint m_wiggleTimer = 0x74A8; // CountdownTimer
        public const uint m_stuckJumpTimer = 0x74C0; // CountdownTimer
        public const uint m_nextCleanupCheckTimestamp = 0x74D8; // GameTime_t
        public const uint m_avgVel = 0x74DC; // float[10]
        public const uint m_avgVelIndex = 0x7504; // int32_t
        public const uint m_avgVelCount = 0x7508; // int32_t
        public const uint m_lastOrigin = 0x750C; // Vector
        public const uint m_lastRadioRecievedTimestamp = 0x751C; // float
        public const uint m_lastRadioSentTimestamp = 0x7520; // float
        public const uint m_radioSubject = 0x7524; // CHandle<CCSPlayerPawn>
        public const uint m_radioPosition = 0x7528; // Vector
        public const uint m_voiceEndTimestamp = 0x7534; // float
        public const uint m_lastValidReactionQueueFrame = 0x7540; // int32_t
    }

    public static class CCSGOPlayerAnimGraphState 
    {
    }

    public static class CCSGOViewModel  // CPredictedViewModel
    {
        public const uint m_bShouldIgnoreOffsetAndAccuracy = 0x8D8; // bool
        public const uint m_nWeaponParity = 0x8DC; // uint32_t
        public const uint m_nOldWeaponParity = 0x8E0; // uint32_t
    }

    public static class CCSGO_TeamIntroCharacterPosition  // CCSGO_TeamPreviewCharacterPosition
    {
    }

    public static class CCSGO_TeamIntroCounterTerroristPosition  // CCSGO_TeamIntroCharacterPosition
    {
    }

    public static class CCSGO_TeamIntroTerroristPosition  // CCSGO_TeamIntroCharacterPosition
    {
    }

    public static class CCSGO_TeamPreviewCharacterPosition  // CBaseEntity
    {
        public const uint m_nVariant = 0x4B0; // int32_t
        public const uint m_nRandom = 0x4B4; // int32_t
        public const uint m_nOrdinal = 0x4B8; // int32_t
        public const uint m_sWeaponName = 0x4C0; // CUtlString
        public const uint m_xuid = 0x4C8; // uint64_t
        public const uint m_agentItem = 0x4D0; // CEconItemView
        public const uint m_glovesItem = 0x748; // CEconItemView
        public const uint m_weaponItem = 0x9C0; // CEconItemView
    }

    public static class CCSGO_TeamSelectCharacterPosition  // CCSGO_TeamPreviewCharacterPosition
    {
    }

    public static class CCSGO_TeamSelectCounterTerroristPosition  // CCSGO_TeamSelectCharacterPosition
    {
    }

    public static class CCSGO_TeamSelectTerroristPosition  // CCSGO_TeamSelectCharacterPosition
    {
    }

    public static class CCSGO_WingmanIntroCharacterPosition  // CCSGO_TeamIntroCharacterPosition
    {
    }

    public static class CCSGO_WingmanIntroCounterTerroristPosition  // CCSGO_WingmanIntroCharacterPosition
    {
    }

    public static class CCSGO_WingmanIntroTerroristPosition  // CCSGO_WingmanIntroCharacterPosition
    {
    }

    public static class CCSGameModeRules 
    {
        public const uint __m_pChainEntity = 0x8; // CNetworkVarChainer
    }

    public static class CCSGameModeRules_Deathmatch  // CCSGameModeRules
    {
        public const uint m_bFirstThink = 0x30; // bool
        public const uint m_bFirstThinkAfterConnected = 0x31; // bool
        public const uint m_flDMBonusStartTime = 0x34; // GameTime_t
        public const uint m_flDMBonusTimeLength = 0x38; // float
        public const uint m_nDMBonusWeaponLoadoutSlot = 0x3C; // int16_t
    }

    public static class CCSGameModeRules_Noop  // CCSGameModeRules
    {
    }

    public static class CCSGameModeRules_Scripted  // CCSGameModeRules
    {
    }

    public static class CCSGameModeScript  // CBasePulseGraphInstance
    {
    }

    public static class CCSGameRules  // CTeamplayRules
    {
        public const uint __m_pChainEntity = 0x98; // CNetworkVarChainer
        public const uint m_coopMissionManager = 0xC0; // CHandle<CBaseEntity>
        public const uint m_bFreezePeriod = 0xC4; // bool
        public const uint m_bWarmupPeriod = 0xC5; // bool
        public const uint m_fWarmupPeriodEnd = 0xC8; // GameTime_t
        public const uint m_fWarmupPeriodStart = 0xCC; // GameTime_t
        public const uint m_nTotalPausedTicks = 0xD0; // int32_t
        public const uint m_nPauseStartTick = 0xD4; // int32_t
        public const uint m_bServerPaused = 0xD8; // bool
        public const uint m_bGamePaused = 0xD9; // bool
        public const uint m_bTerroristTimeOutActive = 0xDA; // bool
        public const uint m_bCTTimeOutActive = 0xDB; // bool
        public const uint m_flTerroristTimeOutRemaining = 0xDC; // float
        public const uint m_flCTTimeOutRemaining = 0xE0; // float
        public const uint m_nTerroristTimeOuts = 0xE4; // int32_t
        public const uint m_nCTTimeOuts = 0xE8; // int32_t
        public const uint m_bTechnicalTimeOut = 0xEC; // bool
        public const uint m_bMatchWaitingForResume = 0xED; // bool
        public const uint m_iRoundTime = 0xF0; // int32_t
        public const uint m_fMatchStartTime = 0xF4; // float
        public const uint m_fRoundStartTime = 0xF8; // GameTime_t
        public const uint m_flRestartRoundTime = 0xFC; // GameTime_t
        public const uint m_bGameRestart = 0x100; // bool
        public const uint m_flGameStartTime = 0x104; // float
        public const uint m_timeUntilNextPhaseStarts = 0x108; // float
        public const uint m_gamePhase = 0x10C; // int32_t
        public const uint m_totalRoundsPlayed = 0x110; // int32_t
        public const uint m_nRoundsPlayedThisPhase = 0x114; // int32_t
        public const uint m_nOvertimePlaying = 0x118; // int32_t
        public const uint m_iHostagesRemaining = 0x11C; // int32_t
        public const uint m_bAnyHostageReached = 0x120; // bool
        public const uint m_bMapHasBombTarget = 0x121; // bool
        public const uint m_bMapHasRescueZone = 0x122; // bool
        public const uint m_bMapHasBuyZone = 0x123; // bool
        public const uint m_bIsQueuedMatchmaking = 0x124; // bool
        public const uint m_nQueuedMatchmakingMode = 0x128; // int32_t
        public const uint m_bIsValveDS = 0x12C; // bool
        public const uint m_bLogoMap = 0x12D; // bool
        public const uint m_bPlayAllStepSoundsOnServer = 0x12E; // bool
        public const uint m_iSpectatorSlotCount = 0x130; // int32_t
        public const uint m_MatchDevice = 0x134; // int32_t
        public const uint m_bHasMatchStarted = 0x138; // bool
        public const uint m_nNextMapInMapgroup = 0x13C; // int32_t
        public const uint m_szTournamentEventName = 0x140; // char[512]
        public const uint m_szTournamentEventStage = 0x340; // char[512]
        public const uint m_szMatchStatTxt = 0x540; // char[512]
        public const uint m_szTournamentPredictionsTxt = 0x740; // char[512]
        public const uint m_nTournamentPredictionsPct = 0x940; // int32_t
        public const uint m_flCMMItemDropRevealStartTime = 0x944; // GameTime_t
        public const uint m_flCMMItemDropRevealEndTime = 0x948; // GameTime_t
        public const uint m_bIsDroppingItems = 0x94C; // bool
        public const uint m_bIsQuestEligible = 0x94D; // bool
        public const uint m_bIsHltvActive = 0x94E; // bool
        public const uint m_nGuardianModeWaveNumber = 0x950; // int32_t
        public const uint m_nGuardianModeSpecialKillsRemaining = 0x954; // int32_t
        public const uint m_nGuardianModeSpecialWeaponNeeded = 0x958; // int32_t
        public const uint m_nGuardianGrenadesToGiveBots = 0x95C; // int32_t
        public const uint m_nNumHeaviesToSpawn = 0x960; // int32_t
        public const uint m_numGlobalGiftsGiven = 0x964; // uint32_t
        public const uint m_numGlobalGifters = 0x968; // uint32_t
        public const uint m_numGlobalGiftsPeriodSeconds = 0x96C; // uint32_t
        public const uint m_arrFeaturedGiftersAccounts = 0x970; // uint32_t[4]
        public const uint m_arrFeaturedGiftersGifts = 0x980; // uint32_t[4]
        public const uint m_arrProhibitedItemIndices = 0x990; // uint16_t[100]
        public const uint m_arrTournamentActiveCasterAccounts = 0xA58; // uint32_t[4]
        public const uint m_numBestOfMaps = 0xA68; // int32_t
        public const uint m_nHalloweenMaskListSeed = 0xA6C; // int32_t
        public const uint m_bBombDropped = 0xA70; // bool
        public const uint m_bBombPlanted = 0xA71; // bool
        public const uint m_iRoundWinStatus = 0xA74; // int32_t
        public const uint m_eRoundWinReason = 0xA78; // int32_t
        public const uint m_bTCantBuy = 0xA7C; // bool
        public const uint m_bCTCantBuy = 0xA7D; // bool
        public const uint m_flGuardianBuyUntilTime = 0xA80; // GameTime_t
        public const uint m_iMatchStats_RoundResults = 0xA84; // int32_t[30]
        public const uint m_iMatchStats_PlayersAlive_CT = 0xAFC; // int32_t[30]
        public const uint m_iMatchStats_PlayersAlive_T = 0xB74; // int32_t[30]
        public const uint m_TeamRespawnWaveTimes = 0xBEC; // float[32]
        public const uint m_flNextRespawnWave = 0xC6C; // GameTime_t[32]
        public const uint m_nServerQuestID = 0xCEC; // int32_t
        public const uint m_vMinimapMins = 0xCF0; // Vector
        public const uint m_vMinimapMaxs = 0xCFC; // Vector
        public const uint m_MinimapVerticalSectionHeights = 0xD08; // float[8]
        public const uint m_bDontIncrementCoopWave = 0xD28; // bool
        public const uint m_bSpawnedTerrorHuntHeavy = 0xD29; // bool
        public const uint m_nEndMatchMapGroupVoteTypes = 0xD2C; // int32_t[10]
        public const uint m_nEndMatchMapGroupVoteOptions = 0xD54; // int32_t[10]
        public const uint m_nEndMatchMapVoteWinner = 0xD7C; // int32_t
        public const uint m_iNumConsecutiveCTLoses = 0xD80; // int32_t
        public const uint m_iNumConsecutiveTerroristLoses = 0xD84; // int32_t
        public const uint m_bHasHostageBeenTouched = 0xDA0; // bool
        public const uint m_flIntermissionStartTime = 0xDA4; // GameTime_t
        public const uint m_flIntermissionEndTime = 0xDA8; // GameTime_t
        public const uint m_bLevelInitialized = 0xDAC; // bool
        public const uint m_iTotalRoundsPlayed = 0xDB0; // int32_t
        public const uint m_iUnBalancedRounds = 0xDB4; // int32_t
        public const uint m_endMatchOnRoundReset = 0xDB8; // bool
        public const uint m_endMatchOnThink = 0xDB9; // bool
        public const uint m_iFreezeTime = 0xDBC; // int32_t
        public const uint m_iNumTerrorist = 0xDC0; // int32_t
        public const uint m_iNumCT = 0xDC4; // int32_t
        public const uint m_iNumSpawnableTerrorist = 0xDC8; // int32_t
        public const uint m_iNumSpawnableCT = 0xDCC; // int32_t
        public const uint m_arrSelectedHostageSpawnIndices = 0xDD0; // CUtlVector<int32_t>
        public const uint m_bFirstConnected = 0xDE8; // bool
        public const uint m_bCompleteReset = 0xDE9; // bool
        public const uint m_bPickNewTeamsOnReset = 0xDEA; // bool
        public const uint m_bScrambleTeamsOnRestart = 0xDEB; // bool
        public const uint m_bSwapTeamsOnRestart = 0xDEC; // bool
        public const uint m_nEndMatchTiedVotes = 0xDF8; // CUtlVector<int32_t>
        public const uint m_bNeedToAskPlayersForContinueVote = 0xE14; // bool
        public const uint m_numQueuedMatchmakingAccounts = 0xE18; // uint32_t
        public const uint m_pQueuedMatchmakingReservationString = 0xE20; // char*
        public const uint m_numTotalTournamentDrops = 0xE28; // uint32_t
        public const uint m_numSpectatorsCountMax = 0xE2C; // uint32_t
        public const uint m_numSpectatorsCountMaxTV = 0xE30; // uint32_t
        public const uint m_numSpectatorsCountMaxLnk = 0xE34; // uint32_t
        public const uint m_bForceTeamChangeSilent = 0xE40; // bool
        public const uint m_bLoadingRoundBackupData = 0xE41; // bool
        public const uint m_nMatchInfoShowType = 0xE78; // int32_t
        public const uint m_flMatchInfoDecidedTime = 0xE7C; // float
        public const uint m_flCoopRespawnAndHealTime = 0xE98; // float
        public const uint m_coopBonusCoinsFound = 0xE9C; // int32_t
        public const uint m_coopBonusPistolsOnly = 0xEA0; // bool
        public const uint m_coopPlayersInDeploymentZone = 0xEA1; // bool
        public const uint m_coopMissionDeadPlayerRespawnEnabled = 0xEA2; // bool
        public const uint mTeamDMLastWinningTeamNumber = 0xEA4; // int32_t
        public const uint mTeamDMLastThinkTime = 0xEA8; // float
        public const uint m_flTeamDMLastAnnouncementTime = 0xEAC; // float
        public const uint m_iAccountTerrorist = 0xEB0; // int32_t
        public const uint m_iAccountCT = 0xEB4; // int32_t
        public const uint m_iSpawnPointCount_Terrorist = 0xEB8; // int32_t
        public const uint m_iSpawnPointCount_CT = 0xEBC; // int32_t
        public const uint m_iMaxNumTerrorists = 0xEC0; // int32_t
        public const uint m_iMaxNumCTs = 0xEC4; // int32_t
        public const uint m_iLoserBonus = 0xEC8; // int32_t
        public const uint m_iLoserBonusMostRecentTeam = 0xECC; // int32_t
        public const uint m_tmNextPeriodicThink = 0xED0; // float
        public const uint m_bVoiceWonMatchBragFired = 0xED4; // bool
        public const uint m_fWarmupNextChatNoticeTime = 0xED8; // float
        public const uint m_iHostagesRescued = 0xEE0; // int32_t
        public const uint m_iHostagesTouched = 0xEE4; // int32_t
        public const uint m_flNextHostageAnnouncement = 0xEE8; // float
        public const uint m_bNoTerroristsKilled = 0xEEC; // bool
        public const uint m_bNoCTsKilled = 0xEED; // bool
        public const uint m_bNoEnemiesKilled = 0xEEE; // bool
        public const uint m_bCanDonateWeapons = 0xEEF; // bool
        public const uint m_firstKillTime = 0xEF4; // float
        public const uint m_firstBloodTime = 0xEFC; // float
        public const uint m_hostageWasInjured = 0xF18; // bool
        public const uint m_hostageWasKilled = 0xF19; // bool
        public const uint m_bVoteCalled = 0xF28; // bool
        public const uint m_bServerVoteOnReset = 0xF29; // bool
        public const uint m_flVoteCheckThrottle = 0xF2C; // float
        public const uint m_bBuyTimeEnded = 0xF30; // bool
        public const uint m_nLastFreezeEndBeep = 0xF34; // int32_t
        public const uint m_bTargetBombed = 0xF38; // bool
        public const uint m_bBombDefused = 0xF39; // bool
        public const uint m_bMapHasBombZone = 0xF3A; // bool
        public const uint m_vecMainCTSpawnPos = 0xF58; // Vector
        public const uint m_CTSpawnPointsMasterList = 0xF68; // CUtlVector<SpawnPoint*>
        public const uint m_TerroristSpawnPointsMasterList = 0xF80; // CUtlVector<SpawnPoint*>
        public const uint m_iNextCTSpawnPoint = 0xF98; // int32_t
        public const uint m_iNextTerroristSpawnPoint = 0xF9C; // int32_t
        public const uint m_CTSpawnPoints = 0xFA0; // CUtlVector<SpawnPoint*>
        public const uint m_TerroristSpawnPoints = 0xFB8; // CUtlVector<SpawnPoint*>
        public const uint m_bIsUnreservedGameServer = 0xFD0; // bool
        public const uint m_fAutobalanceDisplayTime = 0xFD4; // float
        public const uint m_bAllowWeaponSwitch = 0x1240; // bool
        public const uint m_bRoundTimeWarningTriggered = 0x1241; // bool
        public const uint m_phaseChangeAnnouncementTime = 0x1244; // GameTime_t
        public const uint m_fNextUpdateTeamClanNamesTime = 0x1248; // float
        public const uint m_flLastThinkTime = 0x124C; // GameTime_t
        public const uint m_fAccumulatedRoundOffDamage = 0x1250; // float
        public const uint m_nShorthandedBonusLastEvalRound = 0x1254; // int32_t
        public const uint m_nMatchAbortedEarlyReason = 0x14D0; // int32_t
        public const uint m_bHasTriggeredRoundStartMusic = 0x14D4; // bool
        public const uint m_bHasTriggeredCoopSpawnReset = 0x14D5; // bool
        public const uint m_bSwitchingTeamsAtRoundReset = 0x14D6; // bool
        public const uint m_pGameModeRules = 0x14F0; // CCSGameModeRules*
        public const uint m_BtGlobalBlackboard = 0x14F8; // KeyValues3
        public const uint m_hPlayerResource = 0x1560; // CHandle<CBaseEntity>
        public const uint m_RetakeRules = 0x1568; // CRetakeGameRules
        public const uint m_GuardianBotSkillLevelMax = 0x174C; // int32_t
        public const uint m_GuardianBotSkillLevelMin = 0x1750; // int32_t
        public const uint m_arrTeamUniqueKillWeaponsMatch = 0x1758; // CUtlVector<int32_t>[4]
        public const uint m_bTeamLastKillUsedUniqueWeaponMatch = 0x17B8; // bool[4]
        public const uint m_nMatchEndCount = 0x17E0; // uint8_t
        public const uint m_nTTeamIntroVariant = 0x17E4; // int32_t
        public const uint m_nCTTeamIntroVariant = 0x17E8; // int32_t
        public const uint m_bTeamIntroPeriod = 0x17EC; // bool
        public const uint m_fTeamIntroPeriodEnd = 0x17F0; // GameTime_t
        public const uint m_bPlayedTeamIntroVO = 0x17F4; // bool
        public const uint m_flLastPerfSampleTime = 0x5800; // double
        public const uint m_bSkipNextServerPerfSample = 0x5808; // bool
    }

    public static class CCSGameRulesProxy  // CGameRulesProxy
    {
        public const uint m_pGameRules = 0x4B0; // CCSGameRules*
    }

    public static class CCSMinimapBoundary  // CBaseEntity
    {
    }

    public static class CCSObserverPawn  // CCSPlayerPawnBase
    {
    }

    public static class CCSObserver_CameraServices  // CCSPlayerBase_CameraServices
    {
    }

    public static class CCSObserver_MovementServices  // CPlayer_MovementServices
    {
    }

    public static class CCSObserver_ObserverServices  // CPlayer_ObserverServices
    {
    }

    public static class CCSObserver_UseServices  // CPlayer_UseServices
    {
    }

    public static class CCSObserver_ViewModelServices  // CPlayer_ViewModelServices
    {
    }

    public static class CCSPlace  // CServerOnlyModelEntity
    {
        public const uint m_name = 0x708; // CUtlSymbolLarge
    }

    public static class CCSPlayerBase_CameraServices  // CPlayer_CameraServices
    {
        public const uint m_iFOV = 0x170; // uint32_t
        public const uint m_iFOVStart = 0x174; // uint32_t
        public const uint m_flFOVTime = 0x178; // GameTime_t
        public const uint m_flFOVRate = 0x17C; // float
        public const uint m_hZoomOwner = 0x180; // CHandle<CBaseEntity>
        public const uint m_hTriggerFogList = 0x188; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_hLastFogTrigger = 0x1A0; // CHandle<CBaseEntity>
    }

    public static class CCSPlayerController  // CBasePlayerController
    {
        public const uint m_pInGameMoneyServices = 0x6B8; // CCSPlayerController_InGameMoneyServices*
        public const uint m_pInventoryServices = 0x6C0; // CCSPlayerController_InventoryServices*
        public const uint m_pActionTrackingServices = 0x6C8; // CCSPlayerController_ActionTrackingServices*
        public const uint m_pDamageServices = 0x6D0; // CCSPlayerController_DamageServices*
        public const uint m_iPing = 0x6D8; // uint32_t
        public const uint m_bHasCommunicationAbuseMute = 0x6DC; // bool
        public const uint m_szCrosshairCodes = 0x6E0; // CUtlSymbolLarge
        public const uint m_iPendingTeamNum = 0x6E8; // uint8_t
        public const uint m_flForceTeamTime = 0x6EC; // GameTime_t
        public const uint m_iCompTeammateColor = 0x6F0; // int32_t
        public const uint m_bEverPlayedOnTeam = 0x6F4; // bool
        public const uint m_bAttemptedToGetColor = 0x6F5; // bool
        public const uint m_iTeammatePreferredColor = 0x6F8; // int32_t
        public const uint m_bTeamChanged = 0x6FC; // bool
        public const uint m_bInSwitchTeam = 0x6FD; // bool
        public const uint m_bHasSeenJoinGame = 0x6FE; // bool
        public const uint m_bJustBecameSpectator = 0x6FF; // bool
        public const uint m_bSwitchTeamsOnNextRoundReset = 0x700; // bool
        public const uint m_bRemoveAllItemsOnNextRoundReset = 0x701; // bool
        public const uint m_szClan = 0x708; // CUtlSymbolLarge
        public const uint m_szClanName = 0x710; // char[32]
        public const uint m_iCoachingTeam = 0x730; // int32_t
        public const uint m_nPlayerDominated = 0x738; // uint64_t
        public const uint m_nPlayerDominatingMe = 0x740; // uint64_t
        public const uint m_iCompetitiveRanking = 0x748; // int32_t
        public const uint m_iCompetitiveWins = 0x74C; // int32_t
        public const uint m_iCompetitiveRankType = 0x750; // int8_t
        public const uint m_iCompetitiveRankingPredicted_Win = 0x754; // int32_t
        public const uint m_iCompetitiveRankingPredicted_Loss = 0x758; // int32_t
        public const uint m_iCompetitiveRankingPredicted_Tie = 0x75C; // int32_t
        public const uint m_nEndMatchNextMapVote = 0x760; // int32_t
        public const uint m_unActiveQuestId = 0x764; // uint16_t
        public const uint m_nQuestProgressReason = 0x768; // QuestProgress::Reason
        public const uint m_unPlayerTvControlFlags = 0x76C; // uint32_t
        public const uint m_iDraftIndex = 0x798; // int32_t
        public const uint m_msQueuedModeDisconnectionTimestamp = 0x79C; // uint32_t
        public const uint m_uiAbandonRecordedReason = 0x7A0; // uint32_t
        public const uint m_bCannotBeKicked = 0x7A4; // bool
        public const uint m_bEverFullyConnected = 0x7A5; // bool
        public const uint m_bAbandonAllowsSurrender = 0x7A6; // bool
        public const uint m_bAbandonOffersInstantSurrender = 0x7A7; // bool
        public const uint m_bDisconnection1MinWarningPrinted = 0x7A8; // bool
        public const uint m_bScoreReported = 0x7A9; // bool
        public const uint m_nDisconnectionTick = 0x7AC; // int32_t
        public const uint m_bControllingBot = 0x7B8; // bool
        public const uint m_bHasControlledBotThisRound = 0x7B9; // bool
        public const uint m_bHasBeenControlledByPlayerThisRound = 0x7BA; // bool
        public const uint m_nBotsControlledThisRound = 0x7BC; // int32_t
        public const uint m_bCanControlObservedBot = 0x7C0; // bool
        public const uint m_hPlayerPawn = 0x7C4; // CHandle<CCSPlayerPawn>
        public const uint m_hObserverPawn = 0x7C8; // CHandle<CCSObserverPawn>
        public const uint m_DesiredObserverMode = 0x7CC; // int32_t
        public const uint m_hDesiredObserverTarget = 0x7D0; // CEntityHandle
        public const uint m_bPawnIsAlive = 0x7D4; // bool
        public const uint m_iPawnHealth = 0x7D8; // uint32_t
        public const uint m_iPawnArmor = 0x7DC; // int32_t
        public const uint m_bPawnHasDefuser = 0x7E0; // bool
        public const uint m_bPawnHasHelmet = 0x7E1; // bool
        public const uint m_nPawnCharacterDefIndex = 0x7E2; // uint16_t
        public const uint m_iPawnLifetimeStart = 0x7E4; // int32_t
        public const uint m_iPawnLifetimeEnd = 0x7E8; // int32_t
        public const uint m_iPawnBotDifficulty = 0x7EC; // int32_t
        public const uint m_hOriginalControllerOfCurrentPawn = 0x7F0; // CHandle<CCSPlayerController>
        public const uint m_iScore = 0x7F4; // int32_t
        public const uint m_iRoundScore = 0x7F8; // int32_t
        public const uint m_iRoundsWon = 0x7FC; // int32_t
        public const uint m_vecKills = 0x800; // CNetworkUtlVectorBase<EKillTypes_t>
        public const uint m_iMVPs = 0x818; // int32_t
        public const uint m_nUpdateCounter = 0x81C; // int32_t
        public const uint m_flSmoothedPing = 0x820; // float
        public const uint m_lastHeldVoteTimer = 0xF8C8; // IntervalTimer
        public const uint m_bShowHints = 0xF8E0; // bool
        public const uint m_iNextTimeCheck = 0xF8E4; // int32_t
        public const uint m_bJustDidTeamKill = 0xF8E8; // bool
        public const uint m_bPunishForTeamKill = 0xF8E9; // bool
        public const uint m_bGaveTeamDamageWarning = 0xF8EA; // bool
        public const uint m_bGaveTeamDamageWarningThisRound = 0xF8EB; // bool
        public const uint m_dblLastReceivedPacketPlatFloatTime = 0xF8F0; // double
        public const uint m_LastTeamDamageWarningTime = 0xF8F8; // GameTime_t
        public const uint m_LastTimePlayerWasDisconnectedForPawnsRemove = 0xF8FC; // GameTime_t
    }

    public static class CCSPlayerController_ActionTrackingServices  // CPlayerControllerComponent
    {
        public const uint m_perRoundStats = 0x40; // CUtlVectorEmbeddedNetworkVar<CSPerRoundStats_t>
        public const uint m_matchStats = 0x90; // CSMatchStats_t
        public const uint m_iNumRoundKills = 0x148; // int32_t
        public const uint m_iNumRoundKillsHeadshots = 0x14C; // int32_t
        public const uint m_unTotalRoundDamageDealt = 0x150; // uint32_t
    }

    public static class CCSPlayerController_DamageServices  // CPlayerControllerComponent
    {
        public const uint m_nSendUpdate = 0x40; // int32_t
        public const uint m_DamageList = 0x48; // CUtlVectorEmbeddedNetworkVar<CDamageRecord>
    }

    public static class CCSPlayerController_InGameMoneyServices  // CPlayerControllerComponent
    {
        public const uint m_bReceivesMoneyNextRound = 0x40; // bool
        public const uint m_iAccountMoneyEarnedForNextRound = 0x44; // int32_t
        public const uint m_iAccount = 0x48; // int32_t
        public const uint m_iStartAccount = 0x4C; // int32_t
        public const uint m_iTotalCashSpent = 0x50; // int32_t
        public const uint m_iCashSpentThisRound = 0x54; // int32_t
    }

    public static class CCSPlayerController_InventoryServices  // CPlayerControllerComponent
    {
        public const uint m_unMusicID = 0x40; // uint16_t
        public const uint m_rank = 0x44; // MedalRank_t[6]
        public const uint m_nPersonaDataPublicLevel = 0x5C; // int32_t
        public const uint m_nPersonaDataPublicCommendsLeader = 0x60; // int32_t
        public const uint m_nPersonaDataPublicCommendsTeacher = 0x64; // int32_t
        public const uint m_nPersonaDataPublicCommendsFriendly = 0x68; // int32_t
        public const uint m_unEquippedPlayerSprayIDs = 0xF48; // uint32_t[1]
        public const uint m_vecServerAuthoritativeWeaponSlots = 0xF50; // CUtlVectorEmbeddedNetworkVar<ServerAuthoritativeWeaponSlot_t>
    }

    public static class CCSPlayerPawn  // CCSPlayerPawnBase
    {
        public const uint m_pBulletServices = 0x1550; // CCSPlayer_BulletServices*
        public const uint m_pHostageServices = 0x1558; // CCSPlayer_HostageServices*
        public const uint m_pBuyServices = 0x1560; // CCSPlayer_BuyServices*
        public const uint m_pActionTrackingServices = 0x1568; // CCSPlayer_ActionTrackingServices*
        public const uint m_pRadioServices = 0x1570; // CCSPlayer_RadioServices*
        public const uint m_pDamageReactServices = 0x1578; // CCSPlayer_DamageReactServices*
        public const uint m_nCharacterDefIndex = 0x1580; // uint16_t
        public const uint m_hPreviousModel = 0x1588; // CStrongHandle<InfoForResourceTypeCModel>
        public const uint m_bHasFemaleVoice = 0x1590; // bool
        public const uint m_strVOPrefix = 0x1598; // CUtlString
        public const uint m_szLastPlaceName = 0x15A0; // char[18]
        public const uint m_bInHostageResetZone = 0x1660; // bool
        public const uint m_bInBuyZone = 0x1661; // bool
        public const uint m_bWasInBuyZone = 0x1662; // bool
        public const uint m_bInHostageRescueZone = 0x1663; // bool
        public const uint m_bInBombZone = 0x1664; // bool
        public const uint m_bWasInHostageRescueZone = 0x1665; // bool
        public const uint m_iRetakesOffering = 0x1668; // int32_t
        public const uint m_iRetakesOfferingCard = 0x166C; // int32_t
        public const uint m_bRetakesHasDefuseKit = 0x1670; // bool
        public const uint m_bRetakesMVPLastRound = 0x1671; // bool
        public const uint m_iRetakesMVPBoostItem = 0x1674; // int32_t
        public const uint m_RetakesMVPBoostExtraUtility = 0x1678; // loadout_slot_t
        public const uint m_flHealthShotBoostExpirationTime = 0x167C; // GameTime_t
        public const uint m_flLandseconds = 0x1680; // float
        public const uint m_aimPunchAngle = 0x1684; // QAngle
        public const uint m_aimPunchAngleVel = 0x1690; // QAngle
        public const uint m_aimPunchTickBase = 0x169C; // int32_t
        public const uint m_aimPunchTickFraction = 0x16A0; // float
        public const uint m_aimPunchCache = 0x16A8; // CUtlVector<QAngle>
        public const uint m_bIsBuyMenuOpen = 0x16C0; // bool
        public const uint m_xLastHeadBoneTransform = 0x1CF0; // CTransform
        public const uint m_bLastHeadBoneTransformIsValid = 0x1D10; // bool
        public const uint m_lastLandTime = 0x1D14; // GameTime_t
        public const uint m_bOnGroundLastTick = 0x1D18; // bool
        public const uint m_iPlayerLocked = 0x1D1C; // int32_t
        public const uint m_flTimeOfLastInjury = 0x1D24; // GameTime_t
        public const uint m_flNextSprayDecalTime = 0x1D28; // GameTime_t
        public const uint m_bNextSprayDecalTimeExpedited = 0x1D2C; // bool
        public const uint m_nRagdollDamageBone = 0x1D30; // int32_t
        public const uint m_vRagdollDamageForce = 0x1D34; // Vector
        public const uint m_vRagdollDamagePosition = 0x1D40; // Vector
        public const uint m_szRagdollDamageWeaponName = 0x1D4C; // char[64]
        public const uint m_bRagdollDamageHeadshot = 0x1D8C; // bool
        public const uint m_vRagdollServerOrigin = 0x1D90; // Vector
        public const uint m_EconGloves = 0x1DA0; // CEconItemView
        public const uint m_qDeathEyeAngles = 0x2018; // QAngle
        public const uint m_bSkipOneHeadConstraintUpdate = 0x2024; // bool
    }

    public static class CCSPlayerPawnBase  // CBasePlayerPawn
    {
        public const uint m_CTouchExpansionComponent = 0xB68; // CTouchExpansionComponent
        public const uint m_pPingServices = 0xBB8; // CCSPlayer_PingServices*
        public const uint m_pViewModelServices = 0xBC0; // CPlayer_ViewModelServices*
        public const uint m_iDisplayHistoryBits = 0xBC8; // uint32_t
        public const uint m_flLastAttackedTeammate = 0xBCC; // float
        public const uint m_hOriginalController = 0xBD0; // CHandle<CCSPlayerController>
        public const uint m_blindUntilTime = 0xBD4; // GameTime_t
        public const uint m_blindStartTime = 0xBD8; // GameTime_t
        public const uint m_allowAutoFollowTime = 0xBDC; // GameTime_t
        public const uint m_entitySpottedState = 0xBE0; // EntitySpottedState_t
        public const uint m_nSpotRules = 0xBF8; // int32_t
        public const uint m_iPlayerState = 0xBFC; // CSPlayerState
        public const uint m_chickenIdleSoundTimer = 0xC08; // CountdownTimer
        public const uint m_chickenJumpSoundTimer = 0xC20; // CountdownTimer
        public const uint m_vecLastBookmarkedPosition = 0xCD8; // Vector
        public const uint m_flLastDistanceTraveledNotice = 0xCE4; // float
        public const uint m_flAccumulatedDistanceTraveled = 0xCE8; // float
        public const uint m_flLastFriendlyFireDamageReductionRatio = 0xCEC; // float
        public const uint m_bRespawning = 0xCF0; // bool
        public const uint m_nLastPickupPriority = 0xCF4; // int32_t
        public const uint m_flLastPickupPriorityTime = 0xCF8; // float
        public const uint m_bIsScoped = 0xCFC; // bool
        public const uint m_bIsWalking = 0xCFD; // bool
        public const uint m_bResumeZoom = 0xCFE; // bool
        public const uint m_bIsDefusing = 0xCFF; // bool
        public const uint m_bIsGrabbingHostage = 0xD00; // bool
        public const uint m_iBlockingUseActionInProgress = 0xD04; // CSPlayerBlockingUseAction_t
        public const uint m_fImmuneToGunGameDamageTime = 0xD08; // GameTime_t
        public const uint m_bGunGameImmunity = 0xD0C; // bool
        public const uint m_fMolotovDamageTime = 0xD10; // float
        public const uint m_bHasMovedSinceSpawn = 0xD14; // bool
        public const uint m_bCanMoveDuringFreezePeriod = 0xD15; // bool
        public const uint m_flGuardianTooFarDistFrac = 0xD18; // float
        public const uint m_flNextGuardianTooFarHurtTime = 0xD1C; // float
        public const uint m_flDetectedByEnemySensorTime = 0xD20; // GameTime_t
        public const uint m_flDealtDamageToEnemyMostRecentTimestamp = 0xD24; // float
        public const uint m_flLastEquippedHelmetTime = 0xD28; // GameTime_t
        public const uint m_flLastEquippedArmorTime = 0xD2C; // GameTime_t
        public const uint m_nHeavyAssaultSuitCooldownRemaining = 0xD30; // int32_t
        public const uint m_bResetArmorNextSpawn = 0xD34; // bool
        public const uint m_flLastBumpMineBumpTime = 0xD38; // GameTime_t
        public const uint m_flEmitSoundTime = 0xD3C; // GameTime_t
        public const uint m_iNumSpawns = 0xD40; // int32_t
        public const uint m_iShouldHaveCash = 0xD44; // int32_t
        public const uint m_bInvalidSteamLogonDelayed = 0xD48; // bool
        public const uint m_flLastAction = 0xD4C; // GameTime_t
        public const uint m_flNameChangeHistory = 0xD50; // float[5]
        public const uint m_fLastGivenDefuserTime = 0xD64; // float
        public const uint m_fLastGivenBombTime = 0xD68; // float
        public const uint m_bHasNightVision = 0xD6C; // bool
        public const uint m_bNightVisionOn = 0xD6D; // bool
        public const uint m_fNextRadarUpdateTime = 0xD70; // float
        public const uint m_flLastMoneyUpdateTime = 0xD74; // float
        public const uint m_MenuStringBuffer = 0xD78; // char[1024]
        public const uint m_fIntroCamTime = 0x1178; // float
        public const uint m_nMyCollisionGroup = 0x117C; // int32_t
        public const uint m_bInNoDefuseArea = 0x1180; // bool
        public const uint m_bKilledByTaser = 0x1181; // bool
        public const uint m_iMoveState = 0x1184; // int32_t
        public const uint m_grenadeParameterStashTime = 0x1188; // GameTime_t
        public const uint m_bGrenadeParametersStashed = 0x118C; // bool
        public const uint m_angStashedShootAngles = 0x1190; // QAngle
        public const uint m_vecStashedGrenadeThrowPosition = 0x119C; // Vector
        public const uint m_vecStashedVelocity = 0x11A8; // Vector
        public const uint m_angShootAngleHistory = 0x11B4; // QAngle[2]
        public const uint m_vecThrowPositionHistory = 0x11CC; // Vector[2]
        public const uint m_vecVelocityHistory = 0x11E4; // Vector[2]
        public const uint m_bDiedAirborne = 0x11FC; // bool
        public const uint m_iBombSiteIndex = 0x1200; // CEntityIndex
        public const uint m_nWhichBombZone = 0x1204; // int32_t
        public const uint m_bInBombZoneTrigger = 0x1208; // bool
        public const uint m_bWasInBombZoneTrigger = 0x1209; // bool
        public const uint m_iDirection = 0x120C; // int32_t
        public const uint m_iShotsFired = 0x1210; // int32_t
        public const uint m_ArmorValue = 0x1214; // int32_t
        public const uint m_flFlinchStack = 0x1218; // float
        public const uint m_flVelocityModifier = 0x121C; // float
        public const uint m_flHitHeading = 0x1220; // float
        public const uint m_nHitBodyPart = 0x1224; // int32_t
        public const uint m_iHostagesKilled = 0x1228; // int32_t
        public const uint m_vecTotalBulletForce = 0x122C; // Vector
        public const uint m_flFlashDuration = 0x1238; // float
        public const uint m_flFlashMaxAlpha = 0x123C; // float
        public const uint m_flProgressBarStartTime = 0x1240; // float
        public const uint m_iProgressBarDuration = 0x1244; // int32_t
        public const uint m_bWaitForNoAttack = 0x1248; // bool
        public const uint m_flLowerBodyYawTarget = 0x124C; // float
        public const uint m_bStrafing = 0x1250; // bool
        public const uint m_lastStandingPos = 0x1254; // Vector
        public const uint m_ignoreLadderJumpTime = 0x1260; // float
        public const uint m_ladderSurpressionTimer = 0x1268; // CountdownTimer
        public const uint m_lastLadderNormal = 0x1280; // Vector
        public const uint m_lastLadderPos = 0x128C; // Vector
        public const uint m_thirdPersonHeading = 0x1298; // QAngle
        public const uint m_flSlopeDropOffset = 0x12A4; // float
        public const uint m_flSlopeDropHeight = 0x12A8; // float
        public const uint m_vHeadConstraintOffset = 0x12AC; // Vector
        public const uint m_iLastWeaponFireUsercmd = 0x12C0; // int32_t
        public const uint m_angEyeAngles = 0x12C4; // QAngle
        public const uint m_bVCollisionInitted = 0x12D0; // bool
        public const uint m_storedSpawnPosition = 0x12D4; // Vector
        public const uint m_storedSpawnAngle = 0x12E0; // QAngle
        public const uint m_bIsSpawning = 0x12EC; // bool
        public const uint m_bHideTargetID = 0x12ED; // bool
        public const uint m_nNumDangerZoneDamageHits = 0x12F0; // int32_t
        public const uint m_bHud_MiniScoreHidden = 0x12F4; // bool
        public const uint m_bHud_RadarHidden = 0x12F5; // bool
        public const uint m_nLastKillerIndex = 0x12F8; // CEntityIndex
        public const uint m_nLastConcurrentKilled = 0x12FC; // int32_t
        public const uint m_nDeathCamMusic = 0x1300; // int32_t
        public const uint m_iAddonBits = 0x1304; // int32_t
        public const uint m_iPrimaryAddon = 0x1308; // int32_t
        public const uint m_iSecondaryAddon = 0x130C; // int32_t
        public const uint m_currentDeafnessFilter = 0x1310; // CUtlStringToken
        public const uint m_NumEnemiesKilledThisSpawn = 0x1314; // int32_t
        public const uint m_NumEnemiesKilledThisRound = 0x1318; // int32_t
        public const uint m_NumEnemiesAtRoundStart = 0x131C; // int32_t
        public const uint m_wasNotKilledNaturally = 0x1320; // bool
        public const uint m_vecPlayerPatchEconIndices = 0x1324; // uint32_t[5]
        public const uint m_iDeathFlags = 0x1338; // int32_t
        public const uint m_hPet = 0x133C; // CHandle<CChicken>
        public const uint m_unCurrentEquipmentValue = 0x1508; // uint16_t
        public const uint m_unRoundStartEquipmentValue = 0x150A; // uint16_t
        public const uint m_unFreezetimeEndEquipmentValue = 0x150C; // uint16_t
        public const uint m_nSurvivalTeamNumber = 0x1510; // int32_t
        public const uint m_bHasDeathInfo = 0x1514; // bool
        public const uint m_flDeathInfoTime = 0x1518; // float
        public const uint m_vecDeathInfoOrigin = 0x151C; // Vector
        public const uint m_bKilledByHeadshot = 0x1528; // bool
        public const uint m_LastHitBox = 0x152C; // int32_t
        public const uint m_LastHealth = 0x1530; // int32_t
        public const uint m_flLastCollisionCeiling = 0x1534; // float
        public const uint m_flLastCollisionCeilingChangeTime = 0x1538; // float
        public const uint m_pBot = 0x1540; // CCSBot*
        public const uint m_bBotAllowActive = 0x1548; // bool
        public const uint m_bCommittingSuicideOnTeamChange = 0x1549; // bool
    }

    public static class CCSPlayerResource  // CBaseEntity
    {
        public const uint m_bHostageAlive = 0x4B0; // bool[12]
        public const uint m_isHostageFollowingSomeone = 0x4BC; // bool[12]
        public const uint m_iHostageEntityIDs = 0x4C8; // CEntityIndex[12]
        public const uint m_bombsiteCenterA = 0x4F8; // Vector
        public const uint m_bombsiteCenterB = 0x504; // Vector
        public const uint m_hostageRescueX = 0x510; // int32_t[4]
        public const uint m_hostageRescueY = 0x520; // int32_t[4]
        public const uint m_hostageRescueZ = 0x530; // int32_t[4]
        public const uint m_bEndMatchNextMapAllVoted = 0x540; // bool
        public const uint m_foundGoalPositions = 0x541; // bool
    }

    public static class CCSPlayer_ActionTrackingServices  // CPlayerPawnComponent
    {
        public const uint m_hLastWeaponBeforeC4AutoSwitch = 0x208; // CHandle<CBasePlayerWeapon>
        public const uint m_bIsRescuing = 0x23C; // bool
        public const uint m_weaponPurchasesThisMatch = 0x240; // WeaponPurchaseTracker_t
        public const uint m_weaponPurchasesThisRound = 0x298; // WeaponPurchaseTracker_t
    }

    public static class CCSPlayer_BulletServices  // CPlayerPawnComponent
    {
        public const uint m_totalHitsOnServer = 0x40; // int32_t
    }

    public static class CCSPlayer_BuyServices  // CPlayerPawnComponent
    {
        public const uint m_vecSellbackPurchaseEntries = 0xC8; // CUtlVectorEmbeddedNetworkVar<SellbackPurchaseEntry_t>
    }

    public static class CCSPlayer_CameraServices  // CCSPlayerBase_CameraServices
    {
    }

    public static class CCSPlayer_DamageReactServices  // CPlayerPawnComponent
    {
    }

    public static class CCSPlayer_HostageServices  // CPlayerPawnComponent
    {
        public const uint m_hCarriedHostage = 0x40; // CHandle<CBaseEntity>
        public const uint m_hCarriedHostageProp = 0x44; // CHandle<CBaseEntity>
    }

    public static class CCSPlayer_ItemServices  // CPlayer_ItemServices
    {
        public const uint m_bHasDefuser = 0x40; // bool
        public const uint m_bHasHelmet = 0x41; // bool
        public const uint m_bHasHeavyArmor = 0x42; // bool
    }

    public static class CCSPlayer_MovementServices  // CPlayer_MovementServices_Humanoid
    {
        public const uint m_flMaxFallVelocity = 0x220; // float
        public const uint m_vecLadderNormal = 0x224; // Vector
        public const uint m_nLadderSurfacePropIndex = 0x230; // int32_t
        public const uint m_flDuckAmount = 0x234; // float
        public const uint m_flDuckSpeed = 0x238; // float
        public const uint m_bDuckOverride = 0x23C; // bool
        public const uint m_bDesiresDuck = 0x23D; // bool
        public const uint m_flDuckOffset = 0x240; // float
        public const uint m_nDuckTimeMsecs = 0x244; // uint32_t
        public const uint m_nDuckJumpTimeMsecs = 0x248; // uint32_t
        public const uint m_nJumpTimeMsecs = 0x24C; // uint32_t
        public const uint m_flLastDuckTime = 0x250; // float
        public const uint m_vecLastPositionAtFullCrouchSpeed = 0x260; // Vector2D
        public const uint m_duckUntilOnGround = 0x268; // bool
        public const uint m_bHasWalkMovedSinceLastJump = 0x269; // bool
        public const uint m_bInStuckTest = 0x26A; // bool
        public const uint m_flStuckCheckTime = 0x278; // float[64][2]
        public const uint m_nTraceCount = 0x478; // int32_t
        public const uint m_StuckLast = 0x47C; // int32_t
        public const uint m_bSpeedCropped = 0x480; // bool
        public const uint m_nOldWaterLevel = 0x484; // int32_t
        public const uint m_flWaterEntryTime = 0x488; // float
        public const uint m_vecForward = 0x48C; // Vector
        public const uint m_vecLeft = 0x498; // Vector
        public const uint m_vecUp = 0x4A4; // Vector
        public const uint m_vecPreviouslyPredictedOrigin = 0x4B0; // Vector
        public const uint m_bMadeFootstepNoise = 0x4BC; // bool
        public const uint m_iFootsteps = 0x4C0; // int32_t
        public const uint m_bOldJumpPressed = 0x4C4; // bool
        public const uint m_flJumpPressedTime = 0x4C8; // float
        public const uint m_flJumpUntil = 0x4CC; // float
        public const uint m_flJumpVel = 0x4D0; // float
        public const uint m_fStashGrenadeParameterWhen = 0x4D4; // GameTime_t
        public const uint m_nButtonDownMaskPrev = 0x4D8; // uint64_t
        public const uint m_flOffsetTickCompleteTime = 0x4E0; // float
        public const uint m_flOffsetTickStashedSpeed = 0x4E4; // float
        public const uint m_flStamina = 0x4E8; // float
        public const uint m_flHeightAtJumpStart = 0x4EC; // float
        public const uint m_flMaxJumpHeightThisJump = 0x4F0; // float
    }

    public static class CCSPlayer_PingServices  // CPlayerPawnComponent
    {
        public const uint m_flPlayerPingTokens = 0x40; // GameTime_t[5]
        public const uint m_hPlayerPing = 0x54; // CHandle<CBaseEntity>
    }

    public static class CCSPlayer_RadioServices  // CPlayerPawnComponent
    {
        public const uint m_flGotHostageTalkTimer = 0x40; // GameTime_t
        public const uint m_flDefusingTalkTimer = 0x44; // GameTime_t
        public const uint m_flC4PlantTalkTimer = 0x48; // GameTime_t
        public const uint m_flRadioTokenSlots = 0x4C; // GameTime_t[3]
        public const uint m_bIgnoreRadio = 0x58; // bool
    }

    public static class CCSPlayer_UseServices  // CPlayer_UseServices
    {
        public const uint m_hLastKnownUseEntity = 0x40; // CHandle<CBaseEntity>
        public const uint m_flLastUseTimeStamp = 0x44; // GameTime_t
        public const uint m_flTimeStartedHoldingUse = 0x48; // GameTime_t
        public const uint m_flTimeLastUsedWindow = 0x4C; // GameTime_t
    }

    public static class CCSPlayer_ViewModelServices  // CPlayer_ViewModelServices
    {
        public const uint m_hViewModel = 0x40; // CHandle<CBaseViewModel>[3]
    }

    public static class CCSPlayer_WaterServices  // CPlayer_WaterServices
    {
        public const uint m_NextDrownDamageTime = 0x40; // float
        public const uint m_nDrownDmgRate = 0x44; // int32_t
        public const uint m_AirFinishedTime = 0x48; // GameTime_t
        public const uint m_flWaterJumpTime = 0x4C; // float
        public const uint m_vecWaterJumpVel = 0x50; // Vector
        public const uint m_flSwimSoundTime = 0x5C; // float
    }

    public static class CCSPlayer_WeaponServices  // CPlayer_WeaponServices
    {
        public const uint m_flNextAttack = 0xB0; // GameTime_t
        public const uint m_bIsLookingAtWeapon = 0xB4; // bool
        public const uint m_bIsHoldingLookAtWeapon = 0xB5; // bool
        public const uint m_hSavedWeapon = 0xB8; // CHandle<CBasePlayerWeapon>
        public const uint m_nTimeToMelee = 0xBC; // int32_t
        public const uint m_nTimeToSecondary = 0xC0; // int32_t
        public const uint m_nTimeToPrimary = 0xC4; // int32_t
        public const uint m_nTimeToSniperRifle = 0xC8; // int32_t
        public const uint m_bIsBeingGivenItem = 0xCC; // bool
        public const uint m_bIsPickingUpItemWithUse = 0xCD; // bool
        public const uint m_bPickedUpWeapon = 0xCE; // bool
    }

    public static class CCSPulseServerFuncs_Globals 
    {
    }

    public static class CCSSprite  // CSprite
    {
    }

    public static class CCSTeam  // CTeam
    {
        public const uint m_nLastRecievedShorthandedRoundBonus = 0x568; // int32_t
        public const uint m_nShorthandedRoundBonusStartRound = 0x56C; // int32_t
        public const uint m_bSurrendered = 0x570; // bool
        public const uint m_szTeamMatchStat = 0x571; // char[512]
        public const uint m_numMapVictories = 0x774; // int32_t
        public const uint m_scoreFirstHalf = 0x778; // int32_t
        public const uint m_scoreSecondHalf = 0x77C; // int32_t
        public const uint m_scoreOvertime = 0x780; // int32_t
        public const uint m_szClanTeamname = 0x784; // char[129]
        public const uint m_iClanID = 0x808; // uint32_t
        public const uint m_szTeamFlagImage = 0x80C; // char[8]
        public const uint m_szTeamLogoImage = 0x814; // char[8]
        public const uint m_flNextResourceTime = 0x81C; // float
        public const uint m_iLastUpdateSentAt = 0x820; // int32_t
    }

    public static class CCSWeaponBase  // CBasePlayerWeapon
    {
        public const uint m_bRemoveable = 0xC88; // bool
        public const uint m_flFireSequenceStartTime = 0xC90; // float
        public const uint m_nFireSequenceStartTimeChange = 0xC94; // int32_t
        public const uint m_nFireSequenceStartTimeAck = 0xC98; // int32_t
        public const uint m_ePlayerFireEvent = 0xC9C; // PlayerAnimEvent_t
        public const uint m_ePlayerFireEventAttackType = 0xCA0; // WeaponAttackType_t
        public const uint m_seqIdle = 0xCA4; // HSequence
        public const uint m_seqFirePrimary = 0xCA8; // HSequence
        public const uint m_seqFireSecondary = 0xCAC; // HSequence
        public const uint m_thirdPersonFireSequences = 0xCB0; // CUtlVector<HSequence>
        public const uint m_hCurrentThirdPersonSequence = 0xCC8; // HSequence
        public const uint m_nSilencerBoneIndex = 0xCCC; // int32_t
        public const uint m_thirdPersonSequences = 0xCD0; // HSequence[6]
        public const uint m_bPlayerAmmoStockOnPickup = 0xCF0; // bool
        public const uint m_bRequireUseToTouch = 0xCF1; // bool
        public const uint m_iState = 0xCF4; // CSWeaponState_t
        public const uint m_flLastTimeInAir = 0xCF8; // GameTime_t
        public const uint m_flLastDeployTime = 0xCFC; // GameTime_t
        public const uint m_nViewModelIndex = 0xD00; // uint32_t
        public const uint m_bReloadsWithClips = 0xD04; // bool
        public const uint m_flTimeWeaponIdle = 0xD20; // GameTime_t
        public const uint m_bFireOnEmpty = 0xD24; // bool
        public const uint m_OnPlayerPickup = 0xD28; // CEntityIOOutput
        public const uint m_weaponMode = 0xD50; // CSWeaponMode
        public const uint m_flTurningInaccuracyDelta = 0xD54; // float
        public const uint m_vecTurningInaccuracyEyeDirLast = 0xD58; // Vector
        public const uint m_flTurningInaccuracy = 0xD64; // float
        public const uint m_fAccuracyPenalty = 0xD68; // float
        public const uint m_flLastAccuracyUpdateTime = 0xD6C; // GameTime_t
        public const uint m_fAccuracySmoothedForZoom = 0xD70; // float
        public const uint m_fScopeZoomEndTime = 0xD74; // GameTime_t
        public const uint m_iRecoilIndex = 0xD78; // int32_t
        public const uint m_flRecoilIndex = 0xD7C; // float
        public const uint m_bBurstMode = 0xD80; // bool
        public const uint m_nPostponeFireReadyTicks = 0xD84; // GameTick_t
        public const uint m_flPostponeFireReadyFrac = 0xD88; // float
        public const uint m_bInReload = 0xD8C; // bool
        public const uint m_bReloadVisuallyComplete = 0xD8D; // bool
        public const uint m_flDroppedAtTime = 0xD90; // GameTime_t
        public const uint m_bIsHauledBack = 0xD94; // bool
        public const uint m_bSilencerOn = 0xD95; // bool
        public const uint m_flTimeSilencerSwitchComplete = 0xD98; // GameTime_t
        public const uint m_iOriginalTeamNumber = 0xD9C; // int32_t
        public const uint m_flNextAttackRenderTimeOffset = 0xDA0; // float
        public const uint m_bCanBePickedUp = 0xDB8; // bool
        public const uint m_bUseCanOverrideNextOwnerTouchTime = 0xDB9; // bool
        public const uint m_nextOwnerTouchTime = 0xDBC; // GameTime_t
        public const uint m_nextPrevOwnerTouchTime = 0xDC0; // GameTime_t
        public const uint m_hPrevOwner = 0xDC4; // CHandle<CCSPlayerPawn>
        public const uint m_nDropTick = 0xDC8; // GameTick_t
        public const uint m_donated = 0xDEC; // bool
        public const uint m_fLastShotTime = 0xDF0; // GameTime_t
        public const uint m_bWasOwnedByCT = 0xDF4; // bool
        public const uint m_bWasOwnedByTerrorist = 0xDF5; // bool
        public const uint m_bFiredOutOfAmmoEvent = 0xDF6; // bool
        public const uint m_numRemoveUnownedWeaponThink = 0xDF8; // int32_t
        public const uint m_IronSightController = 0xE00; // CIronSightController
        public const uint m_iIronSightMode = 0xE18; // int32_t
        public const uint m_flLastLOSTraceFailureTime = 0xE1C; // GameTime_t
        public const uint m_iNumEmptyAttacks = 0xE20; // int32_t
    }

    public static class CCSWeaponBaseGun  // CCSWeaponBase
    {
        public const uint m_zoomLevel = 0xE28; // int32_t
        public const uint m_iBurstShotsRemaining = 0xE2C; // int32_t
        public const uint m_silencedModelIndex = 0xE38; // int32_t
        public const uint m_inPrecache = 0xE3C; // bool
        public const uint m_bNeedsBoltAction = 0xE3D; // bool
        public const uint m_bSkillReloadAvailable = 0xE3E; // bool
        public const uint m_bSkillReloadLiftedReloadKey = 0xE3F; // bool
        public const uint m_bSkillBoltInterruptAvailable = 0xE40; // bool
        public const uint m_bSkillBoltLiftedFireKey = 0xE41; // bool
    }

    public static class CCSWeaponBaseVData  // CBasePlayerWeaponVData
    {
        public const uint m_WeaponType = 0x240; // CSWeaponType
        public const uint m_WeaponCategory = 0x244; // CSWeaponCategory
        public const uint m_szViewModel = 0x248; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_szPlayerModel = 0x328; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_szWorldDroppedModel = 0x408; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_szAimsightLensMaskModel = 0x4E8; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_szMagazineModel = 0x5C8; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeCModel>>
        public const uint m_szHeatEffect = 0x6A8; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_szEjectBrassEffect = 0x788; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_szMuzzleFlashParticleAlt = 0x868; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_szMuzzleFlashThirdPersonParticle = 0x948; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_szMuzzleFlashThirdPersonParticleAlt = 0xA28; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_szTracerParticle = 0xB08; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_GearSlot = 0xBE8; // gear_slot_t
        public const uint m_GearSlotPosition = 0xBEC; // int32_t
        public const uint m_DefaultLoadoutSlot = 0xBF0; // loadout_slot_t
        public const uint m_sWrongTeamMsg = 0xBF8; // CUtlString
        public const uint m_nPrice = 0xC00; // int32_t
        public const uint m_nKillAward = 0xC04; // int32_t
        public const uint m_nPrimaryReserveAmmoMax = 0xC08; // int32_t
        public const uint m_nSecondaryReserveAmmoMax = 0xC0C; // int32_t
        public const uint m_bMeleeWeapon = 0xC10; // bool
        public const uint m_bHasBurstMode = 0xC11; // bool
        public const uint m_bIsRevolver = 0xC12; // bool
        public const uint m_bCannotShootUnderwater = 0xC13; // bool
        public const uint m_szName = 0xC18; // CUtlString
        public const uint m_szAnimExtension = 0xC20; // CUtlString
        public const uint m_eSilencerType = 0xC28; // CSWeaponSilencerType
        public const uint m_nCrosshairMinDistance = 0xC2C; // int32_t
        public const uint m_nCrosshairDeltaDistance = 0xC30; // int32_t
        public const uint m_flCycleTime = 0xC34; // CFiringModeFloat
        public const uint m_flMaxSpeed = 0xC3C; // CFiringModeFloat
        public const uint m_flSpread = 0xC44; // CFiringModeFloat
        public const uint m_flInaccuracyCrouch = 0xC4C; // CFiringModeFloat
        public const uint m_flInaccuracyStand = 0xC54; // CFiringModeFloat
        public const uint m_flInaccuracyJump = 0xC5C; // CFiringModeFloat
        public const uint m_flInaccuracyLand = 0xC64; // CFiringModeFloat
        public const uint m_flInaccuracyLadder = 0xC6C; // CFiringModeFloat
        public const uint m_flInaccuracyFire = 0xC74; // CFiringModeFloat
        public const uint m_flInaccuracyMove = 0xC7C; // CFiringModeFloat
        public const uint m_flRecoilAngle = 0xC84; // CFiringModeFloat
        public const uint m_flRecoilAngleVariance = 0xC8C; // CFiringModeFloat
        public const uint m_flRecoilMagnitude = 0xC94; // CFiringModeFloat
        public const uint m_flRecoilMagnitudeVariance = 0xC9C; // CFiringModeFloat
        public const uint m_nTracerFrequency = 0xCA4; // CFiringModeInt
        public const uint m_flInaccuracyJumpInitial = 0xCAC; // float
        public const uint m_flInaccuracyJumpApex = 0xCB0; // float
        public const uint m_flInaccuracyReload = 0xCB4; // float
        public const uint m_nRecoilSeed = 0xCB8; // int32_t
        public const uint m_nSpreadSeed = 0xCBC; // int32_t
        public const uint m_flTimeToIdleAfterFire = 0xCC0; // float
        public const uint m_flIdleInterval = 0xCC4; // float
        public const uint m_flAttackMovespeedFactor = 0xCC8; // float
        public const uint m_flHeatPerShot = 0xCCC; // float
        public const uint m_flInaccuracyPitchShift = 0xCD0; // float
        public const uint m_flInaccuracyAltSoundThreshold = 0xCD4; // float
        public const uint m_flBotAudibleRange = 0xCD8; // float
        public const uint m_szUseRadioSubtitle = 0xCE0; // CUtlString
        public const uint m_bUnzoomsAfterShot = 0xCE8; // bool
        public const uint m_bHideViewModelWhenZoomed = 0xCE9; // bool
        public const uint m_nZoomLevels = 0xCEC; // int32_t
        public const uint m_nZoomFOV1 = 0xCF0; // int32_t
        public const uint m_nZoomFOV2 = 0xCF4; // int32_t
        public const uint m_flZoomTime0 = 0xCF8; // float
        public const uint m_flZoomTime1 = 0xCFC; // float
        public const uint m_flZoomTime2 = 0xD00; // float
        public const uint m_flIronSightPullUpSpeed = 0xD04; // float
        public const uint m_flIronSightPutDownSpeed = 0xD08; // float
        public const uint m_flIronSightFOV = 0xD0C; // float
        public const uint m_flIronSightPivotForward = 0xD10; // float
        public const uint m_flIronSightLooseness = 0xD14; // float
        public const uint m_angPivotAngle = 0xD18; // QAngle
        public const uint m_vecIronSightEyePos = 0xD24; // Vector
        public const uint m_nDamage = 0xD30; // int32_t
        public const uint m_flHeadshotMultiplier = 0xD34; // float
        public const uint m_flArmorRatio = 0xD38; // float
        public const uint m_flPenetration = 0xD3C; // float
        public const uint m_flRange = 0xD40; // float
        public const uint m_flRangeModifier = 0xD44; // float
        public const uint m_flFlinchVelocityModifierLarge = 0xD48; // float
        public const uint m_flFlinchVelocityModifierSmall = 0xD4C; // float
        public const uint m_flRecoveryTimeCrouch = 0xD50; // float
        public const uint m_flRecoveryTimeStand = 0xD54; // float
        public const uint m_flRecoveryTimeCrouchFinal = 0xD58; // float
        public const uint m_flRecoveryTimeStandFinal = 0xD5C; // float
        public const uint m_nRecoveryTransitionStartBullet = 0xD60; // int32_t
        public const uint m_nRecoveryTransitionEndBullet = 0xD64; // int32_t
        public const uint m_flThrowVelocity = 0xD68; // float
        public const uint m_vSmokeColor = 0xD6C; // Vector
        public const uint m_szAnimClass = 0xD78; // CUtlString
    }

    public static class CChangeLevel  // CBaseTrigger
    {
        public const uint m_sMapName = 0x8A8; // CUtlString
        public const uint m_sLandmarkName = 0x8B0; // CUtlString
        public const uint m_OnChangeLevel = 0x8B8; // CEntityIOOutput
        public const uint m_bTouched = 0x8E0; // bool
        public const uint m_bNoTouch = 0x8E1; // bool
        public const uint m_bNewChapter = 0x8E2; // bool
        public const uint m_bOnChangeLevelFired = 0x8E3; // bool
    }

    public static class CChicken  // CDynamicProp
    {
        public const uint m_AttributeManager = 0xB28; // CAttributeContainer
        public const uint m_OriginalOwnerXuidLow = 0xDF0; // uint32_t
        public const uint m_OriginalOwnerXuidHigh = 0xDF4; // uint32_t
        public const uint m_updateTimer = 0xDF8; // CountdownTimer
        public const uint m_stuckAnchor = 0xE10; // Vector
        public const uint m_stuckTimer = 0xE20; // CountdownTimer
        public const uint m_collisionStuckTimer = 0xE38; // CountdownTimer
        public const uint m_isOnGround = 0xE50; // bool
        public const uint m_vFallVelocity = 0xE54; // Vector
        public const uint m_activity = 0xE60; // ChickenActivity
        public const uint m_activityTimer = 0xE68; // CountdownTimer
        public const uint m_turnRate = 0xE80; // float
        public const uint m_fleeFrom = 0xE84; // CHandle<CBaseEntity>
        public const uint m_moveRateThrottleTimer = 0xE88; // CountdownTimer
        public const uint m_startleTimer = 0xEA0; // CountdownTimer
        public const uint m_vocalizeTimer = 0xEB8; // CountdownTimer
        public const uint m_flWhenZombified = 0xED0; // GameTime_t
        public const uint m_jumpedThisFrame = 0xED4; // bool
        public const uint m_leader = 0xED8; // CHandle<CCSPlayerPawn>
        public const uint m_reuseTimer = 0xEE0; // CountdownTimer
        public const uint m_hasBeenUsed = 0xEF8; // bool
        public const uint m_jumpTimer = 0xF00; // CountdownTimer
        public const uint m_flLastJumpTime = 0xF18; // float
        public const uint m_bInJump = 0xF1C; // bool
        public const uint m_isWaitingForLeader = 0xF1D; // bool
        public const uint m_repathTimer = 0x2F28; // CountdownTimer
        public const uint m_inhibitDoorTimer = 0x2F40; // CountdownTimer
        public const uint m_inhibitObstacleAvoidanceTimer = 0x2FD0; // CountdownTimer
        public const uint m_vecPathGoal = 0x2FF0; // Vector
        public const uint m_flActiveFollowStartTime = 0x2FFC; // GameTime_t
        public const uint m_followMinuteTimer = 0x3000; // CountdownTimer
        public const uint m_vecLastEggPoopPosition = 0x3018; // Vector
        public const uint m_vecEggsPooped = 0x3028; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_BlockDirectionTimer = 0x3048; // CountdownTimer
    }

    public static class CCollisionProperty 
    {
        public const uint m_collisionAttribute = 0x10; // VPhysicsCollisionAttribute_t
        public const uint m_vecMins = 0x40; // Vector
        public const uint m_vecMaxs = 0x4C; // Vector
        public const uint m_usSolidFlags = 0x5A; // uint8_t
        public const uint m_nSolidType = 0x5B; // SolidType_t
        public const uint m_triggerBloat = 0x5C; // uint8_t
        public const uint m_nSurroundType = 0x5D; // SurroundingBoundsType_t
        public const uint m_CollisionGroup = 0x5E; // uint8_t
        public const uint m_nEnablePhysics = 0x5F; // uint8_t
        public const uint m_flBoundingRadius = 0x60; // float
        public const uint m_vecSpecifiedSurroundingMins = 0x64; // Vector
        public const uint m_vecSpecifiedSurroundingMaxs = 0x70; // Vector
        public const uint m_vecSurroundingMaxs = 0x7C; // Vector
        public const uint m_vecSurroundingMins = 0x88; // Vector
        public const uint m_vCapsuleCenter1 = 0x94; // Vector
        public const uint m_vCapsuleCenter2 = 0xA0; // Vector
        public const uint m_flCapsuleRadius = 0xAC; // float
    }

    public static class CColorCorrection  // CBaseEntity
    {
        public const uint m_flFadeInDuration = 0x4B0; // float
        public const uint m_flFadeOutDuration = 0x4B4; // float
        public const uint m_flStartFadeInWeight = 0x4B8; // float
        public const uint m_flStartFadeOutWeight = 0x4BC; // float
        public const uint m_flTimeStartFadeIn = 0x4C0; // GameTime_t
        public const uint m_flTimeStartFadeOut = 0x4C4; // GameTime_t
        public const uint m_flMaxWeight = 0x4C8; // float
        public const uint m_bStartDisabled = 0x4CC; // bool
        public const uint m_bEnabled = 0x4CD; // bool
        public const uint m_bMaster = 0x4CE; // bool
        public const uint m_bClientSide = 0x4CF; // bool
        public const uint m_bExclusive = 0x4D0; // bool
        public const uint m_MinFalloff = 0x4D4; // float
        public const uint m_MaxFalloff = 0x4D8; // float
        public const uint m_flCurWeight = 0x4DC; // float
        public const uint m_netlookupFilename = 0x4E0; // char[512]
        public const uint m_lookupFilename = 0x6E0; // CUtlSymbolLarge
    }

    public static class CColorCorrectionVolume  // CBaseTrigger
    {
        public const uint m_bEnabled = 0x8A8; // bool
        public const uint m_MaxWeight = 0x8AC; // float
        public const uint m_FadeDuration = 0x8B0; // float
        public const uint m_bStartDisabled = 0x8B4; // bool
        public const uint m_Weight = 0x8B8; // float
        public const uint m_lookupFilename = 0x8BC; // char[512]
        public const uint m_LastEnterWeight = 0xABC; // float
        public const uint m_LastEnterTime = 0xAC0; // GameTime_t
        public const uint m_LastExitWeight = 0xAC4; // float
        public const uint m_LastExitTime = 0xAC8; // GameTime_t
    }

    public static class CCommentaryAuto  // CBaseEntity
    {
        public const uint m_OnCommentaryNewGame = 0x4B0; // CEntityIOOutput
        public const uint m_OnCommentaryMidGame = 0x4D8; // CEntityIOOutput
        public const uint m_OnCommentaryMultiplayerSpawn = 0x500; // CEntityIOOutput
    }

    public static class CCommentarySystem 
    {
        public const uint m_bCommentaryConvarsChanging = 0x11; // bool
        public const uint m_bCommentaryEnabledMidGame = 0x12; // bool
        public const uint m_flNextTeleportTime = 0x14; // GameTime_t
        public const uint m_iTeleportStage = 0x18; // int32_t
        public const uint m_bCheatState = 0x1C; // bool
        public const uint m_bIsFirstSpawnGroupToLoad = 0x1D; // bool
        public const uint m_hCurrentNode = 0x38; // CHandle<CPointCommentaryNode>
        public const uint m_hActiveCommentaryNode = 0x3C; // CHandle<CPointCommentaryNode>
        public const uint m_hLastCommentaryNode = 0x40; // CHandle<CPointCommentaryNode>
        public const uint m_vecNodes = 0x48; // CUtlVector<CHandle<CPointCommentaryNode>>
    }

    public static class CCommentaryViewPosition  // CSprite
    {
    }

    public static class CConstantForceController 
    {
        public const uint m_linear = 0xC; // Vector
        public const uint m_angular = 0x18; // RotationVector
        public const uint m_linearSave = 0x24; // Vector
        public const uint m_angularSave = 0x30; // RotationVector
    }

    public static class CConstraintAnchor  // CBaseAnimGraph
    {
        public const uint m_massScale = 0x890; // float
    }

    public static class CCoopBonusCoin  // CDynamicProp
    {
    }

    public static class CCopyRecipientFilter 
    {
        public const uint m_Flags = 0x8; // int32_t
        public const uint m_Recipients = 0x10; // CUtlVector<CPlayerSlot>
    }

    public static class CCredits  // CPointEntity
    {
        public const uint m_OnCreditsDone = 0x4B0; // CEntityIOOutput
        public const uint m_bRolledOutroCredits = 0x4D8; // bool
        public const uint m_flLogoLength = 0x4DC; // float
    }

    public static class CDEagle  // CCSWeaponBaseGun
    {
    }

    public static class CDamageRecord 
    {
        public const uint m_PlayerDamager = 0x28; // CHandle<CCSPlayerPawnBase>
        public const uint m_PlayerRecipient = 0x2C; // CHandle<CCSPlayerPawnBase>
        public const uint m_hPlayerControllerDamager = 0x30; // CHandle<CCSPlayerController>
        public const uint m_hPlayerControllerRecipient = 0x34; // CHandle<CCSPlayerController>
        public const uint m_szPlayerDamagerName = 0x38; // CUtlString
        public const uint m_szPlayerRecipientName = 0x40; // CUtlString
        public const uint m_DamagerXuid = 0x48; // uint64_t
        public const uint m_RecipientXuid = 0x50; // uint64_t
        public const uint m_iDamage = 0x58; // int32_t
        public const uint m_iActualHealthRemoved = 0x5C; // int32_t
        public const uint m_iNumHits = 0x60; // int32_t
        public const uint m_iLastBulletUpdate = 0x64; // int32_t
        public const uint m_bIsOtherEnemy = 0x68; // bool
        public const uint m_killType = 0x69; // EKillTypes_t
    }

    public static class CDebugHistory  // CBaseEntity
    {
        public const uint m_nNpcEvents = 0x44F0; // int32_t
    }

    public static class CDecoyGrenade  // CBaseCSGrenade
    {
    }

    public static class CDecoyProjectile  // CBaseCSGrenadeProjectile
    {
        public const uint m_nDecoyShotTick = 0xA48; // int32_t
        public const uint m_shotsRemaining = 0xA4C; // int32_t
        public const uint m_fExpireTime = 0xA50; // GameTime_t
        public const uint m_decoyWeaponDefIndex = 0xA60; // uint16_t
    }

    public static class CDynamicLight  // CBaseModelEntity
    {
        public const uint m_ActualFlags = 0x700; // uint8_t
        public const uint m_Flags = 0x701; // uint8_t
        public const uint m_LightStyle = 0x702; // uint8_t
        public const uint m_On = 0x703; // bool
        public const uint m_Radius = 0x704; // float
        public const uint m_Exponent = 0x708; // int32_t
        public const uint m_InnerAngle = 0x70C; // float
        public const uint m_OuterAngle = 0x710; // float
        public const uint m_SpotRadius = 0x714; // float
    }

    public static class CDynamicProp  // CBreakableProp
    {
        public const uint m_bCreateNavObstacle = 0xA10; // bool
        public const uint m_bUseHitboxesForRenderBox = 0xA11; // bool
        public const uint m_bUseAnimGraph = 0xA12; // bool
        public const uint m_pOutputAnimBegun = 0xA18; // CEntityIOOutput
        public const uint m_pOutputAnimOver = 0xA40; // CEntityIOOutput
        public const uint m_pOutputAnimLoopCycleOver = 0xA68; // CEntityIOOutput
        public const uint m_OnAnimReachedStart = 0xA90; // CEntityIOOutput
        public const uint m_OnAnimReachedEnd = 0xAB8; // CEntityIOOutput
        public const uint m_iszDefaultAnim = 0xAE0; // CUtlSymbolLarge
        public const uint m_nDefaultAnimLoopMode = 0xAE8; // AnimLoopMode_t
        public const uint m_bAnimateOnServer = 0xAEC; // bool
        public const uint m_bRandomizeCycle = 0xAED; // bool
        public const uint m_bStartDisabled = 0xAEE; // bool
        public const uint m_bScriptedMovement = 0xAEF; // bool
        public const uint m_bFiredStartEndOutput = 0xAF0; // bool
        public const uint m_bForceNpcExclude = 0xAF1; // bool
        public const uint m_bCreateNonSolid = 0xAF2; // bool
        public const uint m_bIsOverrideProp = 0xAF3; // bool
        public const uint m_iInitialGlowState = 0xAF4; // int32_t
        public const uint m_nGlowRange = 0xAF8; // int32_t
        public const uint m_nGlowRangeMin = 0xAFC; // int32_t
        public const uint m_glowColor = 0xB00; // Color
        public const uint m_nGlowTeam = 0xB04; // int32_t
    }

    public static class CDynamicPropAlias_cable_dynamic  // CDynamicProp
    {
    }

    public static class CDynamicPropAlias_dynamic_prop  // CDynamicProp
    {
    }

    public static class CDynamicPropAlias_prop_dynamic_override  // CDynamicProp
    {
    }

    public static class CEconEntity  // CBaseFlex
    {
        public const uint m_AttributeManager = 0x930; // CAttributeContainer
        public const uint m_OriginalOwnerXuidLow = 0xBF8; // uint32_t
        public const uint m_OriginalOwnerXuidHigh = 0xBFC; // uint32_t
        public const uint m_nFallbackPaintKit = 0xC00; // int32_t
        public const uint m_nFallbackSeed = 0xC04; // int32_t
        public const uint m_flFallbackWear = 0xC08; // float
        public const uint m_nFallbackStatTrak = 0xC0C; // int32_t
        public const uint m_hOldProvidee = 0xC10; // CHandle<CBaseEntity>
        public const uint m_iOldOwnerClass = 0xC14; // int32_t
    }

    public static class CEconItemAttribute 
    {
        public const uint m_iAttributeDefinitionIndex = 0x30; // uint16_t
        public const uint m_flValue = 0x34; // float
        public const uint m_flInitialValue = 0x38; // float
        public const uint m_nRefundableCurrency = 0x3C; // int32_t
        public const uint m_bSetBonus = 0x40; // bool
    }

    public static class CEconItemView  // IEconItemInterface
    {
        public const uint m_iItemDefinitionIndex = 0x38; // uint16_t
        public const uint m_iEntityQuality = 0x3C; // int32_t
        public const uint m_iEntityLevel = 0x40; // uint32_t
        public const uint m_iItemID = 0x48; // uint64_t
        public const uint m_iItemIDHigh = 0x50; // uint32_t
        public const uint m_iItemIDLow = 0x54; // uint32_t
        public const uint m_iAccountID = 0x58; // uint32_t
        public const uint m_iInventoryPosition = 0x5C; // uint32_t
        public const uint m_bInitialized = 0x68; // bool
        public const uint m_AttributeList = 0x70; // CAttributeList
        public const uint m_NetworkedDynamicAttributes = 0xD0; // CAttributeList
        public const uint m_szCustomName = 0x130; // char[161]
        public const uint m_szCustomNameOverride = 0x1D1; // char[161]
    }

    public static class CEconWearable  // CEconEntity
    {
        public const uint m_nForceSkin = 0xC18; // int32_t
        public const uint m_bAlwaysAllow = 0xC1C; // bool
    }

    public static class CEffectData 
    {
        public const uint m_vOrigin = 0x8; // Vector
        public const uint m_vStart = 0x14; // Vector
        public const uint m_vNormal = 0x20; // Vector
        public const uint m_vAngles = 0x2C; // QAngle
        public const uint m_hEntity = 0x38; // CEntityHandle
        public const uint m_hOtherEntity = 0x3C; // CEntityHandle
        public const uint m_flScale = 0x40; // float
        public const uint m_flMagnitude = 0x44; // float
        public const uint m_flRadius = 0x48; // float
        public const uint m_nSurfaceProp = 0x4C; // CUtlStringToken
        public const uint m_nEffectIndex = 0x50; // CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_nDamageType = 0x58; // uint32_t
        public const uint m_nPenetrate = 0x5C; // uint8_t
        public const uint m_nMaterial = 0x5E; // uint16_t
        public const uint m_nHitBox = 0x60; // uint16_t
        public const uint m_nColor = 0x62; // uint8_t
        public const uint m_fFlags = 0x63; // uint8_t
        public const uint m_nAttachmentIndex = 0x64; // AttachmentHandle_t
        public const uint m_nAttachmentName = 0x68; // CUtlStringToken
        public const uint m_iEffectName = 0x6C; // uint16_t
        public const uint m_nExplosionType = 0x6E; // uint8_t
    }

    public static class CEnableMotionFixup  // CBaseEntity
    {
    }

    public static class CEntityBlocker  // CBaseModelEntity
    {
    }

    public static class CEntityComponent 
    {
    }

    public static class CEntityDissolve  // CBaseModelEntity
    {
        public const uint m_flFadeInStart = 0x700; // float
        public const uint m_flFadeInLength = 0x704; // float
        public const uint m_flFadeOutModelStart = 0x708; // float
        public const uint m_flFadeOutModelLength = 0x70C; // float
        public const uint m_flFadeOutStart = 0x710; // float
        public const uint m_flFadeOutLength = 0x714; // float
        public const uint m_flStartTime = 0x718; // GameTime_t
        public const uint m_nDissolveType = 0x71C; // EntityDisolveType_t
        public const uint m_vDissolverOrigin = 0x720; // Vector
        public const uint m_nMagnitude = 0x72C; // uint32_t
    }

    public static class CEntityFlame  // CBaseEntity
    {
        public const uint m_hEntAttached = 0x4B0; // CHandle<CBaseEntity>
        public const uint m_bCheapEffect = 0x4B4; // bool
        public const uint m_flSize = 0x4B8; // float
        public const uint m_bUseHitboxes = 0x4BC; // bool
        public const uint m_iNumHitboxFires = 0x4C0; // int32_t
        public const uint m_flHitboxFireScale = 0x4C4; // float
        public const uint m_flLifetime = 0x4C8; // GameTime_t
        public const uint m_hAttacker = 0x4CC; // CHandle<CBaseEntity>
        public const uint m_iDangerSound = 0x4D0; // int32_t
        public const uint m_flDirectDamagePerSecond = 0x4D4; // float
        public const uint m_iCustomDamageType = 0x4D8; // int32_t
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

    public static class CEntitySubclassVDataBase 
    {
    }

    public static class CEnvBeam  // CBeam
    {
        public const uint m_active = 0x7A0; // int32_t
        public const uint m_spriteTexture = 0x7A8; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_iszStartEntity = 0x7B0; // CUtlSymbolLarge
        public const uint m_iszEndEntity = 0x7B8; // CUtlSymbolLarge
        public const uint m_life = 0x7C0; // float
        public const uint m_boltWidth = 0x7C4; // float
        public const uint m_noiseAmplitude = 0x7C8; // float
        public const uint m_speed = 0x7CC; // int32_t
        public const uint m_restrike = 0x7D0; // float
        public const uint m_iszSpriteName = 0x7D8; // CUtlSymbolLarge
        public const uint m_frameStart = 0x7E0; // int32_t
        public const uint m_vEndPointWorld = 0x7E4; // Vector
        public const uint m_vEndPointRelative = 0x7F0; // Vector
        public const uint m_radius = 0x7FC; // float
        public const uint m_TouchType = 0x800; // Touch_t
        public const uint m_iFilterName = 0x808; // CUtlSymbolLarge
        public const uint m_hFilter = 0x810; // CHandle<CBaseEntity>
        public const uint m_iszDecal = 0x818; // CUtlSymbolLarge
        public const uint m_OnTouchedByEntity = 0x820; // CEntityIOOutput
    }

    public static class CEnvBeverage  // CBaseEntity
    {
        public const uint m_CanInDispenser = 0x4B0; // bool
        public const uint m_nBeverageType = 0x4B4; // int32_t
    }

    public static class CEnvCombinedLightProbeVolume  // CBaseEntity
    {
        public const uint m_Color = 0x1508; // Color
        public const uint m_flBrightness = 0x150C; // float
        public const uint m_hCubemapTexture = 0x1510; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_bCustomCubemapTexture = 0x1518; // bool
        public const uint m_hLightProbeTexture = 0x1520; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hLightProbeDirectLightIndicesTexture = 0x1528; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hLightProbeDirectLightScalarsTexture = 0x1530; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hLightProbeDirectLightShadowsTexture = 0x1538; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_vBoxMins = 0x1540; // Vector
        public const uint m_vBoxMaxs = 0x154C; // Vector
        public const uint m_bMoveable = 0x1558; // bool
        public const uint m_nHandshake = 0x155C; // int32_t
        public const uint m_nEnvCubeMapArrayIndex = 0x1560; // int32_t
        public const uint m_nPriority = 0x1564; // int32_t
        public const uint m_bStartDisabled = 0x1568; // bool
        public const uint m_flEdgeFadeDist = 0x156C; // float
        public const uint m_vEdgeFadeDists = 0x1570; // Vector
        public const uint m_nLightProbeSizeX = 0x157C; // int32_t
        public const uint m_nLightProbeSizeY = 0x1580; // int32_t
        public const uint m_nLightProbeSizeZ = 0x1584; // int32_t
        public const uint m_nLightProbeAtlasX = 0x1588; // int32_t
        public const uint m_nLightProbeAtlasY = 0x158C; // int32_t
        public const uint m_nLightProbeAtlasZ = 0x1590; // int32_t
        public const uint m_bEnabled = 0x15A9; // bool
    }

    public static class CEnvCubemap  // CBaseEntity
    {
        public const uint m_hCubemapTexture = 0x530; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_bCustomCubemapTexture = 0x538; // bool
        public const uint m_flInfluenceRadius = 0x53C; // float
        public const uint m_vBoxProjectMins = 0x540; // Vector
        public const uint m_vBoxProjectMaxs = 0x54C; // Vector
        public const uint m_bMoveable = 0x558; // bool
        public const uint m_nHandshake = 0x55C; // int32_t
        public const uint m_nEnvCubeMapArrayIndex = 0x560; // int32_t
        public const uint m_nPriority = 0x564; // int32_t
        public const uint m_flEdgeFadeDist = 0x568; // float
        public const uint m_vEdgeFadeDists = 0x56C; // Vector
        public const uint m_flDiffuseScale = 0x578; // float
        public const uint m_bStartDisabled = 0x57C; // bool
        public const uint m_bDefaultEnvMap = 0x57D; // bool
        public const uint m_bDefaultSpecEnvMap = 0x57E; // bool
        public const uint m_bIndoorCubeMap = 0x57F; // bool
        public const uint m_bCopyDiffuseFromDefaultCubemap = 0x580; // bool
        public const uint m_bEnabled = 0x590; // bool
    }

    public static class CEnvCubemapBox  // CEnvCubemap
    {
    }

    public static class CEnvCubemapFog  // CBaseEntity
    {
        public const uint m_flEndDistance = 0x4B0; // float
        public const uint m_flStartDistance = 0x4B4; // float
        public const uint m_flFogFalloffExponent = 0x4B8; // float
        public const uint m_bHeightFogEnabled = 0x4BC; // bool
        public const uint m_flFogHeightWidth = 0x4C0; // float
        public const uint m_flFogHeightEnd = 0x4C4; // float
        public const uint m_flFogHeightStart = 0x4C8; // float
        public const uint m_flFogHeightExponent = 0x4CC; // float
        public const uint m_flLODBias = 0x4D0; // float
        public const uint m_bActive = 0x4D4; // bool
        public const uint m_bStartDisabled = 0x4D5; // bool
        public const uint m_flFogMaxOpacity = 0x4D8; // float
        public const uint m_nCubemapSourceType = 0x4DC; // int32_t
        public const uint m_hSkyMaterial = 0x4E0; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_iszSkyEntity = 0x4E8; // CUtlSymbolLarge
        public const uint m_hFogCubemapTexture = 0x4F0; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_bHasHeightFogEnd = 0x4F8; // bool
        public const uint m_bFirstTime = 0x4F9; // bool
    }

    public static class CEnvDecal  // CBaseModelEntity
    {
        public const uint m_hDecalMaterial = 0x700; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_flWidth = 0x708; // float
        public const uint m_flHeight = 0x70C; // float
        public const uint m_flDepth = 0x710; // float
        public const uint m_nRenderOrder = 0x714; // uint32_t
        public const uint m_bProjectOnWorld = 0x718; // bool
        public const uint m_bProjectOnCharacters = 0x719; // bool
        public const uint m_bProjectOnWater = 0x71A; // bool
        public const uint m_flDepthSortBias = 0x71C; // float
    }

    public static class CEnvDetailController  // CBaseEntity
    {
        public const uint m_flFadeStartDist = 0x4B0; // float
        public const uint m_flFadeEndDist = 0x4B4; // float
    }

    public static class CEnvEntityIgniter  // CBaseEntity
    {
        public const uint m_flLifetime = 0x4B0; // float
    }

    public static class CEnvEntityMaker  // CPointEntity
    {
        public const uint m_vecEntityMins = 0x4B0; // Vector
        public const uint m_vecEntityMaxs = 0x4BC; // Vector
        public const uint m_hCurrentInstance = 0x4C8; // CHandle<CBaseEntity>
        public const uint m_hCurrentBlocker = 0x4CC; // CHandle<CBaseEntity>
        public const uint m_vecBlockerOrigin = 0x4D0; // Vector
        public const uint m_angPostSpawnDirection = 0x4DC; // QAngle
        public const uint m_flPostSpawnDirectionVariance = 0x4E8; // float
        public const uint m_flPostSpawnSpeed = 0x4EC; // float
        public const uint m_bPostSpawnUseAngles = 0x4F0; // bool
        public const uint m_iszTemplate = 0x4F8; // CUtlSymbolLarge
        public const uint m_pOutputOnSpawned = 0x500; // CEntityIOOutput
        public const uint m_pOutputOnFailedSpawn = 0x528; // CEntityIOOutput
    }

    public static class CEnvExplosion  // CModelPointEntity
    {
        public const uint m_iMagnitude = 0x700; // int32_t
        public const uint m_flPlayerDamage = 0x704; // float
        public const uint m_iRadiusOverride = 0x708; // int32_t
        public const uint m_flInnerRadius = 0x70C; // float
        public const uint m_spriteScale = 0x710; // int32_t
        public const uint m_flDamageForce = 0x714; // float
        public const uint m_hInflictor = 0x718; // CHandle<CBaseEntity>
        public const uint m_iCustomDamageType = 0x71C; // int32_t
        public const uint m_iszExplosionType = 0x728; // CUtlSymbolLarge
        public const uint m_iszCustomEffectName = 0x730; // CUtlSymbolLarge
        public const uint m_iszCustomSoundName = 0x738; // CUtlSymbolLarge
        public const uint m_iClassIgnore = 0x740; // Class_T
        public const uint m_iClassIgnore2 = 0x744; // Class_T
        public const uint m_iszEntityIgnoreName = 0x748; // CUtlSymbolLarge
        public const uint m_hEntityIgnore = 0x750; // CHandle<CBaseEntity>
    }

    public static class CEnvFade  // CLogicalEntity
    {
        public const uint m_fadeColor = 0x4B0; // Color
        public const uint m_Duration = 0x4B4; // float
        public const uint m_HoldDuration = 0x4B8; // float
        public const uint m_OnBeginFade = 0x4C0; // CEntityIOOutput
    }

    public static class CEnvFireSensor  // CBaseEntity
    {
        public const uint m_bEnabled = 0x4B0; // bool
        public const uint m_bHeatAtLevel = 0x4B1; // bool
        public const uint m_radius = 0x4B4; // float
        public const uint m_targetLevel = 0x4B8; // float
        public const uint m_targetTime = 0x4BC; // float
        public const uint m_levelTime = 0x4C0; // float
        public const uint m_OnHeatLevelStart = 0x4C8; // CEntityIOOutput
        public const uint m_OnHeatLevelEnd = 0x4F0; // CEntityIOOutput
    }

    public static class CEnvFireSource  // CBaseEntity
    {
        public const uint m_bEnabled = 0x4B0; // bool
        public const uint m_radius = 0x4B4; // float
        public const uint m_damage = 0x4B8; // float
    }

    public static class CEnvFunnel  // CBaseEntity
    {
    }

    public static class CEnvGlobal  // CLogicalEntity
    {
        public const uint m_outCounter = 0x4B0; // CEntityOutputTemplate<int32_t>
        public const uint m_globalstate = 0x4D8; // CUtlSymbolLarge
        public const uint m_triggermode = 0x4E0; // int32_t
        public const uint m_initialstate = 0x4E4; // int32_t
        public const uint m_counter = 0x4E8; // int32_t
    }

    public static class CEnvHudHint  // CPointEntity
    {
        public const uint m_iszMessage = 0x4B0; // CUtlSymbolLarge
    }

    public static class CEnvInstructorHint  // CPointEntity
    {
        public const uint m_iszName = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszReplace_Key = 0x4B8; // CUtlSymbolLarge
        public const uint m_iszHintTargetEntity = 0x4C0; // CUtlSymbolLarge
        public const uint m_iTimeout = 0x4C8; // int32_t
        public const uint m_iDisplayLimit = 0x4CC; // int32_t
        public const uint m_iszIcon_Onscreen = 0x4D0; // CUtlSymbolLarge
        public const uint m_iszIcon_Offscreen = 0x4D8; // CUtlSymbolLarge
        public const uint m_iszCaption = 0x4E0; // CUtlSymbolLarge
        public const uint m_iszActivatorCaption = 0x4E8; // CUtlSymbolLarge
        public const uint m_Color = 0x4F0; // Color
        public const uint m_fIconOffset = 0x4F4; // float
        public const uint m_fRange = 0x4F8; // float
        public const uint m_iPulseOption = 0x4FC; // uint8_t
        public const uint m_iAlphaOption = 0x4FD; // uint8_t
        public const uint m_iShakeOption = 0x4FE; // uint8_t
        public const uint m_bStatic = 0x4FF; // bool
        public const uint m_bNoOffscreen = 0x500; // bool
        public const uint m_bForceCaption = 0x501; // bool
        public const uint m_iInstanceType = 0x504; // int32_t
        public const uint m_bSuppressRest = 0x508; // bool
        public const uint m_iszBinding = 0x510; // CUtlSymbolLarge
        public const uint m_bAllowNoDrawTarget = 0x518; // bool
        public const uint m_bAutoStart = 0x519; // bool
        public const uint m_bLocalPlayerOnly = 0x51A; // bool
    }

    public static class CEnvInstructorVRHint  // CPointEntity
    {
        public const uint m_iszName = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszHintTargetEntity = 0x4B8; // CUtlSymbolLarge
        public const uint m_iTimeout = 0x4C0; // int32_t
        public const uint m_iszCaption = 0x4C8; // CUtlSymbolLarge
        public const uint m_iszStartSound = 0x4D0; // CUtlSymbolLarge
        public const uint m_iLayoutFileType = 0x4D8; // int32_t
        public const uint m_iszCustomLayoutFile = 0x4E0; // CUtlSymbolLarge
        public const uint m_iAttachType = 0x4E8; // int32_t
        public const uint m_flHeightOffset = 0x4EC; // float
    }

    public static class CEnvLaser  // CBeam
    {
        public const uint m_iszLaserTarget = 0x7A0; // CUtlSymbolLarge
        public const uint m_pSprite = 0x7A8; // CSprite*
        public const uint m_iszSpriteName = 0x7B0; // CUtlSymbolLarge
        public const uint m_firePosition = 0x7B8; // Vector
        public const uint m_flStartFrame = 0x7C4; // float
    }

    public static class CEnvLightProbeVolume  // CBaseEntity
    {
        public const uint m_hLightProbeTexture = 0x1488; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hLightProbeDirectLightIndicesTexture = 0x1490; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hLightProbeDirectLightScalarsTexture = 0x1498; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hLightProbeDirectLightShadowsTexture = 0x14A0; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_vBoxMins = 0x14A8; // Vector
        public const uint m_vBoxMaxs = 0x14B4; // Vector
        public const uint m_bMoveable = 0x14C0; // bool
        public const uint m_nHandshake = 0x14C4; // int32_t
        public const uint m_nPriority = 0x14C8; // int32_t
        public const uint m_bStartDisabled = 0x14CC; // bool
        public const uint m_nLightProbeSizeX = 0x14D0; // int32_t
        public const uint m_nLightProbeSizeY = 0x14D4; // int32_t
        public const uint m_nLightProbeSizeZ = 0x14D8; // int32_t
        public const uint m_nLightProbeAtlasX = 0x14DC; // int32_t
        public const uint m_nLightProbeAtlasY = 0x14E0; // int32_t
        public const uint m_nLightProbeAtlasZ = 0x14E4; // int32_t
        public const uint m_bEnabled = 0x14F1; // bool
    }

    public static class CEnvMicrophone  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_hMeasureTarget = 0x4B4; // CHandle<CBaseEntity>
        public const uint m_nSoundMask = 0x4B8; // int32_t
        public const uint m_flSensitivity = 0x4BC; // float
        public const uint m_flSmoothFactor = 0x4C0; // float
        public const uint m_flMaxRange = 0x4C4; // float
        public const uint m_iszSpeakerName = 0x4C8; // CUtlSymbolLarge
        public const uint m_hSpeaker = 0x4D0; // CHandle<CBaseEntity>
        public const uint m_bAvoidFeedback = 0x4D4; // bool
        public const uint m_iSpeakerDSPPreset = 0x4D8; // int32_t
        public const uint m_iszListenFilter = 0x4E0; // CUtlSymbolLarge
        public const uint m_hListenFilter = 0x4E8; // CHandle<CBaseFilter>
        public const uint m_SoundLevel = 0x4F0; // CEntityOutputTemplate<float>
        public const uint m_OnRoutedSound = 0x518; // CEntityIOOutput
        public const uint m_OnHeardSound = 0x540; // CEntityIOOutput
        public const uint m_szLastSound = 0x568; // char[256]
        public const uint m_iLastRoutedFrame = 0x668; // int32_t
    }

    public static class CEnvMuzzleFlash  // CPointEntity
    {
        public const uint m_flScale = 0x4B0; // float
        public const uint m_iszParentAttachment = 0x4B8; // CUtlSymbolLarge
    }

    public static class CEnvParticleGlow  // CParticleSystem
    {
        public const uint m_flAlphaScale = 0xC78; // float
        public const uint m_flRadiusScale = 0xC7C; // float
        public const uint m_flSelfIllumScale = 0xC80; // float
        public const uint m_ColorTint = 0xC84; // Color
        public const uint m_hTextureOverride = 0xC88; // CStrongHandle<InfoForResourceTypeCTextureBase>
    }

    public static class CEnvProjectedTexture  // CModelPointEntity
    {
        public const uint m_hTargetEntity = 0x700; // CHandle<CBaseEntity>
        public const uint m_bState = 0x704; // bool
        public const uint m_bAlwaysUpdate = 0x705; // bool
        public const uint m_flLightFOV = 0x708; // float
        public const uint m_bEnableShadows = 0x70C; // bool
        public const uint m_bSimpleProjection = 0x70D; // bool
        public const uint m_bLightOnlyTarget = 0x70E; // bool
        public const uint m_bLightWorld = 0x70F; // bool
        public const uint m_bCameraSpace = 0x710; // bool
        public const uint m_flBrightnessScale = 0x714; // float
        public const uint m_LightColor = 0x718; // Color
        public const uint m_flIntensity = 0x71C; // float
        public const uint m_flLinearAttenuation = 0x720; // float
        public const uint m_flQuadraticAttenuation = 0x724; // float
        public const uint m_bVolumetric = 0x728; // bool
        public const uint m_flNoiseStrength = 0x72C; // float
        public const uint m_flFlashlightTime = 0x730; // float
        public const uint m_nNumPlanes = 0x734; // uint32_t
        public const uint m_flPlaneOffset = 0x738; // float
        public const uint m_flVolumetricIntensity = 0x73C; // float
        public const uint m_flColorTransitionTime = 0x740; // float
        public const uint m_flAmbient = 0x744; // float
        public const uint m_SpotlightTextureName = 0x748; // char[512]
        public const uint m_nSpotlightTextureFrame = 0x948; // int32_t
        public const uint m_nShadowQuality = 0x94C; // uint32_t
        public const uint m_flNearZ = 0x950; // float
        public const uint m_flFarZ = 0x954; // float
        public const uint m_flProjectionSize = 0x958; // float
        public const uint m_flRotation = 0x95C; // float
        public const uint m_bFlipHorizontal = 0x960; // bool
    }

    public static class CEnvScreenOverlay  // CPointEntity
    {
        public const uint m_iszOverlayNames = 0x4B0; // CUtlSymbolLarge[10]
        public const uint m_flOverlayTimes = 0x500; // float[10]
        public const uint m_flStartTime = 0x528; // GameTime_t
        public const uint m_iDesiredOverlay = 0x52C; // int32_t
        public const uint m_bIsActive = 0x530; // bool
    }

    public static class CEnvShake  // CPointEntity
    {
        public const uint m_limitToEntity = 0x4B0; // CUtlSymbolLarge
        public const uint m_Amplitude = 0x4B8; // float
        public const uint m_Frequency = 0x4BC; // float
        public const uint m_Duration = 0x4C0; // float
        public const uint m_Radius = 0x4C4; // float
        public const uint m_stopTime = 0x4C8; // GameTime_t
        public const uint m_nextShake = 0x4CC; // GameTime_t
        public const uint m_currentAmp = 0x4D0; // float
        public const uint m_maxForce = 0x4D4; // Vector
        public const uint m_shakeCallback = 0x4E8; // CPhysicsShake
    }

    public static class CEnvSky  // CBaseModelEntity
    {
        public const uint m_hSkyMaterial = 0x700; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_hSkyMaterialLightingOnly = 0x708; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_bStartDisabled = 0x710; // bool
        public const uint m_vTintColor = 0x711; // Color
        public const uint m_vTintColorLightingOnly = 0x715; // Color
        public const uint m_flBrightnessScale = 0x71C; // float
        public const uint m_nFogType = 0x720; // int32_t
        public const uint m_flFogMinStart = 0x724; // float
        public const uint m_flFogMinEnd = 0x728; // float
        public const uint m_flFogMaxStart = 0x72C; // float
        public const uint m_flFogMaxEnd = 0x730; // float
        public const uint m_bEnabled = 0x734; // bool
    }

    public static class CEnvSoundscape  // CServerOnlyEntity
    {
        public const uint m_OnPlay = 0x4B0; // CEntityIOOutput
        public const uint m_flRadius = 0x4D8; // float
        public const uint m_soundscapeName = 0x4E0; // CUtlSymbolLarge
        public const uint m_soundEventName = 0x4E8; // CUtlSymbolLarge
        public const uint m_bOverrideWithEvent = 0x4F0; // bool
        public const uint m_soundscapeIndex = 0x4F4; // int32_t
        public const uint m_soundscapeEntityListId = 0x4F8; // int32_t
        public const uint m_soundEventHash = 0x4FC; // uint32_t
        public const uint m_positionNames = 0x500; // CUtlSymbolLarge[8]
        public const uint m_hProxySoundscape = 0x540; // CHandle<CEnvSoundscape>
        public const uint m_bDisabled = 0x544; // bool
    }

    public static class CEnvSoundscapeAlias_snd_soundscape  // CEnvSoundscape
    {
    }

    public static class CEnvSoundscapeProxy  // CEnvSoundscape
    {
        public const uint m_MainSoundscapeName = 0x548; // CUtlSymbolLarge
    }

    public static class CEnvSoundscapeProxyAlias_snd_soundscape_proxy  // CEnvSoundscapeProxy
    {
    }

    public static class CEnvSoundscapeTriggerable  // CEnvSoundscape
    {
    }

    public static class CEnvSoundscapeTriggerableAlias_snd_soundscape_triggerable  // CEnvSoundscapeTriggerable
    {
    }

    public static class CEnvSpark  // CPointEntity
    {
        public const uint m_flDelay = 0x4B0; // float
        public const uint m_nMagnitude = 0x4B4; // int32_t
        public const uint m_nTrailLength = 0x4B8; // int32_t
        public const uint m_nType = 0x4BC; // int32_t
        public const uint m_OnSpark = 0x4C0; // CEntityIOOutput
    }

    public static class CEnvSplash  // CPointEntity
    {
        public const uint m_flScale = 0x4B0; // float
    }

    public static class CEnvTilt  // CPointEntity
    {
        public const uint m_Duration = 0x4B0; // float
        public const uint m_Radius = 0x4B4; // float
        public const uint m_TiltTime = 0x4B8; // float
        public const uint m_stopTime = 0x4BC; // GameTime_t
    }

    public static class CEnvTracer  // CPointEntity
    {
        public const uint m_vecEnd = 0x4B0; // Vector
        public const uint m_flDelay = 0x4BC; // float
    }

    public static class CEnvViewPunch  // CPointEntity
    {
        public const uint m_flRadius = 0x4B0; // float
        public const uint m_angViewPunch = 0x4B4; // QAngle
    }

    public static class CEnvVolumetricFogController  // CBaseEntity
    {
        public const uint m_flScattering = 0x4B0; // float
        public const uint m_flAnisotropy = 0x4B4; // float
        public const uint m_flFadeSpeed = 0x4B8; // float
        public const uint m_flDrawDistance = 0x4BC; // float
        public const uint m_flFadeInStart = 0x4C0; // float
        public const uint m_flFadeInEnd = 0x4C4; // float
        public const uint m_flIndirectStrength = 0x4C8; // float
        public const uint m_nIndirectTextureDimX = 0x4CC; // int32_t
        public const uint m_nIndirectTextureDimY = 0x4D0; // int32_t
        public const uint m_nIndirectTextureDimZ = 0x4D4; // int32_t
        public const uint m_vBoxMins = 0x4D8; // Vector
        public const uint m_vBoxMaxs = 0x4E4; // Vector
        public const uint m_bActive = 0x4F0; // bool
        public const uint m_flStartAnisoTime = 0x4F4; // GameTime_t
        public const uint m_flStartScatterTime = 0x4F8; // GameTime_t
        public const uint m_flStartDrawDistanceTime = 0x4FC; // GameTime_t
        public const uint m_flStartAnisotropy = 0x500; // float
        public const uint m_flStartScattering = 0x504; // float
        public const uint m_flStartDrawDistance = 0x508; // float
        public const uint m_flDefaultAnisotropy = 0x50C; // float
        public const uint m_flDefaultScattering = 0x510; // float
        public const uint m_flDefaultDrawDistance = 0x514; // float
        public const uint m_bStartDisabled = 0x518; // bool
        public const uint m_bEnableIndirect = 0x519; // bool
        public const uint m_bIsMaster = 0x51A; // bool
        public const uint m_hFogIndirectTexture = 0x520; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_nForceRefreshCount = 0x528; // int32_t
        public const uint m_bFirstTime = 0x52C; // bool
    }

    public static class CEnvVolumetricFogVolume  // CBaseEntity
    {
        public const uint m_bActive = 0x4B0; // bool
        public const uint m_vBoxMins = 0x4B4; // Vector
        public const uint m_vBoxMaxs = 0x4C0; // Vector
        public const uint m_bStartDisabled = 0x4CC; // bool
        public const uint m_flStrength = 0x4D0; // float
        public const uint m_nFalloffShape = 0x4D4; // int32_t
        public const uint m_flFalloffExponent = 0x4D8; // float
    }

    public static class CEnvWind  // CBaseEntity
    {
        public const uint m_EnvWindShared = 0x4B0; // CEnvWindShared
    }

    public static class CEnvWindShared 
    {
        public const uint m_flStartTime = 0x8; // GameTime_t
        public const uint m_iWindSeed = 0xC; // uint32_t
        public const uint m_iMinWind = 0x10; // uint16_t
        public const uint m_iMaxWind = 0x12; // uint16_t
        public const uint m_windRadius = 0x14; // int32_t
        public const uint m_iMinGust = 0x18; // uint16_t
        public const uint m_iMaxGust = 0x1A; // uint16_t
        public const uint m_flMinGustDelay = 0x1C; // float
        public const uint m_flMaxGustDelay = 0x20; // float
        public const uint m_flGustDuration = 0x24; // float
        public const uint m_iGustDirChange = 0x28; // uint16_t
        public const uint m_location = 0x2C; // Vector
        public const uint m_iszGustSound = 0x38; // int32_t
        public const uint m_iWindDir = 0x3C; // int32_t
        public const uint m_flWindSpeed = 0x40; // float
        public const uint m_currentWindVector = 0x44; // Vector
        public const uint m_CurrentSwayVector = 0x50; // Vector
        public const uint m_PrevSwayVector = 0x5C; // Vector
        public const uint m_iInitialWindDir = 0x68; // uint16_t
        public const uint m_flInitialWindSpeed = 0x6C; // float
        public const uint m_OnGustStart = 0x70; // CEntityIOOutput
        public const uint m_OnGustEnd = 0x98; // CEntityIOOutput
        public const uint m_flVariationTime = 0xC0; // GameTime_t
        public const uint m_flSwayTime = 0xC4; // GameTime_t
        public const uint m_flSimTime = 0xC8; // GameTime_t
        public const uint m_flSwitchTime = 0xCC; // GameTime_t
        public const uint m_flAveWindSpeed = 0xD0; // float
        public const uint m_bGusting = 0xD4; // bool
        public const uint m_flWindAngleVariation = 0xD8; // float
        public const uint m_flWindSpeedVariation = 0xDC; // float
        public const uint m_iEntIndex = 0xE0; // CEntityIndex
    }

    public static class CEnvWindShared_WindAveEvent_t 
    {
        public const uint m_flStartWindSpeed = 0x0; // float
        public const uint m_flAveWindSpeed = 0x4; // float
    }

    public static class CEnvWindShared_WindVariationEvent_t 
    {
        public const uint m_flWindAngleVariation = 0x0; // float
        public const uint m_flWindSpeedVariation = 0x4; // float
    }

    public static class CFilterAttributeInt  // CBaseFilter
    {
        public const uint m_sAttributeName = 0x508; // CUtlStringToken
    }

    public static class CFilterClass  // CBaseFilter
    {
        public const uint m_iFilterClass = 0x508; // CUtlSymbolLarge
    }

    public static class CFilterContext  // CBaseFilter
    {
        public const uint m_iFilterContext = 0x508; // CUtlSymbolLarge
    }

    public static class CFilterEnemy  // CBaseFilter
    {
        public const uint m_iszEnemyName = 0x508; // CUtlSymbolLarge
        public const uint m_flRadius = 0x510; // float
        public const uint m_flOuterRadius = 0x514; // float
        public const uint m_nMaxSquadmatesPerEnemy = 0x518; // int32_t
        public const uint m_iszPlayerName = 0x520; // CUtlSymbolLarge
    }

    public static class CFilterLOS  // CBaseFilter
    {
    }

    public static class CFilterMassGreater  // CBaseFilter
    {
        public const uint m_fFilterMass = 0x508; // float
    }

    public static class CFilterModel  // CBaseFilter
    {
        public const uint m_iFilterModel = 0x508; // CUtlSymbolLarge
    }

    public static class CFilterMultiple  // CBaseFilter
    {
        public const uint m_nFilterType = 0x508; // filter_t
        public const uint m_iFilterName = 0x510; // CUtlSymbolLarge[10]
        public const uint m_hFilter = 0x560; // CHandle<CBaseEntity>[10]
        public const uint m_nFilterCount = 0x588; // int32_t
    }

    public static class CFilterName  // CBaseFilter
    {
        public const uint m_iFilterName = 0x508; // CUtlSymbolLarge
    }

    public static class CFilterProximity  // CBaseFilter
    {
        public const uint m_flRadius = 0x508; // float
    }

    public static class CFire  // CBaseModelEntity
    {
        public const uint m_hEffect = 0x700; // CHandle<CBaseFire>
        public const uint m_hOwner = 0x704; // CHandle<CBaseEntity>
        public const uint m_nFireType = 0x708; // int32_t
        public const uint m_flFuel = 0x70C; // float
        public const uint m_flDamageTime = 0x710; // GameTime_t
        public const uint m_lastDamage = 0x714; // GameTime_t
        public const uint m_flFireSize = 0x718; // float
        public const uint m_flLastNavUpdateTime = 0x71C; // GameTime_t
        public const uint m_flHeatLevel = 0x720; // float
        public const uint m_flHeatAbsorb = 0x724; // float
        public const uint m_flDamageScale = 0x728; // float
        public const uint m_flMaxHeat = 0x72C; // float
        public const uint m_flLastHeatLevel = 0x730; // float
        public const uint m_flAttackTime = 0x734; // float
        public const uint m_bEnabled = 0x738; // bool
        public const uint m_bStartDisabled = 0x739; // bool
        public const uint m_bDidActivate = 0x73A; // bool
        public const uint m_OnIgnited = 0x740; // CEntityIOOutput
        public const uint m_OnExtinguished = 0x768; // CEntityIOOutput
    }

    public static class CFireCrackerBlast  // CInferno
    {
    }

    public static class CFireSmoke  // CBaseFire
    {
        public const uint m_nFlameModelIndex = 0x4C0; // int32_t
        public const uint m_nFlameFromAboveModelIndex = 0x4C4; // int32_t
    }

    public static class CFiringModeFloat 
    {
        public const uint m_flValues = 0x0; // float[2]
    }

    public static class CFiringModeInt 
    {
        public const uint m_nValues = 0x0; // int32_t[2]
    }

    public static class CFish  // CBaseAnimGraph
    {
        public const uint m_pool = 0x890; // CHandle<CFishPool>
        public const uint m_id = 0x894; // uint32_t
        public const uint m_x = 0x898; // float
        public const uint m_y = 0x89C; // float
        public const uint m_z = 0x8A0; // float
        public const uint m_angle = 0x8A4; // float
        public const uint m_angleChange = 0x8A8; // float
        public const uint m_forward = 0x8AC; // Vector
        public const uint m_perp = 0x8B8; // Vector
        public const uint m_poolOrigin = 0x8C4; // Vector
        public const uint m_waterLevel = 0x8D0; // float
        public const uint m_speed = 0x8D4; // float
        public const uint m_desiredSpeed = 0x8D8; // float
        public const uint m_calmSpeed = 0x8DC; // float
        public const uint m_panicSpeed = 0x8E0; // float
        public const uint m_avoidRange = 0x8E4; // float
        public const uint m_turnTimer = 0x8E8; // CountdownTimer
        public const uint m_turnClockwise = 0x900; // bool
        public const uint m_goTimer = 0x908; // CountdownTimer
        public const uint m_moveTimer = 0x920; // CountdownTimer
        public const uint m_panicTimer = 0x938; // CountdownTimer
        public const uint m_disperseTimer = 0x950; // CountdownTimer
        public const uint m_proximityTimer = 0x968; // CountdownTimer
        public const uint m_visible = 0x980; // CUtlVector<CFish*>
    }

    public static class CFishPool  // CBaseEntity
    {
        public const uint m_fishCount = 0x4C0; // int32_t
        public const uint m_maxRange = 0x4C4; // float
        public const uint m_swimDepth = 0x4C8; // float
        public const uint m_waterLevel = 0x4CC; // float
        public const uint m_isDormant = 0x4D0; // bool
        public const uint m_fishes = 0x4D8; // CUtlVector<CHandle<CFish>>
        public const uint m_visTimer = 0x4F0; // CountdownTimer
    }

    public static class CFists  // CCSWeaponBase
    {
        public const uint m_bPlayingUninterruptableAct = 0xE28; // bool
        public const uint m_nUninterruptableActivity = 0xE2C; // PlayerAnimEvent_t
        public const uint m_bRestorePrevWep = 0xE30; // bool
        public const uint m_hWeaponBeforePrevious = 0xE34; // CHandle<CBasePlayerWeapon>
        public const uint m_hWeaponPrevious = 0xE38; // CHandle<CBasePlayerWeapon>
        public const uint m_bDelayedHardPunchIncoming = 0xE3C; // bool
        public const uint m_bDestroyAfterTaunt = 0xE3D; // bool
    }

    public static class CFlashbang  // CBaseCSGrenade
    {
    }

    public static class CFlashbangProjectile  // CBaseCSGrenadeProjectile
    {
        public const uint m_flTimeToDetonate = 0xA40; // float
        public const uint m_numOpponentsHit = 0xA44; // uint8_t
        public const uint m_numTeammatesHit = 0xA45; // uint8_t
    }

    public static class CFogController  // CBaseEntity
    {
        public const uint m_fog = 0x4B0; // fogparams_t
        public const uint m_bUseAngles = 0x518; // bool
        public const uint m_iChangedVariables = 0x51C; // int32_t
    }

    public static class CFogTrigger  // CBaseTrigger
    {
        public const uint m_fog = 0x8A8; // fogparams_t
    }

    public static class CFogVolume  // CServerOnlyModelEntity
    {
        public const uint m_fogName = 0x700; // CUtlSymbolLarge
        public const uint m_postProcessName = 0x708; // CUtlSymbolLarge
        public const uint m_colorCorrectionName = 0x710; // CUtlSymbolLarge
        public const uint m_bDisabled = 0x720; // bool
        public const uint m_bInFogVolumesList = 0x721; // bool
    }

    public static class CFootstepControl  // CBaseTrigger
    {
        public const uint m_source = 0x8A8; // CUtlSymbolLarge
        public const uint m_destination = 0x8B0; // CUtlSymbolLarge
    }

    public static class CFootstepTableHandle 
    {
    }

    public static class CFuncBrush  // CBaseModelEntity
    {
        public const uint m_iSolidity = 0x700; // BrushSolidities_e
        public const uint m_iDisabled = 0x704; // int32_t
        public const uint m_bSolidBsp = 0x708; // bool
        public const uint m_iszExcludedClass = 0x710; // CUtlSymbolLarge
        public const uint m_bInvertExclusion = 0x718; // bool
        public const uint m_bScriptedMovement = 0x719; // bool
    }

    public static class CFuncConveyor  // CBaseModelEntity
    {
        public const uint m_szConveyorModels = 0x700; // CUtlSymbolLarge
        public const uint m_flTransitionDurationSeconds = 0x708; // float
        public const uint m_angMoveEntitySpace = 0x70C; // QAngle
        public const uint m_vecMoveDirEntitySpace = 0x718; // Vector
        public const uint m_flTargetSpeed = 0x724; // float
        public const uint m_nTransitionStartTick = 0x728; // GameTick_t
        public const uint m_nTransitionDurationTicks = 0x72C; // int32_t
        public const uint m_flTransitionStartSpeed = 0x730; // float
        public const uint m_hConveyorModels = 0x738; // CNetworkUtlVectorBase<CHandle<CBaseEntity>>
    }

    public static class CFuncElectrifiedVolume  // CFuncBrush
    {
        public const uint m_EffectName = 0x720; // CUtlSymbolLarge
        public const uint m_EffectInterpenetrateName = 0x728; // CUtlSymbolLarge
        public const uint m_EffectZapName = 0x730; // CUtlSymbolLarge
        public const uint m_iszEffectSource = 0x738; // CUtlSymbolLarge
    }

    public static class CFuncIllusionary  // CBaseModelEntity
    {
    }

    public static class CFuncInteractionLayerClip  // CBaseModelEntity
    {
        public const uint m_bDisabled = 0x700; // bool
        public const uint m_iszInteractsAs = 0x708; // CUtlSymbolLarge
        public const uint m_iszInteractsWith = 0x710; // CUtlSymbolLarge
    }

    public static class CFuncLadder  // CBaseModelEntity
    {
        public const uint m_vecLadderDir = 0x700; // Vector
        public const uint m_Dismounts = 0x710; // CUtlVector<CHandle<CInfoLadderDismount>>
        public const uint m_vecLocalTop = 0x728; // Vector
        public const uint m_vecPlayerMountPositionTop = 0x734; // Vector
        public const uint m_vecPlayerMountPositionBottom = 0x740; // Vector
        public const uint m_flAutoRideSpeed = 0x74C; // float
        public const uint m_bDisabled = 0x750; // bool
        public const uint m_bFakeLadder = 0x751; // bool
        public const uint m_bHasSlack = 0x752; // bool
        public const uint m_surfacePropName = 0x758; // CUtlSymbolLarge
        public const uint m_OnPlayerGotOnLadder = 0x760; // CEntityIOOutput
        public const uint m_OnPlayerGotOffLadder = 0x788; // CEntityIOOutput
    }

    public static class CFuncLadderAlias_func_useableladder  // CFuncLadder
    {
    }

    public static class CFuncMonitor  // CFuncBrush
    {
        public const uint m_targetCamera = 0x720; // CUtlString
        public const uint m_nResolutionEnum = 0x728; // int32_t
        public const uint m_bRenderShadows = 0x72C; // bool
        public const uint m_bUseUniqueColorTarget = 0x72D; // bool
        public const uint m_brushModelName = 0x730; // CUtlString
        public const uint m_hTargetCamera = 0x738; // CHandle<CBaseEntity>
        public const uint m_bEnabled = 0x73C; // bool
        public const uint m_bDraw3DSkybox = 0x73D; // bool
        public const uint m_bStartEnabled = 0x73E; // bool
    }

    public static class CFuncMoveLinear  // CBaseToggle
    {
        public const uint m_authoredPosition = 0x780; // MoveLinearAuthoredPos_t
        public const uint m_angMoveEntitySpace = 0x784; // QAngle
        public const uint m_vecMoveDirParentSpace = 0x790; // Vector
        public const uint m_soundStart = 0x7A0; // CUtlSymbolLarge
        public const uint m_soundStop = 0x7A8; // CUtlSymbolLarge
        public const uint m_currentSound = 0x7B0; // CUtlSymbolLarge
        public const uint m_flBlockDamage = 0x7B8; // float
        public const uint m_flStartPosition = 0x7BC; // float
        public const uint m_flMoveDistance = 0x7C0; // float
        public const uint m_OnFullyOpen = 0x7D0; // CEntityIOOutput
        public const uint m_OnFullyClosed = 0x7F8; // CEntityIOOutput
        public const uint m_bCreateMovableNavMesh = 0x820; // bool
        public const uint m_bCreateNavObstacle = 0x821; // bool
    }

    public static class CFuncMoveLinearAlias_momentary_door  // CFuncMoveLinear
    {
    }

    public static class CFuncNavBlocker  // CBaseModelEntity
    {
        public const uint m_bDisabled = 0x700; // bool
        public const uint m_nBlockedTeamNumber = 0x704; // int32_t
    }

    public static class CFuncNavObstruction  // CBaseModelEntity
    {
        public const uint m_bDisabled = 0x708; // bool
    }

    public static class CFuncPlat  // CBasePlatTrain
    {
        public const uint m_sNoise = 0x7A8; // CUtlSymbolLarge
    }

    public static class CFuncPlatRot  // CFuncPlat
    {
        public const uint m_end = 0x7B0; // QAngle
        public const uint m_start = 0x7BC; // QAngle
    }

    public static class CFuncPropRespawnZone  // CBaseEntity
    {
    }

    public static class CFuncRotating  // CBaseModelEntity
    {
        public const uint m_vecMoveAng = 0x700; // QAngle
        public const uint m_flFanFriction = 0x70C; // float
        public const uint m_flAttenuation = 0x710; // float
        public const uint m_flVolume = 0x714; // float
        public const uint m_flTargetSpeed = 0x718; // float
        public const uint m_flMaxSpeed = 0x71C; // float
        public const uint m_flBlockDamage = 0x720; // float
        public const uint m_flTimeScale = 0x724; // float
        public const uint m_NoiseRunning = 0x728; // CUtlSymbolLarge
        public const uint m_bReversed = 0x730; // bool
        public const uint m_angStart = 0x73C; // QAngle
        public const uint m_bStopAtStartPos = 0x748; // bool
        public const uint m_vecClientOrigin = 0x74C; // Vector
        public const uint m_vecClientAngles = 0x758; // QAngle
    }

    public static class CFuncShatterglass  // CBaseModelEntity
    {
        public const uint m_hGlassMaterialDamaged = 0x700; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_hGlassMaterialUndamaged = 0x708; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_hConcreteMaterialEdgeFace = 0x710; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_hConcreteMaterialEdgeCaps = 0x718; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_hConcreteMaterialEdgeFins = 0x720; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_matPanelTransform = 0x728; // matrix3x4_t
        public const uint m_matPanelTransformWsTemp = 0x758; // matrix3x4_t
        public const uint m_vecShatterGlassShards = 0x788; // CUtlVector<uint32_t>
        public const uint m_PanelSize = 0x7A0; // Vector2D
        public const uint m_vecPanelNormalWs = 0x7A8; // Vector
        public const uint m_nNumShardsEverCreated = 0x7B4; // int32_t
        public const uint m_flLastShatterSoundEmitTime = 0x7B8; // GameTime_t
        public const uint m_flLastCleanupTime = 0x7BC; // GameTime_t
        public const uint m_flInitAtTime = 0x7C0; // GameTime_t
        public const uint m_flGlassThickness = 0x7C4; // float
        public const uint m_flSpawnInvulnerability = 0x7C8; // float
        public const uint m_bBreakSilent = 0x7CC; // bool
        public const uint m_bBreakShardless = 0x7CD; // bool
        public const uint m_bBroken = 0x7CE; // bool
        public const uint m_bHasRateLimitedShards = 0x7CF; // bool
        public const uint m_bGlassNavIgnore = 0x7D0; // bool
        public const uint m_bGlassInFrame = 0x7D1; // bool
        public const uint m_bStartBroken = 0x7D2; // bool
        public const uint m_iInitialDamageType = 0x7D3; // uint8_t
        public const uint m_szDamagePositioningEntityName01 = 0x7D8; // CUtlSymbolLarge
        public const uint m_szDamagePositioningEntityName02 = 0x7E0; // CUtlSymbolLarge
        public const uint m_szDamagePositioningEntityName03 = 0x7E8; // CUtlSymbolLarge
        public const uint m_szDamagePositioningEntityName04 = 0x7F0; // CUtlSymbolLarge
        public const uint m_vInitialDamagePositions = 0x7F8; // CUtlVector<Vector>
        public const uint m_vExtraDamagePositions = 0x810; // CUtlVector<Vector>
        public const uint m_OnBroken = 0x828; // CEntityIOOutput
        public const uint m_iSurfaceType = 0x851; // uint8_t
    }

    public static class CFuncTankTrain  // CFuncTrackTrain
    {
        public const uint m_OnDeath = 0x850; // CEntityIOOutput
    }

    public static class CFuncTimescale  // CBaseEntity
    {
        public const uint m_flDesiredTimescale = 0x4B0; // float
        public const uint m_flAcceleration = 0x4B4; // float
        public const uint m_flMinBlendRate = 0x4B8; // float
        public const uint m_flBlendDeltaMultiplier = 0x4BC; // float
        public const uint m_isStarted = 0x4C0; // bool
    }

    public static class CFuncTrackAuto  // CFuncTrackChange
    {
    }

    public static class CFuncTrackChange  // CFuncPlatRot
    {
        public const uint m_trackTop = 0x7C8; // CPathTrack*
        public const uint m_trackBottom = 0x7D0; // CPathTrack*
        public const uint m_train = 0x7D8; // CFuncTrackTrain*
        public const uint m_trackTopName = 0x7E0; // CUtlSymbolLarge
        public const uint m_trackBottomName = 0x7E8; // CUtlSymbolLarge
        public const uint m_trainName = 0x7F0; // CUtlSymbolLarge
        public const uint m_code = 0x7F8; // TRAIN_CODE
        public const uint m_targetState = 0x7FC; // int32_t
        public const uint m_use = 0x800; // int32_t
    }

    public static class CFuncTrackTrain  // CBaseModelEntity
    {
        public const uint m_ppath = 0x700; // CHandle<CPathTrack>
        public const uint m_length = 0x704; // float
        public const uint m_vPosPrev = 0x708; // Vector
        public const uint m_angPrev = 0x714; // QAngle
        public const uint m_controlMins = 0x720; // Vector
        public const uint m_controlMaxs = 0x72C; // Vector
        public const uint m_lastBlockPos = 0x738; // Vector
        public const uint m_lastBlockTick = 0x744; // int32_t
        public const uint m_flVolume = 0x748; // float
        public const uint m_flBank = 0x74C; // float
        public const uint m_oldSpeed = 0x750; // float
        public const uint m_flBlockDamage = 0x754; // float
        public const uint m_height = 0x758; // float
        public const uint m_maxSpeed = 0x75C; // float
        public const uint m_dir = 0x760; // float
        public const uint m_iszSoundMove = 0x768; // CUtlSymbolLarge
        public const uint m_iszSoundMovePing = 0x770; // CUtlSymbolLarge
        public const uint m_iszSoundStart = 0x778; // CUtlSymbolLarge
        public const uint m_iszSoundStop = 0x780; // CUtlSymbolLarge
        public const uint m_strPathTarget = 0x788; // CUtlSymbolLarge
        public const uint m_flMoveSoundMinDuration = 0x790; // float
        public const uint m_flMoveSoundMaxDuration = 0x794; // float
        public const uint m_flNextMoveSoundTime = 0x798; // GameTime_t
        public const uint m_flMoveSoundMinPitch = 0x79C; // float
        public const uint m_flMoveSoundMaxPitch = 0x7A0; // float
        public const uint m_eOrientationType = 0x7A4; // TrainOrientationType_t
        public const uint m_eVelocityType = 0x7A8; // TrainVelocityType_t
        public const uint m_OnStart = 0x7B8; // CEntityIOOutput
        public const uint m_OnNext = 0x7E0; // CEntityIOOutput
        public const uint m_OnArrivedAtDestinationNode = 0x808; // CEntityIOOutput
        public const uint m_bManualSpeedChanges = 0x830; // bool
        public const uint m_flDesiredSpeed = 0x834; // float
        public const uint m_flSpeedChangeTime = 0x838; // GameTime_t
        public const uint m_flAccelSpeed = 0x83C; // float
        public const uint m_flDecelSpeed = 0x840; // float
        public const uint m_bAccelToSpeed = 0x844; // bool
        public const uint m_flTimeScale = 0x848; // float
        public const uint m_flNextMPSoundTime = 0x84C; // GameTime_t
    }

    public static class CFuncTrain  // CBasePlatTrain
    {
        public const uint m_hCurrentTarget = 0x7A8; // CHandle<CBaseEntity>
        public const uint m_activated = 0x7AC; // bool
        public const uint m_hEnemy = 0x7B0; // CHandle<CBaseEntity>
        public const uint m_flBlockDamage = 0x7B4; // float
        public const uint m_flNextBlockTime = 0x7B8; // GameTime_t
        public const uint m_iszLastTarget = 0x7C0; // CUtlSymbolLarge
    }

    public static class CFuncTrainControls  // CBaseModelEntity
    {
    }

    public static class CFuncVPhysicsClip  // CBaseModelEntity
    {
        public const uint m_bDisabled = 0x700; // bool
    }

    public static class CFuncVehicleClip  // CBaseModelEntity
    {
    }

    public static class CFuncWall  // CBaseModelEntity
    {
        public const uint m_nState = 0x700; // int32_t
    }

    public static class CFuncWallToggle  // CFuncWall
    {
    }

    public static class CFuncWater  // CBaseModelEntity
    {
        public const uint m_BuoyancyHelper = 0x700; // CBuoyancyHelper
    }

    public static class CGameChoreoServices  // IChoreoServices
    {
        public const uint m_hOwner = 0x8; // CHandle<CBaseAnimGraph>
        public const uint m_hScriptedSequence = 0xC; // CHandle<CScriptedSequence>
        public const uint m_scriptState = 0x10; // IChoreoServices::ScriptState_t
        public const uint m_choreoState = 0x14; // IChoreoServices::ChoreoState_t
        public const uint m_flTimeStartedState = 0x18; // GameTime_t
    }

    public static class CGameEnd  // CRulePointEntity
    {
    }

    public static class CGameGibManager  // CBaseEntity
    {
        public const uint m_bAllowNewGibs = 0x4D0; // bool
        public const uint m_iCurrentMaxPieces = 0x4D4; // int32_t
        public const uint m_iMaxPieces = 0x4D8; // int32_t
        public const uint m_iLastFrame = 0x4DC; // int32_t
    }

    public static class CGameMoney  // CRulePointEntity
    {
        public const uint m_OnMoneySpent = 0x710; // CEntityIOOutput
        public const uint m_OnMoneySpentFail = 0x738; // CEntityIOOutput
        public const uint m_nMoney = 0x760; // int32_t
        public const uint m_strAwardText = 0x768; // CUtlString
    }

    public static class CGamePlayerEquip  // CRulePointEntity
    {
    }

    public static class CGamePlayerZone  // CRuleBrushEntity
    {
        public const uint m_OnPlayerInZone = 0x708; // CEntityIOOutput
        public const uint m_OnPlayerOutZone = 0x730; // CEntityIOOutput
        public const uint m_PlayersInCount = 0x758; // CEntityOutputTemplate<int32_t>
        public const uint m_PlayersOutCount = 0x780; // CEntityOutputTemplate<int32_t>
    }

    public static class CGameRules 
    {
        public const uint m_szQuestName = 0x8; // char[128]
        public const uint m_nQuestPhase = 0x88; // int32_t
    }

    public static class CGameRulesProxy  // CBaseEntity
    {
    }

    public static class CGameSceneNode 
    {
        public const uint m_nodeToWorld = 0x10; // CTransform
        public const uint m_pOwner = 0x30; // CEntityInstance*
        public const uint m_pParent = 0x38; // CGameSceneNode*
        public const uint m_pChild = 0x40; // CGameSceneNode*
        public const uint m_pNextSibling = 0x48; // CGameSceneNode*
        public const uint m_hParent = 0x70; // CGameSceneNodeHandle
        public const uint m_vecOrigin = 0x80; // CNetworkOriginCellCoordQuantizedVector
        public const uint m_angRotation = 0xB8; // QAngle
        public const uint m_flScale = 0xC4; // float
        public const uint m_vecAbsOrigin = 0xC8; // Vector
        public const uint m_angAbsRotation = 0xD4; // QAngle
        public const uint m_flAbsScale = 0xE0; // float
        public const uint m_nParentAttachmentOrBone = 0xE4; // int16_t
        public const uint m_bDebugAbsOriginChanges = 0xE6; // bool
        public const uint m_bDormant = 0xE7; // bool
        public const uint m_bForceParentToBeNetworked = 0xE8; // bool
        public const uint m_bDirtyHierarchy = 0x0; // bitfield:1
        public const uint m_bDirtyBoneMergeInfo = 0x0; // bitfield:1
        public const uint m_bNetworkedPositionChanged = 0x0; // bitfield:1
        public const uint m_bNetworkedAnglesChanged = 0x0; // bitfield:1
        public const uint m_bNetworkedScaleChanged = 0x0; // bitfield:1
        public const uint m_bWillBeCallingPostDataUpdate = 0x0; // bitfield:1
        public const uint m_bNotifyBoneTransformsChanged = 0x0; // bitfield:1
        public const uint m_bBoneMergeFlex = 0x0; // bitfield:1
        public const uint m_nLatchAbsOrigin = 0x0; // bitfield:2
        public const uint m_bDirtyBoneMergeBoneToRoot = 0x0; // bitfield:1
        public const uint m_nHierarchicalDepth = 0xEB; // uint8_t
        public const uint m_nHierarchyType = 0xEC; // uint8_t
        public const uint m_nDoNotSetAnimTimeInInvalidatePhysicsCount = 0xED; // uint8_t
        public const uint m_name = 0xF0; // CUtlStringToken
        public const uint m_hierarchyAttachName = 0x130; // CUtlStringToken
        public const uint m_flZOffset = 0x134; // float
        public const uint m_vRenderOrigin = 0x138; // Vector
    }

    public static class CGameSceneNodeHandle 
    {
        public const uint m_hOwner = 0x8; // CEntityHandle
        public const uint m_name = 0xC; // CUtlStringToken
    }

    public static class CGameScriptedMoveData 
    {
        public const uint m_vDest = 0x0; // Vector
        public const uint m_vSrc = 0xC; // Vector
        public const uint m_angSrc = 0x18; // QAngle
        public const uint m_angDst = 0x24; // QAngle
        public const uint m_angCurrent = 0x30; // QAngle
        public const uint m_flAngRate = 0x3C; // float
        public const uint m_flDuration = 0x40; // float
        public const uint m_flStartTime = 0x44; // GameTime_t
        public const uint m_nPrevMoveType = 0x48; // MoveType_t
        public const uint m_bActive = 0x49; // bool
        public const uint m_bTeleportOnEnd = 0x4A; // bool
        public const uint m_bEndOnDestinationReached = 0x4B; // bool
        public const uint m_bIgnoreRotation = 0x4C; // bool
        public const uint m_nType = 0x50; // ScriptedMoveType_t
        public const uint m_bSuccess = 0x54; // bool
        public const uint m_nForcedCrouchState = 0x58; // ForcedCrouchState_t
        public const uint m_bIgnoreCollisions = 0x5C; // bool
    }

    public static class CGameText  // CRulePointEntity
    {
        public const uint m_iszMessage = 0x710; // CUtlSymbolLarge
        public const uint m_textParms = 0x718; // hudtextparms_t
    }

    public static class CGenericConstraint  // CPhysConstraint
    {
        public const uint m_nLinearMotionX = 0x510; // JointMotion_t
        public const uint m_nLinearMotionY = 0x514; // JointMotion_t
        public const uint m_nLinearMotionZ = 0x518; // JointMotion_t
        public const uint m_flLinearFrequencyX = 0x51C; // float
        public const uint m_flLinearFrequencyY = 0x520; // float
        public const uint m_flLinearFrequencyZ = 0x524; // float
        public const uint m_flLinearDampingRatioX = 0x528; // float
        public const uint m_flLinearDampingRatioY = 0x52C; // float
        public const uint m_flLinearDampingRatioZ = 0x530; // float
        public const uint m_flMaxLinearImpulseX = 0x534; // float
        public const uint m_flMaxLinearImpulseY = 0x538; // float
        public const uint m_flMaxLinearImpulseZ = 0x53C; // float
        public const uint m_flBreakAfterTimeX = 0x540; // float
        public const uint m_flBreakAfterTimeY = 0x544; // float
        public const uint m_flBreakAfterTimeZ = 0x548; // float
        public const uint m_flBreakAfterTimeStartTimeX = 0x54C; // GameTime_t
        public const uint m_flBreakAfterTimeStartTimeY = 0x550; // GameTime_t
        public const uint m_flBreakAfterTimeStartTimeZ = 0x554; // GameTime_t
        public const uint m_flBreakAfterTimeThresholdX = 0x558; // float
        public const uint m_flBreakAfterTimeThresholdY = 0x55C; // float
        public const uint m_flBreakAfterTimeThresholdZ = 0x560; // float
        public const uint m_flNotifyForceX = 0x564; // float
        public const uint m_flNotifyForceY = 0x568; // float
        public const uint m_flNotifyForceZ = 0x56C; // float
        public const uint m_flNotifyForceMinTimeX = 0x570; // float
        public const uint m_flNotifyForceMinTimeY = 0x574; // float
        public const uint m_flNotifyForceMinTimeZ = 0x578; // float
        public const uint m_flNotifyForceLastTimeX = 0x57C; // GameTime_t
        public const uint m_flNotifyForceLastTimeY = 0x580; // GameTime_t
        public const uint m_flNotifyForceLastTimeZ = 0x584; // GameTime_t
        public const uint m_bAxisNotifiedX = 0x588; // bool
        public const uint m_bAxisNotifiedY = 0x589; // bool
        public const uint m_bAxisNotifiedZ = 0x58A; // bool
        public const uint m_nAngularMotionX = 0x58C; // JointMotion_t
        public const uint m_nAngularMotionY = 0x590; // JointMotion_t
        public const uint m_nAngularMotionZ = 0x594; // JointMotion_t
        public const uint m_flAngularFrequencyX = 0x598; // float
        public const uint m_flAngularFrequencyY = 0x59C; // float
        public const uint m_flAngularFrequencyZ = 0x5A0; // float
        public const uint m_flAngularDampingRatioX = 0x5A4; // float
        public const uint m_flAngularDampingRatioY = 0x5A8; // float
        public const uint m_flAngularDampingRatioZ = 0x5AC; // float
        public const uint m_flMaxAngularImpulseX = 0x5B0; // float
        public const uint m_flMaxAngularImpulseY = 0x5B4; // float
        public const uint m_flMaxAngularImpulseZ = 0x5B8; // float
        public const uint m_NotifyForceReachedX = 0x5C0; // CEntityIOOutput
        public const uint m_NotifyForceReachedY = 0x5E8; // CEntityIOOutput
        public const uint m_NotifyForceReachedZ = 0x610; // CEntityIOOutput
    }

    public static class CGlowProperty 
    {
        public const uint m_fGlowColor = 0x8; // Vector
        public const uint m_iGlowType = 0x30; // int32_t
        public const uint m_iGlowTeam = 0x34; // int32_t
        public const uint m_nGlowRange = 0x38; // int32_t
        public const uint m_nGlowRangeMin = 0x3C; // int32_t
        public const uint m_glowColorOverride = 0x40; // Color
        public const uint m_bFlashing = 0x44; // bool
        public const uint m_flGlowTime = 0x48; // float
        public const uint m_flGlowStartTime = 0x4C; // float
        public const uint m_bEligibleForScreenHighlight = 0x50; // bool
        public const uint m_bGlowing = 0x51; // bool
    }

    public static class CGradientFog  // CBaseEntity
    {
        public const uint m_hGradientFogTexture = 0x4B0; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_flFogStartDistance = 0x4B8; // float
        public const uint m_flFogEndDistance = 0x4BC; // float
        public const uint m_bHeightFogEnabled = 0x4C0; // bool
        public const uint m_flFogStartHeight = 0x4C4; // float
        public const uint m_flFogEndHeight = 0x4C8; // float
        public const uint m_flFarZ = 0x4CC; // float
        public const uint m_flFogMaxOpacity = 0x4D0; // float
        public const uint m_flFogFalloffExponent = 0x4D4; // float
        public const uint m_flFogVerticalExponent = 0x4D8; // float
        public const uint m_fogColor = 0x4DC; // Color
        public const uint m_flFogStrength = 0x4E0; // float
        public const uint m_flFadeTime = 0x4E4; // float
        public const uint m_bStartDisabled = 0x4E8; // bool
        public const uint m_bIsEnabled = 0x4E9; // bool
        public const uint m_bGradientFogNeedsTextures = 0x4EA; // bool
    }

    public static class CGunTarget  // CBaseToggle
    {
        public const uint m_on = 0x780; // bool
        public const uint m_hTargetEnt = 0x784; // CHandle<CBaseEntity>
        public const uint m_OnDeath = 0x788; // CEntityIOOutput
    }

    public static class CHEGrenade  // CBaseCSGrenade
    {
    }

    public static class CHEGrenadeProjectile  // CBaseCSGrenadeProjectile
    {
    }

    public static class CHandleDummy  // CBaseEntity
    {
    }

    public static class CHandleTest  // CBaseEntity
    {
        public const uint m_Handle = 0x4B0; // CHandle<CBaseEntity>
        public const uint m_bSendHandle = 0x4B4; // bool
    }

    public static class CHintMessage 
    {
        public const uint m_hintString = 0x8; // char*
        public const uint m_args = 0x10; // CUtlVector<char*>
        public const uint m_duration = 0x28; // float
    }

    public static class CHintMessageQueue 
    {
        public const uint m_tmMessageEnd = 0x8; // float
        public const uint m_messages = 0x10; // CUtlVector<CHintMessage*>
        public const uint m_pPlayerController = 0x28; // CBasePlayerController*
    }

    public static class CHitboxComponent  // CEntityComponent
    {
        public const uint m_bvDisabledHitGroups = 0x24; // uint32_t[1]
    }

    public static class CHostage  // CHostageExpresserShim
    {
        public const uint m_OnHostageBeginGrab = 0x9E8; // CEntityIOOutput
        public const uint m_OnFirstPickedUp = 0xA10; // CEntityIOOutput
        public const uint m_OnDroppedNotRescued = 0xA38; // CEntityIOOutput
        public const uint m_OnRescued = 0xA60; // CEntityIOOutput
        public const uint m_entitySpottedState = 0xA88; // EntitySpottedState_t
        public const uint m_nSpotRules = 0xAA0; // int32_t
        public const uint m_uiHostageSpawnExclusionGroupMask = 0xAA4; // uint32_t
        public const uint m_nHostageSpawnRandomFactor = 0xAA8; // uint32_t
        public const uint m_bRemove = 0xAAC; // bool
        public const uint m_vel = 0xAB0; // Vector
        public const uint m_isRescued = 0xABC; // bool
        public const uint m_jumpedThisFrame = 0xABD; // bool
        public const uint m_nHostageState = 0xAC0; // int32_t
        public const uint m_leader = 0xAC4; // CHandle<CBaseEntity>
        public const uint m_lastLeader = 0xAC8; // CHandle<CCSPlayerPawnBase>
        public const uint m_reuseTimer = 0xAD0; // CountdownTimer
        public const uint m_hasBeenUsed = 0xAE8; // bool
        public const uint m_accel = 0xAEC; // Vector
        public const uint m_isRunning = 0xAF8; // bool
        public const uint m_isCrouching = 0xAF9; // bool
        public const uint m_jumpTimer = 0xB00; // CountdownTimer
        public const uint m_isWaitingForLeader = 0xB18; // bool
        public const uint m_repathTimer = 0x2B28; // CountdownTimer
        public const uint m_inhibitDoorTimer = 0x2B40; // CountdownTimer
        public const uint m_inhibitObstacleAvoidanceTimer = 0x2BD0; // CountdownTimer
        public const uint m_wiggleTimer = 0x2BF0; // CountdownTimer
        public const uint m_isAdjusted = 0x2C0C; // bool
        public const uint m_bHandsHaveBeenCut = 0x2C0D; // bool
        public const uint m_hHostageGrabber = 0x2C10; // CHandle<CCSPlayerPawn>
        public const uint m_fLastGrabTime = 0x2C14; // GameTime_t
        public const uint m_vecPositionWhenStartedDroppingToGround = 0x2C18; // Vector
        public const uint m_vecGrabbedPos = 0x2C24; // Vector
        public const uint m_flRescueStartTime = 0x2C30; // GameTime_t
        public const uint m_flGrabSuccessTime = 0x2C34; // GameTime_t
        public const uint m_flDropStartTime = 0x2C38; // GameTime_t
        public const uint m_nApproachRewardPayouts = 0x2C3C; // int32_t
        public const uint m_nPickupEventCount = 0x2C40; // int32_t
        public const uint m_vecSpawnGroundPos = 0x2C44; // Vector
        public const uint m_vecHostageResetPosition = 0x2C64; // Vector
    }

    public static class CHostageAlias_info_hostage_spawn  // CHostage
    {
    }

    public static class CHostageCarriableProp  // CBaseAnimGraph
    {
    }

    public static class CHostageExpresserShim  // CBaseCombatCharacter
    {
        public const uint m_pExpresser = 0x9D0; // CAI_Expresser*
    }

    public static class CHostageRescueZone  // CHostageRescueZoneShim
    {
    }

    public static class CHostageRescueZoneShim  // CBaseTrigger
    {
    }

    public static class CInButtonState 
    {
        public const uint m_pButtonStates = 0x8; // uint64_t[3]
    }

    public static class CIncendiaryGrenade  // CMolotovGrenade
    {
    }

    public static class CInferno  // CBaseModelEntity
    {
        public const uint m_firePositions = 0x710; // Vector[64]
        public const uint m_fireParentPositions = 0xA10; // Vector[64]
        public const uint m_bFireIsBurning = 0xD10; // bool[64]
        public const uint m_BurnNormal = 0xD50; // Vector[64]
        public const uint m_fireCount = 0x1050; // int32_t
        public const uint m_nInfernoType = 0x1054; // int32_t
        public const uint m_nFireEffectTickBegin = 0x1058; // int32_t
        public const uint m_nFireLifetime = 0x105C; // float
        public const uint m_bInPostEffectTime = 0x1060; // bool
        public const uint m_nFiresExtinguishCount = 0x1064; // int32_t
        public const uint m_bWasCreatedInSmoke = 0x1068; // bool
        public const uint m_extent = 0x1270; // Extent
        public const uint m_damageTimer = 0x1288; // CountdownTimer
        public const uint m_damageRampTimer = 0x12A0; // CountdownTimer
        public const uint m_splashVelocity = 0x12B8; // Vector
        public const uint m_InitialSplashVelocity = 0x12C4; // Vector
        public const uint m_startPos = 0x12D0; // Vector
        public const uint m_vecOriginalSpawnLocation = 0x12DC; // Vector
        public const uint m_activeTimer = 0x12E8; // IntervalTimer
        public const uint m_fireSpawnOffset = 0x12F8; // int32_t
        public const uint m_nMaxFlames = 0x12FC; // int32_t
        public const uint m_BookkeepingTimer = 0x1300; // CountdownTimer
        public const uint m_NextSpreadTimer = 0x1318; // CountdownTimer
        public const uint m_nSourceItemDefIndex = 0x1330; // uint16_t
    }

    public static class CInfoData  // CServerOnlyEntity
    {
    }

    public static class CInfoDeathmatchSpawn  // SpawnPoint
    {
    }

    public static class CInfoDynamicShadowHint  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_flRange = 0x4B4; // float
        public const uint m_nImportance = 0x4B8; // int32_t
        public const uint m_nLightChoice = 0x4BC; // int32_t
        public const uint m_hLight = 0x4C0; // CHandle<CBaseEntity>
    }

    public static class CInfoDynamicShadowHintBox  // CInfoDynamicShadowHint
    {
        public const uint m_vBoxMins = 0x4C8; // Vector
        public const uint m_vBoxMaxs = 0x4D4; // Vector
    }

    public static class CInfoEnemyTerroristSpawn  // SpawnPointCoopEnemy
    {
    }

    public static class CInfoGameEventProxy  // CPointEntity
    {
        public const uint m_iszEventName = 0x4B0; // CUtlSymbolLarge
        public const uint m_flRange = 0x4B8; // float
    }

    public static class CInfoInstructorHintBombTargetA  // CPointEntity
    {
    }

    public static class CInfoInstructorHintBombTargetB  // CPointEntity
    {
    }

    public static class CInfoInstructorHintHostageRescueZone  // CPointEntity
    {
    }

    public static class CInfoInstructorHintTarget  // CPointEntity
    {
    }

    public static class CInfoLadderDismount  // CBaseEntity
    {
    }

    public static class CInfoLandmark  // CPointEntity
    {
    }

    public static class CInfoOffscreenPanoramaTexture  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_nResolutionX = 0x4B4; // int32_t
        public const uint m_nResolutionY = 0x4B8; // int32_t
        public const uint m_szLayoutFileName = 0x4C0; // CUtlSymbolLarge
        public const uint m_RenderAttrName = 0x4C8; // CUtlSymbolLarge
        public const uint m_TargetEntities = 0x4D0; // CNetworkUtlVectorBase<CHandle<CBaseModelEntity>>
        public const uint m_nTargetChangeCount = 0x4E8; // int32_t
        public const uint m_vecCSSClasses = 0x4F0; // CNetworkUtlVectorBase<CUtlSymbolLarge>
        public const uint m_szTargetsName = 0x508; // CUtlSymbolLarge
        public const uint m_AdditionalTargetEntities = 0x510; // CUtlVector<CHandle<CBaseModelEntity>>
    }

    public static class CInfoParticleTarget  // CPointEntity
    {
    }

    public static class CInfoPlayerCounterterrorist  // SpawnPoint
    {
    }

    public static class CInfoPlayerStart  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
    }

    public static class CInfoPlayerTerrorist  // SpawnPoint
    {
    }

    public static class CInfoSpawnGroupLandmark  // CPointEntity
    {
    }

    public static class CInfoSpawnGroupLoadUnload  // CLogicalEntity
    {
        public const uint m_OnSpawnGroupLoadStarted = 0x4B0; // CEntityIOOutput
        public const uint m_OnSpawnGroupLoadFinished = 0x4D8; // CEntityIOOutput
        public const uint m_OnSpawnGroupUnloadStarted = 0x500; // CEntityIOOutput
        public const uint m_OnSpawnGroupUnloadFinished = 0x528; // CEntityIOOutput
        public const uint m_iszSpawnGroupName = 0x550; // CUtlSymbolLarge
        public const uint m_iszSpawnGroupFilterName = 0x558; // CUtlSymbolLarge
        public const uint m_iszLandmarkName = 0x560; // CUtlSymbolLarge
        public const uint m_sFixedSpawnGroupName = 0x568; // CUtlString
        public const uint m_flTimeoutInterval = 0x570; // float
        public const uint m_bStreamingStarted = 0x574; // bool
        public const uint m_bUnloadingStarted = 0x575; // bool
    }

    public static class CInfoTarget  // CPointEntity
    {
    }

    public static class CInfoTargetServerOnly  // CServerOnlyPointEntity
    {
    }

    public static class CInfoTeleportDestination  // CPointEntity
    {
    }

    public static class CInfoVisibilityBox  // CBaseEntity
    {
        public const uint m_nMode = 0x4B4; // int32_t
        public const uint m_vBoxSize = 0x4B8; // Vector
        public const uint m_bEnabled = 0x4C4; // bool
    }

    public static class CInfoWorldLayer  // CBaseEntity
    {
        public const uint m_pOutputOnEntitiesSpawned = 0x4B0; // CEntityIOOutput
        public const uint m_worldName = 0x4D8; // CUtlSymbolLarge
        public const uint m_layerName = 0x4E0; // CUtlSymbolLarge
        public const uint m_bWorldLayerVisible = 0x4E8; // bool
        public const uint m_bEntitiesSpawned = 0x4E9; // bool
        public const uint m_bCreateAsChildSpawnGroup = 0x4EA; // bool
        public const uint m_hLayerSpawnGroup = 0x4EC; // uint32_t
    }

    public static class CInstancedSceneEntity  // CSceneEntity
    {
        public const uint m_hOwner = 0xA08; // CHandle<CBaseEntity>
        public const uint m_bHadOwner = 0xA0C; // bool
        public const uint m_flPostSpeakDelay = 0xA10; // float
        public const uint m_flPreDelay = 0xA14; // float
        public const uint m_bIsBackground = 0xA18; // bool
    }

    public static class CInstructorEventEntity  // CPointEntity
    {
        public const uint m_iszName = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszHintTargetEntity = 0x4B8; // CUtlSymbolLarge
        public const uint m_hTargetPlayer = 0x4C0; // CHandle<CBasePlayerPawn>
    }

    public static class CIronSightController 
    {
        public const uint m_bIronSightAvailable = 0x8; // bool
        public const uint m_flIronSightAmount = 0xC; // float
        public const uint m_flIronSightAmountGained = 0x10; // float
        public const uint m_flIronSightAmountBiased = 0x14; // float
    }

    public static class CItem  // CBaseAnimGraph
    {
        public const uint m_OnPlayerTouch = 0x898; // CEntityIOOutput
        public const uint m_bActivateWhenAtRest = 0x8C0; // bool
        public const uint m_OnCacheInteraction = 0x8C8; // CEntityIOOutput
        public const uint m_OnPlayerPickup = 0x8F0; // CEntityIOOutput
        public const uint m_OnGlovePulled = 0x918; // CEntityIOOutput
        public const uint m_vOriginalSpawnOrigin = 0x940; // Vector
        public const uint m_vOriginalSpawnAngles = 0x94C; // QAngle
        public const uint m_bPhysStartAsleep = 0x958; // bool
    }

    public static class CItemAssaultSuit  // CItem
    {
    }

    public static class CItemDefuser  // CItem
    {
        public const uint m_entitySpottedState = 0x968; // EntitySpottedState_t
        public const uint m_nSpotRules = 0x980; // int32_t
    }

    public static class CItemDefuserAlias_item_defuser  // CItemDefuser
    {
    }

    public static class CItemDogtags  // CItem
    {
        public const uint m_OwningPlayer = 0x968; // CHandle<CCSPlayerPawn>
        public const uint m_KillingPlayer = 0x96C; // CHandle<CCSPlayerPawn>
    }

    public static class CItemGeneric  // CItem
    {
        public const uint m_bHasTriggerRadius = 0x970; // bool
        public const uint m_bHasPickupRadius = 0x971; // bool
        public const uint m_flPickupRadiusSqr = 0x974; // float
        public const uint m_flTriggerRadiusSqr = 0x978; // float
        public const uint m_flLastPickupCheck = 0x97C; // GameTime_t
        public const uint m_bPlayerCounterListenerAdded = 0x980; // bool
        public const uint m_bPlayerInTriggerRadius = 0x981; // bool
        public const uint m_hSpawnParticleEffect = 0x988; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_pAmbientSoundEffect = 0x990; // CUtlSymbolLarge
        public const uint m_bAutoStartAmbientSound = 0x998; // bool
        public const uint m_pSpawnScriptFunction = 0x9A0; // CUtlSymbolLarge
        public const uint m_hPickupParticleEffect = 0x9A8; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_pPickupSoundEffect = 0x9B0; // CUtlSymbolLarge
        public const uint m_pPickupScriptFunction = 0x9B8; // CUtlSymbolLarge
        public const uint m_hTimeoutParticleEffect = 0x9C0; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_pTimeoutSoundEffect = 0x9C8; // CUtlSymbolLarge
        public const uint m_pTimeoutScriptFunction = 0x9D0; // CUtlSymbolLarge
        public const uint m_pPickupFilterName = 0x9D8; // CUtlSymbolLarge
        public const uint m_hPickupFilter = 0x9E0; // CHandle<CBaseFilter>
        public const uint m_OnPickup = 0x9E8; // CEntityIOOutput
        public const uint m_OnTimeout = 0xA10; // CEntityIOOutput
        public const uint m_OnTriggerStartTouch = 0xA38; // CEntityIOOutput
        public const uint m_OnTriggerTouch = 0xA60; // CEntityIOOutput
        public const uint m_OnTriggerEndTouch = 0xA88; // CEntityIOOutput
        public const uint m_pAllowPickupScriptFunction = 0xAB0; // CUtlSymbolLarge
        public const uint m_flPickupRadius = 0xAB8; // float
        public const uint m_flTriggerRadius = 0xABC; // float
        public const uint m_pTriggerSoundEffect = 0xAC0; // CUtlSymbolLarge
        public const uint m_bGlowWhenInTrigger = 0xAC8; // bool
        public const uint m_glowColor = 0xAC9; // Color
        public const uint m_bUseable = 0xACD; // bool
        public const uint m_hTriggerHelper = 0xAD0; // CHandle<CItemGenericTriggerHelper>
    }

    public static class CItemGenericTriggerHelper  // CBaseModelEntity
    {
        public const uint m_hParentItem = 0x700; // CHandle<CItemGeneric>
    }

    public static class CItemHeavyAssaultSuit  // CItemAssaultSuit
    {
    }

    public static class CItemKevlar  // CItem
    {
    }

    public static class CItemSoda  // CBaseAnimGraph
    {
    }

    public static class CItem_Healthshot  // CWeaponBaseItem
    {
    }

    public static class CKeepUpright  // CPointEntity
    {
        public const uint m_worldGoalAxis = 0x4B8; // Vector
        public const uint m_localTestAxis = 0x4C4; // Vector
        public const uint m_nameAttach = 0x4D8; // CUtlSymbolLarge
        public const uint m_attachedObject = 0x4E0; // CHandle<CBaseEntity>
        public const uint m_angularLimit = 0x4E4; // float
        public const uint m_bActive = 0x4E8; // bool
        public const uint m_bDampAllRotation = 0x4E9; // bool
    }

    public static class CKnife  // CCSWeaponBase
    {
        public const uint m_bFirstAttack = 0xE28; // bool
    }

    public static class CLightComponent  // CEntityComponent
    {
        public const uint __m_pChainEntity = 0x48; // CNetworkVarChainer
        public const uint m_Color = 0x85; // Color
        public const uint m_SecondaryColor = 0x89; // Color
        public const uint m_flBrightness = 0x90; // float
        public const uint m_flBrightnessScale = 0x94; // float
        public const uint m_flBrightnessMult = 0x98; // float
        public const uint m_flRange = 0x9C; // float
        public const uint m_flFalloff = 0xA0; // float
        public const uint m_flAttenuation0 = 0xA4; // float
        public const uint m_flAttenuation1 = 0xA8; // float
        public const uint m_flAttenuation2 = 0xAC; // float
        public const uint m_flTheta = 0xB0; // float
        public const uint m_flPhi = 0xB4; // float
        public const uint m_hLightCookie = 0xB8; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_nCascades = 0xC0; // int32_t
        public const uint m_nCastShadows = 0xC4; // int32_t
        public const uint m_nShadowWidth = 0xC8; // int32_t
        public const uint m_nShadowHeight = 0xCC; // int32_t
        public const uint m_bRenderDiffuse = 0xD0; // bool
        public const uint m_nRenderSpecular = 0xD4; // int32_t
        public const uint m_bRenderTransmissive = 0xD8; // bool
        public const uint m_flOrthoLightWidth = 0xDC; // float
        public const uint m_flOrthoLightHeight = 0xE0; // float
        public const uint m_nStyle = 0xE4; // int32_t
        public const uint m_Pattern = 0xE8; // CUtlString
        public const uint m_nCascadeRenderStaticObjects = 0xF0; // int32_t
        public const uint m_flShadowCascadeCrossFade = 0xF4; // float
        public const uint m_flShadowCascadeDistanceFade = 0xF8; // float
        public const uint m_flShadowCascadeDistance0 = 0xFC; // float
        public const uint m_flShadowCascadeDistance1 = 0x100; // float
        public const uint m_flShadowCascadeDistance2 = 0x104; // float
        public const uint m_flShadowCascadeDistance3 = 0x108; // float
        public const uint m_nShadowCascadeResolution0 = 0x10C; // int32_t
        public const uint m_nShadowCascadeResolution1 = 0x110; // int32_t
        public const uint m_nShadowCascadeResolution2 = 0x114; // int32_t
        public const uint m_nShadowCascadeResolution3 = 0x118; // int32_t
        public const uint m_bUsesBakedShadowing = 0x11C; // bool
        public const uint m_nShadowPriority = 0x120; // int32_t
        public const uint m_nBakedShadowIndex = 0x124; // int32_t
        public const uint m_bRenderToCubemaps = 0x128; // bool
        public const uint m_nDirectLight = 0x12C; // int32_t
        public const uint m_nIndirectLight = 0x130; // int32_t
        public const uint m_flFadeMinDist = 0x134; // float
        public const uint m_flFadeMaxDist = 0x138; // float
        public const uint m_flShadowFadeMinDist = 0x13C; // float
        public const uint m_flShadowFadeMaxDist = 0x140; // float
        public const uint m_bEnabled = 0x144; // bool
        public const uint m_bFlicker = 0x145; // bool
        public const uint m_bPrecomputedFieldsValid = 0x146; // bool
        public const uint m_vPrecomputedBoundsMins = 0x148; // Vector
        public const uint m_vPrecomputedBoundsMaxs = 0x154; // Vector
        public const uint m_vPrecomputedOBBOrigin = 0x160; // Vector
        public const uint m_vPrecomputedOBBAngles = 0x16C; // QAngle
        public const uint m_vPrecomputedOBBExtent = 0x178; // Vector
        public const uint m_flPrecomputedMaxRange = 0x184; // float
        public const uint m_nFogLightingMode = 0x188; // int32_t
        public const uint m_flFogContributionStength = 0x18C; // float
        public const uint m_flNearClipPlane = 0x190; // float
        public const uint m_SkyColor = 0x194; // Color
        public const uint m_flSkyIntensity = 0x198; // float
        public const uint m_SkyAmbientBounce = 0x19C; // Color
        public const uint m_bUseSecondaryColor = 0x1A0; // bool
        public const uint m_bMixedShadows = 0x1A1; // bool
        public const uint m_flLightStyleStartTime = 0x1A4; // GameTime_t
        public const uint m_flCapsuleLength = 0x1A8; // float
        public const uint m_flMinRoughness = 0x1AC; // float
        public const uint m_bPvsModifyEntity = 0x1C0; // bool
    }

    public static class CLightDirectionalEntity  // CLightEntity
    {
    }

    public static class CLightEntity  // CBaseModelEntity
    {
        public const uint m_CLightComponent = 0x700; // CLightComponent*
    }

    public static class CLightEnvironmentEntity  // CLightDirectionalEntity
    {
    }

    public static class CLightGlow  // CBaseModelEntity
    {
        public const uint m_nHorizontalSize = 0x700; // uint32_t
        public const uint m_nVerticalSize = 0x704; // uint32_t
        public const uint m_nMinDist = 0x708; // uint32_t
        public const uint m_nMaxDist = 0x70C; // uint32_t
        public const uint m_nOuterMaxDist = 0x710; // uint32_t
        public const uint m_flGlowProxySize = 0x714; // float
        public const uint m_flHDRColorScale = 0x718; // float
    }

    public static class CLightOrthoEntity  // CLightEntity
    {
    }

    public static class CLightSpotEntity  // CLightEntity
    {
    }

    public static class CLogicAchievement  // CLogicalEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_iszAchievementEventID = 0x4B8; // CUtlSymbolLarge
        public const uint m_OnFired = 0x4C0; // CEntityIOOutput
    }

    public static class CLogicActiveAutosave  // CLogicAutosave
    {
        public const uint m_TriggerHitPoints = 0x4C0; // int32_t
        public const uint m_flTimeToTrigger = 0x4C4; // float
        public const uint m_flStartTime = 0x4C8; // GameTime_t
        public const uint m_flDangerousTime = 0x4CC; // float
    }

    public static class CLogicAuto  // CBaseEntity
    {
        public const uint m_OnMapSpawn = 0x4B0; // CEntityIOOutput
        public const uint m_OnDemoMapSpawn = 0x4D8; // CEntityIOOutput
        public const uint m_OnNewGame = 0x500; // CEntityIOOutput
        public const uint m_OnLoadGame = 0x528; // CEntityIOOutput
        public const uint m_OnMapTransition = 0x550; // CEntityIOOutput
        public const uint m_OnBackgroundMap = 0x578; // CEntityIOOutput
        public const uint m_OnMultiNewMap = 0x5A0; // CEntityIOOutput
        public const uint m_OnMultiNewRound = 0x5C8; // CEntityIOOutput
        public const uint m_OnVREnabled = 0x5F0; // CEntityIOOutput
        public const uint m_OnVRNotEnabled = 0x618; // CEntityIOOutput
        public const uint m_globalstate = 0x640; // CUtlSymbolLarge
    }

    public static class CLogicAutosave  // CLogicalEntity
    {
        public const uint m_bForceNewLevelUnit = 0x4B0; // bool
        public const uint m_minHitPoints = 0x4B4; // int32_t
        public const uint m_minHitPointsToCommit = 0x4B8; // int32_t
    }

    public static class CLogicBranch  // CLogicalEntity
    {
        public const uint m_bInValue = 0x4B0; // bool
        public const uint m_Listeners = 0x4B8; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_OnTrue = 0x4D0; // CEntityIOOutput
        public const uint m_OnFalse = 0x4F8; // CEntityIOOutput
    }

    public static class CLogicBranchList  // CLogicalEntity
    {
        public const uint m_nLogicBranchNames = 0x4B0; // CUtlSymbolLarge[16]
        public const uint m_LogicBranchList = 0x530; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_eLastState = 0x548; // CLogicBranchList::LogicBranchListenerLastState_t
        public const uint m_OnAllTrue = 0x550; // CEntityIOOutput
        public const uint m_OnAllFalse = 0x578; // CEntityIOOutput
        public const uint m_OnMixed = 0x5A0; // CEntityIOOutput
    }

    public static class CLogicCase  // CLogicalEntity
    {
        public const uint m_nCase = 0x4B0; // CUtlSymbolLarge[32]
        public const uint m_nShuffleCases = 0x5B0; // int32_t
        public const uint m_nLastShuffleCase = 0x5B4; // int32_t
        public const uint m_uchShuffleCaseMap = 0x5B8; // uint8_t[32]
        public const uint m_OnCase = 0x5D8; // CEntityIOOutput[32]
        public const uint m_OnDefault = 0xAD8; // CEntityOutputTemplate<CVariantBase<CVariantDefaultAllocator>>
    }

    public static class CLogicCollisionPair  // CLogicalEntity
    {
        public const uint m_nameAttach1 = 0x4B0; // CUtlSymbolLarge
        public const uint m_nameAttach2 = 0x4B8; // CUtlSymbolLarge
        public const uint m_disabled = 0x4C0; // bool
        public const uint m_succeeded = 0x4C1; // bool
    }

    public static class CLogicCompare  // CLogicalEntity
    {
        public const uint m_flInValue = 0x4B0; // float
        public const uint m_flCompareValue = 0x4B4; // float
        public const uint m_OnLessThan = 0x4B8; // CEntityOutputTemplate<float>
        public const uint m_OnEqualTo = 0x4E0; // CEntityOutputTemplate<float>
        public const uint m_OnNotEqualTo = 0x508; // CEntityOutputTemplate<float>
        public const uint m_OnGreaterThan = 0x530; // CEntityOutputTemplate<float>
    }

    public static class CLogicDistanceAutosave  // CLogicalEntity
    {
        public const uint m_iszTargetEntity = 0x4B0; // CUtlSymbolLarge
        public const uint m_flDistanceToPlayer = 0x4B8; // float
        public const uint m_bForceNewLevelUnit = 0x4BC; // bool
        public const uint m_bCheckCough = 0x4BD; // bool
        public const uint m_bThinkDangerous = 0x4BE; // bool
        public const uint m_flDangerousTime = 0x4C0; // float
    }

    public static class CLogicDistanceCheck  // CLogicalEntity
    {
        public const uint m_iszEntityA = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszEntityB = 0x4B8; // CUtlSymbolLarge
        public const uint m_flZone1Distance = 0x4C0; // float
        public const uint m_flZone2Distance = 0x4C4; // float
        public const uint m_InZone1 = 0x4C8; // CEntityIOOutput
        public const uint m_InZone2 = 0x4F0; // CEntityIOOutput
        public const uint m_InZone3 = 0x518; // CEntityIOOutput
    }

    public static class CLogicEventListener  // CLogicalEntity
    {
        public const uint m_strEventName = 0x4C0; // CUtlString
        public const uint m_bIsEnabled = 0x4C8; // bool
        public const uint m_nTeam = 0x4CC; // int32_t
        public const uint m_OnEventFired = 0x4D0; // CEntityIOOutput
    }

    public static class CLogicGameEvent  // CLogicalEntity
    {
        public const uint m_iszEventName = 0x4B0; // CUtlSymbolLarge
    }

    public static class CLogicGameEventListener  // CLogicalEntity
    {
        public const uint m_OnEventFired = 0x4C0; // CEntityIOOutput
        public const uint m_iszGameEventName = 0x4E8; // CUtlSymbolLarge
        public const uint m_iszGameEventItem = 0x4F0; // CUtlSymbolLarge
        public const uint m_bEnabled = 0x4F8; // bool
        public const uint m_bStartDisabled = 0x4F9; // bool
    }

    public static class CLogicLineToEntity  // CLogicalEntity
    {
        public const uint m_Line = 0x4B0; // CEntityOutputTemplate<Vector>
        public const uint m_SourceName = 0x4D8; // CUtlSymbolLarge
        public const uint m_StartEntity = 0x4E0; // CHandle<CBaseEntity>
        public const uint m_EndEntity = 0x4E4; // CHandle<CBaseEntity>
    }

    public static class CLogicMeasureMovement  // CLogicalEntity
    {
        public const uint m_strMeasureTarget = 0x4B0; // CUtlSymbolLarge
        public const uint m_strMeasureReference = 0x4B8; // CUtlSymbolLarge
        public const uint m_strTargetReference = 0x4C0; // CUtlSymbolLarge
        public const uint m_hMeasureTarget = 0x4C8; // CHandle<CBaseEntity>
        public const uint m_hMeasureReference = 0x4CC; // CHandle<CBaseEntity>
        public const uint m_hTarget = 0x4D0; // CHandle<CBaseEntity>
        public const uint m_hTargetReference = 0x4D4; // CHandle<CBaseEntity>
        public const uint m_flScale = 0x4D8; // float
        public const uint m_nMeasureType = 0x4DC; // int32_t
    }

    public static class CLogicNPCCounter  // CBaseEntity
    {
        public const uint m_OnMinCountAll = 0x4B0; // CEntityIOOutput
        public const uint m_OnMaxCountAll = 0x4D8; // CEntityIOOutput
        public const uint m_OnFactorAll = 0x500; // CEntityOutputTemplate<float>
        public const uint m_OnMinPlayerDistAll = 0x528; // CEntityOutputTemplate<float>
        public const uint m_OnMinCount_1 = 0x550; // CEntityIOOutput
        public const uint m_OnMaxCount_1 = 0x578; // CEntityIOOutput
        public const uint m_OnFactor_1 = 0x5A0; // CEntityOutputTemplate<float>
        public const uint m_OnMinPlayerDist_1 = 0x5C8; // CEntityOutputTemplate<float>
        public const uint m_OnMinCount_2 = 0x5F0; // CEntityIOOutput
        public const uint m_OnMaxCount_2 = 0x618; // CEntityIOOutput
        public const uint m_OnFactor_2 = 0x640; // CEntityOutputTemplate<float>
        public const uint m_OnMinPlayerDist_2 = 0x668; // CEntityOutputTemplate<float>
        public const uint m_OnMinCount_3 = 0x690; // CEntityIOOutput
        public const uint m_OnMaxCount_3 = 0x6B8; // CEntityIOOutput
        public const uint m_OnFactor_3 = 0x6E0; // CEntityOutputTemplate<float>
        public const uint m_OnMinPlayerDist_3 = 0x708; // CEntityOutputTemplate<float>
        public const uint m_hSource = 0x730; // CEntityHandle
        public const uint m_iszSourceEntityName = 0x738; // CUtlSymbolLarge
        public const uint m_flDistanceMax = 0x740; // float
        public const uint m_bDisabled = 0x744; // bool
        public const uint m_nMinCountAll = 0x748; // int32_t
        public const uint m_nMaxCountAll = 0x74C; // int32_t
        public const uint m_nMinFactorAll = 0x750; // int32_t
        public const uint m_nMaxFactorAll = 0x754; // int32_t
        public const uint m_iszNPCClassname_1 = 0x760; // CUtlSymbolLarge
        public const uint m_nNPCState_1 = 0x768; // int32_t
        public const uint m_bInvertState_1 = 0x76C; // bool
        public const uint m_nMinCount_1 = 0x770; // int32_t
        public const uint m_nMaxCount_1 = 0x774; // int32_t
        public const uint m_nMinFactor_1 = 0x778; // int32_t
        public const uint m_nMaxFactor_1 = 0x77C; // int32_t
        public const uint m_flDefaultDist_1 = 0x784; // float
        public const uint m_iszNPCClassname_2 = 0x788; // CUtlSymbolLarge
        public const uint m_nNPCState_2 = 0x790; // int32_t
        public const uint m_bInvertState_2 = 0x794; // bool
        public const uint m_nMinCount_2 = 0x798; // int32_t
        public const uint m_nMaxCount_2 = 0x79C; // int32_t
        public const uint m_nMinFactor_2 = 0x7A0; // int32_t
        public const uint m_nMaxFactor_2 = 0x7A4; // int32_t
        public const uint m_flDefaultDist_2 = 0x7AC; // float
        public const uint m_iszNPCClassname_3 = 0x7B0; // CUtlSymbolLarge
        public const uint m_nNPCState_3 = 0x7B8; // int32_t
        public const uint m_bInvertState_3 = 0x7BC; // bool
        public const uint m_nMinCount_3 = 0x7C0; // int32_t
        public const uint m_nMaxCount_3 = 0x7C4; // int32_t
        public const uint m_nMinFactor_3 = 0x7C8; // int32_t
        public const uint m_nMaxFactor_3 = 0x7CC; // int32_t
        public const uint m_flDefaultDist_3 = 0x7D4; // float
    }

    public static class CLogicNPCCounterAABB  // CLogicNPCCounter
    {
        public const uint m_vDistanceOuterMins = 0x7F0; // Vector
        public const uint m_vDistanceOuterMaxs = 0x7FC; // Vector
        public const uint m_vOuterMins = 0x808; // Vector
        public const uint m_vOuterMaxs = 0x814; // Vector
    }

    public static class CLogicNPCCounterOBB  // CLogicNPCCounterAABB
    {
    }

    public static class CLogicNavigation  // CLogicalEntity
    {
        public const uint m_isOn = 0x4B8; // bool
        public const uint m_navProperty = 0x4BC; // navproperties_t
    }

    public static class CLogicPlayerProxy  // CLogicalEntity
    {
        public const uint m_hPlayer = 0x4B0; // CHandle<CBaseEntity>
        public const uint m_PlayerHasAmmo = 0x4B8; // CEntityIOOutput
        public const uint m_PlayerHasNoAmmo = 0x4E0; // CEntityIOOutput
        public const uint m_PlayerDied = 0x508; // CEntityIOOutput
        public const uint m_RequestedPlayerHealth = 0x530; // CEntityOutputTemplate<int32_t>
    }

    public static class CLogicProximity  // CPointEntity
    {
    }

    public static class CLogicRelay  // CLogicalEntity
    {
        public const uint m_OnTrigger = 0x4B0; // CEntityIOOutput
        public const uint m_OnSpawn = 0x4D8; // CEntityIOOutput
        public const uint m_bDisabled = 0x500; // bool
        public const uint m_bWaitForRefire = 0x501; // bool
        public const uint m_bTriggerOnce = 0x502; // bool
        public const uint m_bFastRetrigger = 0x503; // bool
        public const uint m_bPassthoughCaller = 0x504; // bool
    }

    public static class CLogicScript  // CPointEntity
    {
    }

    public static class CLogicalEntity  // CServerOnlyEntity
    {
    }

    public static class CMapInfo  // CPointEntity
    {
        public const uint m_iBuyingStatus = 0x4B0; // int32_t
        public const uint m_flBombRadius = 0x4B4; // float
        public const uint m_iPetPopulation = 0x4B8; // int32_t
        public const uint m_bUseNormalSpawnsForDM = 0x4BC; // bool
        public const uint m_bDisableAutoGeneratedDMSpawns = 0x4BD; // bool
        public const uint m_flBotMaxVisionDistance = 0x4C0; // float
        public const uint m_iHostageCount = 0x4C4; // int32_t
        public const uint m_bFadePlayerVisibilityFarZ = 0x4C8; // bool
    }

    public static class CMapVetoPickController  // CBaseEntity
    {
        public const uint m_bPlayedIntroVcd = 0x4B0; // bool
        public const uint m_bNeedToPlayFiveSecondsRemaining = 0x4B1; // bool
        public const uint m_dblPreMatchDraftSequenceTime = 0x4D0; // double
        public const uint m_bPreMatchDraftStateChanged = 0x4D8; // bool
        public const uint m_nDraftType = 0x4DC; // int32_t
        public const uint m_nTeamWinningCoinToss = 0x4E0; // int32_t
        public const uint m_nTeamWithFirstChoice = 0x4E4; // int32_t[64]
        public const uint m_nVoteMapIdsList = 0x5E4; // int32_t[7]
        public const uint m_nAccountIDs = 0x600; // int32_t[64]
        public const uint m_nMapId0 = 0x700; // int32_t[64]
        public const uint m_nMapId1 = 0x800; // int32_t[64]
        public const uint m_nMapId2 = 0x900; // int32_t[64]
        public const uint m_nMapId3 = 0xA00; // int32_t[64]
        public const uint m_nMapId4 = 0xB00; // int32_t[64]
        public const uint m_nMapId5 = 0xC00; // int32_t[64]
        public const uint m_nStartingSide0 = 0xD00; // int32_t[64]
        public const uint m_nCurrentPhase = 0xE00; // int32_t
        public const uint m_nPhaseStartTick = 0xE04; // int32_t
        public const uint m_nPhaseDurationTicks = 0xE08; // int32_t
        public const uint m_OnMapVetoed = 0xE10; // CEntityOutputTemplate<CUtlSymbolLarge>
        public const uint m_OnMapPicked = 0xE38; // CEntityOutputTemplate<CUtlSymbolLarge>
        public const uint m_OnSidesPicked = 0xE60; // CEntityOutputTemplate<int32_t>
        public const uint m_OnNewPhaseStarted = 0xE88; // CEntityOutputTemplate<int32_t>
        public const uint m_OnLevelTransition = 0xEB0; // CEntityOutputTemplate<int32_t>
    }

    public static class CMarkupVolume  // CBaseModelEntity
    {
        public const uint m_bEnabled = 0x700; // bool
    }

    public static class CMarkupVolumeTagged  // CMarkupVolume
    {
        public const uint m_bIsGroup = 0x738; // bool
        public const uint m_bGroupByPrefab = 0x739; // bool
        public const uint m_bGroupByVolume = 0x73A; // bool
        public const uint m_bGroupOtherGroups = 0x73B; // bool
        public const uint m_bIsInGroup = 0x73C; // bool
    }

    public static class CMarkupVolumeTagged_Nav  // CMarkupVolumeTagged
    {
    }

    public static class CMarkupVolumeTagged_NavGame  // CMarkupVolumeWithRef
    {
        public const uint m_bFloodFillAttribute = 0x758; // bool
    }

    public static class CMarkupVolumeWithRef  // CMarkupVolumeTagged
    {
        public const uint m_bUseRef = 0x740; // bool
        public const uint m_vRefPos = 0x744; // Vector
        public const uint m_flRefDot = 0x750; // float
    }

    public static class CMathColorBlend  // CLogicalEntity
    {
        public const uint m_flInMin = 0x4B0; // float
        public const uint m_flInMax = 0x4B4; // float
        public const uint m_OutColor1 = 0x4B8; // Color
        public const uint m_OutColor2 = 0x4BC; // Color
        public const uint m_OutValue = 0x4C0; // CEntityOutputTemplate<Color>
    }

    public static class CMathCounter  // CLogicalEntity
    {
        public const uint m_flMin = 0x4B0; // float
        public const uint m_flMax = 0x4B4; // float
        public const uint m_bHitMin = 0x4B8; // bool
        public const uint m_bHitMax = 0x4B9; // bool
        public const uint m_bDisabled = 0x4BA; // bool
        public const uint m_OutValue = 0x4C0; // CEntityOutputTemplate<float>
        public const uint m_OnGetValue = 0x4E8; // CEntityOutputTemplate<float>
        public const uint m_OnHitMin = 0x510; // CEntityIOOutput
        public const uint m_OnHitMax = 0x538; // CEntityIOOutput
        public const uint m_OnChangedFromMin = 0x560; // CEntityIOOutput
        public const uint m_OnChangedFromMax = 0x588; // CEntityIOOutput
    }

    public static class CMathRemap  // CLogicalEntity
    {
        public const uint m_flInMin = 0x4B0; // float
        public const uint m_flInMax = 0x4B4; // float
        public const uint m_flOut1 = 0x4B8; // float
        public const uint m_flOut2 = 0x4BC; // float
        public const uint m_flOldInValue = 0x4C0; // float
        public const uint m_bEnabled = 0x4C4; // bool
        public const uint m_OutValue = 0x4C8; // CEntityOutputTemplate<float>
        public const uint m_OnRoseAboveMin = 0x4F0; // CEntityIOOutput
        public const uint m_OnRoseAboveMax = 0x518; // CEntityIOOutput
        public const uint m_OnFellBelowMin = 0x540; // CEntityIOOutput
        public const uint m_OnFellBelowMax = 0x568; // CEntityIOOutput
    }

    public static class CMelee  // CCSWeaponBase
    {
    }

    public static class CMessage  // CPointEntity
    {
        public const uint m_iszMessage = 0x4B0; // CUtlSymbolLarge
        public const uint m_MessageVolume = 0x4B8; // float
        public const uint m_MessageAttenuation = 0x4BC; // int32_t
        public const uint m_Radius = 0x4C0; // float
        public const uint m_sNoise = 0x4C8; // CUtlSymbolLarge
        public const uint m_OnShowMessage = 0x4D0; // CEntityIOOutput
    }

    public static class CMessageEntity  // CPointEntity
    {
        public const uint m_radius = 0x4B0; // int32_t
        public const uint m_messageText = 0x4B8; // CUtlSymbolLarge
        public const uint m_drawText = 0x4C0; // bool
        public const uint m_bDeveloperOnly = 0x4C1; // bool
        public const uint m_bEnabled = 0x4C2; // bool
    }

    public static class CModelPointEntity  // CBaseModelEntity
    {
    }

    public static class CModelState 
    {
        public const uint m_hModel = 0xA0; // CStrongHandle<InfoForResourceTypeCModel>
        public const uint m_ModelName = 0xA8; // CUtlSymbolLarge
        public const uint m_bClientClothCreationSuppressed = 0xE8; // bool
        public const uint m_MeshGroupMask = 0x180; // uint64_t
        public const uint m_nIdealMotionType = 0x222; // int8_t
        public const uint m_nForceLOD = 0x223; // int8_t
        public const uint m_nClothUpdateFlags = 0x224; // int8_t
    }

    public static class CMolotovGrenade  // CBaseCSGrenade
    {
    }

    public static class CMolotovProjectile  // CBaseCSGrenadeProjectile
    {
        public const uint m_bIsIncGrenade = 0xA40; // bool
        public const uint m_bDetonated = 0xA4C; // bool
        public const uint m_stillTimer = 0xA50; // IntervalTimer
        public const uint m_bHasBouncedOffPlayer = 0xB30; // bool
    }

    public static class CMomentaryRotButton  // CRotButton
    {
        public const uint m_Position = 0x8C8; // CEntityOutputTemplate<float>
        public const uint m_OnUnpressed = 0x8F0; // CEntityIOOutput
        public const uint m_OnFullyOpen = 0x918; // CEntityIOOutput
        public const uint m_OnFullyClosed = 0x940; // CEntityIOOutput
        public const uint m_OnReachedPosition = 0x968; // CEntityIOOutput
        public const uint m_lastUsed = 0x990; // int32_t
        public const uint m_start = 0x994; // QAngle
        public const uint m_end = 0x9A0; // QAngle
        public const uint m_IdealYaw = 0x9AC; // float
        public const uint m_sNoise = 0x9B0; // CUtlSymbolLarge
        public const uint m_bUpdateTarget = 0x9B8; // bool
        public const uint m_direction = 0x9BC; // int32_t
        public const uint m_returnSpeed = 0x9C0; // float
        public const uint m_flStartPosition = 0x9C4; // float
    }

    public static class CMotorController 
    {
        public const uint m_speed = 0x8; // float
        public const uint m_maxTorque = 0xC; // float
        public const uint m_axis = 0x10; // Vector
        public const uint m_inertiaFactor = 0x1C; // float
    }

    public static class CMultiLightProxy  // CLogicalEntity
    {
        public const uint m_iszLightNameFilter = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszLightClassFilter = 0x4B8; // CUtlSymbolLarge
        public const uint m_flLightRadiusFilter = 0x4C0; // float
        public const uint m_flBrightnessDelta = 0x4C4; // float
        public const uint m_bPerformScreenFade = 0x4C8; // bool
        public const uint m_flTargetBrightnessMultiplier = 0x4CC; // float
        public const uint m_flCurrentBrightnessMultiplier = 0x4D0; // float
        public const uint m_vecLights = 0x4D8; // CUtlVector<CHandle<CLightEntity>>
    }

    public static class CMultiSource  // CLogicalEntity
    {
        public const uint m_rgEntities = 0x4B0; // CHandle<CBaseEntity>[32]
        public const uint m_rgTriggered = 0x530; // int32_t[32]
        public const uint m_OnTrigger = 0x5B0; // CEntityIOOutput
        public const uint m_iTotal = 0x5D8; // int32_t
        public const uint m_globalstate = 0x5E0; // CUtlSymbolLarge
    }

    public static class CMultiplayRules  // CGameRules
    {
    }

    public static class CMultiplayer_Expresser  // CAI_ExpresserWithFollowup
    {
        public const uint m_bAllowMultipleScenes = 0x70; // bool
    }

    public static class CNavHullPresetVData 
    {
        public const uint m_vecNavHulls = 0x0; // CUtlVector<CUtlString>
    }

    public static class CNavHullVData 
    {
        public const uint m_bAgentEnabled = 0x0; // bool
        public const uint m_agentRadius = 0x4; // float
        public const uint m_agentHeight = 0x8; // float
        public const uint m_agentShortHeightEnabled = 0xC; // bool
        public const uint m_agentShortHeight = 0x10; // float
        public const uint m_agentMaxClimb = 0x14; // float
        public const uint m_agentMaxSlope = 0x18; // int32_t
        public const uint m_agentMaxJumpDownDist = 0x1C; // float
        public const uint m_agentMaxJumpHorizDistBase = 0x20; // float
        public const uint m_agentMaxJumpUpDist = 0x24; // float
        public const uint m_agentBorderErosion = 0x28; // int32_t
    }

    public static class CNavLinkAnimgraphVar 
    {
        public const uint m_strAnimgraphVar = 0x0; // CUtlString
        public const uint m_unAlignmentDegrees = 0x8; // uint32_t
    }

    public static class CNavLinkAreaEntity  // CPointEntity
    {
        public const uint m_flWidth = 0x4B0; // float
        public const uint m_vLocatorOffset = 0x4B4; // Vector
        public const uint m_qLocatorAnglesOffset = 0x4C0; // QAngle
        public const uint m_strMovementForward = 0x4D0; // CUtlSymbolLarge
        public const uint m_strMovementReverse = 0x4D8; // CUtlSymbolLarge
        public const uint m_nNavLinkIdForward = 0x4E0; // int32_t
        public const uint m_nNavLinkIdReverse = 0x4E4; // int32_t
        public const uint m_bEnabled = 0x4E8; // bool
        public const uint m_strFilterName = 0x4F0; // CUtlSymbolLarge
        public const uint m_hFilter = 0x4F8; // CHandle<CBaseFilter>
        public const uint m_OnNavLinkStart = 0x500; // CEntityIOOutput
        public const uint m_OnNavLinkFinish = 0x528; // CEntityIOOutput
        public const uint m_bIsTerminus = 0x550; // bool
    }

    public static class CNavLinkMovementVData 
    {
        public const uint m_bIsInterpolated = 0x0; // bool
        public const uint m_unRecommendedDistance = 0x4; // uint32_t
        public const uint m_vecAnimgraphVars = 0x8; // CUtlVector<CNavLinkAnimgraphVar>
    }

    public static class CNavSpaceInfo  // CPointEntity
    {
        public const uint m_bCreateFlightSpace = 0x4B0; // bool
    }

    public static class CNavVolume 
    {
    }

    public static class CNavVolumeAll  // CNavVolumeVector
    {
    }

    public static class CNavVolumeBreadthFirstSearch  // CNavVolumeCalculatedVector
    {
        public const uint m_vStartPos = 0xA0; // Vector
        public const uint m_flSearchDist = 0xAC; // float
    }

    public static class CNavVolumeCalculatedVector  // CNavVolume
    {
    }

    public static class CNavVolumeMarkupVolume  // CNavVolume
    {
    }

    public static class CNavVolumeSphere  // CNavVolume
    {
        public const uint m_vCenter = 0x70; // Vector
        public const uint m_flRadius = 0x7C; // float
    }

    public static class CNavVolumeSphericalShell  // CNavVolumeSphere
    {
        public const uint m_flRadiusInner = 0x80; // float
    }

    public static class CNavVolumeVector  // CNavVolume
    {
        public const uint m_bHasBeenPreFiltered = 0x78; // bool
    }

    public static class CNavWalkable  // CPointEntity
    {
    }

    public static class CNetworkOriginCellCoordQuantizedVector 
    {
        public const uint m_cellX = 0x10; // uint16_t
        public const uint m_cellY = 0x12; // uint16_t
        public const uint m_cellZ = 0x14; // uint16_t
        public const uint m_nOutsideWorld = 0x16; // uint16_t
        public const uint m_vecX = 0x18; // CNetworkedQuantizedFloat
        public const uint m_vecY = 0x20; // CNetworkedQuantizedFloat
        public const uint m_vecZ = 0x28; // CNetworkedQuantizedFloat
    }

    public static class CNetworkOriginQuantizedVector 
    {
        public const uint m_vecX = 0x10; // CNetworkedQuantizedFloat
        public const uint m_vecY = 0x18; // CNetworkedQuantizedFloat
        public const uint m_vecZ = 0x20; // CNetworkedQuantizedFloat
    }

    public static class CNetworkTransmitComponent 
    {
        public const uint m_nTransmitStateOwnedCounter = 0x16C; // uint8_t
    }

    public static class CNetworkVelocityVector 
    {
        public const uint m_vecX = 0x10; // CNetworkedQuantizedFloat
        public const uint m_vecY = 0x18; // CNetworkedQuantizedFloat
        public const uint m_vecZ = 0x20; // CNetworkedQuantizedFloat
    }

    public static class CNetworkViewOffsetVector 
    {
        public const uint m_vecX = 0x10; // CNetworkedQuantizedFloat
        public const uint m_vecY = 0x18; // CNetworkedQuantizedFloat
        public const uint m_vecZ = 0x20; // CNetworkedQuantizedFloat
    }

    public static class CNetworkedSequenceOperation 
    {
        public const uint m_hSequence = 0x8; // HSequence
        public const uint m_flPrevCycle = 0xC; // float
        public const uint m_flCycle = 0x10; // float
        public const uint m_flWeight = 0x14; // CNetworkedQuantizedFloat
        public const uint m_bSequenceChangeNetworked = 0x1C; // bool
        public const uint m_bDiscontinuity = 0x1D; // bool
        public const uint m_flPrevCycleFromDiscontinuity = 0x20; // float
        public const uint m_flPrevCycleForAnimEventDetection = 0x24; // float
    }

    public static class CNullEntity  // CBaseEntity
    {
    }

    public static class COmniLight  // CBarnLight
    {
        public const uint m_flInnerAngle = 0x928; // float
        public const uint m_flOuterAngle = 0x92C; // float
        public const uint m_bShowLight = 0x930; // bool
    }

    public static class COrnamentProp  // CDynamicProp
    {
        public const uint m_initialOwner = 0xB08; // CUtlSymbolLarge
    }

    public static class CParticleSystem  // CBaseModelEntity
    {
        public const uint m_szSnapshotFileName = 0x700; // char[512]
        public const uint m_bActive = 0x900; // bool
        public const uint m_bFrozen = 0x901; // bool
        public const uint m_flFreezeTransitionDuration = 0x904; // float
        public const uint m_nStopType = 0x908; // int32_t
        public const uint m_bAnimateDuringGameplayPause = 0x90C; // bool
        public const uint m_iEffectIndex = 0x910; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_flStartTime = 0x918; // GameTime_t
        public const uint m_flPreSimTime = 0x91C; // float
        public const uint m_vServerControlPoints = 0x920; // Vector[4]
        public const uint m_iServerControlPointAssignments = 0x950; // uint8_t[4]
        public const uint m_hControlPointEnts = 0x954; // CHandle<CBaseEntity>[64]
        public const uint m_bNoSave = 0xA54; // bool
        public const uint m_bNoFreeze = 0xA55; // bool
        public const uint m_bNoRamp = 0xA56; // bool
        public const uint m_bStartActive = 0xA57; // bool
        public const uint m_iszEffectName = 0xA58; // CUtlSymbolLarge
        public const uint m_iszControlPointNames = 0xA60; // CUtlSymbolLarge[64]
        public const uint m_nDataCP = 0xC60; // int32_t
        public const uint m_vecDataCPValue = 0xC64; // Vector
        public const uint m_nTintCP = 0xC70; // int32_t
        public const uint m_clrTint = 0xC74; // Color
    }

    public static class CPathCorner  // CPointEntity
    {
        public const uint m_flWait = 0x4B0; // float
        public const uint m_flRadius = 0x4B4; // float
        public const uint m_OnPass = 0x4B8; // CEntityIOOutput
    }

    public static class CPathCornerCrash  // CPathCorner
    {
    }

    public static class CPathKeyFrame  // CLogicalEntity
    {
        public const uint m_Origin = 0x4B0; // Vector
        public const uint m_Angles = 0x4BC; // QAngle
        public const uint m_qAngle = 0x4D0; // Quaternion
        public const uint m_iNextKey = 0x4E0; // CUtlSymbolLarge
        public const uint m_flNextTime = 0x4E8; // float
        public const uint m_pNextKey = 0x4F0; // CPathKeyFrame*
        public const uint m_pPrevKey = 0x4F8; // CPathKeyFrame*
        public const uint m_flSpeed = 0x500; // float
    }

    public static class CPathParticleRope  // CBaseEntity
    {
        public const uint m_bStartActive = 0x4B0; // bool
        public const uint m_flMaxSimulationTime = 0x4B4; // float
        public const uint m_iszEffectName = 0x4B8; // CUtlSymbolLarge
        public const uint m_PathNodes_Name = 0x4C0; // CUtlVector<CUtlSymbolLarge>
        public const uint m_flParticleSpacing = 0x4D8; // float
        public const uint m_flSlack = 0x4DC; // float
        public const uint m_flRadius = 0x4E0; // float
        public const uint m_ColorTint = 0x4E4; // Color
        public const uint m_nEffectState = 0x4E8; // int32_t
        public const uint m_iEffectIndex = 0x4F0; // CStrongHandle<InfoForResourceTypeIParticleSystemDefinition>
        public const uint m_PathNodes_Position = 0x4F8; // CNetworkUtlVectorBase<Vector>
        public const uint m_PathNodes_TangentIn = 0x510; // CNetworkUtlVectorBase<Vector>
        public const uint m_PathNodes_TangentOut = 0x528; // CNetworkUtlVectorBase<Vector>
        public const uint m_PathNodes_Color = 0x540; // CNetworkUtlVectorBase<Vector>
        public const uint m_PathNodes_PinEnabled = 0x558; // CNetworkUtlVectorBase<bool>
        public const uint m_PathNodes_RadiusScale = 0x570; // CNetworkUtlVectorBase<float>
    }

    public static class CPathParticleRopeAlias_path_particle_rope_clientside  // CPathParticleRope
    {
    }

    public static class CPathTrack  // CPointEntity
    {
        public const uint m_pnext = 0x4B0; // CPathTrack*
        public const uint m_pprevious = 0x4B8; // CPathTrack*
        public const uint m_paltpath = 0x4C0; // CPathTrack*
        public const uint m_flRadius = 0x4C8; // float
        public const uint m_length = 0x4CC; // float
        public const uint m_altName = 0x4D0; // CUtlSymbolLarge
        public const uint m_nIterVal = 0x4D8; // int32_t
        public const uint m_eOrientationType = 0x4DC; // TrackOrientationType_t
        public const uint m_OnPass = 0x4E0; // CEntityIOOutput
    }

    public static class CPhysBallSocket  // CPhysConstraint
    {
        public const uint m_flFriction = 0x508; // float
        public const uint m_bEnableSwingLimit = 0x50C; // bool
        public const uint m_flSwingLimit = 0x510; // float
        public const uint m_bEnableTwistLimit = 0x514; // bool
        public const uint m_flMinTwistAngle = 0x518; // float
        public const uint m_flMaxTwistAngle = 0x51C; // float
    }

    public static class CPhysBox  // CBreakable
    {
        public const uint m_damageType = 0x7C0; // int32_t
        public const uint m_massScale = 0x7C4; // float
        public const uint m_damageToEnableMotion = 0x7C8; // int32_t
        public const uint m_flForceToEnableMotion = 0x7CC; // float
        public const uint m_angPreferredCarryAngles = 0x7D0; // QAngle
        public const uint m_bNotSolidToWorld = 0x7DC; // bool
        public const uint m_bEnableUseOutput = 0x7DD; // bool
        public const uint m_iExploitableByPlayer = 0x7E0; // int32_t
        public const uint m_flTouchOutputPerEntityDelay = 0x7E4; // float
        public const uint m_OnDamaged = 0x7E8; // CEntityIOOutput
        public const uint m_OnAwakened = 0x810; // CEntityIOOutput
        public const uint m_OnMotionEnabled = 0x838; // CEntityIOOutput
        public const uint m_OnPlayerUse = 0x860; // CEntityIOOutput
        public const uint m_OnStartTouch = 0x888; // CEntityIOOutput
        public const uint m_hCarryingPlayer = 0x8B0; // CHandle<CBasePlayerPawn>
    }

    public static class CPhysConstraint  // CLogicalEntity
    {
        public const uint m_nameAttach1 = 0x4B8; // CUtlSymbolLarge
        public const uint m_nameAttach2 = 0x4C0; // CUtlSymbolLarge
        public const uint m_breakSound = 0x4C8; // CUtlSymbolLarge
        public const uint m_forceLimit = 0x4D0; // float
        public const uint m_torqueLimit = 0x4D4; // float
        public const uint m_teleportTick = 0x4D8; // uint32_t
        public const uint m_minTeleportDistance = 0x4DC; // float
        public const uint m_OnBreak = 0x4E0; // CEntityIOOutput
    }

    public static class CPhysExplosion  // CPointEntity
    {
        public const uint m_bExplodeOnSpawn = 0x4B0; // bool
        public const uint m_flMagnitude = 0x4B4; // float
        public const uint m_flDamage = 0x4B8; // float
        public const uint m_radius = 0x4BC; // float
        public const uint m_targetEntityName = 0x4C0; // CUtlSymbolLarge
        public const uint m_flInnerRadius = 0x4C8; // float
        public const uint m_flPushScale = 0x4CC; // float
        public const uint m_bConvertToDebrisWhenPossible = 0x4D0; // bool
        public const uint m_OnPushedPlayer = 0x4D8; // CEntityIOOutput
    }

    public static class CPhysFixed  // CPhysConstraint
    {
        public const uint m_flLinearFrequency = 0x508; // float
        public const uint m_flLinearDampingRatio = 0x50C; // float
        public const uint m_flAngularFrequency = 0x510; // float
        public const uint m_flAngularDampingRatio = 0x514; // float
        public const uint m_bEnableLinearConstraint = 0x518; // bool
        public const uint m_bEnableAngularConstraint = 0x519; // bool
    }

    public static class CPhysForce  // CPointEntity
    {
        public const uint m_nameAttach = 0x4B8; // CUtlSymbolLarge
        public const uint m_force = 0x4C0; // float
        public const uint m_forceTime = 0x4C4; // float
        public const uint m_attachedObject = 0x4C8; // CHandle<CBaseEntity>
        public const uint m_wasRestored = 0x4CC; // bool
        public const uint m_integrator = 0x4D0; // CConstantForceController
    }

    public static class CPhysHinge  // CPhysConstraint
    {
        public const uint m_soundInfo = 0x510; // ConstraintSoundInfo
        public const uint m_NotifyMinLimitReached = 0x598; // CEntityIOOutput
        public const uint m_NotifyMaxLimitReached = 0x5C0; // CEntityIOOutput
        public const uint m_bAtMinLimit = 0x5E8; // bool
        public const uint m_bAtMaxLimit = 0x5E9; // bool
        public const uint m_hinge = 0x5EC; // constraint_hingeparams_t
        public const uint m_hingeFriction = 0x62C; // float
        public const uint m_systemLoadScale = 0x630; // float
        public const uint m_bIsAxisLocal = 0x634; // bool
        public const uint m_flMinRotation = 0x638; // float
        public const uint m_flMaxRotation = 0x63C; // float
        public const uint m_flInitialRotation = 0x640; // float
        public const uint m_flMotorFrequency = 0x644; // float
        public const uint m_flMotorDampingRatio = 0x648; // float
        public const uint m_flAngleSpeed = 0x64C; // float
        public const uint m_flAngleSpeedThreshold = 0x650; // float
        public const uint m_OnStartMoving = 0x658; // CEntityIOOutput
        public const uint m_OnStopMoving = 0x680; // CEntityIOOutput
    }

    public static class CPhysHingeAlias_phys_hinge_local  // CPhysHinge
    {
    }

    public static class CPhysImpact  // CPointEntity
    {
        public const uint m_damage = 0x4B0; // float
        public const uint m_distance = 0x4B4; // float
        public const uint m_directionEntityName = 0x4B8; // CUtlSymbolLarge
    }

    public static class CPhysLength  // CPhysConstraint
    {
        public const uint m_offset = 0x508; // Vector[2]
        public const uint m_vecAttach = 0x520; // Vector
        public const uint m_addLength = 0x52C; // float
        public const uint m_minLength = 0x530; // float
        public const uint m_totalLength = 0x534; // float
        public const uint m_bEnableCollision = 0x538; // bool
    }

    public static class CPhysMagnet  // CBaseAnimGraph
    {
        public const uint m_OnMagnetAttach = 0x890; // CEntityIOOutput
        public const uint m_OnMagnetDetach = 0x8B8; // CEntityIOOutput
        public const uint m_massScale = 0x8E0; // float
        public const uint m_forceLimit = 0x8E4; // float
        public const uint m_torqueLimit = 0x8E8; // float
        public const uint m_MagnettedEntities = 0x8F0; // CUtlVector<magnetted_objects_t>
        public const uint m_bActive = 0x908; // bool
        public const uint m_bHasHitSomething = 0x909; // bool
        public const uint m_flTotalMass = 0x90C; // float
        public const uint m_flRadius = 0x910; // float
        public const uint m_flNextSuckTime = 0x914; // GameTime_t
        public const uint m_iMaxObjectsAttached = 0x918; // int32_t
    }

    public static class CPhysMotor  // CLogicalEntity
    {
        public const uint m_nameAttach = 0x4B0; // CUtlSymbolLarge
        public const uint m_hAttachedObject = 0x4B8; // CHandle<CBaseEntity>
        public const uint m_spinUp = 0x4BC; // float
        public const uint m_additionalAcceleration = 0x4C0; // float
        public const uint m_angularAcceleration = 0x4C4; // float
        public const uint m_lastTime = 0x4C8; // GameTime_t
        public const uint m_motor = 0x4E0; // CMotorController
    }

    public static class CPhysPulley  // CPhysConstraint
    {
        public const uint m_position2 = 0x508; // Vector
        public const uint m_offset = 0x514; // Vector[2]
        public const uint m_addLength = 0x52C; // float
        public const uint m_gearRatio = 0x530; // float
    }

    public static class CPhysSlideConstraint  // CPhysConstraint
    {
        public const uint m_axisEnd = 0x510; // Vector
        public const uint m_slideFriction = 0x51C; // float
        public const uint m_systemLoadScale = 0x520; // float
        public const uint m_initialOffset = 0x524; // float
        public const uint m_bEnableLinearConstraint = 0x528; // bool
        public const uint m_bEnableAngularConstraint = 0x529; // bool
        public const uint m_flMotorFrequency = 0x52C; // float
        public const uint m_flMotorDampingRatio = 0x530; // float
        public const uint m_bUseEntityPivot = 0x534; // bool
        public const uint m_soundInfo = 0x538; // ConstraintSoundInfo
    }

    public static class CPhysThruster  // CPhysForce
    {
        public const uint m_localOrigin = 0x510; // Vector
    }

    public static class CPhysTorque  // CPhysForce
    {
        public const uint m_axis = 0x510; // Vector
    }

    public static class CPhysWheelConstraint  // CPhysConstraint
    {
        public const uint m_flSuspensionFrequency = 0x508; // float
        public const uint m_flSuspensionDampingRatio = 0x50C; // float
        public const uint m_flSuspensionHeightOffset = 0x510; // float
        public const uint m_bEnableSuspensionLimit = 0x514; // bool
        public const uint m_flMinSuspensionOffset = 0x518; // float
        public const uint m_flMaxSuspensionOffset = 0x51C; // float
        public const uint m_bEnableSteeringLimit = 0x520; // bool
        public const uint m_flMinSteeringAngle = 0x524; // float
        public const uint m_flMaxSteeringAngle = 0x528; // float
        public const uint m_flSteeringAxisFriction = 0x52C; // float
        public const uint m_flSpinAxisFriction = 0x530; // float
    }

    public static class CPhysicalButton  // CBaseButton
    {
    }

    public static class CPhysicsEntitySolver  // CLogicalEntity
    {
        public const uint m_hMovingEntity = 0x4B8; // CHandle<CBaseEntity>
        public const uint m_hPhysicsBlocker = 0x4BC; // CHandle<CBaseEntity>
        public const uint m_separationDuration = 0x4C0; // float
        public const uint m_cancelTime = 0x4C4; // GameTime_t
    }

    public static class CPhysicsProp  // CBreakableProp
    {
        public const uint m_MotionEnabled = 0xA10; // CEntityIOOutput
        public const uint m_OnAwakened = 0xA38; // CEntityIOOutput
        public const uint m_OnAwake = 0xA60; // CEntityIOOutput
        public const uint m_OnAsleep = 0xA88; // CEntityIOOutput
        public const uint m_OnPlayerUse = 0xAB0; // CEntityIOOutput
        public const uint m_OnPlayerPickup = 0xAD8; // CEntityIOOutput
        public const uint m_OnOutOfWorld = 0xB00; // CEntityIOOutput
        public const uint m_massScale = 0xB28; // float
        public const uint m_inertiaScale = 0xB2C; // float
        public const uint m_buoyancyScale = 0xB30; // float
        public const uint m_damageType = 0xB34; // int32_t
        public const uint m_damageToEnableMotion = 0xB38; // int32_t
        public const uint m_flForceToEnableMotion = 0xB3C; // float
        public const uint m_bThrownByPlayer = 0xB40; // bool
        public const uint m_bDroppedByPlayer = 0xB41; // bool
        public const uint m_bTouchedByPlayer = 0xB42; // bool
        public const uint m_bFirstCollisionAfterLaunch = 0xB43; // bool
        public const uint m_iExploitableByPlayer = 0xB44; // int32_t
        public const uint m_bHasBeenAwakened = 0xB48; // bool
        public const uint m_bIsOverrideProp = 0xB49; // bool
        public const uint m_fNextCheckDisableMotionContactsTime = 0xB4C; // GameTime_t
        public const uint m_iInitialGlowState = 0xB50; // int32_t
        public const uint m_nGlowRange = 0xB54; // int32_t
        public const uint m_nGlowRangeMin = 0xB58; // int32_t
        public const uint m_glowColor = 0xB5C; // Color
        public const uint m_bForceNavIgnore = 0xB60; // bool
        public const uint m_bNoNavmeshBlocker = 0xB61; // bool
        public const uint m_bForceNpcExclude = 0xB62; // bool
        public const uint m_bShouldAutoConvertBackFromDebris = 0xB63; // bool
        public const uint m_bMuteImpactEffects = 0xB64; // bool
        public const uint m_bAcceptDamageFromHeldObjects = 0xB6C; // bool
        public const uint m_bEnableUseOutput = 0xB6D; // bool
        public const uint m_bAwake = 0xB6E; // bool
        public const uint m_nCollisionGroupOverride = 0xB70; // int32_t
    }

    public static class CPhysicsPropMultiplayer  // CPhysicsProp
    {
    }

    public static class CPhysicsPropOverride  // CPhysicsProp
    {
    }

    public static class CPhysicsPropRespawnable  // CPhysicsProp
    {
        public const uint m_vOriginalSpawnOrigin = 0xB78; // Vector
        public const uint m_vOriginalSpawnAngles = 0xB84; // QAngle
        public const uint m_vOriginalMins = 0xB90; // Vector
        public const uint m_vOriginalMaxs = 0xB9C; // Vector
        public const uint m_flRespawnDuration = 0xBA8; // float
    }

    public static class CPhysicsShake 
    {
        public const uint m_force = 0x8; // Vector
    }

    public static class CPhysicsSpring  // CBaseEntity
    {
        public const uint m_flFrequency = 0x4B8; // float
        public const uint m_flDampingRatio = 0x4BC; // float
        public const uint m_flRestLength = 0x4C0; // float
        public const uint m_nameAttachStart = 0x4C8; // CUtlSymbolLarge
        public const uint m_nameAttachEnd = 0x4D0; // CUtlSymbolLarge
        public const uint m_start = 0x4D8; // Vector
        public const uint m_end = 0x4E4; // Vector
        public const uint m_teleportTick = 0x4F0; // uint32_t
    }

    public static class CPhysicsWire  // CBaseEntity
    {
        public const uint m_nDensity = 0x4B0; // int32_t
    }

    public static class CPlantedC4  // CBaseAnimGraph
    {
        public const uint m_bBombTicking = 0x890; // bool
        public const uint m_flC4Blow = 0x894; // GameTime_t
        public const uint m_nBombSite = 0x898; // int32_t
        public const uint m_nSourceSoundscapeHash = 0x89C; // int32_t
        public const uint m_OnBombDefused = 0x8A0; // CEntityIOOutput
        public const uint m_OnBombBeginDefuse = 0x8C8; // CEntityIOOutput
        public const uint m_OnBombDefuseAborted = 0x8F0; // CEntityIOOutput
        public const uint m_bCannotBeDefused = 0x918; // bool
        public const uint m_entitySpottedState = 0x920; // EntitySpottedState_t
        public const uint m_nSpotRules = 0x938; // int32_t
        public const uint m_bTrainingPlacedByPlayer = 0x93C; // bool
        public const uint m_bHasExploded = 0x93D; // bool
        public const uint m_flTimerLength = 0x940; // float
        public const uint m_bBeingDefused = 0x944; // bool
        public const uint m_fLastDefuseTime = 0x94C; // GameTime_t
        public const uint m_flDefuseLength = 0x954; // float
        public const uint m_flDefuseCountDown = 0x958; // GameTime_t
        public const uint m_bBombDefused = 0x95C; // bool
        public const uint m_hBombDefuser = 0x960; // CHandle<CCSPlayerPawn>
        public const uint m_hControlPanel = 0x964; // CHandle<CBaseEntity>
        public const uint m_iProgressBarTime = 0x968; // int32_t
        public const uint m_bVoiceAlertFired = 0x96C; // bool
        public const uint m_bVoiceAlertPlayed = 0x96D; // bool[4]
        public const uint m_flNextBotBeepTime = 0x974; // GameTime_t
        public const uint m_bPlantedAfterPickup = 0x97C; // bool
        public const uint m_angCatchUpToPlayerEye = 0x980; // QAngle
        public const uint m_flLastSpinDetectionTime = 0x98C; // GameTime_t
    }

    public static class CPlatTrigger  // CBaseModelEntity
    {
        public const uint m_pPlatform = 0x700; // CHandle<CFuncPlat>
    }

    public static class CPlayerControllerComponent 
    {
        public const uint __m_pChainEntity = 0x8; // CNetworkVarChainer
    }

    public static class CPlayerPawnComponent 
    {
        public const uint __m_pChainEntity = 0x8; // CNetworkVarChainer
    }

    public static class CPlayerPing  // CBaseEntity
    {
        public const uint m_hPlayer = 0x4B8; // CHandle<CCSPlayerPawn>
        public const uint m_hPingedEntity = 0x4BC; // CHandle<CBaseEntity>
        public const uint m_iType = 0x4C0; // int32_t
        public const uint m_bUrgent = 0x4C4; // bool
        public const uint m_szPlaceName = 0x4C5; // char[18]
    }

    public static class CPlayerSprayDecal  // CModelPointEntity
    {
        public const uint m_nUniqueID = 0x700; // int32_t
        public const uint m_unAccountID = 0x704; // uint32_t
        public const uint m_unTraceID = 0x708; // uint32_t
        public const uint m_rtGcTime = 0x70C; // uint32_t
        public const uint m_vecEndPos = 0x710; // Vector
        public const uint m_vecStart = 0x71C; // Vector
        public const uint m_vecLeft = 0x728; // Vector
        public const uint m_vecNormal = 0x734; // Vector
        public const uint m_nPlayer = 0x740; // int32_t
        public const uint m_nEntity = 0x744; // int32_t
        public const uint m_nHitbox = 0x748; // int32_t
        public const uint m_flCreationTime = 0x74C; // float
        public const uint m_nTintID = 0x750; // int32_t
        public const uint m_nVersion = 0x754; // uint8_t
        public const uint m_ubSignature = 0x755; // uint8_t[128]
    }

    public static class CPlayerVisibility  // CBaseEntity
    {
        public const uint m_flVisibilityStrength = 0x4B0; // float
        public const uint m_flFogDistanceMultiplier = 0x4B4; // float
        public const uint m_flFogMaxDensityMultiplier = 0x4B8; // float
        public const uint m_flFadeTime = 0x4BC; // float
        public const uint m_bStartDisabled = 0x4C0; // bool
        public const uint m_bIsEnabled = 0x4C1; // bool
    }

    public static class CPlayer_AutoaimServices  // CPlayerPawnComponent
    {
    }

    public static class CPlayer_CameraServices  // CPlayerPawnComponent
    {
        public const uint m_vecCsViewPunchAngle = 0x40; // QAngle
        public const uint m_nCsViewPunchAngleTick = 0x4C; // GameTick_t
        public const uint m_flCsViewPunchAngleTickRatio = 0x50; // float
        public const uint m_PlayerFog = 0x58; // fogplayerparams_t
        public const uint m_hColorCorrectionCtrl = 0x98; // CHandle<CColorCorrection>
        public const uint m_hViewEntity = 0x9C; // CHandle<CBaseEntity>
        public const uint m_hTonemapController = 0xA0; // CHandle<CTonemapController2>
        public const uint m_audio = 0xA8; // audioparams_t
        public const uint m_PostProcessingVolumes = 0x120; // CNetworkUtlVectorBase<CHandle<CPostProcessingVolume>>
        public const uint m_flOldPlayerZ = 0x138; // float
        public const uint m_flOldPlayerViewOffsetZ = 0x13C; // float
        public const uint m_hTriggerSoundscapeList = 0x158; // CUtlVector<CHandle<CEnvSoundscapeTriggerable>>
    }

    public static class CPlayer_FlashlightServices  // CPlayerPawnComponent
    {
    }

    public static class CPlayer_ItemServices  // CPlayerPawnComponent
    {
    }

    public static class CPlayer_MovementServices  // CPlayerPawnComponent
    {
        public const uint m_nImpulse = 0x40; // int32_t
        public const uint m_nButtons = 0x48; // CInButtonState
        public const uint m_nQueuedButtonDownMask = 0x68; // uint64_t
        public const uint m_nQueuedButtonChangeMask = 0x70; // uint64_t
        public const uint m_nButtonDoublePressed = 0x78; // uint64_t
        public const uint m_pButtonPressedCmdNumber = 0x80; // uint32_t[64]
        public const uint m_nLastCommandNumberProcessed = 0x180; // uint32_t
        public const uint m_nToggleButtonDownMask = 0x188; // uint64_t
        public const uint m_flMaxspeed = 0x190; // float
        public const uint m_arrForceSubtickMoveWhen = 0x194; // float[4]
        public const uint m_flForwardMove = 0x1A4; // float
        public const uint m_flLeftMove = 0x1A8; // float
        public const uint m_flUpMove = 0x1AC; // float
        public const uint m_vecLastMovementImpulses = 0x1B0; // Vector
        public const uint m_vecOldViewAngles = 0x1BC; // QAngle
    }

    public static class CPlayer_MovementServices_Humanoid  // CPlayer_MovementServices
    {
        public const uint m_flStepSoundTime = 0x1D0; // float
        public const uint m_flFallVelocity = 0x1D4; // float
        public const uint m_bInCrouch = 0x1D8; // bool
        public const uint m_nCrouchState = 0x1DC; // uint32_t
        public const uint m_flCrouchTransitionStartTime = 0x1E0; // GameTime_t
        public const uint m_bDucked = 0x1E4; // bool
        public const uint m_bDucking = 0x1E5; // bool
        public const uint m_bInDuckJump = 0x1E6; // bool
        public const uint m_groundNormal = 0x1E8; // Vector
        public const uint m_flSurfaceFriction = 0x1F4; // float
        public const uint m_surfaceProps = 0x1F8; // CUtlStringToken
        public const uint m_nStepside = 0x208; // int32_t
        public const uint m_iTargetVolume = 0x20C; // int32_t
        public const uint m_vecSmoothedVelocity = 0x210; // Vector
    }

    public static class CPlayer_ObserverServices  // CPlayerPawnComponent
    {
        public const uint m_iObserverMode = 0x40; // uint8_t
        public const uint m_hObserverTarget = 0x44; // CHandle<CBaseEntity>
        public const uint m_iObserverLastMode = 0x48; // ObserverMode_t
        public const uint m_bForcedObserverMode = 0x4C; // bool
    }

    public static class CPlayer_UseServices  // CPlayerPawnComponent
    {
    }

    public static class CPlayer_ViewModelServices  // CPlayerPawnComponent
    {
    }

    public static class CPlayer_WaterServices  // CPlayerPawnComponent
    {
    }

    public static class CPlayer_WeaponServices  // CPlayerPawnComponent
    {
        public const uint m_bAllowSwitchToNoWeapon = 0x40; // bool
        public const uint m_hMyWeapons = 0x48; // CNetworkUtlVectorBase<CHandle<CBasePlayerWeapon>>
        public const uint m_hActiveWeapon = 0x60; // CHandle<CBasePlayerWeapon>
        public const uint m_hLastWeapon = 0x64; // CHandle<CBasePlayerWeapon>
        public const uint m_iAmmo = 0x68; // uint16_t[32]
        public const uint m_bPreventWeaponPickup = 0xA8; // bool
    }

    public static class CPointAngleSensor  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_nLookAtName = 0x4B8; // CUtlSymbolLarge
        public const uint m_hTargetEntity = 0x4C0; // CHandle<CBaseEntity>
        public const uint m_hLookAtEntity = 0x4C4; // CHandle<CBaseEntity>
        public const uint m_flDuration = 0x4C8; // float
        public const uint m_flDotTolerance = 0x4CC; // float
        public const uint m_flFacingTime = 0x4D0; // GameTime_t
        public const uint m_bFired = 0x4D4; // bool
        public const uint m_OnFacingLookat = 0x4D8; // CEntityIOOutput
        public const uint m_OnNotFacingLookat = 0x500; // CEntityIOOutput
        public const uint m_TargetDir = 0x528; // CEntityOutputTemplate<Vector>
        public const uint m_FacingPercentage = 0x550; // CEntityOutputTemplate<float>
    }

    public static class CPointAngularVelocitySensor  // CPointEntity
    {
        public const uint m_hTargetEntity = 0x4B0; // CHandle<CBaseEntity>
        public const uint m_flThreshold = 0x4B4; // float
        public const uint m_nLastCompareResult = 0x4B8; // int32_t
        public const uint m_nLastFireResult = 0x4BC; // int32_t
        public const uint m_flFireTime = 0x4C0; // GameTime_t
        public const uint m_flFireInterval = 0x4C4; // float
        public const uint m_flLastAngVelocity = 0x4C8; // float
        public const uint m_lastOrientation = 0x4CC; // QAngle
        public const uint m_vecAxis = 0x4D8; // Vector
        public const uint m_bUseHelper = 0x4E4; // bool
        public const uint m_AngularVelocity = 0x4E8; // CEntityOutputTemplate<float>
        public const uint m_OnLessThan = 0x510; // CEntityIOOutput
        public const uint m_OnLessThanOrEqualTo = 0x538; // CEntityIOOutput
        public const uint m_OnGreaterThan = 0x560; // CEntityIOOutput
        public const uint m_OnGreaterThanOrEqualTo = 0x588; // CEntityIOOutput
        public const uint m_OnEqualTo = 0x5B0; // CEntityIOOutput
    }

    public static class CPointBroadcastClientCommand  // CPointEntity
    {
    }

    public static class CPointCamera  // CBaseEntity
    {
        public const uint m_FOV = 0x4B0; // float
        public const uint m_Resolution = 0x4B4; // float
        public const uint m_bFogEnable = 0x4B8; // bool
        public const uint m_FogColor = 0x4B9; // Color
        public const uint m_flFogStart = 0x4C0; // float
        public const uint m_flFogEnd = 0x4C4; // float
        public const uint m_flFogMaxDensity = 0x4C8; // float
        public const uint m_bActive = 0x4CC; // bool
        public const uint m_bUseScreenAspectRatio = 0x4CD; // bool
        public const uint m_flAspectRatio = 0x4D0; // float
        public const uint m_bNoSky = 0x4D4; // bool
        public const uint m_fBrightness = 0x4D8; // float
        public const uint m_flZFar = 0x4DC; // float
        public const uint m_flZNear = 0x4E0; // float
        public const uint m_bCanHLTVUse = 0x4E4; // bool
        public const uint m_bDofEnabled = 0x4E5; // bool
        public const uint m_flDofNearBlurry = 0x4E8; // float
        public const uint m_flDofNearCrisp = 0x4EC; // float
        public const uint m_flDofFarCrisp = 0x4F0; // float
        public const uint m_flDofFarBlurry = 0x4F4; // float
        public const uint m_flDofTiltToGround = 0x4F8; // float
        public const uint m_TargetFOV = 0x4FC; // float
        public const uint m_DegreesPerSecond = 0x500; // float
        public const uint m_bIsOn = 0x504; // bool
        public const uint m_pNext = 0x508; // CPointCamera*
    }

    public static class CPointCameraVFOV  // CPointCamera
    {
        public const uint m_flVerticalFOV = 0x510; // float
    }

    public static class CPointClientCommand  // CPointEntity
    {
    }

    public static class CPointClientUIDialog  // CBaseClientUIEntity
    {
        public const uint m_hActivator = 0x8B0; // CHandle<CBaseEntity>
        public const uint m_bStartEnabled = 0x8B4; // bool
    }

    public static class CPointClientUIWorldPanel  // CBaseClientUIEntity
    {
        public const uint m_bIgnoreInput = 0x8B0; // bool
        public const uint m_bLit = 0x8B1; // bool
        public const uint m_bFollowPlayerAcrossTeleport = 0x8B2; // bool
        public const uint m_flWidth = 0x8B4; // float
        public const uint m_flHeight = 0x8B8; // float
        public const uint m_flDPI = 0x8BC; // float
        public const uint m_flInteractDistance = 0x8C0; // float
        public const uint m_flDepthOffset = 0x8C4; // float
        public const uint m_unOwnerContext = 0x8C8; // uint32_t
        public const uint m_unHorizontalAlign = 0x8CC; // uint32_t
        public const uint m_unVerticalAlign = 0x8D0; // uint32_t
        public const uint m_unOrientation = 0x8D4; // uint32_t
        public const uint m_bAllowInteractionFromAllSceneWorlds = 0x8D8; // bool
        public const uint m_vecCSSClasses = 0x8E0; // CNetworkUtlVectorBase<CUtlSymbolLarge>
        public const uint m_bOpaque = 0x8F8; // bool
        public const uint m_bNoDepth = 0x8F9; // bool
        public const uint m_bRenderBackface = 0x8FA; // bool
        public const uint m_bUseOffScreenIndicator = 0x8FB; // bool
        public const uint m_bExcludeFromSaveGames = 0x8FC; // bool
        public const uint m_bGrabbable = 0x8FD; // bool
        public const uint m_bOnlyRenderToTexture = 0x8FE; // bool
        public const uint m_bDisableMipGen = 0x8FF; // bool
        public const uint m_nExplicitImageLayout = 0x900; // int32_t
    }

    public static class CPointClientUIWorldTextPanel  // CPointClientUIWorldPanel
    {
        public const uint m_messageText = 0x908; // char[512]
    }

    public static class CPointCommentaryNode  // CBaseAnimGraph
    {
        public const uint m_iszPreCommands = 0x890; // CUtlSymbolLarge
        public const uint m_iszPostCommands = 0x898; // CUtlSymbolLarge
        public const uint m_iszCommentaryFile = 0x8A0; // CUtlSymbolLarge
        public const uint m_iszViewTarget = 0x8A8; // CUtlSymbolLarge
        public const uint m_hViewTarget = 0x8B0; // CHandle<CBaseEntity>
        public const uint m_hViewTargetAngles = 0x8B4; // CHandle<CBaseEntity>
        public const uint m_iszViewPosition = 0x8B8; // CUtlSymbolLarge
        public const uint m_hViewPosition = 0x8C0; // CHandle<CBaseEntity>
        public const uint m_hViewPositionMover = 0x8C4; // CHandle<CBaseEntity>
        public const uint m_bPreventMovement = 0x8C8; // bool
        public const uint m_bUnderCrosshair = 0x8C9; // bool
        public const uint m_bUnstoppable = 0x8CA; // bool
        public const uint m_flFinishedTime = 0x8CC; // GameTime_t
        public const uint m_vecFinishOrigin = 0x8D0; // Vector
        public const uint m_vecOriginalAngles = 0x8DC; // QAngle
        public const uint m_vecFinishAngles = 0x8E8; // QAngle
        public const uint m_bPreventChangesWhileMoving = 0x8F4; // bool
        public const uint m_bDisabled = 0x8F5; // bool
        public const uint m_vecTeleportOrigin = 0x8F8; // Vector
        public const uint m_flAbortedPlaybackAt = 0x904; // GameTime_t
        public const uint m_pOnCommentaryStarted = 0x908; // CEntityIOOutput
        public const uint m_pOnCommentaryStopped = 0x930; // CEntityIOOutput
        public const uint m_bActive = 0x958; // bool
        public const uint m_flStartTime = 0x95C; // GameTime_t
        public const uint m_flStartTimeInCommentary = 0x960; // float
        public const uint m_iszTitle = 0x968; // CUtlSymbolLarge
        public const uint m_iszSpeakers = 0x970; // CUtlSymbolLarge
        public const uint m_iNodeNumber = 0x978; // int32_t
        public const uint m_iNodeNumberMax = 0x97C; // int32_t
        public const uint m_bListenedTo = 0x980; // bool
    }

    public static class CPointEntity  // CBaseEntity
    {
    }

    public static class CPointEntityFinder  // CBaseEntity
    {
        public const uint m_hEntity = 0x4B0; // CHandle<CBaseEntity>
        public const uint m_iFilterName = 0x4B8; // CUtlSymbolLarge
        public const uint m_hFilter = 0x4C0; // CHandle<CBaseFilter>
        public const uint m_iRefName = 0x4C8; // CUtlSymbolLarge
        public const uint m_hReference = 0x4D0; // CHandle<CBaseEntity>
        public const uint m_FindMethod = 0x4D4; // EntFinderMethod_t
        public const uint m_OnFoundEntity = 0x4D8; // CEntityIOOutput
    }

    public static class CPointGamestatsCounter  // CPointEntity
    {
        public const uint m_strStatisticName = 0x4B0; // CUtlSymbolLarge
        public const uint m_bDisabled = 0x4B8; // bool
    }

    public static class CPointGiveAmmo  // CPointEntity
    {
        public const uint m_pActivator = 0x4B0; // CHandle<CBaseEntity>
    }

    public static class CPointHurt  // CPointEntity
    {
        public const uint m_nDamage = 0x4B0; // int32_t
        public const uint m_bitsDamageType = 0x4B4; // int32_t
        public const uint m_flRadius = 0x4B8; // float
        public const uint m_flDelay = 0x4BC; // float
        public const uint m_strTarget = 0x4C0; // CUtlSymbolLarge
        public const uint m_pActivator = 0x4C8; // CHandle<CBaseEntity>
    }

    public static class CPointPrefab  // CServerOnlyPointEntity
    {
        public const uint m_targetMapName = 0x4B0; // CUtlSymbolLarge
        public const uint m_forceWorldGroupID = 0x4B8; // CUtlSymbolLarge
        public const uint m_associatedRelayTargetName = 0x4C0; // CUtlSymbolLarge
        public const uint m_fixupNames = 0x4C8; // bool
        public const uint m_bLoadDynamic = 0x4C9; // bool
        public const uint m_associatedRelayEntity = 0x4CC; // CHandle<CPointPrefab>
    }

    public static class CPointProximitySensor  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_hTargetEntity = 0x4B4; // CHandle<CBaseEntity>
        public const uint m_Distance = 0x4B8; // CEntityOutputTemplate<float>
    }

    public static class CPointPulse  // CBaseEntity
    {
        public const uint m_sNameFixupStaticPrefix = 0x5C8; // CUtlSymbolLarge
        public const uint m_sNameFixupParent = 0x5D0; // CUtlSymbolLarge
        public const uint m_sNameFixupLocal = 0x5D8; // CUtlSymbolLarge
    }

    public static class CPointPush  // CPointEntity
    {
        public const uint m_bEnabled = 0x4B0; // bool
        public const uint m_flMagnitude = 0x4B4; // float
        public const uint m_flRadius = 0x4B8; // float
        public const uint m_flInnerRadius = 0x4BC; // float
        public const uint m_flConeOfInfluence = 0x4C0; // float
        public const uint m_iszFilterName = 0x4C8; // CUtlSymbolLarge
        public const uint m_hFilter = 0x4D0; // CHandle<CBaseFilter>
    }

    public static class CPointScript  // CBaseEntity
    {
    }

    public static class CPointServerCommand  // CPointEntity
    {
    }

    public static class CPointTeleport  // CServerOnlyPointEntity
    {
        public const uint m_vSaveOrigin = 0x4B0; // Vector
        public const uint m_vSaveAngles = 0x4BC; // QAngle
        public const uint m_bTeleportParentedEntities = 0x4C8; // bool
        public const uint m_bTeleportUseCurrentAngle = 0x4C9; // bool
    }

    public static class CPointTemplate  // CLogicalEntity
    {
        public const uint m_iszWorldName = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszSource2EntityLumpName = 0x4B8; // CUtlSymbolLarge
        public const uint m_iszEntityFilterName = 0x4C0; // CUtlSymbolLarge
        public const uint m_flTimeoutInterval = 0x4C8; // float
        public const uint m_bAsynchronouslySpawnEntities = 0x4CC; // bool
        public const uint m_pOutputOnSpawned = 0x4D0; // CEntityIOOutput
        public const uint m_clientOnlyEntityBehavior = 0x4F8; // PointTemplateClientOnlyEntityBehavior_t
        public const uint m_ownerSpawnGroupType = 0x4FC; // PointTemplateOwnerSpawnGroupType_t
        public const uint m_createdSpawnGroupHandles = 0x500; // CUtlVector<uint32_t>
        public const uint m_SpawnedEntityHandles = 0x518; // CUtlVector<CEntityHandle>
        public const uint m_ScriptSpawnCallback = 0x530; // HSCRIPT
        public const uint m_ScriptCallbackScope = 0x538; // HSCRIPT
    }

    public static class CPointValueRemapper  // CBaseEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_bUpdateOnClient = 0x4B1; // bool
        public const uint m_nInputType = 0x4B4; // ValueRemapperInputType_t
        public const uint m_iszRemapLineStartName = 0x4B8; // CUtlSymbolLarge
        public const uint m_iszRemapLineEndName = 0x4C0; // CUtlSymbolLarge
        public const uint m_hRemapLineStart = 0x4C8; // CHandle<CBaseEntity>
        public const uint m_hRemapLineEnd = 0x4CC; // CHandle<CBaseEntity>
        public const uint m_flMaximumChangePerSecond = 0x4D0; // float
        public const uint m_flDisengageDistance = 0x4D4; // float
        public const uint m_flEngageDistance = 0x4D8; // float
        public const uint m_bRequiresUseKey = 0x4DC; // bool
        public const uint m_nOutputType = 0x4E0; // ValueRemapperOutputType_t
        public const uint m_iszOutputEntityName = 0x4E8; // CUtlSymbolLarge
        public const uint m_iszOutputEntity2Name = 0x4F0; // CUtlSymbolLarge
        public const uint m_iszOutputEntity3Name = 0x4F8; // CUtlSymbolLarge
        public const uint m_iszOutputEntity4Name = 0x500; // CUtlSymbolLarge
        public const uint m_hOutputEntities = 0x508; // CNetworkUtlVectorBase<CHandle<CBaseEntity>>
        public const uint m_nHapticsType = 0x520; // ValueRemapperHapticsType_t
        public const uint m_nMomentumType = 0x524; // ValueRemapperMomentumType_t
        public const uint m_flMomentumModifier = 0x528; // float
        public const uint m_flSnapValue = 0x52C; // float
        public const uint m_flCurrentMomentum = 0x530; // float
        public const uint m_nRatchetType = 0x534; // ValueRemapperRatchetType_t
        public const uint m_flRatchetOffset = 0x538; // float
        public const uint m_flInputOffset = 0x53C; // float
        public const uint m_bEngaged = 0x540; // bool
        public const uint m_bFirstUpdate = 0x541; // bool
        public const uint m_flPreviousValue = 0x544; // float
        public const uint m_flPreviousUpdateTickTime = 0x548; // GameTime_t
        public const uint m_vecPreviousTestPoint = 0x54C; // Vector
        public const uint m_hUsingPlayer = 0x558; // CHandle<CBasePlayerPawn>
        public const uint m_flCustomOutputValue = 0x55C; // float
        public const uint m_iszSoundEngage = 0x560; // CUtlSymbolLarge
        public const uint m_iszSoundDisengage = 0x568; // CUtlSymbolLarge
        public const uint m_iszSoundReachedValueZero = 0x570; // CUtlSymbolLarge
        public const uint m_iszSoundReachedValueOne = 0x578; // CUtlSymbolLarge
        public const uint m_iszSoundMovingLoop = 0x580; // CUtlSymbolLarge
        public const uint m_Position = 0x590; // CEntityOutputTemplate<float>
        public const uint m_PositionDelta = 0x5B8; // CEntityOutputTemplate<float>
        public const uint m_OnReachedValueZero = 0x5E0; // CEntityIOOutput
        public const uint m_OnReachedValueOne = 0x608; // CEntityIOOutput
        public const uint m_OnReachedValueCustom = 0x630; // CEntityIOOutput
        public const uint m_OnEngage = 0x658; // CEntityIOOutput
        public const uint m_OnDisengage = 0x680; // CEntityIOOutput
    }

    public static class CPointVelocitySensor  // CPointEntity
    {
        public const uint m_hTargetEntity = 0x4B0; // CHandle<CBaseEntity>
        public const uint m_vecAxis = 0x4B4; // Vector
        public const uint m_bEnabled = 0x4C0; // bool
        public const uint m_fPrevVelocity = 0x4C4; // float
        public const uint m_flAvgInterval = 0x4C8; // float
        public const uint m_Velocity = 0x4D0; // CEntityOutputTemplate<float>
    }

    public static class CPointWorldText  // CModelPointEntity
    {
        public const uint m_messageText = 0x700; // char[512]
        public const uint m_FontName = 0x900; // char[64]
        public const uint m_bEnabled = 0x940; // bool
        public const uint m_bFullbright = 0x941; // bool
        public const uint m_flWorldUnitsPerPx = 0x944; // float
        public const uint m_flFontSize = 0x948; // float
        public const uint m_flDepthOffset = 0x94C; // float
        public const uint m_Color = 0x950; // Color
        public const uint m_nJustifyHorizontal = 0x954; // PointWorldTextJustifyHorizontal_t
        public const uint m_nJustifyVertical = 0x958; // PointWorldTextJustifyVertical_t
        public const uint m_nReorientMode = 0x95C; // PointWorldTextReorientMode_t
    }

    public static class CPostProcessingVolume  // CBaseTrigger
    {
        public const uint m_hPostSettings = 0x8B8; // CStrongHandle<InfoForResourceTypeCPostProcessingResource>
        public const uint m_flFadeDuration = 0x8C0; // float
        public const uint m_flMinLogExposure = 0x8C4; // float
        public const uint m_flMaxLogExposure = 0x8C8; // float
        public const uint m_flMinExposure = 0x8CC; // float
        public const uint m_flMaxExposure = 0x8D0; // float
        public const uint m_flExposureCompensation = 0x8D4; // float
        public const uint m_flExposureFadeSpeedUp = 0x8D8; // float
        public const uint m_flExposureFadeSpeedDown = 0x8DC; // float
        public const uint m_flTonemapEVSmoothingRange = 0x8E0; // float
        public const uint m_bMaster = 0x8E4; // bool
        public const uint m_bExposureControl = 0x8E5; // bool
        public const uint m_flRate = 0x8E8; // float
        public const uint m_flTonemapPercentTarget = 0x8EC; // float
        public const uint m_flTonemapPercentBrightPixels = 0x8F0; // float
        public const uint m_flTonemapMinAvgLum = 0x8F4; // float
    }

    public static class CPrecipitation  // CBaseTrigger
    {
    }

    public static class CPrecipitationBlocker  // CBaseModelEntity
    {
    }

    public static class CPrecipitationVData  // CEntitySubclassVDataBase
    {
        public const uint m_szParticlePrecipitationEffect = 0x28; // CResourceNameTyped<CWeakHandle<InfoForResourceTypeIParticleSystemDefinition>>
        public const uint m_flInnerDistance = 0x108; // float
        public const uint m_nAttachType = 0x10C; // ParticleAttachment_t
        public const uint m_bBatchSameVolumeType = 0x110; // bool
        public const uint m_nRTEnvCP = 0x114; // int32_t
        public const uint m_nRTEnvCPComponent = 0x118; // int32_t
        public const uint m_szModifier = 0x120; // CUtlString
    }

    public static class CPredictedViewModel  // CBaseViewModel
    {
    }

    public static class CProjectedDecal  // CPointEntity
    {
        public const uint m_nTexture = 0x4B0; // int32_t
        public const uint m_flDistance = 0x4B4; // float
    }

    public static class CPropDoorRotating  // CBasePropDoor
    {
        public const uint m_vecAxis = 0xD98; // Vector
        public const uint m_flDistance = 0xDA4; // float
        public const uint m_eSpawnPosition = 0xDA8; // PropDoorRotatingSpawnPos_t
        public const uint m_eOpenDirection = 0xDAC; // PropDoorRotatingOpenDirection_e
        public const uint m_eCurrentOpenDirection = 0xDB0; // PropDoorRotatingOpenDirection_e
        public const uint m_flAjarAngle = 0xDB4; // float
        public const uint m_angRotationAjarDeprecated = 0xDB8; // QAngle
        public const uint m_angRotationClosed = 0xDC4; // QAngle
        public const uint m_angRotationOpenForward = 0xDD0; // QAngle
        public const uint m_angRotationOpenBack = 0xDDC; // QAngle
        public const uint m_angGoal = 0xDE8; // QAngle
        public const uint m_vecForwardBoundsMin = 0xDF4; // Vector
        public const uint m_vecForwardBoundsMax = 0xE00; // Vector
        public const uint m_vecBackBoundsMin = 0xE0C; // Vector
        public const uint m_vecBackBoundsMax = 0xE18; // Vector
        public const uint m_bAjarDoorShouldntAlwaysOpen = 0xE24; // bool
        public const uint m_hEntityBlocker = 0xE28; // CHandle<CEntityBlocker>
    }

    public static class CPropDoorRotatingBreakable  // CPropDoorRotating
    {
        public const uint m_bBreakable = 0xE30; // bool
        public const uint m_isAbleToCloseAreaPortals = 0xE31; // bool
        public const uint m_currentDamageState = 0xE34; // int32_t
        public const uint m_damageStates = 0xE38; // CUtlVector<CUtlSymbolLarge>
    }

    public static class CPulseCell_Inflow_GameEvent  // CPulseCell_Inflow_BaseEntrypoint
    {
        public const uint m_EventName = 0x70; // CBufferString
    }

    public static class CPulseCell_Outflow_PlayVCD  // CPulseCell_BaseFlow
    {
        public const uint m_vcdFilename = 0x48; // CUtlString
        public const uint m_OnFinished = 0x50; // CPulse_OutflowConnection
        public const uint m_Triggers = 0x60; // CUtlVector<CPulse_OutflowConnection>
    }

    public static class CPulseCell_SoundEventStart  // CPulseCell_BaseFlow
    {
        public const uint m_Type = 0x48; // SoundEventStartType_t
    }

    public static class CPulseCell_Step_EntFire  // CPulseCell_BaseFlow
    {
        public const uint m_Input = 0x48; // CUtlString
    }

    public static class CPulseCell_Step_SetAnimGraphParam  // CPulseCell_BaseFlow
    {
        public const uint m_ParamName = 0x48; // CUtlString
    }

    public static class CPulseCell_Value_FindEntByName  // CPulseCell_BaseValue
    {
        public const uint m_EntityType = 0x48; // CUtlString
    }

    public static class CPulseGraphInstance_ServerPointEntity  // CBasePulseGraphInstance
    {
    }

    public static class CPulseServerFuncs 
    {
    }

    public static class CPulseServerFuncs_Sounds 
    {
    }

    public static class CPushable  // CBreakable
    {
    }

    public static class CRR_Response 
    {
        public const uint m_Type = 0x0; // uint8_t
        public const uint m_szResponseName = 0x1; // char[192]
        public const uint m_szMatchingRule = 0xC1; // char[128]
        public const uint m_Params = 0x148; // ResponseParams
        public const uint m_fMatchScore = 0x168; // float
        public const uint m_szSpeakerContext = 0x170; // char*
        public const uint m_szWorldContext = 0x178; // char*
        public const uint m_Followup = 0x180; // ResponseFollowup
        public const uint m_pchCriteriaNames = 0x1B8; // CUtlVector<CUtlSymbol>
        public const uint m_pchCriteriaValues = 0x1D0; // CUtlVector<char*>
    }

    public static class CRagdollConstraint  // CPhysConstraint
    {
        public const uint m_xmin = 0x508; // float
        public const uint m_xmax = 0x50C; // float
        public const uint m_ymin = 0x510; // float
        public const uint m_ymax = 0x514; // float
        public const uint m_zmin = 0x518; // float
        public const uint m_zmax = 0x51C; // float
        public const uint m_xfriction = 0x520; // float
        public const uint m_yfriction = 0x524; // float
        public const uint m_zfriction = 0x528; // float
    }

    public static class CRagdollMagnet  // CPointEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_radius = 0x4B4; // float
        public const uint m_force = 0x4B8; // float
        public const uint m_axis = 0x4BC; // Vector
    }

    public static class CRagdollManager  // CBaseEntity
    {
        public const uint m_iCurrentMaxRagdollCount = 0x4B0; // int8_t
        public const uint m_iMaxRagdollCount = 0x4B4; // int32_t
        public const uint m_bSaveImportant = 0x4B8; // bool
    }

    public static class CRagdollProp  // CBaseAnimGraph
    {
        public const uint m_ragdoll = 0x898; // ragdoll_t
        public const uint m_bStartDisabled = 0x8D0; // bool
        public const uint m_ragPos = 0x8D8; // CNetworkUtlVectorBase<Vector>
        public const uint m_ragAngles = 0x8F0; // CNetworkUtlVectorBase<QAngle>
        public const uint m_hRagdollSource = 0x908; // CHandle<CBaseEntity>
        public const uint m_lastUpdateTickCount = 0x90C; // uint32_t
        public const uint m_allAsleep = 0x910; // bool
        public const uint m_bFirstCollisionAfterLaunch = 0x911; // bool
        public const uint m_hDamageEntity = 0x914; // CHandle<CBaseEntity>
        public const uint m_hKiller = 0x918; // CHandle<CBaseEntity>
        public const uint m_hPhysicsAttacker = 0x91C; // CHandle<CBasePlayerPawn>
        public const uint m_flLastPhysicsInfluenceTime = 0x920; // GameTime_t
        public const uint m_flFadeOutStartTime = 0x924; // GameTime_t
        public const uint m_flFadeTime = 0x928; // float
        public const uint m_vecLastOrigin = 0x92C; // Vector
        public const uint m_flAwakeTime = 0x938; // GameTime_t
        public const uint m_flLastOriginChangeTime = 0x93C; // GameTime_t
        public const uint m_nBloodColor = 0x940; // int32_t
        public const uint m_strOriginClassName = 0x948; // CUtlSymbolLarge
        public const uint m_strSourceClassName = 0x950; // CUtlSymbolLarge
        public const uint m_bHasBeenPhysgunned = 0x958; // bool
        public const uint m_bShouldTeleportPhysics = 0x959; // bool
        public const uint m_flBlendWeight = 0x95C; // float
        public const uint m_flDefaultFadeScale = 0x960; // float
        public const uint m_ragdollMins = 0x968; // CUtlVector<Vector>
        public const uint m_ragdollMaxs = 0x980; // CUtlVector<Vector>
        public const uint m_bShouldDeleteActivationRecord = 0x998; // bool
        public const uint m_bValidatePoweredRagdollPose = 0x9F8; // bool
    }

    public static class CRagdollPropAlias_physics_prop_ragdoll  // CRagdollProp
    {
    }

    public static class CRagdollPropAttached  // CRagdollProp
    {
        public const uint m_boneIndexAttached = 0xA38; // uint32_t
        public const uint m_ragdollAttachedObjectIndex = 0xA3C; // uint32_t
        public const uint m_attachmentPointBoneSpace = 0xA40; // Vector
        public const uint m_attachmentPointRagdollSpace = 0xA4C; // Vector
        public const uint m_bShouldDetach = 0xA58; // bool
        public const uint m_bShouldDeleteAttachedActivationRecord = 0xA68; // bool
    }

    public static class CRandSimTimer  // CSimpleSimTimer
    {
        public const uint m_minInterval = 0x8; // float
        public const uint m_maxInterval = 0xC; // float
    }

    public static class CRandStopwatch  // CStopwatchBase
    {
        public const uint m_minInterval = 0xC; // float
        public const uint m_maxInterval = 0x10; // float
    }

    public static class CRangeFloat 
    {
        public const uint m_pValue = 0x0; // float[2]
    }

    public static class CRangeInt 
    {
        public const uint m_pValue = 0x0; // int32_t[2]
    }

    public static class CRectLight  // CBarnLight
    {
        public const uint m_bShowLight = 0x928; // bool
    }

    public static class CRemapFloat 
    {
        public const uint m_pValue = 0x0; // float[4]
    }

    public static class CRenderComponent  // CEntityComponent
    {
        public const uint __m_pChainEntity = 0x10; // CNetworkVarChainer
        public const uint m_bIsRenderingWithViewModels = 0x50; // bool
        public const uint m_nSplitscreenFlags = 0x54; // uint32_t
        public const uint m_bEnableRendering = 0x60; // bool
        public const uint m_bInterpolationReadyToDraw = 0xB0; // bool
    }

    public static class CResponseCriteriaSet 
    {
        public const uint m_nNumPrefixedContexts = 0x28; // int32_t
        public const uint m_bOverrideOnAppend = 0x2C; // bool
    }

    public static class CResponseQueue 
    {
        public const uint m_ExpresserTargets = 0x50; // CUtlVector<CAI_Expresser*>
    }

    public static class CResponseQueue_CDeferredResponse 
    {
        public const uint m_contexts = 0x10; // CResponseCriteriaSet
        public const uint m_fDispatchTime = 0x40; // float
        public const uint m_hIssuer = 0x44; // CHandle<CBaseEntity>
        public const uint m_response = 0x50; // CRR_Response
        public const uint m_bResponseValid = 0x238; // bool
    }

    public static class CRetakeGameRules 
    {
        public const uint m_nMatchSeed = 0xF8; // int32_t
        public const uint m_bBlockersPresent = 0xFC; // bool
        public const uint m_bRoundInProgress = 0xFD; // bool
        public const uint m_iFirstSecondHalfRound = 0x100; // int32_t
        public const uint m_iBombSite = 0x104; // int32_t
    }

    public static class CRevertSaved  // CModelPointEntity
    {
        public const uint m_loadTime = 0x700; // float
        public const uint m_Duration = 0x704; // float
        public const uint m_HoldTime = 0x708; // float
    }

    public static class CRopeKeyframe  // CBaseModelEntity
    {
        public const uint m_RopeFlags = 0x708; // uint16_t
        public const uint m_iNextLinkName = 0x710; // CUtlSymbolLarge
        public const uint m_Slack = 0x718; // int16_t
        public const uint m_Width = 0x71C; // float
        public const uint m_TextureScale = 0x720; // float
        public const uint m_nSegments = 0x724; // uint8_t
        public const uint m_bConstrainBetweenEndpoints = 0x725; // bool
        public const uint m_strRopeMaterialModel = 0x728; // CUtlSymbolLarge
        public const uint m_iRopeMaterialModelIndex = 0x730; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_Subdiv = 0x738; // uint8_t
        public const uint m_nChangeCount = 0x739; // uint8_t
        public const uint m_RopeLength = 0x73A; // int16_t
        public const uint m_fLockedPoints = 0x73C; // uint8_t
        public const uint m_bCreatedFromMapFile = 0x73D; // bool
        public const uint m_flScrollSpeed = 0x740; // float
        public const uint m_bStartPointValid = 0x744; // bool
        public const uint m_bEndPointValid = 0x745; // bool
        public const uint m_hStartPoint = 0x748; // CHandle<CBaseEntity>
        public const uint m_hEndPoint = 0x74C; // CHandle<CBaseEntity>
        public const uint m_iStartAttachment = 0x750; // AttachmentHandle_t
        public const uint m_iEndAttachment = 0x751; // AttachmentHandle_t
    }

    public static class CRopeKeyframeAlias_move_rope  // CRopeKeyframe
    {
    }

    public static class CRotButton  // CBaseButton
    {
    }

    public static class CRotDoor  // CBaseDoor
    {
        public const uint m_bSolidBsp = 0x988; // bool
    }

    public static class CRuleBrushEntity  // CRuleEntity
    {
    }

    public static class CRuleEntity  // CBaseModelEntity
    {
        public const uint m_iszMaster = 0x700; // CUtlSymbolLarge
    }

    public static class CRulePointEntity  // CRuleEntity
    {
        public const uint m_Score = 0x708; // int32_t
    }

    public static class CSAdditionalMatchStats_t  // CSAdditionalPerRoundStats_t
    {
        public const uint m_numRoundsSurvived = 0x14; // int32_t
        public const uint m_maxNumRoundsSurvived = 0x18; // int32_t
        public const uint m_numRoundsSurvivedTotal = 0x1C; // int32_t
        public const uint m_iRoundsWonWithoutPurchase = 0x20; // int32_t
        public const uint m_iRoundsWonWithoutPurchaseTotal = 0x24; // int32_t
        public const uint m_numFirstKills = 0x28; // int32_t
        public const uint m_numClutchKills = 0x2C; // int32_t
        public const uint m_numPistolKills = 0x30; // int32_t
        public const uint m_numSniperKills = 0x34; // int32_t
        public const uint m_iNumSuicides = 0x38; // int32_t
        public const uint m_iNumTeamKills = 0x3C; // int32_t
        public const uint m_iTeamDamage = 0x40; // int32_t
    }

    public static class CSAdditionalPerRoundStats_t 
    {
        public const uint m_numChickensKilled = 0x0; // int32_t
        public const uint m_killsWhileBlind = 0x4; // int32_t
        public const uint m_bombCarrierkills = 0x8; // int32_t
        public const uint m_iBurnDamageInflicted = 0xC; // int32_t
        public const uint m_iDinks = 0x10; // int32_t
    }

    public static class CSMatchStats_t  // CSPerRoundStats_t
    {
        public const uint m_iEnemy5Ks = 0x68; // int32_t
        public const uint m_iEnemy4Ks = 0x6C; // int32_t
        public const uint m_iEnemy3Ks = 0x70; // int32_t
        public const uint m_iEnemy2Ks = 0x74; // int32_t
        public const uint m_iUtility_Count = 0x78; // int32_t
        public const uint m_iUtility_Successes = 0x7C; // int32_t
        public const uint m_iUtility_Enemies = 0x80; // int32_t
        public const uint m_iFlash_Count = 0x84; // int32_t
        public const uint m_iFlash_Successes = 0x88; // int32_t
        public const uint m_nHealthPointsRemovedTotal = 0x8C; // int32_t
        public const uint m_nHealthPointsDealtTotal = 0x90; // int32_t
        public const uint m_nShotsFiredTotal = 0x94; // int32_t
        public const uint m_nShotsOnTargetTotal = 0x98; // int32_t
        public const uint m_i1v1Count = 0x9C; // int32_t
        public const uint m_i1v1Wins = 0xA0; // int32_t
        public const uint m_i1v2Count = 0xA4; // int32_t
        public const uint m_i1v2Wins = 0xA8; // int32_t
        public const uint m_iEntryCount = 0xAC; // int32_t
        public const uint m_iEntryWins = 0xB0; // int32_t
    }

    public static class CSPerRoundStats_t 
    {
        public const uint m_iKills = 0x30; // int32_t
        public const uint m_iDeaths = 0x34; // int32_t
        public const uint m_iAssists = 0x38; // int32_t
        public const uint m_iDamage = 0x3C; // int32_t
        public const uint m_iEquipmentValue = 0x40; // int32_t
        public const uint m_iMoneySaved = 0x44; // int32_t
        public const uint m_iKillReward = 0x48; // int32_t
        public const uint m_iLiveTime = 0x4C; // int32_t
        public const uint m_iHeadShotKills = 0x50; // int32_t
        public const uint m_iObjective = 0x54; // int32_t
        public const uint m_iCashEarned = 0x58; // int32_t
        public const uint m_iUtilityDamage = 0x5C; // int32_t
        public const uint m_iEnemiesFlashed = 0x60; // int32_t
    }

    public static class CSceneEntity  // CPointEntity
    {
        public const uint m_iszSceneFile = 0x4B8; // CUtlSymbolLarge
        public const uint m_iszResumeSceneFile = 0x4C0; // CUtlSymbolLarge
        public const uint m_iszTarget1 = 0x4C8; // CUtlSymbolLarge
        public const uint m_iszTarget2 = 0x4D0; // CUtlSymbolLarge
        public const uint m_iszTarget3 = 0x4D8; // CUtlSymbolLarge
        public const uint m_iszTarget4 = 0x4E0; // CUtlSymbolLarge
        public const uint m_iszTarget5 = 0x4E8; // CUtlSymbolLarge
        public const uint m_iszTarget6 = 0x4F0; // CUtlSymbolLarge
        public const uint m_iszTarget7 = 0x4F8; // CUtlSymbolLarge
        public const uint m_iszTarget8 = 0x500; // CUtlSymbolLarge
        public const uint m_hTarget1 = 0x508; // CHandle<CBaseEntity>
        public const uint m_hTarget2 = 0x50C; // CHandle<CBaseEntity>
        public const uint m_hTarget3 = 0x510; // CHandle<CBaseEntity>
        public const uint m_hTarget4 = 0x514; // CHandle<CBaseEntity>
        public const uint m_hTarget5 = 0x518; // CHandle<CBaseEntity>
        public const uint m_hTarget6 = 0x51C; // CHandle<CBaseEntity>
        public const uint m_hTarget7 = 0x520; // CHandle<CBaseEntity>
        public const uint m_hTarget8 = 0x524; // CHandle<CBaseEntity>
        public const uint m_bIsPlayingBack = 0x528; // bool
        public const uint m_bPaused = 0x529; // bool
        public const uint m_bMultiplayer = 0x52A; // bool
        public const uint m_bAutogenerated = 0x52B; // bool
        public const uint m_flForceClientTime = 0x52C; // float
        public const uint m_flCurrentTime = 0x530; // float
        public const uint m_flFrameTime = 0x534; // float
        public const uint m_bCancelAtNextInterrupt = 0x538; // bool
        public const uint m_fPitch = 0x53C; // float
        public const uint m_bAutomated = 0x540; // bool
        public const uint m_nAutomatedAction = 0x544; // int32_t
        public const uint m_flAutomationDelay = 0x548; // float
        public const uint m_flAutomationTime = 0x54C; // float
        public const uint m_hWaitingForThisResumeScene = 0x550; // CHandle<CBaseEntity>
        public const uint m_bWaitingForResumeScene = 0x554; // bool
        public const uint m_bPausedViaInput = 0x555; // bool
        public const uint m_bPauseAtNextInterrupt = 0x556; // bool
        public const uint m_bWaitingForActor = 0x557; // bool
        public const uint m_bWaitingForInterrupt = 0x558; // bool
        public const uint m_bInterruptedActorsScenes = 0x559; // bool
        public const uint m_bBreakOnNonIdle = 0x55A; // bool
        public const uint m_hActorList = 0x560; // CNetworkUtlVectorBase<CHandle<CBaseFlex>>
        public const uint m_hRemoveActorList = 0x578; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_nSceneFlushCounter = 0x5A0; // int32_t
        public const uint m_nSceneStringIndex = 0x5A4; // uint16_t
        public const uint m_OnStart = 0x5A8; // CEntityIOOutput
        public const uint m_OnCompletion = 0x5D0; // CEntityIOOutput
        public const uint m_OnCanceled = 0x5F8; // CEntityIOOutput
        public const uint m_OnPaused = 0x620; // CEntityIOOutput
        public const uint m_OnResumed = 0x648; // CEntityIOOutput
        public const uint m_OnTrigger = 0x670; // CEntityIOOutput[16]
        public const uint m_hInterruptScene = 0x980; // CHandle<CSceneEntity>
        public const uint m_nInterruptCount = 0x984; // int32_t
        public const uint m_bSceneMissing = 0x988; // bool
        public const uint m_bInterrupted = 0x989; // bool
        public const uint m_bCompletedEarly = 0x98A; // bool
        public const uint m_bInterruptSceneFinished = 0x98B; // bool
        public const uint m_bRestoring = 0x98C; // bool
        public const uint m_hNotifySceneCompletion = 0x990; // CUtlVector<CHandle<CSceneEntity>>
        public const uint m_hListManagers = 0x9A8; // CUtlVector<CHandle<CSceneListManager>>
        public const uint m_iszSoundName = 0x9E8; // CUtlSymbolLarge
        public const uint m_hActor = 0x9F0; // CHandle<CBaseFlex>
        public const uint m_hActivator = 0x9F4; // CHandle<CBaseEntity>
        public const uint m_BusyActor = 0x9F8; // int32_t
        public const uint m_iPlayerDeathBehavior = 0x9FC; // SceneOnPlayerDeath_t
    }

    public static class CSceneEntityAlias_logic_choreographed_scene  // CSceneEntity
    {
    }

    public static class CSceneEventInfo 
    {
        public const uint m_iLayer = 0x0; // int32_t
        public const uint m_iPriority = 0x4; // int32_t
        public const uint m_hSequence = 0x8; // HSequence
        public const uint m_flWeight = 0xC; // float
        public const uint m_bIsMoving = 0x10; // bool
        public const uint m_bHasArrived = 0x11; // bool
        public const uint m_flInitialYaw = 0x14; // float
        public const uint m_flTargetYaw = 0x18; // float
        public const uint m_flFacingYaw = 0x1C; // float
        public const uint m_nType = 0x20; // int32_t
        public const uint m_flNext = 0x24; // GameTime_t
        public const uint m_bIsGesture = 0x28; // bool
        public const uint m_bShouldRemove = 0x29; // bool
        public const uint m_hTarget = 0x54; // CHandle<CBaseEntity>
        public const uint m_nSceneEventId = 0x58; // uint32_t
        public const uint m_bClientSide = 0x5C; // bool
        public const uint m_bStarted = 0x5D; // bool
    }

    public static class CSceneListManager  // CLogicalEntity
    {
        public const uint m_hListManagers = 0x4B0; // CUtlVector<CHandle<CSceneListManager>>
        public const uint m_iszScenes = 0x4C8; // CUtlSymbolLarge[16]
        public const uint m_hScenes = 0x548; // CHandle<CBaseEntity>[16]
    }

    public static class CScriptComponent  // CEntityComponent
    {
        public const uint m_scriptClassName = 0x30; // CUtlSymbolLarge
    }

    public static class CScriptItem  // CItem
    {
        public const uint m_OnPlayerPickup = 0x968; // CEntityIOOutput
        public const uint m_MoveTypeOverride = 0x990; // MoveType_t
    }

    public static class CScriptNavBlocker  // CFuncNavBlocker
    {
        public const uint m_vExtent = 0x710; // Vector
    }

    public static class CScriptTriggerHurt  // CTriggerHurt
    {
        public const uint m_vExtent = 0x948; // Vector
    }

    public static class CScriptTriggerMultiple  // CTriggerMultiple
    {
        public const uint m_vExtent = 0x8D0; // Vector
    }

    public static class CScriptTriggerOnce  // CTriggerOnce
    {
        public const uint m_vExtent = 0x8D0; // Vector
    }

    public static class CScriptTriggerPush  // CTriggerPush
    {
        public const uint m_vExtent = 0x8D0; // Vector
    }

    public static class CScriptUniformRandomStream 
    {
        public const uint m_hScriptScope = 0x8; // HSCRIPT
        public const uint m_nInitialSeed = 0x9C; // int32_t
    }

    public static class CScriptedSequence  // CBaseEntity
    {
        public const uint m_iszEntry = 0x4B0; // CUtlSymbolLarge
        public const uint m_iszPreIdle = 0x4B8; // CUtlSymbolLarge
        public const uint m_iszPlay = 0x4C0; // CUtlSymbolLarge
        public const uint m_iszPostIdle = 0x4C8; // CUtlSymbolLarge
        public const uint m_iszModifierToAddOnPlay = 0x4D0; // CUtlSymbolLarge
        public const uint m_iszNextScript = 0x4D8; // CUtlSymbolLarge
        public const uint m_iszEntity = 0x4E0; // CUtlSymbolLarge
        public const uint m_iszSyncGroup = 0x4E8; // CUtlSymbolLarge
        public const uint m_nMoveTo = 0x4F0; // ScriptedMoveTo_t
        public const uint m_bIsPlayingPreIdle = 0x4F4; // bool
        public const uint m_bIsPlayingEntry = 0x4F5; // bool
        public const uint m_bIsPlayingAction = 0x4F6; // bool
        public const uint m_bIsPlayingPostIdle = 0x4F7; // bool
        public const uint m_bLoopPreIdleSequence = 0x4F8; // bool
        public const uint m_bLoopActionSequence = 0x4F9; // bool
        public const uint m_bLoopPostIdleSequence = 0x4FA; // bool
        public const uint m_bSynchPostIdles = 0x4FB; // bool
        public const uint m_bIgnoreGravity = 0x4FC; // bool
        public const uint m_bDisableNPCCollisions = 0x4FD; // bool
        public const uint m_bKeepAnimgraphLockedPost = 0x4FE; // bool
        public const uint m_bDontAddModifiers = 0x4FF; // bool
        public const uint m_flRadius = 0x500; // float
        public const uint m_flRepeat = 0x504; // float
        public const uint m_flPlayAnimFadeInTime = 0x508; // float
        public const uint m_flMoveInterpTime = 0x50C; // float
        public const uint m_flAngRate = 0x510; // float
        public const uint m_iDelay = 0x514; // int32_t
        public const uint m_startTime = 0x518; // GameTime_t
        public const uint m_bWaitForBeginSequence = 0x51C; // bool
        public const uint m_saved_effects = 0x520; // int32_t
        public const uint m_savedFlags = 0x524; // int32_t
        public const uint m_savedCollisionGroup = 0x528; // int32_t
        public const uint m_interruptable = 0x52C; // bool
        public const uint m_sequenceStarted = 0x52D; // bool
        public const uint m_bPrevAnimatedEveryTick = 0x52E; // bool
        public const uint m_bForcedAnimatedEveryTick = 0x52F; // bool
        public const uint m_bPositionRelativeToOtherEntity = 0x530; // bool
        public const uint m_hTargetEnt = 0x534; // CHandle<CBaseEntity>
        public const uint m_hNextCine = 0x538; // CHandle<CScriptedSequence>
        public const uint m_bThinking = 0x53C; // bool
        public const uint m_bInitiatedSelfDelete = 0x53D; // bool
        public const uint m_bIsTeleportingDueToMoveTo = 0x53E; // bool
        public const uint m_bAllowCustomInterruptConditions = 0x53F; // bool
        public const uint m_hLastFoundEntity = 0x540; // CHandle<CBaseEntity>
        public const uint m_hForcedTarget = 0x544; // CHandle<CBaseAnimGraph>
        public const uint m_bDontCancelOtherSequences = 0x548; // bool
        public const uint m_bForceSynch = 0x549; // bool
        public const uint m_bTargetWasAsleep = 0x54A; // bool
        public const uint m_bPreventUpdateYawOnFinish = 0x54B; // bool
        public const uint m_bEnsureOnNavmeshOnFinish = 0x54C; // bool
        public const uint m_onDeathBehavior = 0x550; // ScriptedOnDeath_t
        public const uint m_ConflictResponse = 0x554; // ScriptedConflictResponse_t
        public const uint m_OnBeginSequence = 0x558; // CEntityIOOutput
        public const uint m_OnActionStartOrLoop = 0x580; // CEntityIOOutput
        public const uint m_OnEndSequence = 0x5A8; // CEntityIOOutput
        public const uint m_OnPostIdleEndSequence = 0x5D0; // CEntityIOOutput
        public const uint m_OnCancelSequence = 0x5F8; // CEntityIOOutput
        public const uint m_OnCancelFailedSequence = 0x620; // CEntityIOOutput
        public const uint m_OnScriptEvent = 0x648; // CEntityIOOutput[8]
        public const uint m_matOtherToMain = 0x790; // CTransform
        public const uint m_hInteractionMainEntity = 0x7B0; // CHandle<CBaseEntity>
        public const uint m_iPlayerDeathBehavior = 0x7B4; // int32_t
    }

    public static class CSensorGrenade  // CBaseCSGrenade
    {
    }

    public static class CSensorGrenadeProjectile  // CBaseCSGrenadeProjectile
    {
        public const uint m_fExpireTime = 0xA40; // GameTime_t
        public const uint m_fNextDetectPlayerSound = 0xA44; // GameTime_t
        public const uint m_hDisplayGrenade = 0xA48; // CHandle<CBaseEntity>
    }

    public static class CServerOnlyEntity  // CBaseEntity
    {
    }

    public static class CServerOnlyModelEntity  // CBaseModelEntity
    {
    }

    public static class CServerOnlyPointEntity  // CServerOnlyEntity
    {
    }

    public static class CServerRagdollTrigger  // CBaseTrigger
    {
    }

    public static class CShatterGlassShard 
    {
        public const uint m_hShardHandle = 0x8; // uint32_t
        public const uint m_vecPanelVertices = 0x10; // CUtlVector<Vector2D>
        public const uint m_vLocalPanelSpaceOrigin = 0x28; // Vector2D
        public const uint m_hModel = 0x30; // CStrongHandle<InfoForResourceTypeCModel>
        public const uint m_hPhysicsEntity = 0x38; // CHandle<CShatterGlassShardPhysics>
        public const uint m_hParentPanel = 0x3C; // CHandle<CFuncShatterglass>
        public const uint m_hParentShard = 0x40; // uint32_t
        public const uint m_ShatterStressType = 0x44; // ShatterGlassStressType
        public const uint m_vecStressVelocity = 0x48; // Vector
        public const uint m_bCreatedModel = 0x54; // bool
        public const uint m_flLongestEdge = 0x58; // float
        public const uint m_flShortestEdge = 0x5C; // float
        public const uint m_flLongestAcross = 0x60; // float
        public const uint m_flShortestAcross = 0x64; // float
        public const uint m_flSumOfAllEdges = 0x68; // float
        public const uint m_flArea = 0x6C; // float
        public const uint m_nOnFrameEdge = 0x70; // OnFrame
        public const uint m_nParentPanelsNthShard = 0x74; // int32_t
        public const uint m_nSubShardGeneration = 0x78; // int32_t
        public const uint m_vecAverageVertPosition = 0x7C; // Vector2D
        public const uint m_bAverageVertPositionIsValid = 0x84; // bool
        public const uint m_vecPanelSpaceStressPositionA = 0x88; // Vector2D
        public const uint m_vecPanelSpaceStressPositionB = 0x90; // Vector2D
        public const uint m_bStressPositionAIsValid = 0x98; // bool
        public const uint m_bStressPositionBIsValid = 0x99; // bool
        public const uint m_bFlaggedForRemoval = 0x9A; // bool
        public const uint m_flPhysicsEntitySpawnedAtTime = 0x9C; // GameTime_t
        public const uint m_bShatterRateLimited = 0xA0; // bool
        public const uint m_hEntityHittingMe = 0xA4; // CHandle<CBaseEntity>
        public const uint m_vecNeighbors = 0xA8; // CUtlVector<uint32_t>
    }

    public static class CShatterGlassShardPhysics  // CPhysicsProp
    {
        public const uint m_bDebris = 0xB78; // bool
        public const uint m_hParentShard = 0xB7C; // uint32_t
        public const uint m_ShardDesc = 0xB80; // shard_model_desc_t
    }

    public static class CShower  // CModelPointEntity
    {
    }

    public static class CSimTimer  // CSimpleSimTimer
    {
        public const uint m_interval = 0x8; // float
    }

    public static class CSimpleMarkupVolumeTagged  // CMarkupVolumeTagged
    {
    }

    public static class CSimpleSimTimer 
    {
        public const uint m_next = 0x0; // GameTime_t
        public const uint m_nWorldGroupId = 0x4; // WorldGroupId_t
    }

    public static class CSimpleStopwatch  // CStopwatchBase
    {
    }

    public static class CSingleplayRules  // CGameRules
    {
        public const uint m_bSinglePlayerGameEnding = 0x90; // bool
    }

    public static class CSkeletonAnimationController  // ISkeletonAnimationController
    {
        public const uint m_pSkeletonInstance = 0x8; // CSkeletonInstance*
    }

    public static class CSkeletonInstance  // CGameSceneNode
    {
        public const uint m_modelState = 0x160; // CModelState
        public const uint m_bIsAnimationEnabled = 0x390; // bool
        public const uint m_bUseParentRenderBounds = 0x391; // bool
        public const uint m_bDisableSolidCollisionsForHierarchy = 0x392; // bool
        public const uint m_bDirtyMotionType = 0x0; // bitfield:1
        public const uint m_bIsGeneratingLatchedParentSpaceState = 0x0; // bitfield:1
        public const uint m_materialGroup = 0x394; // CUtlStringToken
        public const uint m_nHitboxSet = 0x398; // uint8_t
    }

    public static class CSkillDamage 
    {
        public const uint m_flDamage = 0x0; // CSkillFloat
        public const uint m_flPhysicsForceDamage = 0x10; // float
    }

    public static class CSkillFloat 
    {
        public const uint m_pValue = 0x0; // float[4]
    }

    public static class CSkillInt 
    {
        public const uint m_pValue = 0x0; // int32_t[4]
    }

    public static class CSkyCamera  // CBaseEntity
    {
        public const uint m_skyboxData = 0x4B0; // sky3dparams_t
        public const uint m_skyboxSlotToken = 0x540; // CUtlStringToken
        public const uint m_bUseAngles = 0x544; // bool
        public const uint m_pNext = 0x548; // CSkyCamera*
    }

    public static class CSkyboxReference  // CBaseEntity
    {
        public const uint m_worldGroupId = 0x4B0; // WorldGroupId_t
        public const uint m_hSkyCamera = 0x4B4; // CHandle<CSkyCamera>
    }

    public static class CSmokeGrenade  // CBaseCSGrenade
    {
    }

    public static class CSmokeGrenadeProjectile  // CBaseCSGrenadeProjectile
    {
        public const uint m_nSmokeEffectTickBegin = 0xA58; // int32_t
        public const uint m_bDidSmokeEffect = 0xA5C; // bool
        public const uint m_nRandomSeed = 0xA60; // int32_t
        public const uint m_vSmokeColor = 0xA64; // Vector
        public const uint m_vSmokeDetonationPos = 0xA70; // Vector
        public const uint m_VoxelFrameData = 0xA80; // CUtlVector<uint8_t>
        public const uint m_flLastBounce = 0xA98; // GameTime_t
        public const uint m_fllastSimulationTime = 0xA9C; // GameTime_t
    }

    public static class CSmoothFunc 
    {
        public const uint m_flSmoothAmplitude = 0x8; // float
        public const uint m_flSmoothBias = 0xC; // float
        public const uint m_flSmoothDuration = 0x10; // float
        public const uint m_flSmoothRemainingTime = 0x14; // float
        public const uint m_nSmoothDir = 0x18; // int32_t
    }

    public static class CSound 
    {
        public const uint m_hOwner = 0x0; // CHandle<CBaseEntity>
        public const uint m_hTarget = 0x4; // CHandle<CBaseEntity>
        public const uint m_iVolume = 0x8; // int32_t
        public const uint m_flOcclusionScale = 0xC; // float
        public const uint m_iType = 0x10; // int32_t
        public const uint m_iNextAudible = 0x14; // int32_t
        public const uint m_flExpireTime = 0x18; // GameTime_t
        public const uint m_iNext = 0x1C; // int16_t
        public const uint m_bNoExpirationTime = 0x1E; // bool
        public const uint m_ownerChannelIndex = 0x20; // int32_t
        public const uint m_vecOrigin = 0x24; // Vector
        public const uint m_bHasOwner = 0x30; // bool
    }

    public static class CSoundAreaEntityBase  // CBaseEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_iszSoundAreaType = 0x4B8; // CUtlSymbolLarge
        public const uint m_vPos = 0x4C0; // Vector
    }

    public static class CSoundAreaEntityOrientedBox  // CSoundAreaEntityBase
    {
        public const uint m_vMin = 0x4D0; // Vector
        public const uint m_vMax = 0x4DC; // Vector
    }

    public static class CSoundAreaEntitySphere  // CSoundAreaEntityBase
    {
        public const uint m_flRadius = 0x4D0; // float
    }

    public static class CSoundEnt  // CPointEntity
    {
        public const uint m_iFreeSound = 0x4B0; // int32_t
        public const uint m_iActiveSound = 0x4B4; // int32_t
        public const uint m_cLastActiveSounds = 0x4B8; // int32_t
        public const uint m_SoundPool = 0x4BC; // CSound[128]
    }

    public static class CSoundEnvelope 
    {
        public const uint m_current = 0x0; // float
        public const uint m_target = 0x4; // float
        public const uint m_rate = 0x8; // float
        public const uint m_forceupdate = 0xC; // bool
    }

    public static class CSoundEventAABBEntity  // CSoundEventEntity
    {
        public const uint m_vMins = 0x558; // Vector
        public const uint m_vMaxs = 0x564; // Vector
    }

    public static class CSoundEventEntity  // CBaseEntity
    {
        public const uint m_bStartOnSpawn = 0x4B0; // bool
        public const uint m_bToLocalPlayer = 0x4B1; // bool
        public const uint m_bStopOnNew = 0x4B2; // bool
        public const uint m_bSaveRestore = 0x4B3; // bool
        public const uint m_bSavedIsPlaying = 0x4B4; // bool
        public const uint m_flSavedElapsedTime = 0x4B8; // float
        public const uint m_iszSourceEntityName = 0x4C0; // CUtlSymbolLarge
        public const uint m_iszAttachmentName = 0x4C8; // CUtlSymbolLarge
        public const uint m_onGUIDChanged = 0x4D0; // CEntityOutputTemplate<uint64_t>
        public const uint m_onSoundFinished = 0x4F8; // CEntityIOOutput
        public const uint m_iszSoundName = 0x540; // CUtlSymbolLarge
        public const uint m_hSource = 0x550; // CEntityHandle
    }

    public static class CSoundEventEntityAlias_snd_event_point  // CSoundEventEntity
    {
    }

    public static class CSoundEventOBBEntity  // CSoundEventEntity
    {
        public const uint m_vMins = 0x558; // Vector
        public const uint m_vMaxs = 0x564; // Vector
    }

    public static class CSoundEventParameter  // CBaseEntity
    {
        public const uint m_iszParamName = 0x4B8; // CUtlSymbolLarge
        public const uint m_flFloatValue = 0x4C0; // float
    }

    public static class CSoundEventPathCornerEntity  // CSoundEventEntity
    {
        public const uint m_iszPathCorner = 0x558; // CUtlSymbolLarge
        public const uint m_iCountMax = 0x560; // int32_t
        public const uint m_flDistanceMax = 0x564; // float
        public const uint m_flDistMaxSqr = 0x568; // float
        public const uint m_flDotProductMax = 0x56C; // float
        public const uint bPlaying = 0x570; // bool
    }

    public static class CSoundOpvarSetAABBEntity  // CSoundOpvarSetPointEntity
    {
        public const uint m_vDistanceInnerMins = 0x648; // Vector
        public const uint m_vDistanceInnerMaxs = 0x654; // Vector
        public const uint m_vDistanceOuterMins = 0x660; // Vector
        public const uint m_vDistanceOuterMaxs = 0x66C; // Vector
        public const uint m_nAABBDirection = 0x678; // int32_t
        public const uint m_vInnerMins = 0x67C; // Vector
        public const uint m_vInnerMaxs = 0x688; // Vector
        public const uint m_vOuterMins = 0x694; // Vector
        public const uint m_vOuterMaxs = 0x6A0; // Vector
    }

    public static class CSoundOpvarSetEntity  // CBaseEntity
    {
        public const uint m_iszStackName = 0x4B8; // CUtlSymbolLarge
        public const uint m_iszOperatorName = 0x4C0; // CUtlSymbolLarge
        public const uint m_iszOpvarName = 0x4C8; // CUtlSymbolLarge
        public const uint m_nOpvarType = 0x4D0; // int32_t
        public const uint m_nOpvarIndex = 0x4D4; // int32_t
        public const uint m_flOpvarValue = 0x4D8; // float
        public const uint m_OpvarValueString = 0x4E0; // CUtlSymbolLarge
        public const uint m_bSetOnSpawn = 0x4E8; // bool
    }

    public static class CSoundOpvarSetOBBEntity  // CSoundOpvarSetAABBEntity
    {
    }

    public static class CSoundOpvarSetOBBWindEntity  // CSoundOpvarSetPointBase
    {
        public const uint m_vMins = 0x548; // Vector
        public const uint m_vMaxs = 0x554; // Vector
        public const uint m_vDistanceMins = 0x560; // Vector
        public const uint m_vDistanceMaxs = 0x56C; // Vector
        public const uint m_flWindMin = 0x578; // float
        public const uint m_flWindMax = 0x57C; // float
        public const uint m_flWindMapMin = 0x580; // float
        public const uint m_flWindMapMax = 0x584; // float
    }

    public static class CSoundOpvarSetPathCornerEntity  // CSoundOpvarSetPointEntity
    {
        public const uint m_flDistMinSqr = 0x660; // float
        public const uint m_flDistMaxSqr = 0x664; // float
        public const uint m_iszPathCornerEntityName = 0x668; // CUtlSymbolLarge
    }

    public static class CSoundOpvarSetPointBase  // CBaseEntity
    {
        public const uint m_bDisabled = 0x4B0; // bool
        public const uint m_hSource = 0x4B4; // CEntityHandle
        public const uint m_iszSourceEntityName = 0x4C0; // CUtlSymbolLarge
        public const uint m_vLastPosition = 0x518; // Vector
        public const uint m_iszStackName = 0x528; // CUtlSymbolLarge
        public const uint m_iszOperatorName = 0x530; // CUtlSymbolLarge
        public const uint m_iszOpvarName = 0x538; // CUtlSymbolLarge
        public const uint m_iOpvarIndex = 0x540; // int32_t
        public const uint m_bUseAutoCompare = 0x544; // bool
    }

    public static class CSoundOpvarSetPointEntity  // CSoundOpvarSetPointBase
    {
        public const uint m_OnEnter = 0x548; // CEntityIOOutput
        public const uint m_OnExit = 0x570; // CEntityIOOutput
        public const uint m_bAutoDisable = 0x598; // bool
        public const uint m_flDistanceMin = 0x5DC; // float
        public const uint m_flDistanceMax = 0x5E0; // float
        public const uint m_flDistanceMapMin = 0x5E4; // float
        public const uint m_flDistanceMapMax = 0x5E8; // float
        public const uint m_flOcclusionRadius = 0x5EC; // float
        public const uint m_flOcclusionMin = 0x5F0; // float
        public const uint m_flOcclusionMax = 0x5F4; // float
        public const uint m_flValSetOnDisable = 0x5F8; // float
        public const uint m_bSetValueOnDisable = 0x5FC; // bool
        public const uint m_nSimulationMode = 0x600; // int32_t
        public const uint m_nVisibilitySamples = 0x604; // int32_t
        public const uint m_vDynamicProxyPoint = 0x608; // Vector
        public const uint m_flDynamicMaximumOcclusion = 0x614; // float
        public const uint m_hDynamicEntity = 0x618; // CEntityHandle
        public const uint m_iszDynamicEntityName = 0x620; // CUtlSymbolLarge
        public const uint m_flPathingDistanceNormFactor = 0x628; // float
        public const uint m_vPathingSourcePos = 0x62C; // Vector
        public const uint m_vPathingListenerPos = 0x638; // Vector
        public const uint m_nPathingSourceIndex = 0x644; // int32_t
    }

    public static class CSoundPatch 
    {
        public const uint m_pitch = 0x8; // CSoundEnvelope
        public const uint m_volume = 0x18; // CSoundEnvelope
        public const uint m_shutdownTime = 0x30; // float
        public const uint m_flLastTime = 0x34; // float
        public const uint m_iszSoundScriptName = 0x38; // CUtlSymbolLarge
        public const uint m_hEnt = 0x40; // CHandle<CBaseEntity>
        public const uint m_soundEntityIndex = 0x44; // CEntityIndex
        public const uint m_soundOrigin = 0x48; // Vector
        public const uint m_isPlaying = 0x54; // int32_t
        public const uint m_Filter = 0x58; // CCopyRecipientFilter
        public const uint m_flCloseCaptionDuration = 0x80; // float
        public const uint m_bUpdatedSoundOrigin = 0x84; // bool
        public const uint m_iszClassName = 0x88; // CUtlSymbolLarge
    }

    public static class CSoundStackSave  // CLogicalEntity
    {
        public const uint m_iszStackName = 0x4B0; // CUtlSymbolLarge
    }

    public static class CSplineConstraint  // CPhysConstraint
    {
    }

    public static class CSpotlightEnd  // CBaseModelEntity
    {
        public const uint m_flLightScale = 0x700; // float
        public const uint m_Radius = 0x704; // float
        public const uint m_vSpotlightDir = 0x708; // Vector
        public const uint m_vSpotlightOrg = 0x714; // Vector
    }

    public static class CSprite  // CBaseModelEntity
    {
        public const uint m_hSpriteMaterial = 0x700; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_hAttachedToEntity = 0x708; // CHandle<CBaseEntity>
        public const uint m_nAttachment = 0x70C; // AttachmentHandle_t
        public const uint m_flSpriteFramerate = 0x710; // float
        public const uint m_flFrame = 0x714; // float
        public const uint m_flDieTime = 0x718; // GameTime_t
        public const uint m_nBrightness = 0x728; // uint32_t
        public const uint m_flBrightnessDuration = 0x72C; // float
        public const uint m_flSpriteScale = 0x730; // float
        public const uint m_flScaleDuration = 0x734; // float
        public const uint m_bWorldSpaceScale = 0x738; // bool
        public const uint m_flGlowProxySize = 0x73C; // float
        public const uint m_flHDRColorScale = 0x740; // float
        public const uint m_flLastTime = 0x744; // GameTime_t
        public const uint m_flMaxFrame = 0x748; // float
        public const uint m_flStartScale = 0x74C; // float
        public const uint m_flDestScale = 0x750; // float
        public const uint m_flScaleTimeStart = 0x754; // GameTime_t
        public const uint m_nStartBrightness = 0x758; // int32_t
        public const uint m_nDestBrightness = 0x75C; // int32_t
        public const uint m_flBrightnessTimeStart = 0x760; // GameTime_t
        public const uint m_nSpriteWidth = 0x764; // int32_t
        public const uint m_nSpriteHeight = 0x768; // int32_t
    }

    public static class CSpriteAlias_env_glow  // CSprite
    {
    }

    public static class CSpriteOriented  // CSprite
    {
    }

    public static class CStopwatch  // CStopwatchBase
    {
        public const uint m_interval = 0xC; // float
    }

    public static class CStopwatchBase  // CSimpleSimTimer
    {
        public const uint m_fIsRunning = 0x8; // bool
    }

    public static class CSun  // CBaseModelEntity
    {
        public const uint m_vDirection = 0x700; // Vector
        public const uint m_clrOverlay = 0x70C; // Color
        public const uint m_iszEffectName = 0x710; // CUtlSymbolLarge
        public const uint m_iszSSEffectName = 0x718; // CUtlSymbolLarge
        public const uint m_bOn = 0x720; // bool
        public const uint m_bmaxColor = 0x721; // bool
        public const uint m_flSize = 0x724; // float
        public const uint m_flRotation = 0x728; // float
        public const uint m_flHazeScale = 0x72C; // float
        public const uint m_flAlphaHaze = 0x730; // float
        public const uint m_flAlphaHdr = 0x734; // float
        public const uint m_flAlphaScale = 0x738; // float
        public const uint m_flHDRColorScale = 0x73C; // float
        public const uint m_flFarZScale = 0x740; // float
    }

    public static class CTablet  // CCSWeaponBase
    {
    }

    public static class CTakeDamageInfo 
    {
        public const uint m_vecDamageForce = 0x8; // Vector
        public const uint m_vecDamagePosition = 0x14; // Vector
        public const uint m_vecReportedPosition = 0x20; // Vector
        public const uint m_vecDamageDirection = 0x2C; // Vector
        public const uint m_hInflictor = 0x38; // CHandle<CBaseEntity>
        public const uint m_hAttacker = 0x3C; // CHandle<CBaseEntity>
        public const uint m_hAbility = 0x40; // CHandle<CBaseEntity>
        public const uint m_flDamage = 0x44; // float
        public const uint m_bitsDamageType = 0x48; // int32_t
        public const uint m_iDamageCustom = 0x4C; // int32_t
        public const uint m_iAmmoType = 0x50; // AmmoIndex_t
        public const uint m_flOriginalDamage = 0x60; // float
        public const uint m_bShouldBleed = 0x64; // bool
        public const uint m_bShouldSpark = 0x65; // bool
        public const uint m_nDamageFlags = 0x70; // TakeDamageFlags_t
        public const uint m_nNumObjectsPenetrated = 0x74; // int32_t
        public const uint m_hScriptInstance = 0x78; // HSCRIPT
        public const uint m_bInTakeDamageFlow = 0x94; // bool
    }

    public static class CTakeDamageResult 
    {
        public const uint m_nHealthLost = 0x0; // int32_t
        public const uint m_nDamageTaken = 0x4; // int32_t
    }

    public static class CTakeDamageSummaryScopeGuard 
    {
        public const uint m_vecSummaries = 0x8; // CUtlVector<SummaryTakeDamageInfo_t*>
    }

    public static class CTankTargetChange  // CPointEntity
    {
        public const uint m_newTarget = 0x4B0; // CVariantBase<CVariantDefaultAllocator>
        public const uint m_newTargetName = 0x4C0; // CUtlSymbolLarge
    }

    public static class CTankTrainAI  // CPointEntity
    {
        public const uint m_hTrain = 0x4B0; // CHandle<CFuncTrackTrain>
        public const uint m_hTargetEntity = 0x4B4; // CHandle<CBaseEntity>
        public const uint m_soundPlaying = 0x4B8; // int32_t
        public const uint m_startSoundName = 0x4D0; // CUtlSymbolLarge
        public const uint m_engineSoundName = 0x4D8; // CUtlSymbolLarge
        public const uint m_movementSoundName = 0x4E0; // CUtlSymbolLarge
        public const uint m_targetEntityName = 0x4E8; // CUtlSymbolLarge
    }

    public static class CTeam  // CBaseEntity
    {
        public const uint m_aPlayerControllers = 0x4B0; // CNetworkUtlVectorBase<CHandle<CBasePlayerController>>
        public const uint m_aPlayers = 0x4C8; // CNetworkUtlVectorBase<CHandle<CBasePlayerPawn>>
        public const uint m_iScore = 0x4E0; // int32_t
        public const uint m_szTeamname = 0x4E4; // char[129]
    }

    public static class CTeamplayRules  // CMultiplayRules
    {
    }

    public static class CTestEffect  // CBaseEntity
    {
        public const uint m_iLoop = 0x4B0; // int32_t
        public const uint m_iBeam = 0x4B4; // int32_t
        public const uint m_pBeam = 0x4B8; // CBeam*[24]
        public const uint m_flBeamTime = 0x578; // GameTime_t[24]
        public const uint m_flStartTime = 0x5D8; // GameTime_t
    }

    public static class CTextureBasedAnimatable  // CBaseModelEntity
    {
        public const uint m_bLoop = 0x700; // bool
        public const uint m_flFPS = 0x704; // float
        public const uint m_hPositionKeys = 0x708; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_hRotationKeys = 0x710; // CStrongHandle<InfoForResourceTypeCTextureBase>
        public const uint m_vAnimationBoundsMin = 0x718; // Vector
        public const uint m_vAnimationBoundsMax = 0x724; // Vector
        public const uint m_flStartTime = 0x730; // float
        public const uint m_flStartFrame = 0x734; // float
    }

    public static class CTimeline  // IntervalTimer
    {
        public const uint m_flValues = 0x10; // float[64]
        public const uint m_nValueCounts = 0x110; // int32_t[64]
        public const uint m_nBucketCount = 0x210; // int32_t
        public const uint m_flInterval = 0x214; // float
        public const uint m_flFinalValue = 0x218; // float
        public const uint m_nCompressionType = 0x21C; // TimelineCompression_t
        public const uint m_bStopped = 0x220; // bool
    }

    public static class CTimerEntity  // CLogicalEntity
    {
        public const uint m_OnTimer = 0x4B0; // CEntityIOOutput
        public const uint m_OnTimerHigh = 0x4D8; // CEntityIOOutput
        public const uint m_OnTimerLow = 0x500; // CEntityIOOutput
        public const uint m_iDisabled = 0x528; // int32_t
        public const uint m_flInitialDelay = 0x52C; // float
        public const uint m_flRefireTime = 0x530; // float
        public const uint m_bUpDownState = 0x534; // bool
        public const uint m_iUseRandomTime = 0x538; // int32_t
        public const uint m_bPauseAfterFiring = 0x53C; // bool
        public const uint m_flLowerRandomBound = 0x540; // float
        public const uint m_flUpperRandomBound = 0x544; // float
        public const uint m_flRemainingTime = 0x548; // float
        public const uint m_bPaused = 0x54C; // bool
    }

    public static class CTonemapController2  // CBaseEntity
    {
        public const uint m_flAutoExposureMin = 0x4B0; // float
        public const uint m_flAutoExposureMax = 0x4B4; // float
        public const uint m_flTonemapPercentTarget = 0x4B8; // float
        public const uint m_flTonemapPercentBrightPixels = 0x4BC; // float
        public const uint m_flTonemapMinAvgLum = 0x4C0; // float
        public const uint m_flExposureAdaptationSpeedUp = 0x4C4; // float
        public const uint m_flExposureAdaptationSpeedDown = 0x4C8; // float
        public const uint m_flTonemapEVSmoothingRange = 0x4CC; // float
    }

    public static class CTonemapController2Alias_env_tonemap_controller2  // CTonemapController2
    {
    }

    public static class CTonemapTrigger  // CBaseTrigger
    {
        public const uint m_tonemapControllerName = 0x8A8; // CUtlSymbolLarge
        public const uint m_hTonemapController = 0x8B0; // CEntityHandle
    }

    public static class CTouchExpansionComponent  // CEntityComponent
    {
    }

    public static class CTriggerActiveWeaponDetect  // CBaseTrigger
    {
        public const uint m_OnTouchedActiveWeapon = 0x8A8; // CEntityIOOutput
        public const uint m_iszWeaponClassName = 0x8D0; // CUtlSymbolLarge
    }

    public static class CTriggerBombReset  // CBaseTrigger
    {
    }

    public static class CTriggerBrush  // CBaseModelEntity
    {
        public const uint m_OnStartTouch = 0x700; // CEntityIOOutput
        public const uint m_OnEndTouch = 0x728; // CEntityIOOutput
        public const uint m_OnUse = 0x750; // CEntityIOOutput
        public const uint m_iInputFilter = 0x778; // int32_t
        public const uint m_iDontMessageParent = 0x77C; // int32_t
    }

    public static class CTriggerBuoyancy  // CBaseTrigger
    {
        public const uint m_BuoyancyHelper = 0x8A8; // CBuoyancyHelper
        public const uint m_flFluidDensity = 0x8C8; // float
    }

    public static class CTriggerCallback  // CBaseTrigger
    {
    }

    public static class CTriggerDetectBulletFire  // CBaseTrigger
    {
        public const uint m_bPlayerFireOnly = 0x8A8; // bool
        public const uint m_OnDetectedBulletFire = 0x8B0; // CEntityIOOutput
    }

    public static class CTriggerDetectExplosion  // CBaseTrigger
    {
        public const uint m_OnDetectedExplosion = 0x8E0; // CEntityIOOutput
    }

    public static class CTriggerFan  // CBaseTrigger
    {
        public const uint m_vFanOrigin = 0x8A8; // Vector
        public const uint m_vFanEnd = 0x8B4; // Vector
        public const uint m_vNoise = 0x8C0; // Vector
        public const uint m_flForce = 0x8CC; // float
        public const uint m_flPlayerForce = 0x8D0; // float
        public const uint m_flRampTime = 0x8D4; // float
        public const uint m_bFalloff = 0x8D8; // bool
        public const uint m_bPushPlayer = 0x8D9; // bool
        public const uint m_bRampDown = 0x8DA; // bool
        public const uint m_bAddNoise = 0x8DB; // bool
        public const uint m_RampTimer = 0x8E0; // CountdownTimer
    }

    public static class CTriggerGameEvent  // CBaseTrigger
    {
        public const uint m_strStartTouchEventName = 0x8A8; // CUtlString
        public const uint m_strEndTouchEventName = 0x8B0; // CUtlString
        public const uint m_strTriggerID = 0x8B8; // CUtlString
    }

    public static class CTriggerGravity  // CBaseTrigger
    {
    }

    public static class CTriggerHostageReset  // CBaseTrigger
    {
    }

    public static class CTriggerHurt  // CBaseTrigger
    {
        public const uint m_flOriginalDamage = 0x8A8; // float
        public const uint m_flDamage = 0x8AC; // float
        public const uint m_flDamageCap = 0x8B0; // float
        public const uint m_flLastDmgTime = 0x8B4; // GameTime_t
        public const uint m_flForgivenessDelay = 0x8B8; // float
        public const uint m_bitsDamageInflict = 0x8BC; // int32_t
        public const uint m_damageModel = 0x8C0; // int32_t
        public const uint m_bNoDmgForce = 0x8C4; // bool
        public const uint m_vDamageForce = 0x8C8; // Vector
        public const uint m_thinkAlways = 0x8D4; // bool
        public const uint m_hurtThinkPeriod = 0x8D8; // float
        public const uint m_OnHurt = 0x8E0; // CEntityIOOutput
        public const uint m_OnHurtPlayer = 0x908; // CEntityIOOutput
        public const uint m_hurtEntities = 0x930; // CUtlVector<CHandle<CBaseEntity>>
    }

    public static class CTriggerHurtGhost  // CTriggerHurt
    {
    }

    public static class CTriggerImpact  // CTriggerMultiple
    {
        public const uint m_flMagnitude = 0x8D0; // float
        public const uint m_flNoise = 0x8D4; // float
        public const uint m_flViewkick = 0x8D8; // float
        public const uint m_pOutputForce = 0x8E0; // CEntityOutputTemplate<Vector>
    }

    public static class CTriggerLerpObject  // CBaseTrigger
    {
        public const uint m_iszLerpTarget = 0x8A8; // CUtlSymbolLarge
        public const uint m_hLerpTarget = 0x8B0; // CHandle<CBaseEntity>
        public const uint m_iszLerpTargetAttachment = 0x8B8; // CUtlSymbolLarge
        public const uint m_hLerpTargetAttachment = 0x8C0; // AttachmentHandle_t
        public const uint m_flLerpDuration = 0x8C4; // float
        public const uint m_bLerpRestoreMoveType = 0x8C8; // bool
        public const uint m_bSingleLerpObject = 0x8C9; // bool
        public const uint m_vecLerpingObjects = 0x8D0; // CUtlVector<lerpdata_t>
        public const uint m_iszLerpEffect = 0x8E8; // CUtlSymbolLarge
        public const uint m_iszLerpSound = 0x8F0; // CUtlSymbolLarge
        public const uint m_OnLerpStarted = 0x8F8; // CEntityIOOutput
        public const uint m_OnLerpFinished = 0x920; // CEntityIOOutput
    }

    public static class CTriggerLook  // CTriggerOnce
    {
        public const uint m_hLookTarget = 0x8D0; // CHandle<CBaseEntity>
        public const uint m_flFieldOfView = 0x8D4; // float
        public const uint m_flLookTime = 0x8D8; // float
        public const uint m_flLookTimeTotal = 0x8DC; // float
        public const uint m_flLookTimeLast = 0x8E0; // GameTime_t
        public const uint m_flTimeoutDuration = 0x8E4; // float
        public const uint m_bTimeoutFired = 0x8E8; // bool
        public const uint m_bIsLooking = 0x8E9; // bool
        public const uint m_b2DFOV = 0x8EA; // bool
        public const uint m_bUseVelocity = 0x8EB; // bool
        public const uint m_hActivator = 0x8EC; // CHandle<CBaseEntity>
        public const uint m_bTestOcclusion = 0x8F0; // bool
        public const uint m_OnTimeout = 0x8F8; // CEntityIOOutput
        public const uint m_OnStartLook = 0x920; // CEntityIOOutput
        public const uint m_OnEndLook = 0x948; // CEntityIOOutput
    }

    public static class CTriggerMultiple  // CBaseTrigger
    {
        public const uint m_OnTrigger = 0x8A8; // CEntityIOOutput
    }

    public static class CTriggerOnce  // CTriggerMultiple
    {
    }

    public static class CTriggerPhysics  // CBaseTrigger
    {
        public const uint m_gravityScale = 0x8B8; // float
        public const uint m_linearLimit = 0x8BC; // float
        public const uint m_linearDamping = 0x8C0; // float
        public const uint m_angularLimit = 0x8C4; // float
        public const uint m_angularDamping = 0x8C8; // float
        public const uint m_linearForce = 0x8CC; // float
        public const uint m_flFrequency = 0x8D0; // float
        public const uint m_flDampingRatio = 0x8D4; // float
        public const uint m_vecLinearForcePointAt = 0x8D8; // Vector
        public const uint m_bCollapseToForcePoint = 0x8E4; // bool
        public const uint m_vecLinearForcePointAtWorld = 0x8E8; // Vector
        public const uint m_vecLinearForceDirection = 0x8F4; // Vector
        public const uint m_bConvertToDebrisWhenPossible = 0x900; // bool
    }

    public static class CTriggerProximity  // CBaseTrigger
    {
        public const uint m_hMeasureTarget = 0x8A8; // CHandle<CBaseEntity>
        public const uint m_iszMeasureTarget = 0x8B0; // CUtlSymbolLarge
        public const uint m_fRadius = 0x8B8; // float
        public const uint m_nTouchers = 0x8BC; // int32_t
        public const uint m_NearestEntityDistance = 0x8C0; // CEntityOutputTemplate<float>
    }

    public static class CTriggerPush  // CBaseTrigger
    {
        public const uint m_angPushEntitySpace = 0x8A8; // QAngle
        public const uint m_vecPushDirEntitySpace = 0x8B4; // Vector
        public const uint m_bTriggerOnStartTouch = 0x8C0; // bool
        public const uint m_flAlternateTicksFix = 0x8C4; // float
        public const uint m_flPushSpeed = 0x8C8; // float
    }

    public static class CTriggerRemove  // CBaseTrigger
    {
        public const uint m_OnRemove = 0x8A8; // CEntityIOOutput
    }

    public static class CTriggerSave  // CBaseTrigger
    {
        public const uint m_bForceNewLevelUnit = 0x8A8; // bool
        public const uint m_fDangerousTimer = 0x8AC; // float
        public const uint m_minHitPoints = 0x8B0; // int32_t
    }

    public static class CTriggerSndSosOpvar  // CBaseTrigger
    {
        public const uint m_hTouchingPlayers = 0x8A8; // CUtlVector<CHandle<CBaseEntity>>
        public const uint m_flPosition = 0x8C0; // Vector
        public const uint m_flCenterSize = 0x8CC; // float
        public const uint m_flMinVal = 0x8D0; // float
        public const uint m_flMaxVal = 0x8D4; // float
        public const uint m_flWait = 0x8D8; // float
        public const uint m_opvarName = 0x8E0; // CUtlSymbolLarge
        public const uint m_stackName = 0x8E8; // CUtlSymbolLarge
        public const uint m_operatorName = 0x8F0; // CUtlSymbolLarge
        public const uint m_bVolIs2D = 0x8F8; // bool
        public const uint m_opvarNameChar = 0x8F9; // char[256]
        public const uint m_stackNameChar = 0x9F9; // char[256]
        public const uint m_operatorNameChar = 0xAF9; // char[256]
        public const uint m_VecNormPos = 0xBFC; // Vector
        public const uint m_flNormCenterSize = 0xC08; // float
    }

    public static class CTriggerSoundscape  // CBaseTrigger
    {
        public const uint m_hSoundscape = 0x8A8; // CHandle<CEnvSoundscapeTriggerable>
        public const uint m_SoundscapeName = 0x8B0; // CUtlSymbolLarge
        public const uint m_spectators = 0x8B8; // CUtlVector<CHandle<CBasePlayerPawn>>
    }

    public static class CTriggerTeleport  // CBaseTrigger
    {
        public const uint m_iLandmark = 0x8A8; // CUtlSymbolLarge
        public const uint m_bUseLandmarkAngles = 0x8B0; // bool
        public const uint m_bMirrorPlayer = 0x8B1; // bool
    }

    public static class CTriggerToggleSave  // CBaseTrigger
    {
        public const uint m_bDisabled = 0x8A8; // bool
    }

    public static class CTriggerTripWire  // CBaseTrigger
    {
    }

    public static class CTriggerVolume  // CBaseModelEntity
    {
        public const uint m_iFilterName = 0x700; // CUtlSymbolLarge
        public const uint m_hFilter = 0x708; // CHandle<CBaseFilter>
    }

    public static class CTripWireFire  // CBaseCSGrenade
    {
    }

    public static class CTripWireFireProjectile  // CBaseGrenade
    {
    }

    public static class CVoteController  // CBaseEntity
    {
        public const uint m_iActiveIssueIndex = 0x4B0; // int32_t
        public const uint m_iOnlyTeamToVote = 0x4B4; // int32_t
        public const uint m_nVoteOptionCount = 0x4B8; // int32_t[5]
        public const uint m_nPotentialVotes = 0x4CC; // int32_t
        public const uint m_bIsYesNoVote = 0x4D0; // bool
        public const uint m_acceptingVotesTimer = 0x4D8; // CountdownTimer
        public const uint m_executeCommandTimer = 0x4F0; // CountdownTimer
        public const uint m_resetVoteTimer = 0x508; // CountdownTimer
        public const uint m_nVotesCast = 0x520; // int32_t[64]
        public const uint m_playerHoldingVote = 0x620; // CPlayerSlot
        public const uint m_playerOverrideForVote = 0x624; // CPlayerSlot
        public const uint m_nHighestCountIndex = 0x628; // int32_t
        public const uint m_potentialIssues = 0x630; // CUtlVector<CBaseIssue*>
        public const uint m_VoteOptions = 0x648; // CUtlVector<char*>
    }

    public static class CWaterBullet  // CBaseAnimGraph
    {
    }

    public static class CWeaponAWP  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponAug  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponBaseItem  // CCSWeaponBase
    {
        public const uint m_SequenceCompleteTimer = 0xE28; // CountdownTimer
        public const uint m_bRedraw = 0xE40; // bool
    }

    public static class CWeaponBizon  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponCZ75a  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponElite  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponFamas  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponFiveSeven  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponG3SG1  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponGalilAR  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponGlock  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponHKP2000  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponM249  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponM4A1  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponM4A1Silencer  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponMAC10  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponMP5SD  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponMP7  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponMP9  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponMag7  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponNOVA  // CCSWeaponBase
    {
    }

    public static class CWeaponNegev  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponP250  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponP90  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponRevolver  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponSCAR20  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponSG556  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponSSG08  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponSawedoff  // CCSWeaponBase
    {
    }

    public static class CWeaponShield  // CCSWeaponBaseGun
    {
        public const uint m_flBulletDamageAbsorbed = 0xE48; // float
        public const uint m_flLastBulletHitSoundTime = 0xE4C; // GameTime_t
        public const uint m_flDisplayHealth = 0xE50; // float
    }

    public static class CWeaponTaser  // CCSWeaponBaseGun
    {
        public const uint m_fFireTime = 0xE48; // GameTime_t
    }

    public static class CWeaponTec9  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponUMP45  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponUSPSilencer  // CCSWeaponBaseGun
    {
    }

    public static class CWeaponXM1014  // CCSWeaponBase
    {
    }

    public static class CWeaponZoneRepulsor  // CCSWeaponBaseGun
    {
    }

    public static class CWorld  // CBaseModelEntity
    {
    }

    public static class CommandToolCommand_t 
    {
        public const uint m_bEnabled = 0x0; // bool
        public const uint m_bOpened = 0x1; // bool
        public const uint m_InternalId = 0x4; // uint32_t
        public const uint m_ShortName = 0x8; // CUtlString
        public const uint m_ExecMode = 0x10; // CommandExecMode_t
        public const uint m_SpawnGroup = 0x18; // CUtlString
        public const uint m_PeriodicExecDelay = 0x20; // float
        public const uint m_SpecType = 0x24; // CommandEntitySpecType_t
        public const uint m_EntitySpec = 0x28; // CUtlString
        public const uint m_Commands = 0x30; // CUtlString
        public const uint m_SetDebugBits = 0x38; // DebugOverlayBits_t
        public const uint m_ClearDebugBits = 0x40; // DebugOverlayBits_t
    }

    public static class ConceptHistory_t 
    {
        public const uint timeSpoken = 0x0; // float
        public const uint m_response = 0x8; // CRR_Response
    }

    public static class ConstraintSoundInfo 
    {
        public const uint m_vSampler = 0x8; // VelocitySampler
        public const uint m_soundProfile = 0x20; // SimpleConstraintSoundProfile
        public const uint m_forwardAxis = 0x40; // Vector
        public const uint m_iszTravelSoundFwd = 0x50; // CUtlSymbolLarge
        public const uint m_iszTravelSoundBack = 0x58; // CUtlSymbolLarge
        public const uint m_iszReversalSounds = 0x68; // CUtlSymbolLarge[3]
        public const uint m_bPlayTravelSound = 0x80; // bool
        public const uint m_bPlayReversalSound = 0x81; // bool
    }

    public static class CountdownTimer 
    {
        public const uint m_duration = 0x8; // float
        public const uint m_timestamp = 0xC; // GameTime_t
        public const uint m_timescale = 0x10; // float
        public const uint m_nWorldGroupId = 0x14; // WorldGroupId_t
    }

    public static class EngineCountdownTimer 
    {
        public const uint m_duration = 0x8; // float
        public const uint m_timestamp = 0xC; // float
        public const uint m_timescale = 0x10; // float
    }

    public static class EntityRenderAttribute_t 
    {
        public const uint m_ID = 0x30; // CUtlStringToken
        public const uint m_Values = 0x34; // Vector4D
    }

    public static class EntitySpottedState_t 
    {
        public const uint m_bSpotted = 0x8; // bool
        public const uint m_bSpottedByMask = 0xC; // uint32_t[2]
    }

    public static class Extent 
    {
        public const uint lo = 0x0; // Vector
        public const uint hi = 0xC; // Vector
    }

    public static class FilterDamageType  // CBaseFilter
    {
        public const uint m_iDamageType = 0x508; // int32_t
    }

    public static class FilterHealth  // CBaseFilter
    {
        public const uint m_bAdrenalineActive = 0x508; // bool
        public const uint m_iHealthMin = 0x50C; // int32_t
        public const uint m_iHealthMax = 0x510; // int32_t
    }

    public static class FilterTeam  // CBaseFilter
    {
        public const uint m_iFilterTeam = 0x508; // int32_t
    }

    public static class GameAmmoTypeInfo_t  // AmmoTypeInfo_t
    {
        public const uint m_nBuySize = 0x38; // int32_t
        public const uint m_nCost = 0x3C; // int32_t
    }

    public static class GameTick_t 
    {
        public const uint m_Value = 0x0; // int32_t
    }

    public static class GameTime_t 
    {
        public const uint m_Value = 0x0; // float
    }

    public static class HullFlags_t 
    {
        public const uint m_bHull_Human = 0x0; // bool
        public const uint m_bHull_SmallCentered = 0x1; // bool
        public const uint m_bHull_WideHuman = 0x2; // bool
        public const uint m_bHull_Tiny = 0x3; // bool
        public const uint m_bHull_Medium = 0x4; // bool
        public const uint m_bHull_TinyCentered = 0x5; // bool
        public const uint m_bHull_Large = 0x6; // bool
        public const uint m_bHull_LargeCentered = 0x7; // bool
        public const uint m_bHull_MediumTall = 0x8; // bool
        public const uint m_bHull_Small = 0x9; // bool
    }

    public static class IChoreoServices 
    {
    }

    public static class IEconItemInterface 
    {
    }

    public static class IHasAttributes 
    {
    }

    public static class IRagdoll 
    {
    }

    public static class ISkeletonAnimationController 
    {
    }

    public static class IVehicle 
    {
    }

    public static class IntervalTimer 
    {
        public const uint m_timestamp = 0x8; // GameTime_t
        public const uint m_nWorldGroupId = 0xC; // WorldGroupId_t
    }

    public static class ModelConfigHandle_t 
    {
        public const uint m_Value = 0x0; // uint32_t
    }

    public static class ParticleIndex_t 
    {
        public const uint m_Value = 0x0; // int32_t
    }

    public static class PhysicsRagdollPose_t 
    {
        public const uint __m_pChainEntity = 0x8; // CNetworkVarChainer
        public const uint m_Transforms = 0x30; // CNetworkUtlVectorBase<CTransform>
        public const uint m_hOwner = 0x48; // CHandle<CBaseEntity>
    }

    public static class QuestProgress 
    {
    }

    public static class RagdollCreationParams_t 
    {
        public const uint m_vForce = 0x0; // Vector
        public const uint m_nForceBone = 0xC; // int32_t
    }

    public static class RelationshipOverride_t  // Relationship_t
    {
        public const uint entity = 0x8; // CHandle<CBaseEntity>
        public const uint classType = 0xC; // Class_T
    }

    public static class Relationship_t 
    {
        public const uint disposition = 0x0; // Disposition_t
        public const uint priority = 0x4; // int32_t
    }

    public static class ResponseContext_t 
    {
        public const uint m_iszName = 0x0; // CUtlSymbolLarge
        public const uint m_iszValue = 0x8; // CUtlSymbolLarge
        public const uint m_fExpirationTime = 0x10; // GameTime_t
    }

    public static class ResponseFollowup 
    {
        public const uint followup_concept = 0x0; // char*
        public const uint followup_contexts = 0x8; // char*
        public const uint followup_delay = 0x10; // float
        public const uint followup_target = 0x14; // char*
        public const uint followup_entityiotarget = 0x1C; // char*
        public const uint followup_entityioinput = 0x24; // char*
        public const uint followup_entityiodelay = 0x2C; // float
        public const uint bFired = 0x30; // bool
    }

    public static class ResponseParams 
    {
        public const uint odds = 0x10; // int16_t
        public const uint flags = 0x12; // int16_t
        public const uint m_pFollowup = 0x18; // ResponseFollowup*
    }

    public static class SellbackPurchaseEntry_t 
    {
        public const uint m_unDefIdx = 0x30; // uint16_t
        public const uint m_nCost = 0x34; // int32_t
        public const uint m_nPrevArmor = 0x38; // int32_t
        public const uint m_bPrevHelmet = 0x3C; // bool
        public const uint m_hItem = 0x40; // CEntityHandle
    }

    public static class ServerAuthoritativeWeaponSlot_t 
    {
        public const uint unClass = 0x28; // uint16_t
        public const uint unSlot = 0x2A; // uint16_t
        public const uint unItemDefIdx = 0x2C; // uint16_t
    }

    public static class SimpleConstraintSoundProfile 
    {
        public const uint eKeypoints = 0x8; // SimpleConstraintSoundProfile::SimpleConstraintsSoundProfileKeypoints_t
        public const uint m_keyPoints = 0xC; // float[2]
        public const uint m_reversalSoundThresholds = 0x14; // float[3]
    }

    public static class SpawnPoint  // CServerOnlyPointEntity
    {
        public const uint m_iPriority = 0x4B0; // int32_t
        public const uint m_bEnabled = 0x4B4; // bool
        public const uint m_nType = 0x4B8; // int32_t
    }

    public static class SpawnPointCoopEnemy  // SpawnPoint
    {
        public const uint m_szWeaponsToGive = 0x4C0; // CUtlSymbolLarge
        public const uint m_szPlayerModelToUse = 0x4C8; // CUtlSymbolLarge
        public const uint m_nArmorToSpawnWith = 0x4D0; // int32_t
        public const uint m_nDefaultBehavior = 0x4D4; // SpawnPointCoopEnemy::BotDefaultBehavior_t
        public const uint m_nBotDifficulty = 0x4D8; // int32_t
        public const uint m_bIsAgressive = 0x4DC; // bool
        public const uint m_bStartAsleep = 0x4DD; // bool
        public const uint m_flHideRadius = 0x4E0; // float
        public const uint m_szBehaviorTreeFile = 0x4F0; // CUtlSymbolLarge
    }

    public static class SummaryTakeDamageInfo_t 
    {
        public const uint nSummarisedCount = 0x0; // int32_t
        public const uint info = 0x8; // CTakeDamageInfo
        public const uint result = 0xA0; // CTakeDamageResult
        public const uint hTarget = 0xA8; // CHandle<CBaseEntity>
    }

    public static class VPhysicsCollisionAttribute_t 
    {
        public const uint m_nInteractsAs = 0x8; // uint64_t
        public const uint m_nInteractsWith = 0x10; // uint64_t
        public const uint m_nInteractsExclude = 0x18; // uint64_t
        public const uint m_nEntityId = 0x20; // uint32_t
        public const uint m_nOwnerId = 0x24; // uint32_t
        public const uint m_nHierarchyId = 0x28; // uint16_t
        public const uint m_nCollisionGroup = 0x2A; // uint8_t
        public const uint m_nCollisionFunctionMask = 0x2B; // uint8_t
    }

    public static class VelocitySampler 
    {
        public const uint m_prevSample = 0x0; // Vector
        public const uint m_fPrevSampleTime = 0xC; // GameTime_t
        public const uint m_fIdealSampleRate = 0x10; // float
    }

    public static class ViewAngleServerChange_t 
    {
        public const uint nType = 0x30; // FixAngleSet_t
        public const uint qAngle = 0x34; // QAngle
        public const uint nIndex = 0x40; // uint32_t
    }

    public static class WeaponPurchaseCount_t 
    {
        public const uint m_nItemDefIndex = 0x30; // uint16_t
        public const uint m_nCount = 0x32; // uint16_t
    }

    public static class WeaponPurchaseTracker_t 
    {
        public const uint m_weaponPurchases = 0x8; // CUtlVectorEmbeddedNetworkVar<WeaponPurchaseCount_t>
    }

    public static class audioparams_t 
    {
        public const uint localSound = 0x8; // Vector[8]
        public const uint soundscapeIndex = 0x68; // int32_t
        public const uint localBits = 0x6C; // uint8_t
        public const uint soundscapeEntityListIndex = 0x70; // int32_t
        public const uint soundEventHash = 0x74; // uint32_t
    }

    public static class dynpitchvol_base_t 
    {
        public const uint preset = 0x0; // int32_t
        public const uint pitchrun = 0x4; // int32_t
        public const uint pitchstart = 0x8; // int32_t
        public const uint spinup = 0xC; // int32_t
        public const uint spindown = 0x10; // int32_t
        public const uint volrun = 0x14; // int32_t
        public const uint volstart = 0x18; // int32_t
        public const uint fadein = 0x1C; // int32_t
        public const uint fadeout = 0x20; // int32_t
        public const uint lfotype = 0x24; // int32_t
        public const uint lforate = 0x28; // int32_t
        public const uint lfomodpitch = 0x2C; // int32_t
        public const uint lfomodvol = 0x30; // int32_t
        public const uint cspinup = 0x34; // int32_t
        public const uint cspincount = 0x38; // int32_t
        public const uint pitch = 0x3C; // int32_t
        public const uint spinupsav = 0x40; // int32_t
        public const uint spindownsav = 0x44; // int32_t
        public const uint pitchfrac = 0x48; // int32_t
        public const uint vol = 0x4C; // int32_t
        public const uint fadeinsav = 0x50; // int32_t
        public const uint fadeoutsav = 0x54; // int32_t
        public const uint volfrac = 0x58; // int32_t
        public const uint lfofrac = 0x5C; // int32_t
        public const uint lfomult = 0x60; // int32_t
    }

    public static class dynpitchvol_t  // dynpitchvol_base_t
    {
    }

    public static class fogparams_t 
    {
        public const uint dirPrimary = 0x8; // Vector
        public const uint colorPrimary = 0x14; // Color
        public const uint colorSecondary = 0x18; // Color
        public const uint colorPrimaryLerpTo = 0x1C; // Color
        public const uint colorSecondaryLerpTo = 0x20; // Color
        public const uint start = 0x24; // float
        public const uint end = 0x28; // float
        public const uint farz = 0x2C; // float
        public const uint maxdensity = 0x30; // float
        public const uint exponent = 0x34; // float
        public const uint HDRColorScale = 0x38; // float
        public const uint skyboxFogFactor = 0x3C; // float
        public const uint skyboxFogFactorLerpTo = 0x40; // float
        public const uint startLerpTo = 0x44; // float
        public const uint endLerpTo = 0x48; // float
        public const uint maxdensityLerpTo = 0x4C; // float
        public const uint lerptime = 0x50; // GameTime_t
        public const uint duration = 0x54; // float
        public const uint blendtobackground = 0x58; // float
        public const uint scattering = 0x5C; // float
        public const uint locallightscale = 0x60; // float
        public const uint enable = 0x64; // bool
        public const uint blend = 0x65; // bool
        public const uint m_bNoReflectionFog = 0x66; // bool
        public const uint m_bPadding = 0x67; // bool
    }

    public static class fogplayerparams_t 
    {
        public const uint m_hCtrl = 0x8; // CHandle<CFogController>
        public const uint m_flTransitionTime = 0xC; // float
        public const uint m_OldColor = 0x10; // Color
        public const uint m_flOldStart = 0x14; // float
        public const uint m_flOldEnd = 0x18; // float
        public const uint m_flOldMaxDensity = 0x1C; // float
        public const uint m_flOldHDRColorScale = 0x20; // float
        public const uint m_flOldFarZ = 0x24; // float
        public const uint m_NewColor = 0x28; // Color
        public const uint m_flNewStart = 0x2C; // float
        public const uint m_flNewEnd = 0x30; // float
        public const uint m_flNewMaxDensity = 0x34; // float
        public const uint m_flNewHDRColorScale = 0x38; // float
        public const uint m_flNewFarZ = 0x3C; // float
    }

    public static class hudtextparms_t 
    {
        public const uint color1 = 0x0; // Color
        public const uint color2 = 0x4; // Color
        public const uint effect = 0x8; // uint8_t
        public const uint channel = 0x9; // uint8_t
        public const uint x = 0xC; // float
        public const uint y = 0x10; // float
    }

    public static class lerpdata_t 
    {
        public const uint m_hEnt = 0x0; // CHandle<CBaseEntity>
        public const uint m_MoveType = 0x4; // MoveType_t
        public const uint m_flStartTime = 0x8; // GameTime_t
        public const uint m_vecStartOrigin = 0xC; // Vector
        public const uint m_qStartRot = 0x20; // Quaternion
        public const uint m_nFXIndex = 0x30; // ParticleIndex_t
    }

    public static class locksound_t 
    {
        public const uint sLockedSound = 0x8; // CUtlSymbolLarge
        public const uint sUnlockedSound = 0x10; // CUtlSymbolLarge
        public const uint flwaitSound = 0x18; // GameTime_t
    }

    public static class magnetted_objects_t 
    {
        public const uint hEntity = 0x8; // CHandle<CBaseEntity>
    }

    public static class ragdoll_t 
    {
        public const uint list = 0x0; // CUtlVector<ragdollelement_t>
        public const uint boneIndex = 0x18; // CUtlVector<int32_t>
        public const uint allowStretch = 0x30; // bool
        public const uint unused = 0x31; // bool
    }

    public static class ragdollelement_t 
    {
        public const uint originParentSpace = 0x0; // Vector
        public const uint parentIndex = 0x20; // int32_t
        public const uint m_flRadius = 0x24; // float
    }

    public static class shard_model_desc_t 
    {
        public const uint m_nModelID = 0x8; // int32_t
        public const uint m_hMaterial = 0x10; // CStrongHandle<InfoForResourceTypeIMaterial2>
        public const uint m_solid = 0x18; // ShardSolid_t
        public const uint m_ShatterPanelMode = 0x19; // ShatterPanelMode
        public const uint m_vecPanelSize = 0x1C; // Vector2D
        public const uint m_vecStressPositionA = 0x24; // Vector2D
        public const uint m_vecStressPositionB = 0x2C; // Vector2D
        public const uint m_vecPanelVertices = 0x38; // CNetworkUtlVectorBase<Vector2D>
        public const uint m_flGlassHalfThickness = 0x50; // float
        public const uint m_bHasParent = 0x54; // bool
        public const uint m_bParentFrozen = 0x55; // bool
        public const uint m_SurfacePropStringToken = 0x58; // CUtlStringToken
    }

    public static class sky3dparams_t 
    {
        public const uint scale = 0x8; // int16_t
        public const uint origin = 0xC; // Vector
        public const uint bClip3DSkyBoxNearToWorldFar = 0x18; // bool
        public const uint flClip3DSkyBoxNearToWorldFarOffset = 0x1C; // float
        public const uint fog = 0x20; // fogparams_t
        public const uint m_nWorldGroupID = 0x88; // WorldGroupId_t
    }

    public static class thinkfunc_t 
    {
        public const uint m_hFn = 0x8; // HSCRIPT
        public const uint m_nContext = 0x10; // CUtlStringToken
        public const uint m_nNextThinkTick = 0x14; // GameTick_t
        public const uint m_nLastThinkTick = 0x18; // GameTick_t
    }
}
