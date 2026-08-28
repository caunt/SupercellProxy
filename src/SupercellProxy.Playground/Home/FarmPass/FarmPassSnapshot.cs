using System.Text.Json.Serialization;

namespace SupercellProxy.Playground.Home;

/// <summary>
/// Represents decoded <c language="csharp">FarmPassSnapshot</c> home data.
/// </summary>
internal sealed record FarmPassSnapshot
{
    /// <summary>
    /// Gets or sets the <c language="csharp">Perks</c> value.
    /// </summary>
    [JsonPropertyName("FarmPassPerks_v1")]
    public FarmPassPerkSnapshot[] Perks { get; init; } = [];
}
