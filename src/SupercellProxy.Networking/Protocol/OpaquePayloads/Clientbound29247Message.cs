using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded composite payload from clientbound message 29247.
public sealed record Clientbound29247Message : IMessage
{
    /// Gets the encoded payload while its nested schema remains unconfirmed.
    public Memory<byte> PayloadData { get; init; }

    /// Decodes clientbound message 29247 without inventing nested payload fields.
    public static Clientbound29247Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound29247Message { PayloadData = container.Payload.ReadToEnd() };
    }

    /// Encodes clientbound message 29247.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.Write(PayloadData.Span);

        return new MessageContainer(identifier, version, stream);
    }
}
