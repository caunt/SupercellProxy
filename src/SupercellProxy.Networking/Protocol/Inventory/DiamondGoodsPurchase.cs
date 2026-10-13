using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Inventory;

/// <summary>One good bought with diamonds.</summary>
/// <param name="ItemGlobalId">The bought item data id.</param>
/// <param name="Amount">The bought quantity.</param>
public sealed record DiamondGoodsPurchase(int ItemGlobalId, int Amount) : ICodecProvider<DiamondGoodsPurchase>
{
    /// <inheritdoc />
    public static ICodec<DiamondGoodsPurchase> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.ItemGlobalId,
        Codecs.VarInt,
        static value => value.Amount,
        static (itemGlobalId, amount) => new DiamondGoodsPurchase(itemGlobalId, amount)
    );
}
