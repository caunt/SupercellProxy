using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends.Entries;

/// Carries one server-pushed friend-list update.
public sealed record FriendListUpdateMessage(FriendEntry Entry) : IMessage
{
    /// Decodes one complete friend entry.
    public static FriendListUpdateMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        FriendEntry entry = FriendEntry.Decode(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The friend-list update has trailing data.")
            : new FriendListUpdateMessage(entry);
    }

    /// Encodes this update with the supplied identifier and version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        ArgumentNullException.ThrowIfNull(Entry);

        using MessageStream stream = MessageStream.Create();

        Entry.Encode(stream);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the private entry from diagnostics.
    public override string ToString()
    {
        return nameof(FriendListUpdateMessage);
    }
}
