using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports the result of binding the farm to a Facebook account.</summary>
/// <param name="ResultCode">The result code; 1 to 3 are authentication mismatch, Facebook API timeout and token validation errors.</param>
public sealed record FacebookAccountBoundMessage(int ResultCode) : CodecMessage<FacebookAccountBoundMessage>, ICodecProvider<FacebookAccountBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<FacebookAccountBoundMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.ResultCode, static resultCode => new FacebookAccountBoundMessage(resultCode));
}
