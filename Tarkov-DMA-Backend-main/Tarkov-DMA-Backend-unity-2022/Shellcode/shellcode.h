#pragma once

const int PATCH_SIZE = 16;
const int MAX_PLAYER_COUNT = 80;
const int CHECK_BONE_COUNT = 6;

enum BoneNames
{
    Head = 133,
    CenterMass = 36,
    LeftShoulder = 90,
    RightShoulder = 111,
    LeftKnee = 17,
    RightKnee = 22
};

const int CheckBones[CHECK_BONE_COUNT] = {
    BoneNames::Head,
    BoneNames::CenterMass,
    BoneNames::LeftShoulder,
    BoneNames::RightShoulder,
    BoneNames::LeftKnee,
    BoneNames::RightKnee
};

struct VisibilityCheckOffsets
{
    // List
    uint32_t ListBase;
    uint32_t ListCount;
    uint32_t ListFirstElement;

    // Player
    uint32_t MovementContext;
    uint32_t PlayerBodyClientPlayer;
    uint32_t AIDataClientPlayer;
    uint32_t IsAIClientPlayer;
    uint32_t IsAI;
    uint32_t PlayerBody;
    uint32_t SkeletonValues;
    uint32_t ProceduralWeaponAnimation;
    uint32_t HandsController;

    // Player Body
    uint32_t SkeletonRootJoint;
    uint32_t PointOfView;

    // Point Of View
    uint32_t POV;

    // Movement CTX
    uint32_t LookDirection;

    // Firearm Controller
    uint32_t FirePort;

    // PWA
    uint32_t HandsContainer;

    // PlayerSpring
    uint32_t CameraTransform;

    // GameWorld
    uint32_t RegisteredPlayers;
    uint32_t MainPlayer;

    // OpticCameraManagerContainer
    uint32_t OCMContainerInstance;
    uint32_t OCM;
    uint32_t FPSCamera;

    // OpticCameraManager
    uint32_t OCMCamera;
    uint32_t CurrentOpticSight;

    // OpticSight
    uint32_t LensRenderer;

    // Unity Renderer
    uint32_t LocalAABB;

    // Unity Camera
    uint32_t ViewMatrix;
    uint32_t FOV;
    uint32_t LastPosition;

    // Unity Behaviour
    uint32_t IsAdded;
};

struct RaycastHit
{
    Vector3 point;
    Vector3 normal;
    unsigned int faceId;
    float distance;
    Vector2 uv;
    unsigned int colliderId;
};

struct AABB
{
    Vector3 Center;
    Vector3 Extent;
};

struct ESPData
{
    bool IsScoped;
    Vector3 LensPosition;
    Matrix4x4 ScopeMatrix;
    float ScopeFOV;
    Matrix4x4 CameraMatrix;
    AABB AABB;

    // Weapon Stuff
    Vector3 firePortPos;
    Vector3 firePortDirection;
    Vector3 firePortIntersectionPoint;
    float intersectionTestDistance;
};

using LinecastFunction_t = bool(*)(Vector3, Vector3, RaycastHit&, int);
using Transform_GetPosition_t = void(*)(ulong, Vector3&);
using Transform_SetPosition_t = void(*)(ulong, Vector3&);
using Transform_TransformDirection_t = void(*)(ulong, Vector3&, Vector3&);
using EFT_Player_set_PointOfView = void(*)(ulong, int);
using EFT_Player_FirearmController_AdjustShotVectors = void(*)(ulong, Vector3&, Vector3&);
using OriginalFunction_t = void(*)(ulong);
using ExecutorFunction_t = void(*)(ulong, void*);

struct ShellCodeData_t
{
    bool shellcodeActive;

    LinecastFunction_t linecastFunction;
    Transform_GetPosition_t transform_getPosition;
    Transform_SetPosition_t transform_setPosition;
    Transform_TransformDirection_t transform_transformDirection;
    EFT_Player_set_PointOfView eft_player_set_PointOfView;
    EFT_Player_FirearmController_AdjustShotVectors eft_player_firearmController_adjustShotVectors;
    ExecutorFunction_t executorFunction;

    // TPP Stuff
    bool tpp_currentState;
    bool tpp_enabled;
    float tpp_horizontalDistance;
    float tpp_horizontalOffset;
    float tpp_verticalDistance;

    // ESP Data Stuff
    ulong ocmContainer;
    ESPData espData;

    VisibilityCheckOffsets offsets;

    Vector3 rayStart;
    Vector3 tmpVec3;
    RaycastHit tmpRaycastHit;
    int highPolyWithTerrainMask;
    int hitMask;
    int checkBones[CHECK_BONE_COUNT];

    OriginalFunction_t gameWorld_Update;
    uint8_t originalBytes[PATCH_SIZE];
    uint8_t patchBytes[PATCH_SIZE];

    bool visiblePlayers[MAX_PLAYER_COUNT * CHECK_BONE_COUNT];
};