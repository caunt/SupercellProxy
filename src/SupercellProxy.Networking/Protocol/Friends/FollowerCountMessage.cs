using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries the number of followers of the identified farm.
public sealed record FollowerCountMessage(LongId HomeId, int FollowerCount) : CodecMessage<FollowerCountMessage>, ICodecProvider<FollowerCountMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowerCountMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.LongId,
            static message => message.HomeId,
            Codecs.VarInt,
            static message => message.FollowerCount,
            static (homeId, followerCount) => new FollowerCountMessage(homeId, followerCount)
        ),
        trailingDataMessage: "The follower count has trailing data."
    );

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowerCountMessage);
    }
}
