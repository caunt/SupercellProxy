using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// Carries one server-pushed friend-list update.
public sealed record FriendListUpdateMessage(FriendEntry Entry) : IMessage
{
    /// Decodes one complete friend entry.
    public static FriendListUpdateMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        FriendEntry entry = FriendEntry.Decode(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The friend-list update has trailing data.")
            : new FriendListUpdateMessage(entry);
    }

    /// Encodes this update as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        ArgumentNullException.ThrowIfNull(Entry);

        Entry.Encode(stream);
    }

    /// Omits the private entry from diagnostics.
    public override string ToString()
    {
        return nameof(FriendListUpdateMessage);
    }
}
