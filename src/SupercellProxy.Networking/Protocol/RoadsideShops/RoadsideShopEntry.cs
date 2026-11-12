using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>One stand of a roadside shop: the goods on it, their price, and who bought them.</summary>
/// <param name="BuyerId">Home of the player who bought the stand; <see langword="null"/> or empty while it is unsold.</param>
/// <param name="IsAdvertised">Whether the stand is advertised in the newspaper.</param>
/// <param name="Price">Coin price of the whole stand.</param>
/// <param name="Quantity">How many goods are on the stand.</param>
/// <param name="ItemGlobalId">Global id of the goods on the stand; 0 for an empty stand.</param>
public sealed record RoadsideShopEntry([property: JsonPropertyName("BuyerId")] LongId? BuyerId,
    bool IsAdvertised,
    int Price,
    int Quantity,
    [property: JsonPropertyName("ItemGlobalId")] int ItemGlobalId) : ICodecProvider<RoadsideShopEntry>
{
    /// <inheritdoc />
    public static ICodec<RoadsideShopEntry> Codec { get; } = Codecs.Composite(Codecs.OptionalLongId,
        static value => value.BuyerId,
        Codecs.Boolean,
        static value => value.IsAdvertised,
        Codecs.VarInt,
        static value => value.Price,
        Codecs.VarInt,
        static value => value.Quantity,
        Codecs.VarInt,
        static value => value.ItemGlobalId,
        static (buyerId, isAdvertised, price, quantity, itemGlobalId) => new RoadsideShopEntry(buyerId, isAdvertised, price, quantity, itemGlobalId));

    /// <summary>Gets a value indicating whether someone bought the stand, so its coins wait for the seller to collect them.</summary>
    /// <value><see langword="true"/> if <see cref="BuyerId"/> is neither <see langword="null"/> nor <see cref="LongId.Empty"/>; otherwise, <see langword="false"/>.</value>
    public bool HasBuyer => BuyerId is { } buyer && buyer != LongId.Empty;

    /// <summary>Gets a value indicating whether the stand holds goods nobody bought yet, so a visitor can buy it.</summary>
    /// <value><see langword="true"/> if the stand has a nonzero <see cref="ItemGlobalId"/>, a positive <see cref="Quantity"/> and no buyer; otherwise, <see langword="false"/>.</value>
    public bool IsAvailable => ItemGlobalId is not 0 && Quantity > 0 && !HasBuyer;
}
