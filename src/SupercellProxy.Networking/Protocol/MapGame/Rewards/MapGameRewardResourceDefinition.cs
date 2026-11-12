using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Names a resource and amount in a Valley season reward.</summary>
public sealed record MapGameRewardResourceDefinition
{
    /// <summary>Gets or sets the resource amount.</summary>
    /// <value>How many of the resource the reward grants, from <c>rewardAmount</c>.</value>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }

    /// <summary>Gets or sets the asset resource name.</summary>
    /// <value>The data row name of the granted resource, from <c>reward</c>. Empty by default.</value>
    [JsonPropertyName("reward")]
    public string Name { get; init; } = string.Empty;
}
