using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded polymorphic entry collection from clientbound message 20621.
public sealed record Clientbound20621Message : IMessage
{
    /// Gets the encoded collection while its inner entry schema remains unconfirmed.
    public Memory<byte> EntryCollectionData { get; init; }

    /// Decodes clientbound message 20621 without inventing inner entry fields.
    public static Clientbound20621Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound20621Message { EntryCollectionData = container.Payload.ReadToEnd() };
    }

    /// Encodes clientbound message 20621.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.Write(EntryCollectionData.Span);

        return new MessageContainer(identifier, version, stream);
    }
}
