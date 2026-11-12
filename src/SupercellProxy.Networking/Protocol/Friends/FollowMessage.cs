using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Requests that the client begin following the identified farm.</summary>
/// <param name="HomeId">Home id of the farm to follow; the server answers with a follow response for the follow operation.</param>
public sealed record FollowMessage(LongId HomeId) : CodecMessage<FollowMessage>, ICodecProvider<FollowMessage>
{
    /// <inheritdoc />
    public static ICodec<FollowMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId, static message => message.HomeId, static homeId => new FollowMessage(homeId)),
        trailingDataMessage: "The follow request has trailing data.");

    /// <summary>Omits the private farm id from diagnostic text.</summary>
    /// <returns>Always the type name <c>FollowMessage</c>, without the farm id.</returns>
    public override string ToString()
    {
        return nameof(FollowMessage);
    }
}
