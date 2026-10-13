using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>Reports that a helper completed a truck order of another home.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="HelperHomeId">The helper home id.</param>
/// <param name="OrderIndex">The order index.</param>
/// <param name="OwnerRewards">The owner rewards.</param>
/// <param name="HelperRewards">The helper rewards.</param>
public sealed record RemoteOrderCompletionServerCommand(
    LongId OwnerHomeId,
    LongId HelperHomeId,
    int OrderIndex,
    ReadOnlyMemory<OrderReward> OwnerRewards,
    ReadOnlyMemory<OrderReward> HelperRewards
) : CodecServerCommand<RemoteOrderCompletionServerCommand>, ICommandCodecProvider<RemoteOrderCompletionServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RemoteOrderCompletionServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.OrderIndex,
            Codecs.Memory(OrderReward.Codec),
            static command => command.OwnerRewards,
            Codecs.Memory(OrderReward.Codec),
            static command => command.HelperRewards,
            static (ownerHomeId, helperHomeId, orderIndex, ownerRewards, helperRewards) => new RemoteOrderCompletionServerCommand(ownerHomeId, helperHomeId, orderIndex, ownerRewards, helperRewards)
        )
    );
}
