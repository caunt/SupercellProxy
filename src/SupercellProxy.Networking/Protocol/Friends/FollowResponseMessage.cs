using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Reports the result of a follow relationship operation and its optional friend entry.
public sealed record FollowResponseMessage(FollowResultCode Result, FollowOperation Operation, FriendEntry? Entry) : IMessage
{
    /// Decodes the result, operation, and optional entry in native wire order.
    public static FollowResponseMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        FollowResponseMessage result = new(
            new FollowResultCode(stream.ReadVarInt()),
            new FollowOperation(stream.ReadVarInt()),
            stream.ReadBoolean() ? FriendEntry.Decode(stream) : null
        );

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follow response has trailing data.")
            : result;
    }

    /// Encodes the result, operation, and optional entry as a payload.
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(Result.Value);
        stream.WriteVarInt(Operation.Value);
        stream.WriteBoolean(Entry is not null);
        Entry?.Encode(stream);
    }

    /// Omits the private friend entry from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowResponseMessage);
    }
}
