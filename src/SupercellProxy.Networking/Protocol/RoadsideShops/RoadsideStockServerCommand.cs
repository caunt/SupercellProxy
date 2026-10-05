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
    [property: System.Text.Json.Serialization.JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId,
    [property: System.Text.Json.Serialization.JsonPropertyName("ItemGlobalId")] int ItemGlobalId,
    int SlotIndex,
    int Price,
    int Quantity
) : ServerCommand
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
        LongId owner = stream.ReadLongId();
        int item = stream.ReadInt32();
        int slot = stream.ReadInt32();
        int price = stream.ReadInt32();
        int quantity = stream.ReadInt32();


        return new RoadsideStockServerCommand(owner, item, slot, price, quantity);
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        stream.WriteLongId(HomeOwnerId);
        stream.WriteInt32(ItemGlobalId);
        stream.WriteInt32(SlotIndex);
        stream.WriteInt32(Price);
        stream.WriteInt32(Quantity);
    }
}
