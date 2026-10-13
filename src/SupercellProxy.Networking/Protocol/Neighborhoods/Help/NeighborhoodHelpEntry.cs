using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Help;

/// <summary>One neighborhood help entry entry.</summary>
/// <param name="Value">The value.</param>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="Amount">The amount.</param>
public sealed record NeighborhoodHelpEntry(int Value, LongId AvatarId, int Amount) : ICodecProvider<NeighborhoodHelpEntry>
{
    /// <inheritdoc />
    public static ICodec<NeighborhoodHelpEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Value,
        Codecs.LongId,
        static value => value.AvatarId,
        Codecs.VarInt,
        static value => value.Amount,
        static (@value, avatarId, amount) => new NeighborhoodHelpEntry(value, avatarId, amount)
    );
}
