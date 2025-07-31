using SkiaSharp;
using Tarkov_DMA_Backend.Tarkov.GameAPI;

namespace Tarkov_DMA_Backend.Misc
{
    [JsonSerializable(typeof(JSON.ESPStyleSync))]
    [JsonSerializable(typeof(JSON.EspSync))]
    [JsonSerializable(typeof(JSON.FeatureSync[]))]
    [JsonSerializable(typeof(JSON.IPCClientMessage))]
    [JsonSerializable(typeof(JSON.MapConfig[]))]
    [JsonSerializable(typeof(Dictionary<string, JSON.TarkovItem>))]
    [JsonSerializable(typeof(JSON.ItemFilterSync[]))]
    [JsonSerializable(typeof(JSON.AuthLogin))]
    [JsonSerializable(typeof(JSON.CanLaunch_Base))]
    [JsonSerializable(typeof(JSON.CanLaunch_Success))]
    [JsonSerializable(typeof(JSON.CanLaunch_Error))]
    [JsonSerializable(typeof(JSON.GenericIntValue))]
    [JsonSerializable(typeof(JSON.GenericBoolValue))]
    [JsonSerializable(typeof(JSON.LootFilter[]))]
    [JsonSerializable(typeof(JSON.FeatureToggleHotkeySync[]))]
    [JsonSerializable(typeof(PlayerProfile.RootObject))]
    [JsonSerializable(typeof(Dictionary<string, JSON.HitboxSetting>))]
    internal partial class JSONSourceGenerationContext : JsonSerializerContext { }

    public static class JSON
    {
        public class LootFilter
        {
            [JsonPropertyName("id")]
            public string ID { get; set; }
            [JsonPropertyName("filter_name")]
            public string FilterName { get; set; }
            [JsonPropertyName("color")]
            public string Color {  get; set; }
            [JsonPropertyName("border_color")]
            public string BorderColor { get; set; }
            [JsonPropertyName("text_color")]
            public string TextColor { get; set; }
            [JsonPropertyName("shape")]
            public byte Shape { get; set; }
            [JsonPropertyName("items")]
            public List<string> Items { get; set; }

            [JsonIgnore]
            public SKPaint ColorPaint;
            [JsonIgnore]
            public SKPaint BorderColorPaint;
            [JsonIgnore]
            public SKPaint TextColorPaint;
        }

        public class MapConfig
        {
            [JsonPropertyName("mapID")]
            public string MapID { get; set; }
            [JsonPropertyName("x")]
            public float X { get; set; }
            [JsonPropertyName("y")]
            public float Y { get; set; }
            [JsonPropertyName("scale")]
            public float Scale { get; set; }
            [JsonPropertyName("rotation")]
            public float Rotation { get; set; }
            [JsonPropertyName("pawnRotation")]
            public float PawnRotation { get; set; }
        }

        public class AuthLogin
        {
            [JsonPropertyName("email")]
            public string Email { get; set; }
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        public class CanLaunch_Base
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }
        }

        public class CanLaunch_SuccessMessage
        {
            public string Expiration { get; set; }
            public float Runtime { get; set; }
        }

