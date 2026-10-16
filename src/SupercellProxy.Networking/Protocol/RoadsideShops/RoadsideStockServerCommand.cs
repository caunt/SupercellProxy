using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.RoadsideShops;

/// <summary>
/// Pushes a new or cleared stand of the visited farm's roadside shop to its visitors, replacing their copy of that stand.
/// </summary>
/// <param name="HomeOwnerId">Home that owns the shop; updates for other shops are ignored.</param>
/// <param name="ItemGlobalId">Global id of the goods now on the stand; 0, with price and quantity 0, clears the stand.</param>
/// <param name="SlotIndex">Position of the stand; a position past the known stands adds the missing stands first.</param>
/// <param name="Price">Coin price of the whole stand.</param>
/// <param name="Quantity">How many goods are on the stand.</param>
public sealed record RoadsideStockServerCommand(
    [property: JsonPropertyName("HomeOwnerId")] LongId HomeOwnerId,
    [property: JsonPropertyName("ItemGlobalId")] int ItemGlobalId,
    int SlotIndex,
    int Price,
    int Quantity
) : CodecServerCommand<RoadsideStockServerCommand>, ICommandCodecProvider<RoadsideStockServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RoadsideStockServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.HomeOwnerId,
            Codecs.Int32,
            static command => command.ItemGlobalId,
            Codecs.Int32,
            static command => command.SlotIndex,
            Codecs.Int32,
            static command => command.Price,
            Codecs.Int32,
            static command => command.Quantity,
            static (owner, item, slot, price, quantity) => new RoadsideStockServerCommand(owner, item, slot, price, quantity)
        )
    );
}
