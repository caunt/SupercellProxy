using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.SupercellId;

/// <summary>Reports the result of binding the farm to a Supercell ID; a successful bind carries the Supercell ID token.</summary>
/// <param name="ResultCode">The result code.</param>
/// <param name="Token">The Supercell ID token; the client cannot bind the account without it.</param>
public sealed record SupercellIdAccountBoundMessage(int ResultCode, ReadOnlyMemory<byte>? Token) : CodecMessage<SupercellIdAccountBoundMessage>, ICodecProvider<SupercellIdAccountBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<SupercellIdAccountBoundMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.ResultCode,
        Codecs.OptionalByteArray,
        static message => message.Token,
        static (resultCode, token) => new SupercellIdAccountBoundMessage(resultCode, token)
    );
}
