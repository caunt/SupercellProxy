using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// The avatar's shop purchase receipts and spending totals.
/// </summary>
public sealed record PurchaseHistory : ICodecProvider<PurchaseHistory>
{
    /// <inheritdoc />
    /// <remarks>Native stops after a non-positive version.</remarks>
    public static ICodec<PurchaseHistory> Codec { get; } = Codecs.Create(
        static stream =>
        {
            int version = stream.ReadVarInt();

            return version <= 0 ? new PurchaseHistory { Version = version } : Body.Read(stream) with { Version = version };
        },
        static (stream, history) =>
        {
            stream.WriteVarInt(history.Version);

            if (history.Version > 0)
                Body.Write(stream, history);
        }
    );

    /// <summary>
    /// Gets the purchase receipts ordered by time.
    /// </summary>
    public PurchaseReceipt[] Receipts { get; init; } = [];

    /// <summary>
    /// Gets the totals over all purchases.
    /// </summary>
    public PurchaseTotals Totals { get; init; } = new(TransactionCount: 0, SpentCents: 0, SupercellStoreSpentCents: 0, LastPurchaseTime: 0);

    /// <summary>
    /// Gets the totals keyed by the bought goods' data table id.
    /// </summary>
    public PurchaseTotalsEntry[] TableTotals { get; init; } = [];

    /// <summary>
    /// Gets the totals keyed by the bought money resource's global id.
    /// </summary>
    public PurchaseTotalsEntry[] MoneyTotals { get; init; } = [];

    /// <summary>
    /// Gets or sets the <c language="csharp">Version</c> value.
    /// </summary>
    public int Version { get; init; }
    private static ICodec<PurchaseHistory> Body => field ??= Codecs.Composite(
        Codecs.Array(PurchaseReceipt.Codec, CollectionCount.Bounded),
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
        }
    );
}
