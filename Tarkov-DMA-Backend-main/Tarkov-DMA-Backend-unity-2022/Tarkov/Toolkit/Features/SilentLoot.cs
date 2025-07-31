using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Tarkov.Loot;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.Features
{
    public class SilentLoot : Feature
    {
        private const string thisID = "silentLoot";

        private const string patchClassName = "EFT.Player";
        private const string patchMethod = "SaveInteractionRayInfo";
        private const string patchMethodB = "set_InteractionRayDirectionOnStartOperation";

        private static readonly byte[] signature;
        private static readonly byte[] originalBytes;
        private static readonly byte[] patchBytes = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };

        public static MonoMethod? MonoMethod_SaveInteractionRayInfo = null;
        
        public static ulong InteractionRaycastAddr = 0x0;
        public static uint InteractionRaycastPatchOffset = uint.MaxValue;

        static SilentLoot()
        {
            byte[] b = BitConverter.GetBytes(Offsets.Player.InteractableObject);

            signature = new byte[]
            {
                0x48, 0x89, 0xBE, b[0], b[1], b[2], b[3],   // mov [rsi+00000508],rdi
                0x48, 0x8D, 0x8E, b[0], b[1], b[2], b[3],   // lea rcx,[rsi+00000508]
                0x66, 0x66, 0x90,                           // nop 3
            };

            originalBytes = new byte[] { 0x48, 0x89, 0xBE, b[0], b[1], b[2], b[3] }; // mov [rsi+00000508],rdi (this.InteractableObject = interactableObject;)
        }

        private LootItem _lockedItem = null;

        public SilentLoot(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, Player localPlayer, params object[] parameters)
        {
            if (!ToolkitManager.FeatureState[thisID]) return;

            if (MonoMethod_SaveInteractionRayInfo == null)
            {
                var mClass = EFTDMA.MonoClasses.GetClass("EFT.Player");

                ulong compiledClass = NativeHelper.CompileClass((ulong)mClass);
                if (compiledClass == 0x0)
                    throw new Exception($"Unable to compile class EFT.Player!");

                var mMethod = mClass.FindMethod(patchMethod);
                if (mMethod == 0x0)
                    throw new Exception($"Unable to find method {patchMethod}!");

                ulong methodAddr = NativeHelper.CompileMethod((ulong)mMethod);
                if (methodAddr == 0x0)
                    throw new Exception($"Unable to compile method {patchMethod}!");

                MonoMethod_SaveInteractionRayInfo = new MonoMethod(methodAddr);
            }

            if (InteractionRaycastAddr == 0x0 || InteractionRaycastPatchOffset == uint.MaxValue)
            {
                var mClass = EFTDMA.MonoClasses.GetClass("EFT.Player");

                var mMethod = mClass.FindMethod("InteractionRaycast");
                if (mMethod == 0x0)
                    throw new Exception($"Unable to find method InteractionRaycast!");

                ulong methodAddr = NativeHelper.CompileMethod((ulong)mMethod);
                if (methodAddr == 0x0)
                    throw new Exception($"Unable to compile method InteractionRaycast!");

                InteractionRaycastAddr = methodAddr;

                var methodBytes = Memory.ReadBufferEnsure(InteractionRaycastAddr, 0x1000);
                if (methodBytes == null)
                    throw new Exception("Invalid methodBytes.");

                // Get patch offset
                InteractionRaycastPatchOffset = methodBytes.FindSignature(in signature);
            }

            // Restore native looting behavior
            if (OverriddenState == null && _lockedItem != null)
            {
                if (!Memory.WriteBufferEnsure(InteractionRaycastAddr + InteractionRaycastPatchOffset, originalBytes))
                    return;

                SignatureInfo sigInfo = new(null, new byte[] { 0x55 });
                PatchMethod(MonoMethod_SaveInteractionRayInfo.Value, patchClassName, patchMethod, sigInfo, false);

                _lockedItem = null;
                return;
            }
            else if (OverriddenState == true && _lockedItem == null) // Find the closest filtered item
            {
                if (EFTDMA.Loot == null) return;
                if (EFTDMA.Loot.DisplayLoot == null) return;

                Vector3 localPlayerPosition = localPlayer.Position;

                var LootItems = EFTDMA.Loot.DisplayLoot;
                int lootCount = LootItems.Count;
                float smallestDistance = float.MaxValue;
                LootItem closestItem = null;
                foreach (var lootItemRaw in LootItems)
                {
                    var lootItem = lootItemRaw.Value;
                    if (lootItem == null) continue;

                    float distance = Vector3.Distance(localPlayerPosition, lootItem.Position);

                    if (distance <= LootThroughWalls.newLootRaycastDistance && distance < smallestDistance)
                    {
                        smallestDistance = distance;
                        closestItem = lootItem;
                    }
                }

                if (closestItem != null)
                {
                    if (!Memory.WriteBufferEnsure(InteractionRaycastAddr + InteractionRaycastPatchOffset, patchBytes))
                        return;

                    SignatureInfo sigInfo = new(null, ShellKeeper.PatchReturn);
                    PatchMethod(MonoMethod_SaveInteractionRayInfo.Value, patchClassName, patchMethod, sigInfo, false);

                    _lockedItem = closestItem;

                    // Don't lock up toolkit manager thread
                    Task.Run(async () =>
                    {
                        await Task.Delay(50);

                        // Get correct address based on whether or not this item is in a container
                        ulong usedAddr;
                        if (_lockedItem.ContainerAddress != 0x0)
                            usedAddr = _lockedItem.ContainerAddress;
                        else
                            usedAddr = _lockedItem.BaseAddress;

                        float distance = ToolkitManager.FeatureSettings_float["silentLoot_distance"];
                        Memory.WriteValue(localPlayer.Base + Offsets.Player.InteractionRayOriginOnStartOperation, _lockedItem.Position + new Vector3(0f, distance, 0f));
                        Memory.WriteValue(localPlayer.Base + Offsets.Player.InteractionRayDirectionOnStartOperation, new Vector3(0f, -1f, 0f));

                        Memory.WriteValue(localPlayer.Base + Offsets.Player.InteractableObject, usedAddr);
                    });
                }
            }
        }
    }
}
