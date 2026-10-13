using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>One offer good entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record OfferGood(int GoodGlobalId, int Amount) : ICodecProvider<OfferGood>
{
    /// <inheritdoc />
    public static ICodec<OfferGood> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new OfferGood(goodGlobalId, amount)
    );
}
