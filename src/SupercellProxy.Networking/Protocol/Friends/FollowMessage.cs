using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Friends;

/// Requests that the client begin following the identified farm.
public sealed record FollowMessage(LongIdentifier HomeIdentifier) : IMessage
{
    /// Decodes the target farm identifier.
    public static FollowMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        FollowMessage result = new(container.Payload.ReadLongIdentifier());

        return container.Payload.Position != container.Payload.Length
            ? throw new InvalidDataException(message: "The follow request has trailing data.")
            : result;
    }

    /// Encodes the target farm identifier using the supplied wire version.
    public MessageContainer ToContainer(ushort identifier, ushort version = 0)
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(HomeIdentifier);

        return new MessageContainer(identifier, version, stream);
    }

    /// Omits the private farm identifier from diagnostic text.
    public override string ToString()
    {
        return nameof(FollowMessage);
    }
}
