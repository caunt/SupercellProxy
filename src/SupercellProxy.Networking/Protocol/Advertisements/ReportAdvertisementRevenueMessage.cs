using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports the revenue of a rewarded advertisement.</summary>
/// <param name="Network">Name of the ad network that served the video.</param>
/// <param name="Revenue">Earnings the ad SDK reported for this showing.</param>
/// <param name="Currency">Currency code of <paramref name="Revenue"/>.</param>
/// <param name="Placement">Ad SDK placement name of the in-game offer.</param>
/// <param name="AdvertisementId">Ad identifier the ad SDK reported for this showing.</param>
/// <param name="Precision">How exact the ad SDK says the revenue figure is, as its own label.</param>
public sealed record ReportAdvertisementRevenueMessage(string? Network, float Revenue, string? Currency, string? Placement, string? AdvertisementId, string? Precision) : CodecMessage<ReportAdvertisementRevenueMessage>, ICodecProvider<ReportAdvertisementRevenueMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportAdvertisementRevenueMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Network,
        Codecs.Float,
        static message => message.Revenue,
        Codecs.OptionalString,
        static message => message.Currency,
        Codecs.OptionalString,
        static message => message.Placement,
        Codecs.OptionalString,
        static message => message.AdvertisementId,
        Codecs.OptionalString,
        static message => message.Precision,
        static (network, revenue, currency, placement, advertisementId, precision) => new ReportAdvertisementRevenueMessage(network, revenue, currency, placement, advertisementId, precision)
    );
}
