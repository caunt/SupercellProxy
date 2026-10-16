using System.Text.Json.Serialization;
namespace SupercellProxy.Keys.Models;

internal sealed record IpaDownload(string Version, [property: JsonPropertyName("Url")] Uri Address);
