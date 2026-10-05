using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Configuration used by town passenger requests and service rewards.</summary>
public sealed record TownEventDefinition
{
    /// <summary>Gets the upper request-value range percentage.</summary>
    [JsonPropertyName("maxOrderDifficultyPercent")]
    public int MaximumDifficultyPercentage { get; init; } = 100;

    /// <summary>Gets the three-service selection threshold.</summary>
    [JsonPropertyName("threeServicePercent")]
    public int ThreeServicePercentage { get; init; } = 50;

    /// <summary>Gets the running service duration percentage.</summary>
    [JsonPropertyName("serviceTimePercent")]
    public int ServiceTimePercentage { get; init; } = 100;

    /// <summary>Gets the lower request-value range percentage.</summary>
    [JsonPropertyName("minOrderDifficultyPercent")]
    public int MinimumDifficultyPercentage { get; init; }

    /// <summary>Gets the completion reward set for one service.</summary>
    [JsonPropertyName("1TaskRewardSet")]
    public string? OneServiceRewardSet { get; init; }

    /// <summary>Gets the service coin reward percentage.</summary>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;

    /// <summary>Gets the service experience reward percentage.</summary>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;

    /// <summary>Gets the service reputation reward percentage.</summary>
    [JsonPropertyName("bonusReputation")]
    public int ReputationPercentage { get; init; } = 100;

    /// <summary>Gets the completion reward set for three services.</summary>
    [JsonPropertyName("3TaskRewardSet")]
    public string? ThreeServiceRewardSet { get; init; }

    /// <summary>Gets the completion reward set for two services.</summary>
    [JsonPropertyName("2TaskRewardSet")]
    public string? TwoServiceRewardSet { get; init; }
}
