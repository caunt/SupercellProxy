using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Decoded page data of a decrypt.day file lookup.</summary>
internal sealed record DecryptDayFilePayload
{
    /// <summary>The offered files; null when the page data carries none.</summary>
    [JsonPropertyName("data")]
    public DecryptDayFileList? Data { get; init; }
}
