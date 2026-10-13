using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports a store link-out.</summary>
/// <param name="Placement">The placement.</param>
/// <param name="ProductId">The product id.</param>
/// <param name="Price">The price.</param>
/// <param name="Amount">The amount.</param>
/// <param name="Source">The source.</param>
public sealed record ReportStoreLinkOutMessage(int Placement, int ProductId, int Price, int Amount, int Source) : CodecMessage<ReportStoreLinkOutMessage>, ICodecProvider<ReportStoreLinkOutMessage>
{
    /// <inheritdoc />
    public static ICodec<ReportStoreLinkOutMessage> Codec { get; } = Codecs.Composite(
        Codecs.Int32,
        static message => message.Placement,
        Codecs.Int32,
        static message => message.ProductId,
        Codecs.Int32,
        static message => message.Price,
        Codecs.Int32,
        static message => message.Amount,
        Codecs.VarInt,
        static message => message.Source,
        static (placement, productId, price, amount, source) => new ReportStoreLinkOutMessage(placement, productId, price, amount, source)
    );
}
