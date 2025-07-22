using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;
using Archipelago.MultiClient.Net.Models;

namespace Archipelago.MultiClient.Net.Packets
{
    public class LocationInfoPacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.LocationInfo;

        [JsonProperty("locations")]
        public NetworkItem[] Locations { get; set; }
    }
}
