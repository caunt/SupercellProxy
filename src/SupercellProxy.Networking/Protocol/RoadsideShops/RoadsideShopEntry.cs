using SupercellProxy.Networking.Protocol.Serialization;

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
) : ICodecProvider<RoadsideShopEntry>
{
    /// <inheritdoc />
    public static ICodec<RoadsideShopEntry> Codec { get; } = Codecs.Composite(
        Codecs.OptionalLongId,
        static value => value.BuyerId,
        Codecs.Boolean,
        static value => value.IsAdvertised,
        Codecs.VarInt,
        static value => value.Price,
        Codecs.VarInt,
        static value => value.Quantity,
        Codecs.VarInt,
        static value => value.ItemGlobalId,
        static (buyerId, isAdvertised, price, quantity, itemGlobalId) => new RoadsideShopEntry(buyerId, isAdvertised, price, quantity, itemGlobalId)
    );

    /// <summary>
    /// Gets the Has Buyer value.
    /// </summary>
    public bool HasBuyer => BuyerId is { } buyer && buyer != LongId.Empty;

    /// <summary>
    /// Gets the Is Available value.
    /// </summary>
    public bool IsAvailable => ItemGlobalId is not 0 && Quantity > 0 && !HasBuyer;
}
