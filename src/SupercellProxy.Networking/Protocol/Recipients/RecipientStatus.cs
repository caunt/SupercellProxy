using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Recipients;

/// <summary>One recipient status entry.</summary>
/// <param name="Value0">The value0.</param>
/// <param name="Value1">The value1.</param>
/// <param name="AvatarId">The avatar id.</param>
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
