using SupercellProxy.Networking.Protocol.CommandEncoding;
using SupercellProxy.Networking.Protocol.CommandEncoding.Registration;
using SupercellProxy.Networking.Transport;


namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Create Roadside Listing Command contract.
/// </summary>
public sealed record CreateRoadsideListingCommand : Command
{
    /// <summary>
    /// Provides the Create Roadside Listing Command value or operation.
    /// </summary>
    public CreateRoadsideListingCommand(bool advertise, int slotIndex, int itemGlobalId, bool usePrimaryInventory, int price, int quantity)
    {
        Advertise = advertise;
        SlotIndex = slotIndex;
        ItemGlobalId = itemGlobalId;
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
    public int ItemGlobalId { get; }

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
    public override int Type => CommandRegistry.CreateRoadsideListingCommandType;

    /// <summary>
    /// Gets the Use Primary Inventory value.
    /// </summary>
    public bool UsePrimaryInventory { get; }

    internal static Version ReorderedFieldsVersion { get; } = new(major: 1, minor: 73, build: 81);

    /// <summary>
    /// Decodes a value from the supplied protocol payload.
    /// </summary>
    public static CreateRoadsideListingCommand Decode(MessageStream stream, CommandEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            int itemGlobalId = stream.ReadVarInt();
            bool advertise = stream.ReadBoolean();
            int slotIndex = stream.ReadVarInt();

            return new CreateRoadsideListingCommand(advertise, slotIndex, itemGlobalId, stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadVarInt());
        }

        return new CreateRoadsideListingCommand(stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadVarInt(), stream.ReadBoolean(), stream.ReadVarInt(), stream.ReadVarInt());
    }

    /// <summary>
    /// Encodes this value using the selected wire format.
    /// </summary>
    public override void Encode(MessageStream stream, CommandEnvironment environment)
    {
        if (stream.GameVersion >= ReorderedFieldsVersion)
        {
            stream.WriteVarInt(ItemGlobalId);
            stream.WriteBoolean(Advertise);
            stream.WriteVarInt(SlotIndex);
        }
        else
        {
            stream.WriteBoolean(Advertise);
            stream.WriteVarInt(SlotIndex);
            stream.WriteVarInt(ItemGlobalId);
        }

        stream.WriteBoolean(UsePrimaryInventory);
        stream.WriteVarInt(Price);
        stream.WriteVarInt(Quantity);
    }
}
