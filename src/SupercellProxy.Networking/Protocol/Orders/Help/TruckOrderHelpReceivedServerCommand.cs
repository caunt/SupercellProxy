using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>
/// Reports that another player filled one of the farm's truck orders; the order records that player as its helper, so the owner
/// can send it off without spending goods.
/// </summary>
/// <param name="OwnerHomeId">Home whose order was helped; other homes ignore the command.</param>
/// <param name="HelperHomeId">Home of the player who helped.</param>
/// <param name="HelpId">The help id of the helped order.</param>
/// <param name="OwnerRewards">Rewards listed for the owner; the client does not read them.</param>
/// <param name="HelperRewards">Rewards listed for the helper; the client does not read them.</param>
/// <param name="ListenerValue">A value native passes only to its presentation listener.</param>
public sealed record TruckOrderHelpReceivedServerCommand(
    LongId OwnerHomeId,
    LongId HelperHomeId,
    int HelpId,
    ReadOnlyMemory<TruckHelpReward> OwnerRewards,
    ReadOnlyMemory<TruckHelpReward> HelperRewards,
    int ListenerValue
) : CodecServerCommand<TruckOrderHelpReceivedServerCommand>, ICommandCodecProvider<TruckOrderHelpReceivedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<TruckOrderHelpReceivedServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.HelpId,
            Codecs.Memory(TruckHelpReward.Codec),
            static command => command.OwnerRewards,
            Codecs.Memory(TruckHelpReward.Codec),
            static command => command.HelperRewards,
            Codecs.VarInt,
            static command => command.ListenerValue,
            static (ownerHomeId, helperHomeId, helpId, ownerRewards, helperRewards, listenerValue) => new TruckOrderHelpReceivedServerCommand(ownerHomeId, helperHomeId, helpId, ownerRewards, helperRewards, listenerValue)
        )
    );
}
