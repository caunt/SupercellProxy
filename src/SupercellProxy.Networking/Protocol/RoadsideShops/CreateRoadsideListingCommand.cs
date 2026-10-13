using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Defines the Create Roadside Listing Command contract.
/// </summary>
public sealed record CreateRoadsideListingCommand
    : CodecCommand<CreateRoadsideListingCommand>, ICommandCodecProvider<CreateRoadsideListingCommand>
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

    /// <inheritdoc />
    public static CommandCodec<CreateRoadsideListingCommand> Codec { get; } = Codecs.Command(
        Codecs.Versioned(
            Release17381.Version,
            Codecs.Composite(
                Codecs.VarInt,
                static command => command.ItemGlobalId,
                Codecs.Boolean,
                static command => command.Advertise,
                Codecs.VarInt,
                static command => command.SlotIndex,
                Codecs.Boolean,
                static command => command.UsePrimaryInventory,
                Codecs.VarInt,
                static command => command.Price,
                Codecs.VarInt,
                static command => command.Quantity,
                static (itemGlobalId, advertise, slotIndex, usePrimaryInventory, price, quantity) => new CreateRoadsideListingCommand(advertise, slotIndex, itemGlobalId, usePrimaryInventory, price, quantity)
            ),
            Codecs.Composite(
                Codecs.Boolean,
                static command => command.Advertise,
                Codecs.VarInt,
                static command => command.SlotIndex,
                Codecs.VarInt,
                static command => command.ItemGlobalId,
                Codecs.Boolean,
                static command => command.UsePrimaryInventory,
                Codecs.VarInt,
                static command => command.Price,
                Codecs.VarInt,
                static command => command.Quantity,
                static (advertise, slotIndex, itemGlobalId, usePrimaryInventory, price, quantity) => new CreateRoadsideListingCommand(advertise, slotIndex, itemGlobalId, usePrimaryInventory, price, quantity)
            )
        ),
        CommandLayout.MetadataFirst
    );

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
    /// Gets the Use Primary Inventory value.
    /// </summary>
    public bool UsePrimaryInventory { get; }
}
