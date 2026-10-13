using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>Reports that a neighbor helped one of the avatar's truck orders.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="HelperHomeId">The helper home id.</param>
/// <param name="OrderIndex">The order index.</param>
/// <param name="OwnerRewards">The owner rewards.</param>
/// <param name="HelperRewards">The helper rewards.</param>
/// <param name="HelpId">The help id.</param>
public sealed record TruckOrderHelpReceivedServerCommand(
    LongId OwnerHomeId,
    LongId HelperHomeId,
    int OrderIndex,
    ReadOnlyMemory<TruckHelpReward> OwnerRewards,
    ReadOnlyMemory<TruckHelpReward> HelperRewards,
    int HelpId
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
            static command => command.OrderIndex,
            Codecs.Memory(TruckHelpReward.Codec),
            static command => command.OwnerRewards,
            Codecs.Memory(TruckHelpReward.Codec),
            static command => command.HelperRewards,
            Codecs.VarInt,
            static command => command.HelpId,
            static (ownerHomeId, helperHomeId, orderIndex, ownerRewards, helperRewards, helpId) => new TruckOrderHelpReceivedServerCommand(ownerHomeId, helperHomeId, orderIndex, ownerRewards, helperRewards, helpId)
        )
    );
}
