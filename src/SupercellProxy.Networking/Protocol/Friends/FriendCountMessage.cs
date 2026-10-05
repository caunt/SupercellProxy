using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Reports a farm's friend count to the client's friend manager.
public sealed record FriendCountMessage(LongId HomeId, int FriendCount) : IMessage
{
    /// Decodes the farm id followed by its signed var-integer count.
    public static FriendCountMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        FriendCountMessage result = new(stream.ReadLongId(), stream.ReadVarInt());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The friend count has trailing data.")
            : result;
    }

    /// Encodes the farm id and friend count as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(HomeId);
        stream.WriteVarInt(FriendCount);
    }

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(FriendCountMessage);
    }
}
