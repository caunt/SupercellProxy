using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.SupercellId;

/// <summary>Delivers the Supercell ID notification token to the Supercell ID login manager.</summary>
/// <param name="Token">The token.</param>
public sealed record SupercellIdNotificationTokenMessage(ReadOnlyMemory<byte>? Token) : CodecMessage<SupercellIdNotificationTokenMessage>, ICodecProvider<SupercellIdNotificationTokenMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdNotificationTokenMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalByteArray, static message => message.Token, static (token) => new SupercellIdNotificationTokenMessage(token));
}
