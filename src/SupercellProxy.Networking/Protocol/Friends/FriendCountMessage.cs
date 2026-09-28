using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Reports a farm's friend count to the client's friend manager.
public sealed record FriendCountMessage(LongIdentifier HomeIdentifier, int FriendCount) : IMessage
{
    /// Decodes the farm identifier followed by its signed variable-integer count.
    public static FriendCountMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        FriendCountMessage result = new(container.Payload.ReadLongIdentifier(), container.Payload.ReadVariableInt());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The friend count has trailing data.")
            : result;
    }

    /// Encodes the farm identifier and friend count using the supplied wire version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(HomeIdentifier);
        stream.WriteVariableInt(FriendCount);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the private farm identifier from diagnostic text.
    public override string ToString()
    {
        return nameof(FriendCountMessage);
    }
}
