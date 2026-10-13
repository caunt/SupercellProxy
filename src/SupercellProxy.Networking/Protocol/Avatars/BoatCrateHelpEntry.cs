using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// Represents <c language="csharp">BoatCrateHelpEntry</c>.
/// </summary>
public sealed record BoatCrateHelpEntry(int ExpirationTimestamp, int RequestKind, int CrateIndex, LongId? HelperId) : ICodecProvider<BoatCrateHelpEntry>
{
    /// <inheritdoc />
    public static ICodec<BoatCrateHelpEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.ExpirationTimestamp,
        Codecs.VarInt,
        static value => value.RequestKind,
        Codecs.VarInt,
        static value => value.CrateIndex,
        Codecs.OptionalLongId,
        static value => value.HelperId,
        static (expirationTimestamp, requestKind, crateIndex, helperId) => new BoatCrateHelpEntry(expirationTimestamp, requestKind, crateIndex, helperId)
    );
}
