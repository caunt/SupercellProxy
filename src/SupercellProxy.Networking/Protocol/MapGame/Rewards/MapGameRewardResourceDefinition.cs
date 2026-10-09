using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.MapGame.Rewards;

/// <summary>Names a resource and amount in a Valley season reward.</summary>
public sealed record MapGameRewardResourceDefinition
{
    /// <summary>Gets the resource amount.</summary>
    [JsonPropertyName("rewardAmount")]
    public int Amount { get; init; }

    /// <summary>Gets the asset resource name.</summary>
    [JsonPropertyName("reward")]
    public string Name { get; init; } = string.Empty;
}
