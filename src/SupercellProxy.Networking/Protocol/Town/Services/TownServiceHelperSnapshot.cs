using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>The saved home id of the player who helped a town service.</summary>
public sealed record TownServiceHelperSnapshot
{
    /// <summary>Gets or sets the high identifier word.</summary>
    /// <value>Upper 32 bits of the helper's home id, saved as <c>hi</c>; 0 by default.</value>
    [JsonPropertyName("hi")]
    public int High { get; init; }

    /// <summary>Gets or sets the low identifier word.</summary>
    /// <value>Lower 32 bits of the helper's home id, saved as <c>lo</c>; 0 by default.</value>
    [JsonPropertyName("lo")]
    public int Low { get; init; }
}
