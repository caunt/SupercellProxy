using SupercellProxy.Networking.Protocol.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents Greg's-farm snapshot carried by clientbound message 20699, loaded in native game mode 3.
public sealed record GregFarmDataMessage(OwnHomeDataMessage Data) : CodecMessage<GregFarmDataMessage>, ICodecProvider<GregFarmDataMessage>
{
    /// <inheritdoc />
    public static ICodec<GregFarmDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static (data) => new GregFarmDataMessage(data));
}
