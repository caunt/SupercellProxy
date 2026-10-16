using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>One stand of a roadside shop: the goods on it, their price, and who bought them.</summary>
/// <param name="BuyerId">Home of the player who bought the stand; null or empty while it is unsold.</param>
/// <param name="IsAdvertised">Whether the stand is advertised in the newspaper.</param>
/// <param name="Price">Coin price of the whole stand.</param>
/// <param name="Quantity">How many goods are on the stand.</param>
/// <param name="ItemGlobalId">Global id of the goods on the stand; 0 for an empty stand.</param>
public sealed record RoadsideShopEntry(
    [property: JsonPropertyName("BuyerId")] LongId? BuyerId,
    bool IsAdvertised,
    int Price,
    int Quantity,
    [property: JsonPropertyName("ItemGlobalId")] int ItemGlobalId
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

    /// <summary>Whether someone bought the stand, so its coins wait for the seller to collect them.</summary>
    public bool HasBuyer => BuyerId is { } buyer && buyer != LongId.Empty;

    /// <summary>Whether the stand holds goods nobody bought yet, so a visitor can buy it.</summary>
    public bool IsAvailable => ItemGlobalId is not 0 && Quantity > 0 && !HasBuyer;
}
