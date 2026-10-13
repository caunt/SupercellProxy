using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>
/// Defines the Start Truck Delivery Command contract.
/// </summary>
/// <summary>
/// Defines the Order Index contract.
/// </summary>
public sealed record StartTruckDeliveryCommand(int OrderIndex) : CodecCommand<StartTruckDeliveryCommand>, ICommandCodecProvider<StartTruckDeliveryCommand>
{
    /// <inheritdoc />
    public static CommandCodec<StartTruckDeliveryCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(Codecs.VarInt, static command => command.OrderIndex, static (orderIndex) => new StartTruckDeliveryCommand(orderIndex)),
        CommandLayout.MetadataFirst
    );
}
