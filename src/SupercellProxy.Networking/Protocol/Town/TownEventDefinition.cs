using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Town;

/// <summary>Configuration used by town passenger requests and service rewards.</summary>
public sealed record TownEventDefinition
{
    /// <summary>Gets or sets the upper request-value range percentage.</summary>
    /// <value>Percent of the reputation level's passenger-service value range kept below its maximum, so 80 drops the top 20% of possible request values; 100 by default, which leaves the maximum unchanged.</value>
    [JsonPropertyName("maxOrderDifficultyPercent")]
    public int MaximumDifficultyPercentage { get; init; } = 100;

    /// <summary>Gets or sets the three-service selection threshold.</summary>
    /// <value>Chance in percent that a passenger whose level allows two or three services asks for three, read from <c>threeServicePercent</c>; 50 by default.</value>
    [JsonPropertyName("threeServicePercent")]
    public int ThreeServicePercentage { get; init; } = 50;

    /// <summary>Gets or sets the running service duration percentage.</summary>
    /// <value>Percent of the building's base service time a newly started service takes; 100 (the default) leaves it unchanged.</value>
    [JsonPropertyName("serviceTimePercent")]
    public int ServiceTimePercentage { get; init; } = 100;

    /// <summary>Gets or sets the lower request-value range percentage.</summary>
    /// <value>Percent of the reputation level's passenger-service value range added to its minimum, so 20 drops the cheapest 20% of possible request values; 0 by default.</value>
    [JsonPropertyName("minOrderDifficultyPercent")]
    public int MinimumDifficultyPercentage { get; init; }

    /// <summary>Gets or sets the completion reward set for one service.</summary>
    /// <value>Name of the passenger bonus set row used for passengers with one service request, or <see langword="null"/> to use the table's first set.</value>
    [JsonPropertyName("1TaskRewardSet")]
    public string? OneServiceRewardSet { get; init; }

    /// <summary>Gets or sets the service coin reward percentage.</summary>
    /// <value>Percent applied to the coins a collected service pays; 100 is the default.</value>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;

    /// <summary>Gets or sets the service experience reward percentage.</summary>
    /// <value>Percent applied to the experience a collected service grants; 100 is the default.</value>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;

    /// <summary>Gets or sets the service reputation reward percentage.</summary>
    /// <value>Percent applied to the building's reputation per service when a service is collected; 100 is the default.</value>
    [JsonPropertyName("bonusReputation")]
    public int ReputationPercentage { get; init; } = 100;

    /// <summary>Gets or sets the completion reward set for three services.</summary>
    /// <value>Name of the passenger bonus set row used for passengers with three service requests, or <see langword="null"/> to use the table's first set.</value>
    [JsonPropertyName("3TaskRewardSet")]
    public string? ThreeServiceRewardSet { get; init; }

    /// <summary>Gets or sets the completion reward set for two services.</summary>
    /// <value>Name of the passenger bonus set row used for passengers with two service requests, or <see langword="null"/> to use the table's first set.</value>
    [JsonPropertyName("2TaskRewardSet")]
    public string? TwoServiceRewardSet { get; init; }
}
