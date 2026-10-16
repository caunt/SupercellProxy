using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Reports a connection status; the client only reacts to status 4, which it passes to the loading screen.</summary>
/// <param name="Status">Connection state code; only 4 has a visible effect in the client.</param>
public sealed record ConnectionStatusMessage(int Status) : CodecMessage<ConnectionStatusMessage>, ICodecProvider<ConnectionStatusMessage>
{
    /// <inheritdoc />
    public static ICodec<ConnectionStatusMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Status, static status => new ConnectionStatusMessage(status));
}
