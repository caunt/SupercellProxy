using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests the follower count of the identified farm.
public sealed record RequestFollowerCountMessage(LongId HomeId) : IMessage
{
    /// Decodes the fixed-width farm id.
    public static RequestFollowerCountMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        RequestFollowerCountMessage result = new(stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follower-count request has trailing data.")
            : result;
    }

    /// Encodes the farm id as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(HomeId);
    }

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(RequestFollowerCountMessage);
    }
}
