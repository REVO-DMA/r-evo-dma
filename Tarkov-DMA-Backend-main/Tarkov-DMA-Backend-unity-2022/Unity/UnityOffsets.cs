using Tarkov_DMA_Backend.Tarkov;
using static SDK.Offsets;

namespace Tarkov_DMA_Backend
{
    public readonly struct UnityOffsets
    {
        public readonly struct EFTClass // Any EFT Class that has [10] m_CachedPtr : IntPtr
        {
            public static readonly uint[] To_GameObject = new uint[] { 0x10, 0x30 }; // to GameObject
            public const uint SuperType = 0x28;
        }

        public readonly struct Transform
        {
            public static uint[] GetTransformChain(Bone index)
            {
                return new uint[] { PlayerBody.SkeletonRootJoint, Skeleton._values, UnityList.Base, UnityListBase.Start + (uint)index * 0x8, 0x10 };
            }

            public static uint[] GetTransformChain(int index)
            {
                return new uint[] { PlayerBody.SkeletonRootJoint, Skeleton._values, UnityList.Base, UnityListBase.Start + (uint)index * 0x8, 0x10 };
            }

            public static uint[] GetTransformChainLeadup()
            {
                return new uint[] { PlayerBody.SkeletonRootJoint, Skeleton._values, UnityList.Base };
            }

            public static uint[] GetTransformChainPartial(int index)
            {
                return new uint[] { UnityListBase.Start + (uint)index * 0x8, 0x10 };
            }
        }

        public readonly struct Component
        {
            public const uint Size = 0x38; // Equal to sizeof(Unity::Component) __This is not a struct field__
            public static readonly uint[] To_NativeClassName = new uint[] { 0x0, 0x0, 0x48 }; // String
            public const uint GameObject = 0x30; // To GameObject
        }

        public readonly struct Behaviour
        {
            public const uint IsEnabled = 0x38; // bool, Behaviour : m_Enabled
            public const uint IsAdded = 0x39; // bool, Behaviour : m_IsAdded
        }

        public readonly struct UnityList
        {
            public const uint Base = 0x10; // to UnityListBase
            public const uint Count = 0x18; // int32
        }

        public readonly struct UnityListBase
        {
            public const uint Start = 0x20; // start of list +(i * 0x8)
        }

        public readonly struct UnityString
        {
            public const uint Length = 0x10; // int32
            public const uint Value = 0x14; // string,unicode
        }

        public readonly struct Animator
        {
            public const uint Speed = 0x47C; // float
        }

        public readonly struct ModuleBase
        {
            public const uint GameObjectManager = 0x1CF93E0;
            public const uint AllCameras = 0x1BF8BC0; // Lookup in IDA 's_AllCamera'
            public const uint InputManager = 0x1C91748;
        }

        public readonly struct UnityInputManager
        {
            public const uint CurrentKeyState = 0x58;
        }

        public readonly struct DynamicBitsetBase
        {
            public const uint Bits = 0x8; // ptr to buffer
        }

        public readonly struct UnityTimeManager
        {
            public const uint TimeScale = 0xFC; // float
        }

        public readonly struct TransformInternal
        {
            public const uint Hierarchy = 0x38; // to TransformHierarchy
            public const uint HierarchyIndex = 0x40; // int32
        }

        public readonly struct TransformHierarchy
        {
            public const uint Vertices = 0x18; // Vector128<float>[]
            public const uint Indices = 0x20; // int[]
        }

        public readonly struct UnityPosition
        {
            public const uint Vector3 = 0x90; // value:Vector3
        }

        public readonly struct GameObject
        {
            public static readonly uint[] To_PositionDefault = new uint[] { 0x30, 0x8, 0x38 }; // to UnityPosition
            public static readonly uint[] To_TransformInternal = new uint[] { 0x10, 0x30, 0x30, 0x8 }; // to TransformInternal
            public static readonly uint[] To_ObjectClass = new uint[] { Components, 0x18, 0x28 };
            public const uint Components = 0x30; // dynamic_array<GameObject::ComponentPair,0> ?
            public const uint ObjectName = 0x60; // m_Name
        }

        public readonly struct Material
        {
            public const uint Instance = 0x10; // UnityObject, to base classes of this object
        }

        public readonly struct UnityObject // Inherits from EditorExtension
        {
            public const uint InstanceID = 0x8; // int32 , m_InstanceID
        }

        public readonly struct SkinnedMeshRenderer // SkinnedMeshRenderer : Renderer
        {
            public const uint Renderer = 0x10; // Renderer : Unity::Component
        }

        public readonly struct BaseRenderer
        {
            public const uint RendererData = 0x8; // SharedRendererData (m_RendererData)
        }

        public readonly struct SharedRendererData
        {
            public const uint TransformInfo = 0x0; // TransformInfo (m_TransformInfo)
        }

        public readonly struct TransformInfo
        {
            public const uint LocalAABB = 0x98; // AABB (localAABB)
        }

        public readonly struct Renderer // Renderer : Unity::Component
        {
            public const uint Materials = 0x148; // m_Materials : dynamic_array<PPtr<Material>,0>
            public const uint Count = 0x158; // Extends from m_Materials type (0x20 length?)
        }

        public readonly struct Camera // NOTE: Add previous struct size to get final offset
        {
            public const uint ViewMatrix = 0x100; // Matrix4x4 (m_WorldToClipMatrix)
            public const uint FOV = 0x180; // float (m_FieldOfView)
            public const uint LastPosition = 0x454; // float (m_LastPosition)
            public const uint NearClip = 0x464; // float (m_NearClip)
            public const uint AspectRatio = 0x4F0; // float (m_Aspect)
            public const uint OcclusionCulling = 0x524; // bool (m_OcclusionCulling)
        }
    }
}
