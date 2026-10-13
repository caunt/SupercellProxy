using SupercellProxy.Networking.Protocol.Serialization;

namespace SupercellProxy.Networking.Protocol.Avatars.Collections;

/// <summary>
/// Purchase totals for one key.
/// </summary>
public sealed record PurchaseTotalsEntry(int Key, PurchaseTotals Totals) : ICodecProvider<PurchaseTotalsEntry>
{
    /// <inheritdoc />
    public static ICodec<PurchaseTotalsEntry> Codec { get; } = Codecs.Composite(
        Codecs.VarInt,
        static value => value.Key,
        PurchaseTotals.Codec,
        static value => value.Totals,
        static (key, totals) => new PurchaseTotalsEntry(key, totals)
    );
}
