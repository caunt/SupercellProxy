using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// <summary>Opens the friend book; the server answers with the friend list and pending friend requests.</summary>
public sealed record RequestFriendListMessage : IMessage
{
    /// <summary>Decodes the empty request.</summary>
    public static RequestFriendListMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new RequestFriendListMessage();
    }

    /// <summary>Encodes the empty request.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
    }
}
