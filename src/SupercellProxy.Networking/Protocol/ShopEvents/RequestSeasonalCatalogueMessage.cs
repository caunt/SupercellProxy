using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents;

/// <summary>Requests the seasonal catalogue.</summary>
public sealed record RequestSeasonalCatalogueMessage : CodecMessage<RequestSeasonalCatalogueMessage>, ICodecProvider<RequestSeasonalCatalogueMessage>
{
    /// <inheritdoc />
    public static ICodec<RequestSeasonalCatalogueMessage> Codec { get; } = Codecs.Unit(static () => new RequestSeasonalCatalogueMessage());
}
