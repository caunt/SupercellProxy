using System.Text.Json.Serialization;

namespace SupercellProxy.Keys.DecryptDay;

/// <summary>Data of a decrypt.day app page, split into the parts its web framework renders.</summary>
internal sealed record DecryptDayPageResponse
{
    /// <summary>The page parts; the first one that describes an app is used.</summary>
    [JsonPropertyName("nodes")]
    public DecryptDayPageNode?[] Nodes { get; init; } = [];
}
