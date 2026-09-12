using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// Carries the entry count from clientbound message 21915.
public sealed record Clientbound21915Message : IMessage
{
    /// Gets the decoded entry count.
    public int EntryCount { get; init; }

    /// Decodes clientbound message 21915.
    public static Clientbound21915Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        Clientbound21915Message message = new() { EntryCount = container.Payload.ReadVariableInt() };

        return message.EntryCount is not 0
            ? throw new NotSupportedException(message: "Nonempty clientbound message 21915 entries are not implemented yet.")
            : container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 21915 has trailing data.")
            : message;
    }

    /// Encodes clientbound message 21915.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        if (EntryCount is not 0)
            throw new NotSupportedException(message: "Nonempty clientbound message 21915 entries are not implemented yet.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(EntryCount);

        return new MessageContainer(identifier, version, stream);
    }
}
