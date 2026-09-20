using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests the follower count of the identified farm.
public sealed record RequestFollowerCountMessage(LongIdentifier HomeIdentifier) : IMessage
{
    /// Decodes the fixed-width farm identifier.
    public static RequestFollowerCountMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        RequestFollowerCountMessage result = new(container.Payload.ReadLongIdentifier());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The follower-count request has trailing data.")
            : result;
    }

    /// Encodes the farm identifier using the supplied wire version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(HomeIdentifier);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the private farm identifier from diagnostic text.
    public override string ToString()
    {
        return nameof(RequestFollowerCountMessage);
    }
}
