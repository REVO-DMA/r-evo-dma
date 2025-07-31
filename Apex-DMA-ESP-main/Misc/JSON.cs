using System.Text.Json.Serialization;

namespace apex_dma_esp.Misc
{
    [JsonSerializable(typeof(UserConfig))]
    internal partial class JSONSourceGenerationContext : JsonSerializerContext { }
}
