using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>One revival reward entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record RevivalReward(int GoodGlobalId, int Amount) : ICodecProvider<RevivalReward>
{
    /// <inheritdoc />
    public static ICodec<RevivalReward> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new RevivalReward(goodGlobalId, amount)
    );
}
