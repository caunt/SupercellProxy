using SupercellProxy.Networking.Protocol.Friends.Entries;
using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Reports the result of a follow relationship operation and its optional friend entry.
public sealed record FollowResponseMessage(FollowResultCode Result, FollowOperation Operation, FriendEntry? Entry) : IMessage
{
    /// Decodes the result, operation, and optional entry in native wire order.
    public static FollowResponseMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;

        FollowResponseMessage result = new(
            new FollowResultCode(stream.ReadVariableInt()),
            new FollowOperation(stream.ReadVariableInt()),
            stream.ReadBoolean() ? FriendEntry.Decode(stream) : null
        );

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The follow response has trailing data.")
            : result;
    }

    /// Encodes the result, operation, and optional entry using the supplied wire version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(Result.Value);
        stream.WriteVariableInt(Operation.Value);
        stream.WriteBoolean(Entry is not null);
        Entry?.Encode(stream);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the private friend entry from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowResponseMessage);
    }
}
