using NickBuhro.Translit;
using Tarkov_DMA_Backend.Misc;
using Tarkov_DMA_Backend.Unity.LowLevel;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;

namespace Tarkov_DMA_Backend.Tarkov
{
    public static class MiscUtils
    {
        public static byte[] GetAESKey(MonoClass gameAPIClient)
        {
            try
            {
                ulong gameAPIClientAddress = gameAPIClient.GetVTable(MonoLibrary.GetRootDomain()).GetStaticFieldData();
                if (!MemoryUtils.IsValidAddress(gameAPIClientAddress))
                    throw new Exception("Invalid gameAPIClientAddress");

                ulong aesKeyMonoString = Memory.ReadPtr(gameAPIClientAddress + 0x0, false);

                var aesKey = RemoteBytes.RemoteMonoString.Get(aesKeyMonoString);

                if (aesKey.Length != 24)
                    throw new Exception("Unexpected key length!");

                Logger.WriteLine($"Got Tarkov AES Key: \"{Encoding.UTF8.GetString(aesKey.Data)}\"!");

                // Game does it, and so do we!
                byte[] tmpAES = aesKey.Data;
                int num = tmpAES.Length % 8;
                if (num != 0) Array.Resize(ref tmpAES, tmpAES.Length + (8 - num));

                return tmpAES;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"Error getting AES Key: {ex}");
            }

