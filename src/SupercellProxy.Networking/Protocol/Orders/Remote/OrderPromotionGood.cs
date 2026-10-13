using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>The order promotion good.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record OrderPromotionGood(int GoodGlobalId, int Amount) : ICodecProvider<OrderPromotionGood>
{
    /// <inheritdoc />
    public static ICodec<OrderPromotionGood> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new OrderPromotionGood(goodGlobalId, amount)
    );
}
