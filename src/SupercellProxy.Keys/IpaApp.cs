namespace SupercellProxy.Keys;

internal sealed record IpaApp(string BundleId, IReadOnlyList<AppVersion> Versions);
