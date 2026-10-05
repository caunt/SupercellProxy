using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests the next follower page; the server owns the cursor for this connection.
public sealed record RequestFollowerListPageMessage : IMessage
{
    /// Requires the native empty request payload.
    public static RequestFollowerListPageMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follower-page request has trailing data.")
            : new RequestFollowerListPageMessage();
    }

    /// Encodes an empty payload as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
    }

    /// Identifies the request without exposing client data.
    public override string ToString()
    {
        return nameof(RequestFollowerListPageMessage);
    }
}
