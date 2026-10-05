using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Clientbound neighborhood chat entry; the item-request form is currently proven.</summary>
public sealed record NeighborhoodChatMessage(NeighborhoodItemRequestChatEntry ItemRequest) : IMessage
{
    private const int ItemRequestEntryType = 7;

    /// <summary>Decodes the proven item-request chat entry.</summary>
    public static NeighborhoodChatMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int entryType = stream.ReadVarInt();

        if (entryType != ItemRequestEntryType)
            throw new NotSupportedException($"Neighborhood chat entry type {entryType} is not implemented.");

        NeighborhoodItemRequestChatEntry entry = NeighborhoodItemRequestChatEntry.Decode(stream);

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood chat message has trailing data.")
            : new NeighborhoodChatMessage(entry);
    }

    /// <summary>Encodes the proven item-request chat entry.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.WriteVarInt(ItemRequestEntryType);
        ItemRequest.Encode(stream);
    }

    /// <summary>Omits player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodChatMessage);
    }
}
