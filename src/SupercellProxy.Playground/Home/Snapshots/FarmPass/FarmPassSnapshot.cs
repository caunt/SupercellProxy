using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c>FarmPassSnapshot</c> home data.
/// </summary>
public sealed record FarmPassSnapshot
{
    /// <summary>
    /// Gets or sets the <c>Perks</c> value.
    /// </summary>
    [JsonPropertyName("FarmPassPerks_v1")]
    public FarmPassPerkSnapshot[] Perks { get; init; } = [];
}
