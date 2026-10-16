using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Outer reply of a decrypt.day file lookup, which wraps the file list as encoded page data.</summary>
internal sealed record DecryptDayFileEnvelope
{
    /// <summary>The file list as encoded page data text, decoded in a second step.</summary>
    [JsonPropertyName("data")]
    public string? Data { get; init; }
}
