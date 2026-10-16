using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Stops following the identified farm.</summary>
public sealed record UnfollowMessage(LongId HomeId) : CodecMessage<UnfollowMessage>, ICodecProvider<UnfollowMessage>
{
    /// <inheritdoc />
    public static ICodec<UnfollowMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.LongId, static message => message.HomeId, static homeId => new UnfollowMessage(homeId)),
        trailingDataMessage: "The unfollow request has trailing data."
    );

    /// <summary>Omits the private farm identifier from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(UnfollowMessage);
    }
}
