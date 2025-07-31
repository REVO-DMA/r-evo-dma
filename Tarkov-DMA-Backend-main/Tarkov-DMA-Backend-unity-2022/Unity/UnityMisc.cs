using SkiaSharp;

namespace Tarkov_DMA_Backend.Unity
{
    /// <summary>
    /// Unity Game Object Manager. Contains all Game Objects.
    /// </summary>
    public readonly struct GameObjectManager
    {
        public readonly ulong LastTaggedNode; // 0x0

        public readonly ulong TaggedNodes; // 0x8

        public readonly ulong LastMainCameraTaggedNode; // 0x10

        public readonly ulong MainCameraTaggedNodes; // 0x18

        public readonly ulong LastActiveNode; // 0x20

        public readonly ulong ActiveNodes; // 0x28


        /// <summary>
        /// Returns the Game Object Manager for the current UnityPlayer.
        /// </summary>
        /// <param name="unityBase">UnityPlayer Base Addr</param>
        /// <returns>Game Object Manager</returns>
        public static GameObjectManager Get(ulong unityBase)
        {
            try
            {
                var gomPtr = Memory.ReadPtr(unityBase + UnityOffsets.ModuleBase.GameObjectManager, false);
                return Memory.ReadValue<GameObjectManager>(gomPtr, false);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Loading Game Object Manager", ex);
            }
        }
    }

    /// <summary>
    /// GOM List Node.
    /// </summary>
    public readonly struct BaseObject
    {
        /// <summary>
        /// Previous ListNode
        /// </summary>
        public readonly ulong PreviousObjectLink; // 0x0
        /// <summary>
        /// Next ListNode
        /// </summary>
        public readonly ulong NextObjectLink; // 0x8
        /// <summary>
        /// Current GameObject
        /// </summary>
        public readonly ulong CurrentObject; // 0x10   (to Offsets.GameObject)
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public readonly struct UnityColor
    {
        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;

        public UnityColor(float r, float g, float b, float a = 1f)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public UnityColor(byte r, byte g, byte b, byte a = 255)
        {
            R = r / 255f;
            G = g / 255f;
            B = b / 255f;
            A = a / 255f;
        }

        public UnityColor(string hex)
        {
            var color = SKColor.Parse(hex);

            R = color.Red / 255f;
            G = color.Green / 255f;
            B = color.Blue / 255f;
            A = color.Alpha / 255f;
        }

        public UnityColor(SKColor color)
        {
            R = color.Red / 255f;
            G = color.Green / 255f;
            B = color.Blue / 255f;
            A = color.Alpha / 255f;
        }

        public readonly override string ToString() => $"({R * 255}, {G * 255}, {B * 255}, {A * 255})";

        public static int GetSize()
        {
            return Unsafe.SizeOf<UnityColor>();
        }

        public static uint GetSizeU()
        {
            return (uint)GetSize();
        }

        public static UnityColor Invert(UnityColor color)
        {
            float invertedR = 1f - color.R;
            float invertedG = 1f - color.G;
            float invertedB = 1f - color.B;

            return new(invertedR, invertedG, invertedB, color.A);
        }
    }

    /// <summary>
    /// Most higher level EFT Assembly Classes that implement MonoBehaviour/Behaviour/Component/Object.
    /// </summary>
    public readonly struct ObjectClass
    {
        /// <summary>
        /// Read the Class Name from any ObjectClass that implements MonoBehaviour.
        /// </summary>
        /// <param name="objectClass">ObjectClass address.</param>
        /// <returns>Name (string) of the object class given.</returns>
        public static string ReadName(ulong objectClass, int length = 128, bool useCache = true)
        {
            try
            {
                var namePtr = Memory.ReadPtrChain(objectClass, UnityOffsets.Component.To_NativeClassName, useCache);
                return Memory.ReadString(namePtr, length, useCache);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Reading Object Class Name", ex);
            }
        }
    }
}