using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby.Commands.Rewards;

/// <summary>Records the seen threshold count for one derby reward family.</summary>
public sealed record MarkDerbyRewardsSeenCommand(int Count, bool ThresholdRewards, bool BunnyRewards) : CodecCommand<MarkDerbyRewardsSeenCommand>, ICommandCodecProvider<MarkDerbyRewardsSeenCommand>
{
    /// <inheritdoc />
    public static CommandCodec<MarkDerbyRewardsSeenCommand> Codec { get; } = Codecs.Command(
        type: 178,
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Count,
            Codecs.Boolean,
            static command => command.ThresholdRewards,
            Codecs.Boolean,
            static command => command.BunnyRewards,
            static (count, thresholdRewards, bunnyRewards) => new MarkDerbyRewardsSeenCommand(count, thresholdRewards, bunnyRewards)
        )
    );
}
