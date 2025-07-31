using Reloaded.Assembler;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.MemDMA.ScatterAPI;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.Patcher;

namespace Tarkov_DMA_Backend.Tarkov.Toolkit.AlwaysActiveFeatures
{
    public class StreamerMode : AlwaysActiveFeature
    {
        private const string thisID = "streamerMode";

        private ulong Application_GameObject = 0x0;
        private ulong TarkovApplication = 0x0;

        public StreamerMode(int delayMs) : base(delayMs, thisID) { }

        public override void Run(ref List<ScatterWriteEntry> writes, params object[] parameters)
        {
            bool newState = ToolkitManager.AlwaysActiveFeatureState[thisID];

            if (!newState) return;

            if (EFTDMA.GOM == null)
                return;

            bool spoofName = ToolkitManager.FeatureSettings_bool["streamerMode_SpoofName"];
            bool spoofLevel = ToolkitManager.FeatureSettings_bool["streamerMode_SpoofLevel"];
            bool spoofDogtags = ToolkitManager.FeatureSettings_bool["streamerMode_SpoofDogtags"];
            bool hideOverallInfo = ToolkitManager.FeatureSettings_bool["streamerMode_HideOverallInfo"];
            bool disableNotifications = ToolkitManager.FeatureSettings_bool["streamerMode_DisableNotifications"];

            if (spoofName)
                PatchIsLocalStreamer();

            if (spoofDogtags)
            {
                PatchDogtagNicknameP1();
                PatchDogtagNicknameP2();
            }

            if (disableNotifications)
                DisableNotifier();

            if (hideOverallInfo)
                DisableRightSide();

            if (spoofLevel)
                GloballySpoofLevel();

            var localPlayer = EFTDMA.LocalPlayer;
            ulong localProfile;
            if (localPlayer != null)
                localProfile = localPlayer.Profile;
            else
            {
                if (Application_GameObject == 0x0)
                    Application_GameObject = Memory.GetObjectFromList(EFTDMA.GOM.Value.ActiveNodes, EFTDMA.GOM.Value.LastActiveNode, "Application (Main Client)", 3000);
                if (Application_GameObject == 0x0)
                    throw new Exception("Unable to get \"Application (Main Client)\" Game Object");

                if (TarkovApplication == 0x0)
                    TarkovApplication = Memory.GetObjectComponent(Application_GameObject, "TarkovApplication");
                if (TarkovApplication == 0x0)
                    throw new Exception("Unable to get \"TarkovApplication\" component");

                localProfile = Memory.ReadPtrChain(TarkovApplication, Offsets.TarkovApplication.To_Profile);
            }

            ulong profileInfo = Memory.ReadPtr(localProfile + Offsets.Profile.Info, false);

            if (spoofName)
            {
                ulong usernameAddr = Memory.ReadPtr(profileInfo + Offsets.PlayerInfo.Nickname); // Username
                int originalUsernameLength = Memory.ReadValue<int>(usernameAddr + UnityOffsets.UnityString.Length);

                // Spoof the username
                string result = new(' ', originalUsernameLength);
                writes.Add(ScatterWriteEntry.Create(usernameAddr + UnityOffsets.UnityString.Value, Encoding.Unicode.GetBytes(result)));

                // Spoof the account type
                writes.Add(ScatterWriteEntry.Create(profileInfo + Offsets.PlayerInfo.MemberCategory, (int)Enums.EMemberCategory.Sherpa));
            }
        }

        private bool IsLocalStreamerMethodPatched = false;

        /// <summary>
        /// Force "<Streamer>" text for names.
        /// </summary>
        private void PatchIsLocalStreamer()
        {
            if (IsLocalStreamerMethodPatched) return;

            SignatureInfo sigInfo = new(null, ShellKeeper.PatchTrue);

            PatchMethod(ClassNames.StreamerMode.ClassName, ClassNames.StreamerMode.MethodName, sigInfo, compileClass: true);

            IsLocalStreamerMethodPatched = true;
        }

        private bool DogtagNicknamePatchedP1 = false;
        private static readonly byte[] DogtagNicknameP1Signature = new byte[]
        {
            0x48, 0x8B, 0x40, 0x30 // mov rax,[rax+30]
        };
        /// <summary>
        /// Makes the function return null instead of the nickname field.
        /// </summary>
        private static readonly byte[] DogtagNicknameP1Patch = new byte[]
        {
            0x48, 0x31, 0xC0, 0x90 // xor rax, rax
        };

