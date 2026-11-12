using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Likes the identified farm.</summary>
/// <param name="HomeId">Home id of the visited farm's owner whose farm the player likes.</param>
public sealed record LikeFarmMessage(LongId HomeId) : CodecMessage<LikeFarmMessage>, ICodecProvider<LikeFarmMessage>
{
    /// <inheritdoc />
    public static ICodec<LikeFarmMessage> Codec { get; } = Codecs.Complete(Codecs.Composite(Codecs.LongId, static message => message.HomeId, static homeId => new LikeFarmMessage(homeId)),
        trailingDataMessage: "The farm-like request has trailing data.");

    /// <summary>Omits the private farm identifier from diagnostics.</summary>
    /// <returns>Always the type name <c>LikeFarmMessage</c>, without the farm id.</returns>
    public override string ToString()
    {
        return nameof(LikeFarmMessage);
    }
}
