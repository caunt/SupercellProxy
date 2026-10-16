using System.Text.Json.Serialization;

namespace SupercellProxy.Networking.Protocol.GameObjects;

public sealed partial record GameObjectSnapshot
{
    /// <summary>Gets the passenger spawner's Farm Pass service-help requests on that day.</summary>
    [JsonPropertyName("LastFarmPassHelpRequestCount")]
    public int? LastFarmPassHelpRequestCount { get; init; }

    /// <summary>Gets the passenger spawner's day index of its last service-help request.</summary>
    [JsonPropertyName("LastHelpRequestDayIndex")]
    public int? LastHelpRequestDayIndex { get; init; }

    /// <summary>Gets the passenger spawner's neighbourhood service-help requests on that day.</summary>
    [JsonPropertyName("LastNeighborhoodHelpRequestCount")]
    public int? LastNeighborhoodHelpRequestCount { get; init; }

    /// <summary>Gets the passenger spawner's town-visitor service-help requests on that day.</summary>
    [JsonPropertyName("LastVisitorHelpRequestCount")]
    public int? LastVisitorHelpRequestCount { get; init; }

    /// <summary>Gets the personal train's zero-based passenger-capacity upgrade.</summary>
    [JsonPropertyName("Capacity")]
    public int? PersonalTrainCapacityLevel { get; init; }

    /// <summary>Gets the personal train's zero-based cooldown upgrade.</summary>
    [JsonPropertyName("Cooldown")]
    public int? PersonalTrainCooldownLevel { get; init; }
}
