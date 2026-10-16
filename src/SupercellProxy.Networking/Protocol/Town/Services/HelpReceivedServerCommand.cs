using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>
/// Reports that another player helped one of a town's service requests; the owner's request and waiting service record that
/// player as their helper.
/// </summary>
/// <param name="OwnerHomeId">Home that asked for the help; other homes ignore the command.</param>
/// <param name="HelperHomeId">Home of the player who helped.</param>
/// <param name="HelpId">Id of the helped request, matching the help id kept on the service.</param>
/// <param name="HelpKind">Who the request asked; the client does not read it. Requests use 1 town visitors, 2 the neighbourhood, 3 Farm Pass helpers.</param>
/// <param name="OwnerRewards">Rewards listed for the owner; the client does not read them.</param>
/// <param name="HelperRewards">Rewards listed for the helper; the client does not read them.</param>
public sealed record HelpReceivedServerCommand(
    LongId OwnerHomeId,
    LongId HelperHomeId,
    int HelpId,
    int HelpKind,
    ReadOnlyMemory<HelpReward> OwnerRewards,
    ReadOnlyMemory<HelpReward> HelperRewards
) : CodecServerCommand<HelpReceivedServerCommand>, ICommandCodecProvider<HelpReceivedServerCommand>
{
    /// <inheritdoc />
    public static CommandCodec<HelpReceivedServerCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.LongId,
            static command => command.OwnerHomeId,
            Codecs.LongId,
            static command => command.HelperHomeId,
            Codecs.VarInt,
            static command => command.HelpId,
            Codecs.VarInt,
            static command => command.HelpKind,
            Codecs.Memory(HelpReward.Codec),
            static command => command.OwnerRewards,
            Codecs.Memory(HelpReward.Codec),
            static command => command.HelperRewards,
            static (ownerHomeId, helperHomeId, helpId, helpKind, ownerRewards, helperRewards) => new HelpReceivedServerCommand(ownerHomeId, helperHomeId, helpId, helpKind, ownerRewards, helperRewards)
        )
    );
}
