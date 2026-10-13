using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>The map game shop promotion.</summary>
/// <param name="Reward">The reward.</param>
/// <param name="Claimed">The claimed.</param>
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
