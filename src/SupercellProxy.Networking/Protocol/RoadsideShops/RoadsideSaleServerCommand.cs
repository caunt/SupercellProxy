using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Records the sale of one roadside-shop listing.</para>
/// </summary>
public sealed record RoadsideSaleServerCommand : CodecServerCommand<RoadsideSaleServerCommand>, ICommandCodecProvider<RoadsideSaleServerCommand>
{
    /// <summary>
    /// <para>Initializes a roadside-sale server command.</para>
    /// </summary>
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
        type: 375,
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

    /// <summary>
    /// <para>Gets the buyer's avatar id.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("BuyerAvatarId")]
    public LongId BuyerAvatarId { get; }

    /// <summary>
    /// <para>Gets the sold item's data id.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalId { get; }

    /// <summary>
    /// <para>Gets the listing price.</para>
    /// </summary>
    public int Price { get; }

    /// <summary>
    /// <para>Gets the sold quantity.</para>
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// <para>Gets the roadside-shop owner's avatar id.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("RoadsideOwnerAvatarId")]
    public LongId RoadsideOwnerAvatarId { get; }

    /// <summary>
    /// <para>Gets the sold roadside-shop slot.</para>
    /// </summary>
    public int SlotIndex { get; }
}
