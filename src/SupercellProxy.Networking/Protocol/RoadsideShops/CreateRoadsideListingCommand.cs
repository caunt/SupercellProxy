using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Serverbound command putting goods up for sale on an empty stand of the own roadside shop; the goods leave the barn or silo
/// right away.
/// </summary>
public sealed record CreateRoadsideListingCommand
    : CodecCommand<CreateRoadsideListingCommand>, ICommandCodecProvider<CreateRoadsideListingCommand>
{
    /// <summary>Creates the command for one stand.</summary>
    /// <param name="advertise">Whether to also advertise the stand in the newspaper.</param>
    /// <param name="slotIndex">Position of the empty stand in the shop.</param>
    /// <param name="itemGlobalId">Global id of the goods to sell.</param>
    /// <param name="usePrimaryInventory">Whether the goods come only from the main inventory instead of all inventories together.</param>
    /// <param name="price">Coin price of the whole stand.</param>
    /// <param name="quantity">How many goods go on the stand.</param>
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
    /// Whether the stand is also advertised in the newspaper; that uses a bought advertisement credit when there is one, and
    /// otherwise starts the free advertisement's cooldown.
    /// </summary>
    public bool Advertise { get; }

    /// <summary>Global id of the goods put up for sale.</summary>
    [JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalId { get; }

    /// <summary>Coin price of the whole stand.</summary>
    public int Price { get; }

    /// <summary>How many goods go on the stand.</summary>
    public int Quantity { get; }

    /// <summary>Position of the empty stand in the shop.</summary>
    public int SlotIndex { get; }

    /// <summary>Whether the goods come only from the main inventory instead of all inventories together.</summary>
    public bool UsePrimaryInventory { get; }
}
