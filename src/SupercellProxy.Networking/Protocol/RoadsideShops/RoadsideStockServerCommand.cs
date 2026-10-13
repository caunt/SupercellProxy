using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

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
