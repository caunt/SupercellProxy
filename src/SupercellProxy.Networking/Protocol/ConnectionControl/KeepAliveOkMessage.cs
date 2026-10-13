using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>
/// Represents the <c language="csharp">KeepAliveOkMessage</c> protocol message.
/// </summary>
public sealed record KeepAliveOkMessage : CodecMessage<KeepAliveOkMessage>, ICodecProvider<KeepAliveOkMessage>
{
    /// <inheritdoc />
    public static ICodec<KeepAliveOkMessage> Codec { get; } = Codecs.Unit(static () => new KeepAliveOkMessage());
}
