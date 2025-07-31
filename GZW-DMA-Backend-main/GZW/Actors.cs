using gzw_dma_backend.DMA.ScatterAPI;

namespace gzw_dma_backend.GZW
{
    public static class Actors
    {
        public readonly struct Actor
        {
            public readonly ulong Address;
            public readonly uint ID;
            public readonly string Name;

            public Actor(ulong address, uint id, string name)
            {
                Address = address;
                ID = id;
                Name = name;
            }
        }

        public static IReadOnlyList<Actor> GetList(ulong listBase, int count)
        {
            // Get actors
            var map1 = new ScatterReadMap(count);
            var round1 = map1.AddRound();

            for (int i = 0; i < count; i++)
                round1.AddEntry<MemPointer>(i, 0, listBase + ((uint)i * 0x8));

            map1.Execute();

            bool[] skipIndex = new bool[count];

            // Get actor IDs
            var map2 = new ScatterReadMap(count);
            var round2 = map2.AddRound();

            for (int i = 0; i < count; i++)
            {
                if (map1.Results[i][0].TryGetResult<MemPointer>(out var actor) && actor.ValidateEx())
                    round2.AddEntry<uint>(i, 0, actor + Offsets.UObject.ID);
                else
                    skipIndex[i] = true;
            }

            map2.Execute();

            string[] cachedFNames = new string[count];

            // Get Actor Names - p1
            var map3 = new ScatterReadMap(count);
            var round3 = map3.AddRound();

            for (int i = 0; i < count; i++)
            {
                if (skipIndex[i])
                    continue;

                if (map2.Results[i][0].TryGetResult<uint>(out var actorID))
                {
                    string cachedName = FNamePool.GetFNameFromCache(actorID);
                    if (cachedName != null)
                    {
                        cachedFNames[i] = cachedName;
                        continue;
                    }

                    round3.AddEntry<ulong>(i, 0, FNamePool.GetNamePoolChunk(actorID));
                }
                else
                    skipIndex[i] = true;
            }

            map3.Execute();

            // Get Actor Names - p2
            var map4 = new ScatterReadMap(count);
            var round4 = map4.AddRound();

            for (int i = 0; i < count; i++)
            {
                if (skipIndex[i] || cachedFNames[i] != null)
                    continue;

                if (map2.Results[i][0].TryGetResult<uint>(out var actorID) &&
                    map3.Results[i][0].TryGetResult<ulong>(out var namePoolChunk))
                {
                    // namePoolChunk

                    ulong fNameEntry = FNamePool.GetFNameEntry(actorID, namePoolChunk);

                    round4.AddEntry<short>(i, 0, fNameEntry);
                }
                else
                    skipIndex[i] = true;
            }

            map4.Execute();

            // Get Actor Names - p3
            var map5 = new ScatterReadMap(count);
            var round5 = map5.AddRound();

            for (int i = 0; i < count; i++)
            {
                if (skipIndex[i] || cachedFNames[i] != null)
                    continue;

                if (map2.Results[i][0].TryGetResult<uint>(out var actorID) &&
                    map3.Results[i][0].TryGetResult<ulong>(out var namePoolChunk) &&
                    map4.Results[i][0].TryGetResult<short>(out var fNameEntryHeader))
                {
                    ulong fNameEntry = FNamePool.GetFNameEntry(actorID, namePoolChunk);
                    var stringData = FNamePool.GetStringData(fNameEntry, fNameEntryHeader);

                    round5.AddEntry<string>(i, 0, stringData.StrPtr, stringData.StrLength);
                }
                else
                    skipIndex[i] = true;
            }

            map5.Execute();

            // Create actor list
            List<Actor> actors = new();

            for (int i = 0; i < count; i++)
            {
                if (skipIndex[i])
                    continue;

                if (map1.Results[i][0].TryGetResult<MemPointer>(out var actor) &&
                    map2.Results[i][0].TryGetResult<uint>(out var actorID))
                {
                    if (cachedFNames[i] != null)
                    {
                        actors.Add(new(actor, actorID, cachedFNames[i]));
                    }
                    else if (map5.Results[i][0].TryGetResult<string>(out var actorName))
                    {
                        actors.Add(new(actor, actorID, actorName));
                        FNamePool.AddFNameToCache(actorID, actorName);
                    }
                }
            }

            return actors;
        }

        public readonly struct PlayerActor(ulong address, ulong playerState, ulong mesh, ulong boneArray, byte myTeamID, ulong healthSystemComponent, bool isAI)
        {
            public readonly ulong Address = address;
            public readonly ulong PlayerState = playerState;
            public readonly ulong Mesh = mesh;
            public readonly ulong BoneArray = boneArray;
            public readonly byte MyTeamID = myTeamID;
            public readonly ulong HealthSystemComponent = healthSystemComponent;
            public readonly bool IsAI = isAI;
        }

        public readonly struct ProcessedActors(List<PlayerActor> players)
        {
            public readonly IReadOnlyList<PlayerActor> Players = players;
        }

        public static IReadOnlyList<PlayerActor> ProcessList(IReadOnlyList<Actor> actors)
        {
            List<PlayerActor> players = new();

            int count = actors.Count;

            bool[] skipIndex = new bool[count];

            var map = new ScatterReadMap(count);
            var round1 = map.AddRound();
            var round2 = map.AddRound();

            for (int i = 0; i < count; i++)
            {
                var actor = actors[i];

                if (actor.Name != "BP_MFG_Soldier_C")
                {
                    skipIndex[i] = true;
                    continue;
                }

                int currentScatterId = 0;

                round1.AddEntry<ulong>(i, currentScatterId++, actor.Address + Offsets.APawn.PlayerState);
                var mesh = round1.AddEntry<MemPointer>(i, currentScatterId++, actor.Address + Offsets.ACharacter.Mesh);
                round2.AddEntry<ulong>(i, currentScatterId++, mesh, offset: Offsets.ACharacter.BoneArray);
                round1.AddEntry<byte>(i, currentScatterId++, actor.Address + Offsets.AMFGGameCharacter.MyTeamID + Offsets.FGenericTeamId.TeamID);
                round1.AddEntry<MemPointer>(i, currentScatterId++, actor.Address + Offsets.AMFGGameCharacter.HealthSystemComponent);
                round1.AddEntry<bool>(i, currentScatterId++, actor.Address + Offsets.AMFGGameCharacter.IsAI);
            }

            map.Execute();

            for (int i = 0; i < count; i++)
            {
                if (skipIndex[i])
                    continue;

                var actor = actors[i];

                int currentScatterId = 0;

                if (map.Results[i][currentScatterId++].TryGetResult<ulong>(out var playerState) &&
                    map.Results[i][currentScatterId++].TryGetResult<MemPointer>(out var mesh) && mesh.ValidateEx() &&
                    map.Results[i][currentScatterId++].TryGetResult<ulong>(out var boneArray) &&
                    map.Results[i][currentScatterId++].TryGetResult<byte>(out var teamID) &&
                    map.Results[i][currentScatterId++].TryGetResult<MemPointer>(out var healthSystemComponent) && healthSystemComponent.ValidateEx() &&
                    map.Results[i][currentScatterId++].TryGetResult<bool>(out var isAI))
                {
                    players.Add(new(actor.Address, playerState, mesh, boneArray, teamID, healthSystemComponent, isAI));
                }
            }

            return players;
        }
    }
}
