using SupercellProxy.Networking.Protocol.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents the own-fishing-home snapshot carried by clientbound message 24222.
public sealed record FishingDataMessage(OwnHomeDataMessage Data) : CodecMessage<FishingDataMessage>, ICodecProvider<FishingDataMessage>
{
    /// <inheritdoc />
    public static ICodec<FishingDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static (data) => new FishingDataMessage(data));
}
