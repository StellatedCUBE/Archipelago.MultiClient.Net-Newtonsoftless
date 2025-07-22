using Archipelago.MultiClient.Net.Json;
using System;

namespace Archipelago.MultiClient.Net.Enums
{
    [JsonStringEnum]
    public enum ArchipelagoPacketType
    {
        RoomInfo,
        ConnectionRefused,
        Connected,
        ReceivedItems,
        LocationInfo,
        RoomUpdate,
        PrintJSON,
        Connect,
        ConnectUpdate,
        LocationChecks,
        LocationScouts,
        StatusUpdate,
        Say,
        GetDataPackage,
        DataPackage,
        Sync,
        Bounced,
        Bounce,
        InvalidPacket,
        Get,
        Retrieved,
        Set,
        SetReply,
        SetNotify,
        UpdateHint,
        Unknown
    }
}
