using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayFilePayload response contract.</summary>
internal sealed record DecryptDayFilePayload
{
    /// <summary>Gets the Data value.</summary>
    [JsonPropertyName("data")]
    public DecryptDayFileList? Data { get; init; }

}
