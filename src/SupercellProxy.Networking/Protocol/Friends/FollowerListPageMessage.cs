using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries an ordered follower page and whether the server has more pages available.
public sealed record FollowerListPageMessage(bool HasMorePages, FriendEntry[]? Entries) : IMessage
{
    /// Decodes the pagination flag and nullable, variable-integer-counted entry collection.
    public static FollowerListPageMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        bool hasMorePages = stream.ReadBoolean();
        int count = stream.ReadVariableInt();

        if (count < -1 || count > (stream.Length - stream.Position) / FriendEntry.MinimumEncodedSize)
            throw new InvalidDataException(message: "The follower entry count is invalid.");

        FriendEntry[]? entries = count < 0 ? null : new FriendEntry[count];

        if (entries is not null)
        {
            for (int index = 0; index < entries.Length; index++)
                entries[index] = FriendEntry.Decode(stream);
        }

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follower page has trailing data.")
            : new FollowerListPageMessage(hasMorePages, entries);
    }

    /// Encodes this page using the supplied identifier and version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteBoolean(HasMorePages);
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

    /// Omits the private follower records from diagnostics.
    public override string ToString()
    {
        return nameof(FollowerListPageMessage);
    }
}
