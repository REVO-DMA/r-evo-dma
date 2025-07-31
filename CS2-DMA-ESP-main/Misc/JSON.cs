using System.Text.Json.Serialization;

namespace cs2_dma_esp.Misc
{
    [JsonSerializable(typeof(UserConfig))]
    internal partial class JSONSourceGenerationContext : JsonSerializerContext { }
}
