using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars;

/// <summary>
/// A help request on one boat crate and, once someone fills the crate, who did.
/// </summary>
/// <param name="ExpirationTimestamp">When the record lapses, taken from the boat's help period; 0 when it was recorded during a visit.</param>
/// <param name="RequestKind">Kind of help, as native numbers it.</param>
/// <param name="CrateIndex">Position of the crate on the boat, starting at 0.</param>
/// <param name="HelperId">
/// Account that filled the crate: a helper, or the farm's own account when the owner filled a crate it had asked help for. Null while
/// the crate still waits for help.
/// </param>
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
