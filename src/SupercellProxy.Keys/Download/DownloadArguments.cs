namespace SupercellProxy.Keys.Download;

/// <summary>The command-line options of this command, as <see cref="Application"/> reads them from its arguments.</summary>
internal readonly record struct DownloadArguments(List<string> PositionalArguments, string? OutputOption);
