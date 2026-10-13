using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Fishing;

/// <summary>Shares fishing statistics with a player.</summary>
/// <param name="AvatarId">The avatar id.</param>
/// <param name="Enabled">The enabled.</param>
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
