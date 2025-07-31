using arena_dma_backend.Arena;

namespace arena_dma_backend.Unity
{
    public static class GOM
    {
        public readonly struct BaseObject
        {
            /// <summary>
            /// Previous ListNode
            /// </summary>
            public readonly ulong PreviousObjectLink;
            /// <summary>
            /// Next ListNode
            /// </summary>
            public readonly ulong NextObjectLink;
            /// <summary>
            /// Current GameObject
            /// </summary>
            public readonly ulong CurrentObject;
        }

        public readonly struct GameObjectManager
        {
            public readonly ulong LastTaggedNode;

            public readonly ulong TaggedNodes;

            public readonly ulong LastMainCameraTaggedNode;

            public readonly ulong MainCameraTaggedNodes;

            public readonly ulong LastActiveNode;

            public readonly ulong ActiveNodes;


            /// <summary>
            /// Returns the Game Object Manager for the current UnityPlayer.
            /// </summary>
            /// <returns>Game Object Manager</returns>
            public static GameObjectManager Get()
            {
                try
                {
                    ulong gomPtr = Memory.ReadPtr(Memory.UnityPlayerModuleBase + UnityOffsets.ModuleBase.GameObjectManager);
                    return Memory.ReadValue<GameObjectManager>(gomPtr);
                }
                catch (Exception ex)
                {
                    throw new Exception($"[GOM] -> Get(): Unable to load GOM {ex}");
                }
            }
        }
    }
}
