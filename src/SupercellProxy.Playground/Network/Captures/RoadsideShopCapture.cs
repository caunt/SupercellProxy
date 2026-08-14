namespace SupercellProxy.Playground.Network.Captures;

public sealed record RoadsideShopCapture(
    ushort SourceMessageId,
    string SourceMessageName,
    string Target,
    string? HomeOwnerName,
    string ClientVersion,
    string ContentVersion,
    string ContentFingerprintSha,
    int PayloadLength,
    string PayloadSha256,
    int SlotCount,
    IReadOnlyList<RoadsideShopSlotCapture> Slots);
