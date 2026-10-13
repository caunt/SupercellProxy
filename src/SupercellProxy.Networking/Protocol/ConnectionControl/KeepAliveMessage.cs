using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>
/// Represents the <c language="csharp">KeepAliveMessage</c> protocol message.
/// </summary>
public sealed record KeepAliveMessage : CodecMessage<KeepAliveMessage>, ICodecProvider<KeepAliveMessage>
{
    /// <inheritdoc />
    public static ICodec<KeepAliveMessage> Codec { get; } = Codecs.Unit(static () => new KeepAliveMessage());
}
