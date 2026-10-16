using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Claims the reward of a watched advertisement.</summary>
/// <param name="Completed">Whether the video was watched to the end, which the reward requires.</param>
/// <param name="Placement">Native placement number of the in-game offer the video was watched for.</param>
public sealed record ClaimAdvertisementRewardMessage(bool Completed, int Placement) : CodecMessage<ClaimAdvertisementRewardMessage>, ICodecProvider<ClaimAdvertisementRewardMessage>
{
    /// <inheritdoc />
    public static ICodec<ClaimAdvertisementRewardMessage> Codec { get; } = Codecs.Composite(
        Codecs.Boolean,
        static message => message.Completed,
        Codecs.VarInt,
        static message => message.Placement,
        static (completed, placement) => new ClaimAdvertisementRewardMessage(completed, placement)
    );
}
