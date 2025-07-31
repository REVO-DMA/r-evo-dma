namespace apex_dma_esp.Apex
{
    internal static class Offsets
    {
        // Core
        public const uint LEVEL = 0x1690670;                        // [Miscellaneous]->LevelName
        public const uint LOCAL_PLAYER = 0x2119848;                 // [Miscellaneous]->LocalPlayer
        public const uint ENTITY_LIST = 0x1d6b5d8;                  // [Miscellaneous]->cl_entitylist
        public const uint INPUT_SYSTEM = 0x170f080;

        // HUD
        public const uint VIEWRENDER = 0x73828a0;                   // [Miscellaneous]->ViewRenderer
        public const uint VIEWMATRIX = 0x11a350;                    // [Miscellaneous]->ViewMatrix

        // Player
        public const uint HEALTH = 0x036c;                          // [RecvTable.DT_Player]->m_iHealth
        public const uint MAXHEALTH = 0x04a8;                       // [RecvTable.DT_Player]->m_iMaxHealth
        public const uint SHIELD = 0x01a0;                          // [RecvTable.DT_TitanSoul]->m_shieldHealth
        public const uint MAXSHIELD = 0x01a4;                       // [RecvTable.DT_TitanSoul]->m_shieldHealthMax

        public const uint INATTACK = 0x07383af0;                    // [Buttons]->in_attack

        public const uint CAMERAORIGIN = 0x1f00;                    // [Miscellaneous]->CPlayer!camera_origin
        public const uint STUDIOHDR = 0x1020;                       // [Miscellaneous]->CBaseAnimating!m_pStudioHdr
        public const uint BONES = 0x0dd0 + 0x48;                    // m_nForceBone

        public const uint LOCAL_ORIGIN = 0x017c;                    // [DataMap.C_BaseEntity]->m_vecAbsOrigin
        public const uint ABSVELOCITY = 0x0170;                     // [DataMap.C_BaseEntity]->m_vecAbsVelocity

        public const uint ZOOMING = 0x1c01;                         // [RecvTable.DT_Player]->m_bZooming
        public const uint TEAM_NUMBER = 0x037c;                     // [RecvTable.DT_BaseEntity]->m_iTeamNum
        public const uint NAME = 0x04b9;                            // [RecvTable.DT_BaseEntity]->m_iName
        public const uint LIFE_STATE = 0x06c8;                      // [RecvTable.DT_Player]->m_lifeState
        public const uint BLEEDOUT_STATE = 0x2710;                  // [RecvTable.DT_Player]->m_bleedoutState  
        public const uint LAST_VISIBLE_TIME = 0x19bd + 0x3;         // [RecvTable.DT_BaseCombatCharacter]->m_hudInfo_visibilityTestAlwaysPasses + 0x3
        public const uint LAST_AIMEDAT_TIME = 0x19bd + 0x3 + 0x8;   // [RecvTable.DT_BaseCombatCharacter]->m_hudInfo_visibilityTestAlwaysPasses + 0x3 + 0x8
        public const uint VIEW_ANGLES = 0x2564 - 0x14;              // [DataMap.C_Player]-> m_ammoPoolCapacity - 0x14
        public const uint PUNCH_ANGLES = 0x2468;                    // [DataMap.C_Player]->m_currentFrameLocalPlayer.m_vecPunchWeapon_Angle

        // Weapon
        public const uint WEAPON_HANDLE = 0x1964;                   // [RecvTable.DT_Player]->m_latestPrimaryWeapons
        public const uint WEAPON_INDEX = 0x17a8;                    // [RecvTable.DT_WeaponX]->m_weaponNameIndex
        public const uint PROJECTILESCALE = 0x1EC4;                 // projectile_gravity_scale + [WeaponSettingsMeta]base
        public const uint PROJECTILESPEED = 0x1EBC;                 // projectile_launch_speed + [WeaponSettingsMeta]base
        public const uint OFFHAND_WEAPON = 0x1974;                  // m_latestNonOffhandWeapons
        public const uint CURRENTZOOMFOV = 0x1600 + 0x00b8;         // m_playerData + m_curZoomFOV
        public const uint TARGETZOOMFOV = 0x1600 + 0x00bc;          // m_playerData + m_targetZoomFOV

        // Glow
        public const uint GLOW_ENABLE = 0x294;                      // Script_Highlight_GetCurrentContext
        public const uint GLOW_THROUGH_WALL = 0x278;                // Script_Highlight_SetVisibilityType
        public const uint GLOW_FIX = 0x270;
        public const uint GLOW_HIGHLIGHT_ID = 0x298;                // [DT_HighlightSettings].m_highlightServerActiveStates    
        public const uint GLOW_HIGHLIGHTS = 0xB5C5090;              // [?]->?
    }
}
