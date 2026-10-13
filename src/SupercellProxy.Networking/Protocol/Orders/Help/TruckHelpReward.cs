using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Help;

/// <summary>One truck help reward entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
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
