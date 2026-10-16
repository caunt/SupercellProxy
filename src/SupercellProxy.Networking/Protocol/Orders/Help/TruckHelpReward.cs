using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>One reward listed for helping a truck order.</summary>
/// <param name="GoodGlobalId">Global id of the rewarded resource.</param>
/// <param name="Amount">How much of the resource is rewarded.</param>
public sealed record TruckHelpReward(int GoodGlobalId, int Amount) : ICodecProvider<TruckHelpReward>
{
    /// <inheritdoc />
    public static ICodec<TruckHelpReward> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new TruckHelpReward(goodGlobalId, amount)
    );
}
