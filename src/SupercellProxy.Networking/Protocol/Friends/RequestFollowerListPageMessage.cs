using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests the next follower page; the server owns the cursor for this connection.
public sealed record RequestFollowerListPageMessage : IMessage
{
    /// Requires the native empty request payload.
    public static RequestFollowerListPageMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The follower-page request has trailing data.")
            : new RequestFollowerListPageMessage();
    }

    /// Encodes an empty payload using the supplied identifier and version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        return new MessageContainer(identifier, version, stream);
    }

    /// Identifies the request without exposing client data.
    public override string ToString()
    {
        return nameof(RequestFollowerListPageMessage);
    }
}
