using Tarkov_DMA_Backend.Misc;
using static Tarkov_DMA_Backend.Unity.LowLevel.MonoAPI;

namespace Tarkov_DMA_Backend.Tarkov
{
    public static class NetworkContainer
    {
        private static ulong NetworkContainerAddress = 0x0;

        public static bool TryLoad(MonoClass networkContainer)
        {
            try
            {
                NetworkContainerAddress = networkContainer.GetVTable(MonoLibrary.GetRootDomain()).GetStaticFieldData();
                if (!MemoryUtils.IsValidAddress(NetworkContainerAddress))
                    throw new Exception("Invalid NetworkContainerAddress");
                else
                    Logger.WriteLine($"[NETWORK CONTAINER] -> Initialize(): got NetworkContainer @ 0x{NetworkContainerAddress:X}");

                return true;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[NETWORK CONTAINER] -> Initialize(): Error loading ~ {ex}");
            }

            return false;
        }

        public static string GetSessionToken(bool cached = true)
        {
            try
            {
                ulong phpSessionIdPtr = Memory.ReadPtr(NetworkContainerAddress + Offsets.NetworkContainer.PhpSessionId, cached);
                string phpSessionId = Memory.ReadUnityString(phpSessionIdPtr, cached, 128);

                // Check the format
                if (phpSessionId[0] == 's' && phpSessionId[1] == 'h' && phpSessionId[3] == '-')
                {
                    Logger.WriteLine($"Got Tarkov PhpSessionId: \"{phpSessionId}\"!");
                    return phpSessionId;
                }
                else
                    throw new Exception("Session token was in an invalid format!");
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[NETWORK CONTAINER] -> GetSessionToken(): {ex}");
            }

            return null;
        }

        public static long GetNextRequestIndex()
        {
            try
            {
                var nextRequestIndex = Memory.ReadValue<long>(NetworkContainerAddress + Offsets.NetworkContainer.NextRequestIndex, false);

                Logger.WriteLine($"[NETWORK CONTAINER] -> GetNextRequestIndex(): {nextRequestIndex}");

                return nextRequestIndex;
            }
            catch (Exception ex)
            {
                Logger.WriteLine($"[NETWORK CONTAINER] -> GetNextRequestIndex(): {ex}");
                return Random.Shared.Next(69, 420);
            }
        }
    }
}
