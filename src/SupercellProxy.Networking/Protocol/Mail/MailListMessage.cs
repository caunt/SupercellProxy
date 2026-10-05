using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.Mail;

/// Carries the player's mail entries in native wire order.
public sealed record MailListMessage : IMessage
{
    private const int MaximumEntryCount = 1024;

    /// Gets the decoded mail entries, or null for the native absent-list marker.
    public MailEntry[]? Entries { get; init; }

    /// Gets the decoded entry count.
    public int EntryCount => Entries?.Length ?? -1;

    /// Decodes clientbound message 21915.
    public static MailListMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int count = stream.ReadVarInt();

        if (count < -1 || count > MaximumEntryCount || count > (stream.Length - stream.Position) / MailEntry.MinimumEncodedSize)
            throw new InvalidDataException(message: "The mail-list entry count is invalid.");

        MailEntry[]? entries = count < 0 ? null : new MailEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = MailEntry.Decode(stream);
        }

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The mail-list message has trailing data.")
            : new MailListMessage { Entries = entries };
    }

    /// Encodes clientbound message 21915.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (Entries?.Length > MaximumEntryCount)
            throw new InvalidDataException(message: "The mail-list entry count is invalid.");

        stream.WriteVarInt(EntryCount);

        if (Entries is not null)
        {
            foreach (MailEntry entry in Entries)
            {
                ArgumentNullException.ThrowIfNull(entry);
                entry.Encode(stream);
            }
        }
    }

    /// Omits private mail content from diagnostic text.
    public override string ToString()
    {
        return nameof(MailListMessage);
    }
}
