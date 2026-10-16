using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>Status of one player the social manager tracks as a recipient; the two numbers are not identified.</summary>
/// <param name="Value0">First number of the entry.</param>
/// <param name="Value1">Second number of the entry.</param>
/// <param name="AvatarId">The player the status belongs to.</param>
public sealed record RecipientStatus(int Value0, int Value1, LongId AvatarId) : ICodecProvider<RecipientStatus>
{
    /// <inheritdoc />
    public static ICodec<RecipientStatus> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Value0,
        Codecs.VarInt,
        static value => value.Value1,
        Codecs.LongId,
        static value => value.AvatarId,
        static (value0, value1, avatarId) => new RecipientStatus(value0, value1, avatarId)
    );
}
