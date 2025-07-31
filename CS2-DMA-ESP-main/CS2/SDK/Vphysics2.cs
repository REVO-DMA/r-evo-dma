namespace cs2_dma_esp.SDK.Vphysics2 
{
    public static class CFeIndexedJiggleBone 
    {
        public const uint m_nNode = 0x0; // uint32_t
        public const uint m_nJiggleParent = 0x4; // uint32_t
        public const uint m_jiggleBone = 0x8; // CFeJiggleBone
    }

    public static class CFeJiggleBone 
    {
        public const uint m_nFlags = 0x0; // uint32_t
        public const uint m_flLength = 0x4; // float
        public const uint m_flTipMass = 0x8; // float
        public const uint m_flYawStiffness = 0xC; // float
        public const uint m_flYawDamping = 0x10; // float
        public const uint m_flPitchStiffness = 0x14; // float
        public const uint m_flPitchDamping = 0x18; // float
        public const uint m_flAlongStiffness = 0x1C; // float
        public const uint m_flAlongDamping = 0x20; // float
        public const uint m_flAngleLimit = 0x24; // float
        public const uint m_flMinYaw = 0x28; // float
        public const uint m_flMaxYaw = 0x2C; // float
        public const uint m_flYawFriction = 0x30; // float
        public const uint m_flYawBounce = 0x34; // float
        public const uint m_flMinPitch = 0x38; // float
        public const uint m_flMaxPitch = 0x3C; // float
        public const uint m_flPitchFriction = 0x40; // float
        public const uint m_flPitchBounce = 0x44; // float
        public const uint m_flBaseMass = 0x48; // float
        public const uint m_flBaseStiffness = 0x4C; // float
        public const uint m_flBaseDamping = 0x50; // float
        public const uint m_flBaseMinLeft = 0x54; // float
        public const uint m_flBaseMaxLeft = 0x58; // float
        public const uint m_flBaseLeftFriction = 0x5C; // float
        public const uint m_flBaseMinUp = 0x60; // float
        public const uint m_flBaseMaxUp = 0x64; // float
        public const uint m_flBaseUpFriction = 0x68; // float
        public const uint m_flBaseMinForward = 0x6C; // float
        public const uint m_flBaseMaxForward = 0x70; // float
        public const uint m_flBaseForwardFriction = 0x74; // float
        public const uint m_flRadius0 = 0x78; // float
        public const uint m_flRadius1 = 0x7C; // float
        public const uint m_vPoint0 = 0x80; // Vector
        public const uint m_vPoint1 = 0x8C; // Vector
        public const uint m_nCollisionMask = 0x98; // uint16_t
    }

    public static class CFeMorphLayer 
    {
        public const uint m_Name = 0x0; // CUtlString
        public const uint m_nNameHash = 0x8; // uint32_t
        public const uint m_Nodes = 0x10; // CUtlVector<uint16_t>
        public const uint m_InitPos = 0x28; // CUtlVector<Vector>
        public const uint m_Gravity = 0x40; // CUtlVector<float>
        public const uint m_GoalStrength = 0x58; // CUtlVector<float>
        public const uint m_GoalDamping = 0x70; // CUtlVector<float>
    }

    public static class CFeNamedJiggleBone 
    {
        public const uint m_strParentBone = 0x0; // CUtlString
        public const uint m_transform = 0x10; // CTransform
        public const uint m_nJiggleParent = 0x30; // uint32_t
        public const uint m_jiggleBone = 0x34; // CFeJiggleBone
    }

    public static class CFeVertexMapBuildArray 
    {
        public const uint m_Array = 0x0; // CUtlVector<FeVertexMapBuild_t*>
    }

    public static class CRegionSVM 
    {
        public const uint m_Planes = 0x0; // CUtlVector<RnPlane_t>
        public const uint m_Nodes = 0x18; // CUtlVector<uint32_t>
    }

    public static class CastSphereSATParams_t 
    {
        public const uint m_vRayStart = 0x0; // Vector
        public const uint m_vRayDelta = 0xC; // Vector
        public const uint m_flRadius = 0x18; // float
        public const uint m_flMaxFraction = 0x1C; // float
        public const uint m_flScale = 0x20; // float
        public const uint m_pHull = 0x28; // RnHull_t*
    }

    public static class CovMatrix3 
    {
        public const uint m_vDiag = 0x0; // Vector
        public const uint m_flXY = 0xC; // float
        public const uint m_flXZ = 0x10; // float
        public const uint m_flYZ = 0x14; // float
    }

    public static class Dop26_t 
    {
        public const uint m_flSupport = 0x0; // float[26]
    }

    public static class FeAnimStrayRadius_t 
    {
        public const uint nNode = 0x0; // uint16_t[2]
        public const uint flMaxDist = 0x4; // float
        public const uint flRelaxationFactor = 0x8; // float
    }

    public static class FeAxialEdgeBend_t 
    {
        public const uint te = 0x0; // float
        public const uint tv = 0x4; // float
        public const uint flDist = 0x8; // float
        public const uint flWeight = 0xC; // float[4]
        public const uint nNode = 0x1C; // uint16_t[6]
    }

    public static class FeBandBendLimit_t 
    {
        public const uint flDistMin = 0x0; // float
        public const uint flDistMax = 0x4; // float
        public const uint nNode = 0x8; // uint16_t[6]
    }

    public static class FeBoxRigid_t 
    {
        public const uint tmFrame2 = 0x0; // CTransform
        public const uint nNode = 0x20; // uint16_t
        public const uint nCollisionMask = 0x22; // uint16_t
        public const uint vSize = 0x24; // Vector
        public const uint nVertexMapIndex = 0x30; // uint16_t
        public const uint nFlags = 0x32; // uint16_t
    }

    public static class FeBuildBoxRigid_t  // FeBoxRigid_t
    {
        public const uint m_nPriority = 0x40; // int32_t
        public const uint m_nVertexMapHash = 0x44; // uint32_t
    }

    public static class FeBuildSphereRigid_t  // FeSphereRigid_t
    {
        public const uint m_nPriority = 0x20; // int32_t
        public const uint m_nVertexMapHash = 0x24; // uint32_t
    }

    public static class FeBuildTaperedCapsuleRigid_t  // FeTaperedCapsuleRigid_t
    {
        public const uint m_nPriority = 0x30; // int32_t
        public const uint m_nVertexMapHash = 0x34; // uint32_t
    }

    public static class FeCollisionPlane_t 
    {
        public const uint nCtrlParent = 0x0; // uint16_t
        public const uint nChildNode = 0x2; // uint16_t
        public const uint m_Plane = 0x4; // RnPlane_t
        public const uint flStrength = 0x14; // float
    }

    public static class FeCtrlOffset_t 
    {
        public const uint vOffset = 0x0; // Vector
        public const uint nCtrlParent = 0xC; // uint16_t
        public const uint nCtrlChild = 0xE; // uint16_t
    }

    public static class FeCtrlOsOffset_t 
    {
        public const uint nCtrlParent = 0x0; // uint16_t
        public const uint nCtrlChild = 0x2; // uint16_t
    }

    public static class FeCtrlSoftOffset_t 
    {
        public const uint nCtrlParent = 0x0; // uint16_t
        public const uint nCtrlChild = 0x2; // uint16_t
        public const uint vOffset = 0x4; // Vector
        public const uint flAlpha = 0x10; // float
    }

    public static class FeEdgeDesc_t 
    {
        public const uint nEdge = 0x0; // uint16_t[2]
        public const uint nSide = 0x4; // uint16_t[2][2]
        public const uint nVirtElem = 0xC; // uint16_t[2]
    }

    public static class FeEffectDesc_t 
    {
        public const uint sName = 0x0; // CUtlString
        public const uint nNameHash = 0x8; // uint32_t
        public const uint nType = 0xC; // int32_t
        public const uint m_Params = 0x10; // KeyValues3
    }

    public static class FeFitInfluence_t 
    {
        public const uint nVertexNode = 0x0; // uint32_t
        public const uint flWeight = 0x4; // float
        public const uint nMatrixNode = 0x8; // uint32_t
    }

    public static class FeFitMatrix_t 
    {
        public const uint bone = 0x0; // CTransform
        public const uint vCenter = 0x20; // Vector
        public const uint nEnd = 0x2C; // uint16_t
        public const uint nNode = 0x2E; // uint16_t
        public const uint nBeginDynamic = 0x30; // uint16_t
    }

    public static class FeFitWeight_t 
    {
        public const uint flWeight = 0x0; // float
        public const uint nNode = 0x4; // uint16_t
        public const uint nDummy = 0x6; // uint16_t
    }

    public static class FeFollowNode_t 
    {
        public const uint nParentNode = 0x0; // uint16_t
        public const uint nChildNode = 0x2; // uint16_t
        public const uint flWeight = 0x4; // float
    }

    public static class FeKelagerBend2_t 
    {
        public const uint flWeight = 0x0; // float[3]
        public const uint flHeight0 = 0xC; // float
        public const uint nNode = 0x10; // uint16_t[3]
        public const uint nReserved = 0x16; // uint16_t
    }

    public static class FeMorphLayerDepr_t 
    {
        public const uint m_Name = 0x0; // CUtlString
        public const uint m_nNameHash = 0x8; // uint32_t
        public const uint m_Nodes = 0x10; // CUtlVector<uint16_t>
        public const uint m_InitPos = 0x28; // CUtlVector<Vector>
        public const uint m_Gravity = 0x40; // CUtlVector<float>
        public const uint m_GoalStrength = 0x58; // CUtlVector<float>
        public const uint m_GoalDamping = 0x70; // CUtlVector<float>
        public const uint m_nFlags = 0x88; // uint32_t
    }

    public static class FeNodeBase_t 
    {
        public const uint nNode = 0x0; // uint16_t
        public const uint nDummy = 0x2; // uint16_t[3]
        public const uint nNodeX0 = 0x8; // uint16_t
        public const uint nNodeX1 = 0xA; // uint16_t
        public const uint nNodeY0 = 0xC; // uint16_t
        public const uint nNodeY1 = 0xE; // uint16_t
        public const uint qAdjust = 0x10; // QuaternionStorage
    }

    public static class FeNodeIntegrator_t 
    {
        public const uint flPointDamping = 0x0; // float
        public const uint flAnimationForceAttraction = 0x4; // float
        public const uint flAnimationVertexAttraction = 0x8; // float
        public const uint flGravity = 0xC; // float
    }

    public static class FeNodeReverseOffset_t 
    {
        public const uint vOffset = 0x0; // Vector
        public const uint nBoneCtrl = 0xC; // uint16_t
        public const uint nTargetNode = 0xE; // uint16_t
    }

    public static class FeNodeWindBase_t 
    {
        public const uint nNodeX0 = 0x0; // uint16_t
        public const uint nNodeX1 = 0x2; // uint16_t
        public const uint nNodeY0 = 0x4; // uint16_t
        public const uint nNodeY1 = 0x6; // uint16_t
    }

    public static class FeProxyVertexMap_t 
    {
        public const uint m_Name = 0x0; // CUtlString
        public const uint m_flWeight = 0x8; // float
    }

    public static class FeQuad_t 
    {
        public const uint nNode = 0x0; // uint16_t[4]
        public const uint flSlack = 0x8; // float
        public const uint vShape = 0xC; // Vector4D[4]
    }

    public static class FeRigidColliderIndices_t 
    {
        public const uint m_nTaperedCapsuleRigidIndex = 0x0; // uint16_t
        public const uint m_nSphereRigidIndex = 0x2; // uint16_t
        public const uint m_nBoxRigidIndex = 0x4; // uint16_t
        public const uint m_nCollisionPlaneIndex = 0x6; // uint16_t
    }

    public static class FeRodConstraint_t 
    {
        public const uint nNode = 0x0; // uint16_t[2]
        public const uint flMaxDist = 0x4; // float
        public const uint flMinDist = 0x8; // float
        public const uint flWeight0 = 0xC; // float
        public const uint flRelaxationFactor = 0x10; // float
    }

    public static class FeSimdAnimStrayRadius_t 
    {
        public const uint nNode = 0x0; // uint16_t[4][2]
        public const uint flMaxDist = 0x10; // fltx4
        public const uint flRelaxationFactor = 0x20; // fltx4
    }

    public static class FeSimdNodeBase_t 
    {
        public const uint nNode = 0x0; // uint16_t[4]
        public const uint nNodeX0 = 0x8; // uint16_t[4]
        public const uint nNodeX1 = 0x10; // uint16_t[4]
        public const uint nNodeY0 = 0x18; // uint16_t[4]
        public const uint nNodeY1 = 0x20; // uint16_t[4]
        public const uint nDummy = 0x28; // uint16_t[4]
        public const uint qAdjust = 0x30; // FourQuaternions
    }

    public static class FeSimdQuad_t 
    {
        public const uint nNode = 0x0; // uint16_t[4][4]
        public const uint f4Slack = 0x20; // fltx4
        public const uint vShape = 0x30; // FourVectors[4]
        public const uint f4Weights = 0xF0; // fltx4[4]
    }

    public static class FeSimdRodConstraint_t 
    {
        public const uint nNode = 0x0; // uint16_t[4][2]
        public const uint f4MaxDist = 0x10; // fltx4
        public const uint f4MinDist = 0x20; // fltx4
        public const uint f4Weight0 = 0x30; // fltx4
        public const uint f4RelaxationFactor = 0x40; // fltx4
    }

    public static class FeSimdSpringIntegrator_t 
    {
        public const uint nNode = 0x0; // uint16_t[4][2]
        public const uint flSpringRestLength = 0x10; // fltx4
        public const uint flSpringConstant = 0x20; // fltx4
        public const uint flSpringDamping = 0x30; // fltx4
        public const uint flNodeWeight0 = 0x40; // fltx4
    }

    public static class FeSimdTri_t 
    {
        public const uint nNode = 0x0; // uint32_t[4][3]
        public const uint w1 = 0x30; // fltx4
        public const uint w2 = 0x40; // fltx4
        public const uint v1x = 0x50; // fltx4
        public const uint v2 = 0x60; // FourVectors2D
    }

    public static class FeSoftParent_t 
    {
        public const uint nParent = 0x0; // int32_t
        public const uint flAlpha = 0x4; // float
    }

    public static class FeSourceEdge_t 
    {
        public const uint nNode = 0x0; // uint16_t[2]
    }

    public static class FeSphereRigid_t 
    {
        public const uint vSphere = 0x0; // fltx4
        public const uint nNode = 0x10; // uint16_t
        public const uint nCollisionMask = 0x12; // uint16_t
        public const uint nVertexMapIndex = 0x14; // uint16_t
        public const uint nFlags = 0x16; // uint16_t
    }

    public static class FeSpringIntegrator_t 
    {
        public const uint nNode = 0x0; // uint16_t[2]
        public const uint flSpringRestLength = 0x4; // float
        public const uint flSpringConstant = 0x8; // float
        public const uint flSpringDamping = 0xC; // float
        public const uint flNodeWeight0 = 0x10; // float
    }

    public static class FeStiffHingeBuild_t 
    {
        public const uint flMaxAngle = 0x0; // float
        public const uint flStrength = 0x4; // float
        public const uint flMotionBias = 0x8; // float[3]
        public const uint nNode = 0x14; // uint16_t[3]
    }

    public static class FeTaperedCapsuleRigid_t 
    {
        public const uint vSphere = 0x0; // fltx4[2]
        public const uint nNode = 0x20; // uint16_t
        public const uint nCollisionMask = 0x22; // uint16_t
        public const uint nVertexMapIndex = 0x24; // uint16_t
        public const uint nFlags = 0x26; // uint16_t
    }

    public static class FeTaperedCapsuleStretch_t 
    {
        public const uint nNode = 0x0; // uint16_t[2]
        public const uint nCollisionMask = 0x4; // uint16_t
        public const uint nDummy = 0x6; // uint16_t
        public const uint flRadius = 0x8; // float[2]
    }

    public static class FeTreeChildren_t 
    {
        public const uint nChild = 0x0; // uint16_t[2]
    }

    public static class FeTri_t 
    {
        public const uint nNode = 0x0; // uint16_t[3]
        public const uint w1 = 0x8; // float
        public const uint w2 = 0xC; // float
        public const uint v1x = 0x10; // float
        public const uint v2 = 0x14; // Vector2D
    }

    public static class FeTwistConstraint_t 
    {
        public const uint nNodeOrient = 0x0; // uint16_t
        public const uint nNodeEnd = 0x2; // uint16_t
        public const uint flTwistRelax = 0x4; // float
        public const uint flSwingRelax = 0x8; // float
    }

    public static class FeVertexMapBuild_t 
    {
        public const uint m_VertexMapName = 0x0; // CUtlString
        public const uint m_nNameHash = 0x8; // uint32_t
        public const uint m_Color = 0xC; // Color
        public const uint m_flVolumetricSolveStrength = 0x10; // float
        public const uint m_nScaleSourceNode = 0x14; // int32_t
        public const uint m_Weights = 0x18; // CUtlVector<float>
    }

    public static class FeVertexMapDesc_t 
    {
        public const uint sName = 0x0; // CUtlString
        public const uint nNameHash = 0x8; // uint32_t
        public const uint nColor = 0xC; // uint32_t
        public const uint nFlags = 0x10; // uint32_t
        public const uint nVertexBase = 0x14; // uint16_t
        public const uint nVertexCount = 0x16; // uint16_t
        public const uint nMapOffset = 0x18; // uint32_t
        public const uint nNodeListOffset = 0x1C; // uint32_t
        public const uint vCenterOfMass = 0x20; // Vector
        public const uint flVolumetricSolveStrength = 0x2C; // float
        public const uint nScaleSourceNode = 0x30; // int16_t
        public const uint nNodeListCount = 0x32; // uint16_t
    }

    public static class FeWeightedNode_t 
    {
        public const uint nNode = 0x0; // uint16_t
        public const uint nWeight = 0x2; // uint16_t
    }

    public static class FeWorldCollisionParams_t 
    {
        public const uint flWorldFriction = 0x0; // float
        public const uint flGroundFriction = 0x4; // float
        public const uint nListBegin = 0x8; // uint16_t
        public const uint nListEnd = 0xA; // uint16_t
    }

    public static class FourCovMatrices3 
    {
        public const uint m_vDiag = 0x0; // FourVectors
        public const uint m_flXY = 0x30; // fltx4
        public const uint m_flXZ = 0x40; // fltx4
        public const uint m_flYZ = 0x50; // fltx4
    }

    public static class FourVectors2D 
    {
        public const uint x = 0x0; // fltx4
        public const uint y = 0x10; // fltx4
    }

    public static class IPhysicsPlayerController 
    {
    }

    public static class OldFeEdge_t 
    {
        public const uint m_flK = 0x0; // float[3]
        public const uint invA = 0xC; // float
        public const uint t = 0x10; // float
        public const uint flThetaRelaxed = 0x14; // float
        public const uint flThetaFactor = 0x18; // float
        public const uint c01 = 0x1C; // float
        public const uint c02 = 0x20; // float
        public const uint c03 = 0x24; // float
        public const uint c04 = 0x28; // float
        public const uint flAxialModelDist = 0x2C; // float
        public const uint flAxialModelWeights = 0x30; // float[4]
        public const uint m_nNode = 0x40; // uint16_t[4]
    }

    public static class PhysFeModelDesc_t 
    {
        public const uint m_CtrlHash = 0x0; // CUtlVector<uint32_t>
        public const uint m_CtrlName = 0x18; // CUtlVector<CUtlString>
        public const uint m_nStaticNodeFlags = 0x30; // uint32_t
        public const uint m_nDynamicNodeFlags = 0x34; // uint32_t
        public const uint m_flLocalForce = 0x38; // float
        public const uint m_flLocalRotation = 0x3C; // float
        public const uint m_nNodeCount = 0x40; // uint16_t
        public const uint m_nStaticNodes = 0x42; // uint16_t
        public const uint m_nRotLockStaticNodes = 0x44; // uint16_t
        public const uint m_nFirstPositionDrivenNode = 0x46; // uint16_t
        public const uint m_nSimdTriCount1 = 0x48; // uint16_t
        public const uint m_nSimdTriCount2 = 0x4A; // uint16_t
        public const uint m_nSimdQuadCount1 = 0x4C; // uint16_t
        public const uint m_nSimdQuadCount2 = 0x4E; // uint16_t
        public const uint m_nQuadCount1 = 0x50; // uint16_t
        public const uint m_nQuadCount2 = 0x52; // uint16_t
        public const uint m_nTreeDepth = 0x54; // uint16_t
        public const uint m_nNodeBaseJiggleboneDependsCount = 0x56; // uint16_t
        public const uint m_nRopeCount = 0x58; // uint16_t
        public const uint m_Ropes = 0x60; // CUtlVector<uint16_t>
        public const uint m_NodeBases = 0x78; // CUtlVector<FeNodeBase_t>
        public const uint m_SimdNodeBases = 0x90; // CUtlVector<FeSimdNodeBase_t>
        public const uint m_Quads = 0xA8; // CUtlVector<FeQuad_t>
        public const uint m_SimdQuads = 0xC0; // CUtlVector<FeSimdQuad_t>
        public const uint m_SimdTris = 0xD8; // CUtlVector<FeSimdTri_t>
        public const uint m_SimdRods = 0xF0; // CUtlVector<FeSimdRodConstraint_t>
        public const uint m_InitPose = 0x108; // CUtlVector<CTransform>
        public const uint m_Rods = 0x120; // CUtlVector<FeRodConstraint_t>
        public const uint m_Twists = 0x138; // CUtlVector<FeTwistConstraint_t>
        public const uint m_AxialEdges = 0x150; // CUtlVector<FeAxialEdgeBend_t>
        public const uint m_NodeInvMasses = 0x168; // CUtlVector<float>
        public const uint m_CtrlOffsets = 0x180; // CUtlVector<FeCtrlOffset_t>
        public const uint m_CtrlOsOffsets = 0x198; // CUtlVector<FeCtrlOsOffset_t>
        public const uint m_FollowNodes = 0x1B0; // CUtlVector<FeFollowNode_t>
        public const uint m_CollisionPlanes = 0x1C8; // CUtlVector<FeCollisionPlane_t>
        public const uint m_NodeIntegrator = 0x1E0; // CUtlVector<FeNodeIntegrator_t>
        public const uint m_SpringIntegrator = 0x1F8; // CUtlVector<FeSpringIntegrator_t>
        public const uint m_SimdSpringIntegrator = 0x210; // CUtlVector<FeSimdSpringIntegrator_t>
        public const uint m_WorldCollisionParams = 0x228; // CUtlVector<FeWorldCollisionParams_t>
        public const uint m_LegacyStretchForce = 0x240; // CUtlVector<float>
        public const uint m_NodeCollisionRadii = 0x258; // CUtlVector<float>
        public const uint m_DynNodeFriction = 0x270; // CUtlVector<float>
        public const uint m_LocalRotation = 0x288; // CUtlVector<float>
        public const uint m_LocalForce = 0x2A0; // CUtlVector<float>
        public const uint m_TaperedCapsuleStretches = 0x2B8; // CUtlVector<FeTaperedCapsuleStretch_t>
        public const uint m_TaperedCapsuleRigids = 0x2D0; // CUtlVector<FeTaperedCapsuleRigid_t>
        public const uint m_SphereRigids = 0x2E8; // CUtlVector<FeSphereRigid_t>
        public const uint m_WorldCollisionNodes = 0x300; // CUtlVector<uint16_t>
        public const uint m_TreeParents = 0x318; // CUtlVector<uint16_t>
        public const uint m_TreeCollisionMasks = 0x330; // CUtlVector<uint16_t>
        public const uint m_TreeChildren = 0x348; // CUtlVector<FeTreeChildren_t>
        public const uint m_FreeNodes = 0x360; // CUtlVector<uint16_t>
        public const uint m_FitMatrices = 0x378; // CUtlVector<FeFitMatrix_t>
        public const uint m_FitWeights = 0x390; // CUtlVector<FeFitWeight_t>
        public const uint m_ReverseOffsets = 0x3A8; // CUtlVector<FeNodeReverseOffset_t>
        public const uint m_AnimStrayRadii = 0x3C0; // CUtlVector<FeAnimStrayRadius_t>
        public const uint m_SimdAnimStrayRadii = 0x3D8; // CUtlVector<FeSimdAnimStrayRadius_t>
        public const uint m_KelagerBends = 0x3F0; // CUtlVector<FeKelagerBend2_t>
        public const uint m_CtrlSoftOffsets = 0x408; // CUtlVector<FeCtrlSoftOffset_t>
        public const uint m_JiggleBones = 0x420; // CUtlVector<CFeIndexedJiggleBone>
        public const uint m_SourceElems = 0x438; // CUtlVector<uint16_t>
        public const uint m_GoalDampedSpringIntegrators = 0x450; // CUtlVector<uint32_t>
        public const uint m_Tris = 0x468; // CUtlVector<FeTri_t>
        public const uint m_nTriCount1 = 0x480; // uint16_t
        public const uint m_nTriCount2 = 0x482; // uint16_t
        public const uint m_nReservedUint8 = 0x484; // uint8_t
        public const uint m_nExtraPressureIterations = 0x485; // uint8_t
        public const uint m_nExtraGoalIterations = 0x486; // uint8_t
        public const uint m_nExtraIterations = 0x487; // uint8_t
        public const uint m_BoxRigids = 0x488; // CUtlVector<FeBoxRigid_t>
        public const uint m_DynNodeVertexSet = 0x4A0; // CUtlVector<uint8_t>
        public const uint m_VertexSetNames = 0x4B8; // CUtlVector<uint32_t>
        public const uint m_RigidColliderPriorities = 0x4D0; // CUtlVector<FeRigidColliderIndices_t>
        public const uint m_MorphLayers = 0x4E8; // CUtlVector<FeMorphLayerDepr_t>
        public const uint m_MorphSetData = 0x500; // CUtlVector<uint8_t>
        public const uint m_VertexMaps = 0x518; // CUtlVector<FeVertexMapDesc_t>
        public const uint m_VertexMapValues = 0x530; // CUtlVector<uint8_t>
        public const uint m_Effects = 0x548; // CUtlVector<FeEffectDesc_t>
        public const uint m_LockToParent = 0x560; // CUtlVector<FeCtrlOffset_t>
        public const uint m_LockToGoal = 0x578; // CUtlVector<uint16_t>
        public const uint m_DynNodeWindBases = 0x590; // CUtlVector<FeNodeWindBase_t>
        public const uint m_flInternalPressure = 0x5A8; // float
        public const uint m_flDefaultTimeDilation = 0x5AC; // float
        public const uint m_flWindage = 0x5B0; // float
        public const uint m_flWindDrag = 0x5B4; // float
        public const uint m_flDefaultSurfaceStretch = 0x5B8; // float
        public const uint m_flDefaultThreadStretch = 0x5BC; // float
        public const uint m_flDefaultGravityScale = 0x5C0; // float
        public const uint m_flDefaultVelAirDrag = 0x5C4; // float
        public const uint m_flDefaultExpAirDrag = 0x5C8; // float
        public const uint m_flDefaultVelQuadAirDrag = 0x5CC; // float
        public const uint m_flDefaultExpQuadAirDrag = 0x5D0; // float
        public const uint m_flRodVelocitySmoothRate = 0x5D4; // float
        public const uint m_flQuadVelocitySmoothRate = 0x5D8; // float
        public const uint m_flAddWorldCollisionRadius = 0x5DC; // float
        public const uint m_flDefaultVolumetricSolveAmount = 0x5E0; // float
        public const uint m_nRodVelocitySmoothIterations = 0x5E4; // uint16_t
        public const uint m_nQuadVelocitySmoothIterations = 0x5E6; // uint16_t
    }

    public static class RnBlendVertex_t 
    {
        public const uint m_nWeight0 = 0x0; // uint16_t
        public const uint m_nIndex0 = 0x2; // uint16_t
        public const uint m_nWeight1 = 0x4; // uint16_t
        public const uint m_nIndex1 = 0x6; // uint16_t
        public const uint m_nWeight2 = 0x8; // uint16_t
        public const uint m_nIndex2 = 0xA; // uint16_t
        public const uint m_nFlags = 0xC; // uint16_t
        public const uint m_nTargetIndex = 0xE; // uint16_t
    }

    public static class RnBodyDesc_t 
    {
        public const uint m_sDebugName = 0x0; // CUtlString
        public const uint m_vPosition = 0x8; // Vector
        public const uint m_qOrientation = 0x14; // QuaternionStorage
        public const uint m_vLinearVelocity = 0x24; // Vector
        public const uint m_vAngularVelocity = 0x30; // Vector
        public const uint m_vLocalMassCenter = 0x3C; // Vector
        public const uint m_LocalInertiaInv = 0x48; // Vector[3]
        public const uint m_flMassInv = 0x6C; // float
        public const uint m_flGameMass = 0x70; // float
        public const uint m_flInertiaScaleInv = 0x74; // float
        public const uint m_flLinearDamping = 0x78; // float
        public const uint m_flAngularDamping = 0x7C; // float
        public const uint m_flLinearDrag = 0x80; // float
        public const uint m_flAngularDrag = 0x84; // float
        public const uint m_flLinearBuoyancyDrag = 0x88; // float
        public const uint m_flAngularBuoyancyDrag = 0x8C; // float
        public const uint m_vLastAwakeForceAccum = 0x90; // Vector
        public const uint m_vLastAwakeTorqueAccum = 0x9C; // Vector
        public const uint m_flBuoyancyFactor = 0xA8; // float
        public const uint m_flGravityScale = 0xAC; // float
        public const uint m_flTimeScale = 0xB0; // float
        public const uint m_nBodyType = 0xB4; // int32_t
        public const uint m_nGameIndex = 0xB8; // uint32_t
        public const uint m_nGameFlags = 0xBC; // uint32_t
        public const uint m_nMinVelocityIterations = 0xC0; // int8_t
        public const uint m_nMinPositionIterations = 0xC1; // int8_t
        public const uint m_nMassPriority = 0xC2; // int8_t
        public const uint m_bEnabled = 0xC3; // bool
        public const uint m_bSleeping = 0xC4; // bool
        public const uint m_bIsContinuousEnabled = 0xC5; // bool
        public const uint m_bDragEnabled = 0xC6; // bool
        public const uint m_bBuoyancyDragEnabled = 0xC7; // bool
        public const uint m_bGravityDisabled = 0xC8; // bool
        public const uint m_bSpeculativeEnabled = 0xC9; // bool
        public const uint m_bHasShadowController = 0xCA; // bool
    }

    public static class RnCapsuleDesc_t  // RnShapeDesc_t
    {
        public const uint m_Capsule = 0x10; // RnCapsule_t
    }

    public static class RnCapsule_t 
    {
        public const uint m_vCenter = 0x0; // Vector[2]
        public const uint m_flRadius = 0x18; // float
    }

    public static class RnFace_t 
    {
        public const uint m_nEdge = 0x0; // uint8_t
    }

    public static class RnHalfEdge_t 
    {
        public const uint m_nNext = 0x0; // uint8_t
        public const uint m_nTwin = 0x1; // uint8_t
        public const uint m_nOrigin = 0x2; // uint8_t
        public const uint m_nFace = 0x3; // uint8_t
    }

    public static class RnHullDesc_t  // RnShapeDesc_t
    {
        public const uint m_Hull = 0x10; // RnHull_t
    }

    public static class RnHull_t 
    {
        public const uint m_vCentroid = 0x0; // Vector
        public const uint m_flMaxAngularRadius = 0xC; // float
        public const uint m_Bounds = 0x10; // AABB_t
        public const uint m_vOrthographicAreas = 0x28; // Vector
        public const uint m_MassProperties = 0x34; // matrix3x4_t
        public const uint m_flVolume = 0x64; // float
        public const uint m_Vertices = 0x68; // CUtlVector<RnVertex_t>
        public const uint m_VertexPositions = 0x80; // CUtlVector<Vector>
        public const uint m_Edges = 0x98; // CUtlVector<RnHalfEdge_t>
        public const uint m_Faces = 0xB0; // CUtlVector<RnFace_t>
        public const uint m_FacePlanes = 0xC8; // CUtlVector<RnPlane_t>
        public const uint m_nFlags = 0xE0; // uint32_t
        public const uint m_pRegionSVM = 0xE8; // CRegionSVM*
    }

    public static class RnMeshDesc_t  // RnShapeDesc_t
    {
        public const uint m_Mesh = 0x10; // RnMesh_t
    }

    public static class RnMesh_t 
    {
        public const uint m_vMin = 0x0; // Vector
        public const uint m_vMax = 0xC; // Vector
        public const uint m_Nodes = 0x18; // CUtlVector<RnNode_t>
        public const uint m_Vertices = 0x30; // CUtlVectorSIMDPaddedVector
        public const uint m_Triangles = 0x48; // CUtlVector<RnTriangle_t>
        public const uint m_Wings = 0x60; // CUtlVector<RnWing_t>
        public const uint m_Materials = 0x78; // CUtlVector<uint8_t>
        public const uint m_vOrthographicAreas = 0x90; // Vector
        public const uint m_nFlags = 0x9C; // uint32_t
        public const uint m_nDebugFlags = 0xA0; // uint32_t
    }

    public static class RnNode_t 
    {
        public const uint m_vMin = 0x0; // Vector
        public const uint m_nChildren = 0xC; // uint32_t
        public const uint m_vMax = 0x10; // Vector
        public const uint m_nTriangleOffset = 0x1C; // uint32_t
    }

    public static class RnPlane_t 
    {
        public const uint m_vNormal = 0x0; // Vector
        public const uint m_flOffset = 0xC; // float
    }

    public static class RnShapeDesc_t 
    {
        public const uint m_nCollisionAttributeIndex = 0x0; // uint32_t
        public const uint m_nSurfacePropertyIndex = 0x4; // uint32_t
        public const uint m_UserFriendlyName = 0x8; // CUtlString
    }

    public static class RnSoftbodyCapsule_t 
    {
        public const uint m_vCenter = 0x0; // Vector[2]
        public const uint m_flRadius = 0x18; // float
        public const uint m_nParticle = 0x1C; // uint16_t[2]
    }

    public static class RnSoftbodyParticle_t 
    {
        public const uint m_flMassInv = 0x0; // float
    }

    public static class RnSoftbodySpring_t 
    {
        public const uint m_nParticle = 0x0; // uint16_t[2]
        public const uint m_flLength = 0x4; // float
    }

    public static class RnSphereDesc_t  // RnShapeDesc_t
    {
        public const uint m_Sphere = 0x10; // RnSphere_t
    }

    public static class RnSphere_t 
    {
        public const uint m_vCenter = 0x0; // Vector
        public const uint m_flRadius = 0xC; // float
    }

    public static class RnTriangle_t 
    {
        public const uint m_nIndex = 0x0; // int32_t[3]
    }

    public static class RnVertex_t 
    {
        public const uint m_nEdge = 0x0; // uint8_t
    }

    public static class RnWing_t 
    {
        public const uint m_nIndex = 0x0; // int32_t[3]
    }

    public static class VertexPositionColor_t 
    {
        public const uint m_vPosition = 0x0; // Vector
    }

    public static class VertexPositionNormal_t 
    {
        public const uint m_vPosition = 0x0; // Vector
        public const uint m_vNormal = 0xC; // Vector
    }

    public static class constraint_axislimit_t 
    {
        public const uint flMinRotation = 0x0; // float
        public const uint flMaxRotation = 0x4; // float
        public const uint flMotorTargetAngSpeed = 0x8; // float
        public const uint flMotorMaxTorque = 0xC; // float
    }

    public static class constraint_breakableparams_t 
    {
        public const uint strength = 0x0; // float
        public const uint forceLimit = 0x4; // float
        public const uint torqueLimit = 0x8; // float
        public const uint bodyMassScale = 0xC; // float[2]
        public const uint isActive = 0x14; // bool
    }

    public static class constraint_hingeparams_t 
    {
        public const uint worldPosition = 0x0; // Vector
        public const uint worldAxisDirection = 0xC; // Vector
        public const uint hingeAxis = 0x18; // constraint_axislimit_t
        public const uint constraint = 0x28; // constraint_breakableparams_t
    }

    public static class vphysics_save_cphysicsbody_t  // RnBodyDesc_t
    {
        public const uint m_nOldPointer = 0xD0; // uint64_t
    }
}
