using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// <para>Records the sale of one roadside-shop listing.</para>
/// </summary>
public sealed record RoadsideSaleServerCommand : ServerCommand
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

    /// <inheritdoc />
    public override int Type => CommandRegistry.RoadsideSaleServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideSaleServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongId buyerAvatarId = stream.ReadLongId();
        LongId roadsideOwnerAvatarId = stream.ReadLongId();
        int itemGlobalId = stream.ReadInt32();
        int slotIndex = stream.ReadInt32();
        int price = stream.ReadInt32();
        int quantity = stream.ReadInt32();


        return new RoadsideSaleServerCommand(buyerAvatarId, roadsideOwnerAvatarId, itemGlobalId, slotIndex, price, quantity);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(BuyerAvatarId);
        stream.WriteLongId(RoadsideOwnerAvatarId);
        stream.WriteInt32(ItemGlobalId);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Price);
        stream.WriteInt32(Quantity);
    }
}
