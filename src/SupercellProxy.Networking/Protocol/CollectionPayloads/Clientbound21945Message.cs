using System.Globalization;

using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// Carries the retained empty entry collection from clientbound message 21945.
public sealed record Clientbound21945Message : IMessage
{
    private const int MaximumEntryCount = 1000;

    /// Gets the decoded entries in wire order.
    public Message21945Entry[] Entries { get; init; } = [];

    /// Decodes clientbound message 21945.
    public static Clientbound21945Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        int entryCount = container.Payload.ReadVariableInt();

        if (uint.CreateTruncating(entryCount) > MaximumEntryCount)
            throw new InvalidDataException(string.Create(CultureInfo.InvariantCulture, $"Invalid message-21945 entry count: {entryCount}."));

        Message21945Entry[] entries = new Message21945Entry[entryCount];

        for (int index = 0; index < entries.Length; index++)
            entries[index] = Message21945Entry.Decode(container.Payload);

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "Clientbound message 21945 has trailing data.")
            : new Clientbound21945Message { Entries = entries };
    }

    /// Encodes clientbound message 21945.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        if (Entries.Length > MaximumEntryCount)
            throw new InvalidDataException(message: "Too many message-21945 entries.");

        stream.WriteArray(Entries, static (output, entry) => entry.Encode(output));

        return new MessageContainer(identifier, version, stream);
    }
}
