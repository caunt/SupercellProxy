using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Likes the identified farm.</summary>
public sealed record LikeFarmMessage(LongId HomeId) : CodecMessage<LikeFarmMessage>, ICodecProvider<LikeFarmMessage>
{
    /// <inheritdoc />
    public static ICodec<LikeFarmMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(Codecs.LongId, static message => message.HomeId, static (homeId) => new LikeFarmMessage(homeId)),
        trailingDataMessage: "The farm-like request has trailing data."
    );

    /// <summary>Omits the private farm identifier from diagnostics.</summary>
    public override string ToString()
    {
        return nameof(LikeFarmMessage);
    }
}