        public class CanLaunch_Success
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }
            [JsonPropertyName("message")]
            public CanLaunch_SuccessMessage Message { get; set; }
        }

        public class CanLaunch_Error
        {
            [JsonPropertyName("success")]
            public bool Success { get; set; }
            [JsonPropertyName("message")]
            public string[] Message { get; set; }
        }

        public class GenericIntValue
        {
            public int Value { get; set; }
        }

        public class GenericBoolValue
        {
            public bool Value { get; set; }
        }

        public class ItemFilterSync
        {
            public string ID { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }

        public class FeatureSettingSync
        {
            public string ID { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }

        public class FeatureSync
        {
            public string ID { get; set; }
            public bool Enabled { get; set; }
            public FeatureSettingSync[] Settings { get; set; }
        }

        public class FeatureToggleHotkeySync
        {
            public string ID { get; set; }
            public int Hotkey { get; set; }
        }

        public class ESPSettingSync
        {
            public string ID { get; set; }
            public string Type { get; set; }
            public string Value { get; set; }
        }

        public class EspSync
        {
            public ESPSettingSync[] Settings { get; set; }
        }

        public class PlayerTypeEspStyle
        {
            [JsonPropertyName("id")]
            public int ID { get; set; }

            [JsonPropertyName("boxColorVisible")]
            public string BoxColorVisible { get; set; }
            [JsonPropertyName("boxColorInvisible")]
            public string BoxColorInvisible { get; set; }

            [JsonPropertyName("boneColorVisible")]
            public string BoneColorVisible { get; set; }
            [JsonPropertyName("boneColorInvisible")]
            public string BoneColorInvisible { get; set; }

            [JsonPropertyName("fontColor")]
            public string FontColor { get; set; }
        }

        public class ESPStyleSync
        {
            public PlayerTypeEspStyle[] Styles { get; set; }
        }

        public class HitboxSetting
        {
            [JsonPropertyName("chance")]
            public float Chance { get; set; }
            [JsonPropertyName("smartTargeting")]
            public bool SmartTargeting { get; set; }
            [JsonPropertyName("availableBones")]
            public string[] AvailableBones { get; set; }
            [JsonPropertyName("selectedBone")]
            public string SelectedBone { get; set; }
            [JsonPropertyName("side")]
            public string Side { get; set; }
        }

        public class IPCClientMessage
        {
            public int MessageType { get; set; }
            public string Data { get; set; } = string.Empty;
        }

        public class TarkovItem
        {
            public class ItemCategories_t
            {
                [JsonPropertyName("isKey")]
                public bool IsKey { get; set; }
                [JsonPropertyName("isBarterItem")]
                public bool IsBarterItem { get; set; }
                [JsonPropertyName("isContainer")]
                public bool IsContainer { get; set; }
                [JsonPropertyName("isFood")]
                public bool IsFood { get; set; }
                [JsonPropertyName("isDrink")]
                public bool IsDrink { get; set; }
                [JsonPropertyName("isMedical")]
                public bool IsMedical { get; set; }
                [JsonPropertyName("isSpecialEquipment")]
                public bool IsSpecialEquipment { get; set; }
                [JsonPropertyName("isMap")]
                public bool IsMap { get; set; }
                [JsonPropertyName("isAmmo")]
                public bool IsAmmo { get; set; }
                [JsonPropertyName("isAmmoPack")]
                public bool IsAmmoPack { get; set; }
                [JsonPropertyName("isCurrency")]
                public bool IsCurrency { get; set; }
                [JsonPropertyName("isRepairKit")]
                public bool IsRepairKit { get; set; }
                [JsonPropertyName("isOther")]
                public bool IsOther { get; set; }
                [JsonPropertyName("isSight")]
                public bool IsSight { get; set; }
                [JsonPropertyName("isGear")]
                public bool IsGear { get; set; }
                [JsonPropertyName("isWeapon")]
                public bool IsWeapon { get; set; }
                [JsonPropertyName("isMeleeWeapon")]
                public bool IsMeleeWeapon { get; set; }
                [JsonPropertyName("isThrowable")]
                public bool IsThrowable { get; set; }
                [JsonPropertyName("isWeaponPart")]
                public bool IsWeaponPart { get; set; }
            }

            public class TraderPrice_t
            {
                [JsonPropertyName("name")]
                public string Name { get; set; }
                [JsonPropertyName("price")]
                public int Price { get; set; }
                [JsonPropertyName("priceStr")]
                public string PriceStr { get; set; }
                [JsonPropertyName("pricePerSlot")]
                public int PricePerSlot { get; set; }
                [JsonPropertyName("ppsStr")]
                public string PPS_Str { get; set; }
            }

            public class SellPrice_t
            {
                [JsonPropertyName("highestPrice")]
                public TraderPrice_t HighestPrice { get; set; }
                [JsonPropertyName("hasPrice")]
                public bool HasPrice { get; set; }
            }

            public class BuyPrice_t
            {
                [JsonPropertyName("lowestPrice")]
                public TraderPrice_t LowestPrice { get; set; }
                [JsonPropertyName("hasPrice")]
                public bool HasPrice { get; set; }
            }

            public class ItemPrice_t
            {
                [JsonPropertyName("sell")]
                public SellPrice_t Sell { get; set; }
                [JsonPropertyName("buy")]
                public BuyPrice_t Buy { get; set; }
                [JsonPropertyName("hasPrice")]
                public bool HasPrice { get; set; }
            }

            [JsonPropertyName("id")]
            public string ID { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("shortName")]
            public string ShortName { get; set; }
            [JsonPropertyName("width")]
            public int Width { get; set; }
            [JsonPropertyName("height")]
            public int Height { get; set; }
            [JsonPropertyName("categories")]
            public ItemCategories_t Categories { get; set; }
            [JsonPropertyName("itemPrice")]
            public ItemPrice_t ItemPrice { get; set; }

            // Loot Filter
            [JsonIgnore]
            public LootFilter ActiveLootFilter = null;
            [JsonIgnore]
            public bool FilterItem = false;
        }

        public static Dictionary<string, HitboxSetting> DeserializeHitboxSettings(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.DictionaryStringHitboxSetting);
        }

        public static ESPStyleSync DeserializeEspStyleSync(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.ESPStyleSync);
        }

        public static EspSync DeserializeEspSync(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.EspSync);
        }

        public static FeatureSync[] DeserializeFeatureSync(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.FeatureSyncArray);
        }

        public static FeatureToggleHotkeySync[] DeserializeFeatureToggleHotkeySync(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.FeatureToggleHotkeySyncArray);
        }

        public static IPCClientMessage DeserializeIPCClientMessage(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.IPCClientMessage);
        }

        public static MapConfig[] DeserializeMapConfigs(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.MapConfigArray);
        }

        public static Dictionary<string, TarkovItem> DeserializeTarkovItems(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.DictionaryStringTarkovItem);
        }

        public static ItemFilterSync[] DeserializeItemFilterSync(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.ItemFilterSyncArray);
        }

        public static AuthLogin DeserializeAuthLogin(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.AuthLogin);
        }

        public static string SerializeAuthLogin(AuthLogin data)
        {
            return JsonSerializer.Serialize(data, JSONSourceGenerationContext.Default.AuthLogin);
        }

        public static CanLaunch_Base DeserializeCanLaunch_Base(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.CanLaunch_Base);
        }

        public static CanLaunch_Success DeserializeCanLaunch_Success(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.CanLaunch_Success);
        }

        public static CanLaunch_Error DeserializeCanLaunch_Error(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.CanLaunch_Error);
        }

        public static GenericIntValue DeserializeGenericIntValue(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.GenericIntValue);
        }

        public static GenericBoolValue DeserializeGenericBoolValue(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.GenericBoolValue);
        }

        public static LootFilter DeserializeLootFilter(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.LootFilter);
        }

        public static LootFilter[] DeserializeAllLootFilters(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.LootFilterArray);
        }

        public static PlayerProfile.RootObject DeserializePlayerProfile(string data)
        {
            return JsonSerializer.Deserialize(data, JSONSourceGenerationContext.Default.RootObject);
        }
    }
}
