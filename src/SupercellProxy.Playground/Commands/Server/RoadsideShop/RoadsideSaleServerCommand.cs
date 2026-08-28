using SupercellProxy.Playground.Logic;
using SupercellProxy.Playground.Network.Transport;

namespace SupercellProxy.Playground.Commands;

/// <summary>
/// <para>Records the sale of one roadside-shop listing.</para>
/// </summary>
internal sealed record RoadsideSaleServerCommand : ServerCommand
{
    /// <summary>
    /// <para>Initializes a roadside-sale server command.</para>
    /// </summary>
    public RoadsideSaleServerCommand(
        LongId buyerAvatarId,
        LongId roadsideOwnerAvatarId,
        int itemGlobalId,
        int slotIndex,
        int price,
        int quantity,
        int serverCommandId,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandId, executionPhaseCounter, debugData0, debugData1)
    {
        BuyerAvatarId = buyerAvatarId;
        RoadsideOwnerAvatarId = roadsideOwnerAvatarId;
        ItemGlobalId = itemGlobalId;
        SlotIndex = slotIndex;
        Price = price;
        Quantity = quantity;
    }

    /// <inheritdoc />
    public override int Type => 375;

    /// <summary>
    /// <para>Gets the buyer's avatar identifier.</para>
    /// </summary>
    public LongId BuyerAvatarId { get; }

    /// <summary>
    /// <para>Gets the roadside-shop owner's avatar identifier.</para>
    /// </summary>
    public LongId RoadsideOwnerAvatarId { get; }

    /// <summary>
    /// <para>Gets the sold item's data identifier.</para>
    /// </summary>
    public int ItemGlobalId { get; }

    /// <summary>
    /// <para>Gets the sold roadside-shop slot.</para>
    /// </summary>
    public int SlotIndex { get; }

    /// <summary>
    /// <para>Gets the listing price.</para>
    /// </summary>
    public int Price { get; }

    /// <summary>
    /// <para>Gets the sold quantity.</para>
    /// </summary>
    public int Quantity { get; }

    internal static RoadsideSaleServerCommand Decode(
        MessageStream stream,
        CommandEnvironment environment
    )
    {
        var buyerAvatarId = stream.ReadLongId();
        var roadsideOwnerAvatarId = stream.ReadLongId();
        var itemGlobalId = stream.ReadInt32();
        var slotIndex = stream.ReadInt32();
        var price = stream.ReadInt32();
        var quantity = stream.ReadInt32();
        var commandFields = DecodeServerCommand(stream, environment);
        return new RoadsideSaleServerCommand(
            buyerAvatarId,
            roadsideOwnerAvatarId,
            itemGlobalId,
            slotIndex,
            price,
            quantity,
            commandFields.ServerCommandId,
            commandFields.CommandFields.ExecutionPhaseCounter,
            commandFields.CommandFields.DebugData0,
            commandFields.CommandFields.DebugData1
        );
    }

    internal override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(BuyerAvatarId);
        stream.WriteLongId(RoadsideOwnerAvatarId);
        stream.WriteInt32(ItemGlobalId);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Price);
        stream.WriteInt32(Quantity);
        EncodeServerCommand(stream, environment);
    }
}
