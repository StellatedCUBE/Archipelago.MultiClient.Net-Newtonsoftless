using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;

namespace Archipelago.MultiClient.Net.Packets
{
    public class GetDataPackagePacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.GetDataPackage;

        [JsonProperty("games")]
        public string[] Games { get; set; }
    }
}
