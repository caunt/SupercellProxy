using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries the number of followers of the identified farm.
public sealed record FollowerCountMessage(LongId HomeId, int FollowerCount) : IMessage
{
    /// Decodes the farm id followed by its signed var-integer count.
    public static FollowerCountMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        FollowerCountMessage result = new(stream.ReadLongId(), stream.ReadVarInt());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follower count has trailing data.")
            : result;
    }

    /// Encodes the farm id and follower count as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(HomeId);
        stream.WriteVarInt(FollowerCount);
    }

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowerCountMessage);
    }
}
