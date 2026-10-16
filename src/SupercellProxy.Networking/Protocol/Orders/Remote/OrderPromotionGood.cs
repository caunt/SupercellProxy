using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>What an order's promotion box gives.</summary>
/// <param name="GoodGlobalId">Global id of the rewarded resource.</param>
/// <param name="Amount">How much of the resource the box gives.</param>
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
