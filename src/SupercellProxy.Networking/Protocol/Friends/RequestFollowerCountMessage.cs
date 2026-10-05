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

    /// Encodes the farm identifier as a payload.
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(HomeIdentifier);

        return stream;
    }

    /// Omits the private farm identifier from diagnostic text.
    public override string ToString()
    {
        return nameof(RequestFollowerCountMessage);
    }
}
