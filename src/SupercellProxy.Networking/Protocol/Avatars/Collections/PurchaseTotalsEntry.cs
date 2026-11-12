using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Purchase totals for one key.
/// </summary>
/// <param name="Key">The VarInt key the totals are grouped by: the bought goods' data table id in <see cref="PurchaseHistory.TableTotals"/>, or the bought money resource's global id in <see cref="PurchaseHistory.MoneyTotals"/>.</param>
/// <param name="Totals">The transaction count, spending and last purchase time accumulated over the purchases that fall under <paramref name="Key"/>.</param>
public sealed record PurchaseTotalsEntry(int Key, PurchaseTotals Totals) : ICodecProvider<PurchaseTotalsEntry>
{
    /// <inheritdoc />
    public static ICodec<PurchaseTotalsEntry> Codec { get; } = Codecs.Composite(Codecs.VarInt,
        static value => value.Key,
        PurchaseTotals.Codec,
        static value => value.Totals,
        static (key, totals) => new PurchaseTotalsEntry(key, totals));
}
