using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Deeplinks;

/// <summary>Sends a deeplink action the client could not handle locally.</summary>
/// <param name="Action">The action.</param>
public sealed record HandleDeeplinkActionMessage(string? Action) : CodecMessage<HandleDeeplinkActionMessage>, ICodecProvider<HandleDeeplinkActionMessage>
{
    /// <inheritdoc />
    public static ICodec<HandleDeeplinkActionMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalString, static message => message.Action, static (action) => new HandleDeeplinkActionMessage(action));
}
