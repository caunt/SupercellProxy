using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Clientbound neighborhood chat entry; the item-request form is currently proven.</summary>
public sealed record NeighborhoodChatMessage(NeighborhoodItemRequestChatEntry ItemRequest) : IMessage
{
    private const int ItemRequestEntryType = 7;

    /// <summary>Decodes the proven item-request chat entry.</summary>
    public static NeighborhoodChatMessage Create(MessageContainer container)
    {
        ArgumentNullException.ThrowIfNull(container);
        MessageStream stream = container.Payload;
        int entryType = stream.ReadVariableInt();

        if (entryType != ItemRequestEntryType)
            throw new NotSupportedException($"Neighborhood chat entry type {entryType} is not implemented.");

        NeighborhoodItemRequestChatEntry entry = NeighborhoodItemRequestChatEntry.Decode(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood chat message has trailing data.")
            : new NeighborhoodChatMessage(entry);
    }

    /// <summary>Encodes the proven item-request chat entry.</summary>
    public MessageStream ToStream()
    {
        using MessageStream stream = MessageStream.Create();

        stream.WriteVariableInt(ItemRequestEntryType);
        ItemRequest.Encode(stream);

        return stream;
    }

    /// <summary>Omits player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodChatMessage);
    }
}
