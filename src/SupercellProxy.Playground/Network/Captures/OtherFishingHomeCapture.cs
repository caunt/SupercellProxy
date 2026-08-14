using SupercellProxy.Playground.Network.Messages.Clientbound;
using SupercellProxy.Playground.Resources.Csv;

namespace SupercellProxy.Playground.Network.Captures;

public sealed record OtherFishingHomeCapture(
    ushort MessageId,
    string MessageName,
    string Target,
    string ClientVersion,
    string ContentVersion,
    string ContentFingerprintSha,
    int PayloadLength,
    string PayloadSha256,
    string Payload,
    OtherFishingHomeDataMessage Decoded,
    IReadOnlyList<LogicDataTableResource> LogicDataTables,
    IReadOnlyList<RoadsideShopSlotCapture> RoadsideShop);
