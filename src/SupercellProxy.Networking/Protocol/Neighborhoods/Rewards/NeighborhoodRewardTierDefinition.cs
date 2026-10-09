using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods;

/// <summary>A resource reward at a neighborhood point threshold.</summary>
public sealed record NeighborhoodRewardTierDefinition
{
    /// <summary>Gets the configured reward quantity.</summary>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }
    /// <summary>Gets the inclusive maximum experience level, or zero for no upper limit.</summary>
    [JsonPropertyName("maxXpLevel")]
    public int MaximumLevel { get; init; }
    /// <summary>Gets the inclusive minimum experience level.</summary>
    [JsonPropertyName("minXpLevel")]
    public int MinimumLevel { get; init; }
    /// <summary>Gets the neighborhood points required.</summary>
    [JsonPropertyName("points")]
    public int Points { get; init; }
    /// <summary>Gets the native reward resource name.</summary>
    [JsonPropertyName("reward")]
    public string Reward { get; init; } = string.Empty;
}
