using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Progression;

/// <summary>Applies the rewards of newly unlocked features.</summary>
/// <param name="Level">Experience level that unlocked the feature.</param>
/// <param name="FeatureIndex">Which of that level's unlocked features gives the rewards.</param>
public sealed record ApplyFeatureUnlockRewardsCommand(int Level, int FeatureIndex) : CodecCommand<ApplyFeatureUnlockRewardsCommand>, ICommandCodecProvider<ApplyFeatureUnlockRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ApplyFeatureUnlockRewardsCommand> Codec { get; } = Codecs.Command(
        Codecs.Composite(
            Codecs.VarInt,
            static command => command.Level,
            Codecs.VarInt,
            static command => command.FeatureIndex,
            static (level, featureIndex) => new ApplyFeatureUnlockRewardsCommand(level, featureIndex)
        ),
        CommandLayout.MetadataFirst
    );
}
