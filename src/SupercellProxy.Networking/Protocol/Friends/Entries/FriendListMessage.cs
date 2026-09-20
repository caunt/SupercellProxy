using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// Carries a nullable, mode-specific list of friend entries.
public sealed record FriendListMessage(int Mode, FriendEntry[]? Entries) : IMessage
{
    /// Selects the separate recipient-list consumer.
    public const int RecipientMode = 4;
    /// Selects the native in-game friend and pending-request list.
    public const int RelationshipMode = 3;

    private const int MaximumEntryCount = 100_000;

    /// Decodes the mode and friend-entry collection.
    public static FriendListMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int mode = stream.ReadVariableInt();
        int count = stream.ReadVariableInt();

        if (count < -1 || count > MaximumEntryCount || count > (stream.Length - stream.Position) / FriendEntry.MinimumEncodedSize)
            throw new InvalidDataException(message: "The friend-entry count is invalid.");

        FriendEntry[]? entries = count < 0 ? null : new FriendEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = FriendEntry.Decode(stream);
        }

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The friend list has trailing data.")
            : new FriendListMessage(mode, entries);
    }

    /// Encodes this list with the supplied identifier and version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        if (Entries?.Length > MaximumEntryCount)
            throw new InvalidDataException(message: "The friend-entry count exceeds the native limit.");

        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Mode);
        stream.WriteVariableInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (FriendEntry entry in Entries)
            {
                ArgumentNullException.ThrowIfNull(entry);
                entry.Encode(stream);
            }
        }

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits private entries from diagnostics.
    public override string ToString()
    {
        return nameof(FriendListMessage);
    }
}
