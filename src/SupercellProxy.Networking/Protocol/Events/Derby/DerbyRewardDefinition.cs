using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Events.Derby;

/// <summary>Replaces one league's placement or threshold reward selection.</summary>
public sealed record DerbyRewardDefinition
{
    /// <summary>Gets or sets the native placement or threshold column name.</summary>
    /// <value>The <c>rewardCategoryType</c> string from the event JSON naming which reward column of the league is replaced, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("rewardCategoryType")]
    public string? Category { get; init; }

    /// <summary>Gets or sets the optional weighted choices replacing a reward set.</summary>
    /// <value>The <c>rewardItems</c> array of weighted item choices; <see langword="null"/> when the event names a <see cref="RewardSetName"/> instead, and an individual element is <see langword="null"/> wherever the JSON array holds an empty entry.</value>
    [JsonPropertyName("rewardItems")]
    public DerbyRewardItemDefinition?[]? Items { get; init; }

    /// <summary>Gets or sets the native league asset name.</summary>
    /// <value>The <c>leagueType</c> row name of the league whose rewards this entry replaces, or <see langword="null"/> when the event omits it.</value>
    [JsonPropertyName("leagueType")]
    public string? LeagueName { get; init; }

    /// <summary>Gets or sets the optional reward set to use instead of individual choices.</summary>
    /// <value>The <c>rewardSet</c> asset row name of a whole reward set, or <see langword="null"/> when the choices come from <see cref="Items"/>.</value>
    [JsonPropertyName("rewardSet")]
    public string? RewardSetName { get; init; }
}
