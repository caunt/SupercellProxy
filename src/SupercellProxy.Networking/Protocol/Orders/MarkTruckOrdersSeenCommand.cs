using System.Text.Json.Serialization;

using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Serverbound command clearing the "new" badge of every order on the order board once the player opened it.</summary>
/// <param name="OrderTableGlobalId">Global id of the order board's game object.</param>
public sealed record MarkTruckOrdersSeenCommand([property: JsonPropertyName("OrderTableGlobalId")] int OrderTableGlobalId) : CodecCommand<MarkTruckOrdersSeenCommand>, ICommandCodecProvider<MarkTruckOrdersSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkTruckOrdersSeenCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OrderTableGlobalId, static table => new MarkTruckOrdersSeenCommand(table))
    );
}
