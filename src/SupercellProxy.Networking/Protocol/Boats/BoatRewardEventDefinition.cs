using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.Boats;

/// <summary>Reads only the event configuration involved in boat crate rewards.</summary>
public sealed record BoatRewardEventDefinition
{
    /// <summary>Gets the replacement boat-crate reward resource.</summary>
    public string? BoatCrateExpOverride { get; init; }
    /// <summary>Gets the coin reward percentage.</summary>
    [JsonPropertyName("bonusCoins")]
    public int CoinPercentage { get; init; } = 100;
    /// <summary>Gets the experience reward percentage.</summary>
    [JsonPropertyName("bonusXP")]
    public int ExperiencePercentage { get; init; } = 100;
    /// <summary>Gets the seasonal production building whose XP is replaced.</summary>
    [JsonPropertyName("SeasonalObject")]
    public string? SeasonalBuilding { get; init; }
}
