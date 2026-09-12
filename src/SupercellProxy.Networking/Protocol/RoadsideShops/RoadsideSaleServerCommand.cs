using SupercellProxy.Networking.Protocol.CommandEncoding;
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
    public RoadsideSaleServerCommand(
        LongIdentifier buyerAvatarIdentifier,
        LongIdentifier roadsideOwnerAvatarIdentifier,
        int itemGlobalIdentifier,
        int slotIndex,
        int price,
        int quantity,
        int serverCommandIdentifier,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(serverCommandIdentifier, executionPhaseCounter, debugData0, debugData1)
    {
        BuyerAvatarIdentifier = buyerAvatarIdentifier;
        RoadsideOwnerAvatarIdentifier = roadsideOwnerAvatarIdentifier;
        ItemGlobalIdentifier = itemGlobalIdentifier;
        SlotIndex = slotIndex;
        Price = price;
        Quantity = quantity;
    }

    /// <summary>
    /// <para>Gets the buyer's avatar identifier.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("BuyerAvatarId")]
    public LongIdentifier BuyerAvatarIdentifier { get; }

    /// <summary>
    /// <para>Gets the sold item's data identifier.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalIdentifier { get; }

    /// <summary>
    /// <para>Gets the listing price.</para>
    /// </summary>
    public int Price { get; }

    /// <summary>
    /// <para>Gets the sold quantity.</para>
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// <para>Gets the roadside-shop owner's avatar identifier.</para>
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("RoadsideOwnerAvatarId")]
    public LongIdentifier RoadsideOwnerAvatarIdentifier { get; }

    /// <summary>
    /// <para>Gets the sold roadside-shop slot.</para>
    /// </summary>
    public int SlotIndex { get; }

    /// <inheritdoc />
    public override int Type => 375;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideSaleServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier buyerAvatarIdentifier = stream.ReadLongIdentifier();
        LongIdentifier roadsideOwnerAvatarIdentifier = stream.ReadLongIdentifier();
        int itemGlobalIdentifier = stream.ReadInt32();
        int slotIndex = stream.ReadInt32();
        int price = stream.ReadInt32();
        int quantity = stream.ReadInt32();
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new RoadsideSaleServerCommand(
            buyerAvatarIdentifier,
            roadsideOwnerAvatarIdentifier,
            itemGlobalIdentifier,
            slotIndex,
            price,
            quantity,
            serverCommandIdentifier,
            commandFields.ExecutionPhaseCounter,
            commandFields.DebugData0,
            commandFields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongIdentifier(BuyerAvatarIdentifier);
        stream.WriteLongIdentifier(RoadsideOwnerAvatarIdentifier);
        stream.WriteInt32(ItemGlobalIdentifier);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Price);
        stream.WriteInt32(Quantity);
        EncodeServerCommand(stream, environment);
    }
}
