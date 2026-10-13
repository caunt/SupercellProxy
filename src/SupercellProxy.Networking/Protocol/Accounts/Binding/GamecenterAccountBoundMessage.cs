using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Reports the result of binding the farm to a Game Center account.</summary>
/// <param name="ResultCode">The result code; 1 to 3 are authentication mismatch, API timeout and token validation errors.</param>
public sealed record GamecenterAccountBoundMessage(int ResultCode) : CodecMessage<GamecenterAccountBoundMessage>, ICodecProvider<GamecenterAccountBoundMessage>
{
    /// <inheritdoc />
    public static ICodec<GamecenterAccountBoundMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.ResultCode, static (resultCode) => new GamecenterAccountBoundMessage(resultCode));
}
