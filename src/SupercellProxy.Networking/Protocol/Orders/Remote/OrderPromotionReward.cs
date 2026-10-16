using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>Promotion box attached to a truck order, as the server sends it for another farm.</summary>
/// <param name="Reward">What the box gives; null when it holds nothing.</param>
/// <param name="Claimed">Whether the box's reward was already collected.</param>
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
