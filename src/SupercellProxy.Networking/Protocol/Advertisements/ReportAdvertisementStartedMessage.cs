using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports that a rewarded advertisement started.</summary>
/// <param name="Network">Name of the ad network serving the video.</param>
/// <param name="Placement">Ad SDK placement name of the in-game offer.</param>
/// <param name="AdvertisementId">Ad identifier the ad SDK reported for this showing.</param>
public sealed record ReportAdvertisementStartedMessage(string? Network, string? Placement, string? AdvertisementId) : CodecMessage<ReportAdvertisementStartedMessage>, ICodecProvider<ReportAdvertisementStartedMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportAdvertisementStartedMessage> Codec { get; } = Codecs.Composite(Codecs.OptionalUtf8String,
        static message => message.Network,
        Codecs.OptionalUtf8String,
        static message => message.Placement,
        Codecs.OptionalUtf8String,
        static message => message.AdvertisementId,
        static (network, placement, advertisementId) => new ReportAdvertisementStartedMessage(network, placement, advertisementId));
}
