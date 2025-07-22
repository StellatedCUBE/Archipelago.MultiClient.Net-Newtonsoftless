using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Packets
{
    public class LocationChecksPacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.LocationChecks;

        [JsonProperty("locations")]
        public long[] Locations { get; set; }    }
}
