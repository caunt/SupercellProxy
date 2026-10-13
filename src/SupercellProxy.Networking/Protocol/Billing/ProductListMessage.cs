using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Billing;

/// <summary>Lists the store products the server offers.</summary>
/// <param name="ProductIds">The product ids.</param>
public sealed record ProductListMessage(ReadOnlyMemory<string?>? ProductIds) : CodecMessage<ProductListMessage>, ICodecProvider<ProductListMessage>
{
    /// <inheritdoc />
    public static ICodec<ProductListMessage> Codec { get; } = Codecs.Composite(
        Codecs.NullableMemory(Codecs.OptionalString),
        static message => message.ProductIds,
        static (productIds) => new ProductListMessage(productIds)
    );
}
