using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>One order reward entry.</summary>
/// <param name="GoodGlobalId">The good data id.</param>
/// <param name="Amount">The amount.</param>
public sealed record OrderReward(int GoodGlobalId, int Amount) : ICodecProvider<OrderReward>
{
    /// <inheritdoc />
    public static ICodec<OrderReward> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.GoodGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (goodGlobalId, amount) => new OrderReward(goodGlobalId, amount)
    );
}
