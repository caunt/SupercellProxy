using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Discards one truck order and starts its free replacement countdown.</summary>
public sealed record CancelTruckOrderCommand(int OrderIndex) : CodecCommand<CancelTruckOrderCommand>, ICommandCodecProvider<CancelTruckOrderCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CancelTruckOrderCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.Byte, static command => (byte)command.OrderIndex, static orderIndex => new CancelTruckOrderCommand(orderIndex))
    );
}
