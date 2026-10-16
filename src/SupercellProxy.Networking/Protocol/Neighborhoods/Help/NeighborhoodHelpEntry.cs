using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Help;

/// <summary>One helper in a farm's neighbourhood help entries.</summary>
/// <param name="Value">Number sent before the helper; its meaning is not identified.</param>
/// <param name="AvatarId">Account of the helper.</param>
/// <param name="Amount">How much the helper gave or did; its unit is not identified.</param>
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
