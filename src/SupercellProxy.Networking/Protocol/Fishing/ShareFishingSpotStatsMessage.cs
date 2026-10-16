using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Shares fishing statistics with a player.</summary>
/// <param name="AvatarId">Player to share the statistics with.</param>
/// <param name="Enabled">True to share; false to stop sharing.</param>
public sealed record ShareFishingSpotStatsMessage(LongId AvatarId, bool Enabled) : CodecMessage<ShareFishingSpotStatsMessage>, ICodecProvider<ShareFishingSpotStatsMessage>
{
    /// <inheritdoc />
    public static ICodec<ShareFishingSpotStatsMessage> Codec { get; } = Codecs.Composite(
        Codecs.LongId,
        static message => message.AvatarId,
        Codecs.Boolean,
        static message => message.Enabled,
        static (avatarId, enabled) => new ShareFishingSpotStatsMessage(avatarId, enabled)
    );
}
