using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports that a rewarded advertisement started.</summary>
/// <param name="Network">The network.</param>
/// <param name="Placement">The placement.</param>
/// <param name="AdvertisementId">The advertisement id.</param>
public sealed record ReportAdvertisementStartedMessage(string? Network, string? Placement, string? AdvertisementId) : CodecMessage<ReportAdvertisementStartedMessage>, ICodecProvider<ReportAdvertisementStartedMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportAdvertisementStartedMessage> Codec { get; } = Codecs.Composite(
        Codecs.OptionalString,
        static message => message.Network,
        Codecs.OptionalString,
        static message => message.Placement,
        Codecs.OptionalString,
        static message => message.AdvertisementId,
        static (network, placement, advertisementId) => new ReportAdvertisementStartedMessage(network, placement, advertisementId)
    );
}
