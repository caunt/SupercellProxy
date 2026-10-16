using SupercellProxy.Networking.Protocol.MessageEncoding;
using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Telemetry;

/// <summary>Reports that the player followed a link out to the app store for a product.</summary>
/// <param name="Placement">Where in the game the offer was shown; its values are not identified.</param>
/// <param name="ProductId">Offered product; its id space is not identified.</param>
/// <param name="Price">Price shown for the product.</param>
/// <param name="Amount">How much of the product the offer gives.</param>
/// <param name="Source">What led to the offer; its values are not identified.</param>
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
