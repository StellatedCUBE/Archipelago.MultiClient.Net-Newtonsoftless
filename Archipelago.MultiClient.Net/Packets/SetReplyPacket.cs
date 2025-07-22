using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Packets
{
    public class SetReplyPacket : SetPacket
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.SetReply;

        [JsonProperty("value")]
        public JObject Value { get; set; }

        [JsonProperty("original_value")]
        public JObject OriginalValue { get; set; }
	}
}
