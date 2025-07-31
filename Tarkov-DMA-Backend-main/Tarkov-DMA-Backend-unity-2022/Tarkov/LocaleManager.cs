using Tarkov_DMA_Backend.MemDMA.EFT;
using Tarkov_DMA_Backend.Unity.Collections;
using Tarkov_DMA_Backend.Unity.LowLevel;

namespace Tarkov_DMA_Backend.Tarkov
{
    public static class LocaleManager
    {
        public static Dictionary<string, string> Locale = new();

        /// <summary>
        /// May need to be updated in the future as the game gets more strings.
        /// </summary>
        private const int MaximumCount = 50000;
        /// <summary>
        /// May need to be updated in the future as the game gets more strings.
        /// </summary>
        private const int MinimumCount = 20000;

        public static bool TryLoad()
        {
			try
			{
                ulong instance = Memory.ReadPtr(EFTDMA.MonoClasses.GetStaticClass(ClassNames.LocaleManager.ClassName_ClassToken) + Offsets.LocaleManager.Instance, false);

                ulong currentCultureAddr = Memory.ReadPtr(instance + Offsets.LocaleManager.CurrentCulture, false);
                string currentCulture = Memory.ReadUnityString(currentCultureAddr, false);

                ulong dictionary = Memory.ReadPtr(instance + Offsets.LocaleManager.LocaleDictionary, false);
                MemDictionary<ulong, ulong> localizations = new(dictionary, false);

                foreach (var localization in localizations.Items)
                {
                    string language = Memory.ReadUnityString(localization.Key, false);
                    if (language != currentCulture)
                        continue;

                    MemDictionary<ulong, ulong> englishLocale = new(localization.Value, false, MaximumCount);
                    Dictionary<ulong, string> keys = Memory.ScatterUnityStrings(englishLocale.Items.Keys);
                    Dictionary<ulong, string> values = Memory.ScatterUnityStrings(englishLocale.Items.Values);

                    foreach (var enItem in englishLocale.Items)
                    {
                        if (!keys.TryGetValue(enItem.Key, out string key) ||
                            !values.TryGetValue(enItem.Value, out string value))
                        {
                            continue;
                        }

                        Locale.TryAdd(key, value);
                    }
                }

                // Sanity check
                if (Locale.Count < MinimumCount)
                {
                    Logger.WriteLine($"[LOCALE MANAGER] -> TryLoad(): {Locale.Count} does not fall within the valid range!");
                    return false;
                }

                Logger.WriteLine($"[LOCALE MANAGER] -> TryLoad(): Loaded {Locale.Count} items!");

                //ulong itemTemplates = Memory.ReadPtr(Memory.ReadPtr(MonoAPI.SingletonClasses.ItemsManager) + 0x10);
                //MemDictionary<ulong, ulong> items = new(itemTemplates, false);
                //var itemIDs = Memory.ScatterUnityStrings(items.Items.Keys);
                //foreach (var item in items.Items)
                //{
                //    try
                //    {
                //        ulong shortNameAddr = Memory.ReadPtr(item.Value + Offsets.ItemTemplate.ShortName, false);
                //        string shortName = Memory.ReadUnityString(shortNameAddr, false);
                //        if (itemIDs.TryGetValue(item.Key, out var itemID))
                //        {
                //            var ggez = Locale.Where(x => x.Key.Contains(itemID));
                //            foreach (var id in ggez)
                //            {
                //                Logger.WriteLine($"{id.Key} -> {id.Value}");
                //            }
                //        }
                //    }
                //    catch { }
                //}

                return true;
			}
			catch (Exception ex)
			{
				Logger.WriteLine($"[LOCALE MANAGER] -> TryLoad(): Failed to load ~ {ex}");
				return false;
			}
        }

        public static string GetItem(string id)
        {
            if (!Locale.TryGetValue(id, out string localizedValue))
                return "";

            return localizedValue;
        }

        public static string GetItemName(string id) => GetItem($"{id} Name");

        public static string GetItemShortName(string id) => GetItem($"{id} ShortName");

        public static string GetItemDescription(string id) => GetItem($"{id} Description");
    }
}
