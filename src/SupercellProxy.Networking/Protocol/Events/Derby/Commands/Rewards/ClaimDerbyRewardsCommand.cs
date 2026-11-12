using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Claims the selected ordinary, bingo and bunny derby reward choices.</summary>
/// <param name="Selection">The chosen reward index for each earned threshold, bingo and bunny reward.</param>
public sealed record ClaimDerbyRewardsCommand(DerbyRewardSelection Selection) : CodecCommand<ClaimDerbyRewardsCommand>, ICommandCodecProvider<ClaimDerbyRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ClaimDerbyRewardsCommand> Codec { get; } = Codecs.Command(Codecs.Composite(DerbyRewardSelection.Codec, static command => command.Selection, static selection => new ClaimDerbyRewardsCommand(selection)));
}
