using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Transport;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Chat;

/// <summary>Receives an individual neighborhood stream entry.</summary>
public sealed record NeighborhoodChatMessage : IMessage
{
    private const int ItemRequestEntryType = 7;
    private readonly NeighborhoodItemRequestChatEntry? _itemRequest;

    /// <summary>Creates an item-request message.</summary>
    public NeighborhoodChatMessage(NeighborhoodItemRequestChatEntry itemRequest)
    {
        _itemRequest = itemRequest;
    }

    /// <summary>Creates an action-entry message.</summary>
    public NeighborhoodChatMessage(NeighborhoodActionChatEntry action)
    {
        Action = action;
    }

    /// <summary>Gets the action entry, when present.</summary>
    public NeighborhoodActionChatEntry? Action { get; init; }

    /// <summary>Gets whether the entry announces an item request.</summary>
    public bool IsItemRequest => _itemRequest is not null;

    /// <summary>Gets or replaces the item request; unavailable for other entry types.</summary>
    public NeighborhoodItemRequestChatEntry ItemRequest
    {
        get => _itemRequest ?? throw new InvalidOperationException(message: "The chat entry is not an item request.");
        init => _itemRequest = value;
    }

    /// <summary>Decodes a supported native neighborhood stream entry.</summary>
    public static NeighborhoodChatMessage Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        int entryType = stream.ReadVarInt();

        NeighborhoodChatMessage message = entryType == ItemRequestEntryType
            ? new(NeighborhoodItemRequestChatEntry.Decode(stream))
            : NeighborhoodActionChatEntry.Supports(entryType)
            ? new(NeighborhoodActionChatEntry.Decode(entryType, stream))
            : throw new NotSupportedException($"Neighborhood chat entry type {entryType} is not implemented.");

        return stream.Position != stream.Length
            ? throw new InvalidDataException(message: "The neighborhood chat message has trailing data.")
            : message;
    }

    /// <summary>Encodes the entry with its native discriminator.</summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (_itemRequest is { } request && Action is null)
        {
            stream.WriteVarInt(ItemRequestEntryType);
            request.Encode(stream);
        }
        else if (Action is { } action && _itemRequest is null)
        {
            stream.WriteVarInt(action.EntryType);
            action.Encode(stream);
        }
        else
        {
            throw new InvalidDataException(message: "A neighborhood chat message requires exactly one entry.");
        }
    }

    /// <summary>Omits player details from diagnostic text.</summary>
    public override string ToString()
    {
        return nameof(NeighborhoodChatMessage);
    }
}
