using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>Reports that a helper completed a truck order of another home.</summary>
/// <param name="OwnerHomeId">Home whose order was helped; its order records the helper.</param>
/// <param name="HelperHomeId">Home of the player who helped; only that home pays the goods and gets the rewards.</param>
/// <param name="HelpId">The help id of the helped order.</param>
/// <param name="DeliveredGoods">The goods the helper delivers, spent from the helper's storage.</param>
/// <param name="HelperRewards">What the helper gets for helping, such as coins and experience.</param>
public sealed record RemoteOrderCompletionServerCommand(
    LongId OwnerHomeId,
    LongId HelperHomeId,
    int HelpId,
    ReadOnlyMemory<OrderReward> DeliveredGoods,
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
            static command => command.HelpId,
            Codecs.Memory(OrderReward.Codec),
            static command => command.DeliveredGoods,
            Codecs.Memory(OrderReward.Codec),
            static command => command.HelperRewards,
            static (ownerHomeId, helperHomeId, helpId, deliveredGoods, helperRewards) => new RemoteOrderCompletionServerCommand(ownerHomeId, helperHomeId, helpId, deliveredGoods, helperRewards)
        )
    );
}
