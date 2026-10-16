using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Homes;

/// <summary>
/// Serverbound request to visit another player's fishing area. The reply also carries the owner's full avatar, which makes it the
/// cheapest way to read a farm's roadside shop.
/// </summary>
public sealed record VisitOtherFishingHomeMessage : CodecMessage<VisitOtherFishingHomeMessage>, ICodecProvider<VisitOtherFishingHomeMessage>
{
    /// <inheritdoc />
    public static ICodec<VisitOtherFishingHomeMessage> Codec { get; } = Codecs.Composite(Codecs.LongId, static message => message.Target, static target => new VisitOtherFishingHomeMessage { Target = target });

    /// <summary>
    /// Home id of the farm whose fishing area to visit.
    /// </summary>
    public required LongId Target { get; init; }
}
