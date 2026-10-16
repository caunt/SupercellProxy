using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>One kind of goods a store package grants.</summary>
/// <param name="GoodGlobalId">Global id of the goods.</param>
/// <param name="Amount">How many are granted; negative amounts take goods away.</param>
public sealed record ShopPurchaseGood(int GoodGlobalId, int Amount) : ICodecProvider<ShopPurchaseGood>
{
    /// <inheritdoc />
    public static ICodec<ShopPurchaseGood> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new ShopPurchaseGood(goodGlobalId, amount)
    );
}
