using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries an ordered follower page and whether the server has more pages available.
public sealed record FollowerListPageMessage(bool HasMorePages, FriendEntry[]? Entries) : IMessage
{
    /// Decodes the pagination flag and nullable, var-integer-counted entry collection.
    public static FollowerListPageMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        bool hasMorePages = stream.ReadBoolean();
        int count = stream.ReadVarInt();

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

    /// Encodes this page as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteBoolean(HasMorePages);
        stream.WriteVarInt(Entries?.Length ?? -1);

        if (Entries is not null)
        {
            foreach (FriendEntry entry in Entries)
            {
                ArgumentNullException.ThrowIfNull(entry);
                entry.Encode(stream);
            }
        }
    }

    /// Omits the private follower records from diagnostics.
    public override string ToString()
    {
        return nameof(FollowerListPageMessage);
    }
}
