using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// One shop purchase: time, price, whether it was external, and the goods bought.
/// </summary>
/// <param name="Timestamp">The time of the purchase, written as an Int64; receipts in <see cref="PurchaseHistory.Receipts"/> are ordered by it.</param>
/// <param name="Price">The price paid, written as a VarInt; its unit (cents or diamonds) is not confirmed by the code.</param>
/// <param name="External">A VarInt flag that is non-zero when the purchase was made outside the in-game shop; the exact external sources it covers are not yet identified.</param>
/// <param name="Goods">The goods bought, as pairs of an Int32 global id and a VarInt amount; empty when the receipt lists no goods.</param>
public sealed record PurchaseReceipt(long Timestamp, int Price, int External, KeyValuePair<int, int>[] Goods) : ICodecProvider<PurchaseReceipt>
{
    /// <inheritdoc />
    public static ICodec<PurchaseReceipt> Codec { get; } = Codecs.Composite(Codecs.Int64,
        static receipt => receipt.Timestamp,
        Codecs.VarInt,
        static receipt => receipt.Price,
        Codecs.VarInt,
        static receipt => receipt.External,
        Codecs.Array(Codecs.Composite(Codecs.Int32,
                static good => good.Key,
                Codecs.VarInt,
                static good => good.Value,
                static (key, value) => new KeyValuePair<int, int>(key, value)),
            CollectionCount.Bounded),
        static receipt => receipt.Goods,
        static (timestamp, price, external, goods) => new PurchaseReceipt(timestamp, price, external, goods));
}
