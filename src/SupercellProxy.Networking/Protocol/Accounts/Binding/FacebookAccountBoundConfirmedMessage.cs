using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Confirms a Facebook binding the server reported.</summary>
public sealed record FacebookAccountBoundConfirmedMessage : CodecMessage<FacebookAccountBoundConfirmedMessage>, ICodecProvider<FacebookAccountBoundConfirmedMessage>
{
    /// <inheritdoc />
    public static ICodec<FacebookAccountBoundConfirmedMessage> Codec { get; } = Codecs.Unit(static () => new FacebookAccountBoundConfirmedMessage());
}
