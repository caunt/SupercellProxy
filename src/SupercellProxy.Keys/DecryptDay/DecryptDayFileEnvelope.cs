using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayFileEnvelope response contract.</summary>
internal sealed record DecryptDayFileEnvelope
{
    /// <summary>Gets the Data value.</summary>
    [JsonPropertyName("data")]
    public string? Data { get; init; }

}
