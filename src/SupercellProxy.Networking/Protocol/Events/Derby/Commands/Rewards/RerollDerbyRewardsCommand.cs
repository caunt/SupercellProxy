using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Purchases replacement choices for selected derby reward thresholds.</summary>
/// <param name="Selection">The threshold, bingo and bunny reward groups whose choices the player asks to replace.</param>
public sealed record RerollDerbyRewardsCommand(DerbyRewardSelection Selection) : CodecCommand<RerollDerbyRewardsCommand>, ICommandCodecProvider<RerollDerbyRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<RerollDerbyRewardsCommand> Codec { get; } = Codecs.Command(Codecs.Composite(DerbyRewardSelection.Codec, static command => command.Selection, static selection => new RerollDerbyRewardsCommand(selection)));
}
