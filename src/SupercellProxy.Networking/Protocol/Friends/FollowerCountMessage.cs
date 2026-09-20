using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Carries the number of followers of the identified farm.
public sealed record FollowerCountMessage(LongIdentifier HomeIdentifier, int FollowerCount) : IMessage
{
    /// Decodes the farm identifier followed by its signed variable-integer count.
    public static FollowerCountMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        FollowerCountMessage result = new(container.Payload.ReadLongIdentifier(), container.Payload.ReadVariableInt());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The follower count has trailing data.")
            : result;
    }

    /// Encodes the farm identifier and follower count using the supplied wire version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(HomeIdentifier);
        stream.WriteVariableInt(FollowerCount);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the private farm identifier from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowerCountMessage);
    }
}
