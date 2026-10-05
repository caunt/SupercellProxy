using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Sets the second state flag on a Neighborhood stream entry selected by its long identifier.</summary>
public sealed record NeighborhoodStreamEntryFlagMessage(LongIdentifier EntryIdentifier) : IMessage
{
    /// <summary>Decodes the selected stream-entry identifier.</summary>
    public static NeighborhoodStreamEntryFlagMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        LongIdentifier identifier = stream.ReadLongIdentifier();

        return stream.Position == stream.Length
            ? new NeighborhoodStreamEntryFlagMessage(identifier)
            : throw new InvalidDataException(message: "The Neighborhood stream-entry flag message has trailing data.");
    }

    /// <summary>Encodes the selected stream-entry identifier.</summary>
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteLongIdentifier(EntryIdentifier);

        return stream;
    }

    /// <summary>Omits the stream-entry identifier from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodStreamEntryFlagMessage);
    }
}
