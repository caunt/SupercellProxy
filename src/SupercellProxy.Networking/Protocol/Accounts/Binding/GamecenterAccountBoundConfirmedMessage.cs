using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Binding;

/// <summary>Confirms a Game Center binding the server reported.</summary>
public sealed record GamecenterAccountBoundConfirmedMessage : CodecMessage<GamecenterAccountBoundConfirmedMessage>, ICodecProvider<GamecenterAccountBoundConfirmedMessage>
{
    /// <inheritdoc />
    public static ICodec<GamecenterAccountBoundConfirmedMessage> Codec { get; } = Codecs.Unit(static () => new GamecenterAccountBoundConfirmedMessage());
}
