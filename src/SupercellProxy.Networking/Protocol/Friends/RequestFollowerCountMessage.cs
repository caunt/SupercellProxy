using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests the follower count of the identified farm.
public sealed record RequestFollowerCountMessage(LongId HomeId) : CodecMessage<RequestFollowerCountMessage>, ICodecProvider<RequestFollowerCountMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFollowerCountMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.LongId, static message => message.HomeId, static (homeId) => new RequestFollowerCountMessage(homeId)),
        trailingDataMessage: "The follower-count request has trailing data."
    );

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(RequestFollowerCountMessage);
    }
}
