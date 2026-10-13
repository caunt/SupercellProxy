using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Purchase totals saved natively as TransactionCount, USDCentsSpent, SCStoreUSDCentsSpent and LastPurchaseTimeSeconds.
/// </summary>
public sealed record PurchaseTotals(int TransactionCount, int SpentCents, int SupercellStoreSpentCents, long LastPurchaseTime) : ICodecProvider<PurchaseTotals>
{
    /// <inheritdoc />
    public static ICodec<PurchaseTotals> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.TransactionCount,
        Codecs.VarInt,
        static value => value.SpentCents,
        Codecs.VarInt,
        static value => value.SupercellStoreSpentCents,
        Codecs.Int64,
        static value => value.LastPurchaseTime,
        static (transactionCount, spentCents, supercellStoreSpentCents, lastPurchaseTime) => new PurchaseTotals(transactionCount, spentCents, supercellStoreSpentCents, lastPurchaseTime)
    );
}
