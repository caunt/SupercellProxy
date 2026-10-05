using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Represents <c language="csharp">RoadsideShopEntry</c>.
/// </summary>
public sealed record RoadsideShopEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongId? BuyerId,
    bool IsAdvertised,
    int Price,
    int Quantity,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalId
)
{
    /// <summary>
    /// Gets the Has Buyer value.
    /// </summary>
    public bool HasBuyer => BuyerId is { } buyer && buyer != LongId.Empty;

    /// <summary>
    /// Gets the Is Available value.
    /// </summary>
    public bool IsAvailable => ItemGlobalId is not 0 && Quantity > 0 && !HasBuyer;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideShopEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(stream.ReadOptionalLongId(), stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongId(BuyerId);
        stream.WriteBoolean(IsAdvertised);
        stream.WriteVarInt(Price);
        stream.WriteVarInt(Quantity);
        stream.WriteVarInt(ItemGlobalId);
    }
}
