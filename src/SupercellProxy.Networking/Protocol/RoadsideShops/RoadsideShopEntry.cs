using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Represents <c language="csharp">RoadsideShopEntry</c>.
/// </summary>
public sealed record RoadsideShopEntry(
    [property: System.Text.Json.Serialization.JsonPropertyName("BuyerId")] LongIdentifier? BuyerIdentifier,
    bool IsSold,
    int Price,
    int Quantity,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalIdentifier
)
{
    /// <summary>
    /// Gets the Has Buyer value.
    /// </summary>
    public bool HasBuyer => BuyerIdentifier is { } buyer && buyer != LongIdentifier.Empty;

    /// <summary>
    /// Gets the Is Available value.
    /// </summary>
    public bool IsAvailable => ItemGlobalIdentifier is not 0 && Quantity > 0 && !IsSold && !HasBuyer;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideShopEntry Decode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        return new(
            stream.ReadOptionalLongIdentifier(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt()
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public void Encode(MessageStream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        stream.WriteOptionalLongIdentifier(BuyerIdentifier);
        stream.WriteBoolean(IsSold);
        stream.WriteVariableInt(Price);
        stream.WriteVariableInt(Quantity);
        stream.WriteVariableInt(ItemGlobalIdentifier);
    }
}
