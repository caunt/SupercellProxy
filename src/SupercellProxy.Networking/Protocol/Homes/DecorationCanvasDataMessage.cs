using SupercellProxy.Networking.Protocol.Serialization;

using SupercellProxy.Networking.Protocol.MessageEncoding;

namespace SupercellProxy.Networking.Protocol.Homes;

/// Represents the deco-canvas home snapshot carried by clientbound message 28544, loaded in native game mode 9.
public sealed record DecorationCanvasDataMessage(OwnHomeDataMessage Data) : CodecMessage<DecorationCanvasDataMessage>, ICodecProvider<DecorationCanvasDataMessage>
{
    /// <inheritdoc />
    public static ICodec<DecorationCanvasDataMessage> Codec { get; } = Codecs.Composite(OwnHomeDataMessage.Codec, static message => message.Data, static (data) => new DecorationCanvasDataMessage(data));
}
