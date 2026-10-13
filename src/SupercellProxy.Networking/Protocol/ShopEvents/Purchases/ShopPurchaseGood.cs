using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.ShopEvents.Purchases;

/// <summary>One shop purchase good entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
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
