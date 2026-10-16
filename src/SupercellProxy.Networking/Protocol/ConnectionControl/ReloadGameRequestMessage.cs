using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// <summary>Asks the client to reload the game; the native handler only raises the game's reload flag.</summary>
/// <param name="Reason">A value the client does not read.</param>
public sealed record ReloadGameRequestMessage(int Reason) : CodecMessage<ReloadGameRequestMessage>, ICodecProvider<ReloadGameRequestMessage>
{
    /// <inheritdoc />
    public static ICodec<ReloadGameRequestMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Reason, static reason => new ReloadGameRequestMessage(reason));
}
