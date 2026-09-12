using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.CollectionPayloads;

/// <summary>
/// Defines the Clientbound26994 Message contract.
/// </summary>
public sealed record Clientbound26994Message : IMessage
{
    private const int MaximumEntryCount = 10000;

    /// <summary>
    /// Gets the Entries value.
    /// </summary>
    public Message26994Entry[]? Entries { get; init; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static Clientbound26994Message Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int count = stream.ReadVariableInt();

        if (count < -1 || count > MaximumEntryCount || count > (stream.Length - stream.Position) / 10)
            throw new InvalidDataException(message: "The entry-status collection count is invalid.");

        Message26994Entry[]? entries = count < 0 ? null : new Message26994Entry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = Message26994Entry.Decode(stream);
        }

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The entry-status collection has trailing data.")
            : new Clientbound26994Message { Entries = entries };
    }

    /// <summary>
    /// Provides the To Container value or operation.
    /// </summary>
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (Message26994Entry entry in Entries)
                entry.Encode(stream);
        }

        return new MessageContainer(identifier, version, stream);
    }
}
