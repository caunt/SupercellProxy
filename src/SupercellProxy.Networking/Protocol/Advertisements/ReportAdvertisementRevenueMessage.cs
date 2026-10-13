using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports the revenue of a rewarded advertisement.</summary>
/// <param name="Network">The network.</param>
/// <param name="Revenue">The revenue.</param>
/// <param name="Currency">The currency.</param>
/// <param name="Placement">The placement.</param>
/// <param name="AdvertisementId">The advertisement id.</param>
/// <param name="Precision">The precision.</param>
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
