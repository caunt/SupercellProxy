using SupercellProxy.Networking.Protocol.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>The player's town home snapshot carried by clientbound message 28543.</summary>
public sealed record TrainStationDataMessage(OwnHomeDataMessage Data) : CodecMessage<TrainStationDataMessage>, ICodecProvider<TrainStationDataMessage>
{
    /// <inheritdoc />
    public static ICodec<TrainStationDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static (data) => new TrainStationDataMessage(data));
}
