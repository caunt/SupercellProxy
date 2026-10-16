using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>One reward listed for town service help.</summary>
/// <param name="GoodGlobalId">Global id of the rewarded resource.</param>
/// <param name="Amount">How much of the resource is rewarded.</param>
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
