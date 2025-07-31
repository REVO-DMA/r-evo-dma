namespace cs2_dma_esp.SDK.Soundsystem 
{
    public static class CDSPMixgroupModifier 
    {
        public const uint m_mixgroup = 0x0; // CUtlString
        public const uint m_flModifier = 0x8; // float
        public const uint m_flModifierMin = 0xC; // float
        public const uint m_flSourceModifier = 0x10; // float
        public const uint m_flSourceModifierMin = 0x14; // float
        public const uint m_flListenerReverbModifierWhenSourceReverbIsActive = 0x18; // float
    }

    public static class CDSPPresetMixgroupModifierTable 
    {
        public const uint m_table = 0x0; // CUtlVector<CDspPresetModifierList>
    }

    public static class CDspPresetModifierList 
    {
        public const uint m_dspName = 0x0; // CUtlString
        public const uint m_modifiers = 0x8; // CUtlVector<CDSPMixgroupModifier>
    }

    public static class CSosGroupActionLimitSchema  // CSosGroupActionSchema
    {
        public const uint m_nMaxCount = 0x18; // int32_t
        public const uint m_nStopType = 0x1C; // SosActionStopType_t
        public const uint m_nSortType = 0x20; // SosActionSortType_t
    }

    public static class CSosGroupActionSchema 
    {
        public const uint m_name = 0x8; // CUtlString
        public const uint m_actionType = 0x10; // ActionType_t
        public const uint m_actionInstanceType = 0x14; // ActionType_t
    }

    public static class CSosGroupActionSetSoundeventParameterSchema  // CSosGroupActionSchema
    {
        public const uint m_nMaxCount = 0x18; // int32_t
        public const uint m_flMinValue = 0x1C; // float
        public const uint m_flMaxValue = 0x20; // float
        public const uint m_opvarName = 0x28; // CUtlString
        public const uint m_nSortType = 0x30; // SosActionSortType_t
    }

    public static class CSosGroupActionTimeLimitSchema  // CSosGroupActionSchema
    {
        public const uint m_flMaxDuration = 0x18; // float
    }

    public static class CSosGroupBranchPattern 
    {
        public const uint m_bMatchEventName = 0x8; // bool
        public const uint m_bMatchEventSubString = 0x9; // bool
        public const uint m_bMatchEntIndex = 0xA; // bool
        public const uint m_bMatchOpvar = 0xB; // bool
    }

    public static class CSosGroupMatchPattern  // CSosGroupBranchPattern
    {
        public const uint m_matchSoundEventName = 0x10; // CUtlString
        public const uint m_matchSoundEventSubString = 0x18; // CUtlString
        public const uint m_flEntIndex = 0x20; // float
        public const uint m_flOpvar = 0x24; // float
    }

    public static class CSosSoundEventGroupListSchema 
    {
        public const uint m_groupList = 0x0; // CUtlVector<CSosSoundEventGroupSchema>
    }

    public static class CSosSoundEventGroupSchema 
    {
        public const uint m_name = 0x0; // CUtlString
        public const uint m_nType = 0x8; // SosGroupType_t
        public const uint m_bIsBlocking = 0xC; // bool
        public const uint m_nBlockMaxCount = 0x10; // int32_t
        public const uint m_bInvertMatch = 0x14; // bool
        public const uint m_matchPattern = 0x18; // CSosGroupMatchPattern
        public const uint m_branchPattern = 0x40; // CSosGroupBranchPattern
        public const uint m_vActions = 0xB0; // CSosGroupActionSchema*[4]
    }

    public static class CSoundEventMetaData 
    {
        public const uint m_soundEventVMix = 0x0; // CStrongHandle<InfoForResourceTypeCVMixListResource>
    }

    public static class SelectedEditItemInfo_t 
    {
        public const uint m_EditItems = 0x0; // CUtlVector<SosEditItemInfo_t>
    }

    public static class SosEditItemInfo_t 
    {
        public const uint itemType = 0x0; // SosEditItemType_t
        public const uint itemName = 0x8; // CUtlString
        public const uint itemTypeName = 0x10; // CUtlString
        public const uint itemKVString = 0x20; // CUtlString
        public const uint itemPos = 0x28; // Vector2D
    }

    public static class VMixAutoFilterDesc_t 
    {
        public const uint m_flEnvelopeAmount = 0x0; // float
        public const uint m_flAttackTimeMS = 0x4; // float
        public const uint m_flReleaseTimeMS = 0x8; // float
        public const uint m_filter = 0xC; // VMixFilterDesc_t
        public const uint m_flLFOAmount = 0x1C; // float
        public const uint m_flLFORate = 0x20; // float
        public const uint m_flPhase = 0x24; // float
        public const uint m_nLFOShape = 0x28; // VMixLFOShape_t
    }

    public static class VMixBoxverbDesc_t 
    {
        public const uint m_flSizeMax = 0x0; // float
        public const uint m_flSizeMin = 0x4; // float
        public const uint m_flComplexity = 0x8; // float
        public const uint m_flDiffusion = 0xC; // float
        public const uint m_flModDepth = 0x10; // float
        public const uint m_flModRate = 0x14; // float
        public const uint m_bParallel = 0x18; // bool
        public const uint m_filterType = 0x1C; // VMixFilterDesc_t
        public const uint m_flWidth = 0x2C; // float
        public const uint m_flHeight = 0x30; // float
        public const uint m_flDepth = 0x34; // float
        public const uint m_flFeedbackScale = 0x38; // float
        public const uint m_flFeedbackWidth = 0x3C; // float
        public const uint m_flFeedbackHeight = 0x40; // float
        public const uint m_flFeedbackDepth = 0x44; // float
        public const uint m_flOutputGain = 0x48; // float
        public const uint m_flTaps = 0x4C; // float
    }

    public static class VMixConvolutionDesc_t 
    {
        public const uint m_fldbGain = 0x0; // float
        public const uint m_flPreDelayMS = 0x4; // float
        public const uint m_flWetMix = 0x8; // float
        public const uint m_fldbLow = 0xC; // float
        public const uint m_fldbMid = 0x10; // float
        public const uint m_fldbHigh = 0x14; // float
        public const uint m_flLowCutoffFreq = 0x18; // float
        public const uint m_flHighCutoffFreq = 0x1C; // float
    }

    public static class VMixDelayDesc_t 
    {
        public const uint m_feedbackFilter = 0x0; // VMixFilterDesc_t
        public const uint m_bEnableFilter = 0x10; // bool
        public const uint m_flDelay = 0x14; // float
        public const uint m_flDirectGain = 0x18; // float
        public const uint m_flDelayGain = 0x1C; // float
        public const uint m_flFeedbackGain = 0x20; // float
        public const uint m_flWidth = 0x24; // float
    }

    public static class VMixDiffusorDesc_t 
    {
        public const uint m_flSize = 0x0; // float
        public const uint m_flComplexity = 0x4; // float
        public const uint m_flFeedback = 0x8; // float
        public const uint m_flOutputGain = 0xC; // float
    }

    public static class VMixDynamics3BandDesc_t 
    {
        public const uint m_fldbGainOutput = 0x0; // float
        public const uint m_flRMSTimeMS = 0x4; // float
        public const uint m_fldbKneeWidth = 0x8; // float
        public const uint m_flDepth = 0xC; // float
        public const uint m_flWetMix = 0x10; // float
        public const uint m_flTimeScale = 0x14; // float
        public const uint m_flLowCutoffFreq = 0x18; // float
        public const uint m_flHighCutoffFreq = 0x1C; // float
        public const uint m_bPeakMode = 0x20; // bool
        public const uint m_bandDesc = 0x24; // VMixDynamicsBand_t[3]
    }

    public static class VMixDynamicsBand_t 
    {
        public const uint m_fldbGainInput = 0x0; // float
        public const uint m_fldbGainOutput = 0x4; // float
        public const uint m_fldbThresholdBelow = 0x8; // float
        public const uint m_fldbThresholdAbove = 0xC; // float
        public const uint m_flRatioBelow = 0x10; // float
        public const uint m_flRatioAbove = 0x14; // float
        public const uint m_flAttackTimeMS = 0x18; // float
        public const uint m_flReleaseTimeMS = 0x1C; // float
        public const uint m_bEnable = 0x20; // bool
        public const uint m_bSolo = 0x21; // bool
    }

    public static class VMixDynamicsCompressorDesc_t 
    {
        public const uint m_fldbOutputGain = 0x0; // float
        public const uint m_fldbCompressionThreshold = 0x4; // float
        public const uint m_fldbKneeWidth = 0x8; // float
        public const uint m_flCompressionRatio = 0xC; // float
        public const uint m_flAttackTimeMS = 0x10; // float
        public const uint m_flReleaseTimeMS = 0x14; // float
        public const uint m_flRMSTimeMS = 0x18; // float
        public const uint m_flWetMix = 0x1C; // float
        public const uint m_bPeakMode = 0x20; // bool
    }

    public static class VMixDynamicsDesc_t 
    {
        public const uint m_fldbGain = 0x0; // float
        public const uint m_fldbNoiseGateThreshold = 0x4; // float
        public const uint m_fldbCompressionThreshold = 0x8; // float
        public const uint m_fldbLimiterThreshold = 0xC; // float
        public const uint m_fldbKneeWidth = 0x10; // float
        public const uint m_flRatio = 0x14; // float
        public const uint m_flLimiterRatio = 0x18; // float
        public const uint m_flAttackTimeMS = 0x1C; // float
        public const uint m_flReleaseTimeMS = 0x20; // float
        public const uint m_flRMSTimeMS = 0x24; // float
        public const uint m_flWetMix = 0x28; // float
        public const uint m_bPeakMode = 0x2C; // bool
    }

    public static class VMixEQ8Desc_t 
    {
        public const uint m_stages = 0x0; // VMixFilterDesc_t[8]
    }

    public static class VMixEffectChainDesc_t 
    {
        public const uint m_flCrossfadeTime = 0x0; // float
    }

    public static class VMixEnvelopeDesc_t 
    {
        public const uint m_flAttackTimeMS = 0x0; // float
        public const uint m_flHoldTimeMS = 0x4; // float
        public const uint m_flReleaseTimeMS = 0x8; // float
    }

    public static class VMixFilterDesc_t 
    {
        public const uint m_nFilterType = 0x0; // VMixFilterType_t
        public const uint m_nFilterSlope = 0x2; // VMixFilterSlope_t
        public const uint m_bEnabled = 0x3; // bool
        public const uint m_fldbGain = 0x4; // float
        public const uint m_flCutoffFreq = 0x8; // float
        public const uint m_flQ = 0xC; // float
    }

    public static class VMixFreeverbDesc_t 
    {
        public const uint m_flRoomSize = 0x0; // float
        public const uint m_flDamp = 0x4; // float
        public const uint m_flWidth = 0x8; // float
        public const uint m_flLateReflections = 0xC; // float
    }

    public static class VMixModDelayDesc_t 
    {
        public const uint m_feedbackFilter = 0x0; // VMixFilterDesc_t
        public const uint m_bPhaseInvert = 0x10; // bool
        public const uint m_flGlideTime = 0x14; // float
        public const uint m_flDelay = 0x18; // float
        public const uint m_flOutputGain = 0x1C; // float
        public const uint m_flFeedbackGain = 0x20; // float
        public const uint m_flModRate = 0x24; // float
        public const uint m_flModDepth = 0x28; // float
        public const uint m_bApplyAntialiasing = 0x2C; // bool
    }

    public static class VMixOscDesc_t 
    {
        public const uint oscType = 0x0; // VMixLFOShape_t
        public const uint m_freq = 0x4; // float
        public const uint m_flPhase = 0x8; // float
    }

    public static class VMixPannerDesc_t 
    {
        public const uint m_type = 0x0; // VMixPannerType_t
        public const uint m_flStrength = 0x4; // float
    }

    public static class VMixPitchShiftDesc_t 
    {
        public const uint m_nGrainSampleCount = 0x0; // int32_t
        public const uint m_flPitchShift = 0x4; // float
        public const uint m_nQuality = 0x8; // int32_t
        public const uint m_nProcType = 0xC; // int32_t
    }

    public static class VMixPlateverbDesc_t 
    {
        public const uint m_flPrefilter = 0x0; // float
        public const uint m_flInputDiffusion1 = 0x4; // float
        public const uint m_flInputDiffusion2 = 0x8; // float
        public const uint m_flDecay = 0xC; // float
        public const uint m_flDamp = 0x10; // float
        public const uint m_flFeedbackDiffusion1 = 0x14; // float
        public const uint m_flFeedbackDiffusion2 = 0x18; // float
    }

    public static class VMixShaperDesc_t 
    {
        public const uint m_nShape = 0x0; // int32_t
        public const uint m_fldbDrive = 0x4; // float
        public const uint m_fldbOutputGain = 0x8; // float
        public const uint m_flWetMix = 0xC; // float
        public const uint m_nOversampleFactor = 0x10; // int32_t
    }

    public static class VMixSubgraphSwitchDesc_t 
    {
        public const uint m_interpolationMode = 0x0; // VMixSubgraphSwitchInterpolationType_t
        public const uint m_bOnlyTailsOnFadeOut = 0x4; // bool
        public const uint m_flInterpolationTime = 0x8; // float
    }

    public static class VMixUtilityDesc_t 
    {
        public const uint m_nOp = 0x0; // VMixChannelOperation_t
        public const uint m_flInputPan = 0x4; // float
        public const uint m_flOutputBalance = 0x8; // float
        public const uint m_fldbOutputGain = 0xC; // float
        public const uint m_bBassMono = 0x10; // bool
        public const uint m_flBassFreq = 0x14; // float
    }

    public static class VMixVocoderDesc_t 
    {
        public const uint m_nBandCount = 0x0; // int32_t
        public const uint m_flBandwidth = 0x4; // float
        public const uint m_fldBModGain = 0x8; // float
        public const uint m_flFreqRangeStart = 0xC; // float
        public const uint m_flFreqRangeEnd = 0x10; // float
        public const uint m_fldBUnvoicedGain = 0x14; // float
        public const uint m_flAttackTimeMS = 0x18; // float
        public const uint m_flReleaseTimeMS = 0x1C; // float
        public const uint m_nDebugBand = 0x20; // int32_t
        public const uint m_bPeakMode = 0x24; // bool
    }
}
