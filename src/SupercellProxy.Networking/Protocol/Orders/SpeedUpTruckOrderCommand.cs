using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Pays diamonds to replace a truck order slot that is waiting for its next order.</summary>
/// <param name="OrderIndex">The truck order slot index.</param>
public sealed record SpeedUpTruckOrderCommand(int OrderIndex) : CodecCommand<SpeedUpTruckOrderCommand>, ICommandCodecProvider<SpeedUpTruckOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<SpeedUpTruckOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OrderIndex, static orderIndex => new SpeedUpTruckOrderCommand(orderIndex))
    );
}
