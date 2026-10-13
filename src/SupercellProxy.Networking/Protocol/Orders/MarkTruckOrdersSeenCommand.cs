using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Mark Truck Orders Seen Command contract.
/// </summary>
/// <summary>
/// Defines the Order Table Global Id contract.
/// </summary>
public sealed record MarkTruckOrdersSeenCommand([property: System.Text.Json.Serialization.JsonPropertyName("OrderTableGlobalId")] int OrderTableGlobalId) : CodecCommand<MarkTruckOrdersSeenCommand>, ICommandCodecProvider<MarkTruckOrdersSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTruckOrdersSeenCommand> Codec { get; } = Codecs.Command(
        type: 26,
        Codecs.Composite(Codecs.VarInt, static command => command.OrderTableGlobalId, static (table) => new MarkTruckOrdersSeenCommand(table))
    );
}
