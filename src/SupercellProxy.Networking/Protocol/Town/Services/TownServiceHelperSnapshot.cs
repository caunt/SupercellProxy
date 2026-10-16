using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Town.Services;

/// <summary>The saved home id of the player who helped a town service.</summary>
public sealed record TownServiceHelperSnapshot
{
    /// <summary>Gets the high identifier word.</summary>
    [JsonPropertyName("hi")]
    public int High { get; init; }

    /// <summary>Gets the low identifier word.</summary>
    [JsonPropertyName("lo")]
    public int Low { get; init; }
}
