using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Forestry;

/// <summary>One kind of goods a helper earns for reviving another farm's tree.</summary>
/// <param name="GoodGlobalId">Global id of the goods.</param>
/// <param name="Amount">How many the helper gets.</param>
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
