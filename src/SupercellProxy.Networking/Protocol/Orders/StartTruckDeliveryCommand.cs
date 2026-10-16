using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Serverbound command sending the delivery truck off with one order: the order's goods leave the barn and silo, unless another
/// player already filled it, and its rewards wait for collection.
/// </summary>
/// <param name="OrderIndex">Position of the order on the order board.</param>
public sealed record StartTruckDeliveryCommand(int OrderIndex) : CodecCommand<StartTruckDeliveryCommand>, ICommandCodecProvider<StartTruckDeliveryCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartTruckDeliveryCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OrderIndex, static orderIndex => new StartTruckDeliveryCommand(orderIndex)),
        CommandLayout.MetadataFirst
    );
}
