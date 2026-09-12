using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Roadside Stock Server Command contract.
/// </summary>
/// <summary>
/// Defines the Home Owner Id contract.
/// </summary>
/// <summary>
/// Defines the Item Global Id contract.
/// </summary>
/// <summary>
/// Defines the Slot Index contract.
/// </summary>
/// <summary>
/// Defines the Price contract.
/// </summary>
/// <summary>
/// Defines the Quantity contract.
/// </summary>
public sealed record RoadsideStockServerCommand(
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongIdentifier HomeOwnerIdentifier,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalIdentifier,
    int SlotIndex,
    int Price,
    int Quantity,
    int ServerCommandIdentifier,
    int ExecutionPhaseCounter = -1,
    CommandData? DebugData0 = null,
    CommandData? DebugData1 = null
) : ServerCommand(ServerCommandIdentifier, ExecutionPhaseCounter, DebugData0, DebugData1)
{
    /// <summary>
    /// Gets the Type value.
    /// </summary>
    public override int Type => CommandRegistry.RoadsideStockServerCommandType;

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static RoadsideStockServerCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);
        LongIdentifier owner = stream.ReadLongIdentifier();
        int item = stream.ReadInt32();
        int slot = stream.ReadInt32();
        int price = stream.ReadInt32();
        int quantity = stream.ReadInt32();
        (int serverCommandIdentifier, (int ExecutionPhaseCounter, CommandData? DebugData0, CommandData? DebugData1) commandFields) = DecodeServerCommand(stream, environment);

        return new RoadsideStockServerCommand(
            owner,
            item,
            slot,
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
        stream.WriteLongIdentifier(HomeOwnerIdentifier);
        stream.WriteInt32(ItemGlobalIdentifier);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Price);
        stream.WriteInt32(Quantity);
        EncodeServerCommand(stream, environment);
    }
}
