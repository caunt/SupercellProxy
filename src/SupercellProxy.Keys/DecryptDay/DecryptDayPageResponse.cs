using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Represents the typed DecryptDayPageResponse response contract.</summary>
internal sealed record DecryptDayPageResponse
{
    /// <summary>Gets the Nodes value.</summary>
    [JsonPropertyName("nodes")]
    public DecryptDayPageNode?[] Nodes { get; init; } = [];

}
