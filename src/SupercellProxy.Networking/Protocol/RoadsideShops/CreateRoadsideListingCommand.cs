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
    /// <summary>Initializes a new instance of the <see cref="CreateRoadsideListingCommand"/> record. Creates the command for one stand.</summary>
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
    public static CommandCodec<CreateRoadsideListingCommand> Codec { get; } = Codecs.Command(Codecs.Versioned(Release17381.Version,
            Codecs.Composite(Codecs.VarInt,
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
                static (itemGlobalId, advertise, slotIndex, usePrimaryInventory, price, quantity) => new CreateRoadsideListingCommand(advertise, slotIndex, itemGlobalId, usePrimaryInventory, price, quantity)),
            Codecs.Composite(Codecs.Boolean,
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
                static (advertise, slotIndex, itemGlobalId, usePrimaryInventory, price, quantity) => new CreateRoadsideListingCommand(advertise, slotIndex, itemGlobalId, usePrimaryInventory, price, quantity))),
        CommandLayout.MetadataFirst);

    /// <summary>
    /// Gets a value indicating whether the stand is also advertised in the newspaper.
    /// </summary>
    /// <value><see langword="true"/> if the new stand is advertised by this same command; otherwise, <see langword="false"/>.</value>
    public bool Advertise { get; }

    /// <summary>Gets the global id of the goods put up for sale.</summary>
    /// <value>The goods' data id, written as a var-int: the first field from release 1.73.81 on, and the third, after the stand index, before it.</value>
    [JsonPropertyName("ItemGlobalId")]
    public int ItemGlobalId { get; }

    /// <summary>Gets the coin price of the whole stand.</summary>
    /// <value>The coins asked for all <see cref="Quantity"/> goods together, not per unit, written as a var-int.</value>
    public int Price { get; }

    /// <summary>Gets how many goods go on the stand.</summary>
    /// <value>The count of goods put on the stand, written as a var-int.</value>
    public int Quantity { get; }

    /// <summary>Gets the position of the empty stand in the shop.</summary>
    /// <value>The zero-based stand index, written as a var-int.</value>
    public int SlotIndex { get; }

    /// <summary>Gets a value indicating whether the goods come only from the main inventory instead of all inventories together.</summary>
    /// <value><see langword="true"/> if the goods are taken from the main inventory alone rather than spent from all inventories combined; otherwise, <see langword="false"/>.</value>
    public bool UsePrimaryInventory { get; }
}
