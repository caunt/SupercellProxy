using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Friends.Helpers;

/// <summary>Requests the helper list.</summary>
public sealed record RequestHelpersMessage : CodecMessage<RequestHelpersMessage>, ICodecProvider<RequestHelpersMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestHelpersMessage> Codec { get; } = Codecs.Unit(static () => new RequestHelpersMessage());
}
