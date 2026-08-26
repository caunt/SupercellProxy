namespace SupercellProxy.Keys;

internal sealed record DecryptDayAppDetail(
    string Id,
    string BundleId,
    IReadOnlyList<string> Versions
);
