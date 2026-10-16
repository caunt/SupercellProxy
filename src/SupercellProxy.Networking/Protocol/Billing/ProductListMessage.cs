using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Lists the store products the server offers.</summary>
/// <param name="ProductIds">Store product identifiers the client should look up prices for; null when the list was sent as absent.</param>
public sealed record ProductListMessage(ReadOnlyMemory<string?>? ProductIds) : CodecMessage<ProductListMessage>, ICodecProvider<ProductListMessage>
{
    /// <inheritdoc />
    public static ICodec<ProductListMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(Codecs.OptionalString),
        static message => message.ProductIds,
        static productIds => new ProductListMessage(productIds)
    );
}
