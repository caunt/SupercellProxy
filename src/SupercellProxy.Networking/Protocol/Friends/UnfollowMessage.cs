using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Stops following the identified farm.</summary>
/// <param name="HomeId">Home id of the followed farm to drop; the server answers with a follow response for the unfollow operation.</param>
public sealed record UnfollowMessage(LongId HomeId) : CodecMessage<UnfollowMessage>, ICodecProvider<UnfollowMessage>
{
    /// <inheritdoc />
    public static ICodec<UnfollowMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId, static message => message.HomeId, static homeId => new UnfollowMessage(homeId)),
        trailingDataMessage: "The unfollow request has trailing data.");

    /// <summary>Omits the private farm identifier from diagnostics.</summary>
    /// <returns>Always the type name <c>UnfollowMessage</c>, without the farm id.</returns>
    public override string ToString()
    {
        return nameof(UnfollowMessage);
    }
}
