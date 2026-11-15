using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders;

/// <summary>Serverbound command collecting the rewards the delivery truck brought back from its finished deliveries.</summary>
public sealed record CollectTruckDeliveryRewardsCommand() : CodecCommand<CollectTruckDeliveryRewardsCommand>, ICommandCodecProvider<CollectTruckDeliveryRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<CollectTruckDeliveryRewardsCommand> Codec { get; } = Codecs.Command(Codecs.Unit(static () => new CollectTruckDeliveryRewardsCommand()), CommandLayout.MetadataFirst);
}
