using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Tells a seller that one of their roadside stands was bought; the stand records the buyer until the seller collects the coins.
/// </summary>
public sealed record RoadsideSaleServerCommand : CodecServerCommand<RoadsideSaleServerCommand>, ICommandCodecProvider<RoadsideSaleServerCommand>
{
    /// <summary>Creates the command for one sold stand.</summary>
    /// <param name="buyerAvatarId">Home of the player who bought the stand.</param>
    /// <param name="roadsideOwnerAvatarId">Home of the seller.</param>
    /// <param name="itemGlobalId">Global id of the sold goods.</param>
    /// <param name="slotIndex">Position of the sold stand in the seller's shop.</param>
    /// <param name="price">Coin price of the stand.</param>
    /// <param name="quantity">How many goods were on the stand.</param>
    public RoadsideSaleServerCommand(LongId buyerAvatarId, LongId roadsideOwnerAvatarId, int itemGlobalId, int slotIndex, int price, int quantity)
    {
        BuyerAvatarId = buyerAvatarId;
        RoadsideOwnerAvatarId = roadsideOwnerAvatarId;
        ItemGlobalId = itemGlobalId;
        SlotIndex = slotIndex;
        Price = price;
        Quantity = quantity;
    }

    /// <inheritdoc />
    public static CommandCodec<RoadsideSaleServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.BuyerAvatarId,
            Codecs.LongId,
            static command => command.RoadsideOwnerAvatarId,
            Codecs.Int32,
            static command => command.ItemGlobalId,
            Codecs.Int32,
            static command => command.SlotIndex,
            Codecs.Int32,
            static command => command.Price,
            Codecs.Int32,
            static command => command.Quantity,
            static (buyerAvatarId, roadsideOwnerAvatarId, itemGlobalId, slotIndex, price, quantity) => new RoadsideSaleServerCommand(buyerAvatarId, roadsideOwnerAvatarId, itemGlobalId, slotIndex, price, quantity)
        )
    );

    /// <summary>Home of the player who bought the stand.</summary>
    [JsonPropertyName("BuyerAvatarId")]
    public LongId BuyerAvatarId { get; }

    /// <summary>Global id of the sold goods; the stand must still hold them for the sale to apply.</summary>
    [JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalId { get; }

    /// <summary>Coin price of the stand; it must match the stand for the sale to apply.</summary>
    public int Price { get; }

    /// <summary>How many goods were on the stand; it must match the stand for the sale to apply.</summary>
    public int Quantity { get; }

    /// <summary>Home of the seller; other homes ignore the command.</summary>
    [JsonPropertyName("RoadsideOwnerAvatarId")]
    public LongId RoadsideOwnerAvatarId { get; }

    /// <summary>Position of the sold stand in the seller's shop.</summary>
    public int SlotIndex { get; }
}
