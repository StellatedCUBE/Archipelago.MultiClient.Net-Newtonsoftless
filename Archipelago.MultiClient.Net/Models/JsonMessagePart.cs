using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Models
{
    public class JsonMessagePart
    {
        [JsonProperty("type")]
        public JsonMessagePartType? Type { get; set; }

        [JsonProperty("color")]
        public JsonMessagePartColor? Color { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("player")]
        public int? Player { get; set; }

        [JsonProperty("flags")]
        public ItemFlags? Flags { get; set; }

        [JsonProperty("hint_status")]
        public HintStatus? HintStatus { get; set; }
	}
}