using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>Requests a token for an in-game web view.</summary>
public sealed record RequestWebViewTokenMessage : CodecMessage<RequestWebViewTokenMessage>, ICodecProvider<RequestWebViewTokenMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestWebViewTokenMessage> Codec { get; } = Codecs.Unit(static () => new RequestWebViewTokenMessage());
}