        private void PatchDogtagNicknameP1()
        {
            if (DogtagNicknamePatchedP1) return;
            
            SignatureInfo sigInfo = new(DogtagNicknameP1Signature, DogtagNicknameP1Patch, 100);

            PatchMethod("EFT.InventoryLogic.DogtagComponent", ClassNames.DogtagComponent.MethodName, sigInfo, compileClass: true);

            DogtagNicknamePatchedP1 = true;
        }

        private bool DogtagNicknamePatchedP2 = false;
        private const string DogtagNicknameP2SignatureMask = "xx????xxx"; // ignore the jne address
        private static readonly byte[] DogtagNicknameP2Signature = new byte[]
        {
            0x0F, 0x84, 0x0, 0x0, 0x0, 0x0,
            0x4D, 0x8B, 0x66
        };
        /// <summary>
        /// Basically tring to make it so this if statement's contents are not ran:
        /// if (itemComponent3 != null && !string.IsNullOrEmpty(itemComponent3.Nickname))
        /// {
        ///	    text = (examined? itemComponent3.Nickname.SubstringIfNecessary(20) : \uEF86.\uE000(295345));
        /// }
        /// </summary>
        private static readonly byte[] DogtagNicknameP2Patch = new byte[] // patches jne into jmp
        {
            0x90, 0xE9 // nop jmp
        };

        /// <summary>
        /// Patches game code to hide the player nickname on all dogtag item grids.
        /// </summary>
        private void PatchDogtagNicknameP2()
        {
            if (DogtagNicknamePatchedP2) return;

            SignatureInfo sigInfo = new(DogtagNicknameP2Signature, DogtagNicknameP2Signature.Patch(DogtagNicknameP2Patch), 0x1000, DogtagNicknameP2SignatureMask, DogtagNicknameP2SignatureMask, 0, DogtagNicknameP2Patch);

            PatchMethod("EFT.UI.DragAndDrop.GridItemView", ClassNames.GridItemView.MethodName, sigInfo, compileClass: true);

            DogtagNicknamePatchedP2 = true;
        }

        private bool NotifierDisabled = false;

        /// <summary>
        /// Disables all notifications. They would normally show friend names and possibly other incriminating information.
        /// </summary>
        private void DisableNotifier()
        {
            if (NotifierDisabled) return;

            ulong notifier = NativeHelper.FindGameObject("Preloader UI/Preloader UI/BottomPanel/Content/UpperPart/Notifier/Content");
            if (notifier != 0x0)
            {
                NativeHelper.GameObjectSetActive(notifier, false);
                NotifierDisabled = true;
            }
        }

        private bool RightSideDisabled = false;

        /// <summary>
        /// Disables the right side overall tab panel that shows player stats
        /// </summary>
        private void DisableRightSide()
        {
            if (RightSideDisabled) return;

            ulong rightSide = NativeHelper.FindGameObject("Common UI/Common UI/InventoryScreen/Overall Panel/RightSide");
            if (rightSide != 0x0)
            {
                NativeHelper.GameObjectSetActive(rightSide, false);
                RightSideDisabled = true;
            }
        }

        private bool LevelGloballySpoofed = false;
        private static readonly byte[] GloballySpoofLevelSignature = new byte[]
        {
            0x45, 0x85, 0xF6, // test r14d,r14d
            0x0F, 0x84, // je Set+F3
        };

        private void GloballySpoofLevel()
        {
            if (LevelGloballySpoofed) return;

            Assembler assembler = new();

            string[] mnemonicsA = new[]
            {
                "use64",
                "mov rdi, 79",
                "nop",
                "nop",
            };
            byte[] shellcodeA = assembler.Assemble(mnemonicsA);
            SignatureInfo sigInfoA = new(GloballySpoofLevelSignature, shellcodeA, 100);
            PatchMethod("EFT.UI.PlayerLevelPanel", "Set", sigInfoA, compileClass: true);

            SignatureInfo sigInfoB = new(null, ShellKeeper.ReturnInt(79));
            PatchMethod("EFT.Profile+TraderInfo", "get_ProfileLevel", sigInfoB, compileClass: true);

            // EFT.UI.Chat.ChatMember:Show

            LevelGloballySpoofed = true;
        }
    }
}
