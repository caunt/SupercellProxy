namespace SupercellProxy.Keys;

internal sealed record IpaApp(string BundleId, IReadOnlyList<string> Versions);
