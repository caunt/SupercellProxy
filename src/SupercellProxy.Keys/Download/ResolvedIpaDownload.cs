using SupercellProxy.Keys.Models;

namespace SupercellProxy.Keys.Download;

/// <summary>Named result returned by ResolveDownloadAsync.</summary>
internal readonly record struct ResolvedIpaDownload(IpaApp App, IpaDownload Download);
