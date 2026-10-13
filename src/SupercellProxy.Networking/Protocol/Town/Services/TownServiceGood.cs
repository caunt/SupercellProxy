using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>One town service good entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record TownServiceGood(int GoodGlobalId, int Amount) : ICodecProvider<TownServiceGood>
{
    /// <inheritdoc />
    public static ICodec<TownServiceGood> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new TownServiceGood(goodGlobalId, amount)
    );
}
