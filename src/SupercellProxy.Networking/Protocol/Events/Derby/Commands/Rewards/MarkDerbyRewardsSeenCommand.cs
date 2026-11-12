using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Records the seen threshold count for one derby reward family.</summary>
/// <param name="Count">How many rewards of the chosen family the player has now seen.</param>
/// <param name="ThresholdRewards"><see langword="true"/> to record the count for the ordinary point-threshold rewards; with both flags clear the count is for the bingo rewards.</param>
/// <param name="BunnyRewards"><see langword="true"/> to record the count for the bunny rewards.</param>
public sealed record MarkDerbyRewardsSeenCommand(int Count, bool ThresholdRewards, bool BunnyRewards) : CodecCommand<MarkDerbyRewardsSeenCommand>, ICommandCodecProvider<MarkDerbyRewardsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkDerbyRewardsSeenCommand> Codec { get; } = Codecs.Command(Codecs.Composite(Codecs.VarInt,
        static command => command.Count,
        Codecs.Boolean,
        static command => command.ThresholdRewards,
        Codecs.Boolean,
        static command => command.BunnyRewards,
        static (count, thresholdRewards, bunnyRewards) => new MarkDerbyRewardsSeenCommand(count, thresholdRewards, bunnyRewards)));
}
