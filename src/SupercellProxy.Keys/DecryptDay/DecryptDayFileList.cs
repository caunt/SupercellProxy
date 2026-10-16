using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>The downloadable files decrypt.day offers for one app release.</summary>
internal sealed record DecryptDayFileList
{
    /// <summary>Each offered file with its access flags; null when the reply has none.</summary>
    [JsonPropertyName("files")]
    public DecryptDayFileMetadata?[]? Files { get; init; }
}
