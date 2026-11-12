using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Requests the follower count of the identified farm.</summary>
/// <param name="HomeId">Home id of the farm to count followers for, either the player's own farm or the farm currently loaded; the server answers with a <see cref="FollowerCountMessage"/>.</param>
public sealed record RequestFollowerCountMessage(LongId HomeId) : CodecMessage<RequestFollowerCountMessage>, ICodecProvider<RequestFollowerCountMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestFollowerCountMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId, static message => message.HomeId, static homeId => new RequestFollowerCountMessage(homeId)),
        trailingDataMessage: "The follower-count request has trailing data.");

    /// <summary>Omits the private farm id from diagnostic text.</summary>
    /// <returns>Always the type name <c>RequestFollowerCountMessage</c>, without the farm id.</returns>
    public override string ToString()
    {
        return nameof(RequestFollowerCountMessage);
    }
}
