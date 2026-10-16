using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Orders.Remote;

/// <summary>A resource and a count, used for goods delivered to a helped order and for the helper's rewards.</summary>
/// <param name="GoodGlobalId">Global id of the resource.</param>
/// <param name="Amount">How much of the resource; entries below 1 change nothing.</param>
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
