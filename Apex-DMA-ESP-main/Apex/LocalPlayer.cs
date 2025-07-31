namespace apex_dma_esp.Apex
{
    public class LocalPlayer
    {
        public ulong _Base;

        public bool IsDead;
        public bool IsInAttack;
        public bool IsKnocked;
        public bool IsZooming;

        public int Team;
        public Vector3 LocalOrigin;
        public Vector3 CameraPosition;

        public Vector2 ViewAngles;
        public Vector2 PunchAngles;
        public Vector2 PunchAnglesPrevious;
        public Vector2 PunchAnglesDifferent;

        public int WeaponIndex;
        public float WeaponProjectileSpeed;
        public float WeaponProjectileScale;

        public void ResetPointer()
        {
            _Base = 0;
        }

        public void Update()
        {
            try
            {
                _Base = Memory.ReadValue<ulong>(Memory.ModuleBase + Offsets.LOCAL_PLAYER);
                if (_Base == 0) return;

                IsDead = Memory.ReadValue<short>(_Base + Offsets.LIFE_STATE) > 0;
                IsKnocked = Memory.ReadValue<short>(_Base + Offsets.BLEEDOUT_STATE) > 0;
                IsZooming = Memory.ReadValue<short>(_Base + Offsets.ZOOMING) > 0;
                IsInAttack = Memory.ReadValue<short>(Memory.ModuleBase + Offsets.INATTACK) > 0;

                Team = Memory.ReadValue<int>(_Base + Offsets.TEAM_NUMBER);
                LocalOrigin = Memory.ReadValue<Vector3>(_Base + Offsets.LOCAL_ORIGIN);
                CameraPosition = Memory.ReadValue<Vector3>(_Base + Offsets.CAMERAORIGIN);
                ViewAngles = Memory.ReadValue<Vector2>(_Base + Offsets.VIEW_ANGLES);
                PunchAngles = Memory.ReadValue<Vector2>(_Base + Offsets.PUNCH_ANGLES);
                PunchAnglesDifferent = Vector2.Subtract(PunchAnglesPrevious, PunchAngles);
                PunchAnglesPrevious = PunchAngles;

                if (!IsDead && !IsKnocked)
                {
                    ulong WeaponHandle = Memory.ReadValue<ulong>(_Base + Offsets.WEAPON_HANDLE);
                    ulong WeaponHandleMasked = WeaponHandle & 0xffff;
                    ulong WeaponEntity = Memory.ReadValue<ulong>(Memory.ModuleBase + Offsets.ENTITY_LIST + (WeaponHandleMasked << 5));
                    WeaponIndex = Memory.ReadValue<int>(WeaponEntity + Offsets.WEAPON_INDEX);
                    WeaponProjectileSpeed = Memory.ReadValue<float>(WeaponEntity + Offsets.PROJECTILESPEED);
                    WeaponProjectileScale = Memory.ReadValue<float>(WeaponEntity + Offsets.PROJECTILESCALE);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[LOCAL PLAYER] Error updating local player: {ex}");
            }
        }

        public bool IsValid()
        {
            return _Base != 0;
        }

        public bool IsCombatReady()
        {
            if (_Base == 0 || IsDead || IsKnocked) return false;
            return true;
        }
    }
}
