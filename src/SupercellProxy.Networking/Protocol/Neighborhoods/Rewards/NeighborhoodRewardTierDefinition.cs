using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Neighborhoods.Rewards;

/// <summary>A resource reward at a neighborhood point threshold.</summary>
public sealed record NeighborhoodRewardTierDefinition
{
    /// <summary>Gets or sets the configured reward quantity.</summary>
    /// <value>Saved as <c>rewardAmount</c>; how many of <see cref="Reward"/> the entry grants.</value>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the inclusive maximum experience level, or zero for no upper limit.</summary>
    /// <value>Saved as <c>maxXpLevel</c>; the highest level of the entry's level range.</value>
    [JsonPropertyName("maxXpLevel")]
    public int MaximumLevel { get; init; }

    /// <summary>Gets or sets the inclusive minimum experience level.</summary>
    /// <value>Saved as <c>minXpLevel</c>; the lowest level of the entry's level range.</value>
    [JsonPropertyName("minXpLevel")]
    public int MinimumLevel { get; init; }

    /// <summary>Gets or sets the neighborhood points required.</summary>
    /// <value>The neighbourhood-wide point threshold of the entry, saved as <c>points</c>; entries with the same value form one tier.</value>
    [JsonPropertyName("points")]
    public int Points { get; init; }

    /// <summary>Gets or sets the native reward resource name.</summary>
    /// <value>Saved as <c>reward</c>; the table row name of the granted item, such as a resource, goods or decoration. Empty by default.</value>
    [JsonPropertyName("reward")]
    public string Reward { get; init; } = string.Empty;
}
