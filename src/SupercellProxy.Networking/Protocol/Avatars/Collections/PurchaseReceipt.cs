using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// One shop purchase: time, price, whether it was external, and the goods bought.
/// </summary>
public sealed record PurchaseReceipt(long Timestamp, int Price, int External, KeyValuePair<int, int>[] Goods) : ICodecProvider<PurchaseReceipt>
{
    /// <inheritdoc />
    public static ICodec<PurchaseReceipt> Codec { get; } = Codecs.Composite(
        Codecs.Int64,
        static receipt => receipt.Timestamp,
        Codecs.VarInt,
        static receipt => receipt.Price,
        Codecs.VarInt,
        static receipt => receipt.External,
        Codecs.Array(
            Codecs.Composite(
                Codecs.Int32,
                static good => good.Key,
                Codecs.VarInt,
                static good => good.Value,
                static (key, value) => new KeyValuePair<int, int>(key, value)
            ),
            CollectionCount.Bounded
        ),
        static receipt => receipt.Goods,
        static (timestamp, price, external, goods) => new PurchaseReceipt(timestamp, price, external, goods)
    );
}
