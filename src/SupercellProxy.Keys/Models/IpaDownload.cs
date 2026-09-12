namespace SupercellProxy.Keys.Models;

internal sealed record IpaDownload(string Version, [property: System.Text.Json.Serialization.JsonPropertyName("Url")] Uri Address);
