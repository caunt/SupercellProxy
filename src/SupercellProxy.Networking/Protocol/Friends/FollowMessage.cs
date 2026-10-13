using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests that the client begin following the identified farm.
public sealed record FollowMessage(LongId HomeId) : CodecMessage<FollowMessage>, ICodecProvider<FollowMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.LongId, static message => message.HomeId, static (homeId) => new FollowMessage(homeId)),
        trailingDataMessage: "The follow request has trailing data."
    );

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowMessage);
    }
}
