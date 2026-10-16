using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Reports that the client failed to decrypt a server message.</summary>
/// <param name="ErrorCode">Which decryption step failed, as the client numbers its crypto errors.</param>
public sealed record ClientCryptoErrorMessage(int ErrorCode) : CodecMessage<ClientCryptoErrorMessage>, ICodecProvider<ClientCryptoErrorMessage>
{
    /// <inheritdoc />
    public static ICodec<ClientCryptoErrorMessage> Codec { get; } = Codecs.Composite(Codecs.Int32, static message => message.ErrorCode, static errorCode => new ClientCryptoErrorMessage(errorCode));
}
