using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// Carries the mode-specific recipient entries from clientbound message 29415.
public sealed record Clientbound29415Message : IMessage
{
    private const int MaximumEntryCount = 100000;

    /// Gets the owned wire entries, or <see langword="null"/> when the encoded collection is absent.
    public Message29415Entry[]? Entries { get; init; }

    /// Gets the native routing mode.
    public int Mode { get; init; }

    /// Decodes clientbound message 29415.
    public static Clientbound29415Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        int mode = container.Payload.ReadVariableInt();
        int entryCount = container.Payload.ReadVariableInt();

        if (uint.CreateTruncating(entryCount) > MaximumEntryCount)
            return new Clientbound29415Message { Mode = mode };

        Message29415Entry[] entries = new Message29415Entry[entryCount];

        for (int index = 0; index < entries.Length; index++)
            entries[index] = ReadEntry(container.Payload);

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 29415 has trailing data.")
            : new Clientbound29415Message { Mode = mode, Entries = entries };
    }

    /// Encodes clientbound message 29415.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        if (Entries?.Length > MaximumEntryCount)
            throw new InvalidDataException(message: "Clientbound message 29415 has too many entries.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Mode);
        stream.WriteVariableInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (Message29415Entry entry in Entries)
                WriteEntry(stream, entry);
        }

        return new MessageContainer(identifier, version, stream);
    }

    private static Message29415Entry ReadEntry(MessageStream stream)
    {
        return new Message29415Entry(
            ResourceAssociation: stream.ReadLongIdentifier(),
            PrimaryText: stream.ReadOptionalString(),
            OptionalIndexedText: stream.ReadOptionalString(),
            UnretainedText: stream.ReadOptionalString(),
            RequiredText: stream.ReadOptionalString(),
            Value: stream.ReadVariableInt(),
            UnretainedValue: stream.ReadVariableInt(),
            EntryType: stream.ReadVariableInt(),
            UnretainedTrailingValue: stream.ReadVariableInt()
        );
    }

    private static void WriteEntry(MessageStream stream, Message29415Entry entry)
    {
        stream.WriteLongIdentifier(entry.ResourceAssociation);
        stream.WriteOptionalString(entry.PrimaryText);
        stream.WriteOptionalString(entry.OptionalIndexedText);
        stream.WriteOptionalString(entry.UnretainedText);
        stream.WriteOptionalString(entry.RequiredText);
        stream.WriteVariableInt(entry.Value);
        stream.WriteVariableInt(entry.UnretainedValue);
        stream.WriteVariableInt(entry.EntryType);
        stream.WriteVariableInt(entry.UnretainedTrailingValue);
    }
}
