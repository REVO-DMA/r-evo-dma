using apex_dma_esp.MemDMA;
using apex_dma_esp.MemDMA.ScatterAPI;
using static apex_dma_esp.Apex.ApexMath;

namespace apex_dma_esp.Apex
{
    public static class Manager
    {
        public static LocalPlayer LocalPlayer = new();
        public static ConcurrentDictionary<ulong, Player> Players = new();
        public static ConcurrentDictionary<ulong, Player> Dummies = new();
        public static bool IsInMatch = false;

        private static readonly Thread _t1;
        private static readonly Thread _t2;
        private static ulong _updateIteration = 0;

        static Manager()
        {
            _t1 = new Thread(Realtime)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _t2 = new Thread(Deferred)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };

            _t1.Start();
            _t2.Start();
        }

        public static void UpdateBaseInfo()
        {
            static void notInMatch()
            {
                IsInMatch = false;
                _updateIteration = 0;

                Logger.WriteLine("[MANAGER] -> UpdateBaseInfo(): No match detected.");
            }

            Level.Get();
            if (!Level.IsPlayable)
            {
                notInMatch();
                return;
            }

            LocalPlayer.Update();
            if (!LocalPlayer.IsCombatReady())
            {
                notInMatch();
                return;
            }
            else
            {
                IsInMatch = true;
                Logger.WriteLine($"[MANAGER] Players Count: {Players.Count}");
            }
        }

