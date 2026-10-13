using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>One help reward entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record HelpReward(int GoodGlobalId, int Amount) : ICodecProvider<HelpReward>
{
    /// <inheritdoc />
    public static ICodec<HelpReward> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new HelpReward(goodGlobalId, amount)
    );
}
