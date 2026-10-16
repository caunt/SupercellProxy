using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts;

/// <summary>Logs in with a Supercell ID token.</summary>
/// <param name="Token">Supercell ID token issued to this device when the player signed in.</param>
/// <param name="Signature">Signature bytes the client sends alongside the token.</param>
public sealed record SupercellIdLoginMessage(ReadOnlyMemory<byte>? Token, ReadOnlyMemory<byte>? Signature) : CodecMessage<SupercellIdLoginMessage>, ICodecProvider<SupercellIdLoginMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdLoginMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalByteArray,
        static message => message.Token,
        Codecs.OptionalByteArray,
        static message => message.Signature,
        static (token, signature) => new SupercellIdLoginMessage(token, signature)
    );
}