        public static void UpdateEntities()
        {
            _updateIteration++;

            var entities = EntitiesManager.Get();

            ScatterReadMap scatterMap1 = new(entities.Count);
            ScatterReadMap scatterMap2 = new(entities.Count);
            ScatterReadMap scatterMap3 = new(entities.Count);

            var round1 = scatterMap1.AddRound();
            var round2 = scatterMap1.AddRound();
            var round3 = scatterMap1.AddRound();

            var round4 = scatterMap2.AddRound();

            var round5 = scatterMap3.AddRound();

            bool[] skipIndex = new bool[entities.Count];

            // Get initial allocation data for all entities
            for (int i = 0; i < entities.Count; i++)
            {
                ulong entity = entities[i];

                round1.AddEntry<string>(i, 0, entity + Offsets.NAME, 32);

                round1.AddEntry<int>(i, 1, entity + Offsets.TEAM_NUMBER);

                // Hitbox stuff
                round1.AddEntry<ulong>(i, 2, entity + Offsets.BONES);

                var modelPtr = round1.AddEntry<MemPointer>(i, 3, entity + Offsets.STUDIOHDR);
                var studioHDR = round2.AddEntry<MemPointer>(i, 4, modelPtr, null, 0x8);
                var hitboxCache = round3.AddEntry<ushort>(i, 5, studioHDR, null, 0x34);
            }

            scatterMap1.Execute();

            // Get the rest of the hitbox info
            for (int i = 0; i < entities.Count; i++)
            {
                ulong entity = entities[i];

                if (!scatterMap1.Results[i][4].TryGetResult<MemPointer>(out var StudioHDR))
                {
                    skipIndex[i] = true;
                    continue;
                }

                if (!scatterMap1.Results[i][5].TryGetResult<ushort>(out var HitboxCache))
                {
                    skipIndex[i] = true;
                    continue;
                }

                ulong HitboxArray = StudioHDR + (uint)((ushort)(HitboxCache & 0xFFFE) << (4 * (HitboxCache & 1)));
                round4.AddEntry<ushort>(i, 1, HitboxArray, null, 0x4); // IndexCache
            }

            scatterMap2.Execute();

            // Dynamically fetch bone indexes for this entity
            for (int i = 0; i < entities.Count; i++)
            {
                if (skipIndex[i])
                    continue;

                if (!scatterMap1.Results[i][4].TryGetResult<MemPointer>(out var StudioHDR))
                {
                    skipIndex[i] = true;
                    continue;
                }

                if (!scatterMap1.Results[i][5].TryGetResult<ushort>(out var HitboxCache))
                {
                    skipIndex[i] = true;
                    continue;
                }

                if (!scatterMap2.Results[i][1].TryGetResult<ushort>(out var IndexCache))
                {
                    skipIndex[i] = true;
                    continue;
                }

                uint HitboxIndex = (uint)((ushort)(IndexCache & 0xFFFE) << (4 * (IndexCache & 1)));
                ulong HitboxArray = StudioHDR + (uint)((ushort)(HitboxCache & 0xFFFE) << (4 * (HitboxCache & 1)));

                for (int ii = 0; ii < Player.BonesCount; ii++)
                    round5.AddEntry<ushort>(i, ii, HitboxIndex + HitboxArray + (Player.BoneIndices[ii] * 0x20));
            }

            scatterMap3.Execute();

            for (int i = 0; i < entities.Count; i++)
            {
                if (skipIndex[i])
                    continue;

                ulong entity = entities[i];

                if (!scatterMap1.Results[i][0].TryGetResult<string>(out var name))
                    continue;

                if (!scatterMap1.Results[i][1].TryGetResult<int>(out var team))
                    continue;

                // Bone stuff
                if (!scatterMap1.Results[i][2].TryGetResult<ulong>(out var BoneArray))
                    continue;

                ushort[] dynamicBoneIndexes = new ushort[Player.BonesCount];
                for (int ii = 0; ii < Player.BonesCount; ii++)
                    if (scatterMap3.Results[i][ii].TryGetResult<ushort>(out var index))
                        dynamicBoneIndexes[ii] = index;

                try
                {
                    Player newPlayer = new(entity, name, team, BoneArray, dynamicBoneIndexes, _updateIteration);

                    Players.AddOrUpdate(entity, (newItem) => newPlayer, (key, existing) =>
                    {
                        // Realtime
                        newPlayer.LocalOrigin = existing.LocalOrigin;
                        newPlayer.AbsoluteVelocity = existing.AbsoluteVelocity;
                        newPlayer.BonePositions = existing.BonePositions;

                        // Deferred
                        newPlayer.IsDead = existing.IsDead;
                        newPlayer.IsKnocked = existing.IsKnocked;

                        newPlayer.IsAimedAt = existing.IsAimedAt;
                        newPlayer.LastTimeAimedAtPrevious = existing.LastTimeAimedAtPrevious;
                        newPlayer.IsVisible = existing.IsVisible;
                        newPlayer.LastTimeVisiblePrevious = existing.LastTimeVisiblePrevious;

                        newPlayer.Health = existing.Health;
                        newPlayer.MaxHealth = existing.MaxHealth;

                        newPlayer.Shield = existing.Shield;
                        newPlayer.MaxShield = existing.MaxShield;

                        return newPlayer;
                    });
                }
                catch { }
            }

            // Purge all old players
            try
            {
                foreach (var player in Players)
                {
                    if (player.Value.UpdateIteration != _updateIteration)
                        Players.TryRemove(player);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[MANAGER] -> Purge Old Players Exception: {ex}");
            }
        }

        private static void Realtime()
        {
            while (true)
            {
                if (!IsInMatch)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var players = Players.ToArray();
                    int playersLength = players.Length;

                    ScatterReadMap scatterMap = new(playersLength);
                    var scatterRound = scatterMap.AddRound(false);

                    // Add Players
                    for (int i = 0; i < playersLength; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        int currentScatterId = 0;

                        // Local Origin
                        scatterRound.AddEntry<Vector3>(i, currentScatterId++, player._Base + Offsets.LOCAL_ORIGIN);

                        // Velocity
                        scatterRound.AddEntry<Vector3>(i, currentScatterId++, player._Base + Offsets.ABSVELOCITY);

                        // Get all bone positions
                        for (uint ii = 0; ii < Player.BonesCount; ii++)
                        {
                            scatterRound.AddEntry<Matrix3x4>(i, currentScatterId++, player.GetBoneIndexAddress(ii));
                        }
                    }

                    scatterMap.Execute();

                    // Update Players
                    for (int i = 0; i < playersLength; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        int currentScatterId = 0;

                        // Location
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<Vector3>(out var origin) && scatterMap.Results[i][currentScatterId++].TryGetResult<Vector3>(out var velocity))
                        {
                            player.LocalOrigin = origin;
                            player.AbsoluteVelocity = velocity;

                            // Set all bone positions
                            for (int ii = 0; ii < Player.BonesCount; ii++)
                            {
                                if (scatterMap.Results[i][currentScatterId++].TryGetResult<Matrix3x4>(out var boneMatrix))
                                {
                                    Vector3 bonePos = player.GetBonePosition(boneMatrix);
                                    player.BonePositions[ii] = bonePos;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MANAGER] -> Realtime() Exception: {ex}");
                }

                // Update @ approx. 60 FPS
                Thread.Sleep(16);
            }
        }

        private static void Deferred()
        {
            while (true)
            {
                if (!IsInMatch)
                {
                    Thread.Sleep(100);
                    continue;
                }

                try
                {
                    var players = Players.ToArray();
                    int playersLength = players.Length;
                    ScatterReadMap scatterMap = new(playersLength);
                    var scatterRound = scatterMap.AddRound(false);

                    for (int i = 0; i < playersLength; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        int currentScatterId = 0;

                        // State
                        scatterRound.AddEntry<short>(i, currentScatterId++, player._Base + Offsets.LIFE_STATE);
                        scatterRound.AddEntry<short>(i, currentScatterId++, player._Base + Offsets.BLEEDOUT_STATE);

                        // Visibility
                        scatterRound.AddEntry<int>(i, currentScatterId++, player._Base + Offsets.LAST_AIMEDAT_TIME);
                        scatterRound.AddEntry<int>(i, currentScatterId++, player._Base + Offsets.LAST_VISIBLE_TIME);

                        // Health
                        scatterRound.AddEntry<int>(i, currentScatterId++, player._Base + Offsets.HEALTH);
                        scatterRound.AddEntry<int>(i, currentScatterId++, player._Base + Offsets.MAXHEALTH);

                        // Shield
                        scatterRound.AddEntry<int>(i, currentScatterId++, player._Base + Offsets.SHIELD);
                        scatterRound.AddEntry<int>(i, currentScatterId++, player._Base + Offsets.MAXSHIELD);
                    }

                    scatterMap.Execute();

                    for (int i = 0; i < playersLength; i++)
                    {
                        var player = players[i].Value;

                        if (player is null)
                            continue;

                        int currentScatterId = 0;

                        // State
                        if (player.IsDummy())
                        {
                            player.IsDead = false;
                            player.IsKnocked = false;
                            
                            currentScatterId++;
                            currentScatterId++;
                        }
                        else
                        {
                            if (scatterMap.Results[i][currentScatterId++].TryGetResult<short>(out var lifeState))
                                player.IsDead = lifeState > 0;

                            if (scatterMap.Results[i][currentScatterId++].TryGetResult<short>(out var bleedOutState))
                                player.IsKnocked = bleedOutState > 0;
                        }

                        // Visibility
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var lastTimeAimedAt))
                        {
                            player.IsAimedAt = player.LastTimeAimedAtPrevious < lastTimeAimedAt;
                            player.LastTimeAimedAtPrevious = lastTimeAimedAt;
                        }
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var lastVisibleTime))
                        {
                            player.IsVisible = player.IsDummy() || player.IsAimedAt || player.LastTimeVisiblePrevious < lastVisibleTime;
                            player.LastTimeVisiblePrevious = lastVisibleTime;
                        }

                        // Health
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var health))
                            player.Health = health;
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var maxHealth))
                            player.MaxHealth = maxHealth;

                        // Shield
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var shield))
                            player.Shield = shield;
                        if (scatterMap.Results[i][currentScatterId++].TryGetResult<int>(out var maxShield))
                            player.MaxShield = maxShield;
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine($"[MANAGER] -> Deferred() Exception: {ex}");
                }

                Thread.Sleep(250);
            }
        }
    }
}
