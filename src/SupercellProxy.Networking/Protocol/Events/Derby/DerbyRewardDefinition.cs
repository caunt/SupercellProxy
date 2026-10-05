using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Replaces one league's placement or threshold reward selection.</summary>
public sealed record DerbyRewardDefinition
{
    /// <summary>Gets the native placement or threshold column name.</summary>
    [JsonPropertyName("rewardCategoryType")]
    public string? Category { get; init; }

    /// <summary>Gets the optional weighted choices replacing a reward set.</summary>
    [JsonPropertyName("rewardItems")]
    public DerbyRewardItemDefinition?[]? Items { get; init; }

    /// <summary>Gets the native league asset name.</summary>
    [JsonPropertyName("leagueType")]
    public string? LeagueName { get; init; }

    /// <summary>Gets the optional reward set to use instead of individual choices.</summary>
    [JsonPropertyName("rewardSet")]
    public string? RewardSetName { get; init; }
}
