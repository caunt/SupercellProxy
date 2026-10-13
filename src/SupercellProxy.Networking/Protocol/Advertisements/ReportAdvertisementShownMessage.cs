using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Advertisements;

/// <summary>Reports that an advertisement video was shown.</summary>
/// <param name="Placement">The placement.</param>
public sealed record ReportAdvertisementShownMessage(int Placement) : CodecMessage<ReportAdvertisementShownMessage>, ICodecProvider<ReportAdvertisementShownMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportAdvertisementShownMessage> Codec { get; } = Codecs.Composite(Codecs.VarInt, static message => message.Placement, static (placement) => new ReportAdvertisementShownMessage(placement));
}
