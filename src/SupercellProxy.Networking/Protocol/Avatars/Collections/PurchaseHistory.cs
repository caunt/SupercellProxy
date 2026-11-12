using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// The avatar's shop purchase receipts and spending totals.
/// </summary>
public sealed record PurchaseHistory : ICodecProvider<PurchaseHistory>
{
    /// <inheritdoc />
    /// <remarks>Native stops after a non-positive version.</remarks>
    public static ICodec<PurchaseHistory> Codec { get; } = Codecs.Create(static stream =>
    {
        int version = stream.ReadVarInt();

        return version <= 0 ? new PurchaseHistory { Version = version } : Body.Read(stream) with { Version = version };
    },
        static (stream, history) =>
        {
            stream.WriteVarInt(history.Version);

            if (history.Version > 0)
                Body.Write(stream, history);
        });

    /// <summary>
    /// Gets or sets the purchase receipts ordered by time.
    /// </summary>
    /// <value>One receipt per shop purchase, oldest first; empty by default and always empty when <see cref="Version"/> is 0 or less.</value>
    public PurchaseReceipt[] Receipts { get; init; } = [];

    /// <summary>
    /// Gets or sets the totals over all purchases.
    /// </summary>
    /// <value>The transaction count, cents spent and last purchase time summed over every receipt; all zero by default and when <see cref="Version"/> is 0 or less.</value>
    public PurchaseTotals Totals { get; init; } = new(TransactionCount: 0, SpentCents: 0, SupercellStoreSpentCents: 0, LastPurchaseTime: 0);

    /// <summary>
    /// Gets or sets the totals keyed by the bought goods' data table id.
    /// </summary>
    /// <value>One entry per data table that purchased goods came from, each with its own count and spending; empty by default.</value>
    public PurchaseTotalsEntry[] TableTotals { get; init; } = [];

    /// <summary>
    /// Gets or sets the totals keyed by the bought money resource's global id.
    /// </summary>
    /// <value>One entry per money resource (such as diamonds or coins) that was bought, each with its own count and spending; empty by default.</value>
    public PurchaseTotalsEntry[] MoneyTotals { get; init; } = [];

    /// <summary>
    /// Gets or sets the layout revision written first; 0 or less means the history is sent empty, with no receipts or totals following.
    /// </summary>
    /// <value>The VarInt revision read before the body; 0 by default, in which case only this number is written and the other properties keep their empty defaults.</value>
    public int Version { get; init; }
    private static ICodec<PurchaseHistory> Body => field ??= Codecs.Composite(Codecs.Array(PurchaseReceipt.Codec, CollectionCount.Bounded),
        static history => history.Receipts,
        PurchaseTotals.Codec,
        static history => history.Totals,
        Codecs.Array(PurchaseTotalsEntry.Codec, CollectionCount.Bounded),
        static history => history.TableTotals,
        Codecs.Array(PurchaseTotalsEntry.Codec, CollectionCount.Bounded),
        static history => history.MoneyTotals,
        static (receipts, totals, tableTotals, moneyTotals) => new PurchaseHistory
        {
            Receipts = receipts,
            Totals = totals,
            TableTotals = tableTotals,
            MoneyTotals = moneyTotals,
        });
}
