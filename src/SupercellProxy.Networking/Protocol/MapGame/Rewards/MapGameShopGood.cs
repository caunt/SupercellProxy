using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>One map game shop good entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record MapGameShopGood(int GoodGlobalId, int Amount) : ICodecProvider<MapGameShopGood>
{
    /// <inheritdoc />
    public static ICodec<MapGameShopGood> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new MapGameShopGood(goodGlobalId, amount)
    );
}
