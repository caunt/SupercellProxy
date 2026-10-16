using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports the result of binding the farm to a Google Play Games account.</summary>
/// <param name="ResultCode">The result code; 1 to 3 are authentication mismatch, API timeout and token validation errors.</param>
public sealed record GoogleServiceAccountBoundMessage(int ResultCode) : CodecMessage<GoogleServiceAccountBoundMessage>, ICodecProvider<GoogleServiceAccountBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<GoogleServiceAccountBoundMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.ResultCode, static resultCode => new GoogleServiceAccountBoundMessage(resultCode));
}
