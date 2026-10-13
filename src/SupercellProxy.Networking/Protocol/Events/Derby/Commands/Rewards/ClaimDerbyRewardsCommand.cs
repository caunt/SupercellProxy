using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Claims the selected ordinary, bingo and bunny derby reward choices.</summary>
public sealed record ClaimDerbyRewardsCommand(DerbyRewardSelection Selection) : CodecCommand<ClaimDerbyRewardsCommand>, ICommandCodecProvider<ClaimDerbyRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDerbyRewardsCommand> Codec { get; } = Codecs.Command(
        type: 173,
        Codecs.Composite(DerbyRewardSelection.Codec, static command => command.Selection, static (selection) => new ClaimDerbyRewardsCommand(selection))
    );
}
