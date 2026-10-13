using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>The order promotion reward.</summary>
/// <param name="Reward">The reward.</param>
/// <param name="Claimed">The claimed.</param>
public sealed record OrderPromotionReward(OrderPromotionGood? Reward, bool Claimed) : ICodecProvider<OrderPromotionReward>
{
    /// <inheritdoc />
    public static ICodec<OrderPromotionReward> Codec { get; } = Codecs.Composite(
        Codecs.Optional(OrderPromotionGood.Codec),
        static value => value.Reward,
        Codecs.Boolean,
        static value => value.Claimed,
        static (reward, claimed) => new OrderPromotionReward(reward, claimed)
    );
}
