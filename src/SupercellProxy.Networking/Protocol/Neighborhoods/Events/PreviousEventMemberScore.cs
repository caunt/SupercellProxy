using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Events;

/// <summary>One neighbourhood member's final score in the previous neighbourhood event.</summary>
/// <param name="AvatarId">The member's account.</param>
/// <param name="Points">Points the member earned for the neighbourhood.</param>
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
