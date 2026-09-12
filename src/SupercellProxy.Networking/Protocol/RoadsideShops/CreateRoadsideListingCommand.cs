using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Create Roadside Listing Command contract.
/// </summary>
public sealed record CreateRoadsideListingCommand : Command
{
    /// <summary>
    /// Provides the Command Type value or operation.
    /// </summary>
    public const int CommandType = 574;

    /// <summary>
    /// Provides the Create Roadside Listing Command value or operation.
    /// </summary>
    public CreateRoadsideListingCommand(
        bool advertise,
        int slotIndex,
        int itemGlobalIdentifier,
        bool usePrimaryInventory,
        int price,
        int quantity,
        int executionPhaseCounter = -1,
        CommandData? debugData0 = null,
        CommandData? debugData1 = null
    )
        : base(executionPhaseCounter, debugData0, debugData1)
    {
        Advertise = advertise;
        SlotIndex = slotIndex;
        ItemGlobalIdentifier = itemGlobalIdentifier;
        UsePrimaryInventory = usePrimaryInventory;
        Price = price;
        Quantity = quantity;
    }

    /// <summary>
    /// Gets the Advertise value.
    /// </summary>
    public bool Advertise { get; }

    /// <summary>
    /// Gets the Item Global Id value.
    /// </summary>
    [System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalIdentifier { get; }

    /// <summary>
    /// Gets the Price value.
    /// </summary>
    public int Price { get; }

    /// <summary>
    /// Gets the Quantity value.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the Slot Index value.
    /// </summary>
    public int SlotIndex { get; }

    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandType;

    /// <summary>
    /// Gets the Use Primary Inventory value.
    /// </summary>
    public bool UsePrimaryInventory { get; }

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CreateRoadsideListingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) fields = DecodeCommand(stream, environment);

        return new CreateRoadsideListingCommand(
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            stream.ReadBoolean(),
            stream.ReadVariableInt(),
            stream.ReadVariableInt(),
            fields.ExecutionPhaseCounter,
            fields.DebugData0,
            fields.DebugData1
        );
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void EncodeBody(MessageStream stream, CommandEnvironment environment)
    {
        EncodeCommand(stream, environment);
        stream.WriteBoolean(Advertise);
        stream.WriteVariableInt(SlotIndex);
        stream.WriteVariableInt(ItemGlobalIdentifier);
        stream.WriteBoolean(UsePrimaryInventory);
        stream.WriteVariableInt(Price);
        stream.WriteVariableInt(Quantity);
    }
}
