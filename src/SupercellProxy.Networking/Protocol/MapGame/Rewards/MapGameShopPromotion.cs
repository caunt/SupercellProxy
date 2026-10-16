using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Promotion box offered with a Valley shop purchase.</summary>
/// <param name="Reward">What the box gives; null when it holds nothing.</param>
/// <param name="Claimed">Whether the box's reward was already collected.</param>
public sealed record MapGameShopPromotion(MapGameShopGood? Reward, bool Claimed) : ICodecProvider<MapGameShopPromotion>
{
    /// <inheritdoc />
    public static ICodec<MapGameShopPromotion> Codec { get; } = Codecs.Composite(
        Codecs.Optional(MapGameShopGood.Codec),
        static value => value.Reward,
        Codecs.Boolean,
        static value => value.Claimed,
        static (reward, claimed) => new MapGameShopPromotion(reward, claimed)
    );
}
