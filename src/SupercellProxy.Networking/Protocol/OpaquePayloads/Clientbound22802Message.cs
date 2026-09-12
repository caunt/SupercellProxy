using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded collection envelope from clientbound message 22802.
public sealed record Clientbound22802Message : IMessage
{
    /// Gets the encoded collections while their inner schemas remain unconfirmed.
    public Memory<byte> CollectionData { get; init; }

    /// Decodes clientbound message 22802 without inventing inner collection fields.
    public static Clientbound22802Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound22802Message { CollectionData = container.Payload.ReadToEnd() };
    }

    /// Encodes clientbound message 22802.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.Write(CollectionData.Span);

        return new MessageContainer(identifier, version, stream);
    }
}
