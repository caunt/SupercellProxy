using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Purchase totals saved natively as TransactionCount, USDCentsSpent, SCStoreUSDCentsSpent and LastPurchaseTimeSeconds.
/// </summary>
/// <param name="TransactionCount">The number of purchases counted, saved natively as <c>TransactionCount</c>; 0 for an account that never bought anything.</param>
/// <param name="SpentCents">The money spent over those purchases in US cents, saved natively as <c>USDCentsSpent</c>.</param>
/// <param name="SupercellStoreSpentCents">The part of the spending made through the Supercell Store, in US cents, saved natively as <c>SCStoreUSDCentsSpent</c>.</param>
/// <param name="LastPurchaseTime">The time of the latest counted purchase in seconds, saved natively as <c>LastPurchaseTimeSeconds</c> and written as an Int64; 0 when there is none.</param>
public sealed record PurchaseTotals(int TransactionCount, int SpentCents, int SupercellStoreSpentCents, long LastPurchaseTime) : ICodecProvider<PurchaseTotals>
{
    /// <inheritdoc />
    public static ICodec<PurchaseTotals> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static value => value.TransactionCount,
        Codecs.VarInt,
        static value => value.SpentCents,
        Codecs.VarInt,
        static value => value.SupercellStoreSpentCents,
        Codecs.Int64,
        static value => value.LastPurchaseTime,
        static (transactionCount, spentCents, supercellStoreSpentCents, lastPurchaseTime) => new PurchaseTotals(transactionCount, spentCents, supercellStoreSpentCents, lastPurchaseTime));
}
