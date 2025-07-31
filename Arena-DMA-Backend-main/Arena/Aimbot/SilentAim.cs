using arena_dma_backend.Ballistics;
using arena_dma_backend.Misc;
using arena_dma_backend.Mono;
using arena_dma_backend.Unity;

namespace arena_dma_backend.Arena
{
    public sealed class SilentAim : IDisposable
    {
        /// <summary>
        /// The original bytes of the WeaponDirection getter method.
        /// </summary>
        public static readonly byte[] WeaponDirectionGetterOriginalBytes = new byte[]
        {
            0x55,                    					// push rbp
            0x48, 0x8B, 0xEC,              				// mov rbp,rsp
            0x48, 0x81, 0xEC, 0x90, 0x00, 0x00, 0x00,   // sub rsp,00000090
            0x48, 0x89, 0x7D, 0xF8,           			// mov [rbp-08],rdi
            0x48, 0x89, 0x55, 0xF0,           			// mov [rbp-10],rdx
            0x48, 0x8B, 0xF9,              				// mov rdi,rcx
            0x49, 0xBB, 								// mov r11
        };
        /// <summary>
        /// The silent aim bytes of the WeaponDirection getter method.
        /// </summary>
        public static readonly byte[] WeaponDirectionGetterSilentBytes = new byte[]
        {
            0xC7, 0x02, // mov [rdx], xBytes
            0x0, 0x0, 0x0, 0x0, // X

            0xC7, 0x42, 0x04, // mov [rdx+4], yBytes
            0x0, 0x0, 0x0, 0x0, // Y

            0xC7, 0x42, 0x08, // mov [rdx+8], zBytes
            0x0, 0x0, 0x0, 0x0, // Z

            0x48, 0x89, 0xD0, // mov rax, rdx

            0xC3 // ret
        };

        /// <summary>
        /// "EFT.Player+FirearmController:get_WeaponDirection()" method address.
        /// </summary>
        private readonly ulong _saGetter;
        /// <summary>
        /// The target.
        /// </summary>
        private ulong _playerAddress;
        /// <summary>
        /// The user configured aim bone index.
        /// </summary>
        private readonly int _aimBoneIndex = Program.UserConfig.AimBone;

        public SilentAim(ulong saGetter, ulong playerAddress)
        {
            _saGetter = saGetter;
            _playerAddress = playerAddress;
        }

        public void UpdateTarget(ulong newPlayerAddr)
        {
            _playerAddress = newPlayerAddr;
        }

        public bool Update()
        {
            return UpdateWeaponDirection();
        }

        private Vector3 FormTrajectory()
        {
            if (!Game.Players.TryGetValue(_playerAddress, out Player player) || !player.CanRender())
                return Vector3.Zero;

            Transform fireport = AimbotPrewarmer.Fireport;
            if (fireport == null) return Vector3.Zero;

            Vector3 startPos = fireport.GetPosition();

            Vector3 endPos = player.BonePositions[_aimBoneIndex];
            Aimbot.SilentAimPosition = endPos;
            Vector3 targetVelocity = player.Velocity;

            var sim = BallisticsSimulation.Run(startPos, endPos);

            if (Math.Abs(targetVelocity.X) > 25f || Math.Abs(targetVelocity.Y) > 25f || Math.Abs(targetVelocity.Z) > 25f)
                Logger.WriteLine("[AIMBOT] -> FormSilentTrajectory(): Running without prediction.");
            else
            {
                targetVelocity.X *= sim.TravelTime;
                targetVelocity.Y *= sim.TravelTime;
                targetVelocity.Z *= sim.TravelTime;
                endPos.Y += targetVelocity.Y + sim.DropCompensation; // Add drop compensation

                endPos.X += targetVelocity.X;
                endPos.Z += targetVelocity.Z;
            }

            return Vector3.Normalize(endPos - startPos);
        }

        private bool UpdateWeaponDirection()
        {
            Vector3 newDirection = FormTrajectory();
            if (newDirection == Vector3.Zero)
            {
                Aimbot.SilentAimPosition = Vector3.Zero;
                return false;
            }

            byte[] xBytes = BitConverter.GetBytes(newDirection.X);
            byte[] yBytes = BitConverter.GetBytes(newDirection.Y);
            byte[] zBytes = BitConverter.GetBytes(newDirection.Z);

            byte[] silentBytes = new byte[]
            {
                0xC7, 0x02, // mov [rdx], xBytes
                xBytes[0], xBytes[1], xBytes[2], xBytes[3],

                0xC7, 0x42, 0x04, // mov [rdx+4], yBytes
                yBytes[0], yBytes[1], yBytes[2], yBytes[3],

                0xC7, 0x42, 0x08, // mov [rdx+8], zBytes
                zBytes[0], zBytes[1], zBytes[2], zBytes[3],

                0x48, 0x89, 0xD0, // mov rax, rdx

                0xC3 // ret
            };

            var localPlayer = Game.LocalPlayer;
            if (localPlayer != null)
            {
                Memory.WriteValue(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.ShotNeedsFovAdjustments, false);
            }

            // Patch getter
            Memory.WriteBuffer(_saGetter, silentBytes);

            return true;
        }

        public static bool RestoreWeaponDirectionGetter(ulong saGetter)
        {
            try
            {
                return Memory.WriteBufferEnsure(saGetter, WeaponDirectionGetterOriginalBytes);
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[AIMBOT] RestoreWeaponDirectionGetter(): {ex}");
                return false;
            }
        }

        /// <summary>
        /// Gets the "EFT.Player+FirearmController:get_WeaponDirection()" method address.
        /// </summary>
        public static ulong GetGetter()
        {
            try
            {
                var mClass = MonoAPI.Class.FindOne("Assembly-CSharp", "EFT.Player+FirearmController");
                if ((ulong)mClass == 0x0)
                    throw new Exception("Unable to find \"EFT.Player+FirearmController\" class!");

                var mMethod = mClass.FindMethod("get_WeaponDirection");
                if ((ulong)mMethod == 0x0)
                    throw new Exception("Unable to find \"get_WeaponDirection\" method!");

                ulong getter = NativeHelper.CompileMethod((ulong)mMethod);

                // Validate the right method was found
                byte[] validationBytes = Memory.ReadBufferEnsure(getter, WeaponDirectionGetterOriginalBytes.Length);
                if (validationBytes == null)
                    throw new Exception("Unable to get \"get_WeaponDirection\" method address.");
                else if (validationBytes.SignatureExists(in WeaponDirectionGetterOriginalBytes) == false)
                {
                    if (validationBytes.SignatureExists(in WeaponDirectionGetterSilentBytes, "xx????xxx????xxx????xxxx") == false)
                        throw new Exception("Invalid \"get_WeaponDirection\" method address (signature mismatch).");
                }

                return getter;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[SILENT AIM] -> GetGetter(): Error while getting WeaponDirection getter method address: {ex}");
            }

            return 0x0;
        }

        public void Dispose()
        {
            Aimbot.SilentAimPosition = Vector3.Zero;
            Aimbot.SilentAimClean = RestoreWeaponDirectionGetter(_saGetter);
        }
    }
}
