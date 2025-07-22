using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Json;
using System.Collections.Generic;

namespace Archipelago.MultiClient.Net.Packets
{
    public class RetrievedPacket : ArchipelagoPacketBase
    {
        public override ArchipelagoPacketType PacketType => ArchipelagoPacketType.Retrieved;

        [JsonProperty("keys")]
        public Dictionary<string, JObject> Data { get; set; }
    }
}
