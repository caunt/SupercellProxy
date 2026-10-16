using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>A resource and a count in a Valley shop purchase, used both for what it costs and for what it gives.</summary>
/// <param name="GoodGlobalId">Global id of the resource.</param>
/// <param name="Amount">How much of the resource.</param>
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
