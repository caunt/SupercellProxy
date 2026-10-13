using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ConnectionControl;

/// Explains why the server is closing the current client session.
public sealed record TitanDisconnectedMessage(DisconnectReason Reason) : CodecMessage<TitanDisconnectedMessage>, ICodecProvider<TitanDisconnectedMessage>
{
    /// <inheritdoc />
    public static ICodec<TitanDisconnectedMessage> Codec { get; } = Codecs.Complete(
        Codecs.Composite(
            Codecs.Enum<DisconnectReason>(Codecs.Int32),
            static message => message.Reason,
            static (reason) => new TitanDisconnectedMessage(reason)
        ),
        trailingDataMessage: "The disconnect notification has trailing data."
    );
}
