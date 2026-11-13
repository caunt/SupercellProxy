using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Tells a seller that one of their roadside stands was bought; the stand records the buyer until the seller collects the coins.
/// </summary>
public sealed record RoadsideSaleServerCommand : CodecServerCommand<RoadsideSaleServerCommand>, ICommandCodecProvider<RoadsideSaleServerCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RoadsideSaleServerCommand"/> record. Creates the command for one sold stand.</summary>
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
    public static CommandCodec<RoadsideSaleServerCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.LongId,
        static command => command.BuyerAvatarId,
        Codecs.LongId,
        static command => command.RoadsideOwnerAvatarId,
        Codecs.BigEndianInt32,
        static command => command.ItemGlobalId,
        Codecs.BigEndianInt32,
        static command => command.SlotIndex,
        Codecs.BigEndianInt32,
        static command => command.Price,
        Codecs.BigEndianInt32,
        static command => command.Quantity,
        static (buyerAvatarId, roadsideOwnerAvatarId, itemGlobalId, slotIndex, price, quantity) => new RoadsideSaleServerCommand(buyerAvatarId, roadsideOwnerAvatarId, itemGlobalId, slotIndex, price, quantity)));

    /// <summary>Gets the home of the player who bought the stand.</summary>
    /// <value>The buyer's home id, the first field on the wire.</value>
    [JsonPropertyName("BuyerAvatarId")]
    public LongId BuyerAvatarId { get; }

    /// <summary>Gets the global id of the sold goods.</summary>
    /// <value>The goods' data id, written as a big-endian 32-bit integer rather than a var-int.</value>
    [JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalId { get; }

    /// <summary>Gets the coin price of the stand.</summary>
    /// <value>The coins for the whole stand, as a big-endian 32-bit integer; the seller receives them when collecting the sale.</value>
    public int Price { get; }

    /// <summary>Gets how many goods were on the stand.</summary>
    /// <value>The number of goods sold together, as a big-endian 32-bit integer and the last field on the wire.</value>
    public int Quantity { get; }

    /// <summary>Gets the home of the seller.</summary>
    /// <value>The seller's home id, written right after the buyer's.</value>
    [JsonPropertyName("RoadsideOwnerAvatarId")]
    public LongId RoadsideOwnerAvatarId { get; }

    /// <summary>Gets the position of the sold stand in the seller's shop.</summary>
    /// <value>The zero-based stand index, as a big-endian 32-bit integer.</value>
    public int SlotIndex { get; }
}
