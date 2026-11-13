using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Rewards;

/// <summary>Claims the configured one-time account-link reward.</summary>
/// <param name="Kind">Which reward to claim, written as a 32-bit integer: 1 for the Supercell ID connection rewards, 2 for the Hay Day Pop promotion decoration.</param>
public sealed record ClaimAccountLinkRewardCommand(AccountLinkRewardKind Kind) : CodecCommand<ClaimAccountLinkRewardCommand>, ICommandCodecProvider<ClaimAccountLinkRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimAccountLinkRewardCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.Enum<AccountLinkRewardKind>(Codecs.BigEndianInt32),
        static command => command.Kind,
        static kind => new ClaimAccountLinkRewardCommand(kind)));
}
