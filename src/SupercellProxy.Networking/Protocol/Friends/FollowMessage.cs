using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests that the client begin following the identified farm.
public sealed record FollowMessage(LongId HomeId) : IMessage
{
    /// Decodes the target farm id.
    public static FollowMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        FollowMessage result = new(stream.ReadLongId());

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follow request has trailing data.")
            : result;
    }

    /// Encodes the target farm id as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteLongId(HomeId);
    }

    /// Omits the private farm id from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowMessage);
    }
}
