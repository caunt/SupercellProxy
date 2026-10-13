using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>Reports help a neighbor gave to the avatar's visitor or truck order.</summary>
/// <param name="OwnerHomeId">The owner home id.</param>
/// <param name="HelperHomeId">The helper home id.</param>
/// <param name="HelpKind">The help kind.</param>
/// <param name="HelpId">The help id.</param>
/// <param name="OwnerRewards">The owner rewards.</param>
/// <param name="HelperRewards">The helper rewards.</param>
public sealed record HelpReceivedServerCommand(
    LongId OwnerHomeId,
    LongId HelperHomeId,
    int HelpKind,
    int HelpId,
    ReadOnlyMemory<HelpReward> OwnerRewards,
    ReadOnlyMemory<HelpReward> HelperRewards
) : CodecServerCommand<HelpReceivedServerCommand>, ICommandCodecProvider<HelpReceivedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HelpReceivedServerCommand> Codec { get; } = Codecs.Command(
        type: 252,
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.HelpKind,
            Codecs.VarInt,
            static command => command.HelpId,
            Codecs.Memory(HelpReward.Codec),
            static command => command.OwnerRewards,
            Codecs.Memory(HelpReward.Codec),
            static command => command.HelperRewards,
            static (ownerHomeId, helperHomeId, helpKind, helpId, ownerRewards, helperRewards) => new HelpReceivedServerCommand(ownerHomeId, helperHomeId, helpKind, helpId, ownerRewards, helperRewards)
        )
    );
}
