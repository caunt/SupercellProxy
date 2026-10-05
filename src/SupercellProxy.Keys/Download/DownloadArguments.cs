namespace SupercellProxy.Keys.Download;

/// <summary>Named result returned by ParseDownloadArguments.</summary>
internal readonly record struct DownloadArguments(List<string> PositionalArguments, string? OutputOption);
