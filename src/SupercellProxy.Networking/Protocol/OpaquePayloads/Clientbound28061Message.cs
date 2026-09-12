using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.OpaquePayloads;

/// Carries the encoded polymorphic entry collection from clientbound message 28061.
public sealed record Clientbound28061Message : IMessage
{
    /// Gets the encoded collection while its inner entry schema remains unconfirmed.
    public Memory<byte> EntryCollectionData { get; init; }

    /// Decodes clientbound message 28061 without inventing inner entry fields.
    public static Clientbound28061Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return new Clientbound28061Message { EntryCollectionData = container.Payload.ReadToEnd() };
    }

    /// Encodes clientbound message 28061.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.Write(EntryCollectionData.Span);

        return new MessageContainer(identifier, version, stream);
    }
}
