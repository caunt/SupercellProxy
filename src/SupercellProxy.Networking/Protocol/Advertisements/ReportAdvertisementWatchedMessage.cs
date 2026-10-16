using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;
using SupercellProxy.Networking.Protocol.Versions;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports a watched rewarded advertisement.</summary>
/// <param name="Placement">Native placement number of the in-game offer the video was watched for.</param>
/// <param name="RewardType">Kind of reward the offer grants, as native numbers it.</param>
/// <param name="Completed">Whether the advertisement completed.</param>
/// <param name="Duration">How long the video played.</param>
/// <param name="RewardAmount">How much of the reward the offer grants.</param>
/// <param name="Attempt">Which try this was at loading a video for the offer.</param>
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
