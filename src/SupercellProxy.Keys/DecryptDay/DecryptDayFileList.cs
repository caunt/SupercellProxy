using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayFileList response contract.</summary>
internal sealed record DecryptDayFileList
{
    /// <summary>Gets the Files value.</summary>
    [JsonPropertyName("files")]
    public DecryptDayFileMetadata?[]? Files { get; init; }

}
