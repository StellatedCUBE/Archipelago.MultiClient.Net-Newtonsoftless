using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Packets
{
    public class GetPacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.Get;

        [JsonProperty("keys")]
        public string[] Keys { get; set; }
    }
}
