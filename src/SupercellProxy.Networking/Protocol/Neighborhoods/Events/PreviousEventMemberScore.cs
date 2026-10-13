using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>One previous event member score entry.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="Points">The points.</param>
public sealed record PreviousEventMemberScore(LongId AvatarId, int Points) : ICodecProvider<PreviousEventMemberScore>
{
    /// <inheritdoc />
    public static ICodec<PreviousEventMemberScore> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static value => value.AvatarId,
        Codecs.VarInt,
        static value => value.Points,
        static (avatarId, points) => new PreviousEventMemberScore(avatarId, points)
    );
}