            return null;
        }
    }

    public enum PlayerType
    {
        Default,
        LocalPlayer,
        Teammate,
        EnemyPMC,
        PlayerScav,
        AI_PMC,
        Boss,
        BossGuard,
        CultistBoss,
        Cultist,
        Raider,
        Rogue,
        Scav,
        Santa,
        BTR,
        Event,
        Corpse,
        AimbotLocked,
    }

    /// <summary>
    /// Defines Role for an AI Bot Player.
    /// </summary>
    public readonly struct AIRole
    {
        /// <summary>
        /// Name of Bot Player.
        /// </summary>
        public readonly string Name { get; init; }
        /// <summary>
        /// Type of Bot Player.
        /// </summary>
        public readonly PlayerType Type { get; init; }
    }

    public static class AIRoleMisc
    {
        private static bool IsBoss(Enums.WildSpawnType type)
        {
            if (type is Enums.WildSpawnType.bossBully or        // Reshala
                Enums.WildSpawnType.bossKilla or                // Killa
                Enums.WildSpawnType.bossKojaniy or              // Shturman
                Enums.WildSpawnType.bossGluhar or               // Glukhar
                Enums.WildSpawnType.bossSanitar or              // Sanitar
                Enums.WildSpawnType.bossTagilla or              // Tagilla
                Enums.WildSpawnType.followerTagilla or          // Tagilla
                Enums.WildSpawnType.bossKnight or               // Knight
                Enums.WildSpawnType.followerBigPipe or          // Big Pipe
                Enums.WildSpawnType.followerBirdEye or          // Birdeye
                Enums.WildSpawnType.bossZryachiy or             // Zryachiy
                Enums.WildSpawnType.peacefullZryachiyEvent or   // Zryachiy
                Enums.WildSpawnType.ravangeZryachiyEvent or     // Zryachiy
                Enums.WildSpawnType.bossBoar or                 // Kaban
                Enums.WildSpawnType.bossKolontay or             // Kollontay
                Enums.WildSpawnType.bossPartisan)               // Partisan
            {
                return true;
            }

            return false;
        }

        private static bool IsGuard(Enums.WildSpawnType type)
        {
            if (type is Enums.WildSpawnType.followerBully or      // Reshala
                Enums.WildSpawnType.followerKojaniy or            // Shturman
                Enums.WildSpawnType.followerGluharAssault or      // Glukhar
                Enums.WildSpawnType.followerGluharScout or        // Glukhar
                Enums.WildSpawnType.followerGluharSecurity or     // Glukhar
                Enums.WildSpawnType.followerGluharSnipe or        // Glukhar
                Enums.WildSpawnType.followerSanitar or            // Sanitar
                Enums.WildSpawnType.followerBoar or               // Kaban
                Enums.WildSpawnType.followerBoarClose1 or         // Kaban
                Enums.WildSpawnType.followerBoarClose2 or         // Kaban
                Enums.WildSpawnType.bossBoarSniper or             // Kaban
                Enums.WildSpawnType.followerKolontayAssault or    // Kollontay
                Enums.WildSpawnType.followerKolontaySecurity)     // Kollontay
            {
                return true;
            }

            return false;
        }

        private static bool IsScav(Enums.WildSpawnType type)
        {
            if (type is Enums.WildSpawnType.assault or
                Enums.WildSpawnType.assaultGroup or
                Enums.WildSpawnType.cursedAssault or
                Enums.WildSpawnType.crazyAssaultEvent)
            {
                return true;
            }

            return false;
        }

        public static AIRole GetRole(string name, Enums.WildSpawnType type)
        {
            static void LogUndetectedAI(string name, string latin, Enums.WildSpawnType type)
            {
                Logger.WriteLine($"=========================== Undetected AI - Name: \"{name}\" Latin: \"{latin}\" Type: \"{type}\"");
            }

            string latin = Transliteration.CyrillicToLatin(name, Language.Russian);

            if (IsBoss(type))
            {
                return new AIRole()
                {
                    Name = latin,
                    Type = PlayerType.Boss
                };
            }
            else if (IsGuard(type))
            {
                return new AIRole()
                {
                    Name = "Guard",
                    Type = PlayerType.BossGuard
                };
            }
            else if (IsScav(type))
            {
                return new AIRole()
                {
                    Name = "Scav",
                    Type = PlayerType.Scav
                };
            }
            else
            {
                switch (type)
                {
                    case Enums.WildSpawnType.marksman:
                        return new AIRole()
                        {
                            Name = "Sniper",
                            Type = PlayerType.Scav
                        };
                    case Enums.WildSpawnType.sectantPriest:
                        return new AIRole()
                        {
                            Name = "Cultist Boss",
                            Type = PlayerType.CultistBoss
                        };
                    case Enums.WildSpawnType.sectantWarrior:
                        return new AIRole()
                        {
                            Name = "Cultist",
                            Type = PlayerType.Cultist
                        };
                    case Enums.WildSpawnType.followerZryachiy:
                        return new AIRole()
                        {
                            Name = "Cultist",
                            Type = PlayerType.Cultist
                        };
                    case Enums.WildSpawnType.pmcBot:
                        return new AIRole()
                        {
                            Name = "Raider",
                            Type = PlayerType.Raider
                        };
                    case Enums.WildSpawnType.exUsec:
                        return new AIRole()
                        {
                            Name = "Rogue",
                            Type = PlayerType.Rogue
                        };
                    case Enums.WildSpawnType.arenaFighterEvent:
                        return new AIRole()
                        {
                            Name = "Bloodhound",
                            Type = PlayerType.Event
                        };
                    case Enums.WildSpawnType.arenaFighter:
                        return new AIRole()
                        {
                            Name = "Arena Fighter",
                            Type = PlayerType.Event
                        };
                    case Enums.WildSpawnType.skier:
                        return new AIRole()
                        {
                            Name = "Skier Goon",
                            Type = PlayerType.Event
                        };
                    case Enums.WildSpawnType.peacemaker:
                        return new AIRole()
                        {
                            Name = "Peacemaker Goon",
                            Type = PlayerType.Event
                        };
                    case Enums.WildSpawnType.shooterBTR:
                        return new AIRole()
                        {
                            Name = "BTR",
                            Type = PlayerType.BTR
                        };
                    case Enums.WildSpawnType.gifter:
                        return new AIRole()
                        {
                            Name = "Santa",
                            Type = PlayerType.Santa
                        };
                    case Enums.WildSpawnType.pmcBEAR:
                        return new AIRole()
                        {
                            Name = "AI BEAR",
                            Type = PlayerType.AI_PMC
                        };
                    case Enums.WildSpawnType.pmcUSEC:
                        return new AIRole()
                        {
                            Name = "AI USEC",
                            Type = PlayerType.AI_PMC
                        };
                }
            }

            // Undetected AI - Log and return default
            LogUndetectedAI(name, latin, type);
            return new()
            {
                Name = type.ToString(),
                Type = PlayerType.Default
            };
        }
    }

    /// <summary>
    /// Bones Index for Player Transforms.
    /// </summary>
    public enum Bone
    {
        None = -999,
        HumanBase = 0,
        HumanPelvis = 14,
        HumanLThigh1 = 15,
        HumanLThigh2 = 16,
        HumanLCalf = 17,
        HumanLFoot = 18,
        HumanLToe = 19,
        HumanRThigh1 = 20,
        HumanRThigh2 = 21,
        HumanRCalf = 22,
        HumanRFoot = 23,
        HumanRToe = 24,
        HumanSpine1 = 29,
        HumanSpine2 = 36,
        HumanSpine3 = 37,
        HumanLCollarbone = 89,
        HumanLUpperarm = 90,
        HumanLForearm1 = 91,
        HumanLForearm2 = 92,
        HumanLForearm3 = 93,
        HumanLPalm = 94,
        HumanRCollarbone = 110,
        HumanRUpperarm = 111,
        HumanRForearm1 = 112,
        HumanRForearm2 = 113,
        HumanRForearm3 = 114,
        HumanRPalm = 115,
        HumanNeck = 132,
        HumanHead = 133
    };
}
