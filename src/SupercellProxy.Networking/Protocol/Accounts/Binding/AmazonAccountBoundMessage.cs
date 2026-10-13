using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports the result of binding the farm to an Amazon account.</summary>
/// <param name="ResultCode">The result code.</param>
public sealed record AmazonAccountBoundMessage(int ResultCode) : CodecMessage<AmazonAccountBoundMessage>, ICodecProvider<AmazonAccountBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<AmazonAccountBoundMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.ResultCode, static (resultCode) => new AmazonAccountBoundMessage(resultCode));
}
