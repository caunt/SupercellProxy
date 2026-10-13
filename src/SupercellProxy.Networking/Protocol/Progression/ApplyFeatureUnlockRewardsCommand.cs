using SupercellProxy.Networking.Protocol.Commands;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Progression;

/// <summary>Applies the rewards of newly unlocked features.</summary>
/// <param name="Level">The level.</param>
/// <param name="FeatureIndex">The feature index.</param>
public sealed record ApplyFeatureUnlockRewardsCommand(int Level, int FeatureIndex) : CodecCommand<ApplyFeatureUnlockRewardsCommand>, ICommandCodecProvider<ApplyFeatureUnlockRewardsCommand>
{
    /// <inheritdoc />
    public static CommandCodec<ApplyFeatureUnlockRewardsCommand> Codec { get; } = Codecs.Command(
        type: 655,
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
