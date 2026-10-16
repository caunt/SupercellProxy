using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Offers;

/// <summary>One kind of goods an offer grants.</summary>
/// <param name="GoodGlobalId">Global id of the goods.</param>
/// <param name="Amount">How many are granted.</param>
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
