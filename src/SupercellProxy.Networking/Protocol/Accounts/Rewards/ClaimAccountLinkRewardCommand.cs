using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Accounts.Rewards;

/// <summary>Claims the configured one-time account-link reward.</summary>
public sealed record ClaimAccountLinkRewardCommand(AccountLinkRewardKind Kind) : CodecCommand<ClaimAccountLinkRewardCommand>, ICommandCodecProvider<ClaimAccountLinkRewardCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimAccountLinkRewardCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.Enum<AccountLinkRewardKind>(Codecs.Int32),
            static command => command.Kind,
            static (kind) => new ClaimAccountLinkRewardCommand(kind)
        )
    );
}
