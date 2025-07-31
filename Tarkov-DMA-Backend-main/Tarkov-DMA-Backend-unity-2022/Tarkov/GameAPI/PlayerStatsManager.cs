using System.Timers;
using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Misc;
using Timer = System.Timers.Timer;

namespace Tarkov_DMA_Backend.Tarkov.GameAPI
{
    public static class PlayerStatsManager
    {
        private const int StatsLookupInterval = 2500;
        private const int MaxRetries = 4;

        private static readonly ConcurrentDictionary<string, PlayerStats> StatsCache = new();
        private static readonly ConcurrentQueue<StatsLookupQueueEntry> StatsLookupQueue = new();
        private static readonly Timer StatsLookupTimer = new(StatsLookupInterval);

        public struct StatsLookupQueueEntry(ulong playerBase, string accountID)
        {
            public readonly string AccountID = accountID;
            public readonly ulong PlayerBase = playerBase;

            public int Retries = 0;
        }

        public readonly struct PlayerStats(string name, string level, string accountType, string onlineTime, string killDeathRatio, string survivalRate)
        {
            public readonly DateTime TimeStamp = DateTime.UtcNow;

            public readonly string Name = name;
            public readonly string Level = level;
            public readonly string AccountType = accountType;
            public readonly string OnlineTime = onlineTime;
            public readonly string KillDeathRatio = killDeathRatio;
            public readonly string SurvivalRate = survivalRate;
        }

        static PlayerStatsManager()
        {
            StatsLookupTimer.Elapsed += StatsLookupWorker;
            StatsLookupTimer.Start();
        }

        public static PlayerStats? QueueLookup(string accountID, ulong baseAddr)
        {
            // See if this can be gotten from the cache
            if (StatsCache.TryGetValue(accountID, out var stats))
            {
                // Cache stats for X mins
                if (DateTime.UtcNow < stats.TimeStamp.AddMinutes(30))
                    return stats;
            }

            // Check if this player is already queued
            if (StatsLookupQueue.Any(x => x.AccountID == accountID))
                return null;

            // This player is not present in the cache or queued -> queue lookup
            StatsLookupQueue.Enqueue(new(baseAddr, accountID));

            return null;
        }

        private static void StatsLookupWorker(object source, ElapsedEventArgs e)
        {
            if (!StatsLookupQueue.TryDequeue(out var queueEntry))
                return;

            try
            {
                if (EFTDMA.Players.TryGetValue(queueEntry.PlayerBase, out var player))
                {
                    PlayerStats playerStats = Get(queueEntry.AccountID);

                    player.SetStats(playerStats);
                }
            }
            catch (Exception ex)
            {
                // If this lookup failed try to requeue
                if (queueEntry.Retries > MaxRetries)
                {
                    Logger.WriteLine($"[PLAYER STATS MANAGER] -> StatsLookupWorker(): Max retries reached for player at \"0x{queueEntry.PlayerBase:X}\"!");
                    return;
                }

                queueEntry.Retries++;

                StatsLookupQueue.Enqueue(queueEntry);

                Logger.WriteLine($"[PLAYER STATS MANAGER] -> StatsLookupWorker(): {ex}");
            }
        }

        public static PlayerStats Get(string accountID)
        {
            static string CreatePostBody(string accountId)
            {
                return "{" + $"\"accountId\":\"{accountId}\"" + "}";
            }

            static string kdFromCounters(PlayerProfile.OverAllCounters counters)
            {
                int kills, deaths;

                var killsRaw = counters.Items.FirstOrDefault(x => x.Key.Contains("Kills"));
                if (killsRaw == null) kills = 0;
                else kills = killsRaw.Value;

                var deathsRaw = counters.Items.FirstOrDefault(x => x.Key.Contains("Deaths"));
                if (deathsRaw == null) deaths = 0;
                else deaths = deathsRaw.Value;

                if (deaths == 0) return kills.ToString();

                return $"{((float)kills / (float)deaths):F1}";
            }

            static string srFromCounters(PlayerProfile.OverAllCounters counters)
            {
                int sessions, survives;

                var sessionsRaw = counters.Items.FirstOrDefault(x => x.Key.Contains("Sessions"));
                if (sessionsRaw == null) sessions = 0;
                else sessions = sessionsRaw.Value;

                var survivesRaw = counters.Items.FirstOrDefault(x => x.Key.Contains("Survived"));
                if (survivesRaw == null) survives = 0;
                else survives = survivesRaw.Value;

                if (sessions == 0) return "100%";

                return $"{((float)survives / (float)sessions * 100f):F1}%";
            }

            static string GetAccountType(int memberCategory)
            {
                Enums.EMemberCategory emc = (Enums.EMemberCategory)memberCategory;

                if (emc != Enums.EMemberCategory.Default && emc != Enums.EMemberCategory.UniqueId && emc != Enums.EMemberCategory.Unheard)
                    return emc.ToString("G").Replace("UniqueId", "EOD"); // Return all flags that are set
                else if (emc == Enums.EMemberCategory.Default)
                    return "Default";
                else if (emc == Enums.EMemberCategory.UniqueId)
                    return "EOD";
                else if (emc == Enums.EMemberCategory.Unheard)
                    return "Unheard";

                return string.Empty;
            }

            Dictionary<string, string> headers = new()
            {
                { "User-Agent", $"UnityPlayer/{GameData.UNITY_VERSION} (UnityWebRequest/1.0, libcurl/7.80.0-DEV)" },
                { "Cookie", $"PHPSESSID={EFTDMA.SessionToken}" },
                { "App-Version", $"EFT Client {ClassNames.GameVersion.Value}" },
                { "GClient-RequestId", NetworkContainer.GetNextRequestIndex().ToString() },
                { "X-Unity-Version", GameData.UNITY_VERSION },
                { "Accept", "*/*" },
            };

            string postBody = CreatePostBody(accountID);

            string responseString = GameApiClient.Post(GameData.GameHostname, "/client/profile/view", headers, postBody);

            PlayerProfile.RootObject playerProfile = JSON.DeserializePlayerProfile(responseString);
            if (playerProfile.err != 0)
            {
                Logger.WriteLine($"[PLAYER STATS MANAGER]: Get() -> BSG API returned a non-zero error code ~ {playerProfile.err}");
                Logger.WriteLine($"============BSG API Data dump============\n");
                Logger.WriteLine(responseString);
                Logger.WriteLine($"\n==========END BSG API Data dump==========");

                throw new Exception($"BSG API returned a non-zero error code: {playerProfile.err}");
            }

            string name = playerProfile.data.info.nickname;
            string level = Player.GetLevel(playerProfile.data.info.experience);
            string accountType = GetAccountType(playerProfile.data.info.memberCategory);
            string onlineTime = $"{(playerProfile.data.pmcStats.eft.totalInGameTime / 3600):F0}";
            string kd = kdFromCounters(playerProfile.data.pmcStats.eft.overAllCounters);
            string sr = srFromCounters(playerProfile.data.pmcStats.eft.overAllCounters);

            PlayerStats playerStats = new(name, level, accountType, onlineTime, kd, sr);

            // Update cache
            StatsCache.AddOrUpdate(accountID, (newItem) => playerStats, (key, existing) => playerStats);

            return playerStats;
        }
    }
}
