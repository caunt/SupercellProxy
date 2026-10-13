using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports a watched rewarded advertisement.</summary>
/// <param name="Placement">The placement.</param>
/// <param name="RewardType">The reward type.</param>
/// <param name="Completed">Whether the advertisement completed.</param>
/// <param name="Duration">The duration.</param>
/// <param name="RewardAmount">The reward amount.</param>
/// <param name="Attempt">The attempt.</param>
/// <param name="Skipped">Whether the advertisement was skipped.</param>
public sealed record ReportAdvertisementWatchedMessage(int Placement, int RewardType, bool Completed, int Duration, int RewardAmount, int Attempt, bool Skipped)
    : CodecMessage<ReportAdvertisementWatchedMessage>, ICodecProvider<ReportAdvertisementWatchedMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportAdvertisementWatchedMessage> Codec { get; } = Codecs.Versioned(
        Release17381.Version,
        Codecs.Composite(
            Codecs.VarInt,
            static message => message.Placement,
            Codecs.VarInt,
            static message => message.RewardType,
            Codecs.Boolean,
            static message => message.Completed,
            Codecs.VarInt,
            static message => message.Duration,
            Codecs.VarInt,
            static message => message.RewardAmount,
            Codecs.VarInt,
            static message => message.Attempt,
            Codecs.Boolean,
            static message => message.Skipped,
            static (placement, rewardType, completed, duration, rewardAmount, attempt, skipped) => new ReportAdvertisementWatchedMessage(placement, rewardType, completed, duration, rewardAmount, attempt, skipped)
        ),
        Codecs.Composite(
            Codecs.Boolean,
            static message => message.Skipped,
            Codecs.Boolean,
            static message => message.Completed,
            Codecs.VarInt,
            static message => message.Attempt,
            Codecs.VarInt,
            static message => message.Duration,
            Codecs.VarInt,
            static message => message.RewardAmount,
            Codecs.VarInt,
            static message => message.Placement,
            Codecs.VarInt,
            static message => message.RewardType,
            static (skipped, completed, attempt, duration, rewardAmount, placement, rewardType) => new ReportAdvertisementWatchedMessage(placement, rewardType, completed, duration, rewardAmount, attempt, skipped)
        )
    );
}
