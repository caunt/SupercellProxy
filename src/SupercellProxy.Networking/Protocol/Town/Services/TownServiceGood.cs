using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>A resource and a count, used for what a town service needs and what a passenger rewards.</summary>
/// <param name="GoodGlobalId">Global id of the resource.</param>
/// <param name="Amount">How much of the resource is needed or rewarded.</param>
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
