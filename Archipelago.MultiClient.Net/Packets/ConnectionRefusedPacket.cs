using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Packets
{
    public class ConnectionRefusedPacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.ConnectionRefused;

        [JsonProperty("errors")]
        public ConnectionRefusedError[] Errors { get; set; }
    }
}
